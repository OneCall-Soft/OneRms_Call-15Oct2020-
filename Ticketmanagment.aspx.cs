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
using Newtonsoft.Json;

public partial class Ticketmanagment : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public int iTotResolved;
    public int iTotPending;
    public int iTotTicket;
    public string parameter;
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
            ((Label)Master.FindControl("lblHeading")).Text = "Reports";
            RepeterData();
        }
    }

    public void RepeterData()
    {
        try
        {
            iTotResolved = 0;
            iTotPending = 0;
            iTotTicket = 0;
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all#";
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetIssueList", "Post", dataEncrypted);  //   GetDashBoardDetail |GetLastTransactionDetail

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

                    //1. pending list
                    objRes.DS.Tables[0].DefaultView.RowFilter = "RequestType = 'open'";
                    RepeaterPendingIssue.DataSource = objRes.DS.Tables[0].DefaultView;
                    RepeaterPendingIssue.DataBind();
                    RepeaterTickets.DataSource = objRes.DS.Tables[0].DefaultView;
                    RepeaterTickets.DataBind();
                    iTotPending = objRes.DS.Tables[0].DefaultView.Count;

                    //2. resolved list - Resolved
                    objRes.DS.Tables[0].DefaultView.RowFilter = "RequestType = 'close'";
                    RepeaterResolvedIssue.DataSource = objRes.DS.Tables[0].DefaultView;
                    RepeaterResolvedIssue.DataBind();
                    iTotResolved = objRes.DS.Tables[0].DefaultView.Count;

                    iTotTicket = iTotPending + iTotResolved;
                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert('Data Not Found  ')</script>");
                }
            }

            
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert('catch error : " + ex.Message + "  ')</script>");
        }
    }
}