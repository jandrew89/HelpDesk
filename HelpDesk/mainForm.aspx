<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" MaintainScrollPositionOnPostback="true" 
         EnableEventValidation="false" EnableViewState="true" Inherits="QA.mainForm" CodeBehind="mainForm.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Reference Control="~/workflow/WorkflowSteps2.ascx"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" EnablePageMethods="true" />
  <ol class="breadcrumb">
             <li><a href="/default.aspx">Home</a></li>
             <li class="active">Search Page</li>
  </ol>
 <div class="container-fluid">
     <div class="form-horizontal">
        <fieldset>
            <legend>Search
            </legend>
            <div class="form-group">
                <asp:Label ID="lblLocationName" runat="server" CssClass="control-label col-xs-2" Text="Location" Font-Size="Medium"></asp:Label>
                <asp:Label ID="lblLocation" CssClass="control-label col-xs-4" runat="server"></asp:Label>
            </div>
            <div class="form-group">
                   <asp:Label runat="server" ID="lblNumber" CssClass="control-label col-xs-2" Text="Work Order" Font-Size="Medium"></asp:Label>
                   <asp:Label runat="server" ID="lblFriendlyName" CssClass="control-label col-xs-4"></asp:Label>
            </div>
              <div class="form-group">
                    <asp:Label ID="Label2" runat="server" CssClass="control-label col-xs-2" Text="Status" Font-Size="Medium"></asp:Label>
                    <asp:Label runat="server" ID="lblStatus" CssClass="control-label col-xs-4"></asp:Label>
            </div>
              <div class="form-group">
                    <asp:Label ID="Label3" runat="server" CssClass="control-label col-xs-2" Text="Department" Font-Size="Medium"></asp:Label>
                    <div class="col-xs-4">
                        <asp:DropDownList runat="server" ID="ddl_Department" CssClass="form-control" Visible="true"></asp:DropDownList>
                    </div>
            </div>        
              <div class="form-group">
                  <asp:Label ID="Label5" runat="server" CssClass="control-label col-xs-2 text-danger" Text="Equipment" Font-Size="Medium"></asp:Label>
                   <label onclick="fnGetEquipment()" class="control-label col-md-1"><i class="fa fa-file-text-o fa-2x" aria-hidden="true"></i></label>
                  <div class="col-xs-3">
                   <asp:DropDownList runat="server" ID="ddl_Equipment" ClientIDMode="Static" CssClass="form-control" Visible="false"></asp:DropDownList>
                   
                </div>
                 
                  <%--<asp:ImageButton ID="imgBtn_GetEquipment" runat="server" Visible="false" ToolTip="Add new equipment" />--%>
            </div>
            <div class="form-group row">
                 <asp:Label ID="Label1" runat="server" CssClass="control-label col-xs-2 text-danger" Text="Type" Font-Size="Medium"></asp:Label>
               <div class="btn-group btn-group-justified" style="width:32%;" data-toggle="buttons" id="radio1">
                    <label class="btn btn-primary" id="rbElectrical">
                        <input type="radio" name="rbTypes" runat="server" id="rbElectrical1"/> Electrical
                    </label>
                      <label class="btn btn-primary" id="rbMechanical" >
                          <input type="radio"  name="rbTypes" runat="server" id="rbMechanical1"/> Mechanical
                    </label>
                      <label class="btn btn-primary" id="rbOther">
                          <input type="radio" name="rbTypes" runat="server" id="rbOther1"/> Other
                    </label>
  
                </div>
