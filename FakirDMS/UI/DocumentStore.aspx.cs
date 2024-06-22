using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Services.Discovery;

namespace FakirDMS.UI
{
    public partial class DocumentStore : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
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
                BindDropDownListRoom();
                BingGridViewDocumentList();
            }
        }

        #region Page Load Related Event

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

        protected void BindDropDownListRoom()
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetRooms();
                FillList.PopulateDropDownList(_dtRoom, ddlRoomName, "Select Room");
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
                SqlParameter[] parameters = new SqlParameter[5]
                {
                    _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                    _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                    _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
                    _dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearchBy.Text),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "SEARCH")
                };

                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_STORE", parameters);

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

        protected void btnSaveLocation_Click(object sender, EventArgs e)
        {
            try
            {
                String sDocumentId = String.Empty;

                foreach(GridViewRow gvRow in gvDocuments.Rows)
                {
                    String sDocId = ((HiddenField)gvRow.FindControl("gvHfDocumentId")).Value;
                    CheckBox cbSelect = (CheckBox)gvRow.FindControl("gvCbSelect");

                    if(cbSelect.Checked == true)
                    {
                        sDocumentId = sDocumentId+ sDocId + ",";
                    }
                }
                sDocumentId = sDocumentId.Substring(0, sDocumentId.Length - 1);

                if(sDocumentId.Length > 0 )
                {
                    String sQuery = "UPDATE DocumentInfo SET BoxId=" + ddlBoxName.SelectedValue + " WHERE DocumentId IN (" + sDocumentId + ")";
                    _dataManager.ExecuteNonQuery(sQuery);

                    DisplayMessage("Document storage location has been updated.");
                    BingGridViewDocumentList();
                }
                else
                {
                    DisplayMessage("Please select any document first");
                }
            }
            catch(Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion


        #region DropDownList Selected Index Changed

        protected void gvDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocuments.PageIndex = e.NewPageIndex;
            BingGridViewDocumentList();
        }

        protected void ddlRoomName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetRoomWiseRack(ddlRoomName.SelectedValue);
                FillList.PopulateDropDownList(_dtRoom, ddlRackName, "Select Rack");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void ddlRackName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetRackWiseShelf(ddlRackName.SelectedValue);
                FillList.PopulateDropDownList(_dtRoom, ddlShelfName, "Select Shelf");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void ddlShelfName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetShelfWiseBox(ddlShelfName.SelectedValue);
                FillList.PopulateDropDownList(_dtRoom, ddlBoxName, "Select Box");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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