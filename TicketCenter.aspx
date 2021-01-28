<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TicketCenter.aspx.cs" Inherits="TicketCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
       <style>
        .cssPager td {
            padding: 0px 5px;
        }
    </style>
      <script type="text/javascript">

        var filter = 0;

          function Search_Gridview(strKey, T)
          {
                debugger;
                try {

                    filter = 0;
                    var strData = strKey.value.trim().toLowerCase().split(" ");
                    var tblData = document.getElementById("<%=GV_Ticket_Details.ClientID %>");
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

    <script type="text/javascript">
        $(function () {

            $('[id*=btnExport]').on('click', function () {
                ExportToExcel('GV_Ticket_Details');
                // location.reload();
            });

        });


        function ExportToExcel(Id) {
            debugger;
            // get a new date (locale machine date time)
            var date = new Date();
            // get the date as a string
            var n = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
            // get the time as a string
            var time = date.toLocaleTimeString();

            var tab_text = "<Table border='2' bgColor='#ffffff' " +
                            "borderColor='#000000' Text-align='Center' cellSpacing='0' cellPadding='0' " + "style='font-size:10.0pt; font-family:Calibri; background:white'> " +
                          "<TR><TD COLSPAN='18' style='background:yellow; font-size:14.0pt; vertical-align:middle; Text-align:Center;  height:35px;'><B>AU SMALL FINANCE BANK PASSBOOK PRINTING KIOSKS - TICKET REPORT GENERATED ON " + n + " " + time +  "</B></TD></TR></table>";
                       
           
            tab_text += "<table border='2px' style='color:#000;'><tr>";
            var textRange;
            var j = 0;
            tab = document.getElementById(Id);
            var headerRow = $('[id*=GV_Ticket_Details] tr:first');

            tab_text += '<th>S.No</th> ' + headerRow.html() + '</tr><tr>';
            var rows = $('[id*=GV_Ticket_Details] tr:not(:has(th))');
            var x = 0;
            for (j = 0; j < rows.length; j++) {


                if ($(rows[j]).css('display') != 'none' && ($(rows[j]).attr('class') != 'cssPager')) {
                    x++;
                    tab_text = tab_text + '<td>' + x + '</td>' + rows[j].innerHTML + "</tr>";
                }
            }
            tab_text = tab_text + "</table>";
            // tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, ""); //remove if u want links in your table
            // tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
            // tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params
            debugger;
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(tab_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, Id + ".xlsx");
            }
            else {                 //other browser not tested on IE 11
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
            }
            return (sa);
        }

    </script>

     <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">Call Ticket's Report</li>
            </ol>
        </div>
    </header>

    <div class="row mt04">
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header  ">

                    <div class="col-md-2" style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>LHO - Circle List </span>
                            <asp:DropDownList ID="DDL_LHO" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DDL_LHO_SelectedIndexChanged">
                                <asp:ListItem Value="0"><--Select--></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-2" style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Branch Name </span>
                            <asp:DropDownList ID="DDL_Branch" runat="server" AutoPostBack="true" class="form-control" >
                            </asp:DropDownList>
                        </div>
                    </div>

                  <%--  <div class="col-md-2" runat="server">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Location type</span>
                            <asp:DropDownList ID="DDl_location" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>--%>


                    <div class="col-md-2" style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Site type</span>
                            <asp:DropDownList ID="DDl_SiteType" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>


                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Category</span>
                            <asp:DropDownList ID="DDL_category" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Queue type</span>
                            <asp:DropDownList ID="DDL_Queue" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <%--<div class="col-md-2"  >
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Call type</span>
                            <asp:DropDownList ID="DDL_Call" runat="server"  AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>--%>

                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Sub Category</span>
                            <asp:DropDownList ID="DDL_Subcategory" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Status</span>
                            <asp:DropDownList ID="DDL_Status" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>


                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Kiosk Status</span>
                            <asp:DropDownList ID="DDL_KioskStatus" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Assigned To</span>
                            <asp:DropDownList ID="DDL_ASS" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>worklist</span>
                            <asp:DropDownList ID="DDL_worklist" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Tickets</span>
                            <asp:DropDownList ID="DDL_tickets" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-2"  style="display:none;">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>CRS Visits</span>
                            <asp:DropDownList ID="DDL_CRA" runat="server" AutoPostBack="true" class="form-control">
                                <asp:ListItem Value="0">--ALL--</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="col-md-2 pt30" style="display:none;">
                        <div class="form-group">
                            <input id="btnSearch" type="button" value="Search"  class="btn btn-flat btn-success form-control " />
                        </div>
                    </div>
                    
                    <div class="col-md-2 pt30" style="display:none;">
                        <div class="form-group">
                            <input id="btnClear" type="button" value="Clear" class="btn btn-flat btn-success form-control" />
                        </div>
                    </div>

                    <div class="col-md-2">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Enter Ticket No</span>
                            <asp:TextBox ID="txt_ticket" class="form-control" placeholder="Filter by Ticket No" runat="server" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,0)"></asp:TextBox>
                        </div>
                    </div>


                    <%--<div class="col-md-2">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Enter Kiosk ID</span>
                            <asp:TextBox ID="txt_kiosk" class="form-control"  placeholder="Filter by Kiosk ID" runat="server" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,2)"></asp:TextBox>
                        </div>
                    </div>--%>

                   <%-- <div class="col-md-2">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Enter Site ID</span>
                            <asp:TextBox ID="txt_site" class="form-control" runat="server"  placeholder="Filter by Site ID" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,1)"></asp:TextBox>
                        </div>
                    </div>--%>

                    <%--<div class="col-md-2">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Enter Call date</span>
                            <asp:TextBox ID="txt_call_date" class="form-control" runat="server"  placeholder="Filter by Call Date" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,7)"></asp:TextBox>
                        </div>
                    </div>--%>

                    <div class="col-md-2">
                        <div class="form-group">
                            <i class="fa fa-calendar-o"></i><span>Enter Branch Code</span>
                            <asp:TextBox ID="txt_branch_code" class="form-control"  placeholder="Filter by Branch code" runat="server" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,4)"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="form-group">
                            <i class="fa fa-search"></i><span>Search</span>
                            <asp:TextBox ID="txtSearch" class="form-control" runat="server" placeholder="Search for filter Table Records" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,100)"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <br />
                <!-- /.box-header -->
                <div class="box-body" runat="server" visible="false">
                    <div class="col-md-4"></div>
                    <div class=" col-md-4 " style="border: 1px solid #261d77;">
                        All Tickets
                        <table style="width: 100%;">
                            <tr>
                                <th>1H</th>
                                <th>2H</th>
                                <th>4H</th>
                                <th>8H</th>
                                <th>1D</th>
                                <th>3D</th>
                                <th>5D</th>
                                <th>1W</th>
                                <th>Total</th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_1h" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_2h" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_4h" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_8h" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_1D" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_3D" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_5D" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_1W" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Total" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="col-md-4">
                      
                    </div>

                </div>

                <div style="max-height: 800px; overflow-y: scroll; clear: both;">
                   <asp:GridView 
                       ID="GV_Ticket_Details" 
                       class="table table-data table-bordered  table-hover table-responsive-md table-striped text-left  grid-row b_table" 
                       runat="server"
                       AutoGenerateColumns="true">
                   </asp:GridView>
                </div>
                <!-- /.box-body -->

                <%-- box-footer--%>
                <div class="box-footer">
                    <div class="col-md-10">
                       <strong> <asp:Label ID="lbl_tot" runat="server" Text=""></asp:Label>  
                        </strong>
                    </div>
                    <div class="col-md-2">
                        <i class="fa fa-download"></i><span>Download</span>
                        <asp:Button ID="btn_Download" runat="server" Text="Download Report" class="btn btn-flat btn-success form-control" OnClick="btn_Download_Click" />
                    </div>
                </div>

            </div>

        </section>
    </div>

    <div class="row mt20" runat="server" visible="false">
        <div  id="UploadExcel" runat="server"  class="col-md-3 form-group"  >
            <i class="fa fa-search"></i><span> Select Excel File</span>
            <asp:FileUpload ID="BtnExcelUpload" CssClass="form-control fileupload-controls  " runat="server" /><br />
            <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-flat btn-success form-control " Text="Upload" OnClick="btnUpload_Click" />  
        </div>        
    </div>

</asp:Content>

