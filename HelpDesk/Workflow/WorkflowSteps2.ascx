<%@ Control Language="C#" AutoEventWireup="True" Inherits="WF.WorkflowSteps2" EnableViewState="true" ClassName="clsWS2" CodeBehind="WorkflowSteps2.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

        <table border="<%=_border%>" cellpadding="0" cellspacing="0" style="width:<%=_tableWidth%>px;table-layout:fixed">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblHeaderLabel" Font-Size="11pt" CssClass="SP_itemLabel" Text="Workflow"></asp:Label>
                    </td>
                     <%if (wf._enableTemplateMenu)
                        {%>
                            <td style="text-align:right;vertical-align:bottom"> 
                                <img alt="" title="Click for options" onclick="fnMenu1(this, arguments[0], '0', '0', '0');return false" src="http://<%=wf._webServer%><%=wf._appPath %>/workflow/images/Settings16.png" />
                            </td>
                      <%}%>    
                </tr>
        </table>           

       <asp:Panel Width="400px" runat="server" ID="panelComment" style="display:none">            
        <asp:Table runat="server" Width="100%" CellPadding="0" CellSpacing="0">
            <asp:TableRow runat="server" Visible="true">
                <asp:TableCell Width="100%" Height="100%" ColumnSpan="1" BackColor="White">
                    <asp:TextBox runat="server" ID="txtComments" ClientIDMode="Static" CssClass="SP_dataLabel" Height="100%" Width="98%" TextMode="MultiLine" Rows="8"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" Visible="true">
                <asp:TableCell HorizontalAlign="Right" Height="100%" Width="100%" CssClass="SP_dataLabel" BackColor="White">
                    <asp:Button ID="btnSaveSignOff" runat="server" Width="75px" Text="OK" Font-Size="8pt" Font-Names="Arial" OnClientClick="return fnModifyComment();" />
                     &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancelSignOff" runat="server" Width="75px" Font-Size="8pt" Font-Names="Arial" Text="Cancel" />&nbsp;&nbsp;&nbsp;
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Height="100%" Width="100%" CssClass="SP_dataLabel" BackColor="White" ColumnSpan="1">
                    &nbsp;
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>    

    <asp:Button runat="server" ID="btnFake" style="display:none"></asp:Button>   
    <asp:ModalPopupExtender ID="MPE1" runat="server" PopupControlID="panelComment" TargetControlID="btnFake" ClientIDMode="Static"
                BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnCancelSignOff">    
    </asp:ModalPopupExtender>
    <br />    
    <asp:Panel runat="server" ID="panelSteps"></asp:Panel>         
    
    <asp:PlaceHolder runat="server" ID="phMenu1"></asp:PlaceHolder>
    
    <asp:HiddenField runat="server" ID="hdnStepUserID" ClientIDMode="Static" />
    
    <div id="WFdarkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>

<style type="text/css">    
.modalBackground
{    
    background-color:#CCCCFF;
    filter:alpha(opacity=40);
    opacity:0.5;
}
.modalButtons div {
    display:inline;
}
</style>

<script type="text/javascript">
var jmnuStep = null;
var jmnuWorkflow = null;
var stepID = null;
var stepDesc = null;
var stepNumber = null;


    function fnAddComment(stepUsersID) {
        document.getElementById("hdnStepUserID").value = stepUsersID;        
        $find('MPE1').show();
        document.getElementById("txtComments").focus();
    }

    function fnModifyComment() {        
        $find('MPE1').hide();
        var txtComments = document.getElementById("txtComments");
        if(txtComments.value.trim().length == 0)
            return false;        
        showSpinner();
        var stepUserID = document.getElementById("hdnStepUserID").value;
        var userID = "<%=wf._userID%>";
        var BP = "<%=wf._BP%>";
        var useAD = "<%=wf._usesAD%>";
        var retVal = PageMethods.updateComment(stepUserID, txtComments.value, BP, userID, useAD, onModifyComments);
        return false; // true;
    }

    function onModifyComments(response) {
        hideSpinner();
        //alert(response);
        var stepUserID = document.getElementById("hdnStepUserID").value;
        var labelComment = document.getElementById("labelComments_" + stepUserID);
        labelComment.innerText = response;
        var txtComments = document.getElementById("txtComments").value = "";
        //alert(labelComment.innerText);
    }

    function showSpinner() {
        try {
            $find("PROCPanel").show();
        }
        catch (ex) { }
    }

    function hideSpinner() {
        try {
           $find("PROCPanel").hide();
        }
        catch (ex) { }
    }

