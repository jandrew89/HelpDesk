<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA.Maintenance" CodeBehind="Maintenance.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
 <ol class="breadcrumb">
           <li><a href="/default.aspx">Home</a></li>
           <li><a href="/admin.aspx">Adminstration</a></li>
           <li class="active">Maintenance</li>
 </ol>
<div class="container-fluid">
    <div class="panel panel-primary" style="width:240px">
       <div class="panel-heading">
       <h3 class="panel-title">Locations</h3>
    </div>
    <div class="panel-body">
        <asp:DropDownList runat="server" ID="ddl_Locations" CssClass="form-control"></asp:DropDownList>            
     </div>
    </div>  
<asp:GridView ID="GV" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both" BorderColor="#d8d8d8"
        BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="false" AllowSorting="false"
        DataKeyNames="ID" Height="51px" BorderStyle="None" PageSize="500" Font-Size="8pt" CssClass="table" Width="70%"        
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

                 <asp:TemplateField HeaderText="Active">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" VerticalAlign="Top" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("UserID")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <asp:TextBox runat="server" CssClass="SP_dataLabel" ID="txtUserID" Width="140px" Text='<%# Eval("userID")%>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField> 
                
                 <asp:TemplateField HeaderText="Active">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" VerticalAlign="Top" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("friendlyName")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <asp:TextBox runat="server" CssClass="SP_dataLabel" ID="txtFN" Width="140px" Text='<%# Eval("friendlyName")%>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField>                
                                
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
                                                                                          
                <asp:TemplateField HeaderText="Electrical Maintenance">
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
        
 <asp:Panel runat="server" ClientIDMode="Static" ID="panelDepartment" style="width:400px;display:none">
 <div id="popUp_Department" runat="server">          
    <table id="Table2" border="0" class="tblPadding2" style="background-color:white;width:100%">
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="Label4" Text="Location" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:Label runat="server" ID="lblPopUpLocation" Text="" CssClass="SP_dataLabel"></asp:Label>
            </td>
        </tr>
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="lbl111" Text="UserID" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:TextBox ID="txtUserID" Width="225px" CssClass="SP_dataLabel" runat="server" ClientIDMode="Static"></asp:TextBox>                    
            </td>
        </tr>
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="Label1" Text="Name" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:TextBox ID="txtName" Width="225px" CssClass="SP_dataLabel" runat="server" ClientIDMode="Static"></asp:TextBox>                    
            </td>
        </tr>
         
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="Label2" Text="Active" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:CheckBox runat="server" ID="cbActive" CssClass="SP_dataLabel" />
            </td>
        </tr>

        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="Label5" Text="Mechanical" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:CheckBox runat="server" ID="cbMechanical" CssClass="SP_dataLabel" />
            </td>
        </tr>

        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="Label6" Text="Electrical" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:CheckBox runat="server" ID="cbElectrical" CssClass="SP_dataLabel" />
            </td>
        </tr>

        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                <asp:Label runat="server" ID="Label8" Text="Other" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">
                <asp:CheckBox runat="server" ID="cbOther" CssClass="SP_dataLabel" />
            </td>
        </tr>
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        
    </table>   
  </div>
    <asp:HiddenField ID="hdnPopUp_Junk" runat="server" />    

     <ajaxToolkit:ModalPopupExtender ID="MPEUser" runat="server" ClientIDMode="Static"
                TargetControlID="hdnPopUp_Junk"
                BackgroundCssClass="modalBackground"
                PopupControlID="panelDepartment"                
                DropShadow="true"
                CancelControlID="hdnPopUp_Junk" /> 

</asp:Panel>



<asp:HiddenField ID="hdnFocus" runat="server" ClientIDMode="Static" />
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

        switch (document.getElementById("hdnFocus").value) {
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
   
    function fnLocationChanged() {
        __doPostBack('locationChanged', '');
        $find("PROCPanel").show();
    }

    //called by funtion to show popup (configPage in this case)
    function fnShowPopUp(title) {
        //alert(title);        
        document.getElementById("lblPopUpToolbarTitle").innerText = title;
        $find("MPEUser").show();
        document.getElementById("txtUserID").focus();
        return false;
    }

    //called by the 'Save' button on the popup toolbar (initialized in addPopUpToolbar_Department)
    function fnNewValue() {
        $find("MPEUser").hide();
        __doPostBack('newUser', '');
        $find("PROCPanel").show();
    }

    //called by the 'Cancel button on the popup toolbar (initialized in addPopUpToolbar_Department)
    function fnHideToolBarPopUp() {
        $find("MPEUser").hide();
        //document.getElementById("txtDepartment").value = "";
        //document.getElementById("txtDescription").value = "";
    }
    function DeleteUser() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm('Are you sure you want to delete?');
        document.getElementById("darkBackgroundLayer").style.display = "none";
        return retval;
    }
    
</script>

</asp:Content>