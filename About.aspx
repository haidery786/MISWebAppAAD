<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="MISWebAppAAD.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your application description page.</h3>
    <p>Use this area to provide additional information.</p>

    <div style="padding: 50px">

           <h3>Authentication:</h3>
        <table class="table table-striped table-bordered table-hover">
            <tr>
                <td>IsAuthenticated</td>
                <td>
                  <%= HttpContext.Current.User.Identity.IsAuthenticated %></td>
            </tr>
            <tr>
                <td>AuthenticationType</td>
                <td><%= HttpContext.Current.User.Identity.AuthenticationType %></td>
            </tr>
         
        </table>

        <h3>Main Claims:</h3>
        <table class="table table-striped table-bordered table-hover">
            <tr>
                <td>Name</td>
                <td>
                    <asp:Label runat="server" ID="lblName"></asp:Label></td>
            </tr>
            <tr>
                <td>Username</td>
                <td>
                    <asp:Label runat="server" ID="lblUsername"></asp:Label></td>
            </tr>
            <tr>
                <td>Subject</td>
                <td>
                    <asp:Label runat="server" ID="lblSubject"></asp:Label></td>
            </tr>
            <tr>
                <td>TenantId</td>
                <td>
                    <asp:Label runat="server" ID="lblTenantId"></asp:Label></td>
            </tr>
        </table>

        <br />
        <br />

        <asp:Button runat="server" OnClick="LogOut" Text="Sign Out" class="btn btn-primary btn-block btn-flat"/>
     
    </div>

    <script type="text/javascript">
        function Logout() {
            $("#btnLogout").click();
        }
    </script>


</asp:Content>
