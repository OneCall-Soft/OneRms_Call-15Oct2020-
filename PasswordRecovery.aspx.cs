using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PasswordRecovery : System.Web.UI.Page
{
    public static string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] != null || Session["PassChange"] != null || Session["Cpassword"] != null)
        {
            Session["Username"] = null; Response.Redirect("Default.aspx", false);
        }
        if (!IsPostBack)
        {
            Response.Cookies.Add(new HttpCookie("__AntiXsrfToken", "")); // for cookies request null;
            captchaGenerate();
            this.Page.Form.DefaultButton = Login.ID;
        }
    }

    private Int32 GenerateRandomKey(int length)
    {
        const string valid = "1234567890";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return Convert.ToInt32(res.ToString());
    }
    private Int32 GenerateRandomKey(int length, int MaxValue)
    {
        const string valid = "1234567890";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return Convert.ToInt32(res.ToString()) + 100;
    }
    private string GenerateRandomKeyCaptcha(int length)
    {
        const string valid = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
        StringBuilder res = new StringBuilder();
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                res.Append(valid[(int)(num % (uint)valid.Length)]);
            }
        }

        return res.ToString();
    }

    private void captchaGenerate()
    {
       
        Bitmap objBitmap = new Bitmap(130, 80);
        Graphics objGraphics = Graphics.FromImage(objBitmap);
        objGraphics.Clear(Color.White);        

        objGraphics.DrawLine(Pens.Black, GenerateRandomKey(2), GenerateRandomKey(2), GenerateRandomKey(3), GenerateRandomKey(2));
        objGraphics.DrawRectangle(Pens.Blue, GenerateRandomKey(2), GenerateRandomKey(2), GenerateRandomKey(2), GenerateRandomKey(2));
        objGraphics.DrawLine(Pens.Blue, GenerateRandomKey(2), GenerateRandomKey(2), GenerateRandomKey(2), GenerateRandomKey(2));
                
        Brush objBrush =
            default(Brush);
        //create background style  
        HatchStyle[] aHatchStyles = new HatchStyle[]
        {
                HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal, HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
                    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross, HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                    HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
        };
        //create rectangular area  
        RectangleF oRectangleF = new RectangleF(0, 0, 300, 300);
        objBrush = new HatchBrush(aHatchStyles[GenerateRandomKey(2) / 9], Color.FromArgb((GenerateRandomKey(2, 255)), (GenerateRandomKey(2, 255)), (GenerateRandomKey(2, 255))), Color.White);

        objGraphics.FillRectangle(objBrush, oRectangleF);
        //Generate the image for captcha  
        string captchaText = string.Format("{0:X}", GenerateRandomKeyCaptcha(6));
        //add the captcha value in session  
        Session["CaptchaVerify"] = captchaText;
        Font objFont = new Font("Courier New", 15, FontStyle.Bold);
        //Draw the image for captcha  
        objGraphics.DrawString(captchaText, objFont, Brushes.Black, 20, 20);
        MemoryStream ms = new MemoryStream();
        objBitmap.Save(ms, ImageFormat.Gif);

        var base64Data = Convert.ToBase64String(ms.ToArray());
        CapImage.ImageUrl = "data:image/gif;base64," + base64Data;
    }
    protected void Login_Click(object sender, EventArgs e)
    {
        try
        {
            if (useremail.Text == "" || UserID.Text == "" || recoveryQuestion.SelectedIndex == 0 || answer.Text == "" || capCode.Text == "")
            {
                captchaGenerate();
                //var page = HttpContext.Current.CurrentHandler as Page;
                //ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "Error('Please Fill All Details');", true);
                PageUtility.MessageBox(this, "Please Fill All Details");
                return;
            }
            else
            {
                if (capCode.Text != Session["CaptchaVerify"].ToString())
                {
                    captchaGenerate();
                    //var page = HttpContext.Current.CurrentHandler as Page;
                    //ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "Captch('Captcha Mismatch');", true);

                    PageUtility.MessageBox(this, "Captcha Mismatch");
                    return;
                }


                using (WebClient client = new WebClient())
                {
                    PasscodeRecovery passcodeRecovery = new PasscodeRecovery();
                    passcodeRecovery.Answer = answer.Text;
                    passcodeRecovery.Email = useremail.Text;
                    passcodeRecovery.SecurityQuestion = recoveryQuestion.SelectedItem.Text;
                    passcodeRecovery.Username = UserID.Text;
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(PasscodeRecovery));

                    MemoryStream memStrToSend = new MemoryStream();

                    objJsonSerSend.WriteObject(memStrToSend, passcodeRecovery);

                    string data = Encoding.Default.GetString(memStrToSend.ToArray());

                    string result = client.UploadString(URL + "/Passwordrecovery", "POST", data);
             
                    if (result.ToLower().Contains("success"))
                    {
                        var page1 = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page1, page1.GetType(), "alert", "alert('Please Check Your Email For New Password And Login with it');window.location ='Default.aspx';", true);

                       // PageUtility.MessageBox(this, "Captcha Mismatch");

                    }
                    else if(result.ToLower().Contains("password"))
                    {
                        var page2 = HttpContext.Current.CurrentHandler as Page;
                        ScriptManager.RegisterStartupScript(page2, page2.GetType(), "alert", "alert('"+ result.Replace("\"", "") + "');window.location ='Default.aspx';", true);

                    }
                    else
                    {
                        captchaGenerate();
                        PageUtility.MessageBox(this, "Invalid Details ! Please contact ADMINISTRATOR on Email : swayam.lipi@sbi.co.in");             

                        // please contact administrator on Email : swayam.lipi@sbi.co.in
                        //var page = HttpContext.Current.CurrentHandler as Page;
                        //ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "Error('No Such User Exist');", true);
                    }
                }
            }
        }
        catch (Exception ew)
        {
            //var page = HttpContext.Current.CurrentHandler as Page;
            //ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "Error " + ew.Message.ToString() + ew.Message, true);

            PageUtility.MessageBox(this, "Excp- "+ ew.Message.ToString() + ew.Message);

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



    protected void refresh_Click(object sender, ImageClickEventArgs e)
    {
        captchaGenerate();
    }

}