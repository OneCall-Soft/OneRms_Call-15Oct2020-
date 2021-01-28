<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard-V3_1.aspx.cs" EnableEventValidation="true" Inherits="Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <!-- Meta and Title -->
    <meta charset="utf-8" />
    <title>ONE RMS - 12.6.1.9</title>
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0" />--%>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <!-- Favicon -->
    <%--    <link rel="shortcut icon" href="assets/img/favicon.png" />--%>
    <!-- Angular material -->
    <link rel="stylesheet" type="text/css" href="assets/skin/css/angular-material.min.css" />
    <!-- Icomoon -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/icomoon/icomoon.css" />
    <!-- AnimatedSVGIcons -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/animatedsvgicons/css/codropsicons.css" />
    <!-- Iconsweets CSS -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/iconsweets/iconsweets.css" />
    <!-- Octicons CSS -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/octicons/octicons.css" />
    <!-- Summernote -->
    <%--    <link rel="stylesheet" type="text/css" href="assets/js/plugins/summernote/summernote.css" />--%>
    <!-- Stateface CSS -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/stateface/stateface.css" />
    <!-- Magnific popup -->
    <link rel="stylesheet" type="text/css" href="assets/js/plugins/magnific/magnific-popup.css" />
    <!-- c3charts -->
    <link rel="stylesheet" type="text/css" href="assets/js/plugins/c3charts/c3.min.css" />
    <!-- CSS - allcp forms -->
    <link rel="stylesheet" type="text/css" href="assets/allcp/forms/css/forms.css" />
    <!-- Plugins -->
    <link rel="stylesheet" type="text/css" href="assets/js/utility/malihu-custom-scrollbar-plugin-master/jquery.mCustomScrollbar.min.css" />
    <!-- CSS - theme -->
    <link rel="stylesheet" type="text/css" href="assets/skin/default_skin/less/theme.css" />
    <!-- Custom css -->
    <link href="assets/skin/css/custom.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="assets/fonts/zocial/zocial.css" rel="stylesheet" />
    <link href="assets/fonts/font-awesome/fontawesome.min.css" rel="stylesheet" />

    <!--JS Loader-->
    <script src="assets/loader/loader.js"></script>

    <!-- IE8 HTML5 support -->
    <!--[if lt IE 9]>-->
    <script src="assets/js/html5shiv.js"></script>
    <script src="assets/js/respond.min.js"></script>
    <script src="assets/js/Custom.js"></script>
    <style>
        .no-js #loader {
            display: none;
        }

        .js #loader {
            display: block;
            position: absolute;
            left: 100px;
            top: 0;
        }

        .se-pre-con {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url(assets/img/kiosk/loading.gif) center no-repeat #fff;
        }

        .progress {
            background-color: #e8e8e8;
        }

        svg g text {
            font-size: 11px !important;
        }
    </style>
 

    <%--<script src="assets/js/jquery/jquery-1.12.1.min.js"></script>--%>
   <%--<script src="assets/js/jquery/jquery_ui/jquery-ui.min.js"></script>--%>

   <script src="assets/js/jquery/jquery-3.5.1.min.js"></script>
   <script src="assets/js/bootstrap-4.0.min.css"></script>
   <script src="assets/js/bootstrap-4.0.min.js"></script>

   
   <script src="assets/js/modernizr.js"></script>
    <!--[endif]-->
    <script type="text/javascript">

        function printProcess(printMe) {
            var printWin = window.open("", "processPrint");

        }
        $(window).load(function () {
            // Animate loader off screen
            $(".se-pre-con").fadeOut("slow");
        });


    </script>

    <style>
        .logo-slogan {
            font-size: 14px !important;
        }

        .logo-image {
            text-align: center;
            font-size: 20px;
            text-decoration: none;
            margin-top: 5%;
            color: #fff;
        }

            .logo-image a:hover {
                text-decoration: none;
                color: #fff !important;
            }

        .table > tbody > tr > td {
            padding: 6px 5px;
        }

        @media print {

            body * {
                visibility: hidden;
            }

            #section-to-print, #section-to-print * {
                visibility: visible;
            }

            #section-to-print {
                margin: 5px;
            }
        }


        .ds0 {
            display: none;
        }

        .ds1 {
            display: none;
        }

        #chart_div g:first-child {
            /*display:none;*/
        }

        .sidebar-menu {
            padding-top: 100px !important;
        }

        .h-240 {
            height: 240px !important;
        }

        .b_table tbody tr th {
            background-color: #47d178 !important;
            position: sticky !important;
            z-index: 1 !important;
            top: 0 !important;
            color: #fff;
        }

        .b_table th {
            padding-left: 15px;
        }

        .parent_Table tbody tr {
            background-color: #32373a;
            color: white !important;
            border-radius: 100% !important;
        }

            .parent_Table tbody tr td {
                color: #fff;
            }

                .parent_Table tbody tr td a {
                    text-decoration: none;
                    color: #fff;
                }

        .b_table tbody tr:hover {
            color: #000 !important;
        }

        .list-group-item-heading {
            background-color: #0085ee !important;
        }

        .text-list span br {
            display: none !important;
        }

        .ilogo {
            height: 70px;
            width: 80px;
            background-color: white;
            padding: 10px;
            border-radius: 5px;
        }

        @media screen and (-ms-high-contrast: active), (-ms-high-contrast: none) {
            .w280 {
                width: 280px;
            }
        }
    </style>

    <script type="text/javascript">

        function btnRestartCommand(id) {

            var result = confirm("Want to Restart?");
            if (result) {
                //Logic to delete the item
                data = id;
              <%-- alert("<%= SendCommandToKiosk("restart" , "1")%>");--%>
                PageMethods.RestartCommand(data, OnSuccess, OnError);
            }



        }


        function OnSuccess(result) {
            alert(result);
        }
        function OnError() {
            alert('Some error has ocurred!');
        }


        //btnShutdownCommand
        function btnShutdownCommand(id) {

            var result = confirm("Want to Shutdown?");
            if (result) {
                data = id;
              <%-- alert("<%= SendCommandToKiosk("restart" , "1")%>");--%>
                PageMethods.ShutdownCommand(data, OnSuccess, OnError);
            }
        }



    </script>

    <script type="text/javascript">



        function FilterData() {
           
            var data = document.getElementById("filter").value;

            if (data != null && data) {
                var rows = $("#health_list ").find("li").hide();
                rows.filter(":contains('" + data + "')").show();


            }


        }


        function ClearFilterData() {
           
            document.getElementById("filter").value = "";
            $('.targetDiv').show();
            var divsToHide = document.getElementsByClassName("ds0"); //divsToHide is an array
            for (var i = 0; i < divsToHide.length; i++) {
                //   divsToHide[i].style.visibility = "hidden"; // or
                divsToHide[i].style.display = "none"; // depending on what you're doing
            }
        }

    </script>

    <script type="text/javascript">

        //avoid go back event..
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>
</head>




