using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using CoreLibrary;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using FakirDMS.UI;

namespace FakirDMS
{
    public  partial class Default : System.Web.UI.Page
    {
        Cookie userCookie = new Cookie();
        DataManager _dataManager = new DataManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
				//UpdateCategorybyWo();
				 Session["UserInfo"] = null;

                String passwordString= UtilityClass.Decrypt("/KHS6ykGutM=", true);
            }
        }
		//UpdateCategorybyWo using to fetch category data through an API and insert into a table. So we can cross join to update the category in DocumentInfo
		protected async void UpdateCategorybyWo()
		{

			_dataManager = new DataManager();
			SqlParameter[] parameters = new SqlParameter[0]
			{

			};

			DataTable dtDocuments = _dataManager.GetDataTable("temp_GetAllWO", parameters);
			DataTable dtResult;
			List<WOs> documentList = new List<WOs>();
			foreach (DataRow row in dtDocuments.Rows)
			{
				int documentId = Convert.ToInt32(row["DocumentId"]);  // Fetches the DocumentId
				dtResult = await ApiClient.getCategoryByWO(row["RefNo"].ToString());
				foreach (DataRow r in dtResult.Rows)
				{
					string CategoryId = r["CATEGORY_ID"].ToString().Replace("&nbsp;", "");
					//string sQuery = String.Format("insert into tempRefCat(DocumentId, RefNo, CategoryId) values({ 0},{ 1} , { 2})", Convert.ToInt32(row["DocumentId"]), row["RefNo"].ToString(), CategoryId);
                    if(CategoryId=="0")
                    {

                    }
					string sQuery = String.Format("insert into tempRefCat(DocumentId, RefNo, CategoryId) values({0},'{1}',{2})", documentId, row["RefNo"].ToString(), r["CATEGORY_ID"].ToString());
                    _dataManager.ExecuteNonQuery(sQuery);
				}
				//string sQuery = String.Format("insert into tempRefCat(DocumentId, RefNo, CategoryId) values({ 0},{ 1} , { 2})", Convert.ToInt32(row["DocumentId"]), row["RefNo"].ToString(), dtResult.Rows);
			}
		}
        public class WOs
        {
			public int DocumentId { get; set; }
			public string RefNo { get; set; }
		}
        public class WO_CAT
        {
            public string WO_NUMBER { get; set; }
            public string CATEGORY_ID { get; set; }
            public string ACTUAL_CATEGORY_NAME { get; set; }
        }
		protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
				_dataManager = new DataManager();
                //DataTable dtUser = _dataManager.GetDataTable(sQuery);

				SqlParameter[] parameters = new SqlParameter[1]
				{
					_dataManager.MakeInParam("@EmployeeId", SqlDbType.NVarChar, 100, txtEmployeeId.Text.Trim())
				};
				DataTable dtUser = _dataManager.GetDataTable("SP_SELECT_LOGIN_USER", parameters);

				if (dtUser.Rows.Count > 0)
				{
					String sEncryptPassword = UtilityClass.Encrypt(txtPassword.Text.Trim(), true);
					if (sEncryptPassword.Equals(dtUser.Rows[0]["Password"].ToString()))
					{
						userCookie = new Cookie();
						userCookie.SetCookie(CookieKey.UserId.ToString(), dtUser.Rows[0]["UserID"].ToString());
						userCookie.SetCookie(CookieKey.RoleId.ToString(), dtUser.Rows[0]["FlowId"].ToString()); //role id is flow id
						userCookie.SetCookie(CookieKey.EmployeeId.ToString(), txtEmployeeId.Text.Trim());
						userCookie.SetCookie(CookieKey.CompanyId.ToString(), dtUser.Rows[0]["CompanyId"].ToString());
						userCookie.SetCookie(CookieKey.DepartmentId.ToString(), dtUser.Rows[0]["DepartmentId"].ToString());
						userCookie.SetCookie(CookieKey.UserName.ToString(), dtUser.Rows[0]["UserName"].ToString());


						//UpdateItemCategory("0");

						LoadUserWisMenuList();
						if (dtUser.Rows[0]["FlowId"].ToString() == "7" || dtUser.Rows[0]["FlowId"].ToString() == "26")
						{
							Response.Redirect(String.Format("~/UI/PaymentConfirm.aspx", false));
						}
						else if (dtUser.Rows[0]["FlowId"].ToString() == "8" || dtUser.Rows[0]["FlowId"].ToString() == "27")
						{
							Response.Redirect(String.Format("~/UI/BillClosing.aspx"), false);
						}
                        else 
						Response.Redirect("~/UI/Home.aspx", false);
					}
					else
					{
						DisplayMessage("Sorry! Password Invalid! Try Again.");
					}
				}
				else
				{
					userCookie.ClearCookie();
					userCookie.RemoveCookie();
					DisplayMessage("Sorry! Invalid Login! Try Again.");
				}
			}
            catch (Exception ex)
            {
                DisplayMessage("Login Failed.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(txtEmployeeId.Text.Trim(), this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected async void UpdateItemCategory(string CatgId)
        {

            DataTable dtResult;

            dtResult = await ApiClient.GetCategoryInfo(CatgId);


            foreach (DataRow row in dtResult.Rows)
            {

                string CategoryId = row["CATEGORY_ID"].ToString().Replace("&nbsp;", "");
                string CategoryName = row["SHORT_NAME"].ToString().Replace("&nbsp;", "");

                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    _dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, CategoryId),
                    _dataManager.MakeInParam("@CategoryName", SqlDbType.NVarChar, 500,CategoryName)

                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_UPDATE_CATEGORY", parameters);


            }



        }
        protected void LoadUserWisMenuList()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@UserId",  SqlDbType.NVarChar, 500,userCookie.GetCookie(CookieKey.UserId.ToString())),
                    dataManager.MakeInParam("@Action",  SqlDbType.NVarChar, 500,"LoadUserMenu")
                };

                DataTable dtMenuList = dataManager.GetDataTable("SP_SYS_USERS", parameters);
                if (dtMenuList.Rows.Count > 0)
                {
                    Session["MenuList"] = dtMenuList;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(txtEmployeeId.Text, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
    }
}