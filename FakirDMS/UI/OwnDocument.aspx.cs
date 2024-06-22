using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.DynamicData;

namespace FakirDMS.UI
{
    public partial class OwnDocument : System.Web.UI.Page
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

                if (!string.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    String sStatus = Request.QueryString["Status"].ToString();
                    if (sStatus!="-10" )
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(sStatus));

                    BindGridViewOwnDocument(sStatus);
                }
                else
                {
                    BindGridViewOwnDocument();
                }
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
            FillList.PopulateDropDownList(dtExpense, ddlExpenseType, "Select Expense Type");
        }

        protected void LoadDropDownListStatus()
        {
            DataTable dtExpense = PopulateLists.GetStatuses();
            FillList.PopulateDropDownList(dtExpense, ddlStatus, "DisplayField", "ValueField",true, "Select Status", "99");
        }

        #endregion


        #region Page Load Related Event

        protected void BindGridViewOwnDocument()
        {
            try
            {
                string ProcessId = "1";
                if (!string.IsNullOrEmpty(Request.QueryString["ProcessId"])) { ProcessId = Request.QueryString["ProcessId"]; }
            
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[9]
                {
                        _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                        _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpenseType.SelectedValue),
                        _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                        _dataManager.MakeInParam("@StatusId", SqlDbType.NVarChar, 500, ddlStatus.SelectedValue),
                        _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
                        _dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtPartyName.Text),
                        _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "OWN"),
                        _dataManager.MakeInParam("@ProcessId", SqlDbType.NVarChar, 500, ProcessId)
                };
                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_LIST_OWN", parameters);

                lblDraftCount.Text = dtDocuments.Rows.Count.ToString();
                FillList.PopulateGridView(dtDocuments, gvDraftDocuments);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void BindGridViewOwnDocument(String StatusId)
        {
            try
            {
                string ProcessId = "1";

                if (!string.IsNullOrEmpty(Request.QueryString["ProcessId"])) { ProcessId = Request.QueryString["ProcessId"]; }

                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[4]
                {
                     _dataManager.MakeInParam("@StatusId", SqlDbType.NVarChar, 500, StatusId),
                     _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                     _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "OWN") ,
                     _dataManager.MakeInParam("@ProcessId", SqlDbType.NVarChar, 500, ProcessId)

                };
                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_LIST_OWN", parameters);

                lblDraftCount.Text = dtDocuments.Rows.Count.ToString();
                FillList.PopulateGridView(dtDocuments, gvDraftDocuments);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        protected void gvDraftDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDraftDocuments.PageIndex = e.NewPageIndex;
            BindGridViewOwnDocument();
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewOwnDocument();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/OwnDocument.aspx", false);
        }
    }
}