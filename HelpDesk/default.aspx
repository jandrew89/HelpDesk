<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA._Default" CodeBehind="Default.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />

    <asp:Label runat="server" ID="lblMsg" BackColor="White" ForeColor="Red" Font-Size="Medium" Font-Bold="false"></asp:Label>
    <ol class="breadcrumb">
        <li class="active">Home</li>
    </ol>
    <div class="container-fluid">
    <div class="form-horizontal">
        <fieldset>
            <legend>Help Desk
            </legend>
            <div class="form-group">
                <asp:Label ID="lblLocation" runat="server" CssClass="control-label col-xs-2" Text="Location"></asp:Label>

                <div class="col-xs-4">
                    <asp:DropDownList ID="ddl_Location" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                   <asp:Label ID="lblWorkflowIDLabel" runat="server" CssClass="control-label col-xs-2" Text="Work Order ID"></asp:Label>
                <div class="col-xs-4">
                     <asp:TextBox ID="txtWorkflowID" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
          <%--  <div class="form-group">
                    <asp:Label ID="Label4" runat="server" CssClass="control-label col-md-2" Text="Workflow Step"></asp:Label>
                  <div class="col-md-5">
                     <asp:DropDownList runat="server" ID="ddl_WorkflowStep" CssClass="form-control"></asp:DropDownList>
                  </div>
            </div>--%>
              <div class="form-group">
                  <asp:Label ID="lblMPINumberLabel" runat="server" CssClass="control-label col-xs-2" Text="Assigned To"></asp:Label>
               <div class="col-xs-4">
                 <asp:DropDownList ID="ddl_AssignedTo" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>        
            <%--  <div class="form-group">
                  <asp:Label ID="lblMPIRev" runat="server" CssClass="control-label col-md-2" Text="Department"></asp:Label>
                  <div class="col-md-5">
                      <asp:DropDownList ID="ddl_Department" runat="server" CssClass="form-control"></asp:DropDownList>
                   </div>
            </div>--%>
            <%--  <div class="form-group">
                  <asp:Label ID="Label1" runat="server" CssClass="control-label col-md-2" Text="Equipment"></asp:Label>
                <div class="col-md-5">
                    <asp:TextBox ID="txtEquipment" Width="200" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>--%>
            <%--  <div class="form-group">
                  <asp:Label ID="Label3" runat="server" CssClass="control-label col-md-2" Text="Service Required"></asp:Label>
                  <div class="col-md-5">
                      <asp:TextBox ID="txtServiceRequired" Width="200" CssClass="form-control" runat="server"></asp:TextBox>
                  </div>
            </div>--%>
              <div class="form-group">
                  <asp:Label ID="Label2" runat="server" CssClass="control-label col-xs-2" Text="Comments"></asp:Label>
                  <div class="col-xs-4">
                      <asp:TextBox ID="txtComments" Width="250" CssClass="form-control" runat="server"></asp:TextBox>
                  </div>
            </div>
              <div class="form-group">
                  <asp:Label ID="Label6" runat="server" CssClass="control-label col-xs-2" Text="Status"></asp:Label>
                  <div class="col-xs-2">
                       <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Circulating" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Not Started" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Complete" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Rejected" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Cancelled" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                  </div>
            </div>
            <div class="form-group">
                <asp:Label ID="Label12" runat="server" CssClass="control-label col-xs-2" Text="Priority"></asp:Label>
                 <div class="col-xs-2">
                       <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                              <asp:ListItem Text="Lo" Value="0"></asp:ListItem>
                              <asp:ListItem Text="Medium" Value="1"></asp:ListItem>                
                              <asp:ListItem Text="High" Value="2"></asp:ListItem>
                              <asp:ListItem Text="Emergency" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                  </div>
            </div>
              <div class="form-group">
               <asp:Label ID="lblCreatedDateLabel" runat="server" CssClass="control-label col-xs-2" Text="Created Date"></asp:Label>
               <div class="col-xs-1">
                     <asp:Label ID="lblCreatedDateFromLabel" runat="server" CssClass="control-label" Text="From"></asp:Label>
              
                    <asp:TextBox ID="txtCreatedDateFrom" CssClass="form-control" Width="90" runat="server"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="BottomLeft" TargetControlID="txtCreatedDateFrom"></ajaxToolkit:CalendarExtender>
              </div>
                  <div class="col-xs-1">
                    <asp:Label ID="lblCreatedDateToLabel" runat="server" CssClass="control-label" Text="To"></asp:Label>
             
                    <asp:TextBox ID="txtCreatedDateTo" Width="90px" CssClass="form-control" runat="server"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="BottomLeft" TargetControlID="txtCreatedDateTo"></ajaxToolkit:CalendarExtender>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblCreatedByLabel" runat="server" CssClass="control-label col-xs-2" Text="Created By"></asp:Label>
                <div class="col-xs-4">
                    <asp:DropDownList runat="server" ID="ddlCreatedBy" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2">
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClientClick="fnSearch();" />
                </div>
            </div>
        </fieldset>
    </div>
    </div>

    <div class="container-fluid col-md-4">
        <div class="form-group" runat="server" visible="false">
            <td class="SP_itemLabelCell">
                <asp:Label ID="lblMy" runat="server" CssClass="SP_itemLabel2" Text="My Work Orders's"></asp:Label></td>
            <td class="SP_dataLabelCell" colspan="2">
                <asp:RadioButtonList runat="server" ID="rblMyECN" CssClass="SP_dataLabel" RepeatDirection="Horizontal">
                    <asp:ListItem Text="N/A" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="My Item's" Value="1"></asp:ListItem>
                    <asp:ListItem Text="My Active Item's" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </div>

        <div class="form-group" runat="server" visible="false">
            <td class="SP_itemLabelCell" style="white-space: nowrap">
                <asp:Label ID="lblCompleted" runat="server" CssClass="SP_itemLabel2" Text="Include Completed<br>Work Order's"></asp:Label></td>
            <td class="SP_dataLabelCell" colspan="2">
                <asp:CheckBox runat="server" ID="cbIncludeCompleted" CssClass="SP_dataLabel" />
            </td>
        </div>

        <div class="form-group" style="text-align: right">
            <td class="SP_itemLabelCell">&nbsp;</td>
            <td class="SP_dataLabelCell" colspan="2">
                &nbsp;&nbsp;&nbsp;&nbsp;                            
            </td>
        </div>
    </div>

    <asp:HiddenField ID="hdnFocus" runat="server" />
    <asp:HiddenField ID="hdnErrorMsg" runat="server" ClientIDMode="Static" />

    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" ClientIDMode="Static"
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel"
        DropShadow="false"
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" Style="display: none; width: 0px; background: #ffffff; border: 0px solid #668dae;" runat="server">
        <img style="border: none" src="images/ajax-loader.gif" alt="" />
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

    <asp:HiddenField ID="hdnUserID" runat="server" />

    <script type="text/javascript">

        function pageLoad() {
            // Display Error Messages
            if (document.getElementById("hdnErrorMsg").value != "") {
                alert(document.getElementById("hdnErrorMsg").value);
                document.getElementById("hdnErrorMsg").value = "";
            }
        }

        function Admin(BP) {
            window.location = "admin.aspx";
            $find("PROCPanel").show();
        }

        function fnSearch() {
            __doPostBack('Search', '');
            $find("PROCPanel").show();
        }

        function MyWorkfow() {
            __doPostBack('MyWorkfow', '');
            $find("PROCPanel").show();
        }

        function CreateNewWorkfow() {
            __doPostBack('New', '');
            $find("PROCPanel").show();
        }

        function fnReset() {
            __doPostBack('ClearForm', '');
            $find("PROCPanel").show();
        }

        function fnCheckForEnter() {
            if (window.event.keyCode == 13) {
                window.event.returnValue = false;
                window.event.cancelBubble = true;
                fnSearch();
            }
        }

        function fnLocationChanged() {
            __doPostBack('LocationChanged', '');
            $find("PROCPanel").show();
        }

    </script>
</asp:Content>
