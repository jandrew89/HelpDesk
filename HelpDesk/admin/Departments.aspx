<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA.Departments" CodeBehind="Departments.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />
 <ol class="breadcrumb">
           <li><a href="/default.aspx">Home</a></li>
           <li><a href="/admin.aspx">Adminstration</a></li>
           <li class="active">Departments</li>
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
        DataKeyNames="ID,BP" Height="51px" BorderStyle="None" PageSize="500" Font-Size="8pt" CssClass="table" Width="60%"        
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
                                
                <asp:TemplateField HeaderText="Name">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("Name")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <asp:TextBox runat="server" Width="50%" ID="txtName" CssClass="SP_dataLabel" Text='<%#Eval("Name") %>'></asp:TextBox>
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
                       <asp:TextBox runat="server" Width="200px" ID="txtDescription" CssClass="SP_dataLabel" Text='<%#Eval("Description") %>'></asp:TextBox>
                   </EditItemTemplate>
                </asp:TemplateField>                                                                                         

                <asp:TemplateField HeaderText="Supervisor">
                    <ControlStyle />
                    <ItemStyle CssClass="SP_dataLabel" Wrap="false" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Eval("SupervisorFriendlyName")%>' CssClass="SP_dataLabel"></asp:Label>
                   </ItemTemplate>
                    <EditItemTemplate>
                       <asp:TextBox runat="server" Width="175px" ID="txtSupervisor" CssClass="SP_dataLabel" Text='<%# Eval("SupervisorFriendlyName")%>'></asp:TextBox>
                         <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" EnableCaching="true" 
                                  TargetControlID="" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender" 
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected ="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions"
                                  OnClientItemSelected = "AddUser" >    
                         </ajaxToolkit:AutoCompleteExtender>
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
          
     
    <asp:HiddenField runat="server" ID="hdnSupervisor" ClientIDMode="Static" />

    <script type="text/javascript">
        function AddUser(source, eventArgs) {
            //alert("Key:" + eventArgs.get_text() + "- Value:" + eventArgs.get_value());
            //__doPostBack('NewUser', eventArgs.get_value());
            document.getElementById("hdnSupervisor").value = eventArgs.get_value();
            //$find("<%=PROCPanel.ClientID%>").show();
        }

        function ZOrderAutoCompleteOptions(control, args) {
            control._completionListElement.style.zIndex = 10000001;
        }
    </script>

 <asp:Panel runat="server" ClientIDMode="Static" ID="panelDepartment" style="width:400px;display:none">
 <div id="popUp_Department" runat="server">          
    <table id="Table2" border="0" class="tblPadding2" style="background-color:white;width:100%">
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_dataLabelCell1">
                <asp:Label runat="server" ID="lbl111" Text="Department" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell1">
                <asp:TextBox ID="txtDepartment" Width="225px" CssClass="SP_dataLabel" runat="server" ClientIDMode="Static"></asp:TextBox>                    
            </td>
        </tr>
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_dataLabelCell1">
                <asp:Label runat="server" ID="Label1" Text="Description" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell1">
                <asp:TextBox ID="txtDescription" Width="225px" CssClass="SP_dataLabel" runat="server" ClientIDMode="Static"></asp:TextBox>                    
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

    function NewDepartment() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        retval = prompt("Enter new Name", "");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (retval == null || retval == "")
            return;

        document.getElementById("darkBackgroundLayer").style.display = "";
        retval1 = prompt("Enter new description", "");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (retval1 == null || retval1 == "")
            return;

        __doPostBack('newDepartment', retval + ';' + retval1);
        $find("PROCPanel").show();
    }

    function fnLocationChanged() {
        __doPostBack('locationChanged', '');
        $find("PROCPanel").show();
    }
    

    //called by funtion to show popup (configPage in this case)
    function fnAddDepartment(title) {
        //alert(title);        
        document.getElementById("lblPopUpToolbarTitle").innerText = title;
        $find("MPEUser").show();
         document.getElementById("txtDepartment").focus();
         return false;
     }

    //called by the 'Save' button on the popup toolbar (initialized in addPopUpToolbar_Department)
    function fnNewDepartment() {
        $find("MPEUser").hide();
        __doPostBack('newDepartment', '');
        $find("PROCPanel").show();
    }    

    //called by the 'Cancel button on the popup toolbar (initialized in addPopUpToolbar_Department)
    function fnHideToolBarPopUp_Department() {
        $find("MPEUser").hide();
        document.getElementById("txtDepartment").value = "";
        document.getElementById("txtDescription").value = "";
    }
    function DeleteDepartment() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm('Are you sure you want to delete?');
        document.getElementById("darkBackgroundLayer").style.display = "none";
        return retval;
    }
    
</script>

</asp:Content>