<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <title>OneRMS</title>
    <meta name="keywords" content=""/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

    <link rel="shortcut icon" href="assets/img/Lipi Blue.png"/>

    <!-- Angular material -->
    <link rel="stylesheet" type="text/css" href="assets/skin/css/angular-material.min.css"/>
    
    <!-- Icomoon -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/icomoon/icomoon.css"/>    
    
    <!-- AnimatedSVGIcons -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/animatedsvgicons/css/codropsicons.css"/>

    <!-- CSS - allcp forms -->
    <link rel="stylesheet" type="text/css" href="assets/allcp/forms/css/forms.css"/>

    <!-- Plugins -->
    <link rel="stylesheet" type="text/css" href="assets/js/utility/malihu-custom-scrollbar-plugin-master/jquery.mCustomScrollbar.min.css"/>

    <!-- CSS - theme -->
    <link rel="stylesheet" type="text/css" href="assets/skin/default_skin/less/theme.css"/>

      <script type="text/javascript">

        //avoid go back event..
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
     </script>

    <style>
        .ilogo {
            height: 70px;
            width: 80px;
            background-color: white;
            padding: 10px;
            border-radius: 5px;
        }
    </style>


  
</head>
<body class="utility-page sb-l-c sb-r-c" style="position:relative">
    <form id="form1" runat="server" style="height:100%">
        <div id="main" class="animated fadeIn">

    <!-- Main Wrapper -->
    <section id="content_wrapper">

        <%--<div id="canvas-wrapper">
            <canvas id="demo-canvas"></canvas>
        </div>--%>
         <div class="text-center mb20 br3 pt15 pb10" style="position:relative;width:100%;background-color:transparent">
                    <img src="assets/img/output-onlinepngtools.png" alt="" style="position:absolute;right:0px;top:0px;height:150px" draggable="false"/>
                </div>
        <!-- Content -->
        <section id="content" >
            
            <!-- Login Form -->
            <div class="allcp-form theme-primary mw600" >
               <%-- <div class="bg-primary mw600 text-center mb20 br3 pt15 pb10" style="position:absolute;right:0px">
                    <img src="assets/img/output-onlinepngtools.png" alt=""/>
                </div>--%>
                <div class="panel mw600">

                    
                        <div class="panel-body pn mv10" >

                            <div class="section">
                                <label for="username" class="field prepend-icon">
                                   <img src="assets/img/logout.png" draggable="false" id="errorimg" runat="server"></img>
                                </label>
                            </div>
                            <!-- /section -->

                            <div class="section">
                                <label for="password" class="field prepend-icon">
                                 Click to  <a href="Default.aspx" style="">Sign In Again  </a>
                                </label>
                            </div>
                            <!-- /section -->
                            <%--<div class="section">
                                <asp:Image runat="server" ID="CapImage" ImageUrl="~/Captcha.aspx" Height="40px" Width="100%" draggable="false" />
                                <asp:TextBox runat="server" ID="capCode" placeholder="Enter Captcha" class="gui-input" AutoComplete="off"></asp:TextBox>
                            </div>--%>

                            <%--<div class="section">
                                
                                        <a href="PasswordRecovery.aspx" style="">Forgot Password</a>
                                   
                                <asp:Button runat="server" ID="Login" Text="Login" OnClick="Login_Click" class="btn btn-bordered btn-primary pull-right" OnClientClick="return Validation();" />
                                <br />
                                <br /><br />
                                <asp:Label runat="server" ID="errLab" style="display:none;" CssClass="label label-danger"></asp:Label>
                                <%--<button type="submit" >Log in</button>
                            </div>--%>
                            <!-- /section -->

                        </div>
                        <!-- /Form -->
                    
                </div>
                <!-- /Panel -->
            </div>
            <!-- /Spec Form -->

        </section>
        <!-- /Content -->
        
    </section>
    <!-- /Main Wrapper -->
           
            
</div>
        
        
<%--<script src="assets/js/jquery/jquery-1.12.3.min.js"></script>
<script src="assets/js/jquery/jquery_ui/jquery-ui.min.js"></script>--%>
  <script src="assets/js/jquery/jquery-3.5.1.min.js"></script>
  <script src="assets/js/bootstrap-4.0.min.css"></script>
  <script src="assets/js/bootstrap-4.0.min.js"></script>

<!-- AnimatedSVGIcons -->
<script src="assets/fonts/animatedsvgicons/js/snap.svg-min.js"></script>
<script src="assets/fonts/animatedsvgicons/js/svgicons-config.js"></script>
<script src="assets/fonts/animatedsvgicons/js/svgicons.js"></script>
<script src="assets/fonts/animatedsvgicons/js/svgicons-init.js"></script>

<!-- Scroll -->
<script src="assets/js/utility/malihu-custom-scrollbar-plugin-master/jquery.mCustomScrollbar.concat.min.js"></script>
<!-- Summernote -->



<!-- HighCharts Plugin -->
<script src="assets/js/plugins/highcharts/highcharts.js"></script>
<script src="assets/js/plugins/canvasbg/canvasbg.js"></script>
<script src="assets/js/utility/utility.js"></script>
<%--<script src="assets/js/demo/demo.js"></script>--%>
<script src="assets/js/main.js"></script>
<script src="assets/js/demo/widgets_sidebar.js"></script>
<script src="assets/js/pages/dashboard_init.js"></script>
    </form>
     <footer class="main-footer" style="position: absolute; height: 65px; margin-left: 0px; color: #03a9f4; bottom: 0px; width: 100%; opacity: 0.8; background-color: whitesmoke; -webkit-transition: none; -o-transition: none; transition: none; padding-top: 0px;">
                <div class="pull-right  hidden-xs image" style="height: 50px; padding: 5px 30px; /* background-color: white; */">
                    <img style="height: 50px; width: 50px;" src="assets/img/kiosk/LipiLogo.png" draggable="false">
                </div>
                <div style="text-align: center; padding-left: 7%; padding-top: 2%;">
                    <strong>All Rights Reserved By <u>Lipi Data Systems Ltd.</u></strong> 
                </div>
    </footer>
</body>
</html>
