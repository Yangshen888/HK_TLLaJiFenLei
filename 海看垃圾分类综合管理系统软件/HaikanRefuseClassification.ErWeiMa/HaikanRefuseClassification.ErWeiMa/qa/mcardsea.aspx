<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mcardsea.aspx.cs" Inherits="HaikanDemo.qa.mcardsea1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%=Request.QueryString["cardid"] %>
<%=Request.QueryString["mjihao"] %>
<%=Request.QueryString["cjihao"] %>
<%=Request.QueryString["status"] %>
<%=Request.QueryString["time"] %>
        </div>
    </form>
</body>
</html>
