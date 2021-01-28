<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KioskLocationType.aspx.cs" Inherits="Health" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $(window).keydown(function (event) {
                if (event.keyCode == 13 ) {
                    event.preventDefault();
                    return false;
                }
            });
        });
        function CheckNumericKeyInfo(char1, mozChar) {
            if (mozChar != null) { // Look for a Mozilla-compatible browser
                if ((mozChar >= 48 && mozChar <= 57) || char1 == 8)
                    RetVal = true;
                else {
                    RetVal = false;
                }
            }
            else { // Must be an IE-compatible Browser
                if ((char1 >= 48 && char1 <= 57) || char1 == 8) RetVal = true;
                else {
                    RetVal = false;
                }
            }
            return RetVal;
        }
    </script>

    <script>
        function filterBranchCode()
        {
            try
            {
                var input, filter, table, tr, td, i, k = 0, tot = 0, txtValue;
                input = document.getElementById("txtBranchCode");
                filter = input.value.toLowerCase();
                table = document.getElementById("<%=GV_Kiosk_Details.ClientID %>");
                tr = table.getElementsByTagName("tr");
                for (i = 0; i < tr.length; i++) {
                    td = tr[i].getElementsByTagName("td")[0];
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
            }
            catch (e)
            {

            }
            finally
            {
                document.getElementById("Show").innerText = k + ' Out of ';
            }
        }

        function filterSerialNo()
        {
            try
            {
                var input, filter, table, tr, td, i, k = 0, tot = 0, txtValue;
                input = document.getElementById("txtSerial");
                filter = input.value.toLowerCase();
                table = document.getElementById("<%=GV_Kiosk_Details.ClientID %>");
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
                document.getElementById("Show").innerText = k + ' Out of ';

            }

        }

    </script>

     <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">Kiosk LOcation Report</li>
            </ol>
        </div>
    </header>

    <div class="row mt4" >
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header with-border  ">
                    <div class="row" runat="server" id="searchingOption">
                       <div class="col-md-2">

                            <div class="form-group">
                                <i class="fa fa-filter"></i><span>Location Type</span>
                                <asp:DropDownList ID="DDLKioskLocationType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged_KioskLocationType" class="form-control">

                                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="1"> OnSite </asp:ListItem>
                                    <asp:ListItem Value="2">  OutSite </asp:ListItem>
                                    <asp:ListItem Value="3"> TTW </asp:ListItem>
                                      <asp:ListItem Value="4"> High Security/Defence Area </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <%--<div class="col-md-2 pt25">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-flat btn-success form-control" OnClick="btnSearch_Click" />
                        </div>--%>

                     <%--   <div class="col-md-2 ">

                            <i class="fa fa-search"></i><span>Search By Branch Code</span>
                            <asp:TextBox ID="txt_branchcode_searching" class="form-control"  MaxLength="6"  OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)" runat="server" placeholder="Branch Code"></asp:TextBox>

                        </div>

                        <div class="col-md-1">
                            <asp:Button ID="btn_Search_by_branch_code" OnClick="btn_Search_by_branch_code_Click" class="btn btn-success mt25" runat="server" Text="Find" />

                        </div>

                         <div class="col-md-2">

                            <i class="fa fa-search"></i><span>Search By Serial No.</span>
                            <asp:TextBox ID="txt_SerialNo_searching" class="form-control" runat="server" placeholder="Serial No."></asp:TextBox>


                        </div>

                        <div class="col-md-1">
                            <asp:Button ID="btn_Search_by_serial_no" OnClick="btn_Search_by_serial_no_Click" class="btn btn-success mt25" runat="server" Text="Find" />
                        </div>--%>
                        <div class="col-md-2">
                            <i class="fa fa-download"></i><span>Download</span>

                            <asp:Button ID="btnExport" runat="server" Text="Export Records" class="btn btn-flat btn-success form-control" OnClick="btnExport_Click" />
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-2 hide">
                            <i class="fa fa-search"></i><span>Branch Code</span>
                            <a href="#" data-toggle="tooltip" title="Search By Branch Code">
                                <%--   <asp:TextBox ID="txtBranchCode" class="form-control" runat="server" placeholder="Search By Branch Code" onKeyDown="return (event.keyCode!=13);"  OnTextChanged="txtBranchCode_TextChanged" AutoPostBack="True" ></asp:TextBox>--%>
                                <input type="text" id="txtBranchCode" class="form-control" onkeyup="filterBranchCode()" placeholder="Search for Branch Code" />

                            </a>
                        </div>

                        <div class="col-md-2  hide ">
                            <i class="fa fa-search"></i><span>Serial No.</span>
                            <a href="#" data-toggle="tooltip" title="Search By Serial No.">
                                <%--<asp:TextBox ID="txtSerial" class="form-control" runat="server" placeholder="Search By Serial No." onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,3)"></asp:TextBox>--%>
                                <input type="text" id="txtSerial" class="form-control" onkeyup="filterSerialNo()" placeholder="Search for Serial No." />

                            </a>
                        </div>
                         
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body" style="max-height: 500px; overflow-y: scroll;overflow-x:hidden; clear: both;">
                    <asp:GridView ID="GV_Kiosk_Details"
                        class="  table  table-data table-bordered table-hover table-responsive-md table-striped text-left grid-row b_table"
                        runat="server"
                        AutoGenerateColumns="true">
                    </asp:GridView>

                </div>
                <!-- /.box-body -->

                <%-- box-footer--%>
                <div class="box-footer">

                    <div class="col-md-10">
                        <strong>
                            <asp:Label ID="lbl_tot" runat="server" Text=""></asp:Label>
                        </strong>

                        <%--<asp:Label ID="lbl_total" runat="server" Text=""></asp:Label>--%>
                    </div>

               <%--     <div class="col-md-2" style="display: none;">
                        <i class="fa fa-download"></i><span>Download</span>

                        <input id="exportToExcel" type="button" value="Download Report" class="btn btn-flat btn-success form-control" />


                    </div>
                    <div class="col-md-2" runat="server" id="ExportBtn">
                        <i class="fa fa-download"></i><span>Download</span>
                        <asp:Button ID="btn_Download" runat="server" Text="Download Report" class="btn btn-flat btn-success form-control" OnClick="btn_Download_Click" />
                    </div>--%>

                </div>



            </div>

        </section>
    </div>






</asp:Content>

