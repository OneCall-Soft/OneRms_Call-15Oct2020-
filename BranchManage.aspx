<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BranchManage.aspx.cs" Inherits="TransactionReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <script type="text/javascript">
        $(document).ready(function () {
            $(window).keydown(function (event) {
                if (event.keyCode == 13 ) {
                    event.preventDefault();
                    return false;
                }
            });
        });


        function checkSpcialChar(event) {
            if (!((event.keyCode >= 65) && (event.keyCode <= 90) || (event.keyCode >= 97) && (event.keyCode <= 122) ||
                (event.keyCode >= 48) && (event.keyCode <= 57))) {
                event.returnValue = false;
                return;
            }
            event.returnValue = true;
        }

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
        
                <!-- Comment Form -->
                <div class="allcp-form theme-primary tab-pane active  mw600" id="ManageBranch" role="tabpanel">
                    <div class="panel">
                        <div class="panel-heading">
                            <span class="panel-title pn">Manage Branch</span>
                        </div>
                        <!-- /Panel Heading -->


                        <div class="panel-body pn">

                            <div class="section">
                                <p>Select LHO/Circle Name</p>
                                <asp:DropDownList ID="MB_ddl_Circle_Name" class="gui-input" runat="server" AutoPostBack="true" OnSelectedIndexChanged="MB_ddl_Circle_Name_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <!-- /section -->
                            <div class="section">
                                <p>Enter Branch Code</p>
                                <asp:DropDownList ID="MB_ddl_branch_code" class="gui-input" runat="server" AutoPostBack="true" OnSelectedIndexChanged="MB_ddl_branch_code_SelectedIndexChanged" placeholder="Select Branch Code">
                                    <asp:ListItem Selected="True">--Select Branch Code--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <!-- /section -->

                            <div class="section">
                                <p>Branch Name</p>
                                <asp:TextBox runat="server" ID="MB_txt_branchName" class="gui-input" placeholder="Branch Name" onkeypress="return checkSpcialChar(event)"> </asp:TextBox>

                            </div>
                            <!-- /section -->



                            <div class="section">
                                <p>Region</p>
                                <asp:TextBox runat="server" ID="MB_txt_Region"  class="gui-input" placeholder="Region" onkeypress="return checkSpcialChar(event)"  onkeydown="return CheckNumericKeyInfo(event.keyCode, event.which)" MaxLength="2"> </asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section">
                                <p>Module Code</p>
                                <asp:TextBox runat="server" ID="MB_txt_ModuleCode" class="gui-input" placeholder="Module Code" onkeypress="return checkSpcialChar(event)" onkeydown="return CheckNumericKeyInfo(event.keyCode, event.which)" MaxLength="2"> </asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section">
                                <p>Module Name</p>
                                <asp:TextBox runat="server" ID="MB_txt_ModuleName" class="gui-input" placeholder="Module Name" onkeypress="return checkSpcialChar(event)"> </asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section">
                                <p>Network</p>
                                <asp:TextBox runat="server" ID="MB_txt_Network" class="gui-input" placeholder="Network" onkeypress="return checkSpcialChar(event)" onkeydown="return CheckNumericKeyInfo(event.keyCode, event.which)"> </asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section hide">
                                <p>PersonName</p>
                                <asp:TextBox runat="server" ID="MB_txt_PersonName" class="gui-input" placeholder="Contact Person Name"> </asp:TextBox>

                            </div>
                            <!-- /section -->

                            <div class="section hide">
                                <p>Person Number</p>
                                <asp:TextBox runat="server" ID="MB_txt_PersonNumber" class="gui-input" placeholder=" Person Number"  MaxLength="10"  OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)">  </asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMobNo" runat="server" ErrorMessage="In-valid Mobile Number."  ValidationExpression="^([0-9]{10})$" ControlToValidate="MB_txt_PersonNumber" ValidationGroup="Validate" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            
                            </div>
                            <!-- /section -->

                            <div class="section hide">
                                <p>Email Id</p>
                                <asp:TextBox runat="server" ID="MB_txt_Email" class="gui-input" placeholder=" Email"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="MB_txt_Email" ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" Display = "Dynamic" ErrorMessage = "In-valid email address"/>

                            </div>
                            <!-- /section -->
                        </div>
                        <!-- /Form -->
                        <div class="section text-right">
                            <asp:Button ID="MB_btn_Update" runat="server" OnClick="MB_btn_Update_Click" class="btn btn-primary fs14 ph40" Text="Update" />

                        </div>

                        <!-- /Form -->
                    </div>
                    <!-- /Panel -->
                </div>
                <!-- /Comment Form -->

               
            </div>

        </div>

        <!-- /Column Center -->

    </section>





    <script type="text/javascript">


        $(function () {
            $('[id*=txtToDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy-mm-dd",


                //  format: 'L',

            });
        });

        $(function () {
            $('[id*=txtFromDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "yyyy-mm-dd",
                // format: 'L',
            });
        });

        $(function () {

            $('[id*=txtFromDate]').datepicker({
                weekStart: 1,
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', function (selected) {

                if ($('[id*=txtFromDate]').val() == '' || typeof
                    $('[id*=txtFromDate]').val() == "undefined") {
                    $('[id*=txtToDate]').val('').datepicker('setStartDate', null);
                }
                else {
                    $('[id*=txtToDate]').val('').datepicker('setDate', new Date());
                    $('[id*=txtToDate]').datepicker('setStartDate', null);
                    startDate = new Date(selected.date.valueOf());
                    startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
                    $('[id*=txtToDate]').datepicker('setDate', startDate);
                    $('[id*=txtToDate]').datepicker('setStartDate', startDate);
                    $('[id*=txtToDate]').val('').datepicker('update', '');
                }
            });

            $('[id*=txtFromDate]').blur(function (selected) {
                if ($('[id*=txtToDate]').val() != '' && $('[id*=txtFromDate]').val() == '' || typeof $('[id*=txtFromDate]').val() == "undefined") {
                    $('[id*=txtFromDate]').datepicker('setDate', new Date());
                    $('[id*=txtFromDate]').val('').datepicker('setStartDate', null);
                    $('[id*=txtToDate]').datepicker('setDate', new Date());
                    $('[id*=txtToDate]').val('').datepicker('setStartDate', null);
                }
            });



        });



    </script>

</asp:Content>