<body class="of-y-h widgets-tools basic-messages">

    <form id="form1" runat="server">
        <div id="main ">
            <!-- Header   -->
            <header class="navbar navbar-fixed-top " style="box-shadow: 0px 1px 15px 0px #424242;height:40px">
                <ul class="nav navbar-nav navbar-left">
                    <li class="dropdown dropdown-fuse text-white">
                        <div class="navbar-btn btn-group  phn"  data-toggle='tooltip' data-placement='left' title='Report'>
                            <button class="btn-hover-effects dropdown-toggle btn" data-toggle="dropdown" aria-expanded="false">
                                <span class="fa fa-file-text-o"></span>
                                                            
                            </button>
                            <ul class="dropdown-menu" role="menu" runat="server">
                                <li><span>Reports</span></li>
                                <li class="divider"></li>
                                <li><a href="TransactionReport.aspx" style="display: none;" class="text-white">Txn Report</a></li>
                                <li><a href="HealthReport.aspx" class="text-white">Health Report</a></li>
                                <li><a href="CustomizeReport.aspx" class="text-white">Transactions Report</a></li>
                                <li id="TicketReportID" runat="server"><a href="TicketCenter.aspx" class="text-white "><span ></span>Call Ticket's Report</a></li>                              
                                <li style="display: none;"><a href="UploadExcel.aspx" class="text-white">Upload Excel Data</a></li>
                            </ul>                           

                        </div>
                    </li>
                    <li class="" style="">
                        <div style="padding-top:5px" class="navbar-btn btn-group">
                            Reports
                        </div>

                    </li>

                    <li class="dropdown dropdown-fuse text-white" runat="server" id="AdminOptions">
                        <div class="navbar-btn btn-group  phn" data-toggle='tooltip' data-placement='left' title='Admin Tools'>
                            <button class="btn-hover-effects dropdown-toggle btn" data-toggle="dropdown" aria-expanded="false">
                                <span class="fa fa-magic fs20"></span>

                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li><span>Admin Tool</span></li>
                                <li class="divider"></li>
                                <li><a href="DataPull.aspx" class="text-white"><span class="fa fa-database mr10"></span>Data Pull</a></li>
                                <li><a href="PatchUpdate.aspx" class="text-white"><span class="fa fa-folder mr10"></span>Patch Update</a></li>
                                <%--<li><a href="TicketCenter.aspx" class="text-white "><span class="fa fa-cog mr10"></span>Ticket Details</a></li>--%>
                                <li><a href="BranchManage.aspx" class="text-white"><span class="fa fa-envelope mr10"></span>Branch Management</a></li>
                                <li><a href="KioskMasterReport.aspx" class="text-white"><span class="fa fa-envelope mr10"></span>Kiosk Master</a></li>
                                   <li><a href="KioskLocationType.aspx" class="text-white "><span class="fa fa-envelope mr10"></span>Kiosk Location Report</a></li>
                               <%--  <li><a href="Setting.aspx" class="text-white"><span class="fa fa-cog mr10"></span>Settings</a></li>
                                <li><a href="UserManage.aspx" class="text-white"><span class="fa fa-user mr10"></span>Users</a></li> --%>
                            </ul>
                        </div>
                        <div class="navbar-btn btn-group text-dark">
                            Admin Tool
                        </div>
                    </li>




                </ul>
                <div class="navbar-form navbar-left search-form square" role="search" style="display: none;">
                    <div class="input-group add-on">
                        <input type="text" class="form-control btn-hover-effects" placeholder="Search..." onfocus="this.placeholder=''"
                            onblur="this.placeholder='Search...'" />
                        <button class="btn btn-default text-info-darker  " type="submit">
                            <i class="glyphicon glyphicon-search"></i>
                        </button>
                    </div>
                </div>

                <ul class="nav navbar-nav navbar-right">


                    <li class="dropdown dropdown-fuse navbar-user">
                       
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                            <img class="btn-hover-effects" src="assets/img/avatars/user.jpg" alt="avatar" />
                            <span class="hidden-xs">
                                 <span class="name  text-uppercase"><%= Session["Username"]  %></span>
                            </span>
                            <span class="fa fa-caret-down hidden-xs"></span>
                        </a>

                        <%--    <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                <img class="btn-hover-effects" src="assets/img/avatars/user.jpg" alt="avatar" />
                                <span class="hidden-xs" style="margin-right:10px;">
                                    <span class="name  text-uppercase"><%= Session["Username"]  %></span>
                                </span>
                            </a>
 --%>                      
                        <ul class="dropdown-menu list-group keep-dropdown w230" role="menu">

                            <li class="list-group-item" style="">
                                <span class="fa fa-user"></span>
                                <a href="#" class="">
                                    <asp:Label ID="lblUserLocation" runat="server" Text=""></asp:Label>
                                </a>
                            </li>

                            <li class="dropdown-footer text-center">
                                <a href="About.aspx" class="btn btn-warning">About / Contact
                                </a>
                            </li>


                            <li class="dropdown-footer text-center">
                                <a href="Logout.aspx" class="btn btn-warning">logout
                                </a>
                            </li>
                            <li class="dropdown-footer text-center">
                                <a href="#" class="btn btn-warning">RMS 12.6.1.9
                                </a></li>
                        </ul>
                    </li>

                    <li>
                        <img src="assets/img/kiosk/BankLogo.png" alt="" style="height: 30px; width: 90px;  margin-right: 0px" />
                    </li>
                </ul>




            </header>
            <!-- /Header -->
            <!-- Sidebar  -->
            <aside id="sidebar_left" class="nano affix">
                <!-- Sidebar Left Wrapper  -->
                <div class="sidebar-left-content nano-content">
                    <!-- Sidebar Header -->
                    <header class="sidebar-header navbar-fixed-top w280">
                        <!-- Sidebar - Logo -->
                        <div class="sidebar-widget logo-widget">
                            <div class="media">
                                <a class="logo-image " href="Dashboard-V3_1.aspx" style="">
                                    <img src="assets/img/kiosk/LipiLogo.png" class="ilogo"/>
                                </a>
                                <div class="logo-slogan">
                                    <span class="text-white">Remote Monitoring System</span>
                                </div>
                            </div>
                        </div>
                    </header>
                    <!-- /Sidebar Header -->
                    <!-- Sidebar Menu  -->



                    <ul class="nav sidebar-menu">
                        <li class="sidebar-label pt25 pb20 text-center"></li>
                        <li class="sidebar-stat">
                            <!--client status-->
                            <a href="#" class="fs11 pln" style="background-color:transparent">
                                <span class="fs13 pl35  text-info"></span>
                                <span class="sidebar-title text-muted text-uppercase">Machine Status - Today</span>
                                <span class="pull-right mr30 text-muted"></span>
                                <div id="donutchart1" class="" style="height: 200px; width: 100%"></div>
                            </a>
                        </li>

                        <!--txn status-->
                        <!--   <li class="sidebar-stat pt10">
                          
                            <a href="#" class="fs11 pln">
                                <span class="fs13 pl35 text-info"></span>
                                <span class="sidebar-title text-muted text-uppercase">Txn Status - Today</span>
                                <span class="pull-right mr30 text-muted"></span>
                                <div id="donutchart2" class="" style="height: 200px; width: 100%"></div>

                            </a>
                        </li>  -->


