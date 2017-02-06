<%@ Page Language="C#" AutoEventWireup="True" Inherits="WF.WFInsertStep" MasterPageFile="~/MasterPages/Tools.master" CodeBehind="InsertStep.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:Label ID="BlockLabel" runat="server" BackColor="Red" Font-Bold="true" Width="100%" Font-Names="Arial" ForeColor="White"></asp:Label>    
<table border="0" cellpadding="3" cellspacing="0">
<tr class="SP_Row">
    <td style="white-space:nowrap;width:100px" class="SP_itemLabelCell">
        <asp:Label ID="Label1" runat="server" CssClass="SP_itemLabel" Text="Title"></asp:Label>
    </td>
    <td style="white-space:nowrap;width:200px" class="SP_dataLabelCell">
        <asp:TextBox ID="txtStepTitle" Width="225px" runat="server" CssClass="SP_dataLabel"></asp:TextBox>    
    </td>
</tr>
<tr class="SP_Row">
    <td style="white-space:nowrap" class="SP_itemLabelCell"></td>
     <td style="white-space:nowrap" class="SP_dataLabelCell">
        <asp:RadioButton ID="rbBefore" GroupName="rb" Text="Before" runat="server" CssClass="SP_dataLabel"></asp:RadioButton>
        <asp:RadioButton ID="rbAfter" GroupName="rb" Text="After" Checked="true" runat="server" CssClass="SP_dataLabel"></asp:RadioButton>
        &nbsp;&nbsp;<asp:Label ID="lblStepDescription" runat="server"  CssClass="SP_dataLabel"></asp:Label>
    </td>
</tr>
<tr>
<td colspan="3" style="text-align:right;width:350px" class="modalButtons">    
    <asp:Button ID="SaveUsers" runat="server" style="width:20%;" Text="Save" OnClick="SaveUsers_Click" />&nbsp;&nbsp;  
    <input type="button" style="width:20%;" value="Cancel" onclick="javascript:fnClose()" />
</td>
</tr>
</table>

<script type="text/javascript">
    function fnEsc(e)
    {
        if(e.keyCode == 27)
        {
            window.returnValue = false;
            window.close();
        }
    }

    function fnClose()
    {
        window.returnValue = false;
        window.close();
    }
</script>

</asp:Content>