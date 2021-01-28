using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
public partial class PatchUpdate : System.Web.UI.Page
{
    public string URL = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
    public string KioskHealthjson;
    public DataSet objDataSet;
    public string strconnected, strdisconnected;
    public int Total_Kiosk;
    public string State_List_with_IP;
    public static string App_Version;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblHeading")).Text = "Reports";
            bindstatelist();
        }
    }
    private void bindstatelist()
    {
        if (objDataSet == null)
            objDataSet = new DataSet();

        Reply objRes = new Reply();
        using (WebClient client = new WebClient())
        {
            client.Headers[HttpRequestHeader.ContentType] = "text/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string JsonString = JsonConvert.SerializeObject("");
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

            if (objRes.res)
            {
                filtercategories1.DataSource = objRes.DS;
                filtercategories1.DataTextField = "state";
                filtercategories1.DataBind();
                filtercategories1.Items.Insert(0, new ListItem("Select Circle", "NA"));
                errorlabel.Style["display"] = "block";
                kiosk_data.Text = "";
                kiosklist.Style["display"] = "none";
            }
            else
            {
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('Some Error Occurred');windows.location.href='Dashboard-V3_1.aspx'", true);
            }
        }
    }
    public void bindKioskHealth(string state)
    {
        try
        {
            if (objDataSet == null)
                objDataSet = new DataSet();

            Reply objRes = new Reply();

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "text/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JsonString = JsonConvert.SerializeObject(state);
                EncRequest objEncRequest = new EncRequest();
                objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);


                string result = client.UploadString(URL + "/PatchMachineList", "POST", dataEncrypted);

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
                    for (int iStateIndex = 0; iStateIndex < objRes.DS.Tables[0].Rows.Count; iStateIndex++)
                    {
                        State_List_with_IP += "<tr>" +
                                              "<td class=''>" +
                                              " <label class='option block mn'>" +
                                              " <input type = 'checkbox' name= 'inputname' value= 'FR'> " +
                                              " <span class='checkbox mn'></span> " +
                                              "  </label> " +
                                              " </td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][0].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][1].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][2].ToString() + "</td> " +
                                               " <td > " +
                                              " <span>" + objRes.DS.Tables[0].Rows[iStateIndex][4].ToString() + "</span> " +
                                              " </td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][5].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][6].ToString() + "</td> " +
                                              " <td class=''>" + objRes.DS.Tables[0].Rows[iStateIndex][7].ToString() + "</td> " +
                                              " </tr>";
                    }

                    kiosk_data.Text = State_List_with_IP;
                    errorlabel.Style["display"] = "none";
                    kiosklist.Style["display"] = "block";
                    SearchText.Value = "";
                    string[] strArWinSeries = new string[0];
                    string[] strArLinSeries = new string[0];
                    int[] ptsArWindows = new int[0];
                    int[] ptsArLinux = new int[0];
                    for (int iIterator = 0; iIterator < objRes.DS.Tables[1].Rows.Count; iIterator++)
                    {
                        if (objRes.DS.Tables[1].Rows[iIterator]["OS_Type"].ToString().ToLower().Contains("windows"))
                        {
                            Array.Resize(ref strArWinSeries, strArWinSeries.Length + 1);
                            Array.Resize(ref ptsArWindows, ptsArWindows.Length + 1);

                            strArWinSeries[strArWinSeries.Length - 1] = objRes.DS.Tables[1].Rows[iIterator]["rms_client_version"].ToString();
                            ptsArWindows[ptsArWindows.Length - 1] = Convert.ToInt32(objRes.DS.Tables[1].Rows[iIterator]["Machines_Count"].ToString());
                        }
                        else if (objRes.DS.Tables[1].Rows[iIterator]["OS_Type"].ToString().ToLower().Contains("linux"))
                        {
                            Array.Resize(ref strArLinSeries, strArLinSeries.Length + 1);
                            Array.Resize(ref ptsArLinux, ptsArLinux.Length + 1);

                            strArLinSeries[strArLinSeries.Length - 1] = objRes.DS.Tables[1].Rows[iIterator]["rms_client_version"].ToString();
                            ptsArLinux[ptsArLinux.Length - 1] = Convert.ToInt32(objRes.DS.Tables[1].Rows[iIterator]["Machines_Count"].ToString());
                        }
                    }

                    WindowsChart.Series["Series1"].Points.DataBindXY(strArWinSeries, ptsArWindows);
                    WindowsChart.Legends.Add("Legend1");
                    WindowsChart.Legends[0].Enabled = true;
                    WindowsChart.Legends[0].Docking = Docking.Bottom;
                    WindowsChart.Series[0].LegendText = "#VALX (#VALY)"; //#PERCENT)"; for percent
                    WindowsChart.Series["Series1"]["PieLabelStyle"] = "Disabled";
                    LinuxChart.Series["Series1"].Points.DataBindXY(strArLinSeries, ptsArLinux);
                    LinuxChart.Legends.Add("Legend1");
                    LinuxChart.Legends[0].Enabled = true;
                    LinuxChart.Legends[0].Docking = Docking.Bottom;
                    LinuxChart.Series[0].LegendText = "#VALX (#VALY)"; //#PERCENT)"; for percent
                    LinuxChart.Series["Series1"]["PieLabelStyle"] = "Disabled";
                    WindowsChart.Visible = true;
                    LinuxChart.Visible = true;
                }
                else
                {
                    var page = HttpContext.Current.CurrentHandler as Page;
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('" + objRes.strError + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('catch error : " + ex.Message + "');", true);
        }
    }
    protected void UploadBtn_Click(object sender, EventArgs e)
    {         
        if (!file5.HasFile)
        {
            SearchText.Value = "";
            bindKioskHealth(filtercategories1.SelectedItem.Text);
            uploader5.Text = "";
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Please Upload file');", true);
            return;
        }
        else
        {
            uploader5.Text = "";
            string ipList = hiddenItem.Value;
            hiddenItem.Value = "";
            string[] ipArray = ipList.Split('#');
            if (ipArray.Length == 0)
            {
                SearchText.Value = "";
                bindKioskHealth(filtercategories1.SelectedItem.Text);
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Select Atleast One Kiosk');", true);
                return;
            }

            if (".zip".Contains(Path.GetExtension(file5.FileName).ToLower()))
            {
                if(File.Exists(Server.MapPath(file5.FileName)))
                {
                    File.Delete(Server.MapPath(file5.FileName));
                }

                File.WriteAllBytes(Server.MapPath(file5.FileName), file5.FileBytes);

                if(ZipFile.IsZipFile(Server.MapPath(file5.FileName)))
                {
                    //To Check Password Exits or not  //Lokesh Added[20 Jan 2013]
                    //using (ZipFile zp_patch1 = ZipFile.Read(Server.MapPath(file5.FileName)))
                    //{
                    //    foreach (var e1 in zp_patch1)
                    //    {
                    //        if (e1.IsDirectory == false && e1.UsesEncryption == false)
                    //        {
                    //            var page = HttpContext.Current.CurrentHandler as Page;
                    //            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Invalid patch file');", true);
                    //            return;
                    //        }
                    //    }
                    //}
                    //End

                    //check if zip password is the default(admin) password
                    //if (ZipFile.CheckZipPassword(Server.MapPath(file5.FileName), "lipi@data@systems@df"))
                    //{
                        Array.Resize(ref ipArray, ipArray.Length - 1);
                        ReqCommandExecute reqCommandExecute = new ReqCommandExecute();
                        reqCommandExecute.KioskIPs = ipArray;
                        reqCommandExecute.Command = "patchupdate#" + file5.FileName;
                        reqCommandExecute.DisplayData = "Patch : " + file5.FileName;
                        byte[] data = file5.FileBytes;

                        reqCommandExecute.Patchdata = Convert.ToBase64String(data);
                        using (WebClient client = new WebClient())
                        {
                            string JsonString = JsonConvert.SerializeObject(reqCommandExecute);

                            EncRequest objEncRequest = new EncRequest();
                            objEncRequest.RequestData = AesGcm256.Encrypt(JsonString);
                            string dataEncrypted = JsonConvert.SerializeObject(objEncRequest);

                            client.Headers[HttpRequestHeader.ContentType] = "text/json";
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            string result1 = client.UploadString(URL + "/CommandExecute", "POST", dataEncrypted);

                            EncResponse objResponse = JsonConvert.DeserializeObject<EncResponse>(result1);
                            objResponse.ResponseData = AesGcm256.Decrypt(objResponse.ResponseData);

                            //objRes = JsonConvert.DeserializeObject<Reply>(objResponse.ResponseData);
                            //DataContractJsonSerializer objDCS = new DataContractJsonSerializer(typeof(Reply));
                            //MemoryStream objMS = new MemoryStream(Encoding.UTF8.GetBytes(objResponse.ResponseData));
                            //ResCommandExecute objRes = (ResCommandExecute)objDCS.ReadObject(objMS);

                            Newtonsoft.Json.JsonSerializer json = new Newtonsoft.Json.JsonSerializer();
                            json.NullValueHandling = NullValueHandling.Ignore;
                            StringReader sr = new StringReader(objResponse.ResponseData);
                            Newtonsoft.Json.JsonTextReader reader = new JsonTextReader(sr);
                            ResCommandExecute objRes = json.Deserialize<ResCommandExecute>(reader);

                            if (objRes.Result)
                            {
                                SearchText.Value = "";
                                var page = HttpContext.Current.CurrentHandler as Page;
                                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Patch Updation Requested SuccessFully');", true);
                                bindKioskHealth(filtercategories1.SelectedItem.Text);
                            }
                            else
                            {
                                SearchText.Value = "";
                                var page = HttpContext.Current.CurrentHandler as Page;
                                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Patch Updation Request Failed');", true);
                                bindKioskHealth(filtercategories1.SelectedItem.Text);
                            }
                        }
                    //}
                    //else
                    //{
                    //    var page = HttpContext.Current.CurrentHandler as Page;
                    //    ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Invalid patch file');", true);
                    //    return;
                    //}

                }
                else
                {
                    var page = HttpContext.Current.CurrentHandler as Page;
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Invalid patch file');", true);
                    return;
                }


                
            }
            else
            {
                SearchText.Value = "";
                bindKioskHealth(filtercategories1.SelectedItem.Text);
                var page = HttpContext.Current.CurrentHandler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "errorDisplay('Upload Valid Patch File');", true);
                return;
            }
        }
    }
    protected void WindowsChart_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            bindKioskHealth(filtercategories1.SelectedItem.Text);
            SearchText.Value = e.PostBackValue;
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "Chart_Click('" + e.PostBackValue + "');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
    }

    protected void LinuxChart_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            bindKioskHealth(filtercategories1.SelectedItem.Text);
            SearchText.Value = e.PostBackValue;
            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "Chart_Click('" + e.PostBackValue + "');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message;
        }
    }

    protected void filtercategories1_SelectedIndexChanged(object sender, EventArgs e)
    {
        uploader5.Text = "";
        if (filtercategories1.SelectedIndex == 0)
        {
            kiosk_data.Text = "";
            kiosklist.Style["display"] = "none";
            errorlabel.Style["display"] = "block";
            WindowsChart.Visible = false;
            LinuxChart.Visible = false;
        }
        else
        {
            bindKioskHealth(filtercategories1.SelectedItem.Text);
        }
    }

    protected void btn_refresh_Click(object sender, EventArgs e)
    {
        bindKioskHealth(filtercategories1.SelectedItem.Text);
    }
}