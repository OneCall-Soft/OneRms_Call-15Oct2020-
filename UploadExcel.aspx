<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UploadExcel.aspx.cs" Inherits="UploadExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="row mt20" >
        <section class="col-lg-12 ">
            <!--data table box-->
            <div class="box box-info">
                <div class="box-header  "   >
                 
                    <div class="col-md-3 form-group"  >
                      <i class="fa fa-search"></i><span> Select Excel File</span>
                        <asp:FileUpload ID="BtnExcelUpload" CssClass="form-control fileupload-controls  " runat="server" /><br />

                         <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-flat btn-success form-control " Text="Upload" OnClick="btnUpload_Click" />  
                
                    </div>                  

             
                </div>         
            
                

            </div>

    </section>
    </div>
</asp:Content>

