using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FakirDMS.UI
{
    public partial class Flowpath : System.Web.UI.Page
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

            Form.DefaultButton = btnRefresh.UniqueID;
            if (!IsPostBack)
            {
                LoadDropDownListSerivce();
            }
        }

        #region Page Load Related

        protected void LoadDropDownListSerivce()
        {
            DataTable dtCategory = PopulateLists.GetCategorys();
            FillList.PopulateDropDownList(dtCategory, ddlCategory, "Select Category");
        }

        #endregion

        #region
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGridViewCategoryFlow();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValidation())
                {
                    DataManager dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[30]
                    {
                        dataManager.MakeInParam("@FlowID", SqlDbType.NVarChar, 500, hfFlowId.Value),
                        dataManager.MakeInParam("@CompanyID", SqlDbType.NVarChar, 500, String.Empty),
                        dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                        dataManager.MakeInParam("@FlowName", SqlDbType.NVarChar, 500, txtFlowName.Text),
                        dataManager.MakeInParam("@Description", SqlDbType.NVarChar, 500, txtDiscription.Text),
                        dataManager.MakeInParam("@TnaDays", SqlDbType.NVarChar, 500, txtTnaDays.Text),
                        dataManager.MakeInParam("@SerialNo", SqlDbType.NVarChar, 500, txtSerialNo.Text.ToString()),

                        dataManager.MakeInParam("@IsPO", SqlDbType.NVarChar, 500, (cbPOEnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsPI", SqlDbType.NVarChar, 500, (cbPIEnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsLC", SqlDbType.NVarChar, 500, (cbLCEnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsMR", SqlDbType.NVarChar, 500, (cbMREnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsCL", SqlDbType.NVarChar, 500, "0"),

                        dataManager.MakeInParam("@IsBill", SqlDbType.NVarChar, 500, (cbBillEnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsAmount", SqlDbType.NVarChar, 500, (cbAmountEnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsDiscount", SqlDbType.NVarChar, 500, (cbDiscountEnable.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsAccounts", SqlDbType.NVarChar, 500, (cbAccountEnable.Checked ? "1" : "0")),
						dataManager.MakeInParam("@IsPayableVarcharNo", SqlDbType.NVarChar, 500, (cbAccountPayableVarNo.Checked ? "1" : "0")),dataManager.MakeInParam("@IsAccountPayableVarDate", SqlDbType.NVarChar, 500, (cbAccountPayableVarDate.Checked ? "1" : "0")),dataManager.MakeInParam("@IspaymentVarNo", SqlDbType.NVarChar, 500, (cbAccountPaymentVarNo.Checked ? "1" : "0")),dataManager.MakeInParam("@IsPaymentDate", SqlDbType.NVarChar, 500, (cbAccountPaymentVarDate.Checked ? "1" : "0")),
						dataManager.MakeInParam("@CanUpload", SqlDbType.NVarChar, 500, (cbUpload.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@CanDownload", SqlDbType.NVarChar, 500, (cbDownload.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@CanDelete", SqlDbType.NVarChar, 500, (cbDelete.Checked ? "1" : "0")),

                        dataManager.MakeInParam("@IsSupervisor", SqlDbType.NVarChar, 500, (cbIsSupervisor.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsTeamMember", SqlDbType.NVarChar, 500, (cbIsTeamMember.Checked ? "1" : "0")),

                        dataManager.MakeInParam("@IsApprover", SqlDbType.NVarChar, 500, (cbIsApprover.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsCloser", SqlDbType.NVarChar, 500, (cbIsCloser.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsActive", SqlDbType.NVarChar, 500, (cbIsActive.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, btnSave.Text)
                    };

                    DataTable _dtRerun = _dataManager.GetDataTable("SP_SYS_FLOWPATH", parameters);
                    if (_dtRerun.Rows[0]["Type"].ToString() == "1")
                    {
                        DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                        ClearAllControls();
                        BindGridViewCategoryFlow();
                    }
                    else
                    {
                        DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                    }
                }
                else
                {
                    DisplayMessage(_validationMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected Boolean CheckValidation()
        {
            bool _result = true;
            if (ddlCategory.SelectedValue=="0")
            {
                _validationMessage = "Please select Category name";
                ddlCategory.Focus();
                _result = false;
            }
            else if (String.IsNullOrEmpty(txtFlowName.Text))
            {
                _validationMessage = "Please select flow name";
                txtFlowName.Focus();
                _result = false;
            }
            else if (String.IsNullOrEmpty(txtSerialNo.Text))
            {
                _validationMessage = "Please enter serial number for flow";
                txtSerialNo.Focus();
                _result = false;
            }
            else if (gvCategoryFlow.Rows.Count > 0)
            {
                foreach(GridViewRow row in gvCategoryFlow.Rows)
                {
                    if (txtFlowName.Text == row.Cells[1].Text.ToString() && btnSave.Text=="Save")
                    {
                        _validationMessage = "Flow Name already existed. Please select different flow name";
                        _result = false;
                    }
                }
            }

            return _result;
        }

        protected void ClearAllControls()
        {
            txtFlowName.Text = String.Empty;
            txtSerialNo.Text = String.Empty;
            txtDiscription.Text = String.Empty;
            txtTnaDays.Text = String.Empty;

            cbPIEnable.Checked = false;
            cbPOEnable.Checked = false;
            cbLCEnable.Checked = false;
            cbMREnable.Checked = false;
            //cbChallanEnable.Checked = false;

            cbBillEnable.Checked = false;
            cbAmountEnable.Checked = false;
            cbDiscountEnable.Checked = false;
            cbAccountEnable.Checked = false;

            cbUpload.Checked = false;
            cbDownload.Checked = false; 
            cbDelete.Checked = false;

            cbIsSupervisor.Checked = false;
            cbIsTeamMember.Checked = false;

            cbIsApprover.Checked = false;
            cbIsCloser.Checked = false;
            cbIsActive.Checked = true;

            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
        }

        private void BindGridViewCategoryFlow()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataTable dtResult = dataManager.GetDataTable("SP_SYS_FLOWPATH", parameters);
                FillList.PopulateGridView(dtResult, gvCategoryFlow);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        #region GridView Realted
        protected void gvCategoryFlow_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategoryFlow.PageIndex = e.NewPageIndex;
            BindGridViewCategoryFlow();
        }

        protected void gvCategoryFlow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                hfFlowId.Value = row.Cells[0].Text;
                txtSerialNo.Text = row.Cells[1].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtFlowName.Text = row.Cells[2].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtDiscription.Text = row.Cells[3].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtTnaDays.Text = row.Cells[4].Text.Replace("&nbsp;", "").Replace("&amp;", "&");

                cbPIEnable.Checked = ((CheckBox)row.FindControl("gcbPiEnable")).Checked;
                cbPOEnable.Checked = ((CheckBox)row.FindControl("gcbPoEnable")).Checked;
                cbLCEnable.Checked = ((CheckBox)row.FindControl("gcbLCEnable")).Checked;
                cbMREnable.Checked = ((CheckBox)row.FindControl("gcbMrEnable")).Checked;
                //cbChallanEnable.Checked = ((CheckBox)row.FindControl("gcbClEnable")).Checked;
                
                cbBillEnable.Checked = ((CheckBox)row.FindControl("gcbBillEnable")).Checked;
                cbAmountEnable.Checked = ((CheckBox)row.FindControl("gcbAmountEnable")).Checked;
                cbDiscountEnable.Checked = ((CheckBox)row.FindControl("gcbDiscountEnable")).Checked;
                cbAccountEnable.Checked = ((CheckBox)row.FindControl("gcbAccountEnable")).Checked;
                cbUpload.Checked = ((CheckBox)row.FindControl("gcbUpload")).Checked;
                cbDownload.Checked = ((CheckBox)row.FindControl("gcbDownlaod")).Checked;
                cbDelete.Checked = ((CheckBox)row.FindControl("gcbDelete")).Checked;

                cbIsSupervisor.Checked = ((CheckBox)row.FindControl("gcbIsSupervisor")).Checked;
                cbIsTeamMember.Checked = ((CheckBox)row.FindControl("gcbIsTeamMember")).Checked;

                cbIsApprover.Checked = ((CheckBox)row.FindControl("gcbIsApprover")).Checked;
                cbIsCloser.Checked = ((CheckBox)row.FindControl("gcbIsCloser")).Checked;
                cbIsActive.Checked = ((CheckBox)row.FindControl("gcbIsActive")).Checked;
                btnSave.Text = "Update";
            }

            if (e.CommandName.Equals("DeleteRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                String sQuery = "DELETE FROM Sys_Flowpath WHERE FlowID=" + row.Cells[0].Text;

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery);
                BindGridViewCategoryFlow();
            }
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