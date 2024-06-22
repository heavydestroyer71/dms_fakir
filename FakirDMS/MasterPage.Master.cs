using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using CoreLibrary;

namespace FakirDMS
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        Cookie user = new Cookie();
        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Check User Login Status
            if (String.IsNullOrEmpty(user.GetCookie(CookieKey.UserId.ToString())) || user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }
            #endregion

            if (!Page.IsPostBack)
            {
                GenerateMenuList();
                BindUserNameImage();
            }
        }

        #region Bind Menu and Profile Related
        private void GenerateMenuList()
        {
            try
            {
                if (Session["MenuList"] == null)
                {
                    LoadUserWisMenuList();
                }
                DataTable dtMenus = (DataTable)Session["MenuList"];
                if (dtMenus.Rows.Count > 0)
                {
                    divTopMenu.InnerHtml = MenuDAO.GetTopLevelMenuHTML(dtMenus);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(user.EmployeeId, Page.Title, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void LoadUserWisMenuList()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                    dataManager.MakeInParam("@UserId",  SqlDbType.NVarChar, 500,user.GetCookie(CookieKey.UserId.ToString())),
                    dataManager.MakeInParam("@Action",  SqlDbType.NVarChar, 500,"LoadUserMenu")
            };

            DataTable dtMenuList = dataManager.GetDataTable("SP_SYS_USERS", parameters);
            if (dtMenuList.Rows.Count > 0)
            {
                Session["MenuList"] = dtMenuList;
            }
        }

        protected void BindUserNameImage()
        {
            lbluserName.Text = user.GetCookie(CookieKey.UserName.ToString()) + " [" + user.GetCookie(CookieKey.EmployeeId.ToString()) + "]";

            int myDec;
            bool Result = int.TryParse(user.GetCookie(CookieKey.EmployeeId.ToString()), out myDec);
            if (Result == true)
            {
                string filePath = Server.MapPath("~/Images/EmployeePhoto/") + Convert.ToInt32(user.GetCookie(CookieKey.EmployeeId.ToString())).ToString();     // Request.QueryString["FilePath"];
                filePath = Path.ChangeExtension(filePath, ".jpg");
                if (File.Exists(filePath))
                    Image1.ImageUrl = "~/Images/EmployeePhoto/" + Convert.ToInt32(user.GetCookie(CookieKey.EmployeeId.ToString())).ToString() + ".jpg";
                else
                    Image1.ImageUrl = "~/Images/" + "no_photo.jpg";
            }
            else
                Image1.ImageUrl = Image1.ImageUrl = "~/Images/" + "no_photo.jpg";
        }
        #endregion

        #region Click and Other Related
        protected void lbLogout_Click(object sender, EventArgs e)
        {
            user.ClearCookie();
            user.RemoveCookie();
            Session.RemoveAll();
            Session.Clear();

            Response.Redirect("~/Default.aspx");
        }

        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/Profile.aspx");
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion
    }
}