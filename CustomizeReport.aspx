<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomizeReport.aspx.cs" Inherits="TransactionReport" %>

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
        function filterBranchCode() {

            try {
                var input, filter, table, tr, td, i, k = 0, tot = 0, txtValue;
                input = document.getElementById("txtBranchCode");
                filter = input.value.toLowerCase();
                table = document.getElementById("<%=GV_Txn_Details.ClientID %>");
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

                        }
                    }


                }
            } catch (e) {

            } finally {
                document.getElementById("Show").innerText = k + ' Out of ';

            }



        }

        function filterSerialNo() {

            try {
                var input, filter, table, tr, td, i, k = 0, tot = 0, txtValue;
                input = document.getElementById("txtSerial");
                filter = input.value.toLowerCase();
                table = document.getElementById("<%=GV_Txn_Details.ClientID %>");
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
                <li class="breadcrumb-current-item">Transactions Report</li>
            </ol>
        </div>
    </header>


    <div class="row mt4">
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header with-border ">




                    <div class="row">
                        <div class="col-md-2">

                            <div class="form-group">
                                <i class="fa fa-filter"></i><span>Report Type</span>
                                <asp:DropDownList ID="DDL_Report_Type" runat="server" class="form-control">

                                    <asp:ListItem Value="0" Selected="True"> Daily Txn Report  </asp:ListItem>
                                    <asp:ListItem Value="1"> Summarize Txn Report  </asp:ListItem>
                                    <asp:ListItem Value="2">  Zero Txn Report </asp:ListItem>
                                    <asp:ListItem Value="3"> CBS Error Report </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div class="col-md-2">

                            <div class="form-group">
                                <i class="fa fa-calendar-o"></i><span for="startDate">Start Date</span>
                                <%--   <input id="startDate"  onkeypress="return (event.keyCode!=13);" name="startDate" type="text" class="form-control" />--%>

                                <asp:TextBox ID="txtFromDate" runat="server" onkeypress="return (event.keyCode!=13);" type="Date" style="padding:0px 0px 0px 0px" onkeydown="return false" class="form-control" placeholder="MM-DD-YYYY"></asp:TextBox>
                                <%--   <asp:TextBox ID="txtFromDate" onkeypress="return (event.keyCode!=13);" runat="server" class="form-control" ReadOnly="true" ></asp:TextBox>--%>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="form-group">
                                <i class="fa fa-calendar-o"></i><span for="endDate">End Date</span>
                                <%-- <input id="endDate"   onkeypress="return (event.keyCode!=13);" name="endDate" type="text" class="form-control" />--%>

                                <asp:TextBox ID="txtToDate" onkeydown="return false" type="Date" style="padding:0px 0px 0px 0px"  onkeypress="return (event.keyCode!=13);" runat="server" class="form-control" placeholder="MM-DD-YYYY"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <i class="fa fa-search"></i><span>Filter</span>
                            <asp:Button ID="BtnGo" runat="server" CssClass="form-control btn  btn-success" onkeypress="return (event.keyCode!=32);" Text="Find" OnClientClick="return check()" OnClick="BtnGo_Click" />
                        </div>

                        <div class="col-md-4">
                          <asp:ListBox ID="ListBox1" runat="server" CssClass="form-control text-dark-dark" Style="position: absolute; z-index: 10; height: 124px; font-size:14px"></asp:ListBox>                            
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-2 ">

                            <i class="fa fa-search"></i><span>Search By Branch Code</span>
                            <asp:TextBox ID="txtBranchCode" class="form-control" runat="server"   MaxLength="6"  OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)"></asp:TextBox>

                           
                        </div>

                        <div class="col-md-1">
                            <asp:Button ID="btn_Search_by_branch_code" OnClick="btn_Search_by_branch_code_Click" class="btn btn-success mt25" runat="server" Text="Find" />
                        </div>

                        <div class="col-md-2">

                            <i class="fa fa-search"></i><span>Search By Serial No.</span>
                            <asp:TextBox ID="txtSerial" class="form-control" runat="server"></asp:TextBox>


                        </div>

                        <div class="col-md-1">
                            <asp:Button ID="btn_Search_by_serial_no" OnClick="btn_Search_by_serial_no_Click" class="btn btn-success mt25" runat="server" Text="Find" />


                        </div>

                          <div class="col-md-2">
                             <asp:Button ID="Button1" runat="server" CssClass="form-control btn  btn-success" onkeypress="return (event.keyCode!=32);" Text="Download Excel Report" OnClientClick="return check()" OnClick="BtnGoExcelDownload_Click" />

                                 <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="More than 3 Days Report Directly Downloaded"></asp:Label>
                        </div>



                    </div>
                </div>

                <!-- /.box-header -->
                <div class="box-body" style="max-height: 500px; overflow-y: scroll; clear: both;">
                    <asp:GridView ID="GV_Txn_Details"
                        class="table table-data table-bordered  table-hover table-responsive-md table-striped text-left  grid-row b_table"
                        runat="server"
                        AutoGenerateColumns="true">
                    </asp:GridView>
                    <%--<asp:Label ID="lbl_total" runat="server" Text=""></asp:Label>--%>
                </div>
                <!-- /.box-body -->

                <%-- box-footer--%>
                <div class="box-footer">
                    <div class="col-md-10">
                        <strong>
                            <asp:Label ID="lbl_tot" runat="server" Text=""></asp:Label>
                        </strong>
                    </div>
                    <div class="col-md-2" runat="server" id="ExcelBtn">
                        <i class="fa fa-download"></i><span>Download</span>
                        <asp:Button ID="btnExport" runat="server" Text="Report"
                            class="btn btn-flat btn-success form-control"
                            OnClick="btnExport_Click" />
                    </div>
                </div>
            </div>

        </section>
    </div>


    <script type="text/javascript">


        $(function () {
            $('[id*=txtToDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy-mm-dd",


                //  format: 'L',

            });
        });

        $(function () {
            $('[id*=txtFromDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy-mm-dd",
                // format: 'L',
            });
        });

        $(function () {

            $('[id*=txtFromDate]').datepicker({
                weekStart: 1,
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', function (selected) {

                if ($('[id*=txtFromDate]').val() == '' || typeof
        $('[id*=txtFromDate]').val() == "undefined") {
                    $('[id*=txtToDate]').val('').datepicker('setStartDate', null);
                }
                else {
                    $('[id*=txtToDate]').val('').datepicker('setDate', new Date());
                    $('[id*=txtToDate]').datepicker('setStartDate', null);
                    startDate = new Date(selected.date.valueOf());
                    startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
                    $('[id*=txtToDate]').datepicker('setDate', startDate);
                    $('[id*=txtToDate]').datepicker('setStartDate', startDate);
                    $('[id*=txtToDate]').val('').datepicker('update', '');
                }
            });

            $('[id*=txtFromDate]').blur(function (selected) {
                if ($('[id*=txtToDate]').val() != '' && $('[id*=txtFromDate]').val() == '' || typeof $('[id*=txtFromDate]').val() == "undefined") {
                    $('[id*=txtFromDate]').datepicker('setDate', new Date());
                    $('[id*=txtFromDate]').val('').datepicker('setStartDate', null);
                    $('[id*=txtToDate]').datepicker('setDate', new Date());
                    $('[id*=txtToDate]').val('').datepicker('setStartDate', null);
                }
            });



        });



    </script>

</asp:Content>

