using Newtonsoft.Json;
using System;

using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CreateLHO : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();  

    public DataSet objDataSet;
    public string perameter;

   
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
            ((Label)Master.FindControl("lblHeading")).Text = "Branch Management ";
        }
    }

    protected void btn_Create_Circle_Click(object sender, EventArgs e)
    {
        try
        {
            if ((!string.IsNullOrEmpty(CL_txt_CircleCode.Text)) && (!string.IsNullOrEmpty(CL_txt_CircleName.Text)))
            {   
                if ((CL_txt_CircleName.Text.ToCharArray().All(c => Char.IsLetter(c))) == false)
                {
                    PageUtility.MessageBox(this, "Input data is invalid accepts only alphabetical characters ! Try again");
                    CL_txt_CircleName.Text.Remove(CL_txt_CircleName.Text.Length - 1);
                    return;
                }
               
                if ((CL_txt_CircleCode.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
                {
                    PageUtility.MessageBox(this, "Input data is invalid accepts only numeric value ! Try again");
                    CL_txt_CircleCode.Text.Remove(CL_txt_CircleCode.Text.Length - 1);
                    return;
                }

                BranchDetails branchDetails = new BranchDetails();

                branchDetails.CircleCode = CL_txt_CircleCode.Text;
                branchDetails.CircleName = CL_txt_CircleName.Text;

                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(branchDetails);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/CreateLHO", "POST", dataEncrypted);

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //result = (string)objDCS.ReadObject(objMS);

                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                result = json.Deserialize<string>(reader);

                if (result.Contains("true"))
                {
                    PageUtility.MessageBox(this, "Circle Created Successfully");
                }
                else
                {
                    PageUtility.MessageBox(this, "Circle Code Already Exist!");
                }

            }
            else
            {
                PageUtility.MessageBox(this, "Filed is empty ! Try again");
            }
        }
        catch (Exception ex)
        {

            PageUtility.MessageBox(this, "Catch Error : " + ex.Message.ToString());
        }
        finally
        {
            CL_txt_CircleCode.Text = CL_txt_CircleName.Text = "";
            CL_txt_CircleCode.Focus();           
        }
    }


    public static class PageUtility
    {
        public static void MessageBox(Page page, string strMsg)
        {
            //+ character added after strMsg "')"
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);

        }
    }
}