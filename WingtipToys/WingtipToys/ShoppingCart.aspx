<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="WingtipToys.ShoppingCart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="ShoppingCartTitle" runat="server" class="contentHead">
        <h1>Shopping Cart</h1>
    </div>
    <table width ="100%">
        <tr>
            <td>
                <asp:Button ID="UpdateBtn" runat="server" Text="Update" OnClick="UpdateBtn_Click" /> 
            </td>
            <td>
                <asp:Button ID="EmptyCart" runat="server" Text="Empty Cart" OnClick="EmptyCart_Click"/> 
            </td>
            <td style="text-align:right">
                <asp:Button ID="CheckoutBtn" runat="server" Text="Check Out" OnClick="CheckoutBtn_Click" />
            </td>
        </tr>
        <tr><td><br /></td></tr>
    </table>
    <asp:GridView ID="CartList" runat="server" AutoGenerateColumns="false" ShowFooter="true" GridLines="Vertical" CellPadding="4" ItemType="WingtipToys.Models.CartItem" SelectMethod="GetShoppingCartItems" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField ="ProductID" HeaderText="ID" SortExpression="ProductID"/>
            <asp:BoundField DataField ="Product.ProductName" HeaderText="Name" SortExpression="Product.ProductName" />
            <asp:BoundField DataField ="Product.UnitPrice" HeaderText="Price (Each)" DataFormatString="{0:c}"/>
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="PurchaseQuantity" width="40" runat="server" Text="<%#:Item.Quantity %>"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
               
            <asp:TemplateField HeaderText="Item Total">
                <ItemTemplate>
                    <%#:String.Format("{0:c}",Convert.ToDouble(Item.Quantity) * Convert.ToDouble(Item.Product.UnitPrice)) %>
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remove Items">
                <ItemTemplate>
                    <asp:CheckBox id="Remove" runat="server"></asp:Checkbox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div>
        <asp:Label ID="LabelTotalText" runat="server" Text="Order Total:"></asp:Label>
        <asp:Label ID="lblTotal" runat="server" EnableViewState="false"></asp:Label>
    </div>
</asp:Content>
