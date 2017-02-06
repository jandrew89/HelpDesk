<%@ Page Language="C#" AutoEventWireup="True" Inherits="QA.SearchResultsSettings"  MasterPageFile="~/MasterPages/Tools.master" CodeBehind="SearchResultsSettings.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">     
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />

    <table width="100%">	    
        <tr class="SP_Row"><td style="white-space:nowrap;width:1%">&nbsp</td></tr>                    				    
        <tr class="SP_Row"><td style="white-space:nowrap;width:1%">&nbsp</td>				                                				    
		    <td class="SP_itemLabelCell" style="white-space:nowrap;" colspan="5">
                <asp:label id="lblColumnSelect" runat="server" CssClass="SP_itemLabel" Visible="false" ForeColor="Purple" Font-Size="Medium">Select Columns</asp:label>
		    </td>
        </tr>
				           
	    <tr class="SP_Row"><td style="white-space:nowrap;width:1%">&nbsp</td>	
		    <td style="white-space:nowrap;width:30%;vertical-align:top">
                 <asp:Table runat="server" ID="tblSearch" CellPadding="2" CellSpacing="0" Width="100%"></asp:Table>
		    </td>
		    <td style="white-space:nowrap;width:30%;vertical-align:top">
                 <asp:Table runat="server" ID="tblExcel" CellPadding="2" CellSpacing="0" Width="100%"></asp:Table>
		    </td>            
        </tr>        
        
        <tr class="SP_Row"><td style="white-space:nowrap;width:1%">&nbsp</td></tr>                    				    
				    
	                
    </table>

    <br />       

    
   
   <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="true" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img border="0" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

    <div runat="server" id="darkBackgroundLayer" class="SP_darkenBackground" style="display:none"></div>

    <script type="text/javascript" language="javascript">
        function fnSave() {
            __doPostBack('Save', '');
            $find("<%=PROCPanel.ClientID%>").show();
        }

        function fnReturn() {
            __doPostBack('Return', '');
            $find("<%=PROCPanel.ClientID%>").show();
        }
    </script>
</asp:Content>

