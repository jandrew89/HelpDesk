<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" Inherits="QA._searchResults" EnableEventValidation="false" CodeBehind="SearchResults.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" runat="server" />

<asp:GridView ID="GV" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both" BorderColor="#d8d8d8" 
        BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="true" PagerSettings-Position="TopAndBottom" AllowSorting="true"
        DataKeyNames="WFID" Height="50px" BorderStyle="None" PageSize="50" Font-Size="8pt" CssClass="SP_dataLabel" Width="99%" 
         OnRowCreated="GV_RowCreated" OnSorting="GV_Sorting"  OnPageIndexChanging="GV_PageIndexChanging" onrowdatabound="GV_RowDataBound">
            <FooterStyle BackColor="#CCCC99" />
            <Columns>                
                               
             </Columns>

            <PagerTemplate>                
                <table style="width:100%">
                    <tr>
                    <td style="white-space:nowrap;text-align:center">
                    <div id="pager">
                    <asp:ImageButton ImageAlign="AbsBottom" ID="imgFirst" runat="server" CommandArgument="First" CommandName="Page" OnClientClick="PageIndexChanged();" ImageUrl="images/DataContainer_MoveFirstHS.png" />
                    <asp:ImageButton ImageAlign="AbsBottom" ID="imgPrevious" runat="server" CommandArgument="Prev" CommandName="Page" OnClientClick="PageIndexChanged();" ImageUrl="images/DataContainer_MovePreviousHS.png" />
                    <asp:Label ID="lblPageLabel" runat="server" Text="Page" Font-Names="Arial" Font-Size="7pt"></asp:Label>
                    <asp:DropDownList ID="ddlPageSelector" Font-Names="Arial" Font-Size="8pt" runat="server"></asp:DropDownList>                                    
                    <asp:ImageButton ImageAlign="AbsBottom" ID="imgNext" runat="server" CommandArgument="Next" CommandName="Page" OnClientClick="PageIndexChanged();" ImageUrl="images/DataContainer_MoveNextHS.png" />
                    <asp:ImageButton ImageAlign="AbsBottom" ID="imgLast" runat="server" CommandArgument="Last" CommandName="Page" OnClientClick="PageIndexChanged();" ImageUrl="images/DataContainer_MoveLastHS.png" />                                       
                    </div>          
                    </td>
                   
                    </tr>
                    </table>  
            </PagerTemplate>
            
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <HeaderStyle ForeColor="White" CssClass="SP_dataLabel_Header" Font-Bold="true" />
            <EmptyDataTemplate>
                &nbsp;
            </EmptyDataTemplate>
           <RowStyle Font-Size="12pt" BackColor="#ebf3ff" Font-Names="Verdana" />
           <AlternatingRowStyle BackColor="White" Font-Size="12pt" Font-Names="Verdana" />
           <EditRowStyle Font-Size="Medium" />
           <EmptyDataRowStyle Font-Size="Medium" />
        </asp:GridView>
                   

   <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server"  />     
    
<script type="text/javascript">
    function Search() {
        window.location = "default.aspx?BP=<%=_BP%>";
        $find("<%=PROCPanel.ClientID%>").show();
    }   

    function DownloadToExcel() {
        __doPostBack('DownloadToExcel', '');
        $find("<%=PROCPanel.ClientID%>").show();
        setTimeout(function () { $find("<%=PROCPanel.ClientID%>").hide(); }, 2000);
    }

    function fnPageSelect(id) {
        __doPostBack('PageSelect', id.options[id.selectedIndex].value);
        $find("<%=PROCPanel.ClientID%>").show();
        return false;
    }

    function PageIndexChanged() {
        $find("<%=PROCPanel.ClientID%>").show();
        return true;
    }

    function fnLink(BP, ID) {
        return false;
    }

    function fnSort(ID, column) {
        __doPostBack(ID, column);
        $find("<%=PROCPanel.ClientID%>").show();
    }
 
</script>


</asp:Content>