function fnMenu1(stepImage, eventObj, aStepID, aStepDesc, aStepNumber) {
    stepID = aStepID;
    stepDesc = aStepDesc;
    stepNumber = aStepNumber;
    if (eventObj)
    { }
    else
        eventObj = window.event;
    fnShowWFMenu(stepImage, "myMenu1", eventObj);
}

function fnMenu2(stepImage, eventObj, aStepID, aStepDesc, aStepNumber) {
    stepID = aStepID;
    stepDesc = aStepDesc;
    stepNumber = aStepNumber;
    if (eventObj)
    { }
    else
        eventObj = window.event;
    fnShowWFMenu(stepImage, "myMenu2", eventObj);
}

function fnMenuModifyStep()
{
    retval = fnModifyStep(stepID);
    if (retval == true)
        __doPostBack('', '');        
}

function fnMenuInsertStep()
{
    retval = fnInsertStep(stepID);
    if (retval == true)
        __doPostBack('', '');        
}

function fnMenuDeleteStep() {
    document.getElementById("WFdarkBackgroundLayer").style.display = "";  
    var retval = confirm("Delete " + stepDesc + "\nThis cannot be undone!");
    document.getElementById("WFdarkBackgroundLayer").style.display = "none";
    if(retval == true)            
        __doPostBack('DeleteStep', stepID);    
}

function fnMenuResetStep()
{
    __doPostBack('ResetStep', stepNumber);
}

function fnMenuEmail()
{   
    __doPostBack('Email', stepID);    
}

function fnMenuMoveUp()
{    
    __doPostBack('MoveUp', stepID); 
}

function fnMenuMoveDown()
{    
    __doPostBack('MoveDown', stepID); 
}
    
function showUsers(userID) 
{    
    if(document.getElementById(userID).style.display == "none")
    {
        document.getElementById(userID + "_1").style.display = "none";
        document.getElementById(userID).style.display = "block";
    }
    else
    {
        document.getElementById(userID).style.display = "none";
        document.getElementById(userID + "_1").style.display = "block";
    }
    return false;
}

var args = "BP=<%=wf._BP%>&workflowID=<%=wf._workflowID%>&Template=false";
function fnModifyStep(stepID) {
    document.getElementById("WFdarkBackgroundLayer").style.display = "";
    var args1 = args + "&stepID=" + stepID;
    retval = window.showModalDialog("workflow/modifyUsers.aspx?" + args1, "", "dialogHeight:625px;dialogWidth:625px;center:yes;scroll:yes;resizable:yes;status:no;unadorned:yes;");
    document.getElementById("WFdarkBackgroundLayer").style.display = "none";
    if(retval == true)
        return retval;
    return false;    
}

function fnInsertStep(stepID) {
    document.getElementById("WFdarkBackgroundLayer").style.display = "";
    var args1 = args + "&stepID=" + stepID;
    retval = window.showModalDialog("workflow/InsertStep.aspx?" + args1, "", "dialogHeight:575px;dialogWidth:550px;center:yes;scroll:yes;resizable:yes;status:no;unadorned:yes;");
    document.getElementById("WFdarkBackgroundLayer").style.display = "none";
    if(retval == true)
        return retval;
    return false;    
}
     
    function URLDecode(psEncodeString) {
        // Create a regular expression to search all +s in the string
        var lsRegExp = /\+/g;
        // Return the decoded string
        return unescape(String(psEncodeString).replace(lsRegExp, " "));
    }   

    function trim(s) {
        return s.replace(/^\s+|\s+$/g, "");
    }

    function strip(html) {
        var tmp = document.createElement("DIV");
        tmp.innerHTML = html;
        return tmp.textContenct || tmp.innerText;
    }    
    
</script>

            
