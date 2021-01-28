<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordRecovery.aspx.cs" Inherits="PasswordRecovery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>OneRMS</title>
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

    <script type="text/javascript">

        function GetName() {

            var user = document.getElementById('<%= UserID.ClientID%>').value;
             document.getElementById('<%= UserID.ClientID %>').value = data(user);

             var Sanswer = document.getElementById('<%= answer.ClientID%>').value;
             document.getElementById('<%= answer.ClientID %>').value = data(Sanswer);

         }
    </script>

    <script type="text/javascript">
        function data(s) {
            var Base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }
            var encodedString = Base64.encode(s);
            return encodedString;
        }
    </script>

    <script type="text/javascript">
        function Validation() {
         <%--     if (document.getElementById('<%=UserID.ClientID%>').value == "" || document.getElementById('<%=answer.ClientID%>').value == "" ||document.getElementById('<%=capCode.ClientID%>').value=="" || document.getElementById('<%=useremail.ClientID%>').value=="" ||document.getElementById('<%=recoveryQuestion.ClientID%>').selectedIndex == 0 ) {
           
                   document.getElementById('<%=UserID.ClientID%>').value = ""; document.getElementById('<%=answer.ClientID%>').value = "";
                  document.getElementById('<%=capCode.ClientID%>').value = "";document.getElementById('<%=useremail.ClientID%>').value = "";
                  document.getElementById('<%=recoveryQuestion.ClientID%>').selectedIndex = 0;
                   return false;
               }--%>

            return true;
        }
        function Error(error) {
    <%--      document.getElementById('<%=errLab.ClientID%>').innerHTML = error;
                document.getElementById('<%=errLab.ClientID%>').style.display = "block";--%>
            document.getElementById('<%=UserID.ClientID%>').value = ""; document.getElementById('<%=answer.ClientID%>').value = "";
            document.getElementById('<%=capCode.ClientID%>').value = ""; document.getElementById('<%=useremail.ClientID%>').value = "";
               document.getElementById('<%=recoveryQuestion.ClientID%>').selectedIndex = 0;
        }
        function Captch(error) {
    <%--        document.getElementById('<%=errLab.ClientID%>').innerHTML = error;
            document.getElementById('<%=errLab.ClientID%>').style.display = "block";--%>
            document.getElementById('<%=capCode.ClientID%>').value = "";
        }
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>

    <script>
        window.oncontextmenu = function () {
            return false;
        }
        $(document).keydown(function (event) {
            if (event.keyCode == 123) {
                return false;
            }
            else if ((event.ctrlKey && event.shiftKey && event.keyCode == 73) ||
                     (event.ctrlKey && event.shiftKey && event.keyCode == 74)) {
                return false;
            }
        });
    </script>
