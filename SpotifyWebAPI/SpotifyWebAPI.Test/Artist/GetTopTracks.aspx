<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetTopTracks.aspx.cs" Inherits="SpotifyWebAPI.Test.Artist.GetTopTracks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContentPlaceHolder" runat="server">
    Artist - Get Top Tracks
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="descriptionContentPlaceHolder">
    Get Spotify catalog information about an artist’s top tracks by country.
</asp:Content>
