using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using DocumentFormat.OpenXml.Office2010.Word;
using System.Collections.Generic;

namespace FakirDMS.UI
{
    public partial class BillClosing : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        String _errorMessage = String.Empty;
        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {
			searchPanel.Visible = false;
			divSubmitted.Visible = false;
			#region Check User Login Status
			if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) || _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/.aspx", false));
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
				Session["SelectedRows"] = new List<int>();
				txtTracking.Visible = false;
				//searchPanel.Visible = false;
				LoadDropDownListCompany();
                LoadDropDownListCategory();
                LoadDropDownListTeamMember();
                BingGridViewDocumentList();
                //BingGridViewDocumentList_Submitted();
				LoadDropDownListRevertTo();
				BindDropDownListRoom();
			}
        }

		#region Page Load Related Event
		protected void gvCbSelect_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cbSelect = (CheckBox)sender;
			GridViewRow row = (GridViewRow)cbSelect.NamingContainer;
			HiddenField hfDocumentId = (HiddenField)row.FindControl("gvHfDocumentId");

			if (hfDocumentId != null)
			{
				int documentId = Convert.ToInt32(hfDocumentId.Value);

				// Retrieve or initialize the session list
				List<int> selectedRows = Session["SelectedRows"] as List<int> ?? new List<int>();

				if (cbSelect.Checked)
				{
					// Add to session if not already present
					if (!selectedRows.Contains(documentId))
					{
						selectedRows.Add(documentId);
					}
				}
				else
				{
					// Remove from session if present
					selectedRows.Remove(documentId);
				}

				// Save updated list back to session
				Session["SelectedRows"] = selectedRows;
			}
		}

		private void RestoreSelectedRows()
		{
			// Restore selections from session
			if (Session["SelectedRows"] != null)
			{
				var selectedRows = (List<int>)Session["SelectedRows"];
				foreach (GridViewRow row in gvDocuments.Rows)
				{
					HiddenField hfDocumentId = (HiddenField)row.FindControl("gvHfDocumentId");
					CheckBox cbSelect = (CheckBox)row.FindControl("gvCbSelect");

					if (hfDocumentId != null && cbSelect != null && selectedRows.Contains(Convert.ToInt32(hfDocumentId.Value)))
					{
						cbSelect.Checked = true;
					}
				}
			}
		}
		private void StoreSelectedRows()
		{
			if (Session["SelectedRows"] == null)
			{
				Session["SelectedRows"] = new List<int>();
			}

			// Retrieve the session object as a list
			List<int> selectedRows = (List<int>)Session["SelectedRows"];

			foreach (GridViewRow row in gvDocuments.Rows)
			{
				HiddenField hfDocumentId = (HiddenField)row.FindControl("gvHfDocumentId");
				CheckBox cbSelect = (CheckBox)row.FindControl("gvCbSelect");

				if (hfDocumentId != null && cbSelect != null)
				{
					int documentId = Convert.ToInt32(hfDocumentId.Value);
					if (cbSelect.Checked && !selectedRows.Contains(documentId))
					{
						selectedRows.Add(documentId);
					}
					else if (!cbSelect.Checked && selectedRows.Contains(documentId))
					{
						selectedRows.Remove(documentId);
					}
				}
			}

			Session["SelectedRows"] = selectedRows;
		}
		protected void LoadDropDownListRevertTo()
		{
			try
			{
				StringBuilder sQuery = new StringBuilder();
				sQuery.Append("SELECT FlowID AS ValueField,FlowName AS DisplayField FROM dbo.Sys_Flowpath    ");
				sQuery.Append("WHERE CategoryID=2 AND SerialNo < (   ");
				sQuery.Append("SELECT MAX(F.SerialNo) FROM dbo.DocumentInfo D    ");
				sQuery.Append("INNER JOIN dbo.Sys_Flowpath F ON D.CategoryID=F.CategoryID AND D.FlowId=F.FlowID)");
				sQuery.Append(" ORDER BY SerialNo DESC   ");

				_dataManager = new DataManager();
				DataTable dtRoleList = _dataManager.GetDataTable(sQuery.ToString());
				FillList.PopulateDropDownList(dtRoleList, ddlRevertTo, "Select Role to Revert");
			}
			catch
			{

			}
		}
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

		#region DropDownList Selected Index Changed
		protected void box_ddlRackName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				DataTable _dtRoom = PopulateLists.GetRackWiseShelf(box_ddlRackName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlShelfName, "Select Shelf");

				_dtRoom = PopulateLists.GetShelfWiseBox(box_ddlShelfName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlBoxfName, "Select Box");
			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}
		}

		protected void box_ddlRoomName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				DataTable _dtRoom = PopulateLists.GetRoomWiseRack(box_ddlRoomName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlRackName, "Select Rack");
				_dtRoom = PopulateLists.GetRackWiseShelf(box_ddlRackName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlShelfName, "Select Shelf");
				_dtRoom = PopulateLists.GetShelfWiseBox(box_ddlShelfName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlBoxfName, "Select Box");

			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}
		}

		protected void box_ddlShelfName_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				DataTable _dtRoom = PopulateLists.GetShelfWiseBox(box_ddlShelfName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlBoxfName, "Select Box");
			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}
		}

		#endregion
		protected void LoadDropDownListTeamMember()
        {
        }
		protected void BingGridViewDocumentList_Submitted()
		{
			try
			{
				_dataManager = new DataManager();
				SqlParameter[] parameters = new SqlParameter[6]
				{
					_dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
					_dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
					_dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
					_dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearchBy.Text),
					_dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
					_dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "WORKFLOW")
				};

				DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_ACCOUNTS_SUBMITTED", parameters);

				lblSubmittedDocCount.Text = dtDocuments.Rows.Count.ToString();
				FillList.PopulateGridView(dtDocuments, gvDocumentsSubmitted);
				if (gvDocumentsSubmitted.Rows.Count > 10)
				{
					gridContainer_submit.Style["height"] = "400px"; // Set a fixed height
					gridContainer_submit.Style["overflow-y"] = "auto"; // Enable vertical scrolling
				}
				else
				{
					gridContainer_submit.Style.Remove("height"); // Remove fixed height
					gridContainer_submit.Style.Remove("overflow-y"); // Remove scrolling
				}

			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}
		}
		protected void txtTracking_TextChanged(object sender, EventArgs e)
		
        {
			// Get the search term from the TextBox
			string searchTerm = txtTracking.Text.Trim();

			// Retrieve the data from ViewState
			DataTable GridViewData = ViewState["GridViewData"] as DataTable;

			if (GridViewData != null)
			{
				// Filter data
				DataView dv = GridViewData.DefaultView;
				if (!string.IsNullOrEmpty(searchTerm))
				{
					dv.RowFilter = $"BillRefNo LIKE '%{searchTerm}%'";
				}
				else
				{
					dv.RowFilter = string.Empty; // Show all data if no filter
				}

				// Bind the filtered data to GridView
				gvDocuments.DataSource = dv;
				gvDocuments.DataBind();
			}
		}
		protected void BingGridViewDocumentList()
        {
			//Session["SelectedRows"] = new List<int>();
			try
            {
				_dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[6]
                {
                    _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                    _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                    _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
                    _dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearchBy.Text),
                    _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "WORKFLOW")
                };

                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_ACCOUNTS_SUBMITTED", parameters);

                lblWorkflowCount.Text = dtDocuments.Rows.Count.ToString();
                FillList.PopulateGridView(dtDocuments, gvDocuments);
				ViewState["GridViewData"] = dtDocuments;
				if (gvDocuments.Rows.Count > 20)
				{
					gridContainer.Style["height"] = "400px"; // Set a fixed height
					gridContainer.Style["overflow-y"] = "auto"; // Enable vertical scrolling
				}
				else
				{
					gridContainer.Style.Remove("height"); // Remove fixed height
					gridContainer.Style.Remove("overflow-y"); // Remove scrolling
				}
				//RestoreSelectedRows();
			}
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
		protected void BindDropDownListRoom()
		{
			try
			{
				DataTable _dtRoom = PopulateLists.GetRooms();
				FillList.PopulateDropDownList(_dtRoom, box_ddlRoomName, "Select Room");

				_dtRoom = PopulateLists.GetRackWiseShelf(box_ddlRackName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlRackName, "Select Rack");

				_dtRoom = PopulateLists.GetRackWiseShelf(box_ddlRackName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlShelfName, "Select Shelf");

				_dtRoom = PopulateLists.GetShelfWiseBox(box_ddlShelfName.SelectedValue);
				FillList.PopulateDropDownList(_dtRoom, box_ddlBoxfName, "Select Box");
			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}
		}
		#endregion

		#region DropDownList Checked Event
		protected void ddlTeamMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            // For TeamMember Info 

            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT UserId,UserName,DG.LookupText AS DesignationName,D.LookupText AS DepartmentName,C.LookupText AS CompanyName FROM Sys_Users U   ");
                sQuery.Append("LEFT JOIN dbo.Sys_Lookup DG ON DG.LookupTypeId=3 AND U.DesignationId=DG.LookupValue  ");
                sQuery.Append("LEFT JOIN dbo.Sys_Lookup D ON D.LookupTypeId=2 AND U.DepartmentId=D.LookupValue  ");
                sQuery.Append("LEFT JOIN dbo.Sys_Lookup C ON C.LookupTypeId=1 AND U.CompanyId=C.LookupValue  ");

                _dataManager = new DataManager();
                DataTable _dtUser = _dataManager.GetDataTable(sQuery.ToString());

                if (_dtUser.Rows.Count > 0)
                {
                    //txtDesignation.Text = _dtUser.Rows[0]["DesignationName"].ToString();
                    //txtDepartment.Text = _dtUser.Rows[0]["DepartmentName"].ToString();
                    //txtCompany.Text = _dtUser.Rows[0]["CompanyName"].ToString();
                }
                else
                {
                    //txtDesignation.Text = String.Empty;
                    //txtDepartment.Text = String.Empty;
                    //txtCompany.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

            // For TeamMember TNA

            try
            {


                foreach (GridViewRow gvRow in gvDocuments.Rows)
                {
                    
                    CheckBox cbIsSelect = (CheckBox)gvRow.FindControl("gvCbSelect");

                    if (cbIsSelect.Checked == true)
                    {

                        String sDocId = ((HiddenField)gvRow.FindControl("gvHfDocumentId")).Value;

                        StringBuilder sQuery1 = new StringBuilder();
                        sQuery1.Append("Select isnull(f.TnaDays,0) as TnaDays  from Sys_UserCompany uc");
                        sQuery1.Append(" inner join Sys_Flowpath f on uc.FlowId=f.FlowId");
                        sQuery1.Append(" inner join DocumentInfo AS d on uc.CategoryId=d.CategoryId");
                        sQuery1.Append(" WHERE  d.DocumentId='"+ sDocId.Trim() + "'");

                        _dataManager = new DataManager();
                        DataTable _dtTna = _dataManager.GetDataTable(sQuery1.ToString());

                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

        }



           
        #endregion

        #region Button Click Event
        protected void btnSearch_Click(object sender, EventArgs e)
        {
			btnSearch.Enabled = false;
            BingGridViewDocumentList();
			RestoreSelectedRows();
			btnSearch.Enabled = true;
		}

        protected void btnReload_Click(object sender, EventArgs e)
        {
            ddlCompany.SelectedIndex = -1;
            ddlCategory.SelectedIndex = -1;
            txtRefNo.Text = String.Empty;
            txtSearchBy.Text = String.Empty;
			Session["SelectedRows"] = new List<int>();
			BingGridViewDocumentList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckSaveValidation())
                {
                    DisplayMessage(_errorMessage);
                    return;
                }

                foreach (GridViewRow gvRow in gvDocuments.Rows)
                {
                    String sDocId = ((HiddenField)gvRow.FindControl("gvHfDocumentId")).Value;
                    CheckBox cbIsSelect = (CheckBox)gvRow.FindControl("gvCbSelect");

                    if (cbIsSelect.Checked == true)
                    {
						_dataManager = new DataManager();
						SqlParameter[] parameters = new SqlParameter[7]
						{
							_dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, sDocId),
							_dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, ""),
							_dataManager.MakeInParam("@roomId", SqlDbType.NVarChar, 500, box_ddlRoomName.SelectedValue),
							_dataManager.MakeInParam("@rackId", SqlDbType.NVarChar, 500, box_ddlRackName.SelectedValue),
							_dataManager.MakeInParam("@shelfId", SqlDbType.NVarChar, 500, box_ddlShelfName.SelectedValue),
							_dataManager.MakeInParam("@boxId", SqlDbType.NVarChar, 500, box_ddlBoxfName.SelectedValue),
							_dataManager.MakeInParam("@ClosedBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString()))
						};

						DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_DOCUMENT_CLOSE_ACCOUNTS", parameters);
					}
					//Response.Redirect(Request.Path);
				}

                BingGridViewDocumentList();
				//BingGridViewDocumentList_Submitted();
				DisplayMessage("Submitted Successfully!");
			}
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion

        #region Validation Check Related Methods

        protected Boolean CheckSaveValidation()
        {
            Boolean _result = true;

			return _result;
        }
        #endregion
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }

		protected void btnReject_Click(object sender, EventArgs e)
		{
            if (CheckValidationForRevert())
            {
                foreach (GridViewRow gvRow in gvDocuments.Rows)
                {
                    String sDocId = ((HiddenField)gvRow.FindControl("gvHfDocumentId")).Value;
                    CheckBox cbIsSelect = (CheckBox)gvRow.FindControl("gvCbSelect");

                    if (cbIsSelect.Checked == true)
                    {
                        _dataManager = new DataManager();
                        SqlParameter[] parameters = new SqlParameter[5]
                        {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, sDocId),
                        _dataManager.MakeInParam("@RoleID", SqlDbType.NVarChar, 500, ddlRevertTo.SelectedValue),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text.ToString()),
                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "R")
                        };
                        DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_WORKFLOW_ACTION_SCM", parameters);
                    }
                }
				DisplayMessage("Rejected Successfully!");
				Response.Redirect(Request.RawUrl);
			}
            else
            {

            }
			BingGridViewDocumentList();
		}
		protected Boolean CheckValidationForRevert()
		{
			Boolean _result = true;
			if (ddlRevertTo.SelectedValue == "0")
			{
				_errorMessage = "Please select any role to reject";
				ddlCompany.Focus();
				return false;
			}
			else if (String.IsNullOrEmpty(txtRemarksBoss.Text))
			{
				_errorMessage = "Please write remarks and try again";
				txtRemarksBoss.Focus();
				_result = false;
			}

			return _result;
		}
	}
}