<%--                 <asp:RadioButtonList runat="server" ID="rblType" RepeatDirection="Horizontal" Width="100%" CssClass="radio-inline col-xs-6" Visible="false">
                  <asp:ListItem Text="Electrical" Value="E"></asp:ListItem>
                  <asp:ListItem Text="Mechanical" Value="M"></asp:ListItem>
                  <asp:ListItem Text="Other" Value="O"></asp:ListItem>
                </asp:RadioButtonList>--%>
            </div>
       
            <div class="form-group row">
                 <asp:Label ID="Label6" runat="server" CssClass="control-label col-xs-2 text-danger" Text="Response" Font-Size="Medium"></asp:Label>
                 <div class="btn-group btn-group-justified" style="width:32%;" data-toggle="buttons">
                    <label class="btn btn-primary" id="rbRoutine1">
                        <input type="radio"  name="rbResponse" runat="server" id="rbRoutine"/> Routine
                    </label>
                      <label class="btn btn-primary" id="rbScheduled1">
                          <input type="radio"  name="rbResponse" runat="server" id="rbScheduled"/> Scheduled
                    </label>
                      <label class="btn btn-primary" id="rbEmergency1">
                          <input type="radio" name="rbResponse" runat="server" id="rbEmergency"/> Emergency
                    </label>
  
                </div>
                <%--<div class="radio col-xs-4">
               
                <asp:RadioButtonList runat="server" ID="rblResponse" CssClass="radio-inline" RepeatDirection="Horizontal" Visible="false">
                <asp:ListItem Text="Routine" Value="R"></asp:ListItem>
                <asp:ListItem Text="Scheduled" Value="S"></asp:ListItem>
                <asp:ListItem Text="Emergency" Value="E"></asp:ListItem>
               </asp:RadioButtonList>
          
                </div>--%>
            </div>
             <div class="form-group row">
                 <asp:Label ID="Label7" runat="server" CssClass="control-label col-xs-2 text-danger" Text="Condition" Font-Size="Medium"></asp:Label>
                 <div class="btn-group btn-group-justified" style="width:32%;" data-toggle="buttons">
                    <label class="btn btn-primary" id="rbDown1">
                        <input type="radio"  name="rbCondition" runat="server" id="rbDown"/> Down
                    </label>
                      <label class="btn btn-primary" id="rbRunning1">
                          <input type="radio"  name="rbCondition" runat="server" id="rbRunning"/> Running
                    </label>
                </div>
                 
                <%-- <div class="col-xs-5">
           <asp:RadioButtonList runat="server" ID="rblCondition" CssClass="radio-inline " Width="90%" RepeatDirection="Horizontal" Visible="false">
                <asp:ListItem Text="Down" Value="D"></asp:ListItem>
                <asp:ListItem Text="Running" Value="R"></asp:ListItem>                
            </asp:RadioButtonList>
                </div>--%>
            </div>
            <div class="form-group row">
                <asp:Label ID="Label4" runat="server" CssClass="control-label col-xs-2 text-danger" Text="Service Required" Font-Size="Medium"></asp:Label>
            
            <div class="col-xs-4">
                <asp:Label runat="server" ID="lblComments" CssClass="SP_dataLabel" Visible="false"></asp:Label>            
               <asp:TextBox ID="txtComments" TextMode="MultiLine" Rows="5" Columns="70" Width="100%" CssClass="form-control" runat="server" Visible="false"></asp:TextBox>
            </div>
             </div>
            <div class="form-group row">
                <asp:Label ID="Label9" runat="server" CssClass="control-label col-xs-2 text-danger" Text="Originator"></asp:Label>
                <div class="col-xs-4">
                <asp:TextBox ID="txtOriginator" CssClass="form-control" runat="server" Visible="false"></asp:TextBox>
                    </div>
            </div>
              <div class="form-group row">
                <asp:Label runat="server" ID="Label8" CssClass="control-label col-xs-2" Text="Assigned To"></asp:Label>
                  <div class="col-xs-4">
                    <asp:DropDownList runat="server" ID="ddl_AssignedTo" CssClass="form-control" Visible="false"></asp:DropDownList>
                  </div>
            </div>
              <div class="form-group row">
                  <asp:Label runat="server" ID="Label11" CssClass="control-label col-xs-2" Text="Time Worked"></asp:Label>
                  <asp:Panel runat="server" ID="panelTimeWorked" CssClass="control-label col-xs-4"></asp:Panel>
            </div>
              <div class="form-group row">
                  <asp:Label runat="server" ID="lblCreatedByLabel" CssClass="control-label col-xs-2" Text="Created By"></asp:Label>
                  <asp:Label runat="server" ID="lblCreatedBy" CssClass="control-label col-xs-4"></asp:Label>
            </div>
              <div class="form-group row">
                  <asp:Label runat="server" ID="lblDC" CssClass="control-label col-xs-2" Text="Date Created"></asp:Label>
                  <asp:Label runat="server" ID="lblDateCreated" CssClass="control-label col-xs-4"></asp:Label> 
            </div>
              <div class="form-group row">
                  <asp:Label runat="server" ID="lblAttachmentsLabel" CssClass="control-label col-xs-2" Text="Attachments"></asp:Label>
                  <asp:Panel ID="panelAttachments" runat="server" CssClass="control-label col-xs-4"></asp:Panel>
            </div>
            <div class="form-group row">
                <asp:Label ID="lblRequiredFields" runat="server" Font-Size="7pt" Font-Names="Verdana" CssClass="control-label col-xs-3" ForeColor="Red" Text="*Required fields" Visible="false"></asp:Label>
                 <asp:Label ID="lblTimeStamp" runat="server" Font-Size="7pt" CssClass="control-label col-xs-3" Font-Names="Verdana"></asp:Label>
            </div>
        </fieldset>
    </div>

 </div>       


