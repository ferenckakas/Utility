<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetArtist.aspx.cs" Inherits="SpotifyWebAPI.Test.Artist.GetArtist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContentPlaceHolder" runat="server">
    Artist - Get Artist
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="descriptionContentPlaceHolder">
    Get Spotify catalog information for a single artist identified by their unique Spotify ID.
</asp:Content>
