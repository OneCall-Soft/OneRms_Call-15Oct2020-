<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DataPull.aspx.cs" Inherits="DataPull" %>

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

        .gridviewPager {
            background-color: #fff;
            padding: 2px;
            margin: 2% auto;
        }

            .gridviewPager a {
                margin: auto 1%;
                border-radius: 50%;
                background-color: #545454;
                padding: 5px 10px 5px 10px;
                color: #fff;
                text-decoration: none;
                -o-box-shadow: 1px 1px 1px #111;
                -moz-box-shadow: 1px 1px 1px #111;
                -webkit-box-shadow: 1px 1px 1px #111;
                box-shadow: 1px 1px 1px #111;
            }

                .gridviewPager a:hover {
                    background-color: #337ab7;
                    color: #fff;
                }

            .gridviewPager span {
                background-color: #066091;
                color: #fff;
                -o-box-shadow: 1px 1px 1px #111;
                -moz-box-shadow: 1px 1px 1px #111;
                -webkit-box-shadow: 1px 1px 1px #111;
                box-shadow: 1px 1px 1px #111;
                border-radius: 50%;
                padding: 5px 10px 5px 10px;
            }
             .cssPager td {
            padding: 0px 5px;
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
        function radioChange(str) {
            debugger;
            if (str == '0') {
                document.getElementById('<%=DownloadData.ClientID%>').style.display = "none";
        document.getElementById('<%=SelectDate.ClientID%>').style.display = "block";
    }
    else {
        document.getElementById('<%=DownloadData.ClientID%>').style.display = "block";
        document.getElementById('<%=SelectDate.ClientID%>').style.display = "none";
    }
}
        function ClearDetail()
        {
            debugger;
        document.getElementById('<%=startDT.ClientID%>').value = "";
        document.getElementById('<%=endDT.ClientID%>').value = "";
        document.getElementById('<%=filtercategories.ClientID%>').selectedIndex = 0;
        document.getElementById('<%=orderid.ClientID%>').value = "";
        document.getElementById('<%=srch.ClientID%>').value = "";
        document.getElementById('<%=filtercategories1.ClientID%>').selectedIndex = 0;

        return false;
    }

    function detailValidation() {
        debugger;
        if ((document.getElementById('<%=MachineLogs.ClientID%>').checked || document.getElementById('<%=EJLogs.ClientID%>').checked) && (document.getElementById('<%=startDT.ClientID%>').value == "" || document.getElementById('<%=endDT.ClientID%>').value == "")) {
                errorDisplay("Fill Start Date and End Date");
                return false;
            }
            else {
                if ((document.getElementById('<%=filtercategories.ClientID%>').selectedIndex == 0 || document.getElementById('<%=orderid.ClientID%>').value == "") && document.getElementById('<%=OtherData.ClientID%>').checked) {
                    errorDisplay("Select Type and Give Valid Path");
                    return false;
                }
                else {
                    var hide = document.getElementById('<%=hiddenItem.ClientID%>');
                    hide.value = "";
                    var iplist = "";
                    var checkboxes = document.getElementsByName("inputname");
                    var tblData = document.getElementById("RegisterMachine");
                    for (var i = 0, n = checkboxes.length; i < n; i++) {
                        if (checkboxes[i].checked)
                            iplist += tblData.rows[i + 1].cells[1].innerText + "#";
                    }
                    if (iplist == "") {
                        errorDisplay("Select atleast One Kiosk");
                        return false;
                    }
                    else {
                        hide.value = iplist;
                        return true;
                    }
                }
            }
        }
        function errorDisplay(str) {

            document.getElementById("error").innerHTML = str;
            document.getElementById('<%=myModal.ClientID%>').style.display = "block";
        }

    </script>
    <script type="text/javascript">
        var filter = 0;

        function Search_Gridview(strKey) {
            try {

                filter = 0;
                var strData = strKey.value.toLowerCase().split(" ");
                var tblData = document.getElementById("RegisterMachine");
                <%--var tblData = document.getElementById("<%=kioskDetail.ClientID %>");--%>
                var rowData;
                debugger;
                for (var i = 1; i < tblData.rows.length; i++) {
                    debugger;
                    rowData = tblData.rows[i].innerText;
                    var styleDisplay = 'none';
                    debugger;
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

        function CheckUncheckAll() {

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
       
    </script>

    <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">Data Pull</li>
            </ol>
        </div>
    </header>
    <!-- /Topbar -->


    <!-- Content -->
    <section id="content" class="table-layout animated fadeIn">

        <!-- Column Center -->
        <div class="chute chute-center">

            <!-- AllCP Info -->
            <div class="">

                <div class="row">
                    <div class="col-md-6 col-lg-6 ">

                        <div class="allcp-form">
                            <div class="panel pn" id="">
                                <%--spy1--%>
                                <div class="panel-heading">
                                    <i class="icon-graphic-1"></i>
                                    <span class="panel-title">Data Pull From Machines</span>
                                </div>
                                <div class="panel-body p20 pb25 br-t">
                                    <div class="section">


                                        <div runat="server">
                                            <asp:HiddenField ID="hiddenItem" runat="server" Value="" />
                                            <div class="row">
                                                <h5 class="pln mt20">Search Options</h5>
                                                <%--<h5 class="pln mt20">Filter Options</h5>  --%>
                                                <div class="col-md-6">
                                                    <h6 class="mb15">Select Circle</h6>
                                                    <div class="section mb20">
                                                        <label class="field select">

                                                            <asp:DropDownList runat="server" ID="filtercategories1" AutoPostBack="true" OnSelectedIndexChanged="filtercategories_SelectedIndexChanged"></asp:DropDownList>
                                                            <i class="arrow double"></i>
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <h6 class="mb15">Enter Search Value</h6>
                                                    <div class="section mb20">
                                                        <label for="Search Value" class="field prepend-icon">
                                                            <input type="text" name="Search Value" id="srch" class="gui-input" runat="server" onkeyup="Search_Gridview(this)"
                                                                placeholder="Search Value" onkeydown="return (event.keyCode!=13);" />
                                                            <span class="field-icon">
                                                                <i class="fa fa-tag"></i>
                                                            </span>
                                                        </label>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="row">
                                                <div class="col-md-3">
                                                    <h5>Data to Pull</h5>
                                                </div>
                                                <div class="col-md-3 pt10">
                                                    <label style="">
                                                        <asp:RadioButton ID="MachineLogs" runat="server" Text="Logs" Checked="true" GroupName="Software" onchange="radioChange(0);" />
                                                    </label>
                                                </div>
                                                <div class="col-md-3 pt10" style="display: none;">
                                                    <label style="">
                                                        <asp:RadioButton ID="EJLogs" runat="server" Text="EJ Data" GroupName="Software" onchange="radioChange(0);" />
                                                    </label>
                                                </div>
                                                <div class="col-md-3 pt10">
                                                    <label style="">
                                                        <asp:RadioButton ID="OtherData" runat="server" Text="Other Data" GroupName="Software" onchange="radioChange(1);" />
                                                    </label>
                                                    <br />
                                                </div>
                                            </div>

                                            <div id="SelectDate" runat="server" class="row" style="display: block;">
                                                <div class="col-md-2">
                                                    <h5>Date Range</h5>
                                                </div>
                                                <div class="col-md-5">
                                                    <div class="section">

                                                        <div class="form-group">
                                                            <div class='input-group date'>
                                                                From Date
                                                                             <asp:TextBox runat="server" ID="startDT" TextMode="Date" CssClass="form-control" placeholder="From Date" Style="padding-left: 54px; height: 45px"></asp:TextBox>
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                                <div class="col-md-5">
                                                    <div class="section">


                                                        <div class="form-group">
                                                            <div class='input-group date'>
                                                                To Date
                                                                        <asp:TextBox runat="server" ID="endDT" TextMode="Date" CssClass="form-control" placeholder="To Date" Style="padding-left: 54px; height: 45px"></asp:TextBox>
                                                                <span class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-calendar"></span>
                                                                </span>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>



                                            <div id="DownloadData" class="row" style="display: none;" runat="server">
                                                <div class="col-md-2">
                                                    <h5>Download Data</h5>
                                                </div>
                                                <div class="col-md-5">

                                                    <label class="field select">
                                                        <asp:DropDownList runat="server" ID="filtercategories" name="filter-categories">
                                                            <asp:ListItem Text="Filter by Data"></asp:ListItem>
                                                            <asp:ListItem Text="Directory"></asp:ListItem>
                                                            <asp:ListItem Text="File"></asp:ListItem>
                                                            <asp:ListItem Text="IniUpdate"></asp:ListItem>
                                                        </asp:DropDownList>

                                                        <i class="arrow double"></i>
                                                    </label>
                                                </div>
                                                <div class="col-md-5">
                                                    <label for="order-id" class="field prepend-icon">
                                                        <asp:TextBox runat="server" ID="orderid" autocomplete="off" CssClass="gui-input" placeholder="Directory/File Path/IniCommand"></asp:TextBox>

                                                        <span class="field-icon">
                                                            <i class="fa fa-tag"></i>
                                                        </span>
                                                    </label>
                                                </div>
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                            </div>





                                            <div class="row">
                                                <div class="col-md-3">
                                                </div>

                                                <div class="col-md-3">
                                                    <asp:Button runat="server" ID="downloadCommand" CssClass="btn btn-primary" Width="100%" Text="Download" OnClick="downloadCommand_Click" OnClientClick="return detailValidation();" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Button runat="server" ID="cancel" CssClass="btn btn-primary" Width="100%" Text="Reset" OnClientClick="return ClearDetail();" />
                                                </div>

                                            </div>





                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>


                    <div class="col-md-6 col-lg-6 ">
                        <div class="allcp-form">
                            <div class="panel pn" id="">
                                <%--spy1--%>
                                <div class="panel-heading">
                                    <i class="icon-graphic-1"></i>
                                    <span class="panel-title">Pulled Data </span>
                                </div>
                                <div class="panel-body p20 pb25 br-t">


                                  
                                    <div style="max-height: 350px; overflow-y: scroll; clear: both;">
                                        <asp:GridView ID="GV_Pulled_data" class="  table  table-data table-bordered table-hover table-responsive-md table-striped text-left grid-row b_table"
                                            AutoGenerateColumns="False"
                                            AllowPaging="True"
                                            PageSize="1000" 
                                            OnPageIndexChanging="GV_Pulled_data_PageIndexChanging" 
                                            OnRowCommand="GV_Pulled_data_RowCommand"
                                            runat="server">

                                            <Columns>
                                                <asp:BoundField DataField="branch_code" HeaderText="Branch Code" />
                                                <asp:BoundField DataField="kiosk_ip" HeaderText="Kiosk IP" />
                                                <asp:BoundField DataField="ack_dt" HeaderText="Created Date" />
                                                <asp:TemplateField HeaderText="Data Path">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server"
                                                             CommandArgument='<%# Eval("dataPath") %>' 
                                                            CommandName="downlaod" Text='<%# Eval("filename")%>'>


                                                        </asp:LinkButton>

                                                    </ItemTemplate>

                                                </asp:TemplateField>


                                            </Columns>

                                            <PagerSettings Position="Bottom" FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="cssPager" />

                                        </asp:GridView>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="" id="kiosklist" runat="server">

                    <div class="row">
                        <!-- AllCP Grid -->
                        <div class="col-md-12 allcp-grid">

                            <!-- Perfomance -->

                            <div class="panel mb10" data-panel-title="false" runat="server">
                                <div class="panel-heading">
                                    <div class="col-md-10  fs24  mb20">
                                        Registered Machines
                               
                                    </div>


                                    <div class="col-md-2">

                                        <asp:Button runat="server" ID="btn_refresh" CssClass="btn btn-primary" Width="100%" Text="Refresh" OnClick="btn_refresh_Click" />

                                    </div>
                                </div>
                                <div class="panel-body mtn pn">

                                    <div class="table-responsive">



                                        <table id="RegisterMachine" class="table allcp-form theme-warning tc-checkbox-1 btn-gradient-grey fs13">

                                            <thead>
                                                <tr class="">
                                                    <th class="" style="padding-left: 32px;">
                                                        <input type="checkbox" id="checkUncheckAll" onclick="CheckUncheckAll()" class="checkbox mn" /></th>
                                                    <%--<th class="text-center">Select</th>--%>
                                                    <th class="" style="font-size: small;">Machine IP</th>
                                                    <th class="" style="font-size: small;">Machine ID</th>
                                                    <th class="" style="font-size: small;">Location</th>
                                                    <th class="" style="font-size: small;">Command Type</th>
                                                    <th class="" style="font-size: small;">Command Status</th>
                                                    <th class="" style="font-size: small;">Command Acknowledgement Date</th>
                                                </tr>
                                            </thead>

                                            <tbody>

                                                <asp:Literal ID="kiosk_data" runat="server"></asp:Literal>
                                            </tbody>
                                        </table>


                                    </div>

                                </div>
                            </div>

                        </div>
                        <!-- /AllCP Grid -->
                    </div>

                </div>


                <asp:Label runat="server" ID="errorlabel" CssClass="label label-dark" Text="Select Circle First" Height="30px" Font-Bold="true" Font-Size="Large"></asp:Label>

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

