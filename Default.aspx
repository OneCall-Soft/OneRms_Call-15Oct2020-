<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>AUOneRMS</title>
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="shortcut icon" href="assets/img/Lipi Blue.png" />

    <!-- Angular material -->
    <link rel="stylesheet" type="text/css" href="assets/skin/css/angular-material.min.css" />

    <!-- Icomoon -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/icomoon/icomoon.css" />

    <!-- AnimatedSVGIcons -->
    <link rel="stylesheet" type="text/css" href="assets/fonts/animatedsvgicons/css/codropsicons.css" />

    <!-- CSS - allcp forms -->
    <link rel="stylesheet" type="text/css" href="assets/allcp/forms/css/forms.css" />

    <!-- Plugins -->
    <link rel="stylesheet" type="text/css" href="assets/js/utility/malihu-custom-scrollbar-plugin-master/jquery.mCustomScrollbar.min.css" />

    <!-- CSS - theme -->
    <link rel="stylesheet" type="text/css" href="assets/skin/default_skin/less/theme.css" />

    <script type="text/javascript" src="assets/js/Custom.js"></script>
    <script type="text/javascript">

        //avoid go back event..
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>


     <script type="text/javascript">
        var isSubmitted = false;
        function GetName() {
            var newDate = new Date();
            if (!isSubmitted) {
                var username = document.getElementById('<%= txtUserName.ClientID%>').value;
                document.getElementById('<%= txtUserName.ClientID %>').value = data(username + "&" + newDate.getTime());
                var password = document.getElementById('<%= txtPassword.ClientID%>').value;
                document.getElementById('<%= txtPassword.ClientID %>').value = data(password + "*" + newDate.getTime());

                isSubmitted = true;

                return true;
            }
            else { return false; }
        }

    </script>

    <%-- <script type="text/javascript">
     function data(s)
        {
            var key = CryptoJS.enc.Utf8.parse('1020230803070908');
            var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

            var encString = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(s), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
                });

            return encString;
         }
           </script>--%>


     <script type="text/javascript">
        function data(s) {
            var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
            var encodedString = Base64.encode(s);
            return encodedString;
        }
    </script>

</head>
<body class="utility-page sb-l-c sb-r-c" style="position: relative">
    <form id="form1" runat="server" style="height: 100%" autocomplete="off">
        <div id="main" class="animated fadeIn">

            <!-- Main Wrapper -->
            <section id="content_wrapper">

                <%--<div id="canvas-wrapper">
            <canvas id="demo-canvas"></canvas>
        </div>--%>

                <!-- Content -->
                <section id="content">

                    <!-- Login Form -->
                    <div class="allcp-form theme-primary mw400" style="margin-top: 0%">
                        <%-- <div class="bg-primary mw600 text-center mb20 br3 pt15 pb10" style="position:absolute;right:0px">
                    <img src="assets/img/output-onlinepngtools.png" alt=""/>
                </div>--%>

                        <div class="panel mw400">
                            <div style="text-align: center; background-color: white; padding: 5px 0px;">
                                <img src="assets/img/kiosk/BankLogo.png" alt="" style="height: 40px;"
                                    draggable="false" />
                            </div>
                            <h4>Login</h4>

                            <div class="panel-body pn mv10">
                                <div id="Div_SSOBody" runat="server">
                                 <div class="col-sm-6"><strong>PF ID :</strong></div>
                                <div class="col-sm-6">
                                    <asp:Label ID="lbl_PF" CssClass="text-dark-darker" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="col-sm-6"><strong>Employee Name :</strong></div>
                                <div class="col-sm-6">
                                    <asp:Label ID="lbl_name" CssClass="text-dark-darker" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="col-sm-6"><strong>Branch Code :</strong></div>
                                <div class="col-sm-6">
                                    <asp:Label ID="lbl_code" CssClass="text-dark-darker" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="col-sm-6"><strong>Mobile Number :</strong></div>
                                <div class="col-sm-6">
                                    <asp:Label ID="lbl_Mobile" CssClass="text-dark-darker" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="col-sm-6"><strong>Email ID :</strong></div>
                                <div class="col-sm-6">
                                    <asp:Label ID="lbl_Email" CssClass="text-dark-darker" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="clearfix"></div>

                                     <div class="col-sm-4"><strong></strong></div>
                                <div class="col-sm-8">
                                    <asp:Button ID="btn_login" CssClass="form-control btn-success" runat="server" Text="Login" OnClick="btn_login_Click" />

                                </div>
                                <div class="clearfix"></div>
                                <br />



                                </div>


                                 <div id="Div_UserLogin" runat="server">
                                <div class="col-sm-4" style=" margin-top: 4.0%;">
                                    <strong id="user1" runat="server">UserName :</strong></div>
                                <div class="col-sm-8">                                   
                                    <asp:TextBox ID="txtUserName" class="form-control" runat="server" placeholder="User Name" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="clearfix"></div>
                                <br />

                                 <div class="col-sm-4" style=" margin-top: 4.0%;">
                                     <strong id="pass1" runat="server">Password :</strong></div>
                                <div class="col-sm-8">
                                  <asp:TextBox ID="txtPassword" class="form-control" runat="server" TextMode="Password"  placeholder="Password" autocomplete="off"></asp:TextBox>

                                </div>
                                <div class="clearfix"></div>
                                <br />

                             <div class="col-sm-4"><strong></strong></div>
                                <div class="col-sm-8">
                                    <asp:Button ID="btn_login1" runat="server" Text="Login" OnClientClick="return GetName();" CssClass="form-control btn-success"  OnClick="btn_login_Click" />

                                  

                                </div>
                                <div class="clearfix"></div>

                                      
                   






                                </div>
                               
                                
                                <br />



                                <br />

                                 

                              
                                <div class="col-sm-12">
                                    <strong>
                                        <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label></strong>
                                </div>
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
            <footer class="main-footer" style="position: absolute; height: 65px; margin-left: 0px; color: #03a9f4; bottom: 0px; width: 100%; opacity: 0.8; background-color: whitesmoke; -webkit-transition: none; -o-transition: none; transition: none; padding-top: 0px;">
                <div class="pull-right  hidden-xs image" style="height: 50px; padding: 5px 30px; /* background-color: white; */">
                    <img style="height: 50px; width: 50px;" src="assets/img/kiosk/LipiLogo.png" draggable="false">
                </div>
                <div style="text-align: center; padding-left: 7%; padding-top: 2%;">
                    <strong>All Rights Reserved By <u>Lipi Data Systems Ltd.</u></strong> Version 12.6.1.11
                </div>
            </footer>

        </div>

        <%--<script src="assets/js/jquery/jquery-1.12.1.min.js"></script>--%>
        <%--<script src="assets/js/jquery/jquery_ui/jquery-ui.min.js"></script>--%>
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

        <script src="assets/js/main.js"></script>
        <script src="assets/js/demo/widgets_sidebar.js"></script>

    </form>

</body>
</html>
