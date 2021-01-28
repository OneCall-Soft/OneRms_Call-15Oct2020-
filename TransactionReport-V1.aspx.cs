using System;

using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TransactionReport : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();

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
            ((Label)Master.FindControl("lblHeading")).Text = "Transaction Report";
        }
    }

    public void GetTxnDetail()
    {
        try
        {
           
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString() == "admin")
                {

                    if (DDL_Duration.SelectedIndex == 0)
                    {
                        PageUtility.MessageBox(this, "Alert : Select Duration");
                        return;
                    }
                    else if (DDL_Duration.SelectedIndex == 1)
                    {
                        perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "#" + DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else if (DDL_Duration.SelectedIndex == 2)
                    {
                        perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") + "#" + DateTime.Now.ToString("yyyy-MM-dd");
                    }

                }
                else
                {
                    if (DDL_Duration.SelectedIndex == 0)
                    {
                        PageUtility.MessageBox(this, "Alert : Select Duration");
                        return;
                    }
                    else if (DDL_Duration.SelectedIndex == 1)
                    {
                        perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DateTime.Now.ToString("yyyy-MM-dd") + "#" + DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else if(DDL_Duration.SelectedIndex == 2)
                    {
                        perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") + "#" + DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(string));

                MemoryStream memStrToSend = new MemoryStream();

                objJsonSerSend.WriteObject(memStrToSend, perameter);

                // string data = Encoding.Default.GetString(memStrToSend.ToArray());

                string result = client.UploadString(URL + "/GetSummerizeTxnReport", "POST", "\"" + perameter + "\"");  // , "\"" + perameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

                MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

                DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

                objRes = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);

            }

            if (objRes.res == true)
            {
                GV_Txn_Details.DataSource = objRes.DS.Tables[0];
                GV_Txn_Details.DataBind();
            }
            else
            {
                Response.Write("<script type='text/javascript'>alert( 'Data Not Exist.' )</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }

        finally { GC.Collect(); }
    }

    protected void DDL_Duration_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTxnDetail();
    }

    protected void BtnGo_Click(object sender, EventArgs e)
    {

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