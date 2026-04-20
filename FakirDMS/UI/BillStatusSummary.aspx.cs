using FokirDMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FakirDMS.UI
{
	public partial class BillStatusSummary : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				lblLastUpdated.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
				pnlFilters.Visible = true;
				BindFlowDropdown();
				BindCategoryDropdown();
				//BindMultiCategoryList();
				BindDocumentFlowData();
			}
		}

		private void BindFlowDropdown()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
			string query = "SELECT FlowId, FlowName FROM Sys_Flowpath WHERE FlowId NOT IN (100, 0, 1, 2, 9, 21, 28) ORDER BY FlowName";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					con.Open();
					ddlFlow.DataSource = cmd.ExecuteReader();
					ddlFlow.DataTextField = "FlowName";
					ddlFlow.DataValueField = "FlowId";
					ddlFlow.DataBind();
				}
			}
		}

		private void BindCategoryDropdown()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
			string query = "SELECT CategoryId, CategoryName, * FROM Sys_Category WHERE IsActive = 1 and CategoryId in (2,5)";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					con.Open();
					ddlCategory.DataSource = cmd.ExecuteReader();
					ddlCategory.DataTextField = "CategoryName";
					ddlCategory.DataValueField = "CategoryId";
					ddlCategory.DataBind();
				}
			}
		}

		//private void BindMultiCategoryList()
		//{
		//	string connectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
		//	string query = "SELECT CategoryId, CategoryName FROM Sys_Category ORDER BY CategoryName";

		//	using (SqlConnection con = new SqlConnection(connectionString))
		//	{
		//		using (SqlCommand cmd = new SqlCommand(query, con))
		//		{
		//			con.Open();
		//			lstCategories.DataSource = cmd.ExecuteReader();
		//			lstCategories.DataTextField = "CategoryName";
		//			lstCategories.DataValueField = "CategoryId";
		//			lstCategories.DataBind();
		//		}
		//	}
		//}

		private void BindDocumentFlowData()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
			StringBuilder chartData = new StringBuilder();

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("rpt_GetDocumentFlowAnalysis", con))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					// Add parameters if they have values
					if (!string.IsNullOrEmpty(ddlFlow.SelectedValue))
						cmd.Parameters.AddWithValue("@FlowId", ddlFlow.SelectedValue);

					if (!string.IsNullOrEmpty(ddlCategory.SelectedValue))
						cmd.Parameters.AddWithValue("@CategoryId", ddlCategory.SelectedValue);

					// Handle multiple category selection
					//if (lstCategories.GetSelectedIndices().Length > 0)
					//{
					//	string selectedCategories = string.Join(",", lstCategories.GetSelectedIndices()
					//		.Select(i => lstCategories.Items[i].Value));
					//	cmd.Parameters.AddWithValue("@CategoryList", selectedCategories);
					//}

					con.Open();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					//DataTable dt = new DataTable();
					DataSet ds = new DataSet();

					// 2. Fill the DataSet. This will create "Table", "Table1", etc. automatically
					da.Fill(ds);

					DataTable dt = ds.Tables[0];
					DataTable dt1 = ds.Tables[1];

					gvDocumentFlow.DataSource = dt;
					gvDocumentFlow.DataBind();

					// Calculate summary values
					int totalInHand = dt.AsEnumerable().Sum(row => row.Field<int>("TotalInHand"));
					int totalCompleted = dt.AsEnumerable().Sum(row => row.Field<int>("TotalCompleted"));
					int totalDocuments = dt1.Rows[0]["TotalDocument"].ToString().ToInt(); //totalInHand + totalCompleted;

					int inHandOnTime = dt.AsEnumerable().Sum(row => row.Field<int>("InHandOnTime"));
					int completedOnTime = dt.AsEnumerable().Sum(row => row.Field<int>("CompletedOnTime"));
					int totalOnTime = inHandOnTime + completedOnTime;

					double onTimeRate = totalInHand > 0 ? Math.Round((inHandOnTime * 100.0) / totalInHand, 2) : 0;
					double delayedRate = 100 - onTimeRate;

					// Update summary controls
					lblTotalDocuments.Text = dt1.Rows[0]["TotalDocument"].ToString();
					lblOnTimeRate.Text = onTimeRate.ToString("0.00") + "%";
					lblDelayedRate.Text = delayedRate.ToString("0.00") + "%";
					lblInHandDocuments.Text = totalInHand.ToString();

					//progressOnTime.Style["width"] = onTimeRate.ToString("0.0") + "%";
					//progressDelayed.Style["width"] = delayedRate.ToString("0.0") + "%";

					// Prepare chart data
					var flowLabels = new List<string>();
					var onTimeData = new List<double>();
					var delayedData = new List<double>();
					var distributionData = new List<int>();

					foreach (DataRow row in dt.Rows)
					{
						flowLabels.Add(row["FlowName"].ToString());
						onTimeData.Add(Convert.ToDouble(row["OverallOnTimePercent"]));
						delayedData.Add(Convert.ToDouble(row["OverallDelayedPercent"]));
						distributionData.Add(Convert.ToInt32(row["TotalInHand"]) + Convert.ToInt32(row["TotalCompleted"]));
					}

					// Store chart data in hidden field
					chartData.Append("{");
					chartData.Append("\"flowLabels\": [" + string.Join(",", flowLabels.Select(l => "\"" + l + "\"")) + "],");
					chartData.Append("\"onTimeData\": [" + string.Join(",", onTimeData) + "],");
					chartData.Append("\"delayedData\": [" + string.Join(",", delayedData) + "],");
					chartData.Append("\"distributionData\": [" + string.Join(",", distributionData) + "]");
					chartData.Append("}");

					hfChartData.Value = chartData.ToString();
				}
			}
		}

		protected void btnApplyFilters_Click(object sender, EventArgs e)
		{
			BindDocumentFlowData();
		}

		protected void btnResetFilters_Click(object sender, EventArgs e)
		{
			ddlFlow.SelectedValue = "";
			ddlCategory.SelectedValue = "";
			//lstCategories.ClearSelection();
			BindDocumentFlowData();
		}

		protected void btnToggleFilters_Click(object sender, EventArgs e)
		{
			pnlFilters.Visible = !pnlFilters.Visible;
			btnToggleFilters.Text = pnlFilters.Visible ? "<i class='fas fa-chevron-up'></i>" : "<i class='fas fa-chevron-down'></i>";
		}

		protected void gvDocumentFlow_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				// Ensure the hyperlink works for Total Inhand column
				HyperLink hlTotalInHand = (HyperLink)e.Row.FindControl("hlTotalInHand");
				// Get the data
				DataRowView rowView = (DataRowView)e.Row.DataItem;

				if (hlTotalInHand != null)
				{
					string flowId = rowView["FlowId"].ToString();

					// Make sure the JavaScript function call is properly formatted
					//hlTotalInHand.NavigateUrl = $"javascript:return openWorkflowDetails('{flowId}');";
				}

				Label lblStatus = (Label)e.Row.FindControl("lblStatus");

				double onTimePercent = Convert.ToDouble(rowView["OverallOnTimePercent"]);

				if (lblStatus != null)
				{
					// Set status based on percentage
					if (onTimePercent >= 80)
					{
						lblStatus.Text = "Excellent";
						lblStatus.CssClass = "badge badge-success";
					}
					else if (onTimePercent >= 60)
					{
						lblStatus.Text = "Good";
						lblStatus.CssClass = "badge badge-primary";
					}
					else
					{
						lblStatus.Text = "Needs Attention";
						lblStatus.CssClass = "badge badge-warning";
					}
				}
			}
		}

		protected void gvDocumentFlow_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvDocumentFlow.PageIndex = e.NewPageIndex;
			BindDocumentFlowData();
		}

		protected void btnExport_Click(object sender, EventArgs e)
		{
			// Export to Excel functionality
			Response.Clear();
			Response.Buffer = true;
			Response.ContentType = "application/vnd.ms-excel";
			Response.AddHeader("content-disposition", "attachment;filename=DocumentFlowAnalysis.xls");
			Response.Charset = "";
			this.EnableViewState = false;

			System.IO.StringWriter sw = new System.IO.StringWriter();
			System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

			// Render grid view to HTML
			gvDocumentFlow.RenderControl(htw);

			// Write the HTML to response
			Response.Write(sw.ToString());
			Response.End();
		}

		public override void VerifyRenderingInServerForm(Control control)
		{
			// Required for export functionality
		}
	}
}