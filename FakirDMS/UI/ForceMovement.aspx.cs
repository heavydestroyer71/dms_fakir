using System;
using CoreLibrary;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mime;
using System.Xml;
using FokirDMS;


namespace FakirDMS.UI
{
    public partial class ForceMovement : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
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
            }
        }

        #region Page Load Related

        protected void LoadDropDownListRole()
        {
            DataTable dtRole = PopulateLists.GetFlowsByCategory(hfCategoryId.Value);
            FillList.PopulateDropDownList(dtRole, ddlRole, "Select Flow");
        }

        #endregion

        #region Click Related
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBillRef.Text))
            {
                LoadDocumentInformation();
                LoadDropDownListRole();
            }
            else
            {
                DisplayMessage("Please enter the Tracking No.");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[8]
                {
                     _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value.ToString()),
                     _dataManager.MakeInParam("@RoleId", SqlDbType.NVarChar, 500, ddlRole.SelectedValue),
					 _dataManager.MakeInParam("@PrevRoleId", SqlDbType.NVarChar, 500, lblPrevRoleId.Text),
					 _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, ddlUser.SelectedValue),
					 _dataManager.MakeInParam("@PrevUserId", SqlDbType.NVarChar, 500, lblPrevUserId.Text),
					 _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarks.Text),
                     _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                     _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Forward")
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_FORCE_MOVEMENT", parameters);
                if (_dtReturn.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(_dtReturn.Rows[0]["Result"].ToString()))
                    {
                        DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                    }
                }
            }

            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[3]
                {
                     _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value.ToString()),
                     _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                     _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Delete")
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_FORCE_MOVEMENT", parameters);
                if (_dtReturn.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(_dtReturn.Rows[0]["Result"].ToString()))
                    {
                        DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
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

        #region Others Methods
        private void LoadDocumentInformation()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                     _dataManager.MakeInParam("@BillREfNo",  SqlDbType.NVarChar, 500,txtBillRef.Text.ToString())
                };

                DataSet dsResult = _dataManager.GetDataSet("SP_SELECT_DOCUMENT", parameters);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsResult.Tables[0].Rows)
                    {
                        lblExpenseType.Text = dr["ExpenseType"].ToString();
                        lblCompany.Text = dr["ComapnyName"].ToString();
                        lblDepartment.Text = dr["Department"].ToString();
                        lblDuration.Text = dr["Duration"].ToString();
                        lblBillRefNo.Text = dr["BillREfNo"].ToString();
                        lblBillAmount2.Text = dr["BillAmount"].ToString();
                        lblDuration.Text = dr["Waiting"].ToString();
                        lblCategoryName.Text = dr["CategoryName"].ToString();
                        lblRoleName.Text = dr["RoleName"].ToString();
                        lblPrevRoleId.Text = dr["RoleID"].ToString();
						lblUserName.Text = dr["PrevFlowUserName"].ToString();
						lblPrevUserId.Text = dr["PrevFlowUserId"].ToString();
						hfDocumentId.Value = dr["DocumentId"].ToString();
                        hfCategoryId.Value = dr["CategoryId"].ToString();
                    }
                }
                else
                {
                    lblExpenseType.Text = String.Empty;
                    lblCompany.Text = String.Empty;
                    lblDepartment.Text = String.Empty;
                    lblDuration.Text = String.Empty;
                    lblBillRefNo.Text = String.Empty;
                    lblBillAmount2.Text = String.Empty;
                    lblDuration.Text = String.Empty;
                    lblCategoryName.Text = String.Empty;
                    lblRoleName.Text = String.Empty;
                    hfCategoryId.Value = String.Empty;
                }

                if (dsResult.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsResult.Tables[3].Rows)
                    {
                        lblCurrentStatus.Text = dr["Status"].ToString();
                    }
                }
                else
                {
                    lblCurrentStatus.Text = "Status: Unknown";
                }
            }
            catch (Exception ex)
            {
                lblCurrentStatus.Text = ex.Message;
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
		#endregion

		protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Get the selected role value
			string selectedRole = ddlRole.SelectedValue;

			// FlowId to filter data (example value: 6)
			//int flowId = 6;

			// Fetch data from the database
			DataTable _users = PopulateLists.GetUsersbyFlow(selectedRole.ToInt(), 0 ,"");
			FillList.PopulateDropDownList(_users, ddlUser, "Select User");
		}
	}
}