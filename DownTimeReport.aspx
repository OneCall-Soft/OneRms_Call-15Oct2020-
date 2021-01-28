<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DownTimeReport.aspx.cs" Inherits="TransactionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
    .cssPager td
    {
        padding: 0px 5px;
    }
</style>


    <script type="text/javascript">
        
        var filter = 0;

        function Search_Gridview(strKey, T) {
            try {
          
                filter = 0;
                var strData = strKey.value.trim().toLowerCase().split(" ");
                var tblData = document.getElementById("<%=GV_DownTime_Details.ClientID %>");
                var rowData;

               

                for (var i = 1; i < tblData.rows.length; i++) {
                   
                    if (T == 100) {
                        rowData = tblData.rows[i].innerText;
                    }
                    else {
                        rowData = tblData.rows[i].cells[T].innerText;
                    }


                   
                    var styleDisplay = 'none';
                    var k;
                    for (var j = 0; j < strData.length; j++) {
                            if (rowData.toLowerCase().indexOf(strData[j]) >= 0) {
                                styleDisplay = '';
                                k = 1;
                            }
                            else {
                                k = 0;
                                styleDisplay = 'none';
                                break;
                            }
                        
                    }
                    if (k == 1)
                        filter++;

                    tblData.rows[i].style.display = styleDisplay;
                    //if (strKey.value.trim() == "")
                    //    filter = tblData.rows.length-1;
                }
         

            } catch (e) {

            }
            finally { document.getElementById("Show").innerText = filter + ' Out of '; }
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(window).keydown(function (event) {
                if (event.keyCode == 13 || event.keyCode == 32) {
                    event.preventDefault();
                    return false;
                }
            });
        });

    </script>  

      <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">DownTime Report</li>
            </ol>
        </div>
    </header>

    <div class="row mt4">
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header with-border">

                    <div class="row">
                        <div class="col-md-2">
                            <div class="form-group">
                                <i class="fa fa-calendar-o"></i><span>Month </span>
                                <asp:DropDownList ID="DDL_Duration" runat="server" AutoPostBack="true" class="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-2">
                            <i class="fa fa-search"></i><span>LHO Circle </span>
                            <asp:DropDownList ID="ddl_lho_list" class="form-control" runat="server">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1 ">
                            <asp:Button ID="btnSearchbyLHOcircle" runat="server" Text="Find" class="btn btn-success mt25" OnClick="btnSearchbyLHOcircle_Click" />
                        </div>

                        <div class="col-md-2 ">

                            <i class="fa fa-search"></i><span>Search By Branch Code</span>
                            <asp:TextBox ID="txt_branchcode_searching" class="form-control" MaxLength="6" OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)" runat="server" placeholder="Branch Code"></asp:TextBox>

                        </div>

                        <div class="col-md-1">
                            <asp:Button ID="btn_Search_by_branch_code" OnClick="btn_Search_by_branch_code_Click" class="btn btn-success mt25" runat="server" Text="Find" />

                        </div>

                        <div class="col-md-2" runat="server" visible="false">

                            <i class="fa fa-search"></i><span>Search By Serial No.</span>
                            <asp:TextBox ID="txt_SerialNo_searching" class="form-control" runat="server" placeholder="Serial No."></asp:TextBox>


                        </div>

                        <div class="col-md-1"  runat="server" visible="false">
                            <asp:Button ID="btn_Search_by_serial_no" OnClick="btn_Search_by_serial_no_Click" class="btn btn-success mt25" runat="server" Text="Find" />
                        </div>
                    </div>
                </div>

                <!-- /.box-header -->
                <div class="box-body" style="max-height: 500px; overflow-y: scroll; clear: both;">
                    <asp:GridView ID="GV_DownTime_Details" class="table table-data table-bordered  table-hover table-responsive-md table-striped text-left  grid-row b_table" runat="server" AutoGenerateColumns="true">
                    </asp:GridView>
                </div>
                <!-- /.box-body -->

                <%-- box-footer--%>
                <div class="box-footer">
                    <div class="col-md-10">
                        <strong>
                            <asp:Label ID="lbl_tot" runat="server" Text=""></asp:Label>
                        </strong>
                    </div>
                    <div id="ExportBTN" class="col-md-2" runat="server">
                        <asp:Button ID="btn_Download" runat="server" Text="Download Report" class="btn btn-flat btn-success form-control" OnClick="btn_Download_Click" />
                    </div>
                </div>
            </div>

        </section>
    </div>

   


</asp:Content>

