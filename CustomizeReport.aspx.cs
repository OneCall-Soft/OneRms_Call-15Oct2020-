
using Newtonsoft.Json;
using System;
using ClosedXML.Excel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TransactionReport : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string LBl_Name, FromDate, ToDate;
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
            ExcelBtn.Visible = false;
            ListBox1.Visible = false;
        }
    }
    public void GetCustomeReport()
    {
        try
        {   
            Reply objRes = new Reply();

            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all#all#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text;
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);
               
                string result = client.UploadString(URL + "/GetCustomeTxnReport", "POST", dataEncrypted);

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
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    ExcelBtn.Visible = true;

                    if (DDL_Report_Type.SelectedIndex == 0)
                    {
                        ListBox1.Items.Clear();
                        ListBox1.Visible = true;
                        ListBox1.Items.Add("Error Discription :");
                        ListBox1.Items.Add(" Er01 - Please Contact computer center.");
                        ListBox1.Items.Add(" Er02 - Barcode Read Fails.");
                        ListBox1.Items.Add(" Er03 - Passbook Inserted Wrongly.");
                        ListBox1.Items.Add(" Er04 - Passbook Inserted without barcode sticker.");
                        ListBox1.Items.Add(" Er06 - Middleware service time-out.");
                        ListBox1.Items.Add("  Er07 - Invalid Barcode.");
                        ListBox1.Items.Add("  Er08 - Inactive Barcode.");
                        ListBox1.Items.Add("  Er99 - Default(Generic) error.");
                    }
                    else if (DDL_Report_Type.SelectedIndex == 1)
                    {
                        ListBox1.Items.Clear();
                        ListBox1.Visible = true;
                        ListBox1.Items.Add("Error Discription :");
                        ListBox1.Items.Add(" Er01 - Please Contact computer center.");
                        ListBox1.Items.Add(" Er02 - Barcode Read Fails.");
                        ListBox1.Items.Add(" Er03 - Passbook Inserted Wrongly.");
                        ListBox1.Items.Add(" Er04 - Passbook Inserted without barcode sticker.");
                        ListBox1.Items.Add(" Er06 - Middleware service time-out.");
                        ListBox1.Items.Add("  Er07 - Invalid Barcode.");
                        ListBox1.Items.Add("  Er08 - Inactive Barcode.");
                        ListBox1.Items.Add("  Er99 - Default(Generic) error.");
                    }
                    else if (DDL_Report_Type.SelectedIndex == 2)
                    {
                        ListBox1.Visible = false;
                        ListBox1.Items.Clear();
                    }
                    else if (DDL_Report_Type.SelectedIndex == 3)
                    {
                        ListBox1.Items.Clear();
                        ListBox1.Visible = true;
                        ListBox1.Items.Add("Error Discription :");
                        ListBox1.Items.Add("  0855 - NO TRANSACTIONS FOR SELECTION");
                        ListBox1.Items.Add("  0546 - APPLICATION TIME - OUT, PLEASE ENQUIRE.");
                        ListBox1.Items.Add(" SI002 - Unable to process due to technical error!!");
                        ListBox1.Items.Add("  SI014 - Unable to process due to technical error!!");
                        ListBox1.Items.Add(" SI001 - Unable to process due to technical error!!");
                        ListBox1.Items.Add("  1795 - Invalid Barcode");
                        ListBox1.Items.Add("  0155 - PL CONTACT SERVICE DESK U500 0037");
                        ListBox1.Items.Add("   1796 - Inactive Barcode");
                    }
                    GV_Txn_Details.DataSource = objRes.DS;
                    GV_Txn_Details.DataBind();
                }
                else
                {
                    ((Label)Master.FindControl("lblHeading")).Text = "";
                    lbl_tot.Text = "";
                    PageUtility.MessageBox(this, "Data Not Exist.");
                    GV_Txn_Details.DataSource = null;
                    GV_Txn_Details.DataBind();
                }
            }

            
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message + "");
        }
        finally
        {
            GC.Collect();
        }
    }

    public void GetCustomeReportExcel()
    {
        try
        {
            Reply objRes = new Reply();

            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all#all#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text;
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetCustomeTxnReport", "POST", dataEncrypted);

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

                    //   string   fileName = @"SmartCDK_Report_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".xlsx";
                    GV_Txn_Details.DataSource = objRes.DS;
                    GV_Txn_Details.DataBind();
                    if (GV_Txn_Details.Rows.Count <= 0)
                    {
                        PageUtility.MessageBox(this, "alert : Data Not Exist For Report. ");
                        return;
                    }
                   
                    DateTime fromDT = Convert.ToDateTime(txtFromDate.Text);
                    DateTime toDT = Convert.ToDateTime(txtToDate.Text);

                    // Clear all content output from the buffer stream
                    Response.ClearContent();
                    // Specify the default file name using "content-disposition" RESPONSE header
                    Response.AppendHeader("content-disposition", "attachment; filename=TxnReport(" + DDL_Report_Type.SelectedItem.Text + ")  " + DateTime.Now.ToString("ddMMyy_HHmm") + ".xls");
                    // Set excel as the HTTP MIME type
                    Response.ContentType = "application/excel";
                    // Create an instance of stringWriter for writing information to a string
                    StringWriter stringWriter = new StringWriter();
                    // Create an instance of HtmlTextWriter class for writing markup 
                    // characters and text to an ASP.NET server control output stream
                    HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
                    int ColTot = GV_Txn_Details.Rows[0].Cells.Count;


                    if (DDL_Report_Type.SelectedIndex == 0)
                    {

                        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                             "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + ColTot.ToString() + "'> Error Discription : </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er01 - Please Contact computer center.              </TD><TD COLSPAN='" + (ColTot / 2).ToString() + "'>       Er06 - Middleware service time-out.              </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er02 - Barcode Read Fails.                             </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>  Er07 - Invalid Barcode.                          </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er03 - Passbook Inserted Wrongly.                  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      Er08 - Inactive Barcode.                         </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er04 - Passbook Inserted without barcode sticker.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      Er99 - Default(Generic) error.                   </TD></TR> ");
                    }

                    if (DDL_Report_Type.SelectedIndex == 1)
                    {

                        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                             "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + ColTot.ToString() + "'> Error Discription : </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er01 - Please Contact computer center.  </TD><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er06 - Middleware service time-out.  </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er02 - Barcode Read Fails.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'> Er07 - Invalid Barcode.  </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er03 - Passbook Inserted Wrongly.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'> Er08 - Inactive Barcode.  </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er04 - Passbook Inserted without barcode sticker.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'> Er99 - Default(Generic) error.  </TD></TR> ");
                    }


                    if (DDL_Report_Type.SelectedIndex == 2)
                    {
                        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                             "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> ");
                    }

                    if (DDL_Report_Type.SelectedIndex == 3)
                    {

                        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                             "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                             "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + ColTot.ToString() + "'> Error Discription : </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  0855- NO TRANSACTIONS FOR SELECTION                </TD><TD COLSPAN='" + (ColTot / 2).ToString() + "'>         SI001- Unable to process due to technical error!     </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  0546- APPLICATION TIME-OUT, PLEASE ENQUIRE.        </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      1795- Invalid Barcode                                </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  SI002- Unable to process due to technical error!!      </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>  0155- PL CONTACT SERVICE DESK U500 0037              </TD></TR> " +
                             "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  SI014-Unable to process due to technical error!!   </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      1796-Inactive Barcode                                </TD></TR> ");
                    }

                    GV_Txn_Details.HeaderStyle.ForeColor = Color.White;
                    GV_Txn_Details.HeaderStyle.BackColor = Color.Blue;
                    GV_Txn_Details.HeaderStyle.Font.Bold = true;
                    GV_Txn_Details.Font.Name = "Calibri";
                    GV_Txn_Details.RenderControl(htw);
                    Response.Write(stringWriter.ToString());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                    Response.End();
                    //  lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    //  ExcelBtn.Visible = true;

                    //if (DDL_Report_Type.SelectedIndex == 0)
                    //{
                    //    ListBox1.Items.Clear();
                    //    ListBox1.Visible = true;
                    //    ListBox1.Items.Add("Error Discription :");
                    //    ListBox1.Items.Add(" Er01 - Please Contact computer center.");
                    //    ListBox1.Items.Add(" Er02 - Barcode Read Fails.");
                    //    ListBox1.Items.Add(" Er03 - Passbook Inserted Wrongly.");
                    //    ListBox1.Items.Add(" Er04 - Passbook Inserted without barcode sticker.");
                    //    ListBox1.Items.Add(" Er06 - Middleware service time-out.");
                    //    ListBox1.Items.Add("  Er07 - Invalid Barcode.");
                    //    ListBox1.Items.Add("  Er08 - Inactive Barcode.");
                    //    ListBox1.Items.Add("  Er99 - Default(Generic) error.");
                    //}
                    //else if (DDL_Report_Type.SelectedIndex == 1)
                    //{
                    //    ListBox1.Items.Clear();
                    //    ListBox1.Visible = true;
                    //    ListBox1.Items.Add("Error Discription :");
                    //    ListBox1.Items.Add(" Er01 - Please Contact computer center.");
                    //    ListBox1.Items.Add(" Er02 - Barcode Read Fails.");
                    //    ListBox1.Items.Add(" Er03 - Passbook Inserted Wrongly.");
                    //    ListBox1.Items.Add(" Er04 - Passbook Inserted without barcode sticker.");
                    //    ListBox1.Items.Add(" Er06 - Middleware service time-out.");
                    //    ListBox1.Items.Add("  Er07 - Invalid Barcode.");
                    //    ListBox1.Items.Add("  Er08 - Inactive Barcode.");
                    //    ListBox1.Items.Add("  Er99 - Default(Generic) error.");
                    //}
                    //else if (DDL_Report_Type.SelectedIndex == 2)
                    //{
                    //    ListBox1.Visible = false;
                    //    ListBox1.Items.Clear();
                    //}
                    //else if (DDL_Report_Type.SelectedIndex == 3)
                    //{
                    //    ListBox1.Items.Clear();
                    //    ListBox1.Visible = true;
                    //    ListBox1.Items.Add("Error Discription :");
                    //    ListBox1.Items.Add("  0855 - NO TRANSACTIONS FOR SELECTION");
                    //    ListBox1.Items.Add("  0546 - APPLICATION TIME - OUT, PLEASE ENQUIRE.");
                    //    ListBox1.Items.Add(" SI002 - Unable to process due to technical error!!");
                    //    ListBox1.Items.Add("  SI014 - Unable to process due to technical error!!");
                    //    ListBox1.Items.Add(" SI001 - Unable to process due to technical error!!");
                    //    ListBox1.Items.Add("  1795 - Invalid Barcode");
                    //    ListBox1.Items.Add("  0155 - PL CONTACT SERVICE DESK U500 0037");
                    //    ListBox1.Items.Add("   1796 - Inactive Barcode");
                    //}
                    //GV_Txn_Details.DataSource = objRes.DS;
                    //GV_Txn_Details.DataBind();


                }
                else
                {
                    ((Label)Master.FindControl("lblHeading")).Text = "";
                  //  lbl_tot.Text = "";
                    PageUtility.MessageBox(this, "Data Not Exist.");
                    //GV_Txn_Details.DataSource = null;
                   // GV_Txn_Details.DataBind();
                }
            }


        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message + "");
        }
        finally
        {
            GC.Collect();
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
    protected void BtnGo_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            PageUtility.MessageBox(this, "alert : Please Select Valid Date");
            return;
        }
        DateTime CurrentDate = DateTime.Now;
        DateTime PrevioustDate = DateTime.Now.AddDays(-3);
        if (Convert.ToDateTime(txtFromDate.Text) > CurrentDate)
        {
            PageUtility.MessageBox(this, "alert : Starting date is greater than the Current date. ");
            return;
        }
        if (Convert.ToDateTime(txtToDate.Text) > CurrentDate)
        {
            PageUtility.MessageBox(this, "alert : End date is greater than the Current date. ");
            return;
        }

        double DayDiff = (Convert.ToDateTime(txtToDate.Text) - Convert.ToDateTime(txtFromDate.Text)).TotalDays;
        if (DayDiff > 3)
        {
            PageUtility.MessageBox(this, "alert : End date is greater than the Current date. ");
            return;
        }





        txtBranchCode.Text = "";
        txtSerial.Text = "";
        FromDate = txtFromDate.Text;
        ToDate = txtToDate.Text;
        GetCustomeReport();
    }

    protected void BtnGoExcelDownload_Click(object sender, EventArgs e)
    {
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            PageUtility.MessageBox(this, "alert : Please Select Valid Date");
            return;
        }
        DateTime CurrentDate = DateTime.Now;
        if (Convert.ToDateTime(txtFromDate.Text) > CurrentDate)
        {
            PageUtility.MessageBox(this, "alert : Starting date is greater than the Current date. ");
            return;
        }

      
        txtBranchCode.Text = "";
        txtSerial.Text = "";
        FromDate = txtFromDate.Text;
        ToDate = txtToDate.Text;
        GetCustomeReportExcel();
    }
    public string Excel(DataSet tempDS)
    {
        try
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=CustomizeReport(" + DDL_Report_Type.SelectedItem.Text + ")  " + DateTime.Now.ToString("ddMMyy_HHmm") + ".xls");

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");

            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
              "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> <TR><TD COLSPAN='11' style='background:#FFFF99; font-size:14.0pt; vertical-align:middle; Text-align:Center; height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text + " REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>");

            //column headers
            int columnscount = tempDS.Tables[0].Columns.Count;
            HttpContext.Current.Response.Write("<TR>");
            for (int j = 0; j < columnscount; j++)
            {
                HttpContext.Current.Response.Write("<Td style='background:#0066CC; Text-align:Center; font-size:11.0pt; color:#FFFFFF'>");

                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(tempDS.Tables[0].Columns[j].ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            //string Dt1Time = "";
            foreach (DataRow row in tempDS.Tables[0].Rows)
            {
                //write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < tempDS.Tables[0].Columns.Count; i++)
                {

                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");

                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();


        }
        catch (Exception)
        {

            throw;
        }
        return "A";
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if(GV_Txn_Details.Rows.Count <= 0)
        {
            PageUtility.MessageBox(this, "alert : Data Not Exist For Report. ");
            return;
        }

        DateTime fromDT = Convert.ToDateTime(txtFromDate.Text);
        DateTime toDT = Convert.ToDateTime(txtToDate.Text);

        // Clear all content output from the buffer stream
        Response.ClearContent();
        // Specify the default file name using "content-disposition" RESPONSE header
        Response.AppendHeader("content-disposition", "attachment; filename=TxnReport(" + DDL_Report_Type.SelectedItem.Text + ")  " + DateTime.Now.ToString("ddMMyy_HHmm") + ".xls");
        // Set excel as the HTTP MIME type
        Response.ContentType = "application/excel";
        // Create an instance of stringWriter for writing information to a string
        StringWriter stringWriter = new StringWriter();
        // Create an instance of HtmlTextWriter class for writing markup 
        // characters and text to an ASP.NET server control output stream
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
        int ColTot = GV_Txn_Details.Rows[0].Cells.Count;


        if (DDL_Report_Type.SelectedIndex == 0)
        {

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                 "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + ColTot.ToString() + "'> Error Discription : </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er01 - Please Contact computer center.              </TD><TD COLSPAN='" + (ColTot / 2).ToString() + "'>       Er06 - Middleware service time-out.              </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er02 - Barcode Read Fails.                             </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>  Er07 - Invalid Barcode.                          </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er03 - Passbook Inserted Wrongly.                  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      Er08 - Inactive Barcode.                         </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er04 - Passbook Inserted without barcode sticker.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      Er99 - Default(Generic) error.                   </TD></TR> ");
        }

        if (DDL_Report_Type.SelectedIndex == 1)
        {

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                 "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + ColTot.ToString() + "'> Error Discription : </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er01 - Please Contact computer center.  </TD><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er06 - Middleware service time-out.  </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er02 - Barcode Read Fails.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'> Er07 - Invalid Barcode.  </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er03 - Passbook Inserted Wrongly.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'> Er08 - Inactive Barcode.  </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  Er04 - Passbook Inserted without barcode sticker.  </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'> Er99 - Default(Generic) error.  </TD></TR> ");
        }


        if (DDL_Report_Type.SelectedIndex == 2)
        {
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                 "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> ");
        }

        if (DDL_Report_Type.SelectedIndex == 3)
        {

            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                 "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - " + DDL_Report_Type.SelectedItem.Text.ToUpper() + " GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>" +
                 "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:red; font-size:14.0pt; vertical-align:middle; Text-align:Center;color:#fff; height:35px;'><B>From " + fromDT.ToString("dd-MM-yyyy") + " -  To -" + toDT.ToString("dd-MM-yyyy") + "</B></TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + ColTot.ToString() + "'> Error Discription : </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  0855- NO TRANSACTIONS FOR SELECTION                </TD><TD COLSPAN='" + (ColTot / 2).ToString() + "'>         SI001- Unable to process due to technical error!     </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  0546- APPLICATION TIME-OUT, PLEASE ENQUIRE.        </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      1795- Invalid Barcode                                </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  SI002- Unable to process due to technical error!!      </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>  0155- PL CONTACT SERVICE DESK U500 0037              </TD></TR> " +
                 "<TR style='background:#FFFFFF; font-size:10.0pt; vertical-align:middle; Text-align:left; height:25px;'><TD COLSPAN='" + (ColTot / 2).ToString() + "'>  SI014-Unable to process due to technical error!!   </TD ><TD COLSPAN = '" + (ColTot / 2).ToString() + "'>      1796-Inactive Barcode                                </TD></TR> ");
        }

        GV_Txn_Details.HeaderStyle.ForeColor = Color.White;
        GV_Txn_Details.HeaderStyle.BackColor = Color.Blue;
        GV_Txn_Details.HeaderStyle.Font.Bold = true;
        GV_Txn_Details.Font.Name = "Calibri";
        GV_Txn_Details.RenderControl(htw);
        Response.Write(stringWriter.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
        Response.End();
    }
    
    public void GetCustomeReport_BranchCode(string BranchCode)
    {
        if (txtBranchCode.Text.Length != 6)
        {
            PageUtility.MessageBox(this, "Please enter valid 6 digit branch code with prefix 0.");
            return;
        }

        if(txtFromDate.Text == "")
        {
            PageUtility.MessageBox(this, "Please Select Valid From Date.");
            return;
        }
        
        if (txtToDate.Text == "")
        {
            PageUtility.MessageBox(this, "Please Select Valid To Date.");
            return;
        }

        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {

                    parameter = "all#all#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text + "#" + BranchCode;
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetCustomeTxnReport_BranchCode", "POST", dataEncrypted);  // , "\"" + perameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

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
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    ExcelBtn.Visible = true;
                    GV_Txn_Details.DataSource = objRes.DS;
                    GV_Txn_Details.DataBind();
                }
                else
                {
                    ((Label)Master.FindControl("lblHeading")).Text = "";
                    lbl_tot.Text = "";

                    GV_Txn_Details.DataSource = null;
                    GV_Txn_Details.DataBind();
                    PageUtility.MessageBox(this, "Details Not Found For Specified Branch Code.");
                }

            }

            
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message + "");           
        }
        finally
        {
            GC.Collect();
        }
    }

    public void GetCustomeReport_SerialCode(string SerialCode)
    {
        if (txtFromDate.Text == "")
        {
            PageUtility.MessageBox(this, "Please Select Valid From Date.");
            return;
        }
        else if (txtToDate.Text == "")
        {
            PageUtility.MessageBox(this, "Please Select Valid To Date.");
            return;
        }
        try
        {
            Reply objRes = new Reply();
            // send request
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all#all#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text + "#" + SerialCode;
                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString() + "#" + DDL_Report_Type.SelectedValue.ToString() + "#" + txtFromDate.Text + "#" + txtToDate.Text + "#" + SerialCode;
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);
                string result = client.UploadString(URL + "/GetCustomeTxnReport_SerialCode", "POST", dataEncrypted);

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
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    ExcelBtn.Visible = true;
                    GV_Txn_Details.DataSource = objRes.DS;
                    GV_Txn_Details.DataBind();
                }
                else
                {
                    lbl_tot.Text = "";
                    PageUtility.MessageBox(this, "Details Not Found For Specified Serial Number"); // Response.Write("<script type='text/javascript'>alert( '"+objRes.strError+"' )</script>");
                }

            }

            
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "catch error : " + ex.Message + "");
        }
        finally
        {
            GC.Collect();
        }
    }
    protected void btn_Search_by_branch_code_Click(object sender, EventArgs e)
    {
        GetCustomeReport_BranchCode(txtBranchCode.Text.Trim());
    }
    protected void btn_Search_by_serial_no_Click(object sender, EventArgs e)
    {
        GetCustomeReport_SerialCode(txtSerial.Text.Trim());
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
}