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

namespace FakirDMS.UI
{
    public partial class PaymentConfirm : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        String _errorMessage = String.Empty;
        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {
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
				LoadDropDownListCompany();
                LoadDropDownListCategory();
                LoadDropDownListTeamMember();
                BingGridViewDocumentList();
                BingGridViewDocumentList_Submitted();
				LoadDropDownListRevertTo();

			}
        }

		#region Page Load Related Event
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
			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
			}
		}
		protected void BingGridViewDocumentList()
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

                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_TASK_ASSIGN_ACCOUNTS", parameters);

                lblWorkflowCount.Text = dtDocuments.Rows.Count.ToString();
                FillList.PopulateGridView(dtDocuments, gvDocuments);
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
            BingGridViewDocumentList();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            ddlCompany.SelectedIndex = -1;
            ddlCategory.SelectedIndex = -1;
            txtRefNo.Text = String.Empty;
            txtSearchBy.Text = String.Empty;
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
                    string voucher_no = txtpaymentVoucherNo.Text;
                    string voucher_date = txtFromDate.Text;

                    if (cbIsSelect.Checked == true)
                    {
                        _dataManager = new DataManager();
                        SqlParameter[] parameters = new SqlParameter[5]
                        {
                            _dataManager.MakeInParam("@DocumentId", SqlDbType.NVarChar, 500, sDocId),
                            _dataManager.MakeInParam("@VoucherNo", SqlDbType.NVarChar, 500, voucher_no),
                            _dataManager.MakeInParam("@VoucherDate", SqlDbType.NVarChar, 500, voucher_date),
							_dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
							_dataManager.MakeInParam("@FlowId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.RoleId.ToString()))
						};
                        DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_INITIAL_VOUCHER_NO", parameters);
                    }
					Response.Redirect(Request.Path);
				}

                BingGridViewDocumentList();
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

            if (String.IsNullOrEmpty(txtpaymentVoucherNo.Text))
            {
                _errorMessage = "Please Enter Voucher No";
				txtpaymentVoucherNo.Focus();
                return false;
            }

			if (String.IsNullOrEmpty(txtFromDate.Text))
			{
				_errorMessage = "Please Enter Date";
				txtFromDate.Focus();
				return false;
			}

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
                    string voucher_no = txtpaymentVoucherNo.Text;
                    string voucher_date = txtFromDate.Text;

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

				Response.Redirect(Request.Path);
			}
            else
            {

            }
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