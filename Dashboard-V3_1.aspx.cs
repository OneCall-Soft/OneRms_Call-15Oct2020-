using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.Security;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web.SessionState;

public partial class Dashboard : System.Web.UI.Page
{
    public string path = "";
    public const string AntiXsrfTokenKey = "__AntiXsrfToken";
    public const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    public string _antiXsrfTokenValue;
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string KioskHealthjson;
    public DataSet objds;
    public DataSet LastTransaction;
    public string strconnected, strdisconnected;
    public int Total_Kiosk;
    public string State_List_with_IP;
    public int connected;
    public int Disconnected;
    public int tot_Succ_Txn;
    public int tot_fail_Txn;
    public String KioskHelath_With_State;
    public String TxnList_with_state;
    public string PopupData;
    public string Location;
    public string Role;
    public string perameter;
    public string[] xDates;


    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches in the view state form field in the Page.PreLoad
            //method.
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                _antiXsrfTokenValue = requestCookie.Value;

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,

                    //Add the Anti-XSRF token to the cookie value
                    Value = _antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                    responseCookie.Secure = true;

                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
        }
    }
    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        try
        {

            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                // LNKLogOut.Visible = true;
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");

                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] == null && Session.IsNewSession == false)
        {
            Response.Redirect("Logout.aspx", false);
            return;
        }

        if (globle.UserValue != null && Session.IsNewSession == true)
        {
            Session["Username"] = globle.UserValue;
            Session["Role"] = globle.Role;
            Session["Location"] = "";
            Session["PF_Index"] = globle.PF_Index;
            Session["LoggedIn"] = "Yes";
        }
        else if (globle.UserValue == null)
        {
            Response.Redirect("Logout.aspx", false);
            return;
        }

        if (!IsPostBack)
        {   
            if (Session["Role"].ToString() == "admin")
            {
                AdminOptions.Visible = true;
                lblUserLocation.Text = "Admin";
            }
            else
            {
                lblUserLocation.Text = Session["Location"].ToString() + "-" + Session["Role"].ToString();
                AdminOptions.Visible = false;
            }

            if (globle.CallLogRequired == "false")
                TicketReportID.Style.Add("Display", "None");

            GetKioskHealth();
            GetLastTxn(11);

            if (Session.IsNewSession == false && Session["LoggedIn"] == null)
            {
                bool redirected = false;
                bool isAdded = false;
                SessionIDManager Manager = new SessionIDManager();
                string NewID = Manager.CreateSessionID(Context);
                string OldID = Context.Session.SessionID;
                Manager.SaveSessionID(Context, NewID, out redirected, out isAdded);
                Request.Cookies.Add(new HttpCookie("ASP.NET_SessionId", NewID));
            }
        }
    }


    public void GetLastTxn(int day)
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().Contains("admin"))
                {
                    perameter = "all##" + day;
                }
                else
                {
                    perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + day;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(perameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetLastTransactionDetail", "Post", dataEncrypted);  //   GetDashBoardDetail |GetLastTransactionDetail

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

                LastTransaction = new DataSet();
                LastTransaction.Tables.Add();
                LastTransaction.Tables[0].Columns.Add("Sno");
                LastTransaction.Tables[0].Columns.Add("Date");
                LastTransaction.Tables[0].Columns.Add("LastTransaction");
                DataRow drow;

                int iDSIterator = 1;
                DateTime DataTransaction;

                if (objRes.res == true && objRes.DS.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        drow = LastTransaction.Tables[0].NewRow();
                        drow["Sno"] = i + 1 + Environment.NewLine;
                        DataTransaction = DateTime.Now.AddDays(-iDSIterator);

                        drow["Date"] = DataTransaction.ToString("dd-MM-yyyy");
                        try
                        {
                            bool IsFound = false;
                            int iCount = 0;
                            while (iCount < objRes.DS.Tables[0].Rows.Count)//handled index out of bound exception//
                            {
                                if (DataTransaction.ToString("dd-MM-yyyy").Contains(objRes.DS.Tables[0].Rows[iCount]["txn_dt"].ToString()))
                                {
                                    IsFound = true;
                                    drow["Date"] = DataTransaction.ToString("dd-MM-yyyy");
                                    try
                                    {
                                        drow["LastTransaction"] = Convert.ToInt32(objRes.DS.Tables[0].Rows[iCount]["total_transaction"]);
                                    }
                                    catch (Exception ex)
                                    {
                                        drow["LastTransaction"] = "0";
                                    }
                                    break;
                                }
                                iCount++;
                            }

                            if (IsFound == false)
                            {
                                drow["Date"] = DataTransaction.ToString("dd-MM-yyyy");
                                drow["LastTransaction"] = "0";
                            }
                        }
                        catch (Exception ex)
                        {
                            lbl_txn_error.Text = "" + ex;
                            drow["Date"] = DataTransaction.ToString("dd-MM-yyyy");
                            drow["LastTransaction"] = "0";
                        }

                        iDSIterator++;
                        LastTransaction.Tables[0].Rows.Add(drow);
                        if (LastTransaction.Tables[0].Rows.Count >= 10)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < day; i++)
                    {
                        drow = LastTransaction.Tables[0].NewRow();
                        drow["Sno"] = i + 1;
                        if (i == 0)
                            DataTransaction = DateTime.Today;
                        else
                            DataTransaction = DateTime.Today.AddDays(-i);

                        drow["Date"] = DataTransaction.ToString("dd-MM-yyyy");
                        drow["LastTransaction"] = "0";

                        LastTransaction.Tables[0].Rows.Add(drow);
                    }
                }
            }
        }   
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert('catch error : " + ex.Message + "  ')</script>");
        }
        finally
        {
            dataListTransaction.DataSource = LastTransaction;
            dataListTransaction.DataBind();
        }
    }

    public class SchemaInfo1
    {
        public string Printed_Characters_Full { get; set; }
        public string Printed_Characters_Partial { get; set; }
        public string Line_Feeds { get; set; }
        public string Documents_Insertions { get; set; }
        public string Cover_Openings { get; set; }
        public string Paper_Jams { get; set; }

        public string Front_Scannings { get; set; }
        public string Back_Scannings { get; set; }
        public string Power_on_hours { get; set; }

        public string Standby_hours { get; set; }
        public string Power_on_cycles { get; set; }
    }

    public class SchemaInfo
    {
        public bool Printed_Characters_Full { get; set; }
        public bool Printed_Characters_Partial { get; set; }
        public bool Line_Feeds { get; set; }
        public bool Documents_Insertions { get; set; }
        public bool Cover_Openings { get; set; }
        public bool Paper_Jams { get; set; }

        public bool Front_Scannings { get; set; }
        public bool Back_Scannings { get; set; }
        public bool Power_on_hours { get; set; }

        public bool Standby_hours { get; set; }
        public bool Power_on_cycles { get; set; }
    }

    public class setting
    {
        public SchemaInfo Admin { get; set; }

        public SchemaInfo CircleUser { get; set; }

        public SchemaInfo BranchUser { get; set; }
    }
    protected void TotMc_Command(object sender, CommandEventArgs e)
    {
        string Lho_Name;
        string CommandName;

        if (objds == null)
        {
            objds = new DataSet();
        }
        try
        {
            if (e.CommandName == "TotMc")
            {
                CommandName = e.CommandName;
                Lho_Name = e.CommandArgument.ToString();

                Response.Redirect("LHO_Details.aspx?LN=" + Lho_Name + "&CN=" + CommandName);
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }

    }
    protected void ConMc_Command(object sender, CommandEventArgs e)
    {
        string Lho_Name;
        string CommandName;

        if (objds == null)
        {
            objds = new DataSet();
        }

        try
        {
            if (e.CommandName == "ConMc")
            {
                CommandName = "True";
                Lho_Name = e.CommandArgument.ToString();
                Response.Redirect("LHO_Details.aspx?LN=" + Lho_Name + "&CN=" + CommandName);

            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }

    }
    protected void DisMc_Command(object sender, CommandEventArgs e)
    {
        string Lho_Name;
        string CommandName;

        if (objds == null)
        {
            objds = new DataSet();
        }


        try
        {
            if (e.CommandName == "DisMc")
            {
                CommandName = "False";
                Lho_Name = e.CommandArgument.ToString();
                Response.Redirect("LHO_Details.aspx?LN=" + Lho_Name + "&CN=" + CommandName);
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }

    }

    private bool SendCOmmand(string LHOName, string Command, out DataSet ds)
    {

        Reply objRes = new Reply();
        ds = null;
        using (WebClient client = new WebClient())
        {
            if (Session["Role"].ToString().Contains("admin"))
            {
                perameter = "all#" + LHOName + "#" + Command;   // role # lhoname # commandname (TotMc)
                                                                //-    @Role = N'all',
                                                                //-	@location = N'Ahmedabad',
                                                                //-	@Command = N'totMc'
            }
            else
            {
                perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();
            }

            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(perameter);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string result = client.UploadString(URL + "/Get_LHO_Details", "POST", dataEncrypted);  // , "\"" + perameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

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
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        GetKioskHealth();

        GetLastTxn(11);

    }
    public void GetKioskHealth()
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().Contains("admin"))
                {
                    perameter = "all#";
                }
                else
                {
                    perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();
                }
               
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                string JsonString = JsonConvert.SerializeObject(perameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);
                               
                string result = client.UploadString(URL + "/Get_LHO_Summery", "POST", dataEncrypted);  // , "\"" + perameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);

                //StringReader sr = new StringReader(objResponse.ResponseData);
                //Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);

                //objRes = json.Deserialize<Reply>(sr);
             

            }
            if (objRes.res == true)
            {
               
                DL_LHO_List.DataSource = objRes.DS;
                DL_LHO_List.DataBind();


                for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                {
                    connected += Convert.ToInt32(objRes.DS.Tables[0].Rows[i][2].ToString());
                    Disconnected += Convert.ToInt32(objRes.DS.Tables[0].Rows[i][3].ToString());
                    tot_Succ_Txn += Convert.ToInt32(objRes.DS.Tables[0].Rows[i][5].ToString());
                    tot_fail_Txn += Convert.ToInt32(objRes.DS.Tables[0].Rows[i][6].ToString());
                }

                if (tot_Succ_Txn == 0 && tot_fail_Txn == 0)
                {
                    lbl_txn_error.Text = " No Transaction Occurred Today"; 
                }

            }
            else
            {
                Response.Write("<script type='text/javascript'>alert( 'Response Error : " + objRes.strError + "' )</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }
        finally { GC.Collect(); }
    }
    
    protected void ddl_transactionDays_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLastTxn(Convert.ToInt32(ddl_transactionDays.SelectedValue)+1);
        GetKioskHealth();
    }
}

