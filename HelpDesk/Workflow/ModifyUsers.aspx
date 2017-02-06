<%@ Page Language="C#" AutoEventWireup="True" Inherits="WF.ModifyUsers" MasterPageFile="~/MasterPages/Tools.master" CodeBehind="ModifyUsers.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" EnablePageMethods="true" runat="server" />

<asp:Label ID="BlockLabel" runat="server" BackColor="Red" Font-Bold="true" Width="600px" Font-Names="Arial" ForeColor="White"></asp:Label>        
<table border="0" class="tblPadding2">
<tr class="SP_Row">
    <td style="white-space:nowrap;width:200px" class="SP_itemLabelCell">
        <asp:Label ID="lblTitle" runat="server" CssClass="SP_itemLabel2" Text="Title"></asp:Label>
    </td>
     <td class="SP_itemLabelCell" style="white-space:nowrap;text-align:right">
            <asp:ImageButton runat="server" ImageAlign="Middle" OnClientClick="fnShowToolBarPopUp_NewValue(); return false;" Visible="false" ID="imgProductLine" CssClass="SP_itemLabel2" ImageURL="../images/new add16.png" />
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:DropDownList runat="server" ID="ddl_StepTitles" CssClass="SP_dataLabel"></asp:DropDownList>
        <asp:TextBox ID="txtStepTitle" Width="225px" CssClass="SP_dataLabel" runat="server"></asp:TextBox>
    </td>
</tr>

<tr class="SP_Row">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="Label3" runat="server" CssClass="SP_itemLabel2" Text="Sign-off Page Name"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:Label runat="server" ID="lblDetailURL" CssClass="SP_dataLabel"></asp:Label>
    </td>
</tr>

<tr class="SP_Row" runat="server" id="tr_GroupStep">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="lblGS" runat="server" CssClass="SP_itemLabel2" Text="Group Step"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:CheckBox ID="chkGroup" runat="server" CssClass="SP_dataLabel" />
    </td>
</tr>

<tr class="SP_Row">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="Label6" runat="server" CssClass="SP_itemLabel2" Text="Final Step Options"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:DropDownList ID="ddl_FinalStepOptions" runat="server" CssClass="SP_dataLabel"></asp:DropDownList>        
    </td>
</tr>

<tr class="SP_Row">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="lblPS" runat="server" CssClass="SP_itemLabel2" Text="Parent Step"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:DropDownList runat="server" ID="ddl_ParentSteps" CssClass="SP_dataLabel"></asp:DropDownList>
    </td>
</tr>

<tr class="SP_Row" runat="server" id="tr_URL" visible="false" colspan="2">
    <td style="white-space:nowrap" class="SP_itemLabelCell">
        <asp:Label ID="lblSOL" runat="server" Text="Sign Off Link" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">        
        <asp:DropDownList runat="server" ID="ddl_SignOff" CssClass="SP_dataLabel"></asp:DropDownList>
    </td>
</tr>

<tr class="SP_Row" runat="server" id="tr_PastDueLimit">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="Label4" runat="server" CssClass="SP_itemLabel2" Text="Past Due Limit (Hours)"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:TextBox runat="server" ID="txtPastDueLimit" Width="50" CssClass="SP_dataLabel"></asp:TextBox>
    </td>
</tr>

<tr class="SP_Row" runat="server" id="tr_Reminder">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="Label5" runat="server" CssClass="SP_itemLabel2" Text="Email Reminder Frequency (Hours)"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:TextBox runat="server" ID="txtEmailReminder" Width="50" CssClass="SP_dataLabel"></asp:TextBox>
    </td>
</tr>

<tr class="SP_Row" runat="server" id="tr_RejectOptions">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="lblRO" runat="server" Text="Reject Options" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:DropDownList ID="ddl_Reject" runat="server" CssClass="SP_dataLabel"></asp:DropDownList>        
    </td>    
</tr>

<tr class="SP_Row" runat="server" id="tr_Tooltip">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="Label1" runat="server" Text="Step Tooltip" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:TextBox runat="server" ID="txtHelpToolTip" CssClass="SP_dataLabel" Width="95%" TextMode="MultiLine" Rows="4"></asp:TextBox>
    </td>    
</tr>

