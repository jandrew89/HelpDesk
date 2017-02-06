<%@ Page Language="C#" AutoEventWireup="True" Inherits="QA.GetUser" MasterPageFile="~/MasterPages/Tools.master" CodeBehind="GetUser.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />

<table border="0" class="tblPadding2">
     <tr class="SP_Row">
        <td style="white-space:nowrap" class="SP_itemLabelCell">
            <asp:Label ID="lblLocation" runat="server" CssClass="SP_itemLabel2" Text="Locations"></asp:Label>
        </td>         
        <td style="white-space:nowrap" class="SP_dataLabelCell">
            <asp:DropDownList runat="server" ID="ddl_Locations" CssClass="SP_dataLabel"></asp:DropDownList>            
        </td>
    </tr>

    <tr class="SP_Row">
        <td style="white-space:nowrap;width:150px" class="SP_itemLabelCell">
            <asp:Label ID="lblName" runat="server" Text="Name" CssClass="SP_itemLabel2"></asp:Label>
        </td>
        <td style="white-space:nowrap" class="SP_dataLabelCell">
            <asp:TextBox ID="txtName" Width="175px" CssClass="SP_dataLabel" runat="server"></asp:TextBox>    
        </td>
    </tr>
</table>    
    
    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" EnableCaching="true"
                                  TargetControlID="txtName" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender" 
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected ="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions"
                                  OnClientItemSelected = "AddUser" >    
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
    <asp:HiddenField ID="hdnProc" runat="server"  />     

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

    function AddUser(source, eventArgs) {
        //alert("Key:" + eventArgs.get_text() + "- Value:" + eventArgs.get_value());
        __doPostBack('NewUser', eventArgs.get_value());
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function Return() {
        window.location = "users.aspx?BP=<%=_BP%>";
        $find("<%=PROCPanel.ClientID%>").show();
    }
</script>

</asp:Content>