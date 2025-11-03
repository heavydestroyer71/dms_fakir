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
    public partial class WorkFlow : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
				string flowId = Request.QueryString["FlowId"];
				//string categoryId = Request.QueryString["CategoryId"];
				LoadDashboardData(flowId);
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
			}
			catch (Exception ex)
			{
				DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
				ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

			}
		}
		protected void LoadDashboardData(string FlowId)
        {
            try
            {
                _dataManager = new DataManager();


                string user_flow = FlowId;

                DataSet dsDashboard = new DataSet();

				SqlParameter[] parameters = new SqlParameter[1]
					{
						_dataManager.MakeInParam("@FlowId", SqlDbType.NVarChar, 500, FlowId)
					};
				dsDashboard = _dataManager.GetDataSet("GETALL_WORKFLOW_DOCUMENT_BY_FLOW", parameters);
				if (dsDashboard.Tables[0].Rows.Count > 0)
				{
					lblWorkFlowDocCount.Text = dsDashboard.Tables[0].Rows.Count.ToString();
					FillList.PopulateGridView(dsDashboard.Tables[0], gvWorkflowDocument);
				}
			}
            catch(Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

            }
        }
		protected void gvWorkflowDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName.Equals("ViewHistory"))
			{
				GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
				String sDocId = ((Label)row.FindControl("lblTranID")).Text;

				try
				{
					DataManager _dataManager = new DataManager();
					SqlParameter[] parameters = new SqlParameter[1]
					{
						_dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, sDocId)
					};

					DataTable dtHistory = _dataManager.GetDataTable("SP_DOCUMENT_MOVEMENT_HISTORY", parameters);
					FillList.PopulateGridView(dtHistory, gvHistory);
				}
				catch
				{

				}

				modalExtenderHistory.Show();
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