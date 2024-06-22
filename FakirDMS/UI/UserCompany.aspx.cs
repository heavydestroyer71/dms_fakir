using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Linq;


namespace FakirDMS.UI
{
    public partial class UserCompany : System.Web.UI.Page
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

            if (!Page.IsPostBack)
            {
                LoadCompanyCategoryInfo();
            }
        }

        #region Page Load Related
        protected void BindGridViewUserCompany()
        {
            try
            {
                StringBuilder sQuery =  new StringBuilder();
                sQuery.Append("SELECT UC.TranID,UC.CompanyID,C.LookupText AS CompanyName,isnull(S.CategoryID,7) as CategoryID, ISNULL(S.CategoryName,'') AS CategoryName,      ");
                sQuery.Append("ISNULL(R.FlowID,0) AS RoleID, ISNULL(R.FlowName,'') AS RoleName, CASE WHEN CanPrepare=1 THEN 'Yes' ELSE 'No' END AS CanPrepare     ");
                sQuery.Append("FROM Sys_UserCompany AS UC    ");
                sQuery.Append("LEFT JOIN Sys_Category AS S ON UC.CategoryId = S.CategoryId    ");
                sQuery.Append("LEFT JOIN Sys_Flowpath R ON UC.FlowId=R.FlowID     ");
                sQuery.Append("LEFT JOIN Sys_Lookup AS C ON C.LookupTypeId=1 AND UC.CompanyID = C.LookupValue      ");
                sQuery.Append("WHERE UserID='" + hfUserID.Value+ "'      ");
                sQuery.Append("ORDER BY UC.CompanyId, UC.CategoryID, R.SerialNo ASC");

                _dataManager = new DataManager();
                DataTable _dtUserCompany = _dataManager.GetDataTable(sQuery.ToString());
                if(_dtUserCompany.Rows.Count > 0)
                {
                    FillList.PopulateGridView(_dtUserCompany, gvCompanyWisePermission);
                }
                else
                {
                    InitializeGridViewData();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void InitializeGridViewData()
        {
            DataTable dtCategory = new DataTable();
            DataColumn dc = new DataColumn("TranId");
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1;
            dc.AutoIncrementStep = 1;
            dc.DataType = typeof(Int32);
            dtCategory.Columns.Add(dc);
            dtCategory.Columns.Add("CompanyName");
            dtCategory.Columns.Add("LocationName");
            dtCategory.Columns.Add("CategoryName");
            dtCategory.Columns.Add("RoleName");
            dtCategory.Columns.Add("CanPrepare");
            dtCategory.Rows.Add(dtCategory.NewRow());

            FillList.PopulateGridView(dtCategory, gvCompanyWisePermission);
        }

        private void LoadCompanyCategoryInfo()
        {
            try
            {
                DataTable dtCompany = PopulateLists.GetCompanies();
                DataTable dtLocation = PopulateLists.GetLocations();
                DataTable dtSerivce = PopulateLists.GetCategorys();

                ViewState["Company"] = dtCompany;
                ViewState["Location"] = dtLocation;
                ViewState["Job"] = dtSerivce;
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion

        #region Button Click Related

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSearchString.Text) && txtSearchString.Text.Contains(":") && txtSearchString.Text.Length >= 5)
            {
                String sEmployeeCode = txtSearchString.Text.Substring(0, txtSearchString.Text.IndexOf(':')).Trim();
                txtSearchString.Text = sEmployeeCode;

                DataTable _dtUser = LoadUserInformation(sEmployeeCode);
                if (_dtUser.Rows.Count > 0)
                {
                    hfUserID.Value = _dtUser.Rows[0]["UserId"].ToString();
                    txtName.Text = _dtUser.Rows[0]["UserName"].ToString();
                    txtDesignation.Text = _dtUser.Rows[0]["DesignationName"].ToString();
                    txtDepartment.Text = _dtUser.Rows[0]["DepartmentName"].ToString();
                    txtCompany.Text = _dtUser.Rows[0]["CompanyName"].ToString();

                    BindGridViewUserCompany();
                }
                else
                {
                    hfUserID.Value = String.Empty;
                    txtName.Text = String.Empty;
                    txtDesignation.Text = String.Empty;
                    txtDepartment.Text = String.Empty;
                    txtCompany.Text = String.Empty;

                    DisplayMessage("No user found by the login ID");
                }
            }
            else
            {
                DisplayMessage("Please select any valid user then try again.");
            }
        }

        protected void btnCopyRefresh_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCopyLoginId.Text) && txtCopyLoginId.Text.Contains(":") && txtCopyLoginId.Text.Length >= 5)
            {
                String sEmployeeCode = txtCopyLoginId.Text.Substring(0, txtCopyLoginId.Text.IndexOf(':')).Trim();
                txtCopyLoginId.Text = sEmployeeCode;

                DataTable _dtUser = LoadUserInformation(sEmployeeCode);
                if (_dtUser.Rows.Count > 0)
                {
                    hfCopyUserId.Value = _dtUser.Rows[0]["UserId"].ToString();
                    txtCopyName.Text = _dtUser.Rows[0]["UserName"].ToString();
                    txtCopyDesignation.Text = _dtUser.Rows[0]["DesignationName"].ToString();
                    txtCopyDepartment.Text = _dtUser.Rows[0]["DepartmentName"].ToString();
                    txtCopyCompany.Text = _dtUser.Rows[0]["CompanyName"].ToString();
                }
                else
                {
                    hfCopyUserId.Value = String.Empty;
                    txtCopyName.Text = String.Empty;
                    txtCopyDesignation.Text = String.Empty;
                    txtCopyDepartment.Text = String.Empty;
                    txtCopyCompany.Text = String.Empty;

                    DisplayMessage("No user found by the login ID");
                }
            }
            else
            {
                DisplayMessage("Please select any valid user then try again.");
            }
        }

        protected void btnCopyClear_Click(object sender, EventArgs e)
        {
            hfCopyUserId.Value = String.Empty;
            txtCopyLoginId.Text=String.Empty;
            txtCopyName.Text = String.Empty;
            txtCopyDesignation.Text = String.Empty;
            txtCopyDepartment.Text = String.Empty;
            txtCopyCompany.Text = String.Empty;

            txtCopyLoginId.Focus();
        }

        protected void btnCopyPermission_Click(object sender, EventArgs e)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[4]
                {
                    _dataManager.MakeInParam("@FromUserId", SqlDbType.NVarChar, 500, hfUserID.Value),
                    _dataManager.MakeInParam("@ToUserId", SqlDbType.NVarChar, 500, hfCopyUserId.Value),
                    _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "COPY")
                };
                DataTable dtResult = _dataManager.GetDataTable("SP_SYS_USER_COMPANY_PERMISSION", parameters);

                if(dtResult.Rows.Count > 0)
                {
                    DisplayMessage("Company wise permission has been copy from user "+txtName.Text+" to "+txtCopyName.Text+"");
                }
                else
                {
                    DisplayMessage("Company wise permission copy failed");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected DataTable LoadUserInformation(String sEmployeeCode)
        {
            DataTable dtUserInfo = new DataTable();
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT UserId,UserName,DG.LookupText AS DesignationName,D.LookupText AS DepartmentName,C.LookupText AS CompanyName FROM Sys_Users U   ");
                sQuery.Append("LEFT JOIN dbo.Sys_Lookup DG ON DG.LookupTypeId=3 AND U.DesignationId=DG.LookupValue  ");
                sQuery.Append("LEFT JOIN dbo.Sys_Lookup D ON D.LookupTypeId=2 AND U.DepartmentId=D.LookupValue  ");
                sQuery.Append("LEFT JOIN dbo.Sys_Lookup C ON C.LookupTypeId=1 AND U.CompanyId=C.LookupValue  ");
                sQuery.Append("WHERE LoginID='" + sEmployeeCode.Trim() + "'   ");


                _dataManager = new DataManager();
                dtUserInfo = _dataManager.GetDataTable(sQuery.ToString());
            }

            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return dtUserInfo;
        }
        #endregion

        #region GridView Related Methods
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCategoryName = (DropDownList)gvCompanyWisePermission.FooterRow.FindControl("ddlCategory");
                DropDownList ddlRoleName = (DropDownList)gvCompanyWisePermission.FooterRow.FindControl("ddlRole");

                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT FlowId,FlowName FROM Sys_Flowpath     ");
                sQuery.Append("WHERE CategoryId='"+ ddlCategoryName.SelectedValue + "' AND IsActive=1 ORDER by SerialNo ASC    ");

                _dataManager = new DataManager();
                DataTable dtRoles = _dataManager.GetDataTable(sQuery.ToString());
                FillList.PopulateDropDownList(dtRoles, ddlRoleName, "FlowName", "FlowId", true,"Select Flow","0");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the softwaFlowIDre vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void gvCompanyWisePermission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drview = e.Row.DataItem as DataRowView;

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlCompanyName = (DropDownList)e.Row.FindControl("ddlCompany");
                DataTable dtCompany = (DataTable)ViewState["Company"];
                FillList.PopulateDropDownList(dtCompany, ddlCompanyName, "Select Company");

                DropDownList ddlCategoryName = (DropDownList)e.Row.FindControl("ddlCategory");
                DataTable dtCategory = (DataTable)ViewState["Job"];
                FillList.PopulateDropDownList(dtCategory, ddlCategoryName, "Select Category");
            }
        }

        protected void gvCompanyWisePermission_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                DropDownList ddlAddCompanyName = (DropDownList)gvCompanyWisePermission.FooterRow.FindControl("ddlCompany");
                DropDownList ddlAddCategoryName = (DropDownList)gvCompanyWisePermission.FooterRow.FindControl("ddlCategory");
                DropDownList ddlAddRoleName = (DropDownList)gvCompanyWisePermission.FooterRow.FindControl("ddlRole");
                CheckBox cbPrepare = (CheckBox)gvCompanyWisePermission.FooterRow.FindControl("cbPrepare");
               
                Boolean isCanPrepare = cbPrepare.Checked;

                //if (ddlAddRoleName.SelectedIndex == 1)
                //{
                //    isCanPrepare = true;
                //}

                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("IF NOT EXISTS (SELECT 1 FROM Sys_UserCompany WHERE UserID='" + hfUserID.Value + "' AND CompanyId='" + ddlAddCompanyName.SelectedValue + "'    ");
                sQuery.Append("AND CategoryId='" + ddlAddCategoryName.SelectedValue+"' AND FlowId='"+ddlAddRoleName.SelectedValue+"')    ");
                sQuery.Append("BEGIN    ");
                sQuery.Append("INSERT INTO Sys_UserCompany(UserId,CompanyId,CategoryId,FlowId,CanPrepare,EntryBy,EntryDate)    ");
                sQuery.Append("Values("+ hfUserID.Value + ",'"+ddlAddCompanyName.SelectedValue+"','"+ddlAddCategoryName.SelectedValue+"','"+ddlAddRoleName.SelectedValue+"','"+ isCanPrepare + "','"+ _user.GetCookie(CookieKey.UserId.ToString()) + "',GETDATE())    ");
                sQuery.Append("END");

                _dataManager = new DataManager();
                Int32 iAffRow = _dataManager.ExecuteNonQuery(sQuery.ToString());

                gvCompanyWisePermission.EditIndex = -1;
                BindGridViewUserCompany();

                DisplayMessage(" Record inserted successfully!");
            }
        }
        
        protected void gvCompanyWisePermission_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCompanyWisePermission.EditIndex = -1;
            BindGridViewUserCompany();
        }
        
        protected void gvCompanyWisePermission_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField TranID = (HiddenField)gvCompanyWisePermission.Rows[e.RowIndex].FindControl("hTranID");

            String sQuery = "DELETE FROM Sys_UserCompany WHERE TranID=" + Convert.ToInt32(TranID.Value);
            _dataManager = new DataManager();
            int rec =_dataManager.ExecuteNonQuery(sQuery);

            gvCompanyWisePermission.EditIndex = -1;
            BindGridViewUserCompany();
            DisplayMessage("Record deleted successfully!");
        }
        
        protected void gvCompanyWisePermission_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCompanyWisePermission.EditIndex = e.NewEditIndex;
            BindGridViewUserCompany();
        }

        #endregion

        #region Common Methods
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion
    }
}