<%--                    </ul>
                    <ul style="list-style-type:none;"  >--%>
                       
                        <li class="sidebar-stat pt10">
                            <a href="#" class="fs11 pln " style="background-color:transparent">
                                <span class="fs13 pl35 text-info"></span>
                                <b ><span class="sidebar-title text-muted text-uppercase ">Txn Status - Today</span></b>
                                <span class="pull-right mr30 text-muted"></span>
                                <div class="sidebar-title text-muted fs12">
                                    <asp:Label ID="lbl_txn_error" runat="server" Text=""></asp:Label>
                                </div>
                                <div id="donutchart2" class="" style="height: 200px; width: 100%"></div>
                            </a>
                        </li>
                       
                    </ul>
                     
                    <!-- /Sidebar Menu  -->
                </div>
                <!-- /Sidebar Left Wrapper  -->
            </aside>
            <!-- /Sidebar -->
            <!-- Main Wrapper -->

            <section id="content_wrapper">

                <!-- Content -->
                <section id="content" class="table-layout animated fadeIn">

                    <!-- Column Center -->
                    <div class="chute chute-center ">
                        <div class="chute-icon"></div>
                        <div class="chute-container">
                            <div class="chute-scroller" >

                                <div class="col-sm-4 col-md-4 col-lg-4 " style="display: none">
                                    <h6>Kiosk Health &amp; Txn Details</h6>
                                </div>

                                <div class="col-sm-4 col-md-4 col-lg-4" style="display: none">
                                    <input type="search" id="filter" class="form-control" placeholder="Search" />

                                    <%-- <input type="search" id="accordion_search_bar" class="form-control" onkeydown="return (event.keyCode!=13);" placeholder="Search" />--%>
                                </div>

                                <div class="col-sm-2 col-md-2 col-lg-2" style="display: none">
                                    <input id="searchButton" class="btn btn-flat btn-success form-control" onclick="FilterData();" type="button" value="Search" />
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2" style="display: none">
                                    <input id="ClearButton" class="btn btn-flat btn-warning form-control" onclick="ClearFilterData();" type="button" value="Clear" />
                                </div>


                                <div class="col-md-12 col-sm-12 col-md-offset-5" style="display: block;">

                                    <a id="showall" href="#">
                                        <!--menu-open-->

                                        <span class="badge badge-dark pull-left mt5 mr5 hide"><strong>All LHO List</strong></span>
                                    </a>
                                </div>


                                <div class="row text-center" style="font-size: 12px; text-align: left; font-family: sans-serif;margin-top:1%;">
                                    <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3"><strong>LHO Name         </strong></div>
                                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1"><strong>Total Machines   </strong></div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2"><strong>Connected        </strong></div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2"><strong>Dis-connected    </strong></div>
                                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1"><strong>Total Txn        </strong></div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2"><strong>Successful Txn   </strong></div>
                                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1"><strong>Failure Txn      </strong></div>
                                </div>


                                <div class="panel listgroup-widget pd0" style="padding: 0px">
                                    <div class="list-group ">
                                        <asp:DataList ID="DL_LHO_List" runat="server" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <div class="row list-group-item-heading animated fadeIn  text-center" style="margin: 0px !important">
                                                    <div class="col-xs-2 col-sm-3 col-md-3 col-lg-3">
                                                        <span><%# Eval("Circle") %></span>
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-1">
                                                        <span class="badge badge-default fs14 text-dark-darker  mt5" style ="background-color:#dfe2e5" >
                                                            <asp:LinkButton ID="TotMc" CssClass="fs14 text-dark-darker" Style="text-decoration: none !important;" runat="server"
                                                                Text='<%# Eval("TotalMc") %>'
                                                                CommandName='TotMc'
                                                                CommandArgument='<%#Bind("CircleID") %> '
                                                                OnCommand="TotMc_Command">
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2 col-md-1 col-lg-2">
                                                        <span class="badge badge-default fs14 text-dark-darker mt5" style="background-color:#dfe2e5">
                                                            <asp:LinkButton ID="ConMc" CssClass="fs14 text-dark-darker" Style="text-decoration: none !important;" runat="server"
                                                                Text='<%# Eval("Connected") %>'
                                                                CommandName='ConMc'
                                                                CommandArgument='<%#Bind("CircleID") %>'
                                                                OnCommand="ConMc_Command">
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                                                        <span class="badge badge-default fs14 text-dark-darker mt5" style="background-color:#dfe2e5">
                                                            <asp:LinkButton ID="DisMc" CssClass="fs14 text-dark-darker" Style="text-decoration: none !important;" runat="server"
                                                                Text='<%# Eval("Disconnected") %>'
                                                                CommandName='DisMc'
                                                                CommandArgument='<%#Bind("CircleID") %>'
                                                                OnCommand="DisMc_Command">
                                                            </asp:LinkButton>
                                                        </span>
                                                    </div>
                                                    <div class="col-xs-2 col-sm-1 col-md-1 col-lg-1">
                                                        <span class="badge badge-default fs14 text-dark-darker mt5" style="background-color:#dfe2e5"><%# Eval("TotalTxn") %></span>
                                                    </div>
                                                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-2">
                                                        <span class="badge badge-default fs14 text-dark-darker mt5 " style="background-color:#dfe2e5"><%# Eval("SuccessTxn") %> </span>
                                                    </div>
                                                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                                                        <span class="badge badge-default fs14 text-dark-darker mt5 " style="background-color:#dfe2e5"><%# Eval("FailTxn") %> </span>
                                                    </div>
                                                </div>

                                            </ItemTemplate>
                                        </asp:DataList>
                                    </div>
                                </div>
                                <!-- Scroll Demo -->
                            </div>
                        </div>
                    </div>
                    <!-- /Column Center -->

                    <!-- Column right -->
                    <aside class="chute chute-right chute300 chute-icon-style " style="background-color: #32373a; box-shadow: 0px 1px 15px 0px #424242;">
                        <div class="chute-icon"></div>
                        <div class="chute-container">
                            <div class="chute-scroller">
                                <%--<ul>
                                    <li class="sidebar-stat pt10">
                                        <a href="#" class="fs11 pln">
                                            <span class="fs13 pl35 text-info"></span>
                                            <span class="sidebar-title text-muted text-uppercase">Txn Status - Today</span>
                                            <span class="pull-right mr30 text-muted"></span>
                                            <div id="donutchart2" class="" style="height: 200px; width: 100%"></div>
                                        </a>
                                    </li>
                                </ul>--%>
                                
                                <%--<span class="sidebar-title text-muted fs12">
                                       <asp:Label ID="lbl_txn_error" runat="server" Text=""></asp:Label>
                                </span>--%>

                                <div class="col-md-7 col-sm-6 hide" style="">
                                    <h5>Transaction Details   </h5>
                                </div>
                                <div class="col-md-12 text-center" style="">
                                       <b><span class="sidebar-title text-muted text-center fs14">Last 10 days transaction details 

                                </span></b>
                                </div>
                             

                                <asp:DropDownList ID="ddl_transactionDays"
                                    OnSelectedIndexChanged="ddl_transactionDays_SelectedIndexChanged" AutoPostBack="true"
                                    CssClass="form-control hide" runat="server">
                                    <asp:ListItem Text="Last 7 days" Value="7"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="Last 10 days" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Last 15 days" Value="15"></asp:ListItem>
                                    <asp:ListItem Text="Last 30 days" Value="30"></asp:ListItem>
                                </asp:DropDownList>

                                <div class="panel listgroup-widget" style="margin-top: 25px; padding: 0px">
                                    <ul class="list-group text-list">
                                        <li class="list-group-item " style="padding-bottom: 25px !important"><span class="badge badge-dark pull-left mb10" style="background-color:#2a3342">Day  /  Date  </span><span class="badge badge-dark mb10 pull-right" style="background-color:#2a3342">Transactions</span></li>
                                        <asp:DataList ID="dataListTransaction" runat="server" RepeatLayout="Flow">
                                            <ItemTemplate>
                                                <li class="list-group-item"><%--<span class="pn"><%# Eval("Sno") %> - --%><%# Eval("Date") %></span><span class="badge badge-dark pull-right" style="margin-right:15%; background-color:#2a3342"><%# Eval("LastTransaction") %></span></li>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ul>
                                </div>
                                <!-- Scroll Demo -->
                            </div>
                        </div>
                    </aside>
                    <!-- /Column right -->
                </section>
                <!-- /Content -->
            </section>
        </div>

        <!-- jQuery -->
       

        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
     
        <asp:Timer ID="Timer1" runat="server" Interval="60000" OnTick="Timer1_Tick"></asp:Timer>
        <%--10min--%>
        <%--//10min--%>
    

    </form>


    <script type="text/javascript">

        function showDetails(ID, e1) {
            // alert("kioskID:" + ID + "and data is :"+ e1);

            document.getElementById("pop_serial_number").innerHTML = e1.getAttribute("data-serial_number");
            document.getElementById("pop_contact_person_name").innerHTML = e1.getAttribute("data-contact_person_name");
            document.getElementById("pop_contact_person_number").innerHTML = e1.getAttribute("data-contact_person_number");
            document.getElementById("pop_contact_person_email").innerHTML = e1.getAttribute("data-contact_person_email");
            document.getElementById("pop_KSID").innerHTML = e1.getAttribute("data-ksid");
            document.getElementById("pop_ksip").innerHTML = e1.getAttribute("data-ksip");
            document.getElementById("pop_brname").innerHTML = e1.getAttribute("data-brname");
            //document.getElementById("pop_health_time").innerHTML = e1.getAttribute("data-health_time");
            document.getElementById("pop_pb_status").innerHTML = e1.getAttribute("data-pb_status");
            document.getElementById("pop_ink_status").innerHTML = e1.getAttribute("data-ink_status");
        }

    </script>

    <%--js for submit issue--%>
    <script type="text/javascript">

        function ClearData(ID, e1) {           

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
            document.getElementById("Health_Time").innerHTML = e1.getAttribute("data-health_time");
            document.getElementById("PB_Status").innerHTML = e1.getAttribute("data-pb_status");
            document.getElementById("INK_Status").innerHTML = e1.getAttribute("data-ink_status");
        }

    </script>

    <script>


        $(function () {
            $('#showall').click(function () {

                $('.targetDiv').show();
                var divsToHide = document.getElementsByClassName("ds0"); //divsToHide is an array
                for (var i = 0; i < divsToHide.length; i++) {

                    divsToHide[i].style.display = "none"; // depending on what you're doing
                }


            });
            $('.showSingle').click(function () {
                $('.targetDiv').hide();
                $('.div' + $(this).attr('target')).show();
            });
        });


    </script>




    <!--canvas-->

    <%-- <script src="assets/js/canvasjs.min.js"></script>--%>
    <!-- AnimatedSVGIcons -->
    <script src="assets/fonts/animatedsvgicons/js/snap.svg-min.js"></script>
    <script src="assets/fonts/animatedsvgicons/js/svgicons-config.js"></script>
    <script src="assets/fonts/animatedsvgicons/js/svgicons.js"></script>
    <script src="assets/fonts/animatedsvgicons/js/svgicons-init.js"></script>
    <!-- HighCharts Plugin -->
    <script src="assets/js/plugins/highcharts/highcharts.js"></script>
    <!-- Scroll -->
    <script src="assets/js/utility/malihu-custom-scrollbar-plugin-master/jquery.mCustomScrollbar.concat.min.js"></script>
    <!-- Summernote -->
    <%--    <script src="assets/js/plugins/summernote/summernote.min.js"></script>--%>
    <!-- Magnific Popup Plugin -->
    <script src="assets/js/plugins/magnific/jquery.magnific-popup.js"></script>
    <!-- Theme Scripts -->
    <script src="assets/js/utility/utility.js"></script>
    <script src="assets/js/demo/demo.js"></script>
    <script src="assets/js/main.js"></script>
    <script src="assets/js/demo/widgets_sidebar.js"></script>
    <script src="assets/js/demo/widgets.js"></script>
    <script src="assets/js/pages/dashboard_init.js"></script>
    <script src="assets/js/fontawesome.min.js"></script>

    <%--    <script src="assets/js/pages/basic-messages.js"></script>--%>
    <%--    <script src="assets/js/pages/basic-messages.js"></script>--%>

    <!-- /Scripts -->

    <!-- custom script-->
    <script type="text/javascript">
        google.charts.load("current", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawChart1);
        
        function drawChart1() {
           
            var data = google.visualization.arrayToDataTable([
                ['Task', 'total'],
                ['Connected',      <%= connected %>],
                ['Disconnected',  <%= Disconnected %>],

            ]);

            var options = {

                title: '',
                startAngle: 180,
                pieSliceBorderColor: "none",
                tooltip: { trigger: 'selection' },
                legend: {
                    position: 'top', alignment: 'start',
                    textStyle: {
                        color: '#fff'
                    }
                },
                pieHole: 0.7,
                colors: ['#51cda0', '#6d78ad'],

                backgroundColor:
                    { fill: "#32373a" },

                pieSliceTextStyle: {
                    fontSize: 0.5
                }
            };

            var chart = new google.visualization.PieChart(document.getElementById('donutchart1'));
            chart.draw(data, options);
        }



    </script>

    <script type="text/javascript">
        google.charts.load("current", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawChart2);
        function drawChart2()
        {           
            var data = google.visualization.arrayToDataTable([
                ['Task', 'total'],
                ['Success', <%= tot_Succ_Txn %>],
                ['Failed',    <%= tot_fail_Txn %>],

            ]);

            var options = {

                title: '',

                startAngle: 180,
                pieSliceBorderColor: "none",
                tooltip: { trigger: 'selection' },
                legend: {
                    position: 'top', alignment: 'start',
                    textStyle: {
                        color: '#fff'
                    }
                },
                pieHole: 0.7,
                colors: ['#51cda0', '#6d78ad'],

                backgroundColor:
                    { fill: "#32373a" },
                pieSliceTextStyle: {
                    fontSize: 0.5
                }
            };

            var chart = new google.visualization.PieChart(document.getElementById('donutchart2'));
            chart.draw(data, options);
        }



    </script>

</body>
</html>
