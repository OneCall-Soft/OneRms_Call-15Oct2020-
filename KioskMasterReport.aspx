<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KioskMasterReport.aspx.cs" Inherits="KioskMaster" %>

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

  

     <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">Kiosk Master Report</li>
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
                            <i class="fa fa-search"></i><span> LHO Circle </span>                           
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
                  

                </div>



            </div>

        </section>
    </div>

</asp:Content>

