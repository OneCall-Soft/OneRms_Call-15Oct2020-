using System;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

using System.Web.Services;

using System.Drawing;
using System.Diagnostics;
using Newtonsoft.Json;
using EncryptionDecryption;

public partial class LHO_Details : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string parameter;
    public string LHO_Name;
    public string Command;
    public DataSet objds;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {          
            ((Label)Master.FindControl("lblHeading")).Text = "Reports   ";
            Session["LhoName"] = LHO_Name = Request.QueryString["LN"];
            Session["Command"] = Command = Request.QueryString["CN"];
            DisplayData(LHO_Name, Command);
        }
    }

    protected void DisplayData(string A, string B)
    {
        try
        {
            if (objds == null)
            {
                objds = new DataSet();
            }

            if (SendCOmmand(A, B, out objds))
            {
                lbl_tot.Text = objds.Tables[0].Rows.Count.ToString();

                lbl_lho_name.Text = objds.Tables[0].Rows[0]["LHO"].ToString();

                // b.Call_Log_Status,
                GV_LHO_Details.DataSource = objds.Tables[0];

                if (Session["Role"].ToString() == "admin")
                {
                    GV_LHO_Details.Columns[11].Visible = true;  //Restart
                    GV_LHO_Details.Columns[12].Visible = true;  //Shutdown
                    GV_LHO_Details.Columns[13].Visible = false;  //Remote Machine is not working

                    if (globle.CallLogRequired == "false")
                        GV_LHO_Details.Columns[10].Visible = false;  //Call Log Manually
                }
                else
                {
                    GV_LHO_Details.Columns[11].Visible = false; //Restart
                    GV_LHO_Details.Columns[12].Visible = false; //Shutdown
                    GV_LHO_Details.Columns[13].Visible = false;  //Remote Machine is not working

                    if (globle.CallLogRequired == "false")
                        GV_LHO_Details.Columns[10].Visible = false; //Call Log Manually
                }

                GV_LHO_Details.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            GC.Collect();
        }
    }


    private bool SendCOmmand(string LHOName, string Command, out DataSet ds)
    {
        Reply objRes = new Reply();
        ds = null;
        using (WebClient client = new WebClient())
        {
            if (Session["Role"].ToString() == "branch")
            {
                parameter = "branch#" + Session["Location"].ToString() + "#" + Command;
            }
            else
            {
                parameter = "all#" + LHOName + "#" + Command;
            }

            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string JsonString = JsonConvert.SerializeObject(parameter);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string result = client.UploadString(URL + "/Get_LHO_Details", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

            EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
            objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
            //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
            //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
            //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
            //objRes = (Reply)objDCS.ReadObject(objMS);

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            StringReader sr = new StringReader(objResponse.ResponseData);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            objRes = json.Deserialize<Reply>(reader);

            if (objRes.res == true)
            {
                ds = objRes.DS;
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    protected void GV_LHO_Details_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV_LHO_Details.PageIndex = e.NewPageIndex;

        DisplayData(Session["LhoName"].ToString(), Session["Command"].ToString());
    }

    protected void GV_LHO_Details_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToLower().Trim())
        {
            case "shutdown_command":
                {
                    ReqCommandExecute objReq = new ReqCommandExecute();
                    ResCommandExecute objRes = new ResCommandExecute();

                    int row;
                    row = Convert.ToInt32(e.CommandArgument.ToString());
                    string kiosk_ip = GV_LHO_Details.Rows[row].Cells[1].Text;

                    using (WebClient client = new WebClient())
                    {
                        objReq.KioskIPs = new string[(1)];
                        objReq.Command = "system#shutdown";
                        objReq.KioskIPs[0] = kiosk_ip;
                        objReq.Patchdata = "";
                        objReq.DisplayData = "Shutdown PC";

                    
                        client.Headers[HttpRequestHeader.ContentType] = "text/json";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        string JsonString = JsonConvert.SerializeObject(objReq);
                        EncRequest objEncRequest = new EncRequest();
                        objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                        string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                        string result = client.UploadString(URL + "/CommandExecute", "POST", dataEncrypted);

                        EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                        objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                        //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                        //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                        //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                        //objRes = (ResCommandExecute)objDCS.ReadObject(objMS);


                        Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                        json.NullValueHandling = NullValueHandling.Ignore;
                        StringReader sr = new StringReader(objResponse.ResponseData);
                        Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                        objRes = json.Deserialize<ResCommandExecute>(reader);

                        if (objRes.Result == true)
                        {
                            Response.Write("<script type='text/javascript'>alert('Command Send Successfully')</script>");
                        }
                        else
                        {
                            Response.Write("<script type='text/javascript'>alert('catch error : " + objRes.Error + "  ')</script>");
                        }

                    }
                    
                }
                break;
            case "restart_command":
                {
                    ReqCommandExecute objReq = new ReqCommandExecute();
                    ResCommandExecute objRes = new ResCommandExecute();
                    int row;
                    row = Convert.ToInt32(e.CommandArgument.ToString());
                    string kiosk_ip = GV_LHO_Details.Rows[row].Cells[1].Text;
                    using (WebClient client = new WebClient())
                    {
                        objReq.KioskIPs = new string[(1)];
                        objReq.Command = "system#restart";
                        objReq.KioskIPs[0] = kiosk_ip;
                        objReq.Patchdata = "";
                        objReq.DisplayData = "Restart PC";
                     

                        client.Headers[HttpRequestHeader.ContentType] = "text/json";
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        string JsonString = JsonConvert.SerializeObject(objReq);
                        EncRequest objEncRequest = new EncRequest();
                        objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                        string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                        string result = client.UploadString(URL + "/CommandExecute", "POST", dataEncrypted);

                        EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                        objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                        //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                        //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                        //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                        //objRes = (ResCommandExecute)objDCS.ReadObject(objMS);


                        Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                        json.NullValueHandling = NullValueHandling.Ignore;
                        StringReader sr = new StringReader(objResponse.ResponseData);
                        Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                        objRes = json.Deserialize<ResCommandExecute>(reader);

                        if (objRes.Result == true)
                        {
                            Response.Write("<script type='text/javascript'>alert('Command Send Successfully')</script>");
                        }
                        else
                        {
                            Response.Write("<script type='text/javascript'>alert('catch error : " + objRes.Error + ")</script>");
                        }
                    }
                   
                }
                break;
       
                  default:
                break;
        }
    }

    protected void GV_LHO_Details_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Checking the RowType of the Row  
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //If Salary is less than 10000 than set the Cell BackColor to Red and ForeColor to White  
            if (e.Row.Cells[3].Text == "True")
            {
                e.Row.Cells[3].Text = "Connected";
                // e.Row.Cells[2].BackColor = Color.;
                e.Row.Cells[3].ForeColor = Color.DarkBlue;
            }
            else
            {
                e.Row.Cells[3].Text = "Disconnected";
                e.Row.Cells[3].BackColor = Color.Red;
                e.Row.Cells[3].ForeColor = Color.White;
            }
        }
    }


    [WebMethod(EnableSession = true)]
    public static string Reload(string a)
    {
        LHO_Details obj = new global::LHO_Details();
        obj.DisplayData(HttpContext.Current.Session["LhoName"].ToString(), HttpContext.Current.Session["Command"].ToString());
        return a;
    }

    [WebMethod(EnableSession = true)]
    // [WebMethod]
    public static string SubmitIssue(string IssueTitle, string strKioskID, string strSerialNumber, string strbrCode, string personName, string personNumber, string Email_To, string IssueDetails)
    {
        string IssueDate = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
        string username = HttpContext.Current.Session["Username"].ToString();
        try
        {
            string StrURL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();

            ReqTicketGenerate objReq = new ReqTicketGenerate();

            ResCallOpen objRes = new ResCallOpen();

            using (WebClient client = new WebClient())
            {
                objReq.IssueTitle = IssueTitle;
                objReq.IssueDateTime = IssueDate;
                objReq.IssueStatus = "p";
                objReq.Sender = username;
                objReq.ContactPerson = AESEncrytDecry.DecryptStringAES(personName);
                objReq.ContactPersonMobile = AESEncrytDecry.DecryptStringAES(personNumber);
                objReq.Email_To = AESEncrytDecry.DecryptStringAES(Email_To); 
                objReq.ProblemDescription = AESEncrytDecry.DecryptStringAES(IssueDetails);
                objReq.KioskID = AESEncrytDecry.DecryptStringAES(strKioskID);
                objReq.SerialNumber = AESEncrytDecry.DecryptStringAES(strSerialNumber); 
                objReq.brCode = AESEncrytDecry.DecryptStringAES(strbrCode);      

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string JsonString = JsonConvert.SerializeObject(objReq);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(StrURL + "/GenerateTicket", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //objRes = (ResTicketGenerate)objDCS.ReadObject(objMS);

                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<ResCallOpen>(reader);

                if (objRes.success == true)
                {
                    return "Call Open Successfully On Ticket No : " + objRes.resTicketNumber;                    
                }
                else
                {
                    if (objRes.error != null)
                        return "Call Open Failed : Error Code : " + objRes.error[0].errorCode + " " + "Error Msg : " + objRes.error[0].errorMessage;
                    else
                        return "Call Open Failed : No Connectivity"; 
                }
                
            }
        }
        catch (Exception ex)
        {
            return "Call Open Failed";
        }
        
    }

    [WebMethod(EnableSession = true)]
    public static string CallClosed(string IssueTitle, string strKioskID, string strSerialNumber, string strbrCode, string personName, string personNumber,  string IssueDetails)
    {
        try
        {

            string StrURL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();

            ReqTicketGenerate objReq = new ReqTicketGenerate();

            ResCallClose objRes = new ResCallClose();

            using (WebClient client = new WebClient())
            {
                objReq.KioskID = AESEncrytDecry.DecryptStringAES(strKioskID);
                objReq.brCode = AESEncrytDecry.DecryptStringAES(strbrCode);
                objReq.ContactPersonMobile = AESEncrytDecry.DecryptStringAES(personNumber);
                objReq.ContactPerson = AESEncrytDecry.DecryptStringAES(personName);
                objReq.IssueTitle = IssueTitle;
                objReq.ProblemDescription = AESEncrytDecry.DecryptStringAES(IssueDetails);
                objReq.Sender = HttpContext.Current.Session["PF_Index"].ToString();

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(objReq);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(StrURL + "/CallClose", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //objRes = (ResTicketGenerate)objDCS.ReadObject(objMS);

                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<ResCallClose>(reader);

                if (objRes.success == true)
                {
                    return  "Call Close Successfully On Ticket No :- " + objRes.openTicketNumber;
                }
                else
                {
                    if (objRes.error != null)
                        return "Call Close Failed : " + objRes.error[0].errorCode + " " + "Error Msg : " + objRes.error[0].errorMessage;
                    else
                        return "Call Close Failed : No Connectivity";
                }
            }
        }
        catch (Exception ex)
        {
            return "Call Close Failed";
        }
        
    }



    protected void btn_reload_Click(object sender, EventArgs e)
    {

        DisplayData(Session["LhoName"].ToString(), Session["Command"].ToString());
    }

    protected void btn_Search_by_branch_code_Click(object sender, EventArgs e)
    {
   
    }
}
