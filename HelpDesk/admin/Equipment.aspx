<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA.Equipment" CodeBehind="Equipment.aspx.cs" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
 <ol class="breadcrumb">
           <li><a href="/default.aspx">Home</a></li>
           <li><a href="/admin.aspx">Adminstration</a></li>
           <li class="active">Equipment</li>
 </ol>

<div class="container-fluid">
 <table border="0" class="tblPadding2" style="width:20%">
    <tr class="SP_Row">
        <td style="white-space:nowrap" class="SP_itemLabelCell">
            <asp:Label ID="lblLocation" runat="server" CssClass="SP_itemLabel2" Text="Locations"></asp:Label>
        </td>         
        <td style="white-space:nowrap" class="SP_dataLabelCell">
            <asp:DropDownList runat="server" ID="ddl_Locations" CssClass="SP_dataLabel"></asp:DropDownList>            
        </td>
    </tr>

    <tr class="SP_Row">
        <td style="white-space:nowrap" class="SP_itemLabelCell">
            <asp:Label ID="Label1" runat="server" CssClass="SP_itemLabel2" Text="Department"></asp:Label>
        </td>         
        <td style="white-space:nowrap" class="SP_dataLabelCell">
            <asp:DropDownList runat="server" ID="ddl_Department" CssClass="SP_dataLabel"></asp:DropDownList>            
        </td>
    </tr>
</table>

    <br />
    <asp:GridView ID="GV" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both" BorderColor="#d8d8d8"
        BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="false" AllowSorting="false"
        DataKeyNames="ID,BP" Height="51px" BorderStyle="None" PageSize="500" Font-Size="8pt" CssClass="table" Width="40%"        
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

               <%-- <asp:TemplateField HeaderText="Location">
                    <ItemStyle CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />                    
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("BP")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Width="50%" ID="txtLocation" CssClass="SP_dataLabel" Text='<%#Eval("BP") %>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField> --%>
                                
                <asp:TemplateField HeaderText="Unit">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Unit")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <asp:TextBox runat="server" Width="75" ID="txtUnit" CssClass="SP_dataLabel" Text='<%#Eval("Unit") %>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField>                                                                      
               
                 <asp:TemplateField HeaderText="Model">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("Model")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:TextBox runat="server" Width="175" ID="txtModel" CssClass="SP_dataLabel" Text='<%# Eval("Model")%>'></asp:TextBox>                        
                   </EditItemTemplate>
                </asp:TemplateField>  

                 <asp:TemplateField HeaderText="Mfg">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("Mfg")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:TextBox runat="server" Width="175" ID="txtMfg" CssClass="SP_dataLabel" Text='<%# Eval("Mfg")%>'></asp:TextBox>                        
                   </EditItemTemplate>
                </asp:TemplateField>  
                               
                <asp:TemplateField HeaderText="Description">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("Description")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:TextBox runat="server" Width="200" ID="txtDescription" CssClass="SP_dataLabel" Text='<%#Eval("Description") %>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField>                                                                                         

                 <asp:TemplateField HeaderText="Department">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("Department")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:DropDownList runat="server" ID="ddl_Department" CssClass="SP_dataLabel"></asp:DropDownList>                        
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
    
    
     <%--//************ Add New **************************//--%>
    <asp:Panel runat="server" ID="panelPA" style="display:none">
        <div id="divPA" runat="server">          
            <table id="tblPA" border="0" style="background:White;width:100%;padding:2px" cellspacing="0">              
                <tr><td>&nbsp;</td></tr>
                 <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label8" Text="Location" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:Label runat="server" ID="lblAddLocation" CssClass="SP_dataLabel" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
                <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label4" Text="Unit" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:TextBox runat="server" ID="txtAddUnit" Width="150" CssClass="SP_dataLabel" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label5" Text="Model" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:TextBox runat="server" ID="txtAddModel" Width="150" CssClass="SP_dataLabel" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label6" Text="Manufacturer" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:TextBox runat="server" ID="txtAddMfg" Width="150" CssClass="SP_dataLabel" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label2" Text="Description" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:TextBox runat="server" ID="txtAddDescription" Width="150" CssClass="SP_dataLabel" ClientIDMode="Static"></asp:TextBox>
                    </td>
                </tr>
                 <tr class="SP_Row">        
                    <td style="white-space:nowrap;width:50px" class="SP_itemLabelCell">
                        <asp:Label runat="server" ID="Label3" Text="Department" CssClass="SP_itemLabel2"></asp:Label>
                    </td>
                    <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell">                
                        <asp:DropDownList runat="server" ID="ddl_AddDepartment" CssClass="SP_dataLabel" ClientIDMode="Static"></asp:DropDownList>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>        
            </table>             
        </div>
    <asp:HiddenField ID="hdnPA" runat="server" ClientIDMode="Static" />

   <ajaxToolkit:ModalPopupExtender ID="MPE_PA" runat="server" ClientIDMode="Static"                 
                TargetControlID="hdnPA"
                BackgroundCssClass="modalBackground"
                PopupControlID="panelPA"                
                DropShadow="true"                 
                CancelControlID="hdnPA" />

    <script type="text/javascript">
        //called by button/link/etc. to open modal
        function fnShowPA() {
            $find("MPE_PA").show();
            document.getElementById("txtAddUnit").focus();
            return false;
        }
        //called by toolbar in popup
        function fnSavePA() {
            var v = document.getElementById("txtAddDescription").value;
            if (v == null || v.length == 0) {
                alert("Please enter a description!");
                return;
            }

            //v = document.getElementById("txtAccountNumber").value;
            //if (v == null || v.length == 0) {
            //    alert("Please enter an account number!");
            //    return;
            //}

            //v = document.getElementById("txtPAAmount").value;
            //if (v == null || v.length == 0) {
            //    alert("Please enter an amount!");
            //    return;
            //}

            $find("MPE_PA").hide();
            __doPostBack('newPA', '');
            $find("PROCPanel").show();
        }
        //called by toolbar in popup
        function fnHidePA() {
            $find("MPE_PA").hide();
            document.getElementById("txtAddUnit").value = "";
            document.getElementById("txtAddModel").value = "";
            document.getElementById("txtAddMfg").value = "";
            document.getElementById("txtAddDescription").value = "";            
        }
        //assigned in init method
        function DeletePA(ID) {
            __doPostBack('deletePA', ID);
            $find("PROCPanel").show();
        }
    </script>
</asp:Panel>
     
    

       <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server"  ClientIDMode="Static"
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

    function Sync() {
        __doPostBack('Sync', '');
        $find("PROCPanel").show();
    }
    
    function fnLocationChanged() {
        __doPostBack('locationChanged', '');
        $find("PROCPanel").show();
    }

    function fnEquipmentChanged() {
        __doPostBack('equipmentChanged', '');
        $find("PROCPanel").show();
    }

    function DeleteEquipment() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm('Are you sure you want to delete?');
        document.getElementById("darkBackgroundLayer").style.display = "none";
        return retval;
    }
</script>

</asp:Content>