<asp:Panel ID="panelWorkflowTemplates" runat="server" Visible="false">       

    <table style="width:600px">    
    <tr class="SP_Row">
        <td colspan="10"><hr /></td>
    </tr>    

    <tr>
        <td colspan="2">    
            <asp:Label runat="server" ID="Label20" CssClass="SP_itemLabel" Text="Workflow Templates"></asp:Label><br />    
            <asp:DropDownList ID="ddlWorkflowTemplates" runat="server" CssClass="SP_dataLabel"></asp:DropDownList>
        </td>
        <td style="text-align:right">
            <input type="button" id="btnCopyWorkflow" runat="server" value="Load Template" onclick="LoadTemplate_Click();" />
        </td>
    </tr>    
</table>
</asp:Panel>
          
<table style="width:600px;table-layout:fixed">
    <tr><td colspan="18"><hr /></td></tr>
</table>

<asp:PlaceHolder ID="WorkflowStepDisplay" runat="server"></asp:PlaceHolder>

<asp:Panel runat="server" ID="panel_PopUp_Template" style="display:none">
 <div id="popUp_LoadTemplate" runat="server">
    <table id="Table1" border="0" style="background:White" cellpadding="2" cellspacing="0">
    <tr><td colspan="2" style="background:#21374c">
            <asp:Label runat="server" Font-Names="Arial" BackColor="#21374c" ForeColor="White" ID="lblTemplateName" Text="Load Template"></asp:Label>
        </td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr class="SP_Row">        
        <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell1">
            <label for="rbChoice1" class="SP_dataLabel"><input type="radio" id="rbChoice1" name="rbChoice" checked="checked"  class="SP_dataLabel" value="1" />Overwrite</label><br />
            <label for="rbChoice2" class="SP_dataLabel"><input type="radio" id="rbChoice2" name="rbChoice"  class="SP_dataLabel" value="2" />Append</label>
        </td>
    </tr>
    <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>
    <tr>
        <td colspan="2" style="text-align:right;" class="modalButtons">
            <input type="button" id="Button1" style="width:75px;" value="OK" onclick="fnCopyWorkflow(); return false;" />&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnLTCancel" Width="75px" Text="Cancel" OnClientClick="return false;" />            
        </td>
    </tr>
    </table>   
  </div>
    <asp:HiddenField ID="hdnLTJunk" runat="server" />

   <ajaxToolkit:ModalPopupExtender ID="MPE_Template" runat="server" ClientIDMode="Static"
                TargetControlID="hdnLTJunk"
                BackgroundCssClass="modalBackground"
                PopupControlID="popUp_LoadTemplate"                
                DropShadow="true"                 
                CancelControlID="btnLTCancel" /> 

