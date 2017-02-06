<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="AddAttachment.aspx.cs" Inherits="QA.Attachment"  MasterPageFile="~/MasterPages/Tools.master" EnableEventValidation="false" EnableViewState="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" EnablePageMethods="true" />

    <br />
<div style="margin-left:10px">
    <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload1" runat="server" Width="400px" 
                        UploaderStyle="Modern" UploadingBackColor="#CCFFFF"
                        OnUploadedComplete="AsyncFileUpload1_UploadedComplete"
                        OnClientUploadComplete="uploadComplete"
                        OnClientUploadStarted="uploadStarted"
                        OnClientUploadError="uploadError" /> 
    <br />
    
    <div><span id="labelUploadMsg"></span></div>
</div>

 <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server"  ClientIDMode="Static"
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server"  />     


<script type="text/javascript">


    function Cancel() {        
        window.location = "mainForm.aspx?<%=getQueryString()%>";
        $find("PROCPanel").show();
    }

    var msg = document.getElementById("labelUploadMsg");
     function uploadError(sender, args) {
         //alert(args.get_errorMessage());         
         msg.innerText = args.get_fileName() + " " + args.get_errorMessage() + " ";
         msg.style.color = red;
        }

        function uploadComplete(sender, args) {
            Cancel();
            return;
            var contentType = args.get_contentType();
             var text = args.get_length() + " bytes";
             if (contentType.length > 0) {
                 text += ", " + contentType + "";
             }
           msg.innerText = text;
           //msg.style.color = Black;
       }  

    function uploadStarted(sender, args) {
        msg.innerText = "Upload is started";
        $find("PROCPanel").show();
            //msg.style.color = Black;
      }

</script>
</asp:Content>
