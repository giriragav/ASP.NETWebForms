<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="WingtipToys.ProductDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="productDetail" runat="server" ItemType="WingtipToys.Models.Product" SelectMethod="GetProduct" RenderOuterTable="false">
        <ItemTemplate>
            <div>
                <h1><%#:Item.ProductName %></h1>
            </div>
            <br />
            <table>
                <tr>
                    <td>
                        <img src="Catalog/Images/<%#:Item.ImagePath %>" style="border:solid;height:300px" alt="<%#:Item.ProductName %>"/>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <b>Description:</b>
                        <br />
                        <span><%#:Item.Description %></span>
                        <br />
                        <span><b>Price: </b><%#:String.Format("{0:c}",Item.UnitPrice)%></span>
                        <br />
                        <span><b>Product Number: </b><%#:Item.ProductID %></span>
                        <br />
                        <a href="ShoppingCart.aspx?productID=<%#:Item.ProductID %>">
                            <span class="ProductListItem">
                                <b>Add to Cart</b>
                            </span>
                        </a>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
