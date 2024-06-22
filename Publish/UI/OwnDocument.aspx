<%@ Page Title="Own Document | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="OwnDocument.aspx.cs" Inherits="FakirDMS.UI.OwnDocument" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>

            <div class="panel panel-info">
                <div class="panel-header">Document - Search Criteria</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-1">
                            Company
                        </div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Category</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">
                            Expense Type
                        </div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlExpenseType" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Status</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlStatus" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1">
                            Tracking No
                        </div>
                        <div class="col-2">
                            <asp:TextBox ID="txtRefNo" runat="server" placeholder="Search with tracking number" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            Search With
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtPartyName" placeholder="Search with PO, LC, MRR and Challan" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-3">
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button runat="server" ID="btnReload" Text="Reload" OnClick="btnReload_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div style="margin-top: 20px"></div>
            <div class="panel panel-info">
                <div class="panel-header">Own Documents (<asp:Label ID="lblDraftCount" runat="server" Text="0"></asp:Label>)</div>
                <div class="panel-body">
                    <asp:GridView ID="gvDraftDocuments" runat="server"
                        OnPageIndexChanging="gvDraftDocuments_PageIndexChanging"
                        AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="TranID" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn">
                                <ItemTemplate>
                                    <asp:Label ID="lblTranID" runat="server" Text='<%#Eval("DocumentID") %>'></asp:Label>
                                    <asp:Label ID="lblBillRefNo" runat="server" Text='<%#Eval("BillRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Tracking No.">
                                <ItemTemplate>
                                    <asp:HyperLink ID="ghlTracking" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ref Tracking">
                                <ItemTemplate>
                                    <asp:HyperLink ID="ghlRefTracking" runat="server" NavigateUrl='<%#Eval("RefDocumentPath") %>' Text='<%#Eval("RefDocumentNo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Company" HeaderText="Company" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Item Category" />
                            <asp:BoundField DataField="ReqNo" HeaderText="Req. No." />
                            <asp:BoundField DataField="PoNo" HeaderText="PO No." />
                            <asp:BoundField DataField="PiNo" HeaderText="PI No." />
                            <asp:BoundField DataField="LcNo" HeaderText="LC No." />
                            <asp:BoundField DataField="MrrNo" HeaderText="MRR No." />
                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" HeaderStyle-Width="95px" />
                            <asp:BoundField DataField="LastModified" HeaderText="Last Update" HeaderStyle-Width="95px" />
                            <asp:BoundField DataField="Waiting" HeaderText="Waiting" HeaderStyle-Width="80px" />
                        </Columns>
                        <EmptyDataTemplate>
                            No Own document found for the search criteria.
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="GridViewHeader" />
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updateProgress" runat="server">
        <ProgressTemplate>
            <div class="loading-panel">
                <div class="loading-container">
                    <center>
                        <div style="background-color: white; height: 120px; width: 400px; padding-top: 50px;" class="border border-info rounded-5">
                            <span>Processing, Please wait a moment...</span>
                            <br />
                            <img src="<%= this.ResolveUrl("~/Images/loading-logo.gif")%>" width="350px" alt="Please wait..." />
                        </div>
                    </center>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
