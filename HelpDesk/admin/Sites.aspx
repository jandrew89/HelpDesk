<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" 
    Inherits="QA.Sites" CodeBehind="Sites.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
 <ol class="breadcrumb">
           <li><a href="/default.aspx">Home</a></li>
           <li><a href="/admin.aspx">Adminstration</a></li>
           <li class="active">Sites</li>
 </ol>
    <div class="container-fluid">
 <asp:GridView ID="GV" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both" BorderColor="#d8d8d8"
        BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="false" AllowSorting="false"
        DataKeyNames="BP" Height="51px" BorderStyle="None" PageSize="500" Font-Size="8pt" CssClass="table" Width="60%"        
        onrowdatabound="GV_RowDataBound"
        OnRowCancelingEdit="GV_RowCancelEdit"
        OnRowUpdating="GV_RowUpdating"         
        OnRowDeleting="GV_RowDeleting"        
        onrowediting="GV_RowEditing">
            <FooterStyle BackColor="#CCCC99" />
            <Columns>                
                <asp:CommandField ShowEditButton="true" ItemStyle-CssClass="SP_dataLabel" />                
                <asp:CommandField ShowDeleteButton="true" ItemStyle-CssClass="SP_dataLabel" />                

                <asp:BoundField DataField="BP" HeaderText="BP" ReadOnly="true">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Location">
                    <ItemStyle CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />                    
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="50%" ID="txtLocation" CssClass="SP_dataLabel" Text='<%#Eval("Location") %>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField> 
                                
                <asp:TemplateField HeaderText="Active">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Active")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# Eval("Active")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField>                                                                      
                              
                <asp:TemplateField HeaderText="Create">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("CreateNew")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:CheckBox ID="cbCreateNew" runat="server" Checked='<%# Eval("CreateNew")%>' CssClass="SP_dataLabel" />
                   </EditItemTemplate>
                </asp:TemplateField>                                                                                         
                                
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
    
       <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" ClientIDMode="Static" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="../images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

    <div id="darkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>   
    <asp:HiddenField ID="hdnErrorMsg" runat="server" ClientIDMode="Static" />

<script type="text/javascript">
    function pageLoad() {
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

    function NewLocation() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        retval = prompt("Enter new location", "");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (retval == null || retval == "")
            return;

        document.getElementById("darkBackgroundLayer").style.display = "";
        retval1 = prompt("Enter new location name", "");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (retval1 == null || retval1 == "")
            return;

        __doPostBack('newSite', retval + ';' + retval1);
        $find("PROCPanel").show();
    }

    function DeleteSite() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm('Are you sure you want to delete?');
        document.getElementById("darkBackgroundLayer").style.display = "none";
        return retval;
    }
    
</script>

</asp:Content>