</asp:Panel>


 <asp:Panel runat="server" ID="panelGetUser" style="width:400px;display:none">
 <div id="popUp_NewUser" runat="server">          
    <table id="Table2" border="0" class="tblPadding2" style="background-color:white;width:100%">
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        <tr class="SP_Row">        
            <td style="white-space:nowrap;width:50px" class="SP_dataLabelCell1">
                <asp:Label runat="server" ID="lbl111" Text="User" CssClass="SP_itemLabel2"></asp:Label>
            </td>
            <td style="white-space:nowrap;width:250px" class="SP_dataLabelCell1">
                <asp:TextBox ID="txtName" Width="225px" CssClass="SP_dataLabel" runat="server" AutoCompleteType="None"></asp:TextBox>    
                <asp:Image runat="server" ImageAlign="Middle" ID="imgRequestedBy" CssClass="SP_dataLabel" ImageURL="images/help16.png" tooltip="This field has auto-complete functionality" />
            </td>
        </tr>
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        <tr class="SP_Row"><td>&nbsp;</td></tr>
        
    </table>   
  </div>
    <asp:HiddenField ID="hdnUser_Junk" runat="server" />
    <asp:HiddenField ID="hdnUserType" runat="server" />

     <ajaxToolkit:ModalPopupExtender ID="MPEUser" runat="server" ClientIDMode="Static"
                TargetControlID="hdnUser_Junk"
                BackgroundCssClass="modalBackground"
                PopupControlID="panelGetUser"                
                DropShadow="true"
                CancelControlID="hdnUser_Junk" />

     <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="10" EnableCaching="true"
                                  TargetControlID="txtName" ServiceMethod="GetUserNames" 
                                  CompletionListCssClass="AutoExtender" 
                                  CompletionListItemCssClass="AutoExtenderList" 
                                  CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                  MinimumPrefixLength="2"
                                  FirstRowSelected="true"
                                  CompletionSetCount="15"
                                  OnClientShown ="ZOrderAutoCompleteOptions"
                                  OnClientItemSelected = "AddUser" >    
    </ajaxToolkit:AutoCompleteExtender>       
    
    <asp:HiddenField ID="hdnUserID" runat="server" ClientIDMode="Static" />

