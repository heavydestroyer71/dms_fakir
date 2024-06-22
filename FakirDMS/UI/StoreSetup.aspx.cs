using CoreLibrary;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace FakirDMS.UI
{
    public partial class StoreSetup : System.Web.UI.Page
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

            Form.DefaultButton = btnSearch.UniqueID;
            if (!Page.IsPostBack)
            {
                BindDropDownListRoom();
                BindGridViewLookup();
            }
        }

        #region Page Load Related

        protected void BindDropDownListRoom()
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetRooms();
                FillList.PopulateDropDownList(_dtRoom, rack_ddlRoomName,"Select Room");
                FillList.PopulateDropDownList(_dtRoom, shelf_ddlRoomName, "Select Room");
                FillList.PopulateDropDownList(_dtRoom, box_ddlRoomName, "Select Room");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void BindGridViewLookup()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearch.Text.Trim()),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_STORE", parameters);
                FillList.PopulateGridView(_dtReturn, gvLookup);
                MergeRows(gvLookup, 3);

            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public static void MergeRows(GridView gridView, int ColNumber)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < ColNumber; i++)
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 : previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }

        #endregion


        #region Button Related Methods

        protected void room_btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(room_txtName.Text))
            {
                DisplayMessage("Please enter ROOM Name");
                return;
            }
            else
            {
                String id = hfId.Value;
                String roomName = room_txtName.Text;
                Boolean isActive = room_cbIsActive.Checked;
                String action = room_btnSave.Text;

                DataTable _dtReturn = SaveUpdateInformation(id, "1", "0", roomName, isActive, action);
                if (_dtReturn.Rows[0]["Type"].ToString() == "1")
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                    BindGridViewLookup();

                    room_txtName.Text = String.Empty;
                    room_btnSave.Text = "Save";
                }
                else
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                }
            }
        }

        protected void rack_btnSave_Click(object sender, EventArgs e)
        {
            if (rack_ddlRoomName.SelectedValue=="0")
            {
                DisplayMessage("Please select room name from Rack Section");
                return;
            }
            else if (String.IsNullOrEmpty(rack_txtRackName.Text))
            {
                DisplayMessage("Please enter Rack Name from Rack Section");
                return;
            }
            else
            {
                String id = hfId.Value;
                String parentId = rack_ddlRoomName.SelectedValue;
                String rackName = rack_txtRackName.Text;
                Boolean isActive = rack_cbIsActive.Checked;
                String action = rack_btnSave.Text;

                DataTable _dtReturn = SaveUpdateInformation(id, "2", parentId, rackName, isActive, action);
                if (_dtReturn.Rows[0]["Type"].ToString() == "1")
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                    BindGridViewLookup();

                    rack_ddlRoomName.SelectedIndex=0;
                    rack_txtRackName.Text = String.Empty;
                    rack_cbIsActive.Checked=true;
                    rack_btnSave.Text = "Save";
                }
                else
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                }
            }
        }

        protected void shelf_btnSave_Click(object sender, EventArgs e)
        {
            if (shelf_ddlRackName.SelectedValue == "0")
            {
                DisplayMessage("Please select rack name from Shelf Section");
                return;
            }
            else if (String.IsNullOrEmpty(shelf_txtShelfName.Text))
            {
                DisplayMessage("Please enter Shelf Name from Shelf Section");
                return;
            }
            else
            {
                String id = hfId.Value;
                String parentId = shelf_ddlRackName.SelectedValue;
                String rackName = shelf_txtShelfName.Text;
                Boolean isActive = shelf_cbIsActive.Checked;
                String action = shelf_btnSave.Text;

                DataTable _dtReturn = SaveUpdateInformation(id, "3", parentId, rackName, isActive, action);
                if (_dtReturn.Rows[0]["Type"].ToString() == "1")
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                    BindGridViewLookup();

                    shelf_ddlRackName.SelectedIndex = 0;
                    shelf_txtShelfName.Text = String.Empty;
                    shelf_cbIsActive.Checked = true;
                    shelf_btnSave.Text = "Save";
                }
                else
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                }
            }
        }

        protected void box_btnSave_Click(object sender, EventArgs e)
        {
            if (box_ddlShelfName.SelectedValue == "0")
            {
                DisplayMessage("Please select shelf name from Box Section");
                return;
            }
            else if (String.IsNullOrEmpty(box_txtBoxName.Text))
            {
                DisplayMessage("Please enter Box Name from Box Section");
                return;
            }
            else
            {
                String id = hfId.Value;
                String parentId = box_ddlShelfName.SelectedValue;
                String rackName = box_txtBoxName.Text;
                Boolean isActive = box_cbIsActive.Checked;
                String action = box_btnSave.Text;

                DataTable _dtReturn = SaveUpdateInformation(id, "4", parentId, rackName, isActive, action);
                if (_dtReturn.Rows[0]["Type"].ToString() == "1")
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                    BindGridViewLookup();

                    box_ddlShelfName.SelectedIndex = 0;
                    box_txtBoxName.Text = String.Empty;
                    box_cbIsActive.Checked = true;
                    box_cbIsActive.Text = "Save";
                }
                else
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString());
                }
            }
        }

        protected DataTable SaveUpdateInformation(String sId, String sLevelNo, String sParentId, String sName, Boolean IsActive, String sAction)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[7]
            {
                    dataManager.MakeInParam("@Id", SqlDbType.BigInt, 500, sId),
                    dataManager.MakeInParam("@LevelNo", SqlDbType.BigInt, 500, sLevelNo),
                    dataManager.MakeInParam("@ParentId", SqlDbType.BigInt, 500, sParentId),
                    dataManager.MakeInParam("@Name", SqlDbType.NVarChar, 500, sName),
                    dataManager.MakeInParam("@IsActive", SqlDbType.NVarChar, 500, (IsActive ? "1" : "0")),
                    dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sAction)
            };

            DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_STORE", parameters);
            return _dtReturn;
        }

        #endregion


        #region Load GridView Data
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewLookup();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
            BindGridViewLookup();
        }

        #endregion


        #region GridView Command

        protected void gvLookup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLookup.PageIndex = e.NewPageIndex;
            BindGridViewLookup();
        }

        protected void gvLookup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                //String sLookupId = ((HiddenField)row.FindControl("gHfLookupId")).Value;
                //String sLookupTypeId = ((HiddenField)row.FindControl("gHfLookupTypeId")).Value;
                //String sLookupValue = ((HiddenField)row.FindControl("gHfLookupValue")).Value;
                //String sIsRequired = ((HiddenField)row.FindControl("gHfIsRequired")).Value;

                //hfLookupId.Value= sLookupId.Replace("&nbsp;", "");
                //hfLookupValue.Value = sLookupValue.Replace("&nbsp;","");
                //cbIsActive.Checked = Convert.ToBoolean(sIsRequired);

                //txtSerial.Text = row.Cells[0].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                //txtName.Text = row.Cells[1].Text.Replace("&nbsp;", "").Replace("&amp;", "&");

                //bool isActive = true;
                //if (row.Cells[3].Text == "Inactive")
                //{
                //    isActive = false;
                //}
                //cbIsActive.Checked = isActive;
                //btnSave.Text = "Update";
            }
        }

        #endregion


        #region Others Command
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion


        #region DropDownList Selected Index Changed
        protected void shelf_ddlRoomName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetRoomWiseRack(shelf_ddlRoomName.SelectedValue);
                FillList.PopulateDropDownList(_dtRoom, shelf_ddlRackName, "Select Rack");
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
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void box_ddlRackName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable _dtRoom = PopulateLists.GetRackWiseShelf(box_ddlRackName.SelectedValue);
                FillList.PopulateDropDownList(_dtRoom, box_ddlShelfName, "Select Shelf");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion
    }
}