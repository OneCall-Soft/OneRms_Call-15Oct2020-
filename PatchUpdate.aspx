<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PatchUpdate.aspx.cs" Inherits="PatchUpdate" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }
    </style>
    <script type="text/javascript">
        window.onclick = function (event) {
            if (event.target == document.getElementById('<%=myModal.ClientID%>')) {
                 document.getElementById('<%=myModal.ClientID%>').style.display = "none";
  }
         }
history.pushState(null, null, location.href);
window.onpopstate = function () {
    history.go(1);
};
function Chart_Click(strKey)
{
    try
    {
        filter = 0;
        var strData = strKey.toLowerCase().split(" ");
        var tblData = document.getElementById("RegisterMachine");
        var rowData;

        for (var i = 1; i < tblData.rows.length; i++) {

            rowData = tblData.rows[i].innerText;
            var styleDisplay = 'none';

            for (var j = 0; j < strData.length; j++) {
                if (rowData.toLowerCase().indexOf(strData[j]) >= 0) {
                    styleDisplay = '';
                    filter++;
                }
                else {
                    styleDisplay = 'none';
                    break;
                }
            }

            tblData.rows[i].style.display = styleDisplay;

        }
         <%--   var rowsCount = <%=GV_Kiosk_Health.Rows.Count %>;--%>

            } catch (e) {

            }
            finally { document.getElementById("Show").innerText = filter + ' Out of '; }
        }

        function errorDisplay(str) {
            document.getElementById("error").innerHTML = str;
            document.getElementById('<%=myModal.ClientID%>').style.display = "block";
        }

        function PatchUpdate()
        {
            var hide = document.getElementById('<%=hiddenItem.ClientID%>');
            hide.value = "";
            if (document.getElementById('<%=file5.ClientID%>').files.length == 0)
            {
                errorDisplay("Kindly Upload Patch First");
                return false;
            }
            else if (document.getElementById('<%=file5.ClientID%>').files.length > 1)
            {
                errorDisplay("Only One patch could be Uploaded at Time");
                return false;
            }
            else
            {
                var iplist = "";
                var checkboxes = document.getElementsByName("inputname");
                var tblData = document.getElementById("RegisterMachine");
                for (var i = 0, n = checkboxes.length; i < n; i++) {
                    if (checkboxes[i].checked)
                        iplist += tblData.rows[i + 1].cells[1].innerText + "#";
                }
                if (iplist == "")
                {
                    errorDisplay("Select atleast One Kiosk");
                    return false;
                }
                else
                {
                    hide.value = iplist;
                    return true;
                }
            }
        }
    </script>
    <script type="text/javascript">
        var filter = 0;

        function Search_Gridview(strKey,T) {
            try {


                filter = 0;
                var strData = strKey.value.toLowerCase().split(" ");
                var tblData = document.getElementById("RegisterMachine");
                var rowData;

                for (var i = 1; i < tblData.rows.length; i++)
                {
                    if (T == 100)
                    {
                        rowData = tblData.rows[i].innerText;
                    }
                    else
                    {
                        rowData = tblData.rows[i].cells[T].innerText;
                    }


                   // rowData = tblData.rows[i].innerText;
                    var styleDisplay = 'none';
                    var k;
                    for (var j = 0; j < strData.length; j++) {
                        if (rowData.toLowerCase().indexOf(strData[j]) >= 0) {
                            styleDisplay = '';
                            filter++;
                        }
                        else {
                            styleDisplay = 'none';
                            break;
                        }
                    }

                    tblData.rows[i].style.display = styleDisplay;

                }
         <%--   var rowsCount = <%=GV_Kiosk_Health.Rows.Count %>;--%>

            } catch (e) {

            }
            finally { document.getElementById("Show").innerText = filter + ' Out of '; }
        }


    </script>

    <script type="text/javascript">

        function CheckUncheckAll()
        {
            var selectAllCheckbox = document.getElementById("checkUncheckAll");
            if (selectAllCheckbox.checked == true) {
                var checkboxes = document.getElementsByName("inputname");
                var tblData = document.getElementById("RegisterMachine");
                for (var i = 0, n = checkboxes.length; i < n; i++) {
                    if (tblData.rows[i + 1].style.display == '')
                        checkboxes[i].checked = true;
                }
            } else {
                var checkboxes = document.getElementsByName("inputname");
                var tblData = document.getElementById("RegisterMachine");
                for (var i = 0, n = checkboxes.length; i < n; i++) {
                    if (tblData.rows[i + 1].style.display == '')
                        checkboxes[i].checked = false;
                }
            }
        }

        function patchUploaded()
        {  
            document.getElementById('<%=uploader5.ClientID%>').value = document.getElementById('<%=file5.ClientID%>').value;
        }

    </script>
    <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">Patch Management</li>
            </ol>
        </div>
    </header>

    <section id="content" class="table-layout animated fadeIn">

        <!-- Column Center -->
        <div class="chute chute-center">

            <!-- AllCP Info -->
            <div class="">

                <div id="id1" runat="server">
                    <asp:HiddenField ID="hiddenItem" runat="server" Value="" />
                    <%--<asp:HiddenField ID="HiddenUploadPath" runat="server" Value="" />--%>
                    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

                    <div class="row">
                        <div class="col-md-12 col-lg-12 ">

                            <!-- Poll -->
                            <div class="allcp-form">
                                <div class="panel pn" id="">
                                    <%--spy1--%>
                                    <div class="panel-heading ">
                                        <i class="icon-graphic-1"></i>
                                        <span class="panel-title">Patch Management</span>
                                    </div>
                                    <div class="panel-body p20 pb25 br-t">
                                        <div class="col-sm-6">
                                            <div class="section">
                                 

                                                <h5 class="pln mt20">Search Options</h5>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <h6 class="mb15">Select Circle</h6>
                                                        <div class="section mb20">
                                                            <label class="field select" style="z-index:0">                                                              
                                                                <asp:DropDownList runat="server" ID="filtercategories1" AutoPostBack="true" OnSelectedIndexChanged="filtercategories1_SelectedIndexChanged"></asp:DropDownList>
                                                                <i class="arrow double"></i>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5">
                                                      
                                                    </div>
                                                  
                                                </div>

                                            <div class="option-group field  mt20">
                                                <div class="checkbox-custom checkbox-primary mb5" style="height: 30px">                                                
                                                </div>
                                            </div>



                                            <h5>Select Patch To  Upload</h5>
                                            <label class="field append-icon file file-fw" style="z-index:0">                                        
                                                <asp:Button ID="UploadBtn" runat="server" Text="Update" CssClass="button" OnClick="UploadBtn_Click" OnClientClick="return PatchUpdate();" />
                                                     <asp:FileUpload runat="server" ID="file5" accept=".zip"
                                                    onchange="patchUploaded();" class="gui-file" />
                                                <asp:TextBox runat="server" class="gui-input" ID="uploader5"
                                                    placeholder="File Select"></asp:TextBox>

                                                <i class="fa fa-upload"></i>
                                            </label>


                                         
                                        </div>
                                    </div>

                                
                                    <div class="col-sm-3">
                                        <h5 class="">Windows Patch Versions</h5>
                                        <div id="">
                                            <asp:Chart ID="WindowsChart" runat="server" OnClick="WindowsChart_Click" Visible="false">
                                                <Series>
                                                    <asp:Series ChartArea="ChartArea1" Name="Series1" ChartType="Pie" Legend="Legend1" ToolTip="#VALX (#VALY)" PostBackValue="#VALX">
                                                    </asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1">
                                                        <Area3DStyle Enable3D="True" Inclination="20" Rotation="0" />
                                                    </asp:ChartArea>
                                                </ChartAreas>
                                                <Titles>
                                                    <asp:Title Text="" />
                                                </Titles>
                                            </asp:Chart>
                                        </div>
                                    </div>



                                    <div class="col-sm-3">
                                       <h5 class="">Linux Patch Versions</h5>
                                        <div id="">
                                            <asp:Chart ID="LinuxChart" runat="server" OnClick="LinuxChart_Click" Visible="false">
                                                <Series>
                                                    <asp:Series ChartArea="ChartArea1" Name="Series1" ChartType="Pie" Legend="Legend1" ToolTip="#VALX (#VALY)" PostBackValue="#VALX">
                                                    </asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1">
                                                        <Area3DStyle Enable3D="True" Inclination="20" Rotation="0" />
                                                    </asp:ChartArea>
                                                </ChartAreas>
                                                <Titles>
                                                    <asp:Title Text="" />
                                                </Titles>
                                            </asp:Chart>

                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>

                </div>


                <div id="kiosklist" runat="server">
                    <div class="row">
                        <!-- AllCP Grid -->
                        <div class="col-md-12 allcp-grid">

                            <!-- Perfomance -->
                            <div class="panel mb10" data-panel-title="false">
                               <div class="panel-heading">
                                    <div class=" col-md-3  fs24 mb20">
                                        Registered Machines
                                    </div>
                                    <div class=" col-md-4  mb20">
                                    </div>
                                    <div class="col-md-2">
                                      <asp:Button runat="server" ID="btn_refresh" CssClass="btn btn-primary" Width="100%" Text="Refresh" OnClick="btn_refresh_Click"  />
                                    </div>
                                    <div class=" col-md-3   mb20">
                                        <input type="text" id="SearchText" onkeydown="return (event.keyCode!=13);" name="Search Value" class="form-control" runat="server" onkeyup="Search_Gridview(this,100)"
                                            placeholder="Search Value"/>
                                    </div>
                                </div>
                                <div class="panel-body mtn pn">
                                    <div class="table-responsive">
                                        <table id="RegisterMachine" class="table allcp-form theme-warning tc-checkbox-1 btn-gradient-grey fs13">
                                            <thead>
                                                <tr class="">
                                                    <th class="" style="padding-left: 32px;">
                                                        <input type="checkbox" id="checkUncheckAll" onclick="CheckUncheckAll()" class="checkbox mn" /></th>
                                                    <%-- <th class="text-center">Select</th>--%>
                                                    <th class="" style="font-size: small;">Machine IP</th>
                                                    <th class="" style="font-size: small;">Machine ID</th>
                                                    <th class="" style="font-size: small;">Machine Type</th>
                                                   <%--<th class="" style="font-size: small;">Location</th>--%>
                                                    <th class="" style="font-size: small;">Patch Version (Client - KPROC)</th>
                                                    <th class="" style="font-size: small;">Command Type</th>
                                                       <th class="" style="font-size: small;">status</th>
                                                    <th class="" style="font-size: small;">Update Date Time</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Literal ID="kiosk_data" runat="server"></asp:Literal>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <asp:Label runat="server" ID="errorlabel" CssClass="label label-dark" Text="Select State First" Height="30px" Font-Bold="true" Font-Size="Large"></asp:Label>
                        </div>
                        <!-- /AllCP Grid -->
                    </div>
                </div>
            </div>
        </div>
    </div>
                <!-- /Column Center -->

    </section>
    <div id="myModal" runat="server" class="modal">
        <div class="panel-body" style="background-color: wheat; text-align: center; width: 300px; height: 200px; margin-left: 40%; border-radius: 50px">
            <h6 class="mb20" id="error"></h6>
        </div>
    </div>


</asp:Content>

