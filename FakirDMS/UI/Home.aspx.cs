using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FokirDMS;

namespace FakirDMS
{
    public partial class Home : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {
			#region Check User Login Status
            if (_user.GetCookie(CookieKey.RoleId.ToString())== "7")
			{
				Response.Redirect(String.Format("~/UI/PaymentConfirm.aspx", false));
			}
			if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) || _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }
            #endregion

            if (!IsPostBack)
            {
				LoadTopDashboardData();
                LoadDashboardData();
				if (_user.GetCookie(CookieKey.RoleId.ToString()) == "2" )
				{
					dashboard_status.Visible = true;
					dashboard_total.Visible = true;
					dashboard_all.Visible = false;
				}
				else if (_user.GetCookie(CookieKey.RoleId.ToString()) == "6" || _user.GetCookie(CookieKey.RoleId.ToString()) == "4" || _user.GetCookie(CookieKey.RoleId.ToString()) == "3" || _user.GetCookie(CookieKey.RoleId.ToString()) == "5" )
				{
					dashboard_all.Visible = true;
					dashboard_total.Visible = true;
					dashboard_status.Visible = false;
				}
				else if (_user.GetCookie(CookieKey.RoleId.ToString()) == "8")
				{
					dashboard_all.Visible = false;
					dashboard_total.Visible = false;
					dashboard_status.Visible = false;
				}

			}
        }
		protected void LoadTopDashboardData()
		{
			try
			{
				_dataManager = new DataManager();
				SqlParameter[] parameters = new SqlParameter[1]
				{
						_dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString()))
				};
				DataSet dsDashboard = _dataManager.GetDataSet("SP_SYS_USER_TASK_DASHBOARD", parameters);

				if (dsDashboard.Tables[0].Rows.Count > 0)
				{
                    txtParkingTotal.Text = dsDashboard.Tables[0].Rows[0]["TotParking"].ToString();
                    txtParkingOntime.Text = dsDashboard.Tables[0].Rows[0]["ParkingOntime"].ToString();
                    txtParkingDelay.Text = dsDashboard.Tables[0].Rows[0]["ParkingDelay"].ToString();
					txtPostingTotal.Text = dsDashboard.Tables[0].Rows[0]["TotPosting"].ToString();
					txtPostingOntime.Text = dsDashboard.Tables[0].Rows[0]["PostingOntime"].ToString();
					txtPostingDelay.Text = dsDashboard.Tables[0].Rows[0]["PostingDelay"].ToString();
                    txtClosingTotal.Text = dsDashboard.Tables[0].Rows[0]["TotClosing"].ToString();
					txtClosingOntime.Text = dsDashboard.Tables[0].Rows[0]["ClosingOntime"].ToString();
					txtClosingDelay.Text = dsDashboard.Tables[0].Rows[0]["ClosingDelay"].ToString();
					txtAllocationTotal.Text = dsDashboard.Tables[0].Rows[0]["TotAllocation"].ToString();
				}
			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

			}
		}
		protected void LoadDashboardData()
        {
            try
            {
                _dataManager = new DataManager();
                

                string user_flow = _user.GetCookie(CookieKey.RoleId.ToString());

                DataSet dsDashboard = new DataSet();

				if (user_flow == "7" || user_flow == "8")
                {
					SqlParameter[] parameters = new SqlParameter[2]
				    {
						_dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
						_dataManager.MakeInParam("@FlowId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.RoleId.ToString()))
				    };
					dsDashboard = _dataManager.GetDataSet("SP_SYS_DASHBOARD_ACCOUNTS", parameters);
				}
                else
                {
					SqlParameter[] parameters = new SqlParameter[1]
				    {
                        _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString()))
				    };
					dsDashboard = _dataManager.GetDataSet("SP_SYS_DASHBOARD", parameters);
				}
				

                if (dsDashboard.Tables[0].Rows.Count > 0)
                {
                    lbtnOwnDrafted.Text = dsDashboard.Tables[0].Rows[0]["Drafted"].ToString();
                    lbtnOwnSubmitted.Text = dsDashboard.Tables[0].Rows[0]["Submitted"].ToString();
                    lbtnOwnApproved.Text = dsDashboard.Tables[0].Rows[0]["Approved"].ToString();
                   // lbtnOwnDeleted.Text = dsDashboard.Tables[0].Rows[0]["Deleted"].ToString();
                    lbtnOwnRejected.Text = dsDashboard.Tables[0].Rows[0]["Rejected"].ToString();
                }

                if (dsDashboard.Tables[1].Rows.Count > 0)
                {
                    spanTotalDrafted.InnerText = dsDashboard.Tables[1].Rows[0]["Drafted"].ToString();
                    spanTotalSubmitted.InnerText = dsDashboard.Tables[1].Rows[0]["Submitted"].ToString();
                    spanTotalApproved.InnerText = dsDashboard.Tables[1].Rows[0]["Approved"].ToString();
                   // spanTotalDeleted.InnerText = dsDashboard.Tables[1].Rows[0]["Deleted"].ToString();
                    spanTotalRejected.InnerText = dsDashboard.Tables[1].Rows[0]["Rejected"].ToString();
                }

                if(dsDashboard.Tables[2].Rows.Count > 0)
                {
                    lblOwnDocCount.Text = dsDashboard.Tables[2].Rows.Count.ToString();
                    if (_user.GetCookie(CookieKey.RoleId.ToString())!= "2")
                    {
                        lblOwnDocCount.Text = "0";

						FillList.PopulateGridView(new DataTable(), gvOwnDocuments);
                    }
                    else
                    {
						FillList.PopulateGridView(dsDashboard.Tables[2], gvOwnDocuments);
					}
                }
                else
                {
                    gvOwnDocuments.DataSource = new DataTable();
                    gvOwnDocuments.DataBind();
                }

                if (dsDashboard.Tables[3].Rows.Count > 0)
                {
                    lblWorkFlowDocCount.Text = dsDashboard.Tables[3].Rows.Count.ToString();
                    FillList.PopulateGridView(dsDashboard.Tables[3], gvWorkflowDocument);
                }
                else
                {
                    gvWorkflowDocument.DataSource = new DataTable();
                    gvWorkflowDocument.DataBind();
                }
            }
            catch(Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

            }
        }

        protected void lbtnOwnDrafted_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=0";
            Response.Redirect(sUrl);
        }

        protected void lbtnOwnSubmitted_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=-10";
            Response.Redirect(sUrl);
        }

        protected void lbtnOwnApproved_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=12";
            Response.Redirect(sUrl);
        }

        protected void lbtnOwnRejected_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=11";
            Response.Redirect(sUrl);
        }

        //protected void lbtnOwnDeleted_Click(object sender, EventArgs e)
        //{
        //    String sUrl = "~/UI/OwnDocument.aspx?Status=13";
        //    Response.Redirect(sUrl);
        //}

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
    }
}