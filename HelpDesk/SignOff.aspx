<%@ Page Language="C#" AutoEventWireup="True" Inherits="QA.SignOff" MasterPageFile="~/MasterPages/Tools.master" CodeBehind="SignOff.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
<br />
<table border="0" cellpadding="2" cellspacing="0">
<tr class="SP_Row">
    <td style="white-space:nowrap;width:150px;vertical-align:top" class="SP_itemLabelCell">
        <asp:Label ID="lblComments" runat="server" Text="Comments" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:TextBox runat="server" ID="txtComments" CssClass="SP_dataLabel" TextMode="MultiLine" Rows="5" Columns="72"></asp:TextBox>
    </td>
</tr>
<tr class="SP_Row">
    <td style="white-space:nowrap;width:150px;vertical-align:top" class="SP_itemLabelCell">
        <asp:Label ID="Label1" runat="server" Text="" CssClass="SP_itemLabel2"></asp:Label>
    </td>
    <td style="white-space:nowrap" class="SP_dataLabelCell">
         <asp:RadioButtonList ID="rbSignoffToggle" RepeatLayout="Flow" Font-Names="Arial" style="font-weight:bold;font-size:12px;" RepeatDirection="Horizontal" runat="server">                                    
                                    <asp:ListItem Text="Yes" Selected="True" Value="1" />
                                    <asp:ListItem Text="Reject" Value="2" />
                        </asp:RadioButtonList>
    </td>
</tr>
</table>    
    
<br />
<asp:Label runat="server" ID="lblMsg" BackColor="White" ForeColor="Red" Font-Size="Small" Font-Bold="false"></asp:Label>

 <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

<asp:LinkButton runat="server" ID="junk" ></asp:LinkButton>

<div runat="server" id="WFdarkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>
<script type="text/javascript">
    
    function fnSave() {
        __doPostBack('Save', '');
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function Cancel() {
        __doPostBack('Cancel', '');
        $find("<%=PROCPanel.ClientID%>").show();
    }

    function fnInsertStep(stepID, BP, workflowID) {
        document.getElementById("<%=WFdarkBackgroundLayer.ClientID%>").style.display = "";
        var args1 = "BP=" + BP + "&stepID=" + stepID + "&insertAfter=1&workflowID=" + workflowID;
        retval = window.showModalDialog("/workflow/InsertStep.aspx?" + args1, "", "dialogHeight:575px;dialogWidth:550px;center:yes;scroll:yes;resizable:yes;status:no;unadorned:yes;");
        document.getElementById("<%=WFdarkBackgroundLayer.ClientID%>").style.display = "none";      
    }
    
</script>
</asp:Content>