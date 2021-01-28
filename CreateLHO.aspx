<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateLHO.aspx.cs" Inherits="CreateLHO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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


        function checkSpcialChar(event) {
            if (!((event.keyCode >= 65) && (event.keyCode <= 90) || (event.keyCode >= 97) && (event.keyCode <= 122) ||
                (event.keyCode >= 48) && (event.keyCode <= 57))) {
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
                
             
                <div class="allcp-form theme-info tab-pane active mw600" id="CreateLHO" role="tabpanel" >
                    <div class="panel">
                        <div class="panel-heading">
                            <span class="panel-title pn">Create LHO</span>
                        </div>
                        <!-- /Panel Heading -->


                        <div class="panel-body pn">

                            <div class="section">
                                <p>Circle Code</p>
                                <asp:TextBox runat="server" ID="CL_txt_CircleCode" OnKeyPress="return CheckNumericKeyInfo(event.keyCode, event.which)" MaxLength="3" class="gui-input" placeholder="Circle Code"></asp:TextBox>
                            </div>

                            <div class="section">
                                <p>Circle Name</p>
                                <asp:TextBox runat="server" ID="CL_txt_CircleName" class="gui-input" placeholder="Circle Name" onkeypress="return checkSpcialChar(event)"></asp:TextBox>
                            </div>


                        </div>
                        <!-- /Form -->
                        <div class="section text-right">
                            <asp:Button ID="btn_Create_Circle" runat="server" OnClick="btn_Create_Circle_Click" class="btn btn-primary fs14 ph40" Text="Save" />

                        </div>

                    </div>
                    <!-- /Panel -->
                </div>

            </div>

        </div>

        <!-- /Column Center -->

</section>

</asp:Content>

