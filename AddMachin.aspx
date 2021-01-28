<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddMachin.aspx.cs" Inherits="AddMachin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function ()
        {
            $(window).keydown(function (event) {
                if (event.keyCode == 13 || event.keyCode == 32) {
                    event.preventDefault();
                    return false;
                }
            });
        });
   
        function checkSpcialChar(event)
        {
            if (!((event.keyCode >= 65) && (event.keyCode <= 90) || (event.keyCode >= 97) && (event.keyCode <= 122) || (event.keyCode >= 48) && (event.keyCode <= 57))) {
                event.returnValue = false;
                return;
            }
            event.returnValue = true;
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
                <div class="allcp-form theme-info tab-pane active mw600" id="" role="tabpanel" >
                    <div class="panel">
                        <div class="panel-heading">
                            <span class="panel-title pn">Add Machines</span>
                        </div>
                        <!-- /Panel Heading -->
                        <div class="panel-body pn">
                            <div class="section">
                                <h6>Enter Machine Serial Number</h6>
                                <asp:TextBox runat="server" ID="txt_serial_Number" AutoPostBack="true" class="gui-input" 
                                    OnTextChanged="txt_serial_Number_TextChanged" MaxLength="20" onkeypress="return checkSpcialChar(event)"
                                     placeholder="Enter Machine Serial Number" ></asp:TextBox>
                                <asp:Label runat="server" ID="SerialNoErrorLbl" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <!-- /Form -->
                        <div class="section text-right">
                            <asp:Button ID="btn_Create_Machine" runat="server" Visible="false" OnClick="btn_Create_Machine_Click" class="btn btn-primary fs14 ph40" Text="Register" />
                        </div>
                    </div>
                    <!-- /Panel -->
                </div>
            </div>
        </div>
        <!-- /Column Center -->

        </section>
</asp:Content>

