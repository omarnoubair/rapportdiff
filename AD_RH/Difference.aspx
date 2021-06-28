<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Difference.aspx.cs" Inherits="AD_RH.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-left: auto; margin-right: auto; text-align: center;">
    
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="margin-left: 50px" Text="Différence AD" Width="150px" />
        <asp:Button ID="Button2" runat="server" style="margin-left: 50px" Text="Différence Millenium" Width="130px" OnClick="Button2_Click" />
            <asp:Button ID="Button6" runat="server" Text="Différence Compta" style="margin-left: 50px" Width="130px" OnClick="Button6_Click" />
        <asp:Button ID="Button3" runat="server" style="margin-left: 50px" Text="Différence Avanteam" Width="130px" OnClick="Button3_Click" />
        <asp:Button ID="Button4" runat="server" style="margin-left: 50px" Text="Différence Titanium" Width="130px" OnClick="Button4_Click" />
    
    
    </div>
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Label ID="Label3" runat="server" Text="Fichier des utilisateurs Titanium"></asp:Label>
                <asp:FileUpload ID="FileUpload1" runat="server" style="margin-left: 22px" Width="254px" />
                <asp:Button ID="Button5" runat="server" Text="ok" Width="88px" style="margin-left: 22px" OnClick="Button5_Click" />
            </div>
            <p>

                <asp:Label ID="Label4" runat="server" style="margin-left: 422px" Text="BC: "></asp:Label><asp:DropDownList AutoPostBack="true" ID="DropDownList1" runat="server" Height="38px" Width="218px" style="margin-left: 22px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
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
    <div style="margin-left: auto; margin-right: auto; text-align: center;">
        <p>
            <asp:Label ID="Label2" runat="server" Text="Label" ont-Bold="true" Font-Size="X-Large" CssClass="StrongText"></asp:Label>
        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" style="margin-right: auto; margin-left: auto"  Width="499px">
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
        </p>
        </div>
    </form>
</body>
</html>
