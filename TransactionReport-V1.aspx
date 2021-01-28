<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TransactionReport-V1.aspx.cs" Inherits="TransactionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">
        
        var filter = 0;

        function Search_Gridview(strKey,T) {
            try {
          
                filter = 0;
                var strData = strKey.value.trim().toLowerCase().split(" ");
                var tblData = document.getElementById("<%=GV_Txn_Details.ClientID %>");
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
                ExportToExcel('GV_Txn_Details');
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

            var tab_text = "<h2 align='center'>" + globle.BankName + " PASSBOOK PRINTING KIOSKS - TRANSACTION REPORT GENERATED ON " + n + " " + time + "</h2><br/>";
           // tab_text += "<strong>Error Description</strong>";
            //tab_text += "<ul>";
            //tab_text += "<li>Er 01- Please contact computer centre</li>                          " +
            //            "<li>Er 02- Barcode read fails</li>                                      " +
            //            "<li>Er 03- Passbook inserted wrongly</li>                               " +
            //            "<li>Er 04- Passbook inserted without barcode sticker</li>               " +
            //            "<li>Er 05- Invalid barcode</li>                                         " +
            //            "<li>Er 06- Inactive barcode</li>                                        " +
            //            "<li>Er 07- No data received</li>                                        " +
            //            "<li>Er 08- No transaction for selection, CBS error 855</li>             " +
            //            "<li>Er 09- Middlware service time-out</li>                            " +
            //            "<li>Er 99- Default (Generic) error</li>                                ";
                  
            //tab_text += "</ul>";
            tab_text += "<table border='2px'><tr>";
            var textRange;
            var j = 0;
            tab = document.getElementById(Id);
            var headerRow = $('[id*=GV_Txn_Details] tr:first');

            tab_text += headerRow.html() + '</tr><tr>';
            var rows = $('[id*=GV_Txn_Details] tr:not(:has(th))');
            for (j = 0; j < rows.length; j++) {
                if ($(rows[j]).css('display') != 'none') {
                    tab_text = tab_text + rows[j].innerHTML + "</tr>";
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

    <div class="row mt20" >
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header with-border ">


                      <div class="col-md-2" >

                         
              <i class="fa fa-calendar-o"></i><span > Duration</span>
                          <asp:DropDownList ID="DDL_Duration" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DDL_Duration_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True"> --Select--  </asp:ListItem>
                               <asp:ListItem Value="1" Selected="True"> Today - Current Date  </asp:ListItem>
                               <asp:ListItem Value="2" > From - To </asp:ListItem>
                          </asp:DropDownList>
                               </div>
                     

                    
                   <div class="col-md-2">
                          <i class="fa fa-search"></i>  <span>Kiosk IP</span>
                            <a href="#" data-toggle="tooltip" title="Search By Kiosk IP">
                                <asp:TextBox ID="txtKioskIP" class="form-control" runat="server" placeholder="Search By Kiosk IP" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,0)"></asp:TextBox>
                          
                                </a>
                        </div>

                      <div class="col-md-2">
                          <i class="fa fa-search"></i>  <span>Kiosk ID</span>
                            <a href="#" data-toggle="tooltip" title="Search By Kiosk ID">
                                <asp:TextBox ID="txtKioskID" class="form-control" runat="server" placeholder="Search By Kiosk ID" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,1)"></asp:TextBox>
                          
                                </a>
                        </div>

                  
                          <div class="col-md-2">
                          <i class="fa fa-search"></i>  <span>Branch Code</span>
                            <a href="#" data-toggle="tooltip" title="Search By Branch Code">
                                <asp:TextBox ID="txtBranchCode" class="form-control" runat="server" placeholder="Search By Branch Code" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,3)"></asp:TextBox>
                          
                                </a>
                        </div>

                      <div class="col-md-2">
                          <i class="fa fa-search"></i>  <span>Branch Name</span>
                            <a href="#" data-toggle="tooltip" title="Search By Branch Name">
                                <asp:TextBox ID="TxtBranchName" class="form-control" runat="server" placeholder="Search By Branch Name" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,4)"></asp:TextBox>
                          
                                </a>
                        </div>

                     <div class="col-md-2">
                          <i class="fa fa-search"></i>  <span>LHO Circle </span>
                            <a href="#" data-toggle="tooltip" title="Search By LHO Circle ">
                                <asp:TextBox ID="txtLHO" class="form-control" runat="server" placeholder="Search By LHO Circle" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,2)"></asp:TextBox>
                          
                                </a>
                        </div>
                        

                    <div class="col-md-2">
                        <i class="fa fa-search"></i><span> Search</span>
                        <a href="#" data-toggle="tooltip" title="Search for filter Records">
                            <asp:TextBox ID="txtSearch" class="form-control" runat="server" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this,100)"></asp:TextBox>
                        </a>
                    </div>

                  

                </div>

                <!-- /.box-header -->
                <div class="box-body" style="max-height: 500px; overflow-y: scroll; clear: both;">


                    <asp:GridView ID="GV_Txn_Details" class="table table-data table-bordered  table-hover table-responsive-md table-striped text-left  grid-row b_table" runat="server" AutoGenerateColumns="true" ClientIDMode="AutoID">
                    </asp:GridView>

                </div>
                <!-- /.box-body -->

                <%-- box-footer--%>
                <div class="box-footer">
                     <div class="col-md-10">
                    <strong>Showing :  <span id="Show"></span><span><%=GV_Txn_Details.Rows.Count %></span> Entries.
                    </strong>  </div>
                      <div class="col-md-2">
                       
                        <input id="btnExport" type="button" value="Download Report" class="btn btn-flat btn-success form-control" />

                    </div>

                </div>



            </div>

        </section>
    </div>


</asp:Content>

