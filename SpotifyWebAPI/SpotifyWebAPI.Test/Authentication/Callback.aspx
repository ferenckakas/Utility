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
            <div>        
                <h2>Object Output</h2>    
                <p>
                    <asp:Literal ID="output" runat="server" />
                </p>           
            </div>
        </form>
    </body>
</html>
