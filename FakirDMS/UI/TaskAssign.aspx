<%@ Page Title="Task Assign | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TaskAssign.aspx.cs" Inherits="FakirDMS.UI.TaskAssign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <link href="../assets/css/dashboard.css" rel="stylesheet" />
    <style type="text/css">
        .info-box .info-box-icon {
            height: 90px !important;
            width: 60px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>


            <div class="panel panel-info" style="margin-top: 15px">
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
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtRefNo" placeholder="Search with tracking number" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtSearchBy" placeholder="Search with PO, LC, MRR and Challan" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-2">
                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button runat="server" ID="btnReload" Text="Reload" OnClick="btnReload_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top: 15px">
                <div class="col-md-9">
                    <div class="panel panel-info">
                        <div class="panel-header">Documents List (<asp:Label ID="lblWorkflowCount" runat="server" Text="0"></asp:Label>)</div>
                        <div class="panel-body">

                            <asp:GridView ID="gvDocuments" runat="server"
                                AutoGenerateColumns="false" AllowPaging="false" PageSize="20" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-CssClass="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="gvHfDocumentId" Value='<%#Eval("DocumentID") %>' />
                                            <asp:CheckBox runat="server" ID="gvCbSelect" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tracking No.">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblTo" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>' Target="_blank"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Company" HeaderText="Company" />
                                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                    <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
                                    <asp:BoundField DataField="PoNo" HeaderText="PO No." />
                                    <asp:BoundField DataField="Supplier" HeaderText="Supplier Name" />
                                    <asp:BoundField DataField="MrrNo" HeaderText="MRR No." />
                                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" HeaderStyle-Width="95px" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No workflow document found for the search criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="panel panel-info">
                        <div class="panel-header">Team Member</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-3 col-sm-3">
                                    Member
                                </div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:DropDownList runat="server" ID="ddlTeamMember" OnSelectedIndexChanged="ddlTeamMember_SelectedIndexChanged" AutoPostBack CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">Designation</div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:TextBox runat="server" ID="txtDesignation" Enabled="false" placeholder="Designation" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">Department</div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:TextBox runat="server" ID="txtDepartment" Enabled="false" placeholder="Department" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">Company</div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:TextBox runat="server" ID="txtCompany" Enabled="false" placeholder="Company" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-sm-3">Allocated Days</div>
                                <div class="col-md-9 col-sm-9">
                                    <asp:TextBox runat="server" ID="txtAllocatedDays" placeholder="Number of days" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12" align="center">
                                    <div style="margin-top: 20px;">
                                        <asp:Button runat="server" ID="btnSaveTask" OnClick="btnSaveTask_Click" Text="Task Assign" Width="150px" CssClass="btn btn-md btn-success" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
