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



namespace FakirDMS.UI
{
    public partial class AllDocumentReport : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        String _validationMessage = String.Empty;
        DataManager _dataManager = new DataManager();

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

                BindGridViewWorkflowList("UI");
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            BindGridViewWorkflowList("XLS");

            string ExportFile = "BillStatusReport_"+System.DateTime.Now.ToString("dd/MM/yyyy").Replace("/","_")+".xls";

            StringWriter sw = new StringWriter();

            string str = "<table cellspacing=@0@ rules=@all@ border=@1@ style=@border-collapse:collapse;@>".Replace("@", "\"");
            sw.Write(str);

            sw.Write("<tr><td colspan=16 style=@font-weight: bold; align-content:center; font-size: 50px;@>".Replace("@", "\""));

            sw.Write("Bill Status Report</td></tr></table>");

            // REport Date

            str = "<table cellspacing=@0@ rules=@all@ border=@1@ style=@border-collapse:collapse;@>".Replace("@", "\"");
            sw.Write(str);

            sw.Write("<tr><td colspan=16 style=@font-weight: bold; align-content:center; font-size: 25px;@>".Replace("@", "\""));

            string ReportDate = "From Date: " + txtFromDate.Text + " To Date: " + txtToDate.Text;
            
            sw.Write(ReportDate);

            sw.Write("</td></tr></table>");

            int i= gvWorkflowDocuments.Rows.Count;
			DataTable allData = Session["GridViewData"] as DataTable;
			//sw.Write(FillList.ExportToExcel(gvWorkflowDocuments, allData));
			using (XLWorkbook workbook = new XLWorkbook())
			{
				var worksheet = workbook.Worksheets.Add("Data");

				// Add DataTable data to worksheet
				worksheet.Cell(1, 1).InsertTable(allData);

				// Save the Excel file to MemoryStream
				using (MemoryStream memoryStream = new MemoryStream())
				{
					workbook.SaveAs(memoryStream);
					byte[] bytes = memoryStream.ToArray();

					// Send the Excel file to the client
					Response.Clear();
					Response.Buffer = true;
					Response.Charset = "";
					Response.ContentType = "application/vnd.ms-excel";
					Response.AddHeader("content-disposition","attachment;filename=" + ExportFile);
					Response.BinaryWrite(bytes);
					Response.Flush();
					Response.End();
				}
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