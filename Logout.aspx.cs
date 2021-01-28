using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            else
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                Session["Username"] = null;
                Session.Abandon();
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                Response.Cookies.Add(new HttpCookie("__AntiXsrfToken", ""));
                Request.Cookies.Clear();

                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AddHeader("Expires", "0");
                Session.Abandon(); // Session Expire but cookie do exist
                                   //  Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-30); //Delete the cookie
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                Request.Cookies["Asp.net_sessionId"].Expires = DateTime.UtcNow.AddDays(-1d);
                Response.Cookies["Asp.net_sessionId"].Value = "";
                Response.Cookies["Username"].Value = "";
                Response.Cookies.Add(Request.Cookies["Username"]);

                Session.RemoveAll();
                Session.Abandon();
                Session["Username"] = null;                
                Session.Clear();
                ClearCache();
                string USER = globle.UserValue;         

                FormsAuthentication.SignOut();
            
                Context.ApplicationInstance.CompleteRequest();
                bool redirected = false;
                bool isAdded = false;
                System.Web.SessionState.SessionIDManager Manager = new System.Web.SessionState.SessionIDManager();
                string NewID = Manager.CreateSessionID(Context);
                string OldID = Context.Session.SessionID;                
                Manager.SaveSessionID(Context, NewID, out redirected, out isAdded);
               
            }

        }
        catch (Exception)
        {
          //  string USER = globle.UserValue;
          //  Dictionary<string, string> dic = ((Dictionary<string, string>)Application["Sessions"]);
          //  ((Dictionary<string, string>)Application["Sessions"]).Remove(USER);
        }
  
    }

    public static void ClearCache()
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetExpires(DateTime.Now);
        HttpContext.Current.Response.Cache.SetNoServerCaching();
        HttpContext.Current.Response.Cache.SetNoStore();
        HttpContext.Current.Response.Cookies.Clear();
        HttpContext.Current.Request.Cookies.Clear();
    }

    private void clearchachelocalall()
    {
        string GooglePath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Google\Chrome\User Data\Default\";
        string MozilaPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Mozilla\Firefox\";
        string Opera1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Opera\Opera";
        string Opera2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Opera\Opera";
        string Safari1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Apple Computer\Safari";
        string Safari2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Apple Computer\Safari";
        string IE1 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Intern~1";
        string IE2 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\History";
        string IE3 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Microsoft\Windows\Tempor~1";
        string IE4 = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Microsoft\Windows\Cookies";
        string Flash = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Roaming\Macromedia\Flashp~1";

        //Call This Method ClearAllSettings and Pass String Array Param
        ClearAllSettings(new string[] { GooglePath, MozilaPath, Opera1, Opera2, Safari1, Safari2, IE1, IE2, IE3, IE4, Flash });

    }
    public void ClearAllSettings(string[] ClearPath)
    {
        foreach (string HistoryPath in ClearPath)
        {
            if (Directory.Exists(HistoryPath))
            {
                DoDelete(new DirectoryInfo(HistoryPath));
            }
        }
    }
    void DoDelete(DirectoryInfo folder)
    {
        try
        {
            foreach (FileInfo file in folder.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                { }
            }
            foreach (DirectoryInfo subfolder in folder.GetDirectories())
            {
                DoDelete(subfolder);
            }
        }
        catch
        {
        }
    }

}