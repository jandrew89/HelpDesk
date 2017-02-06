<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA.Admin" EnableEventValidation="false" CodeBehind="Admin.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">  
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
         <ol class="breadcrumb">
             <li><a href="/default.aspx">Home</a></li>
             <li class="active">Adminstration</li>
        </ol>    
    <div class="list-group col-md-4">
        <a href="javascript:ReplaceUsers();" class="list-group-item">Replace Users</a>
        <a href="admin/users.aspx?BP=<%=_BP%>" class="list-group-item">Users</a>
        <a href="workflow/WF_Manager.aspx?BP=<%=_BP%>" class="list-group-item">Workflows</a>
        <a href="javascript:FindUser();" class="list-group-item">Find User</a>
        <a href="javascript:Sites();" class="list-group-item">Sites</a>
        <a href="javascript:Departments();" class="list-group-item">Departments</a>
        <a href="javascript:Maintenance();" class="list-group-item">Maintenance</a>
        <a href="javascript:Equipment();" class="list-group-item">Equipment</a>
    </div>
 
 <%--   <table border="0" cellpadding="2" cellspacing="0" style="width:25%">
        <tr class="SP_Row">
            <td class="SP_itemLabelCell" style="width:150px"><asp:Label ID="lblWorkflowIDLabel" runat="server" CssClass="SP_itemLabel2" Text=""></asp:Label></td>
            <td class="SP_dataLabelCell" style="width:150px"><asp:TextBox ID="txtWorkflowID" Width="100" CssClass="SP_dataLabel2" runat="server"></asp:TextBox></td>
            <td class="SP_dataLabelCell" style="width:50px">
                <asp:LinkButton ID="lbDeleteID" Font-Size="Small" ForeColor="Black" CssClass="SP_dataLabel" runat="server"  OnClientClick="return Delete();" OnClick="btnDelete_Click">Delete</asp:LinkButton>
            </td>
        </tr>
        
         <tr class="SP_Row" runat="server" id="tr1">
                <td class="SP_itemLabelCell"><asp:Label ID="lblRU" runat="server" CssClass="SP_itemLabel2" Text="Replace Users"></asp:Label></td>                
                
                <td class="SP_dataLabelCell">
                    <a id="aManageReplaceUsers1" href="javascript:ReplaceUsers();" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>
                </td>
            </tr>         
        
        <tr class="SP_Row">
            <td class="SP_itemLabelCell"><asp:Label ID="lblUsersLabel" runat="server" CssClass="SP_itemLabel2" Text="Users"></asp:Label></td>                
            
            <td class="SP_dataLabelCell">
                <a href="admin/users.aspx?BP=<%=_BP%>" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>
            </td>
        </tr>        

        <tr class="SP_Row">
            <td class="SP_itemLabelCell"><asp:Label ID="lblWorkflowLabel" runat="server" CssClass="SP_itemLabel2" Text="Workflows"></asp:Label></td>                
            
            <td class="SP_dataLabelCell">
                <a href="workflow/WF_Manager.aspx?BP=<%=_BP%>" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>                    
            </td>
        </tr>    
        
         <tr class="SP_Row" runat="server" id="tr2">
                <td class="SP_itemLabelCell"><asp:Label ID="lblFindUser" runat="server" CssClass="SP_itemLabel2" Text="Find User"></asp:Label></td>                
            
                <td class="SP_dataLabelCell">
                    <a id="a1" href="javascript:FindUser();" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>                    
                </td>
            </tr>
         
        <tr class="SP_Row" runat="server" id="tr3">
                <td class="SP_itemLabelCell"><asp:Label ID="lblSites" runat="server" CssClass="SP_itemLabel2" Text="Sites"></asp:Label></td>                
                
                <td class="SP_dataLabelCell">
                    <a id="a2" href="javascript:Sites();" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>                    
                </td>
        </tr>        

        <tr class="SP_Row" runat="server" id="tr4">
                <td class="SP_itemLabelCell"><asp:Label ID="Label1" runat="server" CssClass="SP_itemLabel2" Text="Departments"></asp:Label></td>                
                
                <td class="SP_dataLabelCell">
                    <a id="a3" href="javascript:Departments();" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>                    
                </td>
        </tr>        

        <tr class="SP_Row" runat="server" id="tr5">
                <td class="SP_itemLabelCell"><asp:Label ID="Label2" runat="server" CssClass="SP_itemLabel2" Text="Maintenance"></asp:Label></td>                
                
                <td class="SP_dataLabelCell">
                    <a id="a4" href="javascript:Maintenance();" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>                    
                </td>
        </tr> 

        <tr class="SP_Row" runat="server" id="tr6">
                <td class="SP_itemLabelCell"><asp:Label ID="Label3" runat="server" CssClass="SP_itemLabel2" Text="Equipment"></asp:Label></td>                
                
                <td class="SP_dataLabelCell">
                    <a id="a5" href="javascript:Equipment();" class="SP_dataLabel" style="color:Black;font-size:small">Manage</a>                    
                </td>
        </tr> 
                    
    </table>        
    <br /><br />--%>

<asp:Label runat="server" ID="lblMsg" BackColor="White" ForeColor="Blue" Font-Size="Small" Font-Bold="false"></asp:Label>

<div id="darkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>

<asp:HiddenField ID="hdnFocus" runat="server" />
<asp:HiddenField ID="hdnErrorMsg" runat="server" ClientIDMode="Static" />  

     <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" ClientIDMode="Static"
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server"  />     


<script type="text/javascript">    

    function pageLoad() {
        // Display Error Messages
        if (document.getElementById("hdnErrorMsg").value != "") {
            alert(document.getElementById("hdnErrorMsg").value);
            document.getElementById("hdnErrorMsg").value = "";
        }
    }

    function Delete() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        if (!confirm('Are you sure you want to delete this Workflow?\nThis action cannot be undone!')) {
            document.getElementById("darkBackgroundLayer").style.display = "none";
            return false;
        }
        document.getElementById("darkBackgroundLayer").style.display = "none";
        return true;
    }

    function Return() {
        window.location = "default.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function FindUser() {
        window.location = "workflow/FindUser.aspx?BP=0";
        $find("PROCPanel").show();
    }

    function ReplaceUsers() {
        window.location = "admin/replaceUser.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function Sites() {
        window.location = "admin/Sites.aspx";
        $find("PROCPanel").show();
    }

    function Departments() {
        window.location = "admin/Departments.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function Maintenance() {
        window.location = "admin/Maintenance.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function Equipment() {
        window.location = "admin/Equipment.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }
</script>

</asp:Content>
