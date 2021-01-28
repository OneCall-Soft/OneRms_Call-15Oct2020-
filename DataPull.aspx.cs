using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DataPull : System.Web.UI.Page
{

    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string KioskHealthjson;
    public DataSet objDataSet;
    public string strconnected, strdisconnected;
    public int Total_Kiosk;
    public string State_List_with_IP;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Role"] != null && Session["Role"].ToString().ToLower() != "admin")
        {
            Response.Redirect("Logout.aspx");
        }

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblHeading")).Text = "Reports";

            startDT.Text = DateTime.Today.ToString("yyyy-MM-dd");
            endDT.Text = DateTime.Today.ToString("yyyy-MM-dd");
            bindstatelist();
        }
    }
    private void bindPulledData(string state)
    {
        try
        {
            if (objDataSet == null)
                objDataSet = new DataSet();

            Reply objRes = new Reply();
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(state);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetPulledData", "POST", dataEncrypted);

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

                if (objRes.res == true && objRes.DS.Tables[0].Rows.Count > 0)
                {
                    GV_Pulled_data.DataSource = objRes.DS.Tables[0];
                    GV_Pulled_data.DataBind();
                }
                else
                {
                    GV_Pulled_data.DataSource = null;
                    GV_Pulled_data.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('catch error : " + ex.Message + "');", true);
        }


    }
    private void bindstatelist()
    {
        try
        {
            if (objDataSet == null)
                objDataSet = new DataSet();

            Reply objRes = new Reply();
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject("");
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetAllStates", "POST", dataEncrypted);

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

                if (objRes.res)
                {
                    filtercategories1.DataSource = objRes.DS;
                    filtercategories1.DataTextField = "state";
                    filtercategories1.DataBind();
                    filtercategories1.Items.Insert(0, new ListItem("Select Circle", "NA"));
                    errorlabel.Style["display"] = "block";
                    kiosk_data.Text = "";
                    kiosklist.Style["display"] = "none";
                }
                else
                {
                    var page = HttpContext.Current.CurrentHandler as Page;
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Some Error Occurred');windows.location.href='Dashboard-V3_1.aspx'", true);
                }
            }
        }
        catch (Exception ex)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('catch error : " + ex.Message + "');", true);
        }

    }
    public void bindKioskHealth(string state)
    {
        try
        {

            if (objDataSet == null)
                objDataSet = new DataSet();

            Reply objRes = new Reply();

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(state);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/CommandsMachineList", "POST", dataEncrypted);

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
                    if (objRes.DS.Tables[1].Rows.Count > 0)
                    {
                        GV_Pulled_data.DataSource = objRes.DS.Tables[1];
                        GV_Pulled_data.DataBind();
                    }
                    else
                    {
                        GV_Pulled_data.DataSource = null;
                        GV_Pulled_data.DataBind();
                    }
                    for (int iStateIndex = 0; iStateIndex < objRes.DS.Tables[0].Rows.Count; iStateIndex++)
                    {

                        State_List_with_IP += "<tr>" +
                                              "<td class=''>" +
                                              " <label class='option block mn'>" +
                                              " <input type = 'checkbox' name= 'inputname' value= 'FR'> " +
                                              " <span class='checkbox mn'></span> " +
                                              "  </label> " +
                                              " </td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][0].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][1].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][2].ToString() + "</td> " +
                                              " <td > " +
                                              " <span>" + objRes.DS.Tables[0].Rows[iStateIndex][3].ToString() + "</span> " +
                                              " </td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][4].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][5].ToString() + "</td> " +
                                              " </tr>";
                    }
                    errorlabel.Style["display"] = "none";
                    kiosklist.Style["display"] = "block";
                    srch.Value = "";
                    kiosk_data.Text = State_List_with_IP;
                }
                else
                {
                    var page = HttpContext.Current.CurrentHandler as Page;
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('" + objRes.strError + "');", true);
     
                }
            }
        }
        catch (Exception ex)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('catch error : " + ex.Message + "');", true);
        }
    }
    protected void downloadCommand_Click(object sender, EventArgs e)
    {
        try
        {
            if ((MachineLogs.Checked || EJLogs.Checked) && (startDT.Text == "" || endDT.Text == ""))
            {
                srch.Value = "";
                startDT.Text = ""; endDT.Text = "";         
                bindKioskHealth(filtercategories1.SelectedItem.Text);
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Fill Start Date and End Date');", true);
                return;
            }
            else if (OtherData.Checked && (orderid.Text == "" || filtercategories.SelectedIndex == 0))
            {
                srch.Value = "";
                startDT.Text = ""; endDT.Text = "";
                bindKioskHealth(filtercategories1.SelectedItem.Text);
                DownloadData.Style["display"] = "block";
                SelectDate.Style["display"] = "none";
                OtherData.Checked = true;
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Select Type and Give Valid Path');", true);
                return;
            }
            else
            {
                ReqCommandExecute reqCommandExecute = new ReqCommandExecute();
                string CommandString = "";
                if (MachineLogs.Checked || EJLogs.Checked)
                {
                    if (MachineLogs.Checked)
                        CommandString = "MachineLogs";
                    else
                        CommandString = "EjLogs";

                    DateTime startDate, endDate;

                    if (!DateTime.TryParseExact(startDT.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) || !DateTime.TryParseExact(endDT.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    {
                        srch.Value = "";
                        startDT.Text = ""; endDT.Text = "";
                        bindKioskHealth(filtercategories1.SelectedItem.Text);
                        var page = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Date Format Not Proper');", true);
                        return;
                    }
                    else if (startDate > endDate || endDate > DateTime.Today)
                    {
                        srch.Value = "";
                        bindKioskHealth(filtercategories1.SelectedItem.Text);
                        startDT.Text = ""; endDT.Text = "";
                        var page = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Start Date can not be greater than End Date and End Date should be less than equal to today');", true);
                        return;
                    }

                    if((endDate.Date - startDate.Date).TotalDays > 7)
                    {
                        srch.Value = "";
                        bindKioskHealth(filtercategories1.SelectedItem.Text);                       
                        var page = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Date difference should be less than or equal to 7 days.');", true);
                        return;
                    }

                    CommandString += "#" + startDate.ToString("yyyy-MM-dd") + "#" + endDate.ToString("yyyy-MM-dd");
                    reqCommandExecute.DisplayData = "Log Pull";
                }
                else
                {
                    CommandString = "OtherData#" + filtercategories.SelectedItem.Text + "#" + orderid.Text;
                    DownloadData.Style["display"] = "block";
                    SelectDate.Style["display"] = "none";
                    OtherData.Checked = true;
                    reqCommandExecute.DisplayData = filtercategories.SelectedItem.Text + " Pull";
                }

                string[] ipArray = hiddenItem.Value.Split('#');
                Array.Resize(ref ipArray, ipArray.Length - 1);
                reqCommandExecute.KioskIPs = ipArray;
                reqCommandExecute.Command = CommandString;
                reqCommandExecute.Patchdata = "";
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    string JsonString = JsonConvert.SerializeObject(reqCommandExecute);
                   EncRequest objEncRequest = new EncRequest();
                   objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);          
                   string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                   string result1 = client.UploadString(URL + "/CommandExecute", "POST", dataEncrypted);

                    EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result1);
                    objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                    //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                    //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                    //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                    //ResCommandExecute objRes = (ResCommandExecute)objDCS.ReadObject(objMS);


                    Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                    json.NullValueHandling = NullValueHandling.Ignore;
                    StringReader sr = new StringReader(objResponse.ResponseData);
                    Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                    ResCommandExecute objRes = json.Deserialize<ResCommandExecute>(reader);

                    if (objRes.Result)
                    {
                        srch.Value = "";
                        startDT.Text = ""; endDT.Text = "";
                        var page = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Request Send SuccessFully');", true);
                        bindKioskHealth(filtercategories1.SelectedItem.Text);
                    }
                    else
                    {
                        srch.Value = "";
                        startDT.Text = ""; endDT.Text = "";
                        var page = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Request Sending Failed');", true);
                        bindKioskHealth(filtercategories1.SelectedItem.Text);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            srch.Value = "";
            startDT.Text = ""; endDT.Text = "";
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Exception :" + ex.Message + "');", true);
            bindKioskHealth(filtercategories1.SelectedItem.Text);
        }
    }
    protected void filtercategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (MachineLogs.Checked || EJLogs.Checked)
        {
            SelectDate.Style["display"] = "block";
            DownloadData.Style["display"] = "none";
        }
        else
        {
            DownloadData.Style["display"] = "block";
            SelectDate.Style["display"] = "none";
        }
        if (filtercategories1.SelectedIndex == 0)
        {
            kiosk_data.Text = "";
            kiosklist.Style["display"] = "none";
            errorlabel.Style["display"] = "block";
        }
        else
        {
            bindKioskHealth(filtercategories1.SelectedItem.Text);
           
        }
    }

    protected void GV_Pulled_data_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV_Pulled_data.PageIndex = e.NewPageIndex;
    }    

    protected void btn_refresh_Click(object sender, EventArgs e)
    {
        bindKioskHealth(filtercategories1.SelectedItem.Text);
    }

    protected void GV_Pulled_data_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "downlaod")
        {
            string fileName ="";
            string path = e.CommandArgument.ToString();
            if (File.Exists(path))
            {
                fileName = Path.GetFileName(path);
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + fileName);
                Response.TransmitFile(path);
                Response.End();
            }
        }
    }
}