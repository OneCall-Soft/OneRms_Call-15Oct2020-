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

public partial class CreateBranch : System.Web.UI.Page
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
            ((Label)Master.FindControl("lblHeading")).Text = "Branch Management ";
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
                CB_ddl_circle_name.DataSource = objRes.DS;
                CB_ddl_circle_name.DataTextField = "state";
                CB_ddl_circle_name.DataValueField = "id";
                CB_ddl_circle_name.DataBind();
                CB_ddl_circle_name.Items.Insert(0, new ListItem("--Select LHO/Circle Name--", "NA"));
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

    protected void btn_save_branch_click(object sender, EventArgs e)
    {
        if (CB_txt_branch_Code.Text == "" ||
            CB_txt_Branch_Name.Text == "" ||
            CB_txt_Module_Code.Text == "" ||
            CB_txt_Module_Name.Text == "" ||
            CB_txt_Network.Text == "" ||
            CB_txt_Region.Text == "" )
        { 
            PageUtility.MessageBox(this, "Please enter all values ");
            return;
        }


        if (CB_txt_branch_Code.Text.Length < 6 )
        {
            PageUtility.MessageBox(this, "Please enter 6 Digita Branch Code  ");
            return;
        }


        if ((CB_txt_branch_Code.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch code accepts only numeric values ! Try again");
            CB_txt_branch_Code.Text.Remove(CB_txt_branch_Code.Text.Length - 1);
            return;
        }

        if ((CB_txt_Branch_Name.Text.ToCharArray().All(c => Char.IsLetter(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch name accepts only alphabetical characters ! Try again");
            CB_txt_Branch_Name.Text.Remove(CB_txt_Branch_Name.Text.Length - 1);
            return;
        }

        if ((CB_txt_Module_Code.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in module code accepts only numeric values ! Try again");
            CB_txt_Module_Code.Text.Remove(CB_txt_Module_Code.Text.Length - 1);
            return;
        }

        if ((CB_txt_Module_Name.Text.ToCharArray().All(c => Char.IsLetter(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in module name accepts only alphabetical characters ! Try again");
            CB_txt_Module_Name.Text.Remove(CB_txt_Module_Name.Text.Length - 1);
            return;
        }

        if ((CB_txt_Network.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch code accepts only numeric values ! Try again");
            CB_txt_Network.Text.Remove(CB_txt_Network.Text.Length - 1);
            return;
        }

        if ((CB_txt_Region.Text.ToCharArray().All(c => Char.IsNumber(c))) == false)
        {
            PageUtility.MessageBox(this, "Input data is invalid in branch code accepts only numeric values ! Try again");
            CB_txt_Region.Text.Remove(CB_txt_Region.Text.Length - 1);
            return;
        } 

        try
        {
            BranchDetails branchDetails = new BranchDetails();
            branchDetails.BranchCode = CB_txt_branch_Code.Text;
            branchDetails.BranchName = CB_txt_Branch_Name.Text;
            branchDetails.PersonName = CB_txt_Contact_Person_Name.Text;
            branchDetails.PersonPhone = CB_txt_Contact_Person_Number.Text;
            branchDetails.PersonEmail = CB_txt_Email.Text;
            branchDetails.ModuleCode = CB_txt_Module_Code.Text;
            branchDetails.ModuleName = CB_txt_Module_Name.Text;
            branchDetails.Network = CB_txt_Network.Text;
            branchDetails.Region = CB_txt_Region.Text;
            branchDetails.CircleCode = CB_ddl_circle_name.SelectedItem.Value;

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(branchDetails);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string result = client.UploadString(URL + "/CreateBranch", "POST", dataEncrypted);

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

            if(result == null)
            {
                PageUtility.MessageBox(this, "Branch Create Failed");
            }
            else if (result.Contains("true"))
            {
                PageUtility.MessageBox(this, "Branch Created Successfully");
            }
            else
            {
                PageUtility.MessageBox(this, "Branch Code Already Exist!");
            }
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "Catch Error : " + ex.Message.ToString());
        }
        finally
        {
            CB_txt_branch_Code.Text = CB_txt_Branch_Name.Text =
            CB_txt_Contact_Person_Name.Text = CB_txt_Contact_Person_Number.Text =
            CB_txt_Email.Text = CB_txt_Module_Code.Text = CB_txt_Module_Name.Text =
            CB_txt_Network.Text = CB_txt_Region.Text = "";
            CB_ddl_circle_name.ClearSelection();           
        }
    }

    protected void CB_txt_branch_Code_TextChanged(object sender, EventArgs e)
    {
        if (CB_txt_branch_Code.Text.Length != 6)
        {
            PageUtility.MessageBox(this, "Please enter 6 digit branch code with prefix 0 ");
           // return;
        }
        if (CB_txt_branch_Code.Text != "")
            SearchBranchName_Code("code", CB_txt_branch_Code.Text.Trim());
    }

    private void SearchBranchName_Code(string type, string Value)
    {
        if (objDataSet == null)
            objDataSet = new DataSet();

        Reply objRes = new Reply();

        using (WebClient client = new WebClient())
        {
            if (Session["Role"].ToString().ToLower().Contains("admin"))
            {
                parameter = type + "#" + Value;
            }
           
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(parameter);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string result = client.UploadString(URL + "/FilterBranchCode_Name", "POST", dataEncrypted);

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

                PageUtility.MessageBox(this, "Branch Name already exist! . Try again.");
                btn_save_branch.Enabled = false;
            }
            else
            {
                btn_save_branch.Enabled = true;
            }
        }
    }


    protected void CB_txt_Branch_Name_TextChanged(object sender, EventArgs e)
    {
        if (CB_txt_Branch_Name.Text != "")
            SearchBranchName_Code("Name", CB_txt_Branch_Name.Text.Trim());
    }
}