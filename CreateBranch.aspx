<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateBranch.aspx.cs" Inherits="CreateBranch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">
         function checkSpcialChar(event) {
             if (!((event.keyCode >= 65) && (event.keyCode <= 90) || (event.keyCode >= 97) && (event.keyCode <= 122) ||
                 (event.keyCode >= 48) && (event.keyCode <= 57))) {
                 event.returnValue = false;
                 return;
             }
             event.returnValue = true;
         }
    </script>
 

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

    <section id="content" class="table-layout">






          <!-- Column Left -->
        <aside class="chute chute-left chute290 chute-icon-style bg-info" data-chute-height="match" style="height: 602px;">
            <div class="chute-icon"></div>
            <div class="chute-container">
                <div class="chute-bin1 stretch1 btn-dimmer mt20">

                    <div class="tab-content pn br-n bg-none allcp-form-list">

                        <ul class="nav list-unstyled" role="tablist">

                            <li class="">
                                <a href="BranchManage.aspx" class="btn btn-info btn-gradient btn-alt btn-block br-n "> Manage Branch</a>
                              
                            </li>

                             <li class="">
                                <a href="CreateBranch.aspx" class="btn btn-info btn-gradient btn-alt btn-block br-n "> Create Branch</a>
                              
                            </li>


                            <li class="">
                        <a href="CreateLHO.aspx" class="btn btn-info btn-gradient btn-alt btn-block br-n "> Create LHO </a>
                                   
                                
                                 </li>

                            
                            <li class="">
                        <a href="AddMachin.aspx" class="btn btn-info btn-gradient btn-alt btn-block br-n "> Add Machine</a>
                                   
                                
                                 </li>
                        </ul>

                    </div>

                </div>
            </div>
        </aside>
        <!-- /Column Left -->


                <!-- Column Center -->
        <div class="chute chute-center" style="height: 622px;">

            <div class="tab-content mw900 center-block center-children">
                
                   <!-- Contact Form -->
                <div class="allcp-form theme-info tab-pane active mw600" id="CreateBranch" role="tabpanel">
                    <div class="panel">
                        <div class="panel-heading">
                            <span class="panel-title pn">Create Branch</span>
                        </div>
                        <!-- /Panel Heading -->


                        <div class="panel-body pn">

                            <div class="section">
                                <p>Select LHO/Circle Name</p>
                                <asp:DropDownList runat="server" class="gui-input" ID="CB_ddl_circle_name"  >
                                </asp:DropDownList>

                            </div>




                            <div class="section"> 
                                <p>Select Branch Code</p>
                                <asp:TextBox runat="server" ID="CB_txt_branch_Code" class="gui-input"  placeholder="Branch Code - eg: 0123456 " MaxLength="6" MinLength="6" TextMode="Phone" AutoPostBack="true" OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)" OnTextChanged="CB_txt_branch_Code_TextChanged"></asp:TextBox>
                            
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="In Valid Branch Code , Enter 6 digit code with prefix 0." ValidationExpression="^([0-5]{6})$" ControlToValidate="CB_txt_branch_Code" ValidationGroup="Validate" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                            
                            </div>

                            <div class="section">
                                <p>Branch Name</p>
                                <asp:TextBox runat="server" ID="CB_txt_Branch_Name" class="gui-input" placeholder="Branch Name" AutoPostBack="true" onkeypress="return checkSpcialChar(event)" OnTextChanged="CB_txt_Branch_Name_TextChanged"></asp:TextBox>
                            </div>

                            <div class="section">
                                <p>Region</p>
                                <asp:TextBox runat="server" ID="CB_txt_Region" class="gui-input" placeholder="Region - eg:01" onkeypress="return checkSpcialChar(event)"  onkeydown="return CheckNumericKeyInfo(event.keyCode, event.which)" MaxLength="2"></asp:TextBox>
                            </div>

                            <!-- /section -->

                            <div class="section">
                                <p>Module Code</p>
                                <asp:TextBox runat="server" ID="CB_txt_Module_Code" class="gui-input" placeholder="Module Code - eg:01" onkeypress="return checkSpcialChar(event)" onkeydown="return CheckNumericKeyInfo(event.keyCode, event.which)" MaxLength="2"></asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section">
                                <p>Module Name</p>
                                <asp:TextBox runat="server" ID="CB_txt_Module_Name" class="gui-input" onkeypress="return checkSpcialChar(event)" placeholder="Module Name"></asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section">
                                <p>Network</p>
                                <asp:TextBox runat="server" ID="CB_txt_Network" class="gui-input" onkeypress="return checkSpcialChar(event)" onkeydown="return CheckNumericKeyInfo(event.keyCode, event.which)" placeholder="Network"></asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section hide">
                                <p>Person Name</p>
                                <asp:TextBox runat="server" ID="CB_txt_Contact_Person_Name" class="gui-input" placeholder="Contact Person Name" ></asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section hide">
                                <p>Person Number</p>
                                <asp:TextBox runat="server" ID="CB_txt_Contact_Person_Number" class="gui-input" placeholder=" Person Number" MaxLength="10"   OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)"></asp:TextBox>
                          
                                       <asp:RegularExpressionValidator ID="revMobNo" runat="server" ErrorMessage="In-valid Mobile Number." ValidationExpression="^([0-9]{10})$" ControlToValidate="CB_txt_Contact_Person_Number" ValidationGroup="Validate" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <!-- /section -->


                            <div class="section hide">
                                <p>Email Id</p>
                                <asp:TextBox runat="server" ID="CB_txt_Email" class="gui-input" placeholder=" Email" TextMode="Email" ToolTip="demo@doamin.com"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="CB_txt_Email" ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" Display = "Dynamic" ErrorMessage = "In-valid email address"/>

                            </div>
                            <!-- /section -->
                        </div>
                        <!-- /Form -->
                        <div class="section text-right">
                            <asp:Button ID="btn_save_branch" runat="server" OnClick="btn_save_branch_click" class="btn btn-primary fs14 ph40" Text="Save" />

                        </div>

                    </div>
                    <!-- /Panel -->
                </div>
                <!-- /Contact Form -->

            </div>

        </div>

        <!-- /Column Center -->

</section>
</asp:Content>

