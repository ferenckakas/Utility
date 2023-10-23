<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetAlbums.aspx.cs" Inherits="SpotifyWebAPI.Test.Album.GetAlbums" %>

<asp:Content ID="content1" runat="server" ContentPlaceHolderID="titleContentPlaceHolder">
    Album - Get Albums
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="descriptionContentPlaceHolder">
    Get Spotify catalog information for multiple albums identified by their Spotify IDs.
</asp:Content>