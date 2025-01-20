using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;



namespace FakirDMS.UI
{
    public partial class AllDocumentReport : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        String _validationMessage = String.Empty;
        DataManager _dataManager = new DataManager();
        DataTable xl_data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Check User Login Status
            if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) || _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }

            try
            {
                DataTable dtMenu = (DataTable)Session["MenuList"];
                String sPageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                bool isPermitted = dtMenu.AsEnumerable().Any(row => row["menu_url"].ToString().Contains(sPageName));
                if (!isPermitted)
                {
                    Response.Redirect(String.Format("~/UI/UnauthorizedPage.aspx"), false);
                    return;
                }
            }
            catch
            {
                Response.Redirect(String.Format("~/Default.aspx"), false);
            }
            #endregion

            if (!IsPostBack)
            {
                LoadDropDownListCompany();
                LoadDropDownListExpense();
                LoadDropDownListCategory();
                LoadDropDownListStatus();

                //BindGridViewWorkflowList("UI");
            }
        }

        #region DropDownList Related

        protected void LoadDropDownListCompany()
        {
            DataTable dtComapy = PopulateLists.GetCompanies();
            FillList.PopulateDropDownList(dtComapy, ddlCompany, "Select Company");
        }

        protected void LoadDropDownListCategory()
        {
            DataTable dtCategorys = PopulateLists.GetCategorys();
            FillList.PopulateDropDownList(dtCategorys, ddlCategory, "Select Category");
        }

        protected void LoadDropDownListExpense()
        {
            DataTable dtExpense = PopulateLists.GetExpenseTypes();
            FillList.PopulateDropDownList(dtExpense, ddlExpense, "Select Expense Type");
        }

        protected void LoadDropDownListStatus()
        {
            DataTable dtExpense = PopulateLists.GetStatuses();
            FillList.PopulateDropDownList(dtExpense, ddlStatus, "DisplayField", "ValueField", true, "Select Status", "99");
        }

        #endregion

        #region Page Load Related Event

        protected void BindGridViewWorkflowList(string DataModel)
        {
            try
            {
                String sActionType = "ALL_DOCUMENT";

                if(cbIsForwarded.Checked== true)
                {
                    sActionType = "FORWARD";
                }

                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[11]
                {
                    _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                    _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                    _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpense.SelectedValue),
                    _dataManager.MakeInParam("@Supplier", SqlDbType.NVarChar, 500, txtSupplier.Text),
                    _dataManager.MakeInParam("@StatusId", SqlDbType.NVarChar, 500, ddlStatus.SelectedValue),
                    _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
                    _dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearchWith.Text),
                    _dataManager.MakeInParam("@FromDate", SqlDbType.NVarChar, 500, txtFromDate.Text),
                    _dataManager.MakeInParam("@ToDate", SqlDbType.NVarChar, 500, txtToDate.Text),
                    _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                   // _dataManager.MakeInParam("@DataModel", SqlDbType.NVarChar, 500, DataModel),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sActionType)
                };
                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_REPORT", parameters);
                xl_data = dtDocuments;
				Session["GridViewData"] = dtDocuments;
				lblWorkflowCount.Text = dtDocuments.Rows.Count.ToString();
                //if (DataModel == "UI")
                    FillList.PopulateGridView(dtDocuments, gvWorkflowDocuments);
				//else
				//{
				//    //string fileName = Server.MapPath("~/ExcelFiles/BillStatus.xls");






				//}

			}
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\n Error:" + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        protected void gvWorkflowDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWorkflowDocuments.PageIndex = e.NewPageIndex;
            BindGridViewWorkflowList("UI");
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewWorkflowList("UI");
        }
		DateTime fromDate;
		DataTable allRow = new DataTable();
		protected void btnExcel_Click(object sender, EventArgs e)
        {
			//BindGridViewWorkflowList("UI");
			String sActionType = "ALL_DOCUMENT";

			if (cbIsForwarded.Checked == true)
			{
				sActionType = "FORWARD";
			}

			try
			{
				_dataManager = new DataManager();
				SqlParameter[] parameters = new SqlParameter[11]
				{
					_dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
					_dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
					_dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpense.SelectedValue),
					_dataManager.MakeInParam("@Supplier", SqlDbType.NVarChar, 500, txtSupplier.Text),
					_dataManager.MakeInParam("@StatusId", SqlDbType.NVarChar, 500, ddlStatus.SelectedValue),
					_dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
					_dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearchWith.Text),
					_dataManager.MakeInParam("@FromDate", SqlDbType.NVarChar, 500, txtFromDate.Text),
					_dataManager.MakeInParam("@ToDate", SqlDbType.NVarChar, 500, txtToDate.Text),
					_dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                   // _dataManager.MakeInParam("@DataModel", SqlDbType.NVarChar, 500, DataModel),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sActionType)
				};
				DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_REPORT", parameters);
				allRow = dtDocuments;
			}

			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}

			// Assuming 'allRow' is your DataTable that contains all rows (from all pages)
			if (allRow != null && allRow.Rows.Count > 0)
			{
				DataTable customizedTable = CustomizeDataTable(allRow);
				// Create a temporary GridView to bind all rows
				GridView gvExport = new GridView();

				// Add a header row with the text "Bill Status Report"
				GridViewRow headerRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
				System.Web.UI.WebControls.TableCell headerCell = new System.Web.UI.WebControls.TableCell();
				headerCell.Text = "<strong>Bill Status Report</strong>"; // Add your custom text here
				headerCell.ColumnSpan = allRow.Columns.Count; // Span across all columns
				headerCell.HorizontalAlign = HorizontalAlign.Center;
				headerRow.Cells.Add(headerCell);

				// Add this row to the GridView
				gvExport.Controls.AddAt(0, headerRow); // Adding the header row before binding data

				// Bind the DataTable to the GridView
				gvExport.DataSource = customizedTable;
				gvExport.DataBind();
				string ExportFile = "BillStatus_" + System.DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "_") + ".xls";
				// Change the GridView column headers (if needed)
				CustomizeGridViewHeaders(gvExport);
				// Export the GridView to Excel
				gvExport.HeaderRow.Cells[0].Text = "Tracking No";
				gvExport.HeaderRow.Cells[2].Text = "Assignee";
				gvExport.HeaderRow.Cells[4].Text = "Expense";
				gvExport.HeaderRow.Cells[3].Text = "Category";
				gvExport.HeaderRow.Cells[6].Text = "PO";
				gvExport.HeaderRow.Cells[5].Text = "Req No";
				gvExport.HeaderRow.Cells[9].Text = "LC No";
				gvExport.HeaderRow.Cells[7].Text = "Supplier";
				gvExport.HeaderRow.Cells[8].Text = "Pi No";
				gvExport.HeaderRow.Cells[10].Text = "Mrr No";
				gvExport.HeaderRow.Cells[11].Text = "Bill Amount";
				gvExport.HeaderRow.Cells[12].Text = "Status";
				gvExport.HeaderRow.Cells[13].Text = "Entry Date";
				gvExport.HeaderRow.Cells[14].Text = "Last Modify";
				gvExport.HeaderRow.Cells[15].Text = "Waiting Time";

				ExportGridViewToExcel(gvExport, ExportFile, "Bill Status");
			}
			else
			{
				// Handle case where no data is available
				//lblResult.Text = "No data available to export!";
				DisplayMessage("No data available to export!");
			}
		}
		private void ExportGridViewToExcel(GridView gv, string fileName, string ReportName)
		{
			// Set response settings for Excel export

			Response.Clear();
			Response.Buffer = true;
			Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
			Response.Charset = "";
			Response.ContentType = "application/vnd.ms-excel";

			using (StringWriter sw = new StringWriter())
			{
				string str = "<table cellspacing=@0@ rules=@all@ border=@1@ style=@border-collapse:collapse;@>".Replace("@", "\"");
				sw.Write(str);

				sw.Write("<tr><td colspan=18 style=@font-weight: bold; align-content:center; font-size: 50px;@>".Replace("@", "\""));

				sw.Write(ReportName+"</td></tr></table>");

				// REport Date

				str = "<table cellspacing=@0@ rules=@all@ border=@1@ style=@border-collapse:collapse;@>".Replace("@", "\"");
				sw.Write(str);

				sw.Write("<tr><td colspan=18 style=@font-weight: bold; align-content:center; font-size: 25px;@>".Replace("@", "\""));

				string ReportDate = "Report Date: " + DateTime.Today.ToString("dd-MM-yyyy");//+ " To Date: " + txtToDate.Text;

				sw.Write(ReportDate);

				sw.Write("</td></tr></table>");

				HtmlTextWriter htw = new HtmlTextWriter(sw);

				// Render the GridView to the HtmlTextWriter
				gv.RenderControl(htw);

				// Output the rendered content to the Response
				Response.Output.Write(sw.ToString());
				Response.Flush();
				Response.End();
			}
		}
		private DataTable CustomizeDataTable(DataTable dt)
		{
			List<string> columnsToShow = new List<string>
			{
				"BillRefNo", "Company","Assignee","CategoryName", "ExpenseTypeName", "ReqNo",
				"PoNo", "SupplierName", "PayMode", "PiNo", "LcNo", "MrrNo", "Currency", "BillAmount",
				"Status", "EntryDate", "LastModified", "Waiting"
			};

			// Create a list of columns that need to be removed
			DataTable newTable = new DataTable();

			// Add columns to the new DataTable in the order specified by columnsToShow
			foreach (string columnName in columnsToShow)
			{
				if (dt.Columns.Contains(columnName))
				{
					// Add the column to the new DataTable
					newTable.Columns.Add(dt.Columns[columnName].ColumnName, dt.Columns[columnName].DataType);
				}
			}

			// Copy the data from the old DataTable to the new DataTable in the correct order
			foreach (DataRow row in dt.Rows)
			{
				DataRow newRow = newTable.NewRow();
				foreach (string columnName in columnsToShow)
				{
					if (dt.Columns.Contains(columnName))
					{
						// Copy the data from the original DataTable to the new one
						newRow[columnName] = row[columnName];
					}
				}
				newTable.Rows.Add(newRow);
			}

			return newTable;
		}
		private void CustomizeGridViewHeaders(GridView gv)
		{
			foreach (DataControlField column in gv.Columns)
			{
				// Check for the column header and replace as needed
				if (column.HeaderText == "BillRefNo")
				{
					column.HeaderText = "Tracking No"; // Change "TrackingNo" to "Tracking No"
				}
				if (column.HeaderText == "ExpenseTypeName")
				{
					column.HeaderText = "Expense"; // You can add more customizations here
				}
				if (column.HeaderText == "ReqNo")
				{
					column.HeaderText = "Req No"; // You can add more customizations here
				}
				if (column.HeaderText == "PoNo")
				{
					column.HeaderText = "Po No"; // You can add more customizations here
				}
				if (column.HeaderText == "PayMode")
				{
					column.HeaderText = "Pay Mode"; // You can add more customizations here
				}
				if (column.HeaderText == "PiNo")
				{
					column.HeaderText = "Pi No"; // You can add more customizations here
				}
				if (column.HeaderText == "LcNo")
				{
					column.HeaderText = "Lc No"; // You can add more customizations here
				}
				if (column.HeaderText == "MrrNo")
				{
					column.HeaderText = "Mrr No"; // You can add more customizations here
				}
				if (column.HeaderText == "BillAmount")
				{
					column.HeaderText = "Bill Amount"; // You can add more customizations here
				}
				if (column.HeaderText == "EntryDate")
				{
					column.HeaderText = "Entry Date"; // You can add more customizations here
				}
				if (column.HeaderText == "LastModified")
				{
					column.HeaderText = "Last Modified"; // You can add more customizations here
				}

				// Add other columns as needed for renaming
			}
		}
		protected void btnReload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/AllDocumentReport.aspx", false);
        }

        protected void gvWorkflowDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ViewHistory"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                String sDocId = ((Label)row.FindControl("lblTranID")).Text;

                try
                {
                    DataManager _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[1]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, sDocId)
                    };

                    DataTable dtHistory = _dataManager.GetDataTable("SP_DOCUMENT_MOVEMENT_HISTORY", parameters);
                    FillList.PopulateGridView(dtHistory, gvHistory);
                }
                catch
                {

                }

                modalExtenderHistory.Show();
            }
        }
    }
}