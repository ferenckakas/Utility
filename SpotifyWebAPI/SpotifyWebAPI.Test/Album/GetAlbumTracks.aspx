<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetAlbumTracks.aspx.cs" Inherits="SpotifyWebAPI.Test.Album.GetAlbumTracks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContentPlaceHolder" runat="server">
    Album - Get Album Tracks
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="descriptionContentPlaceHolder">
    Get Spotify catalog information about an album’s tracks. Optional parameters can be used to limit the number of tracks returned
</asp:Content>
