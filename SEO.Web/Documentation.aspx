<%@ Page Title="Documentation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Documentation.aspx.cs" Inherits="SEO.Web.Documentation" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<velyo:MarkdownContent runat="server" path="~/README.md" />
</asp:Content>
