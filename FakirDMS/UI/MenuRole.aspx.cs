using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using iTextSharp.text;

namespace FakirDMS.UI
{
    public partial class MenuRole : System.Web.UI.Page
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

            if (!this.IsPostBack)
            {
                LoadDropDownListCategory();
            }
        }

        #region Page Load Related

        protected void LoadDropDownListCategory()
        {
            DataTable dtCategory = PopulateLists.GetCategorys();
            FillList.PopulateDropDownList(dtCategory, ddlCategory, "Select Category");
        }

        protected void BindGridViewMenuList()
        {
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT P.PermissionID,A.MenuId, CASE WHEN A.Parent IS NULL THEN A.Title ELSE (A.Parent +' >> '+A.Title) END MenuTitle, A.Description, A.Url,CAST((CASE WHEN P.PermissionID IS NULL THEN 'FALSE' ELSE 'TRUE' END) AS BIT) Assigned   ");
                sQuery.Append("FROM (SELECT S.ID AS MenuId, Title, Description, Url,(SELECT Title FROM Sys_Menu WHERE ID=S.ParentId) AS Parent,S.ParentId, S.SLNo   ");
                sQuery.Append("FROM Sys_Menu S  WHERE S.IsActive=1 AND S.ID NOT IN (SELECT ID FROM Sys_Menu WHERE ParentId=0 AND IsParentOnly=0)) A    ");
                sQuery.Append("LEFT JOIN Sys_MenuPermission P ON A.MenuId=P.MenuID AND P.FlowId=" + ddlRoleName.SelectedValue+"   ");
                sQuery.Append("ORDER BY ParentId, SLNo   ");

                // AND S.CategoryId IS NULL

                _dataManager = new DataManager();
                DataTable dtMenus = _dataManager.GetDataTable(sQuery.ToString());
                FillList.PopulateGridView(dtMenus, gvMenuList);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        #region Click Event Related

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtRole = PopulateLists.GetFlowsByCategory(ddlCategory.SelectedValue);
            FillList.PopulateDropDownList(dtRole, ddlRoleName, "Select Flowpath");
        }

        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridViewMenuList();
        }

        protected void cbSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbMenuItem = (CheckBox)sender;
            GridViewRow grdrDropDownRow = (GridViewRow)cbMenuItem.Parent.Parent;

            if (cbMenuItem.Checked)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("INSERT INTO Sys_MenuPermission(FlowId, MenuID, EntryBy, EntryDate)   ");
                sQuery.Append("VALUES ("+ ddlRoleName.SelectedValue+", "+grdrDropDownRow.Cells[1].Text+", "+1+", GETDATE())");

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery.ToString());
            }
            else
            {
                String sQuery = "DELETE FROM Sys_MenuPermission  WHERE PermissionID=" + grdrDropDownRow.Cells[0].Text;

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery);
            }
        }
        #endregion

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
    }
}