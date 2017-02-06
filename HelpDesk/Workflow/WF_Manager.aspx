<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="../MasterPages/Tools.master" Inherits="WF._Manager" CodeBehind="WF_Manager.aspx.cs" %>

<%@ MasterType VirtualPath="../MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" EnablePageMethods="true" runat="server" />
    <ol class="breadcrumb">
        <li><a href="/default.aspx">Home</a></li>
        <li><a href="/admin.aspx">Adminstration</a></li>
        <li class="active">Workflows</li>
    </ol>
    <div class="list-group" style="width:200px">
        <div class="list-group-item">
            <asp:Label ID="lblTName" runat="server" Text="Template Name" CssClass="list-group-item-heading"></asp:Label>
             <asp:DropDownList runat="server" ID="ddl_Templates" Width="120" CssClass="list-group-item-text">
              </asp:DropDownList>        
        </div>
        <div class="list-group-item">
            <asp:Label ID="lblLocation" runat="server" Text="Location" CssClass="list-group-item-heading"></asp:Label>          
            <asp:DropDownList runat="server" ID="ddl_Location" Width="120" CssClass="list-group-item-text">
                </asp:DropDownList>
        </div>
    </div>
    <br />

    <asp:PlaceHolder ID="panelSteps" runat="server"></asp:PlaceHolder>
    <br />
    <asp:Label runat="server" ID="lblMsg" BackColor="White" ForeColor="Red" Font-Size="Small" Font-Bold="false" Visible="false"></asp:Label>

    <asp:Panel runat="server" ID="panel_PopUp_Template" Style="display: none">
        <div id="popUp_LoadTemplate">
            <table id="Table1" border="0" style="background: White" cellpadding="2" cellspacing="0">
                <tr>
                    <td colspan="2" style="background: #21374c">
                        <asp:Label runat="server" Font-Names="Arial" BackColor="#21374c" ForeColor="White" ID="lblTemplateName" ClientIDMode="Static" Text="Copy Template"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr class="SP_Row">
                    <td style="width: 100px" class="SP_dataLabelCell1">
                        <asp:Label runat="server" ID="Label2" CssClass="SP_itemLabel2" Text="Name"></asp:Label>
                    </td>
                    <td style="white-space: nowrap; width: 250px" class="SP_dataLabelCell1">
                        <asp:TextBox runat="server" ID="txtNewTemplateName" CssClass="SP_dataLabel" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="SP_Row">
                    <td class="SP_dataLabelCell1">
                        <asp:Label runat="server" ID="lblxxx" CssClass="SP_itemLabel2" Text="Location"></asp:Label>
                    </td>
                    <td style="white-space: nowrap; width: 250px" class="SP_dataLabelCell1">
                        <asp:TextBox runat="server" ID="txtLocation" CssClass="SP_dataLabel" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right;" class="modalButtons">
                        <input type="button" id="Button1" style="width: 75px;" value="OK" onclick="fnCopyWorkflow(); return false;" />&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnLTCancel" Width="75px" Text="Cancel" OnClientClick="return false;" />
                    </td>
                </tr>
            </table>
        </div>

        <asp:HiddenField ID="hdnLTJunk" runat="server" />

        <ajaxToolkit:ModalPopupExtender ID="MPE1" runat="server" ClientIDMode="Static"
            TargetControlID="hdnLTJunk"
            BackgroundCssClass="SP_darkenBackground"
            PopupControlID="panel_PopUp_Template"
            DropShadow="true"
            CancelControlID="btnLTCancel" />
        </asp:Panel>


    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" ClientIDMode="Static"
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel"
        DropShadow="false"
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" Style="display: none; width: 0px; background: #ffffff; border: 0px solid #668dae;" runat="server">
        <img style="border: none" src="images/ajax-loader.gif" alt="" />
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

    <asp:PlaceHolder runat="server" ID="phMenu1"></asp:PlaceHolder>

    <div runat="server" id="darkBackgroundLayer" class="SP_darkenBackground" style="display: none" ></div>

    <script type="text/javascript">
        function Return() {
            window.location = "../admin.aspx?BP=<%=wf._BP%>";
        $find("PROCPanel").show();
    }

    function newTemplate() {
        document.getElementById("darkBackgroundLayer").style.display = "";
        retval = prompt("Enter new template name", "");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (retval == null || retval == "")
            return;
        __doPostBack('newTemplate', retval);
        $find("PROCPanel").show();
    }

    function deleteStep(stepID) {
      <%string d = "Are you sure you want to delete this Step";
        string d1 = "This action cannot be undone";%>
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm("<%=d%>" + "\n" + "<%=d1%>" + "!");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (!retval) {
            return;
        }
        __doPostBack('deleteStep', stepID);
        $find("PROCPanel").show();
    }

    function deleteTemplate() {
      <%string d2 = "Are you sure you want to delete this template";
        string d3 = "This action cannot be undone";%>
        document.getElementById("darkBackgroundLayer").style.display = "";
        var retval = confirm("<%=d2%>" + "\n" + "<%=d3%>" + "!");
        document.getElementById("darkBackgroundLayer").style.display = "none";
        if (!retval) {
            return;
        }
        __doPostBack('deleteTemplate', '');
        $find("PROCPanel").show();
    }

    function copyTemplate() {
        $find("MPE1").show();
    }

    function fnCopyWorkflow() {
        __doPostBack('copyTemplate', '');
        $find("PROCPanel").show();
    }

    function fnTemplateChanged() {
        __doPostBack('templateChanged', '');
        $find("PROCPanel").show();
    }

    function fnLocationChanged() {
        __doPostBack('locationChanged', '');
        $find("PROCPanel").show();
    }

    var stepID = null;
    var stepDesc = null;
    var stepNumber = null;

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

    function fnMenuModifyStep() {
        __doPostBack('modifyStep', stepID);
    }

    function fnMenuInsertStep() {
        __doPostBack('insertStep', stepID);
    }

    function fnMenuDeleteStep() {
        deleteStep(stepID);
    }

    function fnMenuMoveUp() {
        __doPostBack('moveUp', stepID);
    }

    function fnMenuMoveDown() {
        __doPostBack('moveDown', stepID);
    }



    </script>

</asp:Content>
