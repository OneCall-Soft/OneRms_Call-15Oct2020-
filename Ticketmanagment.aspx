<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Ticketmanagment.aspx.cs" Inherits="Ticketmanagment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <style>
     pre {
     color:#000 !important}


 </style>

    <header id="topbar" class="alt">
            <div class="topbar-left">
                <ol class="breadcrumb">
                    <li class="breadcrumb-link">
                        <a href="Dashboard-V3_1.aspx">Home</a>
                    </li>
                    <li class="breadcrumb-current-item">Ticket Reported</li>
                </ol>
            </div>
        </header>

     
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager> 
    <section id="content" class="" style="padding-top:10px">
            <div class="row">

                <!-- FAQ Left Column -->
                <div class="col-md-8">

                    <div class="panel">

                        <div class="panel-body pn mtn">
                            <div class="br-b">
                                <h2 class="mb20 mt10">Tickets and Issue Details 
                                    <%-- <span class="badge badge-dark  mt5 mr30" data-toggle="tooltip" data-placement="top" title="" data-original-title="Total Issues"> <% = iTotTicket %> </span>
                                    <span class="badge badge-danger  mt5 mr30" data-toggle="tooltip" data-placement="top" title="" data-original-title="Total Issues"> <% = iTotPending %> </span>
                                    <span class="badge badge-success  mt5 mr30" data-toggle="tooltip" data-placement="top" title="" data-original-title="Total Issues"> <% = iTotResolved %> </span>--%>


                                </h2>
                             
                            
                            </div>
                           

                            <div class="mt40">
                                <h5 class="text-muted mb20 mt40">  Pending Issue -
                                     <span class="badge badge-danger  mt5 mr30" data-toggle="tooltip" data-placement="top" title="" data-original-title="Total Pending Issues"> <% = iTotPending %> </span> </h5>

                                <div class="panel-group accordion accordion-lg" id="accordion2">
                                    <asp:Repeater ID="RepeaterPendingIssue" runat="server">
                                        <HeaderTemplate></HeaderTemplate>
                                        <ItemTemplate>
                                             <div class="panel">
                                        <div class="panel-heading">
                                            <a class="accordion-toggle accordion-icon link-unstyled collapsed" data-toggle="collapse" data-parent="#accordion2" href="#accordion2_<%#Eval("AutoRequestId") %>">
                                               <asp:Label ID="lblTicketNumber1" runat="server" Text='<%#Eval("CallTicketNumber").ToString().Replace("crm","") %>' style="color:#000;"></asp:Label> -   
                                                <asp:Label ID="lblIssueTitle1" runat="server" Text='<%#Eval("IssueCategory") %>' style="color:#000;"></asp:Label>  - 
                                              <asp:Label ID="lblStatus1" runat="server" Text='<%#Eval("RequestType") %>'></asp:Label>
                                            </a>
                                        </div>
                                        <div id="accordion2_<%#Eval("AutoRequestId") %>" class="panel-collapse collapse" style="height: 0px;">
                                            <div class="panel-body">
                                                <asp:Label ID="lblIssueDetails"  runat="server" Text='<%#Eval("ProblemDescription") %>'></asp:Label>
                                             
                                                 <span class="text-info pull-right mt5 mr20 " data-toggle="tooltip" data-placement="top" title="" data-original-title='<%#Eval("User") %>'>
                                                 <i class="fa fa-user" style="font-size:24px"></i>     </span>
                                                 <span class="text-info pull-right mt5 mr5 "> <u> Posted By </u></span>
                                                  <span class="text-dark-darker pull-right mt5 mr5 "> Issue occured on Kiosk Serial number -  <asp:Label ID="lblKioskSerialNumber"  runat="server" Text='<%#Eval("SerialNumber") %>'></asp:Label> -  </span>

                                            </div>
                                        </div>
                                    </div>

                                        </ItemTemplate>
                                        <FooterTemplate>

                                           
                                        </FooterTemplate>

                                    </asp:Repeater>

                                </div>
                            </div>


                              <div class="br-t">
                                <h5 class="text-muted mb20 mt40">  Resolved Issue - 
                                     <span class="badge badge-success  mt5 mr30" data-toggle="tooltip" data-placement="top" title="" data-original-title="Total Resolved Issues"> <% = iTotResolved %> </span> </h5>


                                <div class="panel-group accordion accordion-lg" id="accordion3">
                                  <asp:Repeater ID="RepeaterResolvedIssue" runat="server">
                                        <HeaderTemplate></HeaderTemplate>
                                        <ItemTemplate>
                                             <div class="panel">
                                        <div class="panel-heading">
                                            <a class="accordion-toggle accordion-icon link-unstyled collapsed" data-toggle="collapse" data-parent="#accordion3" href="#accordion3_<%#Eval("AutoRequestId") %>">
                                               <asp:Label ID="lblTicketNumberR1" runat="server" Text='<%#Eval("CallTicketNumber").ToString().Replace("crm","") %>' style="color:#000;"></asp:Label> -   
                                                <asp:Label ID="lblIssueTitleR1" runat="server" Text='<%#Eval("IssueCategory") %>' style="color:#000;"></asp:Label>  - 
                                              <asp:Label ID="lblStatusR1" runat="server" Text='<%#Eval("RequestType") %>'></asp:Label> on 
                                                <asp:Label ID="lblCLosedDate" runat="server" Text='<%#Eval("DateModified") %>' style=""></asp:Label>
                                            </a>
                                        </div>
                                        <div id="accordion3_<%#Eval("AutoRequestId") %>" class="panel-collapse collapse" style="height: 0px;">
                                            <div class="panel-body">
                                                <asp:Label ID="lblIssueDetailsR"  runat="server" Text='<%#Eval("ProblemDescription") %>'></asp:Label>
                                                 <span class="text-info pull-right mt5 mr20 " data-toggle="tooltip" data-placement="top" title="" data-original-title='<%#Eval("User") %>'>
                                                
                                                 <i class="fa fa-user" style="font-size:24px"></i>     </span>

                                                 <span class="text-info pull-right mt5 mr5 "> <u> Posted By </u></span>
                                                
                                                 <span class="text-dark-darker pull-right mt5 mr5 "> Issue occured on Kiosk Serial number -  <asp:Label ID="lblKioskSerialNumber"  runat="server" Text='<%#Eval("SerialNumber") %>'></asp:Label> -  </span>

                                            </div>
                                        </div>
                                    </div>

                                        </ItemTemplate>
                                        <FooterTemplate></FooterTemplate>

                                    </asp:Repeater>
                                </div>
                            </div>
                          
                        </div>
                    </div>
                </div>

                <!-- FAQ Right Column -->
                <div class="col-md-4">
             
                  
                    <div class="panel mb10">
                        <div class="panel-heading">
                            <span class="panel-title"> Recently added Tickets</span>
                        </div>
                        <div class="panel-body text-muted pn pv10">

                            <asp:Repeater ID="RepeaterTickets" runat="server">

                                <HeaderTemplate></HeaderTemplate>
                                <ItemTemplate>
                            <ul class="list-unstyled">
                                <li><strong class="text-dark"> <span>Ticket No. - </span>
                                    <asp:Label ID="lblTicketNumber" runat="server" Text='<%#Eval("CallTicketNumber").ToString().Replace("crm","") %>' style="color:#11a8bb;"></asp:Label>  </strong> -
                                     <asp:Label ID="lblIssueTitle" runat="server" Text='<%#Eval("IssueCategory") %>' style="color:#000;"></asp:Label> 
                                </li>
                                <li>Posted:   <asp:Label ID="lblPostedTime" runat="server" Text='<%#Eval("DateCreated") %>' style="color:#000;"></asp:Label> </li>
                                <li>Call:
                                    <strong class="text-alert-darker"><asp:Label ID="lblStatus" runat="server" Text='<%#Eval("RequestType") %>'></asp:Label> </strong>
                                </li>
                            </ul>

                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:Repeater>



                        </div>
                       
                    </div>

                </div>

            </div>

        </section>

  <!-- ticket Management Form -->
<div class="allcp-form theme-primary popup-basic popup-lg mfp-with-anim mfp-rotateLeft mfp-hide" id="calendarManagment1">
    <div class="panel">
        <div class="panel-heading">
        <span class="panel-title">
          <i class="fa fa-pencil-square-o"></i>New Issue Submit Form
        </span>
        </div>

     
            <div class="panel-body p25">
                <div class="section-divider mt10 mb40">
                    <span>Issue Details</span>
                </div>

                <div class="section row">
                    <div class="col-md-6">
                        <label  class="field prepend-icon">
                            <input type="text" id="IssueTitle" class="gui-input"
                                   placeholder="Select Issue Title"/>
                            <span class="field-icon">
                                <i class="fa fa-bug"></i>
                            </span>
                        </label>
                    </div>

                    <div class="col-md-6">
                        <label for="eventDate" class="field prepend-icon">
                            <input type="text" id="eventDate" name="eventDate" class="gui-input"
                                   placeholder="Issue Date"/>
                            <span class="field-icon">
                                <i class="fa fa-calendar"></i>
                            </span>
                        </label>
                    </div>
                </div>
                <!-- /section -->

                <div class="section row">
                    <div class="col-md-6">
                        <label  class="field prepend-icon">
                            <input type="email" name="email" id="email" class="gui-input" placeholder="Contact Email"/>
                            <span class="field-icon">
                                <i class="fa fa-envelope"></i>
                            </span>
                        </label>
                    </div>
               
                    <div class="col-md-6">
                      
                            <label for="username" class="field prepend-icon">
                                <input type="text" name="username" id="KioskID" class="gui-input"
                                       placeholder="Kiosk ID / Kiosk IP / Branch Code"/>
                                <span class="field-icon">
                                    <i class="fa fa-desktop"></i>
                                </span>
                            </label>
                        
                    
                    </div>
                </div>
                <!-- /section -->

                <div class="section row">
                    <div class="col-xs-12">
                        <label class="field prepend-icon">
                        <textarea class="gui-textarea" id="comment" name="comment"
                                  placeholder="Issue Description"></textarea>
                            <span class="field-icon">
                                <i class="fa fa-comments"></i>
                            </span>
                            <span class="input-footer hidden">
                            <strong>Hint:</strong>Don't be negative or off topic! just be awesome...</span>
                        </label>
                    </div>
                </div>
                <!-- /section -->

            </div>
            <div class="panel-footer ">
                <button type="button" id="SubmitIssue" onclick="btnSubmitIssue()" class="button btn-primary pt15 pb15">Submit Issue</button>
                
                
                 
                <span class="pull-right fs18 mt10 mr20 text-dark-darker" id="lblTKNumber"></span>

            </div>
       
    </div>
</div>



</asp:Content>

