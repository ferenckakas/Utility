<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetArtistAlbums.aspx.cs" Inherits="SpotifyWebAPI.Test.Album.GetArtistAlbums" %>

<asp:Content ID="content1" runat="server" ContentPlaceHolderID="titleContentPlaceHolder">
    Album - Get Artist Albums
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="descriptionContentPlaceHolder">
    Get Spotify catalog information about an artist’s albums. Optional parameters can be specified in the query string to filter and sort the response.
</asp:Content>