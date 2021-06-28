<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Avanteam.aspx.cs" Inherits="AD_RH.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
    <div style="margin-left: auto; margin-right: auto; text-align: center;">
    
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="margin-left: 50px" Text="Recherche utilisateur" Width="177px" />
        <asp:Button ID="Button2" runat="server" style="margin-left: 50px" Text="Profils d'utilisateur inactif" Width="177px" OnClick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" style="margin-left: 50px" Text="Profils des utilisateurs actifs" Width="177px" OnClick="Button3_Click" />
    
    </div>
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
    
                <asp:Label ID="Label3" runat="server" Text="Chercher un utilisateur"></asp:Label>
                <asp:TextBox ID="textbox3" runat="server" style="margin-left: 22px" Width="254px" />
                <asp:Button ID="Button5" runat="server" Text="ok" Width="88px" style="margin-left: 22px" OnClick="Button5_Click" />
            </div>
            <p>

                <asp:DropDownList AutoPostBack="true" Width="250px" style="margin-left: 522px" ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                </asp:DropDownList>


            </p>
    <div style="margin-left: auto; margin-right: auto; text-align: center;">
        <asp:Label ID="Label1" runat="server" Text="Label" ont-Bold="true" Font-Size="X-Large" CssClass="StrongText"></asp:Label>
        </div>    
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="499px" style="margin-right: auto; margin-left: auto">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    
    </form>
</body>
</html>
