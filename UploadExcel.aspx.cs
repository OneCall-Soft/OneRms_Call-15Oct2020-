using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UploadExcel : System.Web.UI.Page
{

    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();


    public string perameter;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            ((Label)Master.FindControl("lblHeading")).Text = "Upload Excel Report";



        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {  //Coneection String by default empty  
        string ConStr = "";
        ResUpdateExcelUpdateStatus objResResUpdateExcelUpdateStatus = new ResUpdateExcelUpdateStatus();
        DataSet objDsExcel = new DataSet();
        //Extantion of the file upload control saving into ext because   
        //there are two types of extation .xls and .xlsx of Excel   
        string ext = Path.GetExtension(BtnExcelUpload.FileName).ToLower();
        //getting the path of the file   
        string path = Server.MapPath("~/MyFolder/" + BtnExcelUpload.FileName);
        if (!path.Contains(".xls"))
        {
            Response.Write("<script type='text/javascript'>alert( 'Please select excel file XLS format.' )</script>");
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

        conn.Close();

        if (File.Exists(path))
            File.Delete(path);
        ///////////////////////////////////////
        Reply objRes = new Reply();
        // send request
        using (WebClient client = new WebClient())
        {
            if (Session["Role"].ToString() == "admin")
            {
                perameter = "all# #";

            }
            else
            {
                perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();
            }
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(string));

            MemoryStream memStrToSend = new MemoryStream();

            objJsonSerSend.WriteObject(memStrToSend, perameter);

            // string data = Encoding.Default.GetString(memStrToSend.ToArray());

            string result = client.UploadString(URL + "/GetAllTicketData", "POST", "\"" + perameter + "\"");  // , "\"" + perameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

            MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

            DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(Reply));

            objRes = (Reply)objJsonSerRecv.ReadObject(memstrToReceive);
        }

        if (objRes.res == true)
        {

            //DataSet DT = new DataSet();
            //DT = objDsExcel;
            //var ExcelRows = DT.Tables[0].AsEnumerable().Select(r => Convert.ToString(r.Field<Double>("Call Ticket Number")));
            //var AllTicketRows = objRes.DS.Tables[0].AsEnumerable().Select(r => r.Field<string>("TicketNUmber"));
            //IEnumerable<string> allIntersectingTicketNumber = ExcelRows.Intersect(AllTicketRows);
            //if (allIntersectingTicketNumber.Any())
            //{
            //    string firstIntersectingEmpName = allIntersectingTicketNumber.FirstOrDefault();
            //    if (firstIntersectingEmpName != null)
            //    {
            //        foreach (string empName in allIntersectingTicketNumber)
            //        {

            //        }
            //        //  yes, there was at least one EmpName that was in both tables
            //    }

            //}

            string strQuery = "";
            int TempCounter = 0;
            for (int i = 0; i < objDsExcel.Tables[0].Rows.Count; i++)
            {

                for (int index = 0; index < objRes.DS.Tables[0].Rows.Count; index++)
                {
                    if (objDsExcel.Tables[0].Rows[i]["Call Ticket Number"].ToString() != "" &&
                        objDsExcel.Tables[0].Rows[i]["Date Ticket Closed"].ToString() != "")
                    {
                        if (objDsExcel.Tables[0].Rows[i]["Call Ticket Number"].ToString().Trim() == objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString().Trim())
                        {
                            DateTime ExcelDT = Convert.ToDateTime(objDsExcel.Tables[0].Rows[i]["Date Ticket Closed"]);
                         //   objSelectedFromDate = DateTime.ParseExact(ExcelDT, "dd-mm-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            if (objRes.DS.Tables[0].Rows[index]["FlmSlm"].ToString() == "" || objRes.DS.Tables[0].Rows[index]["FlmSlm"].ToString() == null)
                            {
                                if (TempCounter != 0)
                                    strQuery += " UNION ALL";

                                strQuery += " update [KMSDB].[dbo].[IssueTable] set FlmSlm='" + objDsExcel.Tables[0].Rows[index]["Flm Slm"].ToString() + "' where " +
                                  " TicketNUmber='" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' ; ";

                            }
                            if (objRes.DS.Tables[0].Rows[index]["CallClosedDT"].ToString() == "" || objRes.DS.Tables[0].Rows[index]["CallClosedDT"].ToString() == null)
                            {
                                if (TempCounter != 0)
                                    strQuery += " UNION ALL";

                                strQuery += " update [KMSDB].[dbo].[IssueTable] set Status = 'Call Closed' , CallClosedDT ='" + ExcelDT.ToString("yyyy-MM-dd HH:mm:ss") + "',FlmSlm='" + objDsExcel.Tables[0].Rows[index]["Flm Slm"].ToString() + "' where " +
                                  " TicketNUmber='" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' ; ";

                                continue;
                            }

                            DateTime DBDT = Convert.ToDateTime(objRes.DS.Tables[0].Rows[index]["CallClosedDT"]);

                            if (ExcelDT< DBDT)
                            {
                                if (TempCounter != 0)
                                    strQuery += " UNION ALL";


                                strQuery += " update [KMSDB].[dbo].[IssueTable] set Status = 'Call Closed' ,  CallClosedDT='" + ExcelDT.ToString("yyyy-MM-dd HH:mm:ss") + "',FlmSlm='" + objDsExcel.Tables[0].Rows[index]["Flm Slm"].ToString() + "' where " +
                                    " TicketNUmber='" + objRes.DS.Tables[0].Rows[index]["TicketNUmber"].ToString() + "' ; ";
                            }
                        }
                    }
                }
            }
            ///////////////////////////////////////          
            // send request
            if (strQuery != "")
            {


                using (WebClient client = new WebClient())
                {
                    if (Session["Role"].ToString() == "admin")
                    {
                        perameter = "all# #";

                    }
                    else
                    {

                        perameter = Session["Role"].ToString() + "#" + Session["Location"].ToString();

                    }
                    client.Headers[HttpRequestHeader.ContentType] = "text/json";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                    ReqUpdateExcelUpdateStatus objReqUpdateExcelUpdateStatus = new ReqUpdateExcelUpdateStatus();
                    objReqUpdateExcelUpdateStatus.strQuery = strQuery;
                    DataContractJsonSerializer objJsonSerSend = new DataContractJsonSerializer(typeof(ReqUpdateExcelUpdateStatus));

                    MemoryStream memStrToSend = new MemoryStream();

                    objJsonSerSend.WriteObject(memStrToSend, objReqUpdateExcelUpdateStatus);

                    string data = Encoding.Default.GetString(memStrToSend.ToArray());

                    string result = client.UploadString(URL + "/UpdateExcelData", "POST", data);  // , "\"" + perameter + "\"" |   GetDashBoardDetail |GetHealthTxnDataWithState

                    MemoryStream memstrToReceive = new MemoryStream(Encoding.UTF8.GetBytes(result));

                    DataContractJsonSerializer objJsonSerRecv = new DataContractJsonSerializer(typeof(ResUpdateExcelUpdateStatus));

                    objResResUpdateExcelUpdateStatus = (ResUpdateExcelUpdateStatus)objJsonSerRecv.ReadObject(memstrToReceive);


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

      

    //  gvExcelFile.DataSource = ds.Tables[0];

    //  gvExcelFile.DataBind();



    }
}