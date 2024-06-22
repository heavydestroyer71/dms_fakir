using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Text;
using System.Linq;
using System.Web.UI.WebControls;

namespace FakirDMS.UI
{
    public partial class TeamSetup : System.Web.UI.Page
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

            if (!Page.IsPostBack)
            {
            }
        }

        #region Button Click Related

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSearchString.Text) && txtSearchString.Text.Contains(":") && txtSearchString.Text.Length >= 3)
            {
                String sEmployeeCode = txtSearchString.Text.Substring(0, txtSearchString.Text.IndexOf(':')).Trim();
                txtSearchString.Text = sEmployeeCode;

                DataTable _dtUser = LoadUserInformation(sEmployeeCode);
                if (_dtUser.Rows.Count > 0)
                {
                    hfSupervisorId.Value = _dtUser.Rows[0]["UserId"].ToString();
                    txtName.Text = _dtUser.Rows[0]["UserName"].ToString();
                    txtDesignation.Text = _dtUser.Rows[0]["DesignationName"].ToString();
                    txtDepartment.Text = _dtUser.Rows[0]["DepartmentName"].ToString();
                    txtCompany.Text = _dtUser.Rows[0]["CompanyName"].ToString();

                    BindGridViewTeamMember();
                }
                else
                {
                    hfSupervisorId.Value = String.Empty;
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
                    hfMemberId.Value = _dtUser.Rows[0]["UserId"].ToString();
                    txtCopyName.Text = _dtUser.Rows[0]["UserName"].ToString();
                    txtCopyDesignation.Text = _dtUser.Rows[0]["DesignationName"].ToString();
                    txtCopyDepartment.Text = _dtUser.Rows[0]["DepartmentName"].ToString();
                    txtCopyCompany.Text = _dtUser.Rows[0]["CompanyName"].ToString();
                }
                else
                {
                    hfMemberId.Value = String.Empty;
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
            hfMemberId.Value = String.Empty;
            txtCopyLoginId.Text=String.Empty;
            txtCopyName.Text = String.Empty;
            txtCopyDesignation.Text = String.Empty;
            txtCopyDepartment.Text = String.Empty;
            txtCopyCompany.Text = String.Empty;

            txtCopyLoginId.Focus();
        }

        protected void btnSaveTeamMembern_Click(object sender, EventArgs e)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[4]
                {
                    _dataManager.MakeInParam("@SupervisorId", SqlDbType.NVarChar, 500, hfSupervisorId.Value),
                    _dataManager.MakeInParam("@TeamMemberId", SqlDbType.NVarChar, 500, hfMemberId.Value),
                    _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "SAVE")
                };
                DataTable dtResult = _dataManager.GetDataTable("SP_SYS_TEAM_MEMBER", parameters);

                if(dtResult.Rows.Count > 0 && Convert.ToInt32(dtResult.Rows[0]["Type"])>0)
                {
                    DisplayMessage(dtResult.Rows[0]["Result"].ToString());
                    BindGridViewTeamMember();
                }
                else
                {
                    DisplayMessage(dtResult.Rows[0]["Result"].ToString());
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

        protected void BindGridViewTeamMember()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@SupervisorId", SqlDbType.NVarChar, 500, hfSupervisorId.Value),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataTable _dsReturn = _dataManager.GetDataTable("SP_SYS_TEAM_MEMBER", parameters);
                FillList.PopulateGridView(_dsReturn, gvTeamMember);

            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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

        protected void gvTeamMember_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                String sMemberId = ((HiddenField)row.FindControl("hfMemberId")).Value;

                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@MemberId", SqlDbType.NVarChar, 500, sMemberId),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "DELETE")
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_TEAM_MEMBER", parameters);
                if (Convert.ToInt32(_dtReturn.Rows[0]["Type"]) > 0)
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                    BindGridViewTeamMember();
                }
                else
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                }

            }
        }
    }
}