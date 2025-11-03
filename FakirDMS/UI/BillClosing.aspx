<%@ Page Title="Bill Closing | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BillClosing.aspx.cs" Inherits="FakirDMS.UI.BillClosing" %>

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
			<div runat="server" id="searchPanel" class="panel panel-info" style="margin-top: 15px">
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
			<div class="row">
				<div class="col-md-1">
					<asp:TextBox
						ID="txtTracking"
						runat="server"
						AutoPostBack="true"
						OnTextChanged="txtTracking_TextChanged"
						PlaceHolder ="Tracking No"
						CssClass="form-control">
					</asp:TextBox>
				</div>
			</div>
			<div class="row" style="margin-top: 15px">
				<div class="col-md-9">
					<div class="panel panel-info">
						<div class="panel-header">Documents List (<asp:Label ID="lblWorkflowCount" runat="server" Text="0"></asp:Label>)</div>
						<div class="panel-body" runat="server" id="gridContainer" style="overflow-y: auto; overflow-x: hidden; height: 400px;">

							<asp:GridView ID="gvDocuments" runat="server"
								AutoGenerateColumns="false" AllowPaging="false" PageSize="20" Width="100%"
								ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
								BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
								<AlternatingRowStyle BackColor="WhiteSmoke" />
								<Columns>
									<asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-CssClass="50px" ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<asp:HiddenField runat="server" ID="gvHfDocumentId" Value='<%#Eval("DocumentID") %>' />
											<asp:CheckBox runat="server" ID="gvCbSelect" AutoPostBack="True" 
    OnCheckedChanged="gvCbSelect_CheckedChanged" />
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
									<asp:BoundField DataField="VoucherNo" HeaderText="Payable Voucher No" />
									<asp:BoundField DataField="VoucherDate" HeaderText="Payable Voucher Date" />
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
					<div runat="server" id="divStoreLocation" class="panel panel-info">
						<div class="panel-header">Store Location</div>
						<div class="panel-body">
							<div class="row">
								<div class="col-md-4">
									Room Name
								</div>
								<div class="col-md-8">
									<asp:DropDownList runat="server" ID="box_ddlRoomName" OnSelectedIndexChanged="box_ddlRoomName_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
								</div>
							</div>
							<div class="row">
								<div class="col-md-4">
									Rack Name
								</div>
								<div class="col-md-8">
									<asp:DropDownList runat="server" ID="box_ddlRackName" OnSelectedIndexChanged="box_ddlRackName_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
								</div>
							</div>
							<div class="row">
								<div class="col-md-4">
									Shelf Name
								</div>
								<div class="col-md-8">
									<asp:DropDownList runat="server" ID="box_ddlShelfName" CssClass="DropDownListStyle" OnSelectedIndexChanged="box_ddlShelfName_SelectedIndexChanged"  AutoPostBack="true" ></asp:DropDownList>
								</div>
							</div>
							<div class="row">
								<div class="col-md-4">
									Box Name
								</div>
								<div class="col-md-8">
									<asp:DropDownList ID="box_ddlBoxfName" runat="server" CssClass="DropDownListStyle" ></asp:DropDownList>
								</div>
							</div>
							<div class="row">
								<div class="col-md-8" align="right"></div>
								<div class="col-md-3" align="right">
									<asp:Button runat="server" OnClick="btnSubmit_Click" Text="Submit" Style="margin-left: auto; display: block;" CssClass="btn btn-success" ID="btnSubmit" />
								</div>
							</div>
						</div>
			</div>
					<div class="row">
						<div class="panel panel-info">
							<div class="panel-header"></div>
							<div class="panel-body">
								<div class="row">
									<div class="col-md-3" align="center">
										Reason
									</div>
									<div class="col-md-9" align="center">
										<asp:TextBox runat="server" ID="txtRemarksBoss" TextMode="MultiLine" Rows="2" CssClass="TextBoxStyle"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-md-8" align="center">
										<asp:DropDownList runat="server" ID="ddlRevertTo" Style="height: 38px;" CssClass="DropDownListStyle"></asp:DropDownList>
									</div>
									<div class="col-md-3" align="right">
										<asp:Button runat="server" ID="btnReject" Text="Reject" CssClass="btn btn-warning" OnClick="btnReject_Click" />
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>

			<div class="row" runat="server" id="divSubmitted" style="margin-top: 15px">
				<div class="panel panel-info">
					<div class="panel-header">Submitted Documents List (<asp:Label ID="lblSubmittedDocCount" runat="server" Text="0"></asp:Label>)</div>
					<div class="panel-body" runat="server" id="gridContainer_submit" style="overflow-y: auto; overflow-x: hidden; height: 400px;">

						<asp:GridView ID="gvDocumentsSubmitted" runat="server"
							AutoGenerateColumns="false" AllowPaging="false" PageSize="20" Width="100%"
							ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
							BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
							<AlternatingRowStyle BackColor="WhiteSmoke" />
							<Columns>
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
								<asp:BoundField DataField="EntryDate" HeaderText="Entry Date" />
								<asp:BoundField DataField="PaymentVoucherNo" HeaderText="Voucher No" />
								<asp:BoundField DataField="PaymentVoucherDate" HeaderText="Voucher Entry Date" />
								<asp:BoundField DataField="SubmittedBy" HeaderText="Submitted by" />
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
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
