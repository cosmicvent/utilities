<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SettingsServiceSample.Controllers.HomeViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index</title>
</head>
<body>
    <b>WelcomeMessage:</b>
    <%= Html.Encode(Model.WelcomeMessage) %><br />
    <b>Version</b>
    <%= Html.Encode(Model.Version) %><br />
    <b>PublishDate</b>
    <%= Html.Encode(String.Format("{0:g}", Model.PublishDate)) %>
</body>
</html>
