<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="true" CodeFile="LHO_Details.aspx.cs" Inherits="LHO_Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .cssPager td {
            padding: 0px 5px;
        }
    </style>

       <script type="text/javascript" src="doc/js/aes.js"></script>

    <script>
        function validateBranchInput()
        {
            var branchcode = document.getElementById("txtBranchCode");
            if (branchcode.length > 6)
            {
                branchcode.innerText = branchcode.innerText % 10;
            }
        }


        function filterBranchCode()
        {  

            try {
                var input,filter, table, tr, td, i, k = 0, tot = 0, txtValue;
                input = document.getElementById("txtBranchCode");
                filter = input.value.toLowerCase();

                if (filter.length < 6)
                {
                    if (filter.length == 0) {

                    }
                    else {
                        alert("Please Input Six Digit Branch Code");
                        return;
                    }
                }

                table = document.getElementById("<%=GV_LHO_Details.ClientID %>");
                tr = table.getElementsByTagName("tr");
                for (i = 0; i < tr.length; i++) {
                    td = tr[i].getElementsByTagName("td")[2];
                    if (td) {
                        txtValue = td.textContent || td.innerText;
                        if (txtValue.toLowerCase().indexOf(filter) > -1) {
                            tr[i].style.display = "";
                            k++;
                        } else {
                            tr[i].style.display = "none";
                            //
                        }
                    }


                }
            } catch (e) {

            } finally {
                // document.getElementById("Show").innerText = k + ' Out of ';
                return false;
            }

        }

        function filterSerialNo() {

            try {
                var input, filter, table, tr, td, i, k = 0, tot = 0, txtValue;
                input = document.getElementById("txtSerial");
                filter = input.value.toLowerCase();
                table = document.getElementById("<%=GV_LHO_Details.ClientID %>");
                tr = table.getElementsByTagName("tr");
                for (i = 0; i < tr.length; i++) {
                    td = tr[i].getElementsByTagName("td")[3];
                    if (td) {
                        txtValue = td.textContent || td.innerText;
                        if (txtValue.toLowerCase().indexOf(filter) > -1) {
                            tr[i].style.display = "";
                            k++;
                        } else {
                            tr[i].style.display = "none";

                        }
                    }


                }
            } catch (e) {

            } finally {
                // document.getElementById("Show").innerText = k + ' Out of ';

            }

        }


    </script>

    <script>
        jQuery(document).ready(function () {
            jQuery("#CPnumber").keypress(function (e) {
                var length = jQuery(this).val().length;
                if (length > 9) {
                    return false;
                } else if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                } else if ((length == 0) && (e.which == 48)) {
                    return false;
                }
            });
        });
         jQuery(document).ready(function () {
            jQuery("#Eng_Mob_number").keypress(function (e) {
                var length = jQuery(this).val().length;
                if (length > 9) {
                    return false;
                } else if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                } else if ((length == 0) && (e.which == 48)) {
                    return false;
                }
            });
        });

        function validateEmail(emailField)
        {
            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

            if (reg.test(emailField.value) == false) {
                alert('Invalid Email Address');
                return false;
            }

            return true;

        }

        function blockSpecialChar(e)
        {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }



    </script>



    <div class="row mt20">
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header with-border  ">

                    <div class="row">
                        <div class="col-md-2">
                            <i class="fa fa-search"></i><span>Branch Code</span>
                            <a href="#" data-toggle="tooltip" title="Search By Branch Code">
                                <%--<asp:TextBox ID="txtBranchCode" class="form-control" runat="server" placeholder="Search By Branch Code" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,0)"></asp:TextBox>--%>
                                <input type="text" id="txtBranchCode" class="form-control" maxlength="6" <%--onkeyup="filterBranchCode()"--%>  placeholder="Search for Branch Code" />
                            </a>
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btn_Search_by_branch_code" OnClick="btn_Search_by_branch_code_Click" OnClientClick="return filterBranchCode()" class="btn btn-success mt25" runat="server" Text="Find" />

                        </div>
                        <div class="col-md-2 hide ">
                            <i class="fa fa-search"></i><span>Serial No.</span>
                            <a href="#" data-toggle="tooltip" title="Search By Serial No.">
                                <%--<asp:TextBox ID="txtSerial" class="form-control" runat="server" placeholder="Search By Serial No." onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,3)"></asp:TextBox>--%>
                                <input type="text" id="txtSerial" class="form-control" onkeyup="filterSerialNo()" placeholder="Search for Serial No." />

                            </a>
                        </div>




                        <div class="col-md-6 text-center">
                            <span>LHO name </span>
                            <br />
                            <span class="badge badge-dark fs14  mt5">
                                <asp:Label ID="lbl_lho_name" CssClass="text-white  " runat="server" Text=""></asp:Label>
                            </span>

                        </div>

                        <div class="col-md-2 text-center">
                            <span>Total Record(s) </span>
                            <br />
                            <span class="badge badge-dark fs14  mt5">
                                <asp:Label ID="lbl_tot" CssClass="text-white   " runat="server" Text=""></asp:Label></span>

                        </div>
                        <div class="col-md-2 ">
                            <asp:Button ID="btn_reload" OnClick="btn_reload_Click" CssClass=" form-control btn-success mt20  " Style="display: none;" runat="server" Text="reload" />

                        </div>
                    </div>
                </div>

                <div class="box-body" style="max-height: 600px; overflow-y: scroll; clear: both;">
                    <asp:GridView ID="GV_LHO_Details" runat="server" Width="100%" Style=""
                        OnRowDataBound="GV_LHO_Details_RowDataBound"
                        OnRowCommand="GV_LHO_Details_RowCommand"
                        AutoGenerateColumns="False"
                        AllowPaging="false"
                        PageSize="200"
                        class="table  table-data table-bordered table-hover table-responsive-md table-striped text-left grid-row b_table"
                        OnPageIndexChanging="GV_LHO_Details_PageIndexChanging">

                        <Columns>
                            <asp:BoundField DataField="Branch_Name" HeaderText="Branch Name - Kiosk ID"></asp:BoundField>
                            <asp:BoundField DataField="kiosk_ip" HeaderText="Kiosk IP"></asp:BoundField>
                            <asp:BoundField DataField="branch_code" HeaderText="Branch Code" />
                            <asp:BoundField DataField="HealthStatus" HeaderText="Connectivity"></asp:BoundField>
                            <asp:BoundField DataField="health_time" HeaderText="Health Time"></asp:BoundField>
                            <asp:BoundField DataField="pb_status" HeaderText="PB Status"></asp:BoundField>
                            <asp:BoundField DataField="touchscreen_status" HeaderText="Touch Status"></asp:BoundField>
                            <asp:BoundField DataField="app_status" HeaderText="Application"></asp:BoundField>
                            <asp:BoundField DataField="ribbon_status" HeaderText="Ink Level"></asp:BoundField>
                            <asp:BoundField DataField="OS_Type" HeaderText="OS Type"></asp:BoundField>
                            <asp:TemplateField HeaderText="Issue">
                                <ItemTemplate>
                                    <div runat="server" visible='<%# Eval("Call_Log_Status").ToString() == "open" ? true : false%>'>
                                        <button type="button" style="" id="btn_CloseIssue" class="btn btn-danger btn-lg" onclick="ClearDataCallClose(this.id,this)"
                                            data-toggle="modal" data-target="#popUpWindowCloseTicket"
                                            data-serial_number1='<%# Eval("f_machine_serial_no") %>'
                                            data-contact_person_name1='<%# Eval("ContactPersonName") %>'
                                            data-contact_person_number1='<%# Eval("ContactPersonNumber") %>'
                                            data-contact_person_email1='<%# Eval("Email_To") %>'
                                            data-ksid1='<%# Eval("kiosk_id") %>'
                                            data-ksip1='<%# Eval("kiosk_ip") %>'
                                           
                                            data-brcode1='<%# Eval("branch_code") %>'
                                            data-health_time1='<%# Eval("health_time") %>'
                                            data-pb_status1='<%# Eval("pb_status") %>'
                                            data-ink_status1='<%# Eval("ribbon_status") %>'>
                                            Issue Close
                                        </button>
                                    </div>

                                    <div runat="server" visible='<%# Eval("Call_Log_Status").ToString() == "open" ? false : true %>'>
                                        <button type="button" style="" id="btnShowPopup" class="btn btn-primary btn-lg" onclick="ClearData(this.id,this)"
                                            data-toggle="modal" data-target="#popUpWindow"
                                            data-serial_number='<%# Eval("f_machine_serial_no") %>'
                                            data-contact_person_name='<%# Eval("ContactPersonName") %>'
                                            data-contact_person_number='<%# Eval("ContactPersonNumber") %>'
                                            data-contact_person_email='<%# Eval("Email_To") %>'
                                            data-ksid='<%# Eval("kiosk_id") %>'
                                            data-ksip='<%# Eval("kiosk_ip") %>'
                                            data-brname='<%# Eval("br_name") %>'
                                            data-brcode='<%# Eval("branch_code") %>'
                                            data-health_time='<%# Eval("health_time") %>'
                                            data-pb_status='<%# Eval("pb_status") %>'
                                            data-ink_status='<%# Eval("ribbon_status") %>'>
                                            Submit Issue
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="System Restart">
                                <ItemTemplate>
                                    <asp:LinkButton ID="RestartButton" runat="server" 
                                        CommandName="Restart_Command" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" OnClientClick="return confirm('Are you sure you want to Restart this Machine?');"
                                        ForeColor="#47d1af" BackColor="Transparent" BorderStyle="None" Text="Restart"/> 
                                    <%--<button onclick="return Confirm('Do you want to Restart this machine.')" >Restart</button>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="System Shutdown">
                                <ItemTemplate>
                                    <asp:LinkButton ID="ShutDownButton" runat="server" 
                                        CommandName="Shutdown_Command" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" OnClientClick="return confirm('Are you sure you want to Shutdown this Machine?');"
                                        ForeColor="#47d1af" BackColor="Transparent" BorderStyle="None" Text="Shutdown"/> 
                                    <%--<button onclick="return Confirm('Do you want to Restart this machine.')" >Restart</button>--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Remote Desktop">
                                <ItemTemplate>
                                    <asp:LinkButton ID="RemoteDesktopButton" runat="server" 
                                        CommandName="RemoteDesktop_Command" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" OnClientClick="return confirm('Are you sure you want to remote this Machine?');"
                                        ForeColor="#47d1af" BackColor="Transparent" BorderStyle="None" Text="RDP"/> 
                                    <%--<button onclick="return Confirm('Do you want to Restart this machine.')" >Restart</button>--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:ButtonField CommandName="Restart_Command" HeaderText="System Restart" Text="Restart" />--%>
                            <%--<asp:ButtonField CommandName="Shutdown_Command" HeaderText="System Shutdown" Text="Shutdown" />--%>
                            <asp:TemplateField HeaderText="Details">
                                <ItemTemplate>
                                    <button type="button" style="" id="btnShowDetails" class="btn btn-primary btn-lg" onclick="showDetails(this.id,this)"
                                        data-toggle="modal" data-target="#popUpDetails"
                                        data-ksid='<%# Eval("kiosk_id") %>'
                                        data-ksip='<%# Eval("kiosk_ip") %>'
                                        data-brname='<%# Eval("br_name") %>'
                                        data-serial_number='<%# Eval("f_machine_serial_no") %>'
                                        data-software_version='<%# Eval("software_version") %>'
                                        data-antivirus='<%# Eval("Anti_Virus") %>'

                                        data-install_date='<%# Eval("install_date") %>'
                                        data-mode='<%# Eval("mode") %>'
                                        data-lastmnt='<%# Eval("lastMnt") %>'
                                        data-lastpm='<%# Eval("lastPM") %>'
                                        data-dnsinfo='<%# Eval("DNS_Info") %>'

                                        data-hostname='<%# Eval("hostname") %>'
                                        data-windowspatches='<%# Eval("windows_patches") %>'
                                          data-mac_address='<%# Eval("mac_address") %>'

                                        data-call_log_status='<%# Eval("Call_Log_Status") %>'
                                        data-drivers='<%# Eval("Drives") %>'
                                        data-ram='<%# Eval("ram") %>'
                                        data-processor='<%# Eval("processor") %>'
                                        data-printed_character_full='<%# Eval("printed_character_full") %>'
                                        data-printed_character_partial='<%# Eval("printed_character_partial") %>'
                                        data-back_scanning='<%# Eval("back_scanning") %>'
                                        data-cover_open='<%# Eval("cover_open") %>'
                                        data-document_insertion='<%# Eval("document_insertion") %>'
                                        data-front_scanning='<%# Eval("front_scanning") %>'
                                        data-line_feed='<%# Eval("line_feed") %>'
                                        data-paper_jam='<%# Eval("paper_jam") %>'
                                        data-power_on_cycles='<%# Eval("power_on_cycles") %>'
                                        data-power_on_hrs='<%# Eval("power_on_hrs") %>'
                                        data-stand_by_hrs='<%# Eval("stand_by_hrs") %>'>
                                        show
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="adstatus" HeaderText="AD Status"></asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" Position="Bottom" LastPageText="Last" Mode="NumericFirstLast" />
                        <PagerStyle CssClass="cssPager" />
                    </asp:GridView>

                </div>



            </div>
        </section>
    </div>


    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>


    <div class="modal fade " id="popUpDetails" style="z-index: 50000000000001" role="dialog" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog col-lg-10 col-md-6 col-md-offset-3" style="width:100%" >
            <div class="modal-content">
                <!-- header -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <%--<div class="row">
                        <div class="col-lg-6">
                             <h3 class="modal-title">Kiosk Details</h3>
                        </div>
                        <div class="col-lg-6">
                            <h3 class="modal-title">Printer Statistics</h3>
                        </div>
                    </div>--%>
                </div>

                <!-- body -->
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-6">
                            <h3 class="modal-title">Kiosk Details</h3>
                            <table class="table  table-data table-bordered   text-left ">

                            <tbody>
                                <tr>
                                    <td class="col-sm-3"><b>Kiosk ID :</b></td>
                                    <td class="col-sm-3">
                                        <label id="Kiosk_ID_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Kiosk IP :</b></td>
                                    <td class="col-sm-3">
                                        <label id="Kiosk_IP_1"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-sm-3"><b>Branch Name :</b></td>
                                    <td class="col-sm-3">
                                        <label id="Branch_Name_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Serial No :</b></td>
                                    <td class="col-sm-3">
                                        <label id="Serial_Number_1"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-sm-3"><b>Software Version  :</b></td>
                                    <td class="col-sm-3">
                                        <label id="Software_Version_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Anti Virus :</b></td>
                                    <td class="col-sm-3">
                                        <label id="anti_virus_1"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-sm-3"><b>Call Log Status :</b></td>
                                    <td class="col-sm-3">
                                        <label id="call_log_status_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Drives :</b></td>
                                    <td class="col-sm-3">
                                        <label id="drivers_1"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="col-sm-3"><b>Ram :</b></td>
                                    <td class="col-sm-3">
                                        <label id="ram_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Processor :</b></td>
                                    <td class="col-sm-3">
                                        <label id="processor_1"></label>
                                    </td>
                                </tr>

                                 <tr>
                                    <td class="col-sm-3"><b>Install Date :</b></td>
                                    <td class="col-sm-3">
                                        <label id="install_date_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Mode :</b></td>
                                    <td class="col-sm-3">
                                        <label id="mode_1"></label>
                                    </td>
                                </tr>
                                    <tr>
                                    <td class="col-sm-3"><b>Maintenance :</b></td>
                                    <td class="col-sm-3">
                                        <label id="lastmnt_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Preventive Maintenance :</b></td>
                                    <td class="col-sm-3">
                                        <label id="lastpm_1"></label>
                                    </td>
                                </tr>
                                      
                                
                                <tr>
                                    <td class="col-sm-3"><b>Host Name :</b></td>
                                    <td class="col-sm-3">
                                        <label id="hostname_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>Windows Patches :</b></td>
                                    <td class="col-sm-3">
                                        <label id="windowspatches_1"></label>
                                    </td>
                                </tr>


                                   <tr>
                                    <td class="col-sm-3"><b>DNS Info :</b></td>
                                    <td class="col-sm-3">
                                        <label id="dnsinfo_1"></label>
                                    </td>
                                    <td class="col-sm-3"><b>MAC Add ::</b></td>
                                    <td class="col-sm-3">
                                        <label id="mac_address_1"></label>
                                    </td>
                                </tr>

                              

                            </tbody>

                        </table>
                        </div>
                        
                        <div class="col-lg-6">
                            <h3 class="modal-title">Printer Statistics</h3>
                            <table class="table  table-data   text-left ">

                                <tbody>
                                    <tr>
                                        <td class="col-sm-9"><b>printed_character_full :</b></td>
                                        <td class="col-sm-3">
                                            <label id="printed_character_full_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>printed_character_partial :</b></td>
                                        <td class="col-sm-3">
                                            <label id="printed_character_partial_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>back_scanning  :</b></td>
                                        <td class="col-sm-3">
                                            <label id="back_scanning_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>cover_open :</b></td>
                                        <td class="col-sm-3">
                                            <label id="cover_open_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>document_insertion :</b></td>
                                        <td class="col-sm-3">
                                            <label id="document_insertion_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>front_scanning :</b></td>
                                        <td class="col-sm-3">
                                            <label id="front_scanning_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>line_feed :</b></td>
                                        <td class="col-sm-3">
                                            <label id="line_feed_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>paper_jam :</b></td>
                                        <td class="col-sm-3">
                                            <label id="paper_jam_1"></label>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="col-sm-9"><b>power_on_cycles:</b></td>
                                        <td class="col-sm-3">
                                            <label id="power_on_cycles_1"></label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="col-sm-9"><b>power_on_hrs :</b></td>
                                        <td class="col-sm-3">
                                            <label id="power_on_hrs_1"></label>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="col-sm-9"><b>stand_by_hrs :</b></td>
                                        <td class="col-sm-3">
                                            <label id="stand_by_hrs_1"></label>
                                        </td>

                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <!-- Call Logging Form -->
    <div class="modal fade" id="popUpWindow" style="z-index: 50000000000000" role="dialog" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- header -->
                <div class="modal-header">

                    <button type="button" class="close" data-dismiss="modal">&times;</button>

                    <div class="panel-header-menu pull-right mr10 text-muted fs12 ">
                        <span class="badge badge-dark pull-right mt5 pt5 mr20"
                            data-toggle="tooltip" data-placement="top" title=""
                            data-original-title="Current Date & Time">
                            <label id="cDT"></label>
                        </span>
                    </div>
                    <h3 class="modal-title">Post Issue</h3>

                </div>
                <!-- body -->
                <div class="modal-header">
                    <div class="row">

                        <div class="col-md-6">

                            <i class="fa fa-user"></i><span>Contact Person Name : </span>
                            <input type="text" class="form-control" id="CPname" onkeypress="allowAlphaNumericSpace(event)"/>

                        </div>
                        <div class="col-md-6">
                            <i class="fa fa-phone"></i><span>Contact Person Number :</span>
                            <input type="text" class="form-control" id="CPnumber" onkeypress="phoneno()" maxlength="10" />
                        </div>
                        <div class="col-md-12">
                            <i class="fa fa-envelope"></i><span>Email To :</span>
                            <input type="text" id="EmailTo" class="form-control" onblur="validateEmail(this);" />
                        </div>

                        <div class="col-md-12">
                            <i class="fa fa-bug"></i><span>Issue Title</span>
                            <select class="form-control select" id="IssueTitle">
                                <option value="0" selected="selected">Select Issue Title</option>
                                <option value="Power off">Power off </option>
                                <option value="Passbook Printer Related">Passbook Printer Related </option>
                                <option value="System Related">System Related </option>
                                <option value="Network Related">Network Related </option>
                                <option value="Hardware Related">Hardware Related</option>
                                <option value="Other">Other... </option>
                            </select>
                        </div>
                        <hr />
                        <div class="col-md-12">
                            <span class="badge badge-warning  mt5 pt5 ml10">Machine Serial Number -
                                <label id="Kser"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5  ml10">Kiosk ID -   
                                <label id="KSID"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5 ml10">Kiosk IP -   
                                <label id="KSIP"></label>
                            </span>

                            <span class="badge badge-warning  mt5 pt5 ml10">Branch Name -
                                <label id="BRNAME"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5 ml10">Branch Code-
                                <label id="BRCODE"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5 ml10">Last Health Time -
                                <label id="Health_Time"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5 ml10">PB Status -
                                <label id="PB_Status"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5 ml10">Catridge Status -
                                <label id="INK_Status"></label>
                            </span>
                        </div>
                        <div class="col-md-12">

                            <i class="fa fa-edit"></i><span>Issue Details</span>
                            <textarea class="form-control  gui-textarea h-150" maxlength="150" id="IssueDetails" name="IssueDetails" placeholder="Type Issue Details .... Max Length is 150 Character" onkeypress="allowAlphaNumericSpace(event)"></textarea>

                        </div>

                    </div>
                </div>
                <!-- footer -->
                <div class="modal-footer">
                    <button class="btn btn-success pull-left " id="SubmitIssue" onclick="btnSubmitIssue()">Send</button>

                    <span class="pull-right fs18 mt10 mr20 text-dark-darker" id="lblTKNumber"></span>
                </div>

            </div>
        </div>
    </div>


     <!-- Call Closing Form -->
    <div class="modal fade" id="popUpWindowCloseTicket" style="z-index: 50000000000000" role="dialog" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- header -->
                <div class="modal-header">

                    <button type="button" class="close" data-dismiss="modal">&times;</button>

                    <div class="panel-header-menu pull-right mr10 text-muted fs12 ">
                        <span class="badge badge-dark pull-right mt5 pt5 mr20"
                            data-toggle="tooltip" data-placement="top" title=""
                            data-original-title="Current Date & Time">
                            <label id="CloseCall_cDT"></label>
                        </span>
                    </div>
                    <h3 class="modal-title">Close Issue</h3>

                </div>
                <!-- body -->
                <div class="modal-header">
                    <div class="row">

                        <div class="col-md-6">

                            <i class="fa fa-user"></i><span>Engineer Name : </span>
                            <input type="text" class="form-control" id="Eng_name" onkeypress="allowAlphaNumericSpace(event)"/>

                        </div>
                        <div class="col-md-6">
                            <i class="fa fa-phone"></i><span>Engineer Mobile Number :</span>
                            <input type="text" class="form-control" id="Eng_Mob_number" onkeypress="phoneno()" maxlength="10" />
                        </div>                       

                        <div class="col-md-12">
                            <i class="fa fa-bug"></i><span>Issue Title</span>
                            <select class="form-control select" id="Faults">
                                <option value="0" selected="selected">Select Issue Title</option>
                                <option value="Power off">Power off </option>
                                <option value="Passbook Printer Related">Passbook Printer Related </option>
                                <option value="System Related">System Related </option>
                                <option value="Network Related">Network Related </option>
                                <option value="Hardware Related">Hardware Related</option>
                                <option value="Other">Other... </option>
                            </select>
                        </div>
                        <hr />
                        <div class="col-md-12">
                            <span class="badge badge-warning  mt5 pt5 ml10">Machine Serial Number -
                                <label id="CloseCall_Kser"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5  ml10">Kiosk ID -   
                                <label id="CloseCall_KSID"></label>
                            </span>
                            <span class="badge badge-warning  mt5 pt5 ml10">Kiosk IP -   
                                <label id="CloseCall_KSIP"></label>
                            </span>
                         
                            <span class="badge badge-warning  mt5 pt5 ml10">Branch Code-
                                <label id="CloseCall_BRCODE"></label>
                            </span>                          
                            <span class="badge badge-warning  mt5 pt5 ml10">Last Health Time -
                                <label id="CloseCall_Health_Time"></label>
                            </span>
                        </div>
                        <div class="col-md-12">

                            <i class="fa fa-edit"></i><span>Work Done & Observation</span>
                            <textarea class="form-control  gui-textarea h-150" maxlength="150" id="FaultDetails" name="IssueDetails" placeholder="Type Work Done & Observation .... Max Length is 150 Character" onkeypress="allowAlphaNumericSpace(event)"></textarea>

                        </div>

                    </div>
                </div>
                <!-- footer -->
                <div class="modal-footer">
                    <button class="btn btn-success pull-left " id="SubmitFaults" onclick="callClosed()">Send</button>

                    <span class="pull-right fs18 mt10 mr20 text-dark-darker" id="lblCloseStatus"></span>
                </div>

            </div>
        </div>
    </div>


    <script type="text/javascript">

        function ClearData(ID, e1)
        {          
            var today = new Date();
            var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var CdateTime = date + ' ' + time;


            // var data = $.parseJSON($(this).attr('data-button'));
            //  var Tname =  data.name; 

            document.getElementById("IssueTitle").value = "";
            var e = document.getElementById("IssueTitle");
            e.selectedIndex = 0;
            document.getElementById("IssueDetails").value = "";
            document.getElementById("lblTKNumber").innerHTML = "";
            document.getElementById("cDT").innerHTML = CdateTime;
            document.getElementById("Kser").innerHTML = e1.getAttribute("data-serial_number");
            document.getElementById("CPname").value = e1.getAttribute("data-contact_person_name");
            document.getElementById("CPnumber").value = e1.getAttribute("data-contact_person_number");
            document.getElementById("EmailTo").value = e1.getAttribute("data-contact_person_email");
            document.getElementById("KSID").innerHTML = e1.getAttribute("data-ksid");
            document.getElementById("KSIP").innerHTML = e1.getAttribute("data-ksip");
            document.getElementById("BRNAME").innerHTML = e1.getAttribute("data-brname");
            document.getElementById("BRCODE").innerHTML = e1.getAttribute("data-brcode");
            document.getElementById("Health_Time").innerHTML = e1.getAttribute("data-health_time");
            document.getElementById("PB_Status").innerHTML = e1.getAttribute("data-pb_status");
            document.getElementById("INK_Status").innerHTML = e1.getAttribute("data-ink_status");
        }
         function ClearDataCallClose(ID, e1)
         {
             debugger;
            var today = new Date();
            var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
             var CdateTime = date + ' ' + time;
        

            // var data = $.parseJSON($(this).attr('data-button'));
            //  var Tname =  data.name; 

            document.getElementById("Faults").value = "";
            var e = document.getElementById("Faults");
            e.selectedIndex = 0;
            document.getElementById("FaultDetails").value = "";
            document.getElementById("lblCloseStatus").innerHTML = "";
            document.getElementById("CloseCall_cDT").innerHTML = CdateTime;
            document.getElementById("CloseCall_Kser").innerHTML = e1.getAttribute("data-serial_number1");
            document.getElementById("Eng_name").value = e1.getAttribute("data-contact_person_name1");
            document.getElementById("Eng_Mob_number").value = e1.getAttribute("data-contact_person_number1");         
            document.getElementById("CloseCall_KSID").innerHTML = e1.getAttribute("data-ksid1");
            document.getElementById("CloseCall_KSIP").innerHTML = e1.getAttribute("data-ksip1");          
             document.getElementById("CloseCall_BRCODE").innerHTML = e1.getAttribute("data-brcode1");     
                 document.getElementById("CloseCall_Health_Time").innerHTML = e1.getAttribute("data-health_time1");
          
        }

        function callClosed(ID, e1)
        {  var e1 = document.getElementById("Faults");
            var strOpt1 = e1.options[e1.selectedIndex].value;

            if (strOpt1 == 0)
            {
                alert("Please Select Issue Title");
                return;
            }
            

            if (document.getElementById("FaultDetails").value == "")
            {
                alert("Please Fill Issue Details");
                return;
            }

            var result = confirm("Want to Close Issue ?");

            debugger;
            if (result)
            {
                //Logic to                 
                IssueTitle = strOpt1;
               
                var personName = htmlEncode(document.getElementById("Eng_name").value);
                document.getElementById("Eng_name").value = data(personName);
                personName = document.getElementById("Eng_name").value;
                

                var personNumber = htmlEncode(document.getElementById("Eng_Mob_number").value);
                document.getElementById("Eng_Mob_number").value = data(personNumber);
                personNumber = document.getElementById("Eng_Mob_number").value; 

                var IssueDetails = htmlEncode(document.getElementById("FaultDetails").value);
                document.getElementById("FaultDetails").value = data(IssueDetails);
                IssueDetails = htmlEncode(document.getElementById("FaultDetails").value);
               

                var strKioskID = htmlEncode(document.getElementById("CloseCall_KSID").innerHTML);
                document.getElementById("CloseCall_KSID").innerHTML = data(strKioskID);
                strKioskID = htmlEncode(document.getElementById("CloseCall_KSID").innerHTML);
               

                var SerialNumber = htmlEncode(document.getElementById("CloseCall_Kser").innerHTML);
                document.getElementById("CloseCall_Kser").innerHTML = data(SerialNumber);
                SerialNumber = htmlEncode(document.getElementById("CloseCall_Kser").innerHTML);               

                var brCode = htmlEncode(document.getElementById("CloseCall_BRCODE").innerHTML);
                document.getElementById("CloseCall_BRCODE").innerHTML = data(brCode);
                brCode = htmlEncode(document.getElementById("CloseCall_BRCODE").innerHTML);


                document.getElementById("Eng_name").value = "";
                document.getElementById("Eng_Mob_number").value = "";            
                document.getElementById("FaultDetails").value = "";
                document.getElementById("CloseCall_BRCODE").value = "";
                document.getElementById("CloseCall_KSID").innerHTML = ""; 
                document.getElementById("CloseCall_Kser").innerHTML = "";
                document.getElementById("CloseCall_BRCODE").innerHTML = "";
                 
                PageMethods.CallClosed(IssueTitle, strKioskID, SerialNumber, brCode,
                                                    personName, personNumber, 
                    IssueDetails,Onsuccess, Onerror);
                PageMethods.Reload("A");
                 <%--var button = document.getElementById("<%=btn_reload.ClientID %>");
             button.click();--%>
               
            }
           
           <%-- brCode = e1.getAttribute("data-brcode");
            strKioskID = e1.getAttribute("data-ksid");

            PageMethods.CallClosed(brCode, strKioskID, Onsuccess, Onerror);

             var button = document.getElementById("<%=btn_reload.ClientID %>");
             button.click();--%>
        }

        function btnSubmitIssue() {
         
            var e1 = document.getElementById("IssueTitle");
            var strOpt1 = e1.options[e1.selectedIndex].value;

            if (strOpt1 == 0)
            {
                alert("Please Select Issue Title");
                return;
            }
            

            if (document.getElementById("IssueDetails").value == "")
            {
                alert("Please Fill Issue Details");
                return;
            }

            var result = confirm("Want to Submit Issue ?");


            if (result)
            {
                //Logic to                 
                IssueTitle = strOpt1;
               
                var personName = htmlEncode(document.getElementById("CPname").value);
                document.getElementById("CPname").value = data(personName);
                personName = document.getElementById("CPname").value;
                

                var personNumber = htmlEncode(document.getElementById("CPnumber").value);
                document.getElementById("CPnumber").value = data(personNumber);
                personNumber= document.getElementById("CPnumber").value;
              

                var Email_To = htmlEncode(document.getElementById("EmailTo").value);
                document.getElementById("EmailTo").value = data(Email_To);
                Email_To = htmlEncode(document.getElementById("EmailTo").value);
               

                var IssueDetails = htmlEncode(document.getElementById("IssueDetails").value);
                document.getElementById("IssueDetails").value = data(IssueDetails);
                IssueDetails = htmlEncode(document.getElementById("IssueDetails").value);
               

                var strKioskID = htmlEncode(document.getElementById("KSID").innerHTML);
                document.getElementById("KSID").innerHTML = data(strKioskID);
                strKioskID = htmlEncode(document.getElementById("KSID").innerHTML);
               

                var SerialNumber = htmlEncode(document.getElementById("Kser").innerHTML);
                document.getElementById("Kser").innerHTML = data(SerialNumber);
                SerialNumber = htmlEncode(document.getElementById("Kser").innerHTML);               

                var brCode = htmlEncode(document.getElementById("BRCODE").innerHTML);
                document.getElementById("BRCODE").innerHTML = data(brCode);
                brCode = htmlEncode(document.getElementById("BRCODE").innerHTML);


                document.getElementById("CPname").value = "";
                document.getElementById("CPnumber").value = "";
                document.getElementById("EmailTo").value = "";
                document.getElementById("IssueDetails").value = "";
                document.getElementById("BRCODE").value = "";
                document.getElementById("KSID").innerHTML = ""; 
                document.getElementById("Kser").innerHTML = "";
                document.getElementById("BRCODE").innerHTML = "";
                 
                PageMethods.SubmitIssue(IssueTitle, strKioskID, SerialNumber, brCode,
                                                    personName, personNumber, Email_To,
                    IssueDetails, Onsuccess, Onerror);      
                     PageMethods.Reload("A");
               
            }
        }

        function htmlEncode(value)
        {  
            return $('<textarea/>').text(value).html();
        }

        function htmlDecode(value)
        {
            return $('<textarea/>').html(value).text();
        }

        function data(s)
        {
            var key = CryptoJS.enc.Utf8.parse('1020230803070908');
            var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

            var encString = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(s), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
                });

            return encString;
        }


        function Onsuccess(result)
        {     
            alert(result); 
             var button = document.getElementById("<%=btn_reload.ClientID %>");
             button.click();
        }
      

        function Onerror()
        {   
            alert('Some error has ocurred!');    
             var button = document.getElementById("<%=btn_reload.ClientID %>");
             button.click();
        } 

        function allowAlphaNumericSpace(e)
        {
          var code = ('charCode' in e) ? e.charCode : e.keyCode;
          if (!(code == 32) && // space
            !(code > 47 && code < 58) && // numeric (0-9)
            !(code > 64 && code < 91) && // upper alpha (A-Z)
            !(code > 96 && code < 123)) { // lower alpha (a-z)
            e.preventDefault();
        }
}


    </script>


    <script>
        function showDetails(ID, e1)
        {
            document.getElementById("Kiosk_ID_1").innerHTML = e1.getAttribute("data-ksid");
            document.getElementById("Kiosk_IP_1").innerHTML = e1.getAttribute("data-ksip");
            document.getElementById("Branch_Name_1").innerHTML = e1.getAttribute("data-brname");
            document.getElementById("Serial_Number_1").innerHTML = e1.getAttribute("data-serial_number");
            document.getElementById("Software_Version_1").innerHTML = e1.getAttribute("data-software_version");
            document.getElementById("anti_virus_1").innerHTML = e1.getAttribute("data-antivirus");
            document.getElementById("install_date_1").innerHTML = e1.getAttribute("data-install_date");
            document.getElementById("mode_1").innerHTML = e1.getAttribute("data-mode");
            document.getElementById("lastmnt_1").innerHTML = e1.getAttribute("data-lastmnt");                          
            document.getElementById("lastpm_1").innerHTML = e1.getAttribute("data-lastpm");  
            document.getElementById("dnsinfo_1").innerHTML = e1.getAttribute("data-dnsinfo");  

            document.getElementById("hostname_1").innerHTML = e1.getAttribute("data-hostname");  
            document.getElementById("windowspatches_1").innerHTML = e1.getAttribute("data-windowspatches");  
            document.getElementById("mac_address_1").innerHTML = e1.getAttribute("data-mac_address"); 

            document.getElementById("call_log_status_1").innerHTML = e1.getAttribute("data-call_log_status");
            document.getElementById("drivers_1").innerHTML = e1.getAttribute("data-drivers");
            document.getElementById("ram_1").innerHTML = e1.getAttribute("data-ram");
            document.getElementById("processor_1").innerHTML = e1.getAttribute("data-processor");
            document.getElementById("printed_character_full_1").innerHTML = e1.getAttribute("data-printed_character_full");
            document.getElementById("printed_character_partial_1").innerHTML = e1.getAttribute("data-printed_character_partial");
            document.getElementById("back_scanning_1").innerHTML = e1.getAttribute("data-back_scanning");
            document.getElementById("cover_open_1").innerHTML = e1.getAttribute("data-cover_open");
            document.getElementById("document_insertion_1").innerHTML = e1.getAttribute("data-document_insertion");
            document.getElementById("front_scanning_1").innerHTML = e1.getAttribute("data-front_scanning");
            document.getElementById("line_feed_1").innerHTML = e1.getAttribute("data-line_feed");
            document.getElementById("paper_jam_1").innerHTML = e1.getAttribute("data-paper_jam");
            document.getElementById("power_on_cycles_1").innerHTML = e1.getAttribute("data-power_on_cycles");
            document.getElementById("power_on_hrs_1").innerHTML = e1.getAttribute("data-power_on_hrs");
            document.getElementById("stand_by_hrs_1").innerHTML = e1.getAttribute("data-stand_by_hrs");
        }


    </script>
</asp:Content>

