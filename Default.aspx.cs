
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Utilities.Encoders;
using System.Web.Security;
using Microsoft.IdentityModel.Claims;
using System.Threading;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{

    public const string AntiXsrfTokenKey = "__AntiXsrfToken";
    public const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    public string _antiXsrfTokenValue;

    public static string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string strAdmin = System.Configuration.ConfigurationManager.AppSettings["Admin"].ToString();
    

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
        catch (Exception ex)
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
        if (Session["Username"] != null)
        {
            // Session["Username"] = null;

        }
        if (!IsPostBack)
        {
            try
            {
                string LoginType = System.Configuration.ConfigurationManager.AppSettings["SSOLogin"].ToString();

                if (LoginType.ToLower() == "false")
                {
                    Div_UserLogin.Visible = true;
                    Div_SSOBody.Visible = false;
                }
                else
                {  
                    //SSO 
                    Microsoft.IdentityModel.Claims.ClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as Microsoft.IdentityModel.Claims.ClaimsPrincipal;
                    if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
                    {
                        IClaimsIdentity claimsIdentity = claimsPrincipal.Identity as IClaimsIdentity;
                        if (claimsIdentity != null)
                        {
                            if (claimsIdentity.Claims.Count > 0)
                            {
                                lbl_PF.Text = claimsIdentity.Claims[0].Value.Split('@')[0];
                                lbl_name.Text = claimsIdentity.Claims[5].Value;

                                if (strAdmin == "1")
                                {
                                    //Production Server
                                    lbl_code.Text = "0" + claimsIdentity.Claims[17].Value;
                                    lbl_Mobile.Text = claimsIdentity.Claims[27].Value;
                                    lbl_Email.Text = claimsIdentity.Claims[28].Value;
                                }
                                else
                                {
                                    //UAT Server
                                    lbl_code.Text = "0" + claimsIdentity.Claims[11].Value;
                                    lbl_Mobile.Text = claimsIdentity.Claims[20].Value;
                                    lbl_Email.Text = claimsIdentity.Claims[21].Value;
                                }

                                btn_login.Enabled = true;
                            }
                        }
                        else
                        {
                            btn_login.Enabled = false;
                            lbl_error.Visible = true;
                            lbl_error.Text = "User claimsIdentity details not found";
                        }
                    }
                    else
                    {
                        string User = System.Configuration.ConfigurationManager.AppSettings["TestUser"].ToString();

                        if (User == "1")
                        {
                            Div_UserLogin.Visible = false;
                            Div_SSOBody.Visible = true;
                            btn_login.Enabled = false;
                            lbl_error.Visible = true;
                            lbl_error.Text = "User details not found";
                            lbl_PF.Text = "LIP3";
                            lbl_name.Text = "S";
                            lbl_code.Text = "016516";
                            lbl_Mobile.Text = "9999999993";
                            lbl_Email.Text = "3@sbi.co.in";
                            btn_login.Enabled = true;
                        }
                        else
                        {
                            lbl_error.Visible = true;
                            lbl_error.Text = "User details not found";
                            lbl_PF.Text = "LIP4";
                            lbl_name.Text = "T";
                            lbl_code.Text = "000024";
                            lbl_Mobile.Text = "8888888888";
                            lbl_Email.Text = "4@sbi.co.in";
                            btn_login.Enabled = true;
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                btn_login.Enabled = false;
                lbl_error.Visible = true;
                lbl_error.Text = "catch error : - " + ex.Message;
            }
        }
    }

    private string TrimString(string text, char char1)
    {
        string str = null;
        string[] strArr = null;

        str = text;
        char[] splitchar = { char1 };
        strArr = str.Split(splitchar);

        string output = strArr[0];
        return output;
    }


    public static class PageUtility
    {
        public static void MessageBox(System.Web.UI.Page page, string strMsg)
        {
            //+ character added after strMsg "')"
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);
        }
    }


    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class Reply
    {
        public DataSet DS { get; set; }

        public bool res { get; set; }
      
        public string strError { get; set; }
    }

    protected void btn_login_Click(object sender, EventArgs e)
    {
        try
        {
            if(Div_SSOBody.Visible != true)
            {
                if ((!string.IsNullOrEmpty(txtPassword.Text)) && (!string.IsNullOrEmpty(txtUserName.Text)))
                {
                    Reply objRes = new Reply();
                    try
                    {
                        if (txtUserName.Text == "" || txtUserName.Text == null)
                        {
                            txtUserName.Text = "";
                            txtUserName.Text = "";
                            Response.Write("<script>alert('Kindly enter username')</script>");
                            return;
                        }
                        if (txtPassword.Text == "" || txtPassword.Text == null)
                        {

                            txtUserName.Text = "";
                            txtPassword.Text = "";
                            Response.Write("<script>alert('Kindly enter password')</script>");
                            return;
                        }

                        UserLogin userLogin = new UserLogin();
                        var decodedString1 = Base64.Decode(txtUserName.Text);
                        userLogin.UserName = Convert.ToString(TrimString(System.Text.Encoding.UTF8.GetString(decodedString1), '&'));
                        var decodedString2 = Base64.Decode(txtPassword.Text);
                        userLogin.Password = Convert.ToString(TrimString(System.Text.Encoding.UTF8.GetString(decodedString2), '*'));
                        WebClient objWC = new WebClient();
                        objWC.Headers[HttpRequestHeader.ContentType] = "text/json";

                        string JsonString = JsonConvert.SerializeObject(userLogin);
                        EncRequest objEncRequest = new EncRequest();
                        objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                        string EncData = JsonConvert.SerializeObject(objEncRequest);
                        string result = objWC.UploadString(URL + "/User_login", "POST", EncData);

                        EncResponse objEncResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                        objEncResponse.ResponseData = AesGcm256.Decrypt(objEncResponse.ResponseData);

                        JsonSerializer json = new JsonSerializer();
                        json.NullValueHandling = NullValueHandling.Ignore;
                        StringReader sr = new StringReader(objEncResponse.ResponseData);
                        Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                        objRes = json.Deserialize<Reply>(reader);

                        if (objRes.res == true)
                        {
                            if (objRes.DS.Tables[0].Rows[0]["Credential_type"].ToString().ToLower().Trim() == "admin")
                            {
                                globle.BankName = ConfigurationManager.AppSettings["BankName"].ToString();
                                globle.CallLogRequired = ConfigurationManager.AppSettings["CallLogRequired"].ToString();

                                Session["Username"] = userLogin.UserName;
                                Session["Role"] = objRes.DS.Tables[0].Rows[0]["credential_type"].ToString();
                                Session["Location"] = "";
                                Session["PF_Index"] = "";

                                globle.UserValue = userLogin.UserName;
                                globle.Role = "admin";
                                globle.Location = "";
                                globle.PF_Index = "LIP3";

                                Response.Redirect("Dashboard-V3_1.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();

                            }
                            else if (objRes.DS.Tables[0].Rows[0]["Credential_type"].ToString().ToLower().Trim() == "user")
                            {
                                globle.BankName = ConfigurationManager.AppSettings["BankName"].ToString();
                                globle.CallLogRequired = ConfigurationManager.AppSettings["CallLogRequired"].ToString();

                                Session["Username"] = userLogin.UserName;
                                Session["Role"] = "Nonadmin";
                                Session["Location"] = "";
                                Session["PF_Index"] = "";

                                globle.UserValue = userLogin.UserName;
                                globle.Role = "Nonadmin";
                                globle.Location = "";                              

                                Response.Redirect("Dashboard-V3_1.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();
                            }
                            else
                            {
                                txtUserName.Text = "";
                                txtPassword.Text = "";
                                Response.Write("<script>alert('Invalid Username Password')</script>");
                            }


                        }
                      
                        else
                        {
                            txtUserName.Text = "";
                            txtPassword.Text = "";
                            Response.Write("<script>alert('Invalid Username Password')</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        txtUserName.Text = "";
                        txtPassword.Text = "";
                    }
                }
                else
                {
                    PageUtility.MessageBox(this, "Filed is empty ! Try again");
                }
            }
            else
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (lbl_PF.Text.ToLower().Contains("lip"))
                {
                    globle.BankName = ConfigurationManager.AppSettings["BankName"].ToString();
                    globle.CallLogRequired = ConfigurationManager.AppSettings["CallLogRequired"].ToString();
                    Session["Username"] = lbl_name.Text;
                    Session["Role"] = "admin";
                    Session["Location"] = "";
                    Session["PF_Index"] = lbl_PF.Text;
                    globle.UserValue = lbl_name.Text;

                    globle.Role = "admin";
                    globle.Location = "";
                    globle.PF_Index = lbl_PF.Text;


                    Response.Redirect("Dashboard-V3_1.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    globle.BankName = ConfigurationManager.AppSettings["BankName"].ToString();
                    globle.CallLogRequired = ConfigurationManager.AppSettings["CallLogRequired"].ToString();
                    Session["Username"] = lbl_name.Text;
                    Session["Role"] = "Nonadmin";
                    Session["Location"] = "";
                    Session["PF_Index"] = lbl_PF.Text;
                    globle.UserValue = lbl_name.Text;

                    globle.Role = "Nonadmin";
                    globle.Location = "";
                    globle.PF_Index = lbl_PF.Text;

                    Response.Redirect("Dashboard-V3_1.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();

                }
            }

            
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "Exception: -" + ex.Message + "");
        }
    }
}