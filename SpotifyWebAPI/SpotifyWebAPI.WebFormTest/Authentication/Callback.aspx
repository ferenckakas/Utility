<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Callback.aspx.cs" Inherits="SpotifyWebAPI.Test.Authentication.Callback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:LinkButton ID="userGetUser" runat="server" Text="User - Get User" OnClick="userGetUser_Click" />
            <br />
            <asp:LinkButton ID="userGetCurrentUserProfile" runat="server" Text="User - Get Current User Profile (Authentication Required)" OnClick="userGetCurrentUserProfile_Click" />
            <br />
            <asp:LinkButton ID="userGetSavedTracks" runat="server" Text="User - Get Current User Saved Tracks (Authentication Required)" OnClick="userGetSavedTracks_Click" />
            <br />
            <asp:LinkButton ID="userSaveTracks" runat="server" Text="User - Save Tracks (Authentication Required)" OnClick="userSaveTracks_Click" />
            <br />
            <asp:LinkButton ID="userIsTrackSaved" runat="server" Text="User - Is Track Saved (Authentication Required)" OnClick="userIsTrackSaved_Click" />
            <br />
            <asp:LinkButton ID="userDeleteTracks" runat="server" Text="User - Delete Tracks (Authentication Required)" OnClick="userDeleteTracks_Click" />
            <br />
            <asp:LinkButton ID="userGetPlaylist" runat="server" Text="User - Playlist (Authentication Required)" OnClick="userGetPlaylist_Click" />
            <br />
            <asp:LinkButton ID="userGetPlaylists" runat="server" Text="User - Playlists (Authentication Required)" OnClick="userGetPlaylists_Click" />
            <br />
            <asp:LinkButton ID="userCreatePlaylist" runat="server" Text="User - Create Playlist (Authentication Required)" OnClick="userCreatePlaylist_Click" />
            <br />
            <asp:LinkButton ID="userUpdatePlaylist" runat="server" Text="User - Update Playlist (Authentication Required)" OnClick="userUpdatePlaylist_Click" />
            <br />
            <asp:LinkButton ID="userAddToPlaylist" runat="server" Text="User - Add track(s) to playlist (Authentication Required)" OnClick="userAddToPlaylist_Click" />
            <br />
            <asp:LinkButton ID="userBrowseNewAlbums" runat="server" Text="User - Browse New Album Releases (Authentication Required)" OnClick="userBrowseNewAlbums_Click" />
            <br />
            <asp:LinkButton ID="userBrowserFeaturedPlaylists" runat="server" Text="User - Browse Featured Playlists (Authentication Required)" OnClick="userBrowserFeaturedPlaylists_Click" />
            <br />
            <br />
            <asp:LinkButton ID="LinkButton1" runat="server" Text="A" OnClick="A_Click" />
            <asp:TextBox ID="TextBox1" runat="server" Height="117px" TextMode="MultiLine" Width="1179px" />
            <asp:LinkButton ID="LinkButton2" runat="server" Text="B" OnClick="B_Click" />
            <br />
            <br />
            <asp:TextBox ID="TextBoxTitle" runat="server" Text="New"/>
            <br />
            <asp:CheckBox ID="CheckBoxPreFilter" runat="server" Text="Prefilter" />
            <br />
            <asp:TextBox ID="TextBox2" runat="server" Height="300px" TextMode="MultiLine" Width="1179px" />
            <br />
            <asp:LinkButton ID="userCreatePlaylistFromTxt" runat="server" Text="T" OnClick="T_Click" />
            <asp:LinkButton ID="LinkButton3" runat="server" Text="K" OnClick="K_Click" />
            <br />
            <div>        
                <h2>Object Output</h2>    
                <p>
                    <asp:Literal ID="output" runat="server" />
                </p>           
            </div>
            <div id="TableDiv" runat="server"></div>
            <br />
            <asp:table ID="Table" runat="server"></asp:table>
            <br />
            Not Found:
            <br />
            <asp:TextBox ID="TextBoxNotFound" runat="server" Height="300px" TextMode="MultiLine" Width="1179px" />
        </form>
    </body>
</html>
