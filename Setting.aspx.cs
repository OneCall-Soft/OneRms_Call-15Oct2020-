using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;

public partial class Setting : System.Web.UI.Page
{
    public string path = "";
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
            LoadSettingData();
        }


    }

    public void LoadSettingData()
    {
        try
        {
            #region load JSON

            path = HttpContext.Current.Server.MapPath(@"~/setting.json");



            using (StreamReader r = new StreamReader(path))
            {
                var json = r.ReadToEnd();

                var items = JsonConvert.DeserializeObject<setting>(json);


                bool[] Admin = new bool[] { items.Admin.Printed_Characters_Full ,
                                            items.Admin.Printed_Characters_Partial,
                                            items.Admin.Line_Feeds,
                                            items.Admin.Documents_Insertions,
                                            items.Admin.Cover_Openings,
                                            items.Admin.Paper_Jams,
                                            items.Admin.Front_Scannings,
                                            items.Admin.Back_Scannings,
                                            items.Admin.Power_on_hours,
                                            items.Admin.Standby_hours,
                                            items.Admin.Power_on_cycles

                };

                bool[] Circle = new bool[] {items.CircleUser.Printed_Characters_Full ,
                                            items.CircleUser.Printed_Characters_Partial,
                                            items.CircleUser.Line_Feeds,
                                            items.CircleUser.Documents_Insertions,
                                            items.CircleUser.Cover_Openings,
                                            items.CircleUser.Paper_Jams,
                                            items.CircleUser.Front_Scannings,
                                            items.CircleUser.Back_Scannings,
                                            items.CircleUser.Power_on_hours,
                                            items.CircleUser.Standby_hours,
                                            items.CircleUser.Power_on_cycles

                };

                bool[] Branch = new bool[] {items.BranchUser.Printed_Characters_Full ,
                                            items.BranchUser.Printed_Characters_Partial,
                                            items.BranchUser.Line_Feeds,
                                            items.BranchUser.Documents_Insertions,
                                            items.BranchUser.Cover_Openings,
                                            items.BranchUser.Paper_Jams,
                                            items.BranchUser.Front_Scannings,
                                            items.BranchUser.Back_Scannings,
                                            items.BranchUser.Power_on_hours,
                                            items.BranchUser.Standby_hours,
                                            items.BranchUser.Power_on_cycles


                };

                StringBuilder Setting_List = new StringBuilder();

                string[] SettingKey = new string[] { "Printed Characters(Full)", "Printed Characters(Partial)",
                                                    "Line Feeds" , "Documents Insertions","Cover Openings",
                                                    "Paper Jams","Front Scannings","Back Scannings","Power on hours",
                                                    "Standby hours","Power on cycles"};



                for (int i = 0; i < SettingKey.Length; i++)
                {
                    Setting_List.Append("<li class='list-group-item' >   " +
                       "            <span class='badge mr30'>                                                                                                   " +
                       "                <div class='flipswitch switch-info-light switch-inline-table'>                                                       ");


                    if (Branch[i] == true)
                    {
                        Setting_List.Append(" <input type = 'checkbox' runat='server' checked='true' name='flipswitch' class='flipswitch-cb'  id='VAL_Branch_" + i.ToString() + "'/>");
                    }
                    else
                    {
                        Setting_List.Append("<input type = 'checkbox' runat='server' name='flipswitch' class='flipswitch-cb' id='VAL_Branch_" + i.ToString() + "'/>");
                    }



                    Setting_List.Append("                            <label class='flipswitch-label flipswitch-checked' for='VAL_Branch_" + i.ToString() + "'>                                          " +
                       "                                <div class='flipswitch-inner no-yes'></div>                                                          " +
                       "                                <div class='flipswitch-switch'></div>                                                                " +
                       "                            </label>                                                                                                 " +
                       "                        </div>                                                                                                       " +
                       "                                                                                                                                     " +
                       "            </span>                                                                                                                  " +
                       "                                                                                                                                     " +
                       "             <span class='badge mr30'>                                                                                                   " +
                       "                <div class='flipswitch switch-info-light switch-inline-table'>                                                       ");


                    if (Circle[i] == true)
                    {
                        Setting_List.Append(" <input type = 'checkbox' runat='server' checked='true' name='flipswitch' class='flipswitch-cb' id='VAL_Circle_" + i.ToString() + "'/>");
                    }
                    else
                    {
                        Setting_List.Append("<input type = 'checkbox' runat='server'  name='flipswitch' class='flipswitch-cb' id='VAL_Circle_" + i.ToString() + "'/>");
                    }

                    Setting_List.Append("                            <label class='flipswitch-label flipswitch-checked' for='VAL_Circle_" + i.ToString() + "'>                                          " +
                       "                                <div class='flipswitch-inner no-yes'></div>                                                          " +
                       "                                <div class='flipswitch-switch'></div>                                                                " +
                       "                            </label>                                                                                                 " +
                       "                        </div>                                                                                                       " +
                       "                                                                                                                                     " +
                       "            </span>                                                                                                                  " +
                       "                                                                                                                                     " +
                       "             <span class='badge mr20'>                                                                                                   " +
                       "                <div class='flipswitch switch-info-light switch-inline-table'>   ");


                    if (Admin[i] == true)
                    {
                        Setting_List.Append(" <input type = 'checkbox' runat='server' checked='true' name='flipswitch' class='flipswitch-cb' id='VAL_Admin_"+i.ToString()+"'/>");
                    }
                    else
                    {
                        Setting_List.Append("<input type = 'checkbox' runat='server'  name='flipswitch' class='flipswitch-cb' id='VAL_Admin_"+i.ToString()+"'/>");
                    }

                    Setting_List.Append("                            <label class='flipswitch-label flipswitch-checked' for='VAL_Admin_"+i.ToString()+"'>                                          " +
                       "                                <div class='flipswitch-inner no-yes'></div>                                                          " +
                       "                                <div class='flipswitch-switch'></div>                                                                " +
                       "                            </label>                                                                                                 " +
                       "                        </div>                                                                                                       " +
                       "                                                                                                                                     " +
                       "            </span>                                                                                                                  " +
                          "      " + SettingKey[i].ToString() + "                                                                                          " +
                                          "        </li>  ");





                }

                

               PlaceHolder_Setting.Controls.Add(new Literal { Text = Setting_List.ToString() });

            }

            #endregion
        }
        catch (Exception ex)
        {


        }
        finally
        {


        }

    }





    [WebMethod()]

    public static bool PassThings(bool[] data)
    {

    string path = HttpContext.Current.Server.MapPath(@"~/setting.json");


        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            r.Close();

            var list = JsonConvert.DeserializeObject<setting>(json);
            
            //fill value in json key node 

            list.BranchUser.Printed_Characters_Full = data[0];
            list.CircleUser.Printed_Characters_Full = data[1];
            list.Admin.Printed_Characters_Full = data[2];

            list.BranchUser.Printed_Characters_Partial = data[3];
            list.CircleUser.Printed_Characters_Partial = data[4];
            list.Admin.Printed_Characters_Partial = data[5];

            list.BranchUser.Line_Feeds = data[6];
            list.CircleUser.Line_Feeds = data[7];
            list.Admin.Line_Feeds = data[8];

            list.BranchUser.Documents_Insertions = data[9];
            list.CircleUser.Documents_Insertions = data[10];
            list.Admin.Documents_Insertions = data[11];

            list.BranchUser.Cover_Openings = data[12];
            list.CircleUser.Cover_Openings = data[13];
            list.Admin.Cover_Openings = data[14];

            list.BranchUser.Paper_Jams = data[15];
            list.CircleUser.Paper_Jams = data[16];
            list.Admin.Paper_Jams = data[17];

            list.BranchUser.Front_Scannings = data[18];
            list.CircleUser.Front_Scannings = data[19];
            list.Admin.Front_Scannings = data[20];

            list.BranchUser.Back_Scannings = data[21];
            list.CircleUser.Back_Scannings = data[22];
            list.Admin.Back_Scannings = data[23];


            list.BranchUser.Power_on_hours = data[24];
            list.CircleUser.Power_on_hours = data[25];
            list.Admin.Power_on_hours = data[26];


            list.BranchUser.Standby_hours = data[27];
            list.CircleUser.Standby_hours = data[28];
            list.Admin.Standby_hours = data[29];

            list.BranchUser.Power_on_cycles = data[30];
            list.CircleUser.Power_on_cycles = data[31];
            list.Admin.Power_on_cycles = data[32];
            //  item.Admin.Printed_Characters_Full = false;
              var output = JsonConvert.SerializeObject(list, Formatting.Indented);
              File.WriteAllText(path, output);               


        }



        return true;
    }


 

    public class SchemaInfo
    {
        public bool Printed_Characters_Full { get; set; }
        public bool Printed_Characters_Partial { get; set; }
        public bool Line_Feeds { get; set; }
        public bool Documents_Insertions { get; set; }
        public bool Cover_Openings { get; set; }
        public bool Paper_Jams { get; set; }

        public bool Front_Scannings { get; set; }
        public bool Back_Scannings { get; set; }
        public bool Power_on_hours { get; set; }

        public bool Standby_hours { get; set; }
        public bool Power_on_cycles { get; set; }
    }


    public class setting
    {


        public SchemaInfo Admin { get; set; }

        public SchemaInfo CircleUser { get; set; }

        public SchemaInfo BranchUser { get; set; }
    }



    protected void BtnJsonWrite_Click(object sender, EventArgs e)
    {
        LoadSettingData();
    }
}
