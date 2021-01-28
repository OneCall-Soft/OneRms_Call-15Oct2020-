using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using Newtonsoft.Json;

public partial class TicketCenter : System.Web.UI.Page
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
            if (Session["Role"].ToString().ToLower().Contains("admin"))
            {
                UploadExcel.Visible = true;
            }
            else
            {
                UploadExcel.Visible = false;
            }

            ((Label)Master.FindControl("lblHeading")).Text = "Reports";           
            browseLHO();
            GetTicketDetails();
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            //Coneection String by default empty  
            string ConStr = "";
            ResUpdateExcelUpdateStatus objResResUpdateExcelUpdateStatus = new ResUpdateExcelUpdateStatus();
            DataSet objDsExcel = new DataSet();
            //Extantion of the file upload control saving into ext because   
            //there are two types of extation .xls and .xlsx of Excel   
            string ext = Path.GetExtension(BtnExcelUpload.FileName).ToLower();

            //getting the path of the file   
            string path = Server.MapPath("MyFolder/" + BtnExcelUpload.FileName);

            if (!path.Contains(".xls"))
            {
                Response.Write("<script type='text/javascript'>alert( 'Please select excel file XLS format.' )</script>");
                return;
            }
            if (File.Exists(path))
            {
                Response.Write("<script type='text/javascript'>alert( 'File Name Already Exists ! Please Modify file name. (ex: Add current date time after file name . )' )</script>");
                return;
            }
            //saving the file inside the MyFolder of the server  
            BtnExcelUpload.SaveAs(path);
            //  Label1.Text = FileUpload1.FileName + "\'s Data showing into the GridView";
            //checking that extantion is .xls or .xlsx  
            if (ext.Trim() == ".xls")
            {
                //connection string for that file which extantion is .xls  
                ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (ext.Trim() == ".xlsx")
            {
                //connection string for that file which extantion is .xlsx  
                ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            //making query  
            string query = "SELECT * FROM [Sheet1$]";
            //Providing connection  
            OleDbConnection conn = new OleDbConnection(ConStr);
            //checking that connection state is closed or not if closed the   
            //open the connection  
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //create command object  
            OleDbCommand cmd = new OleDbCommand(query, conn);
            // create a data adapter and get the data into dataadapter  
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            //fill the Excel data to data set  
            da.Fill(objDsExcel);



            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
       
            Reply objRes = new Reply();
           
            using (WebClient client = new WebClient())
            {
                if (Session["Role"].ToString().ToLower().Contains("admin"))
                {
                    parameter = "all# #";

                }
                else
                {
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();
                }
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetAllTicketData", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

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

            }

            if (objRes.res == true)
            {
                string strQuery = "";
                int TempCounter = 0;
                for (int i = 0; i < objDsExcel.Tables[0].Rows.Count; i++)
                {
                    for (int index = 0; index < objRes.DS.Tables[0].Rows.Count; index++)
                    {
                        if (objDsExcel.Tables[0].Rows[i]["Call Ticket Number"].ToString() != "" &&
                            objDsExcel.Tables[0].Rows[i]["Date Ticket Closed"].ToString() != "")
                        {
                            if (objDsExcel.Tables[0].Rows[i]["Call Ticket Number"].ToString().Trim() ==
                                objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString().Trim())
                            {
                                DateTime ExcelDT = Convert.ToDateTime(objDsExcel.Tables[0].Rows[i]["Date Ticket Closed"]);
                                //   objSelectedFromDate = DateTime.ParseExact(ExcelDT, "dd-mm-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                                if (objRes.DS.Tables[0].Rows[index]["FlmSlm"].ToString() == "" || objRes.DS.Tables[0].Rows[index]["FlmSlm"].ToString() == null)
                                {
                                    if (TempCounter != 0)
                                        strQuery += " UNION ALL";

                                    strQuery += " update [KMSDB].[dbo].[IssueTable] set FlmSlm='" + objDsExcel.Tables[0].Rows[i]["Flm Slm"].ToString() + "' where " +
                                      " TicketNUmber='" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' ; ";

                                }
                                if (objRes.DS.Tables[0].Rows[index]["CallClosedDT"].ToString() == "" || objRes.DS.Tables[0].Rows[index]["CallClosedDT"].ToString() == null)
                                {
                                    if (TempCounter != 0)
                                        strQuery += " UNION ALL";

                                    strQuery += " update [KMSDB].[dbo].[IssueTable] set Status = 'Call Closed' , CallClosedDT ='" + ExcelDT.ToString("yyyy-MM-dd HH:mm:ss") + "',FlmSlm='" + objDsExcel.Tables[0].Rows[i]["Flm Slm"].ToString() + "' where " +
                                      " TicketNUmber='" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' ;     ";


                                    strQuery += "  update [KMSDB].[dbo].[HEALTH] set [Scount] = 0 , Call_Log_Status = null , [FK_Issue_ID] = null " +
                                        " where FK_Issue_ID = (SELECT IssueId from [KMSDB].[dbo].[IssueTable] where TicketNUmber = '" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' );";

                                    continue;
                                }

                                DateTime DBDT = Convert.ToDateTime(objRes.DS.Tables[0].Rows[index]["CallClosedDT"]);

                                if (ExcelDT < DBDT)
                                {
                                    if (TempCounter != 0)
                                        strQuery += " UNION ALL";


                                    strQuery += " update [KMSDB].[dbo].[IssueTable] set Status = 'Call Closed' ,  CallClosedDT='" + ExcelDT.ToString("yyyy-MM-dd HH:mm:ss") + "',FlmSlm='" + objDsExcel.Tables[0].Rows[i]["Flm Slm"].ToString() + "' where " +
                                        " TicketNUmber='" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' ; ";

                                    strQuery += "  update [KMSDB].[dbo].[HEALTH] set [Scount] = 0 , Call_Log_Status = null , [FK_Issue_ID] = null " +
                                      " where FK_Issue_ID = (SELECT IssueId from [KMSDB].[dbo].[IssueTable] where TicketNUmber = '" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' );";



                                }
                            }
                        }
                    }
                }
                /////          
                // send request
                if (strQuery != "")
                {
                    Response.Write("<script type='text/javascript'>alert( 'Query is : " + strQuery + " ' )</script>");

                    using (WebClient client = new WebClient())
                    {
                        if (Session["Role"].ToString().ToLower().Contains("admin"))
                        {
                            parameter = "all# #";

                        }
                        else
                        {

                            parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();

                        }
                        client.Headers[HttpRequestHeader.ContentType] = "text/json";

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        ReqUpdateExcelUpdateStatus objReqUpdateExcelUpdateStatus = new ReqUpdateExcelUpdateStatus();
                        objReqUpdateExcelUpdateStatus.strQuery = strQuery;

                        string JsonString = JsonConvert.SerializeObject(objReqUpdateExcelUpdateStatus);
                        EncRequest objEncRequest = new EncRequest();
                        objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                        string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);
                        
                        string result = client.UploadString(URL + "/UpdateExcelData", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

                        EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result);
                        objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                        //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                        //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                        //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                        //objResResUpdateExcelUpdateStatus = (ResUpdateExcelUpdateStatus)objDCS.ReadObject(objMS);
                        Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                        json.NullValueHandling = NullValueHandling.Ignore;
                        StringReader sr = new StringReader(objResponse.ResponseData);
                        Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                        objResResUpdateExcelUpdateStatus = json.Deserialize<ResUpdateExcelUpdateStatus>(reader);

                        if (objResResUpdateExcelUpdateStatus.Result == true)
                        {
                            Response.Write("<script type='text/javascript'>alert( 'Excel Data Uploaded Sucessfully.' )</script>");
                        }
                        else
                        {
                            Response.Write("<script type='text/javascript'>alert( 'Failed to upload.' )</script>");
                        }
                    }
                }
                else
                {
                    Response.Write("<script type='text/javascript'>alert( 'No Record found for upload.' )</script>");
                }

            }
            else
            {
                Response.Write("<script type='text/javascript'>alert( 'Data Not Exist.' )</script>");
            }
        }
        catch (Exception ex)
        {
            PageUtility.MessageBox(this, "Catch Error : " + ex.Message);
        }
        finally
        {

            GetTicketDetails();

            GC.Collect();
        }

    }

    public void GetTicketDetails()
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
                    parameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();
                }

                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(parameter);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                string result = client.UploadString(URL + "/GetTicketsReport", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

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
                    lbl_tot.Text = "Total Records are : " + objRes.DS.Tables[0].Rows.Count.ToString();
                    GV_Ticket_Details.DataSource = objRes.DS.Tables[0];
                    GV_Ticket_Details.DataBind();

                    lbl_1h.Text = objRes.DS.Tables[1].Rows[0][0].ToString();
                    lbl_2h.Text = objRes.DS.Tables[1].Rows[1][0].ToString();
                    lbl_4h.Text = objRes.DS.Tables[1].Rows[2][0].ToString();
                    lbl_8h.Text = objRes.DS.Tables[1].Rows[3][0].ToString();
                    lbl_1D.Text = objRes.DS.Tables[1].Rows[4][0].ToString();
                    lbl_3D.Text = objRes.DS.Tables[1].Rows[5][0].ToString();
                    lbl_5D.Text = objRes.DS.Tables[1].Rows[6][0].ToString();
                    lbl_1W.Text = objRes.DS.Tables[1].Rows[7][0].ToString();
                    lbl_Total.Text = objRes.DS.Tables[1].Rows[8][0].ToString();
                }
                else
                {
                    lbl_tot.Text = "";
                    Response.Write("<script type='text/javascript'>alert( 'Data Not Exist.' )</script>");
                }


            }



        }
        catch (Exception ex)
        {
            lbl_tot.Text = "";
            Response.Write("<script type='text/javascript'>alert( 'catch error : " + ex.Message + "' )</script>");
        }

        finally { GC.Collect(); }
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

                string result = client.UploadString(URL + "/GetLHOList", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

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
                        DDL_LHO.Items.Add(objRes.DS.Tables[0].Rows[i][0].ToString());
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
                    parameter = "all# #" + DDL_LHO.SelectedItem.Text;

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
                
                string result = client.UploadString(URL + "/GetBranchList", "POST", dataEncrypted);  // , "\"" + parameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

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
                        DDL_Branch.Items.Add(objRes.DS.Tables[0].Rows[i][0].ToString());
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
            PageUtility.MessageBox(this, "Catch error : " + ex.Message + " ");
        }

        finally { GC.Collect(); }
    }
    
   
    protected void DDL_LHO_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDL_Branch.Items.Clear();

        browseBranch();

    }

  
    public static class PageUtility
    {
        public static void MessageBox(System.Web.UI.Page page, string strMsg)
        {
            //+ character added after strMsg "')"
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);

        }
    }

    protected void btn_Download_Click(object sender, EventArgs e)
    {
        // Clear all content output from the buffer stream
        Response.ClearContent();
        // Specify the default file name using "content-disposition" RESPONSE header
        Response.AppendHeader("content-disposition", "attachment; filename=PbTicketReport " + DateTime.Now.ToString("ddMMyy_HHmm") + ".xls");
        // Set excel as the HTTP MIME type
        Response.ContentType = "application/excel";
        // Create an instance of stringWriter for writing information to a string
        StringWriter stringWriter = new StringWriter();
        // Create an instance of HtmlTextWriter class for writing markup 
        // characters and text to an ASP.NET server control output stream
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);

        int ColTot = GV_Ticket_Details.Rows[0].Cells.Count;

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                "<TR><TD COLSPAN='" + ColTot.ToString() + "' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - TICKET REPORT GENERATED ON " + DateTime.Now.ToString("dd MMM yyyy, hh:mm tt") + "</B></TD></TR>");

        GV_Ticket_Details.HeaderStyle.ForeColor = Color.White;
        GV_Ticket_Details.HeaderStyle.BackColor = Color.Blue;
        GV_Ticket_Details.HeaderStyle.Font.Bold = true;
        GV_Ticket_Details.Font.Name = "Calibri";

        GV_Ticket_Details.RenderControl(htw);


        Response.Write(stringWriter.ToString());

        HttpContext.Current.Response.Flush();

        HttpContext.Current.Response.End();

        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
}