</asp:Panel>

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

    function AddUser(source, eventArgs) {
        document.getElementById("hdnUserID").value = eventArgs.get_value();
        if (document.getElementById("hdnUserType").value.length > 0)
            fnNewUser();
    }

    function ZOrderAutoCompleteOptions(control, args) {
        control._completionListElement.style.zIndex = 10000001;
    }

    function Print() {
        window.open("eMailDisplay.aspx?<%=queryString%>&mailMessage=1", "_blank", "", "");
    }

    function Search() {
        window.location = "default.aspx?BP=<%=_BP%>";
        $find("PROCPanel").show();
    }

    function Cancel() {
        location = "mainForm.aspx?<%=queryString%>";
        $find("PROCPanel").show();
    }

    function Edit() {
        location = "mainForm.aspx?<%=queryString%>&edit=1";
        $find("PROCPanel").show();
    }

    function ReturnToList(args) {
        location = "searchresults.aspx"; //?useSearchCookies=1" + args;
        $find("PROCPanel").show();
    }

    function fnAddUser(title, userType) {
        document.getElementById("lblPopUpToolbarTitle").innerText = title;
        document.getElementById("<%=hdnUserType.ClientID%>").value = userType;
        $find("MPEUser").show();
        document.getElementById("<%=txtName.ClientID%>").focus();
        return false;
    }

    function fnNewUser() {
        if (document.getElementById("<%=hdnUserID.ClientID%>").value.length > 0) {
            $find("MPEUser").hide();        
            __doPostBack('newUser', '');
            $find("PROCPanel").show();
        }
        else
            alert("Please select a user");
    }

    function fnHideToolBarPopUp_User() {
        $find("MPEUser").hide();        
        document.getElementById("<%=txtName.ClientID%>").value = "";
    }

    function ResetWorkflow() {
         <%string er = "This will cause the workflow to be reset back to Not Started";
           string er1 = "Continue";%>
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm("<%=er%>" + "!\n" + "<%=er1%>" + "?");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (!retval) {
            return false;
        }
        location = "mainForm.aspx?<%=queryString%>&edit=1&ResetWorkflow=1";
            $find("PROCPanel").show();
    }

     function CancelWF() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm("Are you sure you want to cancel this Request?");
        if (!retval) {
            document.getElementById("darkBackgroundLayer").style.display = "none";
            return;
        }

        var comment = prompt("Enter the comment or reason", '');
        if (comment == null || comment == "") {
            document.getElementById("darkBackgroundLayer").style.display = "none";
            return;
        }
        comment = comment.replace("'", "\'");
                
        document.getElementById("darkBackgroundLayer").style.display = "none";    
        __doPostBack('CancelWF', comment);
        $find("PROCPanel").show();
    }

    function Save() {
        __doPostBack('Save', '');
        $find("PROCPanel").show();
    }

    function Submit() {
        __doPostBack('Submit', '');
        $find("PROCPanel").show();
    }

    function AddAttachment() {
        __doPostBack('AddAttachment', '');
        $find("PROCPanel").show();
    }

    function fnDeptChanged() {
        __doPostBack('Department', '');
        $find("PROCPanel").show();
    }

        function Start() {
            __doPostBack('Start', '');
            $find("PROCPanel").show();
        }

        function Delete() {
            document.getElementById("darkBackgroundLayer").style.display = "";
            var retval = confirm("Are you sure you want to delete this Workfow\nThis action cannot be undone!");
            document.getElementById("darkBackgroundLayer").style.display = "none";
            if (!retval) {
                return;
            }
            __doPostBack('Delete', '');
            $find("PROCPanel").show();
        }

        function AddAttachment() {
            __doPostBack('AddAttachment', '');
            $find("PROCPanel").show();
        }

        function DeleteAttachment(ID, name) {
            document.getElementById("darkBackgroundLayer").style.display = "";
            var retval = confirm("Are you sure you want to delete this attachment: " + name + "\nThis action cannot be undone!");
            document.getElementById("darkBackgroundLayer").style.display = "none";            
            if (!retval) {
                return false;
            }
            __doPostBack('DeleteAttachment', ID);
            $find("PROCPanel").show();
            return false;            
        }
        

    function showExtender(control, args) {
        control._completionListElement.style.zIndex = 10000001;
    }

    // Template methods
    function fnCopyWorkflow() {
        var retval = getCheckedValue(document.getElementsByName("rbChoice"));
        __doPostBack('CopyWF', retval);
        $find("PROCPanel").show();
    }

    function LoadTemplate_Click() {
        var v = document.getElementById("<%=ddlWorkflowTemplates.ClientID%>");
        var option = v.options[v.selectedIndex].value;
        if (option == "0") {
            alert("Please select a template!");
            return false;
        }
        $find("MPE_Template").show();
    }    
    
     function fnGetEquipment() {
        //var prevReturnValue = window.returnValue;
        //window.returnValue = undefined;

        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = prompt("Enter equipment name", "");
        document.getElementById("darkBackgroundLayer").style.display = "none";

        if (retval == null || retval == "")
            return;

        __doPostBack('newEquipment', retval);
        $find("PROCPanel").show();
        //var ddl = document.getElementById("ddl_Equipment");
        //var option = document.createElement('option');
        //option.value = retval;
        //option.innerHTML = retval;
        //ddl.appendChild(option);
        //ddl.value = retval;        
     }
    (function () {

         console.log('Starting from console');

         document.getElementById('rbElectrical').addEventListener('click', function () {

             document.getElementById('rbElectrical').setAttribute('class', 'btn');
             document.getElementById('rbMechanical').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbOther').setAttribute('class', 'btn btn-primary');

         });

         document.getElementById('rbMechanical').addEventListener('click', function () {

             document.getElementById('rbMechanical').setAttribute('class', 'btn');
             document.getElementById('rbElectrical').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbOther').setAttribute('class', 'btn btn-primary');

         });
         document.getElementById('rbOther').addEventListener('click', function () {

             document.getElementById('rbOther').setAttribute('class', 'btn');
             document.getElementById('rbMechanical').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbElectrical').setAttribute('class', 'btn btn-primary');

         });

         document.getElementById('rbRoutine1').addEventListener('click', function () {

             document.getElementById('rbRoutine1').setAttribute('class', 'btn');
             document.getElementById('rbScheduled1').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbEmergency1').setAttribute('class', 'btn btn-primary');

         });

         document.getElementById('rbScheduled1').addEventListener('click', function () {

             document.getElementById('rbScheduled1').setAttribute('class', 'btn');
             document.getElementById('rbRoutine1').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbEmergency1').setAttribute('class', 'btn btn-primary');

         });
         document.getElementById('rbEmergency1').addEventListener('click', function () {

             document.getElementById('rbEmergency1').setAttribute('class', 'btn');
             document.getElementById('rbScheduled1').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbRoutine1').setAttribute('class', 'btn btn-primary');

         });

         document.getElementById('rbDown1').addEventListener('click', function () {
             document.getElementById('rbRunning1').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbDown1').setAttribute('class', 'btn');
         });

         document.getElementById('rbRunning1').addEventListener('click', function () {
             document.getElementById('rbDown1').setAttribute('class', 'btn btn-primary');
             document.getElementById('rbRunning1').setAttribute('class', 'btn');
         });
     })();
</script>

</asp:Content>