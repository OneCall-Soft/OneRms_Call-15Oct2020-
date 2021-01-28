<%@ Application Language="C#" %>

<script runat="server">

    protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
    {
        //Add Lokesh When Browser close then should be user login
        const string HTTPONLY = ";HttpOnly";
        HttpCookie appCookie = new HttpCookie("AppCookie");
        appCookie.Value = "written " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        appCookie.Path = "/RMS_Security/" + HTTPONLY;
        Response.Cookies.Add(appCookie);
    }

    protected void Application_BeginRequest()
    {
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        //Response.Cache.SetNoStore();
    }

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Dictionary<string, HttpSessionState> sessionData =
              new Dictionary<string, HttpSessionState>();
        Application["s"] = sessionData;

    }

    void Application_End(object sender, EventArgs e)
    {
        try
        {
            Session["Username"] = null;
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            foreach (string cookie in Response.Cookies)
            {
                const string HTTPONLY = ";HttpOnly";
                string path = Response.Cookies[cookie].Path;
                if (path.EndsWith(HTTPONLY) == false)
                {
                    //force HttpOnly to be added to the cookie
                    Response.Cookies[cookie].Path += "/RMS_Security/" + HTTPONLY;
                }
            }
        }
        catch (Exception ex )
        {
            //   string USER = globle.UserValue;
            //  ((Dictionary<string, string>)Application["Sessions"]).Remove(USER);
        }

    }

    void Application_Error(object sender, EventArgs e)
    {      
        Exception ex = Server.GetLastError();
        if (ex is HttpRequestValidationException)
        {
            Response.Clear();
            Response.StatusCode = 200;
            Response.Redirect("CustomError.aspx", false);          
            Response.End();
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {
        Session.Remove("Username");
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        Session["Username"] = "";
    }

</script>
