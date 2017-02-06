<%@ Page Language="C#" AutoEventWireup="True" Inherits="QA.FindUser" MasterPageFile="~/MasterPages/Tools.master" CodeBehind="FindUser.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" EnablePageMethods="true" runat="server" />
 <ol class="breadcrumb">
           <li><a href="/default.aspx">Home</a></li>
           <li><a href="/admin.aspx">Adminstration</a></li>
           <li class="active">Find User</li>
 </ol>
<div class="container-fluid">
    <div class="panel panel-primary" style="width:240px">
       <div class="panel-heading">
       <h3 class="panel-title">User</h3>
    </div>
    <div class="panel-body">
        <asp:TextBox ID="txtName" Width="200px" AutoCompleteType="None" CssClass="SP_dataLabel" runat="server"></asp:TextBox>    
     </div>
    </div>
</div>         
    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" EnableCaching="true"
                                  TargetControlID="txtName" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender" 
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions">    
    </ajaxToolkit:AutoCompleteExtender>
        
      <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

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

    function Search() {
        __doPostBack('Search', ''); //eventArgs.get_value());
        $find("<%=PROCPanel.ClientID%>").show();
    }        

    function Return() {
        window.location = "../admin.aspx?BP=<%=_BP%>";
        $find("<%=PROCPanel.ClientID%>").show();
    }
</script>

</asp:Content>