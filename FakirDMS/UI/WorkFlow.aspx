<%@ Page Title="Document List | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WorkFlow.aspx.cs" Inherits="FakirDMS.WorkFlow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
	<style type="text/css">
		:root {
			--primary-color: #2c3e50;
			--secondary-color: #3498db;
			--accent-color: #e74c3c;
			--light-color: #ecf0f1;
			--success-color: #27ae60;
			--warning-color: #f39c12;
		}

		body {
			font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
			background-color: #f8f9fa;
			color: #333;
		}

		.header {
			background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
			color: white;
			padding: 15px 0;
			/*margin-bottom: 20px;*/
			box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
		}

		.card {
			border-radius: 6px;
			box-shadow: 0 2px 6px rgba(0, 0, 0, 0.08);
			/*margin-bottom: 25px;*/
			border: none;
		}

		.card-header {
			background-color: var(--primary-color);
			color: white;
			font-weight: 600;
			border-radius: 6px 6px 0 0 !important;
			padding: 10px 15px;
			font-size: 0.95rem;
		}

		.filter-section {
			background-color: var(--light-color);
			padding: 12px;
			border-radius: 6px;
		}

		/* Updated compact card styles */
		.summary-card {
			text-align: center;
			padding: 12px 10px;
			transition: all 0.2s ease;
			height: 80%;
			border-left: 4px solid transparent;
		}

			.summary-card:hover {
				transform: translateY(-3px);
				box-shadow: 0 4px 8px rgba(0, 0, 0, 0.12);
			}

			.summary-card .card-title {
				font-size: 0.85rem;
				/*margin-bottom: 8px;*/
				font-weight: 600;
				color: #555;
			}

		.summary-value {
			font-size: 1.4rem;
			font-weight: bold;
			margin: 5px 0;
			line-height: 1.2;
		}

		.summary-card .card-text {
			font-size: 0.75rem;
			margin-bottom: 0;
			color: #777;
		}

		.progress {
			height: 8px;
			margin: 8px 0;
			border-radius: 4px;
		}

		.progress-bar {
			border-radius: 4px;
		}

		.table th {
			background-color: var(--primary-color);
			color: white;
			position: sticky;
			top: 0;
			font-size: 0.85rem;
			padding: 8px 10px;
		}

		.table td {
			padding: 8px 10px;
			font-size: 0.85rem;
			vertical-align: middle;
		}

		.badge {
			font-size: 0.75rem;
			padding: 4px 8px;
			border-radius: 10px;
		}

		.badge-success {
			background-color: var(--success-color);
		}

		.badge-warning {
			background-color: var(--warning-color);
		}

		.badge-danger {
			background-color: var(--accent-color);
		}

		.filter-btn {
			background-color: var(--secondary-color);
			color: white;
			border: none;
			padding: 6px 12px;
			font-size: 0.85rem;
		}

			.filter-btn:hover {
				background-color: var(--primary-color);
				color: white;
			}

		.section-title {
			border-left: 3px solid var(--secondary-color);
			padding-left: 8px;
			margin: 15px 0 12px 0;
			color: var(--primary-color);
			font-size: 1.1rem;
		}

		.chart-container {
			position: relative;
			height: 220px;
			width: 100%;
		}

		.multi-select {
			min-height: 90px;
			font-size: 0.85rem;
		}

		.form-label {
			font-size: 0.9rem;
			margin-bottom: 5px;
			font-weight: 500;
		}

		.form-select {
			font-size: 0.85rem;
			padding: 5px 10px;
		}

		.form-text {
			font-size: 0.75rem;
		}

		.gridview-header {
			white-space: nowrap;
		}

		.percent-column {
			text-align: center;
			font-weight: bold;
		}

		/* Toggle button in the top right corner */
		.card-header {
			display: flex;
			justify-content: space-between;
			align-items: center;
		}

		.toggle-btn {
			background: transparent;
			border: none;
			color: white;
			font-size: 0.9rem;
			padding: 2px 8px;
		}

		@media (max-width: 768px) {
			.card-responsive {
				overflow-x: auto;
			}

			.summary-value {
				font-size: 1.2rem;
			}

			.section-title {
				font-size: 1rem;
			}
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<div class="">
		<h6 class="section-title">Workflow Document (<asp:Label ID="lblWorkFlowDocCount" runat="server" Text="0"></asp:Label>)</h6>
		<div class="card">
			<div style="overflow-x: auto" class="table-responsive card-responsive">
				<asp:GridView ID="gvWorkflowDocument" runat="server"
					AutoGenerateColumns="false" Width="100%" OnRowCommand="gvWorkflowDocuments_RowCommand"
					ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="table table-striped table-hover">
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
						<asp:BoundField DataField="CategoryName" HeaderText="Category" />
						<asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
						<asp:BoundField DataField="PartyBillNo" HeaderText="Bill No" />
						<asp:BoundField DataField="PartyBillDate" HeaderText="Bill Date" />
						<asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
						<asp:BoundField DataField="PoNo" HeaderText="PO No." />
						<asp:BoundField DataField="MrrNo" HeaderText="MRR No." />
						<asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" />
						<asp:BoundField DataField="Assignee" HeaderText="Assignee" />
						<asp:BoundField DataField="Status" HeaderText="Status" />
						<asp:BoundField DataField="CurrentUser" HeaderText="User" />
						<asp:BoundField DataField="RoleName" HeaderText="Flow Name" />
						<asp:TemplateField HeaderStyle-Width="30px">
							<ItemTemplate>
								<asp:ImageButton ID="sbtnViewHistory" runat="server" CommandName="ViewHistory" Width="40px" ImageUrl="~/assets/img/history.png" ToolTip="Click here to view document movement history" />
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>
						Sorry! No workflow document has been found.
					</EmptyDataTemplate>
					<HeaderStyle CssClass="GridViewHeader" />
					<FooterStyle CssClass="GridViewFooterStyle" />
					<PagerStyle CssClass="GridViewPagerStyle" />
				</asp:GridView>
			</div>
		</div>
	</div>


	<asp:LinkButton Text="" ID="lnkFake" runat="server" />
	<asp:ModalPopupExtender runat="server" ID="modalExtenderHistory" PopupControlID="PanelDetails" TargetControlID="lnkFake"
		PopupDragHandleControlID="PopupDetailsHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
	</asp:ModalPopupExtender>
	<asp:Panel runat="server" ID="PanelDetails" Style="display: none;">
		<div class="HellowWorldPopup">
			<div class="PopupHeader" id="PopupDetailsHeader">
				<div style="width: 100%; display: table;">
					<div style="min-width: 550px; display: table-cell;">
						<span style="font-weight: bold; font-size: larger; padding-left: 10px;">Movement History</span>
					</div>
					<div style="display: table-cell; vertical-align: middle;" align="right">
						<asp:ImageButton ID="ImageButton2" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
					</div>
				</div>
			</div>
			<div class="PopupBody">
				<div width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%; padding: 10px;" cellpadding="0" cellspacing="0">
					<asp:GridView ID="gvHistory" runat="server" Width="100%"
						BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px"
						ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle">
						<AlternatingRowStyle BackColor="WhiteSmoke" />
						<HeaderStyle CssClass="GridViewHeader" />
						<FooterStyle CssClass="GridViewFooterStyle" />
						<PagerStyle CssClass="GridViewPagerStyle" />
						<PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
					</asp:GridView>
				</div>
			</div>
		</div>
	</asp:Panel>
</asp:Content>
