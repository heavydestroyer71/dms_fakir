<%@ Page Title="Payment Confirm | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PaymentConfirm.aspx.cs" Inherits="FakirDMS.UI.PaymentConfirm" %>

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
					<div class=" row">
						<div class="panel panel-info">
							<div class="panel-header"></div>
							<div class="panel-body">
								<div class="row">
									<div class="col-md-6 col-sm-3">Payment Voucher No</div>
									<div class="col-md-6 col-sm-9">
										<asp:TextBox runat="server" ID="txtpaymentVoucherNo" placeholder="Payment Voucher" CssClass="TextBoxStyle"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-md-6 col-sm-3">Payment Voucher Date</div>
									<div class="col-md-6 col-sm-9">
										<div class="calendarContainer">
											<asp:TextBox runat="server" ID="txtFromDate" placeholder="MM/DD/YYYY" CssClass="TextBoxStyle"></asp:TextBox>
											<asp:RegularExpressionValidator ID="Validator_txtFromDate" runat="server"
												ControlToValidate="txtFromDate" Display="Dynamic" ForeColor="Red" ValidationExpression="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"
												ErrorMessage=" Date Format must be Month/Day/Year."></asp:RegularExpressionValidator>
											<asp:MaskedEditExtender ID="Masked_txtFromDate" runat="server"
												Enabled="True" Mask="99/99/9999" MaskType="Date"
												OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
												TargetControlID="txtFromDate">
											</asp:MaskedEditExtender>
											<asp:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="CalenderTheme"
												PopupButtonID="txtFromDate" TargetControlID="txtFromDate" Format="MM/dd/yyyy">
											</asp:CalendarExtender>
										</div>

									</div>
								</div>
								<div class="row">
									<div class="col-md-8" align="center">
									</div>
									<div class="col-md-3" align="right">
										<asp:Button runat="server" OnClick="btnSubmit_Click" Text="Submit" Style="margin-left: auto; display: block;" CssClass="btn btn-success" ID="btnSubmit" />
									</div>

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

			<div class="row" style="margin-top: 15px">
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
