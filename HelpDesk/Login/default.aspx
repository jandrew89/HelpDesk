<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Tools.master"
    Inherits="LabRequest.LoginDefault" EnableEventValidation="false" CodeBehind="default.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPages/Tools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="navbar navbar-inverse">
        <div class="container-fluid">
            <div class="navbar-header">
                <div class="navbar-brand">User Login</div>
            </div>
        </div>
    </div>
    <br />
    <div class="container col-md-11">
        <div class="module form-module">
            <div class="toggle">
                <i class="fa fa-times fa-pencil"></i>
            </div>
            <div class="form">
                <h2>Login to your workflow</h2>
                <div>
                    <input type="text" placeholder="Username" id="txtUserID" runat="server" />
                    <input type="password" placeholder="Password" id="txtPWD" runat="server" />
                    <button name="btnSubmit" onserverclick="btnSubmit_Click" runat="server">Login</button>
                </div>
            </div>
            <div class="form">
                <h2>Create an account</h2>
                <div>
                    <input type="text" placeholder="Username" />
                    <input type="password" placeholder="Password" />
                    <input type="email" placeholder="Email Address" />
                    <input type="tel" placeholder="Phone Number" />
                    <button>Register</button>
                </div>
            </div>
            <div class="cta" id="lblMsg" runat="server"></div>
        </div>
    </div>



    <br />
    <br />
    <asp:LinkButton runat="server" ID="lbHelp" Visible="false" Text="Click here to request access or for help" OnClientClick="btnHelp_Click"></asp:LinkButton>
    <script type="text/javascript">
        //ADD SPinner

    </script>

</asp:Content>
