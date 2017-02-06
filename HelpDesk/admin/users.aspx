<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA.users" CodeBehind="users.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
<ol class="breadcrumb">
        <li><a href="/default.aspx">Home</a></li>
        <li><a href="/admin.aspx">Adminstration</a></li>
        <li class="active">Users</li>
    </ol>
    <div class="container-fluid">
<div class="list-group" style="width:200px">
    <div class="list-group-item">
         <asp:Label ID="lblLocation" runat="server" CssClass="list-group-item-heading" Text="Locations"></asp:Label>
         <asp:DropDownList runat="server"  ID="ddl_Locations" CssClass="list-group-item-text"></asp:DropDownList>            
    </div>
</div>

<asp:GridView ID="GV" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both" BorderColor="#d8d8d8"
        BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="false" AllowSorting="false"
        DataKeyNames="ID" Height="51px" BorderStyle="None" PageSize="500" Font-Size="8pt" CssClass="table" Width="60%"        
        onrowdatabound="GV_RowDataBound"
        OnRowCancelingEdit="GV_RowCancelEdit"
        OnRowUpdating="GV_RowUpdating" 
        OnRowDeleting="GV_RowDeleting"        
        onrowediting="GV_RowEditing">
            <FooterStyle BackColor="#CCCC99" />
            <Columns>                
                <asp:CommandField ShowEditButton="true" ItemStyle-CssClass="SP_dataLabel" />
                <asp:CommandField ShowDeleteButton="true" ItemStyle-CssClass="SP_dataLabel" />

                <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="true" Visible="true">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="friendlyName" HeaderText="Name" ReadOnly="true">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>                 
                                
                <asp:TemplateField HeaderText="Active">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" VerticalAlign="Top" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("IsActive")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("isActive")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField>                                                                      
                              
                <asp:TemplateField HeaderText="Admin">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" VerticalAlign="Top" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("IsAdmin")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:CheckBox ID="chkIsAdmin" runat="server" Checked='<%# Eval("IsAdmin")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField>                                                         


             <%--   <asp:TemplateField HeaderText="Electrical Maintenance">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" VerticalAlign="Top" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("isMaintElectrical")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:CheckBox ID="chkisMaintElectrical" runat="server" Checked='<%# Eval("isMaintElectrical")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField> 


                <asp:TemplateField HeaderText="Mechanical Maintenance">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" VerticalAlign="Top" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("isMaintMechanical")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:CheckBox ID="chkisMaintMechanical" runat="server" Checked='<%# Eval("isMaintMechanical")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField>                

                <asp:TemplateField HeaderText="Maintenance Other">
                    <ItemStyle CssClass="SP_dataLabel" VerticalAlign="Top" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ControlStyle />                    
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("isMaintOther")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:CheckBox ID="chkisMaintOther" runat="server" Checked='<%# Eval("isMaintOther")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField>  --%>              
                                
             </Columns>
            
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <HeaderStyle ForeColor="White" CssClass="SP_dataLabel_Header" />
            <EmptyDataTemplate>
                &nbsp;
            </EmptyDataTemplate>
           <RowStyle Font-Size="8pt" Wrap="False" BackColor="#ebf3ff" Font-Names="Verdana" />
           <AlternatingRowStyle BackColor="White" Font-Size="8pt" Font-Names="Verdana" />
           <EditRowStyle Font-Size="Small" />
           <EmptyDataRowStyle Font-Size="Small" />
        </asp:GridView>        
        
        </div>

<asp:HiddenField ID="hdnFocus" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnErrorMsg" runat="server" ClientIDMode="Static"/> 

       <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" ClientIDMode="Static" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <div id="darkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>   
    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="../images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server"  />     

<script type="text/javascript">
    function pageLoad() {

        switch (document.getElementById("hdnFocus>").value) {
            case "UserID":                
                break;
            default:                
                break;
        }
        document.getElementById("hdnFocus").value = "";

        // Display Error Messages
        if (document.getElementById("hdnErrorMsg").value != "") {
            alert(document.getElementById("hdnErrorMsg").value);
            document.getElementById("hdnErrorMsg").value = "";
        }

    }    

    function Return() {
        window.location = "../admin.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function NewUser() {
        window.location = "GetUser.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function fnLocationChanged() {
        __doPostBack('locationChanged', '');
        $find("PROCPanel").show();
    }

    function DeleteUser() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm('Are you sure you want to delete?');
        document.getElementById("darkBackgroundLayer").style.display = "none";
        return retval;
    }
    
</script>

</asp:Content>