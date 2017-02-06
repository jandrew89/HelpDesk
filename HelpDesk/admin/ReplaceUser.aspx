<%@ Page Language="C#" AutoEventWireup="True" Inherits="QA.clsReplaceUser" MasterPageFile="~/MasterPages/Tools.master" CodeBehind="ReplaceUser.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
   <ol class="breadcrumb">
             <li><a href="/default.aspx">Home</a></li>
             <li><a href="/admin.aspx">Adminstration</a></li>
             <li class="active">Replace Users</li>
        </ol>    
<div class="container-fluid">
    
 <div class="form-group">
     <asp:Label ID="lblCU" runat="server" Text="Current User" CssClass="control-label"></asp:Label>
     <asp:TextBox ID="txtName" Width="175px" runat="server" CssClass="form-control"></asp:TextBox>    
 </div>
  <div class="form-group">
     <asp:Label ID="lblNU" runat="server" Text="New User" CssClass="control-label"></asp:Label>
     <asp:TextBox ID="txtNewUser" Width="175px" runat="server" CssClass="form-control"></asp:TextBox>    
 </div>
     <asp:Label ID="Label1" runat="server" Text="Caution:This action will replace step users/creators/step template users<br>This action cannot be undone!"
             CssClass="text-danger"></asp:Label>
</div>

    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" EnableCaching="true"
                                  TargetControlID="txtName" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender" 
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected ="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions"
                                  OnClientItemSelected = "" >    
    </ajaxToolkit:AutoCompleteExtender>

    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="10" EnableCaching="true"
                                  TargetControlID="txtNewUser" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender" 
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions"
                                  OnClientItemSelected = "" >    
    </ajaxToolkit:AutoCompleteExtender>
    
      <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="../images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

    <div runat="server" id="darkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>

    <asp:HiddenField ID="hdnErrorMsg" runat="server" />    

<script type="text/javascript">
    function ZOrderAutoCompleteOptions(control, args) {
        control._completionListElement.style.zIndex = 10000001;
    }

    function pageLoad() {
        // Display Error Messages
        if (document.getElementById("<%=hdnErrorMsg.ClientID%>").value != "") {
             alert(document.getElementById("<%=hdnErrorMsg.ClientID%>").value);
            document.getElementById("<%=hdnErrorMsg.ClientID%>").value = "";
        }
    }   

    function Replace() {
        __doPostBack('Replace', '');
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function Return() {
        window.location = "../admin.aspx?BP=<%=_BP%>";
        $find("<%=PROCPanel.ClientID%>").show();
    }    

    function fnConfirm(message, messageID) {            
         document.getElementById("<%=darkBackgroundLayer.ClientID%>").style.display = "";
         var retval = confirm(message);
         document.getElementById("<%=darkBackgroundLayer.ClientID%>").style.display = "none";
         if (retval == true) {
             __doPostBack('Confirmed', messageID);
             //can't call this because AJAX hasn't fully loaded yet!
             //document.getElementById("PROCPanel").show();
         }
         return false;
     }
</script>

</asp:Content>