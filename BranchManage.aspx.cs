using EncryptionDecryption;
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

public partial class TransactionReport : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();

    public DataSet objDataSet;
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
            bindstatelist();
        }
    }

    private void bindstatelist()
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
                MB_ddl_Circle_Name.DataSource = objRes.DS;
                MB_ddl_Circle_Name.DataTextField = "state";
                MB_ddl_Circle_Name.DataValueField = "id";
                MB_ddl_Circle_Name.DataBind();
                MB_ddl_Circle_Name.Items.Insert(0, new ListItem("--Select LHO/Circle Name--", "NA"));
            }
            else
            {
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Some Error Occurred');windows.location.href='Dashboard-V3_1.aspx'", true);
            }
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
    
    protected void MB_btn_Update_Click(object sender, EventArgs e)
    {

        if (MB_txt_branchName.Text == "" || 
          
            MB_txt_ModuleCode.Text == "" || 
            MB_txt_ModuleName.Text == "" ||
            MB_txt_Network.Text == ""    || 
            MB_txt_Region.Text == "")
        {
            PageUtility.MessageBox(this, "Please enter all values ");
            return;
        }

        if ((MB_txt_branchName.Text.ToCharArray().All(c => Char.IsLetter(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch name accepts only alphabetical characters ! Try again");
            MB_txt_branchName.Text.Remove(MB_txt_branchName.Text.Length - 1);
            return;
        }

        if ((MB_txt_ModuleCode.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in module code accepts only numeric values ! Try again");
            MB_txt_ModuleCode.Text.Remove(MB_txt_ModuleCode.Text.Length - 1);
            return;
        }

        if ((MB_txt_ModuleName.Text.ToCharArray().All(c => Char.IsLetter(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in module name accepts only alphabetical characters ! Try again");
            MB_txt_ModuleName.Text.Remove(MB_txt_ModuleName.Text.Length - 1);
            return;
        }

        if ((MB_txt_Network.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch code accepts only numeric values ! Try again");
            MB_txt_Network.Text.Remove(MB_txt_Network.Text.Length - 1);
            return;
        }

        if ((MB_txt_Region.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch code accepts only numeric values ! Try again");
            MB_txt_Region.Text.Remove(MB_txt_Region.Text.Length - 1);
            return;
        }


        try
        {
            BranchDetails branchDetails = new BranchDetails();
            branchDetails.BranchCode = MB_ddl_branch_code.SelectedItem.Text;
            branchDetails.BranchName = MB_txt_branchName.Text;
            branchDetails.PersonName = MB_txt_PersonName.Text;
            branchDetails.PersonPhone = MB_txt_PersonNumber.Text;
            branchDetails.PersonEmail = MB_txt_Email.Text;
            branchDetails.ModuleCode = MB_txt_ModuleCode.Text;
            branchDetails.ModuleName = MB_txt_ModuleName.Text;
            branchDetails.Network = MB_txt_Network.Text;
            branchDetails.Region = MB_txt_Region.Text;
            branchDetails.CircleCode = MB_ddl_Circle_Name.SelectedItem.Value;

            WebClient client = new WebClient();
            
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(branchDetails);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string result = client.UploadString(URL + "/ModifyBranch", "POST", dataEncrypted);

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
                PageUtility.MessageBox(this, "Branch Update Successfully");
            }
            else
            {
                PageUtility.MessageBox(this, "Failed");
            }

        }
        catch (Exception ex)
        {

            PageUtility.MessageBox(this, "Catch Error : " + ex.Message.ToString());
        }
        finally
        {
            MB_ddl_Circle_Name.ClearSelection();
            MB_ddl_branch_code.ClearSelection();
            MB_txt_branchName.Text = MB_txt_Region.Text =
            MB_txt_ModuleCode.Text = MB_txt_ModuleName.Text =
            MB_txt_Network.Text = MB_txt_PersonName.Text =
            MB_txt_Email.Text = MB_txt_PersonNumber.Text = "";           
        }
    }

    protected void MB_ddl_Circle_Name_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(MB_ddl_Circle_Name.SelectedIndex == 0)
        {
            MB_ddl_branch_code.Items.Clear();
        }
        else
        {
            bindBranchlist(MB_ddl_Circle_Name.SelectedValue);
        }
    }


    private void bindBranchlist(string LHO)
    {
        MB_txt_branchName.Text = MB_txt_Region.Text =
        MB_txt_ModuleCode.Text = MB_txt_ModuleName.Text =
        MB_txt_Network.Text = "";

        if (Session["Role"].ToString().ToLower().Contains("admin"))
        {
            parameter = "all##" + LHO;
        }
        else
        {
            parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + LHO;
        }

        if (objDataSet == null)
            objDataSet = new DataSet();

        Reply objRes = new Reply();

        using (WebClient client = new WebClient())
        {
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(parameter);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);          
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string result = client.UploadString(URL + "/FilterBranchList", "POST", dataEncrypted);

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
                MB_ddl_branch_code.DataSource = objRes.DS;
                MB_ddl_branch_code.DataTextField = "b_code";
                MB_ddl_branch_code.DataBind();
                MB_ddl_branch_code.Items.Insert(0, new ListItem("--Select Branch Code--"));
            }
            else
            {
                PageUtility.MessageBox(this, "Branch Code List Not found");
            }
        }
    }

    private void GetBranchDetails(string BranchCode)
    {
        if (objDataSet == null)
            objDataSet = new DataSet();

        Reply objRes = new Reply();
        
        using (WebClient client = new WebClient())
        {
            parameter =  BranchCode;           

            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(parameter);
            
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);

            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);
                      
            string result = client.UploadString(URL + "/GetBranchDetails", "POST", dataEncrypted );

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
                MB_txt_branchName.Text = objRes.DS.Tables[0].Rows[0]["branch_name"].ToString();
                MB_txt_Region.Text = objRes.DS.Tables[0].Rows[0]["region"].ToString();
                MB_txt_ModuleName.Text = objRes.DS.Tables[0].Rows[0]["module_name"].ToString();
                MB_txt_ModuleCode.Text = objRes.DS.Tables[0].Rows[0]["module_code"].ToString();
                MB_txt_Network.Text = objRes.DS.Tables[0].Rows[0]["network"].ToString();
                MB_txt_PersonName.Text = objRes.DS.Tables[0].Rows[0]["ContactPersonName"].ToString();
                MB_txt_PersonNumber.Text = objRes.DS.Tables[0].Rows[0]["ContactPersonNumber"].ToString();
                MB_txt_Email.Text = objRes.DS.Tables[0].Rows[0]["Email_To"].ToString();
            }
            else
            {
                PageUtility.MessageBox(this, "Data Not Found");

            }

        }

        

    }
    
    protected void MB_ddl_branch_code_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(MB_ddl_branch_code.SelectedIndex == 0)
        {
            MB_txt_branchName.Text = "";
            MB_txt_Region.Text = "";
            MB_txt_ModuleCode.Text = "";
            MB_txt_ModuleName.Text = "";
            MB_txt_Network.Text = "";
            MB_txt_PersonName.Text = "";
            MB_txt_PersonNumber.Text = "";
            MB_txt_Email.Text = "";

        }
        else
        {
            GetBranchDetails(MB_ddl_branch_code.SelectedItem.Text);
        }
    }
}