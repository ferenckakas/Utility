<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="GetRelatedArtists.aspx.cs" Inherits="SpotifyWebAPI.Test.Artist.GetRelatedArtists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="titleContentPlaceHolder" runat="server">
    Artist - Get Related Artists
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="descriptionContentPlaceHolder" runat="server">
    Get Spotify catalog information about artists similar to a given artist. Similarity is based on analysis of the 
    Spotify community’s <a href="http://news.spotify.com/se/2010/02/03/related-artists/" target="_blank">listening history.</a>
</asp:Content>
