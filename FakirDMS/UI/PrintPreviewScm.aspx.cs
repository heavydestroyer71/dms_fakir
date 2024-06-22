using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreLibrary;


namespace FakirDMS.UI
{
    public partial class PrintPreviewScm : System.Web.UI.Page
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
            #endregion

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["DocumentID"]))
                {
                    hfDocumentId.Value = Request.QueryString["DocumentID"];
                    LoadDocumentInformation();
                    BindGridViewDocumentDetails();
                }
            }
        }

        protected void LoadDocumentInformation()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                  _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                DataSet ds = _dataManager.GetDataSet("SP_SELECT_DOCUMENT", parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hfCategoryId.Value = ds.Tables[0].Rows[0]["CategoryId"].ToString();
                    divPageHeader.InnerText = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                    txtCompany.Text = ds.Tables[0].Rows[0]["ComapnyName"].ToString();
                    txtItemType.Text = ds.Tables[0].Rows[0]["ExpenseType"].ToString();
                    txtEntryDate.Text = ds.Tables[0].Rows[0]["EntryDate"].ToString();
                    txtBillRefNo.Text = ds.Tables[0].Rows[0]["BillREfNo"].ToString();
                    txtRefTracking.Text = ds.Tables[0].Rows[0]["RefDocumentNo"].ToString();
                    txtPoNo.Text = ds.Tables[0].Rows[0]["PoNo"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                    txtBillNo.Text = ds.Tables[0].Rows[0]["PartyBillNo"].ToString();
                    txtBillAmount.Text = ds.Tables[0].Rows[0]["BillAmount"].ToString();
                    txtAuditAmount.Text = ds.Tables[0].Rows[0]["DiscountAmount"].ToString();
                    txtFinalAmount.Text = ds.Tables[0].Rows[0]["FinalAmount"].ToString();


                    txtVoucherNo.Text = ds.Tables[0].Rows[0]["VoucherNo"].ToString();
                    txtVoucherDate.Text = ds.Tables[0].Rows[0]["VoucherDate"].ToString();
                    txtPaymentAmount.Text = ds.Tables[0].Rows[0]["PaymentAmount"].ToString();
                    cbBillClosed.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsClosed"].ToString());


                    txtBillDate.Text = ds.Tables[0].Rows[0]["PartyBillDate"].ToString();
                    txtVatAmount.Text = ds.Tables[0].Rows[0]["VatAmount"].ToString();
                    txtVATChallanNo.Text = ds.Tables[0].Rows[0]["VATChallanNo"].ToString();
                    txtVATDate.Text = ds.Tables[0].Rows[0]["VATChallanDate"].ToString();
                }

                if (ds.Tables[1].Rows.Count > 0)    //DocumentComments
                {
                    gvComment.DataSource = ds.Tables[1];
                    gvComment.DataBind();
                }
                else
                {
                    gvComment.DataSource = null;
                }

                if (ds.Tables[2].Rows.Count > 0)    //Document
                {
                    gvAttachment.DataSource = ds.Tables[2];
                    gvAttachment.DataBind();
                }
                else
                {
                    gvAttachment.DataSource = null;
                    gvAttachment.DataBind();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void InitialDataTable()
        {
            DataTable dtDetails = new DataTable();
            DataColumn dc = new DataColumn("SL");
            dc.DataType = typeof(Int32);
            dtDetails.Columns.Add(dc);
            dtDetails.Columns["SL"].DefaultValue = 1;

            dtDetails.Columns.Add("RowId", typeof(Int32));
            dtDetails.Columns.Add("Type", typeof(String));
            dtDetails.Columns.Add("Id", typeof(Int32));
            dtDetails.Columns.Add("RefNo", typeof(String));
            dtDetails.Columns.Add("Date", typeof(String));
            dtDetails.Columns.Add("SupplierName", typeof(String));
            dtDetails.Rows.Add(dtDetails.NewRow());

            ViewState["DetailsData"] = dtDetails;
        }

        protected void BindGridViewDocumentDetails()
        {
            try
            {
                InitialDataTable();

                String sQuery ="SELECT ROW_NUMBER() OVER(PARTITION BY [Type] ORDER BY RowId ASC) AS SL,RowId,DocumentId,Type,Id,RefNo,Date,ChallanNo,ChallanDate,SupplierName,EntryDate FROM DocumentDetails WHERE DocumentId=" + hfDocumentId.Value;

                _dataManager = new DataManager();
                DataTable dtDetails = _dataManager.GetDataTable(sQuery);

                //Bind Requisition GridView
                DataTable dtPO = FilterDataTableByType(dtDetails, "PO");
                if (dtPO.Rows.Count > 0)
                {
                    gvPurchaseOrder.DataSource = dtPO;
                    gvPurchaseOrder.DataBind();
                }
                else
                {
                    gvPurchaseOrder.DataSource = (DataTable)ViewState["DetailsData"];
                    gvPurchaseOrder.DataBind();
                }


                 sQuery = "SELECT ROW_NUMBER() OVER(ORDER BY TransId ASC) AS SL,TransId as RowId,DocumentId,'MR' as Type,TransId as Id,MrrNo as RefNo,MrrDate as Date,ChallanNo,MrrDate as ChallanDate,'' as SupplierName,EntryDate FROM DocumentScmMrr WHERE DocumentId=" + hfDocumentId.Value;

                _dataManager = new DataManager();
                 dtDetails = _dataManager.GetDataTable(sQuery);

              
                //Bind MRR GridView
                DataTable dtMRR = FilterDataTableByType(dtDetails, "MR");
                if (dtMRR.Rows.Count > 0)
                {
                    gvMRR.DataSource = dtMRR;
                    gvMRR.DataBind();
                }
                else
                {
                    gvMRR.DataSource = (DataTable)ViewState["DetailsData"];
                    gvMRR.DataBind();
                }

                //Bind Requisition GridView
                //DataTable dtRequisition = FilterDataTableByType(dtDetails, "CL");
                //if (dtRequisition.Rows.Count > 0)
                //{
                //    gvChallan.DataSource = dtRequisition;
                //    gvChallan.DataBind();
                //}
                //else
                //{
                //    gvChallan.DataSource = (DataTable)ViewState["DetailsData"];
                //    gvChallan.DataBind();
                //}
            }
            catch (Exception ex)
            {
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected static DataTable FilterDataTableByType(DataTable originalTable, string targetType)
        {
            var query = from DataRow row in originalTable.Rows
                        where row.Field<string>("Type") == targetType
                        select row;

            DataTable filteredDataTable = originalTable.Clone();
            foreach (DataRow row in query)
            {
                filteredDataTable.ImportRow(row);
            }

            return filteredDataTable;
        }


        #region Others
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion

        protected void gBtnPreview_Click(object sender, ImageClickEventArgs e)
        {
            SectionVisibility activity = new SectionVisibility(hfCategoryId.Value, hfDocumentId.Value);
            if (activity.IsEnableDownload)
            {
                ImageButton imDownload = (ImageButton)sender;
                GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);
                int transID = Convert.ToInt32(row.Cells[0].Text);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('Viewer.aspx?id=" + transID.ToString() + "');", true);
            }
            else
            {
                DisplayMessage("Sorry! You don't have permission to download attachment.");
            }
        }
    }
}