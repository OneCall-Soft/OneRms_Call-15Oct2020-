using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EncryptionDecryption;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Encoders;

public partial class UserManage : System.Web.UI.Page
{
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string parameter;
    string strUser1;
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

        if (Session["Role"] != null && Session["Role"].ToString().ToLower() != "admin")
        {
            Response.Redirect("Logout.aspx");
        }
        if (!IsPostBack)
        {
            Session["Pass"] = null;

            if (Session["Location"] != null && Session["Location"].ToString().ToLower() == "admin")
            {
                UserType.Items.Add("Admin User");
            }
            else
            {
                UserType.Items.Remove("Admin User");
            }
            bindUserList();
        }
    }
    private void bindUserList()
    {
        try
        {
            Userlist.Items.Clear();
            Reply objRes = new Reply();
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString() == "admin")
                {
                    parameter = "all";
                }
                else
                {
                    parameter = "user";
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetUserList", "POST", dataEncrypted);

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
                    Userlist.DataSource = objRes.DS;
                    Userlist.DataTextField = "username";
                    Userlist.DataBind();
                    Userlist.Items.Insert(0, new ListItem("New User", "NA"));
                }
                else
                {
                    Userlist.Items.Insert(0, new ListItem("New User", "NA"));
                }
            }
        }
        catch (Exception ex)
        {
            Userlist.Items.Insert(0, new ListItem("New User", "NA"));
        }
    }
    protected void UserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (UserType.SelectedIndex == 0)
        {
            locationList.Items.Clear();
            locationList.Items.Add("Select");
            locationList.Enabled = false;
        }
        else if (UserType.SelectedIndex == 2 || UserType.SelectedIndex == 1)
        {
            bindCirclelist(UserType.SelectedValue);
    
            locationList.Visible = true;
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "UpdateTextBox();", true);
        }
        else if(UserType.SelectedIndex == 3)
        {
           locationList.Visible = false; 
        }
    }
    private void bindCirclelist(string type)
    {
        try
        {
            Reply objRes = new Reply();
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetLocationList", "POST", dataEncrypted);

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
                locationList.Items.Clear();
                if (objRes.res == true)
                {
                    locationList.DataSource = objRes.DS;
                    if (UserType.SelectedIndex == 2)
                        locationList.DataTextField = type;
                    else
                        locationList.DataTextField = "branch_name";

                    locationList.DataBind();
                    locationList.Items.Insert(0, new ListItem("Select", "NA"));
                }
                else
                {
                    locationList.Items.Insert(0, new ListItem("No location found", "NA"));
                }
                locationList.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    private static bool hasSpecialChar(string input)
    {
        string specialChar = @"\|!#$%&/()=?@{}.-;'<>_,`~@₹^*+";
        foreach (var item in specialChar)
        {
            if (input.Contains(item)) return true;
        }

        return false;
    }
   
    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        try
        {

            if (btnCreateUser.Text == "Modify The User")
            {
                strUser1 = AESEncrytDecry.EncryptStringAES(usernametext2.Text);
            }
            else
            {
                strUser1 = usernametext2.Text;
            }
            string  strUser = AESEncrytDecry.DecryptStringAES(strUser1);
            usernametext2.Text = "";

            string strpassword = AESEncrytDecry.DecryptStringAES(password.Text);
            password.Text = "";

            string strReptpassword = AESEncrytDecry.DecryptStringAES(repeatPassword.Text);
            repeatPassword.Text = "";

            string strSans = AESEncrytDecry.DecryptStringAES(answer.Text);
            answer.Text = "";

            string strMobile = AESEncrytDecry.DecryptStringAES(mobile_phone.Text);
            mobile_phone.Text = "";

            if (!strpassword.Any(char.IsUpper) || !strpassword.Any(char.IsLower) || strpassword.Length < 8 || !hasSpecialChar(strpassword) || !strpassword.Any(char.IsDigit) || strpassword.Length > 14)
            {
                password.Attributes["value"] = "";
                repeatPassword.Attributes["value"] = "";
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "UserAlert('Password Must Contain atleast One Upper Case, One Lower Case, One Special Character, One Numeric Value and minimum length is 8');", true);
                return;
            }

            WebClient client = new WebClient();
            UserDetails userDetails = new UserDetails();
            userDetails.res = false;
            if (btnCreateUser.Text == "Create New User")
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString1 = JsonConvert.SerializeObject(strUser);
                EncRequest objEncRequest1 = new EncRequest();
                objEncRequest1.RequestData = AesGcm256.Encrypt(JsonString1);
                string dataEncrypted1 = JsonConvert.SerializeObject(objEncRequest1);

                string result = client.UploadString(URL + "/GetUserDetails", "POST", dataEncrypted1);

                EncResponse objResponse1 = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse1.ResponseData = AesGcm256.Decrypt(objResponse1.ResponseData);

                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS1 = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS1 = new MemoryStream(Encoding.UTF8.GetBytes(objResponse1.ResponseData));
                //userDetails = (UserDetails)objDCS1.ReadObject(objMS1);

                Newtonsoft.Json.JsonSerializer json1 = new Newtonsoft.Json.JsonSerializer();
                json1.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr1 = new StringReader(objResponse1.ResponseData);
                Newtonsoft.Json.JsonTextReader reader1 = new JsonTextReader(sr1);
                userDetails = json1.Deserialize<UserDetails>(reader1);

                if (userDetails.res)
                {
                    Response.Write("<script>alert('Username already Exist')</script>");
                    usernametext2.Text = "";
                    password.Text = "";
                    repeatPassword.Text = "";
                    useremail.Text = "";
                    mobile_phone.Text = "";
                    recoveryQuestion.SelectedIndex = 0;
                    answer.Text = "";
                    UserType.SelectedIndex = 0;
                    locationList.SelectedIndex = 0;
                    return;
                }
                else
                {
                    userDetails.res = true;
                }
            }

            userDetails.Username = strUser;
            userDetails.Usertype = UserType.SelectedValue == "Admin User" ? "admin" : UserType.SelectedValue;
            userDetails.location = UserType.SelectedItem.Text == "Admin User" ? "Admin User" : locationList.SelectedItem.Text;
            userDetails.Answer = strSans;
            userDetails.email = useremail.Text;
            if (Session["Pass"] != null && Session["Pass"].ToString() == strpassword)
                userDetails.password = strpassword;
            else
                userDetails.password = "@" + strpassword;

            userDetails.phone = strMobile;
            userDetails.securityQuestion = recoveryQuestion.SelectedItem.Text;
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(userDetails);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string resultUser = client.UploadString(URL + "/CreateUser", "POST", dataEncrypted);

            EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(resultUser);
            objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

            //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
            //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
            //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
            //resultUser = (string)objDCS.ReadObject(objMS);

            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            StringReader sr = new StringReader(objResponse.ResponseData);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            userDetails = json.Deserialize<UserDetails>(reader);

            if (resultUser=="true")
            {

                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "UserSuccess();", true);
                Session["Pass"] = null;
                UserType.SelectedIndex = 0;
                locationList.Items.Clear();
                locationList.Items.Add("Select");
                usernametext2.Text = "";
                password.Text = "";
                repeatPassword.Text = "";
                useremail.Text = "";
                mobile_phone.Text = "";
                answer.Text = "";
                recoveryQuestion.SelectedIndex = 0; reset.Enabled = true; usernametext2.Enabled = true; password.Attributes["value"] = ""; repeatPassword.Attributes["value"] = "";
                btnCreateUser.Text = "Create New User";
                reset.Enabled = true;
                locationList.Enabled = false;
            }
            else
            {
                PageUtility.MessageBox(this, "UserFail.");
            }
        }
        catch (Exception ex)
        {

            PageUtility.MessageBox(this, "Excp -: " + ex.Message);

        }
        finally { bindUserList(); }
    }


    public static class PageUtility
    {
        public static void MessageBox(System.Web.UI.Page page, string strMsg)
        {
            //+ character added after strMsg "')"
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);

        }
    }

    protected void Userlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Userlist.SelectedIndex == 0)
            {
                Session["Pass"] = null;
                UserType.SelectedIndex = 0;
                locationList.Items.Clear();
                locationList.Items.Add("Select");
                usernametext2.Text = ""; password.Text = ""; repeatPassword.Text = ""; useremail.Text = ""; mobile_phone.Text = ""; answer.Text = "";
                recoveryQuestion.SelectedIndex = 0; reset.Enabled = true; usernametext2.Enabled = true; password.Attributes["value"] = ""; repeatPassword.Attributes["value"] = "";
                btnCreateUser.Text = "Create New User";
                usernametext2.Attributes.Remove("disabled");
                locationList.Enabled = false;
                btnDeleteUser.Visible = false;
            }
            else
            {
                string Uname = Userlist.SelectedItem.Text;
                UserDetails userDetails = new UserDetails();
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    string JsonString = JsonConvert.SerializeObject(Uname);
                    EncRequest objEncRequest = new EncRequest();
                    objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                    string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                    string result = client.UploadString(URL + "/GetUserDetails", "POST", dataEncrypted);

                    EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                    objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);


                    //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                    //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                    //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                    //userDetails = (UserDetails)objDCS.ReadObject(objMS);

                    Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                    json.NullValueHandling = NullValueHandling.Ignore;
                    StringReader sr = new StringReader(objResponse.ResponseData);
                    Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                    userDetails = json.Deserialize<UserDetails>(reader);

                    if (userDetails.res)
                    {
                        if (userDetails.Usertype.Trim(' ').ToLower() == "branch")
                            UserType.SelectedIndex = 1;
                        else
                            UserType.SelectedIndex = 2;
                        bindCirclelist(userDetails.Usertype.Trim(' '));
                        locationList.SelectedIndex = locationList.Items.IndexOf(locationList.Items.FindByText(userDetails.location.TrimEnd()));
                       
                        password.Attributes["value"] = userDetails.password;
                        repeatPassword.Attributes["value"] = userDetails.password;
                        useremail.Text = userDetails.email;
                        mobile_phone.Text = userDetails.phone;
                        recoveryQuestion.SelectedIndex = recoveryQuestion.Items.IndexOf(recoveryQuestion.Items.FindByText(userDetails.securityQuestion));
                        answer.Text = userDetails.Answer;
                        Session["Pass"] = userDetails.password;
                        usernametext2.Text = Uname;
                        usernametext2.Enabled = false;


                        if (Session["Role"] != null && Session["Role"].ToString().ToLower() == "admin")
                        {
                            btnReset.Visible = true;
                        }
                        else
                        {
                            btnReset.Visible = false;
                        }


                        reset.Enabled = false;
                        btnCreateUser.Text = "Modify The User";
                        btnDeleteUser.Visible = true;
                        var page = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "UpdateTextBox();", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    protected void reset_Click(object sender, EventArgs e)
    {
        Session["Pass"] = null;
        UserType.SelectedIndex = 0;
        locationList.Items.Clear();
        locationList.Items.Add("Select");
        usernametext2.Text = ""; password.Text = ""; repeatPassword.Text = ""; useremail.Text = ""; mobile_phone.Text = ""; answer.Text = "";
        recoveryQuestion.SelectedIndex = 0; reset.Enabled = true; usernametext2.Enabled = true; password.Attributes["value"] = ""; repeatPassword.Attributes["value"] = "";
        btnCreateUser.Text = "Create New User";
        locationList.Enabled = false;
    }


    protected void btnDeleteUser_Click(object sender, EventArgs e)
    {
        try
        {

            string strUser = usernametext2.Text;

            WebClient client = new WebClient();
            UserDetails userDetails = new UserDetails();
            userDetails.res = false;
            userDetails.Username = strUser;

            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject(userDetails);
            EncRequest objEncRequest = new EncRequest();
            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

            string resultUser = client.UploadString(URL + "/DeleteUser", "POST", dataEncrypted);

            EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(resultUser);
            objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
            //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
            //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
            //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
            //userDetails = (UserDetails)objDCS.ReadObject(objMS);
            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            StringReader sr = new StringReader(objResponse.ResponseData);
            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
            userDetails = json.Deserialize<UserDetails>(reader);

            if (resultUser=="true")
            {
             
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "UserDeleteSuccess();", true);
                Session["Pass"] = null;
                UserType.SelectedIndex = 0;
                locationList.Items.Clear();
                locationList.Items.Add("Select");
                usernametext2.Text = "";
                password.Text = "";
                repeatPassword.Text = "";
                useremail.Text = "";
                mobile_phone.Text = "";
                answer.Text = "";
                recoveryQuestion.SelectedIndex = 0; reset.Enabled = true; usernametext2.Enabled = true; password.Attributes["value"] = ""; repeatPassword.Attributes["value"] = "";
                btnCreateUser.Text = "Create New User";
                reset.Enabled = true;
                locationList.Enabled = false;
            }
            else
            {
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "UserFailToDelete();", true);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            throw;
        }
        finally { bindUserList(); }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(usernametext2.Text))
        {           
            string password = CreateRandomPassword(8);

            ResetPassword objReq = new ResetPassword();            
            objReq.name = usernametext2.Text;
            objReq.password = password;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(objReq);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string resultUser = client.UploadString(URL + "/ResetPassword", "POST", dataEncrypted);

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(resultUser);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);
                //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                //resultUser = (string)objDCS.ReadObject(objMS);

                Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                json.NullValueHandling = NullValueHandling.Ignore;
                StringReader sr = new StringReader(objResponse.ResponseData);
                Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                resultUser = json.Deserialize<string>(reader);

                if (resultUser=="true")
                {
                    PageUtility.MessageBox(this, "Password is :-  " + password);
                }
                else
                {
                    PageUtility.MessageBox(this, "Error :-  " + resultUser);
                }

            }      
        }
        else
        {
            PageUtility.MessageBox(this, "Please enter User Name ! " );
        }
    }


    private static string CreateRandomPassword(int PasswordLength)
    {
        string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (PasswordLength-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(_allowedChars[(int)(num % (uint)_allowedChars.Length)]);
            }
        }
        return res.ToString();
    }
}