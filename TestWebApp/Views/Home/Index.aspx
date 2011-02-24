<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    if (!TestLibrary.MyStartupCode.StartCalled || !TestLibrary.MyStartupCode.Start2Called ||
        !TestLibrary.MyStartupCode.CallMeAfterAppStartCalled || !TestWebApp.TestStartupCode.MyStartupCode.StartCalled ||
        !AppCodeStartupCode.Called) {
        throw new Exception("Startup methods were not correctly called");
    }
%>

    <h2>All startup methods were successfully called!</h2>
</asp:Content>
