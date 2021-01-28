<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserManage.aspx.cs" Inherits="UserManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript" src="doc/js/jquery.1.6.4.js"></script>
    <script type="text/javascript" src="doc/js/aes.js"></script>
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
        function GetName()
        { 
             var user = htmlEncode(document.getElementById('<%= usernametext2.ClientID%>').value);
             document.getElementById('<%= usernametext2.ClientID %>').value = data(user);

             var pwd = htmlEncode(document.getElementById('<%= password.ClientID%>').value);
             document.getElementById('<%= password.ClientID %>').value = data(pwd);

             var Confirmpwd = htmlEncode(document.getElementById('<%= repeatPassword.ClientID%>').value);
             document.getElementById('<%= repeatPassword.ClientID %>').value = data(Confirmpwd);

             var Sans = htmlEncode(document.getElementById('<%= answer.ClientID%>').value);
             document.getElementById('<%= answer.ClientID %>').value = data(Sans);

             var Phone = htmlEncode(document.getElementById('<%= mobile_phone.ClientID %>').value);
             document.getElementById('<%= mobile_phone.ClientID %>').value = data(Phone);
         }
    </script>

      <script type="text/javascript">
        function htmlEncode(value)
        {  
            return $('<textarea/>').text(value).html();
        }
     </script>

     <script type="text/javascript">
        function htmlDecode(value)
        {
            return $('<textarea/>').html(value).text();
        }
     </script>

   <script  type="text/javascript">
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
    </script>

   
 
      <script>
        function ConfirmDelete() {
            if (confirm('Are you sure to delete?'))
                return true;
            else
                return false;
        }
    </script>

    <script type="text/javascript">
               window.onclick = function (event) {
                      if (event.target == document.getElementById('<%=myModal.ClientID%>')) {
                         document.getElementById('<%=myModal.ClientID%>').style.display = "none";
                          }
               }

               function validnumber(e) {
               
                   var unicode = e.keyCode ? e.keyCode : e.charCode
                   if (unicode >= 48 && unicode <= 57) {
                       return true;
                   }
                   else {
                       return false;
                   }
               }

                   function Validation() {
                  
                    var err = "";
                    if (document.getElementById('<%=UserType.ClientID%>').selectedIndex == 0)
                                err = "Select User Type";
                                //     else if (document.getElementById('<%=locationList.ClientID%>').selectedIndex == 0)
                                //   err = "Select User Location";
                            else if (document.getElementById('<%=usernametext2.ClientID%>').value == "")
                                err = "Enter User Name";
                            else if (document.getElementById('<%=password.ClientID%>').value == "")
                                err = "Enter Password";
                            else if (document.getElementById('<%=repeatPassword.ClientID%>').value == "")
                                err = "Enter Confirmation Password";
                            else if (document.getElementById('<%=repeatPassword.ClientID%>').value != document.getElementById('<%=password.ClientID%>').value)
                                err = "Password doesn't matched with Confirmation Password";
                            else if (document.getElementById('<%=recoveryQuestion.ClientID%>').selectedIndex == 0)
                                err = "Select Recovery Question";
                            else if (document.getElementById('<%=answer.ClientID%>').value == "")
                                err = "Enter Recovery Answer";
                            else if (document.getElementById('<%=useremail.ClientID%>').value == "")
                                err = "Enter User EmailId";
                        else if (document.getElementById('<%=mobile_phone.ClientID%>').value == "")
                                err = "Enter Mobile Number";
                    if (err != "") {
                        document.getElementById("error").innerHTML = err;
                        document.getElementById('<%=myModal.ClientID%>').style.display = "block";
                                    return false;
                                }
                                else {
                                    GetName();
                                    return true;
                                }
                            }

            function UpdateTextBox() {
                document.getElementById('<%=usernametext2.ClientID%>').className = "gui-input";
             document.getElementById('<%=usernametext2.ClientID%>').style.color = "black";
         }
         function UserSuccess() {
             document.getElementById("error").innerHTML = "User Details Stored successfully";
             document.getElementById('<%=myModal.ClientID%>').style.display = "block";
            document.getElementById('<%=usernametext2.ClientID%>').className = "gui-input";
            document.getElementById('<%=usernametext2.ClientID%>').style.color = "black";
        }
        function UserDeleteSuccess() {
            document.getElementById("error").innerHTML = "User deleted successfully.";
            document.getElementById('<%=myModal.ClientID%>').style.display = "block";
            document.getElementById('<%=usernametext2.ClientID%>').className = "gui-input";
            document.getElementById('<%=usernametext2.ClientID%>').style.color = "black";
        }
        function UserFail() {
            document.getElementById("error").innerHTML = "Failed To Save User Details";
            document.getElementById('<%=usernametext2.ClientID%>').className = "gui-input";
            document.getElementById('<%=myModal.ClientID%>').style.display = "block";
            document.getElementById('<%=usernametext2.ClientID%>').style.color = "black";
        }
        function UserFailToDelete() {
            document.getElementById("error").innerHTML = "Failed To Delete User Details";
            document.getElementById('<%=usernametext2.ClientID%>').className = "gui-input";
            document.getElementById('<%=myModal.ClientID%>').style.display = "block";
            document.getElementById('<%=usernametext2.ClientID%>').style.color = "black";
        }
        function UserAlert(error) {
            document.getElementById("error").innerHTML = error;
            document.getElementById('<%=usernametext2.ClientID%>').className = "gui-input";
            document.getElementById('<%=myModal.ClientID%>').style.display = "block";
            document.getElementById('<%=usernametext2.ClientID%>').style.color = "black";
        }
        history.pushState(null, null, location.href);
        window.onpopstate = function () {
            history.go(1);
        };
    </script>

    <script type="text/javascript">
        $(function() {
            var $txt = $('input[id$=txtNew]');
            var $ddl = $('select[id$=Userlist]');
            var $items = $('select[id$=Userlist] option');
 
            $txt.keyup(function() {
                searchDdl($txt.val());
            });
 
            function searchDdl(item) {
                $ddl.empty();
                var exp = new RegExp(item, "i");
                var arr = $.grep($items,
                    function(n) {
                        return exp.test($(n).text());
                    });
 
                if (arr.length > 0) {
                    countItemsFound(arr.length);
                    $.each(arr, function() {
                        $ddl.append(this);
                        $ddl.get(0).selectedIndex = 0;
                    }
                    );
                }
                else {
                    countItemsFound(arr.length);
                    $ddl.append("<option>No Items Found</option>");
                }
            }
 
            function countItemsFound(num) {
                $("#para").empty();
                if ($txt.val().length) {
                    $("#para").html(num + " items found");
                }
 
            }
        });
    </script>



    <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">User Management</li>
            </ol>
        </div>
    </header>
    <!-- Client Detail Container -->
    <section id="content" class="table-layout animated fadeIn" style="padding: 20px">
        <!-- UserList-->
        <aside class="" data-chute-height="" style="padding-left: 3%; background-color: #47d1af; width: 20%">

                <div class="chute-container">
                    <div class="chute-affix" data-spy="affix" data-offset-top="200">

                        <div id="nav-spy">
                            <h6 class="mb20">Existing User List</h6>
                            <%--<asp:BulletedList id="Userlist" runat="server" DisplayMode="LinkButton" OnClick="Userlist_Click" CssClass="nav chute-nav">
                                    <asp:ListItem Text="Create New" Selected="True"></asp:ListItem>                                
                                </asp:BulletedList>--%>
                            <div class="section row mhn10">

                        

                                <div class="col-md-12 ph10 mb5">

                                        <asp:TextBox ID="txtNew" runat="server" CssClass="form-control hide" ToolTip="Enter Text Here"></asp:TextBox>
                         
                                     <br />

                                    <label class="field select">
                                        <asp:DropDownList ID="Userlist" runat="server" Style="color: #a9a9ab; background: #fff; border-color: #dddfe3; padding: 15px 20px; font-size: 13px; font-family: 'Open Sans', sans-serif; border: 1px solid #dddfe3; border-radius: 5px; letter-spacing: 0.02em; outline: none; text-indent: 0.01px;" Width="100%" Height="50px" OnSelectedIndexChanged="Userlist_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </label>
                                </div>
                            </div>
                     
                        </div>

                    </div>
                </div>
        </aside>




        <div class="chute chute-center">
            <div class="allcp-form theme-primary mw1000 center-block pb175">
                <div class="panel">
                    <div class="panel-heading">
                       <div class="panel-title pn">User Detail Management</div>
                    </div>

                    <div class="panel-body">
                          <h6 class="mb20" id="spy1">Fill All Details</h6>
                          
                          <div class="section row mhn10">
                                    <div class="col-md-6 ">
                                        <label class="field select" style="z-index: 0">
                                            <asp:DropDownList runat="server" OnSelectedIndexChanged="UserType_SelectedIndexChanged" AutoPostBack="true" ID="UserType">
                                                <asp:ListItem Text="Select User Type"></asp:ListItem>
                                                <asp:ListItem Text="Branch User" Value="branch"></asp:ListItem>
                                                <asp:ListItem Text="Circle" Value="circle"></asp:ListItem>
                                                <%-- <asp:ListItem Text="Admin User" Value="Admin User"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </label>
                                    </div>
                                    <div class="col-md-6">
                                        <label class="field select" style="z-index: 0">
                                            <asp:DropDownList runat="server" ID="locationList" Enabled="false">
                                                <asp:ListItem Text="Select"></asp:ListItem>

                                            </asp:DropDownList>
                                        </label>
                                    </div>
                            </div>

                            <div class="section">
                                <label for="username2" id="lab" runat="server" class="field prepend-icon">
                                    <asp:TextBox runat="server" ID="usernametext2" ForeColor="Black" class="gui-input" AutoComplete="off" placeholder="Username"></asp:TextBox>
                                    <%--<input type="text" name="username2" id="username2" class="gui-input"
                                                               >--%>
                                    <span class="field-icon">
                                        <i class="fa fa-user"></i>
                                    </span>
                                </label>
                            </div>

                            <div class="section row mhn10">
                                <div class="col-md-6 ">
                                    <label for="password" class="field prepend-icon">
                                        <asp:TextBox runat="server" ID="password" placeholder="Create password" MinLength="8" MaxLength="14" AutoComplete="off" class="gui-input" TextMode="Password"></asp:TextBox>

                                        <%--<input type="password" name="password" id="password" class="gui-input"
                                                       placeholder="Create password">--%>
                                        <span class="field-icon">
                                            <i class="fa fa-lock"></i>
                                        </span>
                                    </label>
                                </div>
                                <div class="col-md-6 ">
                                    <label for="repeatPassword" class="field prepend-icon">
                                        <asp:TextBox runat="server" ID="repeatPassword" TextMode="Password" AutoComplete="off" MinLength="8" MaxLength="14" CssClass="gui-input" placeholder="Confirm Password"></asp:TextBox>
                                        <%--<input type="password" name="repeatPassword" id="repeatPassword"
                                                       class="gui-input"
                                                       placeholder="Repeat password">--%>
                                        <span class="field-icon">
                                            <i class="fa fa-unlock"></i>
                                        </span>
                                    </label>
                                </div>
                            </div>

                            <div class="section row mhn10">
                                <div class="col-md-6 ">
                                    <label for="useremail" class="field prepend-icon">
                                        <asp:TextBox runat="server" ID="useremail" TextMode="Email" class="gui-input" AutoComplete="off" placeholder="Email address"></asp:TextBox>
                                        <%--<input type="email" name="useremail" id="useremail" class="gui-input"
                                                           placeholder="Email address">--%>
                                        <span class="field-icon">
                                            <i class="fa fa-envelope"></i>
                                        </span>
                                    </label>

                                </div>
                                <div class="col-md-6 ">
                                    <label for="mobile_phone" class="field prepend-icon">
                                        <asp:TextBox runat="server" ID="mobile_phone" class="gui-input phone-group" placeholder="Mobile number" MaxLength="10" MinLength="10" AutoComplete="off" onkeypress="return validnumber(event);"></asp:TextBox>
                                        <%--<input type="tel" name="mobile_phone" id="mobile_phone"
                                                           class="gui-input phone-group"
                                                           placeholder="Mobile number">--%>
                                        <span class="field-icon">
                                            <i class="fa fa-mobile-phone"></i>
                                        </span>
                                    </label>
                                </div>
                            </div>
                            <div class="section">
                                <label class="field select" style="z-index: 0">
                                    <asp:DropDownList runat="server" ID="recoveryQuestion">
                                        <asp:ListItem Text="Select Security Question"></asp:ListItem>
                                        <asp:ListItem Text="Who is your favourite cricketer?"></asp:ListItem>
                                        <asp:ListItem Text="Which is your favourite book author?"></asp:ListItem>
                                        <asp:ListItem Text="Who is you favourite actor?"></asp:ListItem>
                                        <asp:ListItem Text="What is your pets name?"></asp:ListItem>
                                        <asp:ListItem Text="Custom Text."></asp:ListItem>
                                    </asp:DropDownList>
                                </label>
                            </div>
                            <div class="section" id="spy3">
                                <label for="comment" class="field prepend-icon">
                                    <asp:TextBox runat="server" TextMode="MultiLine" CssClass="gui-textarea" ID="answer" AutoComplete="off" placeholder="Security Answer"></asp:TextBox>
                                    <%-- <textarea class="gui-textarea" id="comment" name="comment"
                                                            placeholder="Your comment"></textarea>--%>
                                    <span class="field-icon">
                                        <i class="fa fa-comment"></i>
                                    </span>
                                </label>
                            </div>

                            <div class="panel-footer text-right">
                                <asp:Button ID="btnReset" CssClass="btn btn-warning  " runat="server" Text="Reset Password" OnClick="btnReset_Click"/>
                       
                                 <asp:Button runat="server" ID="btnCreateUser" CssClass="btn btn-primary " Text="Create New User" OnClick="btnCreateUser_Click" OnClientClick="return Validation();" />
                       
                                 <asp:Button ID="btnDeleteUser" runat="server" Visible="false" CssClass="btn btn-warning " Text="Delete them" OnClientClick="return ConfirmDelete()" OnClick="btnDeleteUser_Click" />

                                <asp:Button runat="server" ID="reset" CssClass="btn " OnClick="reset_Click" Text="Cancel" style="background-color:darkgray" />
                                <%--<button type="reset" class="btn mb5" id="reset" onclick="Clearfield();" runat="server"> Cancel</button>--%>
                            </div>
                   </div>
               </div>
        </div>
  </div>
    </section>
    <div id="myModal" runat="server" class="modal">
        <div class="panel-body" style="background-color: wheat; text-align: center; width: 300px; height: 200px; margin-left: 40%; border-radius: 50px">
            <h6 class="mb20" id="error"></h6>
        </div>
    </div>

</asp:Content>

