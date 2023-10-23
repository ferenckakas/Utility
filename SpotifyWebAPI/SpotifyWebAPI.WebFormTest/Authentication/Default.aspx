<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SpotifyWebAPI.Test.Authentication.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">
            <h2>Spotify Web API Authentication Testing</h2>
            <h3>NOTE: you must go to spotify and create your own Client Id and Client Secret</h3>
            <h4>Client Id:</h4>
            Required. The client ID provided to you by Spotify when you register your application. 
            <br />
            <br />
            <asp:TextBox ID="clientIdTextBox" runat="server" Width="300px" Text="" />
            <h4>Client Secret:</h4>
            Required. The client secret provided to you by Spotify when you register your application. 
            <br />
            <br />
            <asp:TextBox ID="clientSecretTextBox" runat="server" Width="300px" Text="" />
            <h4>Redirect Uri:</h4>
            Required. The URI to redirect to after the user grants/denies permission. This URI needs to have been entered in the 
            Redirect URI whitelist that you specified when you registered your application. The value of redirect_uri here must 
            exactly match one of the values you entered when you registered your application, including upper/lowercase, terminating slashes, etc.
            <br />
            <br />
            <asp:TextBox ID="redirectUriTextBox" runat="server" Text="" Width="300px" />
            <h4>Access Scope(s):</h4>
            Optional. A space-separated list of scopes: see Using Scopes. If no scopes are specified, authorization will be granted only to
            access publicly available information: that is, only information normally visible in the Spotify desktop, web and mobile players. 
            <br />
            <br />
            <asp:CheckBoxList ID="scopeCheckBoxList" runat="server">
                <asp:ListItem Text="playlist-modify-public" Value="playlist-modify-public" Selected="True" />
                <asp:ListItem Text="playlist-modify-private" Value="playlist-modify-private" Selected="True" />
                <asp:ListItem Text="playlist-read-private" Value="playlist-read-private" Selected="True" />
                <asp:ListItem Text="streaming" Value="streaming" Selected="True" />
                <asp:ListItem Text="user-library-modify" Value="user-library-modify" Selected="True" />
                <asp:ListItem Text="user-library-read" Value="user-library-read" Selected="True" />
                <asp:ListItem Text="user-read-private" Value="user-read-private" Selected="True" />
                <asp:ListItem Text="user-read-email" Value="user-read-email" Selected="True" />
            </asp:CheckBoxList>
            <br />
            <br />
            <asp:Button id="startLoginProcess" runat="server" Text="Start Login Process" OnClick="startLoginProcess_Click" />
        </form>
    </body>
</html>
