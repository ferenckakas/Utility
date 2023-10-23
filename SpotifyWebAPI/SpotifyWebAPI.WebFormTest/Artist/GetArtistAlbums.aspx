<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetArtistAlbums.aspx.cs" Inherits="SpotifyWebAPI.Test.Artist.GetArtistAlbums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContentPlaceHolder" runat="server">
    Artist - Get Artist Albums
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="descriptionContentPlaceHolder" runat="server">
    Get Spotify catalog information about an artist’s albums. Optional parameters can be specified in the query string to filter and sort the response.
</asp:Content>