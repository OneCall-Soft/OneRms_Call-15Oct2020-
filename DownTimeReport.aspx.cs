using System;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Drawing;
using Newtonsoft.Json;

public partial class TransactionReport : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();

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
            DateTime dtTempIterator = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        
            for (int i = 0; i < 12; i++)
            {
                DDL_Duration.Items.Add(dtTempIterator.ToString("MMMM-yyyy"));
                dtTempIterator = dtTempIterator.AddMonths(-1);
            }

            ((Label)Master.FindControl("lblHeading")).Text = "Reports";
            browseLHO();
            GetDowntimeDetail();
        }
    }

    protected void GV_DownTime_Details_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV_DownTime_Details.PageIndex = e.NewPageIndex;
        GetDowntimeDetail();
    }    
   
    
    public void browseLHO()
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all# ";
                }
                else
                {
                    return;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
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

                if (objRes.res == true)
                {
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        ddl_lho_list.DataSource = objRes.DS;
                        ddl_lho_list.DataTextField = "state";
                        ddl_lho_list.DataBind();
                        ddl_lho_list.Items.Insert(0, new ListItem("Select Circle", "NA"));
                    }

                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert( 'LHO Not found.' )</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }
        finally
        {
            GC.Collect();
        }
    }

    public void browseBranch()
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all# #" + ddl_lho_list.SelectedItem.Text;
                }
                else
                {
                    return;
                }
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetBranchList", "POST", dataEncrypted); 

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
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        ddl_lho_list.Items.Add(objRes.DS.Tables[0].Rows[i][0].ToString());
                    }

                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert( 'Branch Not found.' )</script>");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }

        finally
        {
            GC.Collect();
        }
    }

    public void GetDowntimeDetail()
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all# #" + DDL_Duration.SelectedItem.Text +"# #";
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Duration.SelectedItem.Text+"# #";
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetDownTimeReport", "POST", dataEncrypted);

                EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
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
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        Decimal T = Convert.ToDecimal(objRes.DS.Tables[0].Rows[i]["Down Time%"]); // total minte 

                        Decimal rem = T / 100;

                        //string dd = span.Days + "." + span.Hours;
                        //Decimal A = Convert.ToDecimal(T) * 8;
                        //Decimal B = ;
                        //int TempHours = 0;
                        //if (span.Days > 0)
                        //{
                        //    TempHours = span.Days * 8;
                        //}
                        //if (span.Hours > 0)
                        //    TempHours + = span.Hours;

                        string B = (100 - rem).ToString();

                        // total (8) hours in a day, In 30 days = 240 hours = 14400 mint
                        objRes.DS.Tables[0].Rows[i]["Down Time%"] = Convert.ToString(rem);
                        //objRes.DS.Tables[0].Rows[i]["Passbook Printer"] = rem.ToString();

                        objRes.DS.Tables[0].Rows[i]["Avalability%"] = B;
                        objRes.DS.AcceptChanges();
                    }
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    GV_DownTime_Details.DataSource = objRes.DS.Tables[0];
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = true;
                }
                else
                {
                    PageUtility.MessageBox(this, "Data Not Exist. ");
                    GV_DownTime_Details.DataSource = null;
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = false;

                    lbl_tot.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message );
        }

        finally { GC.Collect(); }
    }

    protected void DDL_Duration_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetDowntimeDetail();
    }

    protected void BtnGo_Click(object sender, EventArgs e)
    {

    }

    protected void DDL_Branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LHOFilter();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void btn_Download_Click(object sender, EventArgs e)
    {
        // Clear all content output from the buffer stream
        Response.ClearContent();
        // Specify the default file name using "content-disposition" RESPONSE header
        Response.AppendHeader("content-disposition", "attachment; filename=PbDowntimeReport " + DateTime.Now.ToString("ddMMyy_HHmm") + ".xls");
        // Set excel as the HTTP MIME type
        Response.ContentType = "application/excel";
        // Create an instance of stringWriter for writing information to a string
        StringWriter stringWriter = new StringWriter();
        // Create an instance of HtmlTextWriter class for writing markup 
        // characters and text to an ASP.NET server control output stream
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);

        int ColTot = GV_DownTime_Details.Rows[0].Cells.Count;

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - DOWNTIME CALCULATION REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>");

        GV_DownTime_Details.HeaderStyle.ForeColor = Color.White;
        GV_DownTime_Details.HeaderStyle.BackColor = Color.Blue;
        GV_DownTime_Details.HeaderStyle.Font.Bold = true;
        GV_DownTime_Details.Font.Name = "Calibri";

        GV_DownTime_Details.RenderControl(htw);

        Response.Write(stringWriter.ToString());

        HttpContext.Current.Response.Flush();

        HttpContext.Current.Response.End();

        Response.End();
    }

    public static class PageUtility
    {
        public static void MessageBox(System.Web.UI.Page page, string strMsg)
        {
            //+ character added after strMsg "')"
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);
        }
    }
  
    protected void btnSearchbyLHOcircle_Click(object sender, EventArgs e)
    {
        try
        {
            Reply objRes = new Reply();

            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    if(ddl_lho_list.SelectedIndex > 0)
                        parameter = "all# #" + DDL_Duration.SelectedItem.Text + "#circle#"+ ddl_lho_list.SelectedItem.Text;
                    else
                        parameter = "all# #" + DDL_Duration.SelectedItem.Text + "#circle#";
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Duration.SelectedItem.Text + "#circle#" + ddl_lho_list.SelectedItem.Text;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetDownTimeReport", "POST", dataEncrypted);

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
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        Decimal T = Convert.ToDecimal(objRes.DS.Tables[0].Rows[i]["Down Time%"]); // total minte 

                        Decimal rem = T / 100;

                        //string dd = span.Days + "." + span.Hours;
                        //Decimal A = Convert.ToDecimal(T) * 8;
                        //Decimal B = ;
                        //int TempHours = 0;
                        //if (span.Days > 0)
                        //{
                        //    TempHours = span.Days * 8;
                        //}
                        //if (span.Hours > 0)
                        //    TempHours + = span.Hours;

                        string B = (100 - rem).ToString();

                        // total (8) hours in a day, In 30 days = 240 hours = 14400 mint
                        objRes.DS.Tables[0].Rows[i]["Down Time%"] = Convert.ToString(rem);
                        //objRes.DS.Tables[0].Rows[i]["Passbook Printer"] = rem.ToString();

                        objRes.DS.Tables[0].Rows[i]["Avalability%"] = B;
                        objRes.DS.AcceptChanges();

                    }
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    GV_DownTime_Details.DataSource = objRes.DS.Tables[0];
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = true;
                }
                else
                {
                    PageUtility.MessageBox(this, "Data Not Exist. ");
                    GV_DownTime_Details.DataSource = null;
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = false;

                    lbl_tot.Text = "";
                }
            }
        }
        catch(Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message);
        }
    }

    protected void btn_Search_by_branch_code_Click(object sender, EventArgs e)
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all# #" + DDL_Duration.SelectedItem.Text + "#branch#" + txt_branchcode_searching.Text;
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Duration.SelectedItem.Text + "#branch#" + txt_branchcode_searching.Text;
                }
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetDownTimeReport", "POST", dataEncrypted);  

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
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        Decimal T = Convert.ToDecimal(objRes.DS.Tables[0].Rows[i]["Down Time%"]); // total minte 

                        Decimal rem = T / 100;

                        //string dd = span.Days + "." + span.Hours;
                        //Decimal A = Convert.ToDecimal(T) * 8;
                        //Decimal B = ;
                        //int TempHours = 0;
                        //if (span.Days > 0)
                        //{
                        //    TempHours = span.Days * 8;
                        //}
                        //if (span.Hours > 0)
                        //    TempHours + = span.Hours;

                        string B = (100 - rem).ToString();

                        // total (8) hours in a day, In 30 days = 240 hours = 14400 mint
                        objRes.DS.Tables[0].Rows[i]["Down Time%"] = Convert.ToString(rem);
                        //objRes.DS.Tables[0].Rows[i]["Passbook Printer"] = rem.ToString();

                        objRes.DS.Tables[0].Rows[i]["Avalability%"] = B;
                        objRes.DS.AcceptChanges();
                    }
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    GV_DownTime_Details.DataSource = objRes.DS.Tables[0];
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = true;
                }
                else
                {
                    PageUtility.MessageBox(this, "Data Not Exist. ");
                    //  Response.Write("<script type='text/javascript'>alert( 'Data Not Exist.' )</script>");
                    GV_DownTime_Details.DataSource = null;
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = false;

                    lbl_tot.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message);
        }
    }

    protected void btn_Search_by_serial_no_Click(object sender, EventArgs e)
    {
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all# #" + DDL_Duration.SelectedItem.Text + "#serial#" + txt_SerialNo_searching.Text;
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Duration.SelectedItem.Text + "#serial#" + txt_SerialNo_searching.Text;
                }
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetDownTimeReport", "POST", dataEncrypted);

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
                    for (int i = 0; i < objRes.DS.Tables[0].Rows.Count; i++)
                    {
                        Decimal T = Convert.ToDecimal(objRes.DS.Tables[0].Rows[i]["Down Time%"]); // total minte 

                        Decimal rem = T / 100;

                        //string dd = span.Days + "." + span.Hours;
                        //Decimal A = Convert.ToDecimal(T) * 8;
                        //Decimal B = ;
                        //int TempHours = 0;
                        //if (span.Days > 0)
                        //{
                        //    TempHours = span.Days * 8;
                        //}
                        //if (span.Hours > 0)
                        //    TempHours + = span.Hours;

                        string B = (100 - rem).ToString();

                        // total (8) hours in a day, In 30 days = 240 hours = 14400 mint
                        objRes.DS.Tables[0].Rows[i]["Down Time%"] = Convert.ToString(rem);
                        //objRes.DS.Tables[0].Rows[i]["Passbook Printer"] = rem.ToString();

                        objRes.DS.Tables[0].Rows[i]["Avalability%"] = B;
                        objRes.DS.AcceptChanges();
                    }
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    GV_DownTime_Details.DataSource = objRes.DS.Tables[0];
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = true;
                }
                else
                {
                    PageUtility.MessageBox(this, "Data Not Exist. ");
                    //  Response.Write("<script type='text/javascript'>alert( 'Data Not Exist.' )</script>");
                    GV_DownTime_Details.DataSource = null;
                    GV_DownTime_Details.DataBind();
                    ExportBTN.Visible = false;

                    lbl_tot.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message);
        }
    }
}