</head>
<body class="utility-page sb-l-c sb-r-c" style="position: relative;">
    <form id="form1" runat="server" style="height: 100%" autocomplete="off">
        <div id="main" class="animated fadeIn">

            <!-- Main Wrapper -->
            <section id="content_wrapper">

                <div class="text-center mb20 br3 pt15 pb10" style="position: relative; width: 100%; background-color: transparent">
                    <img src="assets/img/output-onlinepngtools.png" alt="" style="position: absolute; right: 0px; top: 0px; height: 150px" draggable="false" />
                </div>
                <!-- Content -->
                <section id="content" >

                    <!-- Login Form -->
                    <div class="allcp-form theme-primary mw450" style="margin-top:0%">
                    <%--    <div class="greeting-field" style="color: #281e74">
                            Password Reset
                        </div>--%>
                        <div class="panel panel-primary">
                            <h4> Password Reset</h4>

                            <div class="panel-body pn mv10">

                                <div class="section">
                                    <label for="username" class="field prepend-icon" style="color: red">
                                        <asp:TextBox runat="server" ID="UserID" draggable="false" oncopy="return false" onpaste="return false" oncut="return false" class="gui-input" AutoComplete="off" placeholder="Enter Username"></asp:TextBox>*
                                    <%--<input type="text" name="username" >--%>
                                        <span class="field-icon">
                                            <i class="fa fa-lock"></i>
                                        </span>
                                        <asp:RequiredFieldValidator ID="TxtChqNUMm" ControlToValidate="UserID" runat="server" ValidationGroup="GridValidate"
                                            ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                </div>
                                <!-- /section -->

                                <div class="section">
                                    <label class="field select" style="color: red">
                                        <asp:DropDownList runat="server" ID="recoveryQuestion">
                                            <asp:ListItem Text="Select Security Question"></asp:ListItem>
                                            <asp:ListItem Text="Who is your favourite cricketer?"></asp:ListItem>
                                            <asp:ListItem Text="Which is your favourite book author?"></asp:ListItem>
                                            <asp:ListItem Text="Who is you favourite actor?"></asp:ListItem>
                                            <asp:ListItem Text="What is your pets name?"></asp:ListItem>
                                            <asp:ListItem Text="Custom Text."></asp:ListItem>
                                        </asp:DropDownList>*
                                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="recoveryQuestion" ValidationGroup="GridValidate" InitialValue="Select Security Question" ErrorMessage="Please select Security Question" />
                                    </label>
                                </div>

                                <div class="section" id="spy3">
                                    <label for="comment" class="field prepend-icon" style="color: red">
                                        <asp:TextBox runat="server" TextMode="MultiLine" oncopy="return false" onpaste="return false" oncut="return false" draggable="false" CssClass="gui-textarea" ID="answer" AutoComplete="off" placeholder="Security Answer"></asp:TextBox>*
                                         <%-- <textarea class="gui-textarea" id="comment" name="comment"
                                                    placeholder="Your comment"></textarea>--%>
                                        <span class="field-icon">
                                            <i class="fa fa-comment"></i>
                                        </span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="answer" runat="server" ValidationGroup="GridValidate"
                                            ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                </div>

                                <div class="section">
                                    <label for="email5" class="field prepend-icon" style="color: red">
                                        <asp:TextBox runat="server" draggable="false" oncopy="return false" onpaste="return false" oncut="return false" ID="useremail" TextMode="Email" class="gui-input" AutoComplete="off" placeholder="Enter Registered Email ID"></asp:TextBox>*
                                                <span class="field-icon">
                                                    <i class="fa fa-envelope"></i>
                                                </span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="useremail" runat="server" ValidationGroup="GridValidate"
                                            ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                </div>
                                <!-- /section -->
                                <div class="section" style="color: red">
                                    <asp:Image runat="server" ID="CapImage" Height="40px" Width="45%" draggable="false" />&nbsp;&nbsp;<asp:ImageButton ImageUrl="~/assets/img/refresh.png" Style="margin-top: 10px; width: 5%; height: 20px" runat="server" draggable="false" ID="refresh" OnClick="refresh_Click" />
                                    <asp:TextBox runat="server" ID="capCode" draggable="false" oncopy="return false" onpaste="return false" oncut="return false" placeholder="Enter Captcha" class="gui-input" Width="45%" AutoComplete="off"></asp:TextBox>*
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="useremail" runat="server" ValidationGroup="GridValidate"
                                    ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>

                                <div class="section">

                                    <a href="Default.aspx">Cancel</a>

                                    <asp:Button runat="server" ID="Login" Text="Reset Password" OnClick="Login_Click" class="btn btn-bordered btn-primary pull-right" ValidationGroup="GridValidate" />
                                    <br />
                                    <br />
                                    <br />


                                    <%--<button type="submit" >Log in</button>--%>
                                </div>
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
            <footer class="main-footer" style="height: 35px; margin-left: 0px; color: #03a9f4; position: absolute; bottom: 0; width: 100%; opacity: 0.8; background-color: whitesmoke;">
                <div class="pull-right  hidden-xs image" style="height: 35px">
                    <img style="height: 35px; width: 100px;" src="assets/img/Lipi%20Blue.png" draggable="false" />
                </div>
                <div style="text-align: center; padding-left: 7%">
                    <strong>All Rights Reserved By <u>Lipi Data Systems Ltd.<u></strong>
                </div>
            </footer>

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
        <script src="assets/js/demo/demo.js"></script>
        <script src="assets/js/main.js"></script>
        <script src="assets/js/demo/widgets_sidebar.js"></script>
        <script src="assets/js/pages/dashboard_init.js"></script>
    </form>
</body>
</html>
