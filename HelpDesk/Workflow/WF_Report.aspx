<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="../MasterPages/Tools.master" Inherits="WF._Report" CodeBehind="WF_Report.aspx.cs" %>
<%@ MasterType VirtualPath="../MasterPages/Tools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<br />
  
<asp:PlaceHolder ID="phControls" runat="server"></asp:PlaceHolder>          


<script lang="Javascript" type="text/javascript">    
  
    function fnClose() {
        try {
            parent.Close();
        }
        catch (Exception)
        { }
    }
</script>

</asp:Content>