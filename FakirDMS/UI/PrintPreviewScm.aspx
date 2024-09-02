<%@ Page Title="Print Preview | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrintPreviewScm.aspx.cs" Inherits="FakirDMS.UI.PrintPreviewScm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="hfCategoryId" />
            <asp:HiddenField runat="server" ID="hfDocumentId" Value="0" />

            <%--General Information--%>
            <div class="panel panel-info">
                <div class="panel-header">General Information (<span runat="server" id="divPageHeader"></span>)</div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-1">Company</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtCompany" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Expense Type</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtItemType" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            Entry Date
                        </div>
                        <div class="col-2">
                            <asp:TextBox ID="txtEntryDate" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Tracking No.</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtBillRefNo" runat="server" placeholder="Tracking No" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                        <div class="row">
                        <div class="col-1">Po No</div>
                        <div class="col-2">
                        <asp:TextBox ID="txtPoNo" runat="server" TextMode="MultiLine"  placeholder="Po No" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Remarks</div>
                        <div class="col-5">
                            <asp:TextBox ID="txtRemarks" runat="server" placeholder="Remarks for this document (if any)" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            Ref# Tracking.
                        </div>
                        <div class="col-2">
                            <asp:TextBox ID="txtRefTracking" runat="server" placeholder="Ref. Tracking No" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <%--PO & MRR Information--%>
            <div class="row" style="margin-top: 15px">
                <%--Purchase Order Information--%>
                <div class="col-6 scrollable">
                    <asp:GridView ID="gvPurchaseOrder" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Purchase Order" DataField="RefNo" />
                            <asp:BoundField HeaderText="Supplier" DataField="SupplierName" />
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                </div>
                 <div class="col-6 scrollable">
                    <%--MRR Information--%>
                    <asp:GridView ID="gvMRR" runat="server" Width="100%" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                    BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <Columns>
                    <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                    <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                    <asp:BoundField HeaderText="MRR No." DataField="RefNo" />
                    <asp:BoundField HeaderText="MRR Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                    <asp:BoundField HeaderText="Challan No." DataField="ChallanNo" />
                    <asp:BoundField HeaderText="Challan Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                    </Columns>
                    <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                     </div>
            </div>

         
                <%--Challan Information--%>
                <%--<div class="col-4  scrollable">
                    <asp:GridView ID="gvChallan" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Challan" DataField="RefNo" />
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                </div>--%>
            </div>

            <%--Bill Information--%>
            <div class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Party Bill Information</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-1">Bill No</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtBillNo" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Bill Amount</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtBillAmount" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Discount Amount</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtAuditAmount" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Final Amount</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtFinalAmount" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1">
                            Bill Date
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtBillDate" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            VAT Amount
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVatAmount" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            VAT Challan
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVATChallanNo" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            VAT Date
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVATDate" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>


                        <%--Account Information--%>
            <div class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Accounts Information</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-2">Payable Voucher No.</div>
                        <div class="col-3">
                            <asp:TextBox runat="server" ID="txtVoucherNo" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-2">Payable Voucher Date</div>
                        <div class="col-3">
                            <asp:TextBox runat="server" ID="txtVoucherDate" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <%--<div class="col-"></div>
                        <div class="col-2">
                            <asp:CheckBox runat="server" ID="cbBillClosed" Text="Bill Close" Enabled="False" CssClass="CheckBoxStyle" />
                        </div>--%>
                    </div>
                    <div class="row" runat="server" ID="secPayable">
                        <div class="col-2">Payment Voucher No.</div>
                        <div class="col-3">
                            <asp:TextBox runat="server" ID="txtPaymentVoucherNo" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-2">Payment Voucher Date</div>
                        <div class="col-3">
                            <asp:TextBox runat="server" ID="txtPaymentVoucherDate" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top: 15px">
                <div class="col-6">

                    <%--Comments Regarding The Documents--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Comments Regarding The Documents</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvComment" runat="server" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-6">
                    <%--Documents Preview--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Documents Preview</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvAttachment" runat="server" Width="100%"
                                AutoGenerateColumns="false" BackColor="#FCFCFC"
                                BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px" CellPadding="8" CellSpacing="4"
                                ShowHeaderWhenEmpty="True" HorizontalAlign="Center" CssClass="ssGridToggle">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="TransID" HeaderText="TransId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="Name" HeaderText="Document Type" />
                                    <asp:BoundField DataField="Title" HeaderText="File Name" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Upload By" />
                                    <asp:BoundField DataField="EntryDate" HeaderText="Upload Date" HeaderStyle-Width="120px" />
                                    <asp:TemplateField HeaderText="Preview" HeaderStyle-Width="50px">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="gBtnPreview" OnClick="gBtnPreview_Click" ImageUrl="~/assets/img/preview.png" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No attachemnt file found for the search criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvAttachment" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
