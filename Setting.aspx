<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Setting.aspx.cs" Inherits="Setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <%--<script src="assets/js/jquery/jquery-1.12.3.min.js"></script>--%>
    
      <style>
        #source_button {
            display: none !important;
        }
    </style>

    <script>
        function nowCheck() {
            debugger;
        // Get all the child elements inside the DIV.
       // var cont = document.getElementById('work_list').childNodes;
     //  var cont = document.getElementsByTagName("input") ;
       var cont = document.getElementsByTagName("input");
        var chk = [];
       
        for (var i = 0; i < cont.length; i++) {
            // Check if the element is a checkbox.
            if (cont[i].type == 'checkbox') {
           
                // Finally, check if the checkbox is checked.
                if (cont[i].checked) {
                   // alert(cont[i].value + ' is checked!');
                    chk.push(true);
                }
                else {
                    chk.push(false);
                }

    

            }
        }
debugger;
 PageMethods.PassThings(chk, OnSuccess, OnError);

    }

     

        function OnSuccess(result) {
            alert('Setting Changed' );
        }
        function OnError() {debugger;
            alert('Some error has ocurred!');
        }



function nowCheck1() {

          debugger;
                //Logic to delete the item
              var  data = "1";
              <%-- alert("<%= SendCommandToKiosk("restart" , "1")%>");--%>
                PageMethods.PassThings(data, OnSuccess, OnError);
          
        }

</script>
 
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>

     
    <header id="topbar" class="alt">
        <div class="topbar-left">
            <ol class="breadcrumb">
                <li class="breadcrumb-link">
                    <a href="#">Home</a>
                </li>
                <li class="breadcrumb-current-item">Settings</li>
            </ol>
        </div>
    </header>


    <section id="content" class="table-layout">

        <!-- Column Left -->
       
        <!-- /Column Left -->

        <!-- Column Center -->
        <div class="chute chute-center" style="">

         



                <div class="row" >

                    <div class="col-md-6">

                        <!-- Tag Group Widget -->

                        <div class="panel listgroup-widget">
                            <div class="panel-heading mb30">
                                <span class="panel-title pn">Printer Statistics - settings </span>
                            </div>
                            <ul class="list-group" id="work_list">
                                <li class="list-group-item br-t">
                                    <span class="badge badge-system mr10">Branch User</span>
                                    <span class="badge badge-system mr20">Circle User</span>

                                    <span class="badge badge-system mr30">Admin</span>

                                    <strong>Values</strong>
                                </li>
                                <asp:PlaceHolder ID="PlaceHolder_Setting" runat="server"></asp:PlaceHolder>


                                
                                <li class='list-group-item'>   <asp:Button ID="BtnJsonWrite" OnClientClick="nowCheck();" runat="server" CssClass="btn btn-rounded btn-success" OnClick="BtnJsonWrite_Click" Text="Update"/>      
                                       </li>
                            </ul>
                        </div>

                    </div>

                   <div class="col-md-6" > 

                  


                   </div>
                </div>


         

        </div>
        <!-- /Column Center -->

    </section>

</asp:Content>

