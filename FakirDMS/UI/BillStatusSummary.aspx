<%@ Page Title="Document Flow Analysis | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BillStatusSummary.aspx.cs" Inherits="FakirDMS.UI.BillStatusSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
	<div class="container-fluid">
		<div class="row">
			<div class="col-12">
				<div class="d-flex justify-content-between align-items-center">
					<h2 class="text-primary"><i class="fas fa-tachometer-alt me-2"></i>Document Flow Analysis</h2>
					<div>
						<span class="badge bg-info"><i class="fas fa-database me-1"></i>Live Data</span>
						<span class="badge bg-secondary ms-2"><i class="fas fa-clock me-1"></i>
							<asp:Label ID="lblLastUpdated" runat="server" Text=""></asp:Label></span>
					</div>
				</div>
				<%--<p class="text-muted">Monitor document processing performance and timelines</p>--%>
			</div>
		</div>

		<!-- Filters Section -->
		<div class="card mb-3">
			<div class="card-header">
				<span><i class="fas fa-filter me-1"></i>Report Filters</span>
				<asp:LinkButton ID="btnToggleFilters" runat="server" CssClass="toggle-btn" OnClick="btnToggleFilters_Click">
                    <i class="fas fa-chevron-down"></i>
				</asp:LinkButton>
			</div>
			<asp:Panel ID="pnlFilters" runat="server" CssClass="card-body" Style="padding: 12px;">
				<div class="row">
					<div class="col-md-4 mb-2">
						<label for="ddlFlow" class="form-label">Flow</label>
						<asp:DropDownList ID="ddlFlow" runat="server" CssClass="form-select" AppendDataBoundItems="true">
							<asp:ListItem Text="All Flows" Value=""></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div class="col-md-4 mb-2">
						<label for="ddlCategory" class="form-label">Category (Single)</label>
						<asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" AppendDataBoundItems="true">
							<asp:ListItem Text="All Categories" Value=""></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div class="col-md-2 mb-2">
					</div>
					<div class="col-md-2 mb-2">
						<%--<label for="lstCategories" class="form-label">Categories (Multiple)</label>
                        <asp:ListBox ID="lstCategories" runat="server" CssClass="form-select multi-select" SelectionMode="Multiple"></asp:ListBox>
                        <small class="form-text text-muted">Hold Ctrl to select multiple categories</small>--%>
						<asp:Button ID="Button1" runat="server" Text="Apply Filters" CssClass="btn filter-btn me-2" OnClick="btnApplyFilters_Click" />
						<asp:Button ID="Button2" runat="server" Text="Reset" CssClass="btn btn-secondary btn-sm" OnClick="btnResetFilters_Click" />
					</div>
				</div>
				<%--<div class="row mt-2">
                    <div class="col-12 text-end">
                        <asp:Button ID="btnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn filter-btn me-2" OnClick="btnApplyFilters_Click" />
                        <asp:Button ID="btnResetFilters" runat="server" Text="Reset" CssClass="btn btn-secondary btn-sm" OnClick="btnResetFilters_Click" />
                    </div>
                </div>--%>
			</asp:Panel>
		</div>

		<!-- Summary Cards -->
		<h3 class="section-title">Performance Overview</h3>
		<div class="row">
			<div class="col-lg-3 col-6">
				<!-- small box -->
				<div class="small-box bg-info">
					<div class="inner">
						<h3>
							<asp:Label ID="lblTotalDocuments" runat="server" Text="0"></asp:Label></h3>

						<p>Total Documents</p>
					</div>
					<div class="icon">
						<i class="ion ion-bag"></i>
					</div>
					<a href="#" class="small-box-footer"></a>
				</div>
			</div>
			<!-- ./col -->
			<div class="col-lg-3 col-6">
				<!-- small box -->
				<div class="small-box bg-success">
					<div class="inner">
						<h3>
							<asp:Label ID="lblInHandDocuments" runat="server" Text="0"></asp:Label></h3>

						<p>In Hand Documents</p>
					</div>
					<div class="icon">
						<i class="ion ion-stats-bars"></i>
					</div>
					<a href="#" class="small-box-footer"></a>
				</div>
			</div>
			<!-- ./col -->
			<div class="col-lg-3 col-6">
				<!-- small box -->
				<div class="small-box bg-warning">
					<div class="inner">
						<h3>
							<asp:Label ID="lblOnTimeRate" runat="server" Text="0%"></asp:Label></h3>

						<p>On Time Rate</p>
					</div>
					<div class="icon">
						<i class="ion ion-person-add"></i>
					</div>
					<a href="#" class="small-box-footer"></a>
				</div>
			</div>
			<!-- ./col -->
			<div class="col-lg-3 col-6">
				<!-- small box -->
				<div class="small-box bg-danger">
					<div class="inner">
						<h3>
							<asp:Label ID="lblDelayedRate" runat="server" Text="0%"></asp:Label></h3>
						<p>Delayed Rate</p>
					</div>
					<div class="icon">
						<i class="ion ion-pie-graph"></i>
					</div>
					<a href="#" class="small-box-footer"></a>
				</div>
			</div>
			<!-- ./col -->
		</div>

		<!-- Data Table -->
		<h3 class="section-title">Document Flow Analysis Results</h3>
		<div class="card">
			<div class="table-responsive card-responsive">
				<asp:GridView ID="gvDocumentFlow" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False"
					EmptyDataText="No data available" OnRowDataBound="gvDocumentFlow_RowDataBound">
					<Columns>
						<asp:BoundField DataField="FlowName" HeaderText="Flow Name" HeaderStyle-CssClass="gridview-header" />
						<asp:BoundField DataField="CategoryName" HeaderText="Category" HeaderStyle-CssClass="gridview-header" />
						<asp:TemplateField HeaderText="Total Inhand" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:HyperLink
									ID="hlTotalInHand"
									runat="server"
									Text='<%# Eval("TotalInHand") %>'
									NavigateUrl='<%# "WorkFlow.aspx?FlowId=" + Eval("FlowId") %>'
									Target="_blank" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="InHandOnTime" HeaderText="In Hand(On Time)" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="InHandOnTimePercent" HeaderText="%" DataFormatString="{0:F2}%" HeaderStyle-CssClass="gridview-header" ItemStyle-CssClass="percent-column" />
						<asp:BoundField DataField="InHandDelayed" HeaderText="In Hand(Delayed)" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="InHandDelayedPercent" HeaderText="%" DataFormatString="{0:F2}%" HeaderStyle-CssClass="gridview-header" ItemStyle-CssClass="percent-column" />
						<asp:BoundField DataField="TotalCompleted" HeaderText="Total Completed" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="CompletedOnTime" HeaderText="Completed On Time" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="CompletedOnTimePercent" HeaderText="%" DataFormatString="{0:F2}%" HeaderStyle-CssClass="gridview-header" ItemStyle-CssClass="percent-column" />
						<asp:BoundField DataField="CompletedDelayed" HeaderText="Completed Delayed" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="CompletedDelayedPercent" HeaderText="%" DataFormatString="{0:F2}%" HeaderStyle-CssClass="gridview-header" ItemStyle-CssClass="percent-column" />
						<asp:BoundField DataField="OverallOnTimePercent" HeaderText="Overall On Time %" DataFormatString="{0:F2}%" HeaderStyle-CssClass="gridview-header" ItemStyle-CssClass="percent-column" />
						<asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="gridview-header" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
			</div>

		</div>

		<!-- Charts Section -->
		<h3 class="section-title">Visualization</h3>
		<div class="row">
			<div class="col-lg-6 mb-3">
				<div class="card">
					<div class="card-header">On-Time vs Delayed Comparison</div>
					<div class="card-body">
						<div class="chart-container">
							<canvas id="performanceChart" width="400" height="250"></canvas>
						</div>
					</div>
				</div>
			</div>
			<div class="col-lg-6 mb-3">
				<div class="card">
					<div class="card-header">Document Distribution by Flow</div>
					<div class="card-body">
						<div class="chart-container">
							<canvas id="flowDistributionChart"></canvas>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

	<!-- Hidden field to store chart data -->
	<asp:HiddenField ID="hfChartData" runat="server" />

	<!-- Script references -->
	<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
	<script type="text/javascript">
		function initCharts() {
			// Get chart data from hidden field
			var chartData = document.getElementById('<%= hfChartData.ClientID %>').value;
			if (chartData) {
				var data = JSON.parse(chartData);

				// Performance Chart
				var perfCtx = document.getElementById('performanceChart').getContext('2d');
				var performanceChart = new Chart(perfCtx, {
					type: 'bar',
					data: {
						labels: data.flowLabels,
						datasets: [
							{
								label: 'On Time %',
								data: data.onTimeData,
								backgroundColor: '#27ae60'
							},
							{
								label: 'Delayed %',
								data: data.delayedData,
								backgroundColor: '#e74c3c'
							}
						]
					},
					options: {
						responsive: true,
						maintainAspectRatio: false,
						scales: {
							y: {
								beginAtZero: true,
								max: 100,
								title: {
									display: true,
									text: 'Percentage'
								}
							}
						}
					}
				});

				// Flow Distribution Chart
				var distCtx = document.getElementById('flowDistributionChart').getContext('2d');
				var distributionChart = new Chart(distCtx, {
					type: 'pie',
					data: {
						labels: data.flowLabels,
						datasets: [{
							data: data.distributionData,
							backgroundColor: [
								'#3498db',
								'#2ecc71',
								'#e74c3c',
								'#f39c12',
								'#9b59b6',
								'#1abc9c',
								'#34495e',
								'#d35400',
								'#7f8c8d',
								'#16a085'
							]
						}]
					},
					options: {
						responsive: true,
						maintainAspectRatio: false,
						plugins: {
							legend: {
								position: 'right'
							}
						}
					}
				});
			}
		}

		// Initialize charts when page loads
		document.addEventListener('DOMContentLoaded', initCharts);

		// Reinitialize charts after AJAX postbacks
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(function () {
			initCharts();
		});
		//function openWorkflowDetails(flowId) {
		//	var url = 'WorkFlow.aspx?FlowId=' + flowId;
		//	window.open(url, '_blank', 'width=1200,height=700,scrollbars=yes,resizable=yes');
		//	return false;
		//}
	</script>
</asp:Content>
