using Newtonsoft.Json;
using System;

using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class KioskMaster : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public DataSet objDataSet;
    public DataTable publicDT;
    public string perameter;

    public static DataTable MyData { get; set; }

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
            

            if (Session["Role"].ToString().ToLower().Contains("admin"))
            {
                bindKioskMasterlist();
                searchingOption.Visible = true;
            }
            else
            {
                searchingOption.Visible = false;             
            }
        }
    }

    private void bindKioskMasterlist()
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

                string result = client.UploadString(URL + "/GetKioskDetails", "POST", dataEncrypted);

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //objRes = (Reply)objDCS.ReadObject(objMS); ;


                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                objRes = json.Deserialize<Reply>(reader);

                if (objRes.res == true && objRes.DS.Tables[0].Rows.Count > 0)
                {
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    GV_Kiosk_Details.DataSource = objRes.DS.Tables[0];
                    GV_Kiosk_Details.DataBind();                    

                    // Clear all content output from the buffer stream
                    Response.ClearContent();
                    // Specify the default file name using "content-disposition" RESPONSE header
                    Response.AppendHeader("content-disposition", "attachment; filename=PbKioskMasterReport " + DateTime.Now.ToString("ddMMyy_HHmm") + ".xls");
                    // Set excel as the HTTP MIME type
                    Response.ContentType = "application/excel";
                    // Create an instance of stringWriter for writing information to a string
                    StringWriter stringWriter = new StringWriter();
                    // Create an instance of HtmlTextWriter class for writing markup 
                    // characters and text to an ASP.NET server control output stream
                    HtmlTextWriter htw = new HtmlTextWriter(stringWriter);

                    int ColTot = GV_Kiosk_Details.Rows[0].Cells.Count;

                    HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                                                       "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                                                       "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - KIOSK MASTER REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>");

                    GV_Kiosk_Details.HeaderStyle.ForeColor = Color.White;
                    GV_Kiosk_Details.HeaderStyle.BackColor = Color.Blue;
                    GV_Kiosk_Details.HeaderStyle.Font.Bold = true;
                    GV_Kiosk_Details.Font.Name = "Calibri";

                    GV_Kiosk_Details.RenderControl(htw);


                    Response.Write(stringWriter.ToString());

                    HttpContext.Current.Response.Flush();

                    HttpContext.Current.Response.End();

                    Response.End();

                }
                else
                {
                    PageUtility.MessageBox(this, "Data Not Exist.");
                    lbl_tot.Text = "";
                    GV_Kiosk_Details.DataSource = null;
                    GV_Kiosk_Details.DataBind();                   
                }
            }
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message + "");
            
        }
    }
  

    public static class PageUtility
    {
        public static void MessageBox(System.Web.UI.Page page, string strMsg)
        {
            //+ character added after strMsg "')"
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);
        }
    }
    
   
   
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
}