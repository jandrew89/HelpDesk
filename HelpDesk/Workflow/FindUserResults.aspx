<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Tools.master" 
    Inherits="QA.FindUserResults" CodeBehind="FindUserResults.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="True" EnablePageMethods="true" runat="server" />
<br />
<asp:GridView ID="GV" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="Both" BorderColor="#d8d8d8"
        BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="false" AllowSorting="false"
        DataKeyNames="WFID" Height="51px" BorderStyle="None" Font-Size="8pt" CssClass="SP_DataLabel" Width="60%"        
        onrowdatabound="GV_RowDataBound">
            <FooterStyle BackColor="#CCCC99" />
            <Columns>     
                 <asp:BoundField DataField="userName" HeaderText="User">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>
                                                           
                <asp:BoundField DataField="friendlyBPName" HeaderText="Location">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Template Name">                    
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hypTemplate" CssClass="SP_dataLabel" Target="_blank"></asp:HyperLink>                        
                   </ItemTemplate>                   
                </asp:TemplateField>   

                <asp:BoundField DataField="stepDescription" HeaderText="Step Description">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>                 

                 <asp:BoundField DataField="CreatedByFriendlyName" HeaderText="Created By">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>    

                 <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:d}">
                    <ItemStyle Wrap="False" CssClass="SP_dataLabel" />
                    <HeaderStyle CssClass="SP_dataLabel_Header" HorizontalAlign="Left" />
                </asp:BoundField>                                  
               
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
        
            <%--//************ PROCPanel **************************//--%>
    <ajaxToolkit:ModalPopupExtender ID="PROCPanel" runat="server" 
        TargetControlID="hdnProc"
        PopupControlID="ProcessingPanel" 
        DropShadow="false" 
        BackgroundCssClass="modalBackground" />

    <asp:Panel ID="ProcessingPanel" style="display:none;width:0px;background:#ffffff;border:0px solid #668dae;" runat="server">        
        <img style="border:none" src="images/ajax-loader.gif" alt="" />       
    </asp:Panel>
    <asp:HiddenField ID="hdnProc" runat="server" />

<script lang="Javascript" type="text/javascript">
    function Return() {
        window.location = "findUser.aspx?BP=0";
        $find("<%=PROCPanel.ClientID%>").show();
    }
</script>

</asp:Content>