<tr class="SP_Row" runat="server" id="tr_RuntimeUser">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="lblRU" runat="server" Text="Add Runtime User" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:DropDownList runat="server" ID="ddl_FieldName" CssClass="SP_dataLabel"></asp:DropDownList>&nbsp;&nbsp;
        <asp:LinkButton ID="lnkBtnAddFieldName" runat="server" Font-Size="8pt" Font-Names="Verdana" onclick="lnkBtnAddFieldName_Click">Add</asp:LinkButton>
    </td>
</tr>

<tr class="SP_Row" runat="server" id="tr_AddUser">
    <td style="white-space:nowrap" class="SP_itemLabelCell" colspan="2">
        <asp:Label ID="lblAU" runat="server" Text="Add User" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:TextBox ID="txtAdduser" Width="225px" CssClass="SP_dataLabel" runat="server"></asp:TextBox>&nbsp;&nbsp;
        <asp:LinkButton ID="lnkBtnAdd" runat="server" Font-Size="8pt" Font-Names="Verdana" onclick="lnkBtnAdd_Click">Add</asp:LinkButton>
    </td>
</tr>
</table>
<br />
<asp:PlaceHolder runat="server" ID="Panel_GV"></asp:PlaceHolder>
    
    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" EnableCaching="true" 
                                  TargetControlID="txtAdduser" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender"
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions"
                                  OnClientItemSelected = "AddUserID" >    
    </ajaxToolkit:AutoCompleteExtender>
    
     <%--//************ Add Value **************************//--%>
    <asp:Panel runat="server" ID="panelAddValue" style="display:none">
        <div id="div_AddValue" runat="server">          
            <table id="tblAddValue" border="0" style="background:White;width:100%;padding:2px" cellspacing="0">              
                <tr><td>&nbsp;</td></tr>
                <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label2" Text="New Value" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:TextBox runat="server" ID="txtNewValue" CssClass="SP_dataLabel"></asp:TextBox>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>        
            </table>             
        </div>
    <asp:HiddenField ID="hdnAddValue" runat="server" />

   <ajaxToolkit:ModalPopupExtender ID="MPEnewValue" runat="server"
                TargetControlID="hdnAddValue"
                BackgroundCssClass="modalBackground"
                PopupControlID="panelAddValue"                
                DropShadow="true"                 
                CancelControlID="hdnAddValue" />
</asp:Panel>


     <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server"
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

    <asp:HiddenField runat="server" ID="hdnUserID" />
    
    <asp:HiddenField runat="server" ID="hdnRefreshHost" />

<script type="text/javascript">
    function ZOrderAutoCompleteOptions(control, args) {
        control._completionListElement.style.zIndex = 10000001;
    }

    function AddUserID(source, eventArgs) {
        //alert(" Key : " + eventArgs.get_text() + "  Value :  " + eventArgs.get_value());
        document.getElementById("<%=hdnUserID.ClientID%>").value = eventArgs.get_value();
    }

    function fnSave() {
        __doPostBack('Save', '')
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function fnReturn() {
        __doPostBack('Return', '')
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function fnTitleChanged() {
        //var ddl_StepTitles = document.getElementById("<%=ddl_StepTitles.ClientID%>");
        //var ddl_StepTitlesValue = ddl_StepTitles.options[ddl_StepTitles.selectedIndex].value;
        //if (ddl_StepTitlesValue == "0") {
        //    document.getElementById("<%=txtStepTitle.ClientID%>").style.display = "";
         //   return false;
       // }
        //if (ddl_StepTitlesValue == "-1") {            
         //   document.getElementById("<%=txtStepTitle.ClientID%>").style.display = "none";
          //     return false;
        //}
        __doPostBack('titleChanged', '');
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function fnShowToolBarPopUp_NewValue() {
        $find("<%=MPEnewValue.ClientID%>").show();
        document.getElementById("<%=txtNewValue.ClientID%>").focus();
        return false;
    }

    function fnHideToolBarPopUp_newValue() {
        $find("<%=MPEnewValue.ClientID%>").hide();
        document.getElementById("<%=txtNewValue.ClientID%>").value = "";
    }

    function fnSaveNewValue() {
        var newValue = document.getElementById("<%=txtNewValue.ClientID%>").value;
        if (newValue == null || newValue.length == 0)
            return;
        $find("<%=MPEnewValue.ClientID%>").hide();
        __doPostBack('newValue', '');
        $find("<%=PROCPanel.ClientID%>").show();
    }

</script>

</asp:Content>