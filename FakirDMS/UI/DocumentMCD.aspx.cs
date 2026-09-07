using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreLibrary;


namespace FakirDMS.UI
{
    public partial class DocumentMCD : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        String _errorMessage = String.Empty;
        DataManager _dataManager = new DataManager();

              

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Check User Login Status

            if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) ||
                _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }

            if (Request.QueryString["mode"] == null)
            {
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
            }

            #endregion

            if (!Page.IsPostBack)
            {
                this.Form.DefaultButton = btnDefault.UniqueID;
                txtEntryDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                divButtonApprove.Visible = false;

                if (!String.IsNullOrEmpty(Request.QueryString["categoryId"]))
                {
                    hfCategoryId.Value = Request.QueryString["categoryId"];
                }

              // UpdateItemCategory("0");

                InitialDataTable();
                LoadDropDownListCompany();
                LoadDropDownListExpense();
                LoadDropDownListAttachmentType();
                BindGridViewOwnDocument();

                if (!String.IsNullOrEmpty(Request.QueryString["DocumentID"]))
                {
                    hfDocumentId.Value = Request.QueryString["DocumentID"];
                    LoadDocumentForUpdate();
                    TrackingNoGeneratedMode();
                    LoadDropDownListRevertTo();
                }

                BindGridViewDocumentDetails();
                ControlVisibility();
            }
        }


        #region Set active/inactive status of Controls

        protected void ControlVisibility()
        {
            SectionVisibility activity = new SectionVisibility(hfCategoryId.Value, hfDocumentId.Value);

            txtBillNo.Enabled = activity.IsEnableBill;
            txtBillDate.Enabled = activity.IsEnableBill;
            txtBillAmount.Enabled = activity.IsEnableAmount;
            txtDiscountAmount.Enabled = activity.IsEnableDiscount;
            txtVatAmount.Enabled = activity.IsEnableBill;
            txtVATChallanNo.Enabled = activity.IsEnableBill;
            txtVATDate.Enabled = activity.IsEnableBill;

            divUploader.Visible = activity.IsVisibleUploader;
            divPageHeader.InnerText = activity.CategoryName;

            if (!activity.IsEnablePO)
            {
                gvPurchaseOrder.Columns[gvPurchaseOrder.Columns.Count - 1].Visible = false;
                gvPurchaseOrder.DataBind();
            }

            if (!activity.IsEnablePI)
            {
                gvProformaInvoice.Columns[gvProformaInvoice.Columns.Count - 1].Visible = false;
                gvProformaInvoice.DataBind();
            }

            if (!activity.IsEnableLC)
            {
                gvLC.Columns[gvLC.Columns.Count - 1].Visible = false;
                gvLC.DataBind();
            }

            if (!activity.IsEnableMR)
            {
                gvMRR.Columns[gvMRR.Columns.Count - 1].Visible = false;
                gvMRR.DataBind();
            }

            if (!activity.IsEnableAccounts)
            {
                divAccountInfo.Visible = false;
            }
            else
            {
                divAccountInfo.Visible = true;
            }

            if (!activity.IsEnableBill)
            {
                divBillInfo.Visible = false;
            }
            else
            {
                divBillInfo.Visible = true;
            }

            //if (!activity.IsEnableCL)
            //{
            //    gvChallan.Columns[gvChallan.Columns.Count - 1].Visible = false;
            //    gvChallan.DataBind();
            //}

            if (activity.IsInitialPath)
            {
                divButtonPrepare.Visible = true;
                divButtonApprove.Visible = false;
            }
            else
            {
                divButtonPrepare.Visible = false;
                divButtonApprove.Visible = true;
            }

            if (activity.IsApprover == true && activity.IsCloser==false)
            {
                btnWorkflowForward.Text = "Approved";
                hfIsApprover.Value = "1";
                btnWorkflowDecline.Visible = true;
            }
            else if (activity.IsApprover == false && activity.IsCloser == true)
            {
                btnWorkflowForward.Text = "Close";
                hfIsApprover.Value = "1";
                ddlRevertTo.Visible = false;
                btnWorkflowReject.Visible = false;
                btnWorkflowDecline.Visible = false;
            }
            else
            {
                hfIsApprover.Value = "0";
                btnWorkflowDecline.Visible = false;
            }
        }

        protected void TrackingNoGeneratedMode()
        {
            ddlCompany.Enabled = false;
            ddlExpenseType.Enabled = false;
            txtRemarks.Enabled = false;
            //btnLoadRefNo.Enabled = false;
            btnCreate.Visible = false;
        }

        #endregion


        #region Control Bind With Data from Database

        protected void LoadDropDownListCompany()
        {
            String sUserId = _user.GetCookie(CookieKey.UserId.ToString());
            String sCategoryId = hfCategoryId.Value;

            DataTable dtComapy = PopulateLists.GetCompaniesByUser_Category(sUserId, sCategoryId);
            FillList.PopulateDropDownList(dtComapy, ddlCompany, "Select Company");
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
        protected void LoadDropDownListExpense()
        {
            DataTable dtExpense = PopulateLists.GetExpenseTypes();
            FillList.PopulateDropDownList(dtExpense, ddlExpenseType, "Select Expense Type");
        }

        protected void LoadDropDownListAttachmentType()
        {
            DataTable dtDocumentType = PopulateLists.GetAttachmentTypes();
            FillList.PopulateDropDownList(dtDocumentType, ddlDocumentType, "Select Document Type");
        }

        private DataSet LoadDocumentInformation()
        {
            DataSet _dsResult = new DataSet();
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                    _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                _dsResult = _dataManager.GetDataSet("SP_SELECT_DOCUMENT", parameters);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."),
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

            return _dsResult;
        }

        protected void LoadDocumentForUpdate()
        {
            try
            {

                DataSet ds = LoadDocumentInformation();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(ds.Tables[0].Rows[0]["CompanyId"].ToString()));
                    ddlExpenseType.SelectedIndex = ddlExpenseType.Items.IndexOf(ddlExpenseType.Items.FindByValue(ds.Tables[0].Rows[0]["ExpenseTypeID"].ToString()));
                    txtEntryDate.Text = ds.Tables[0].Rows[0]["EntryDate"].ToString();

                    txtBillRefNo.Text = ds.Tables[0].Rows[0]["BillREfNo"].ToString();
                    hfRefDocumentID.Value = ds.Tables[0].Rows[0]["RefDocumentId"].ToString();
                    //txtRefTracking.Text = ds.Tables[0].Rows[0]["RefDocumentNo"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                    txtBillNo.Text = ds.Tables[0].Rows[0]["PartyBillNo"].ToString();
                    txtBillAmount.Text = ds.Tables[0].Rows[0]["BillAmount"].ToString();
                    txtDiscountAmount.Text = ds.Tables[0].Rows[0]["DiscountAmount"].ToString();
                    txtFinalAmount.Text = ds.Tables[0].Rows[0]["FinalAmount"].ToString();

                    txtBillDate.Text = ds.Tables[0].Rows[0]["PartyBillDate"].ToString();
                    txtVatAmount.Text = ds.Tables[0].Rows[0]["VatAmount"].ToString();
                    txtVATChallanNo.Text = ds.Tables[0].Rows[0]["VATChallanNo"].ToString();
                    txtVATDate.Text = ds.Tables[0].Rows[0]["VATChallanDate"].ToString();

                    txtVoucherNo.Text = ds.Tables[0].Rows[0]["VoucherNo"].ToString();
                    txtVoucherDate.Text = ds.Tables[0].Rows[0]["VoucherDate"].ToString();
                    txtPaymentAmount.Text = ds.Tables[0].Rows[0]["PaymentAmount"].ToString();
                }

                FillList.PopulateGridView(ds.Tables[1], gvComment); //DocumentComments
                FillList.PopulateGridView(ds.Tables[2], gvAttachment); //Document
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."),
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion


        #region Attachment GridView Related

        protected void gBtnPreview_Click(object sender, EventArgs e)
        {
            LinkButton imDownload = (LinkButton)sender;
            GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);
            int transID = Convert.ToInt32(row.Cells[0].Text);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenWindow", "window.open('Viewer.aspx?id=" + transID.ToString() + "');", true);
        }

        protected void gBtnDownload_Click(object sender, EventArgs e)
        {
            SectionVisibility activity = new SectionVisibility(hfCategoryId.Value, hfDocumentId.Value);
            if (activity.IsEnableDownload)
            {
                string sPathToSaveFileTo = Server.MapPath("~/" + ConfigurationManager.AppSettings["DraftFolder"].ToString() +"/");
                LinkButton imDownload = (LinkButton)sender;
                GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);

                String sFileName = row.Cells[1].Text;
                String sFileTitle = row.Cells[4].Text;
                String sContentType = row.Cells[2].Text;

                FileInfo file = new FileInfo(sPathToSaveFileTo + sFileName);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileTitle);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = sContentType;
                    Response.Flush();
                    Response.TransmitFile(sPathToSaveFileTo + sFileName);
                    Response.End();
                }
                else
                {
                    DisplayMessage("Requested file is not available for download");
                }
            }
            else
            {
                DisplayMessage("Sorry! You don't have permission to download attachment.");
            }
        }

        protected void gBtnDelete_Click(object sender, EventArgs e)
        {
            SectionVisibility activity = new SectionVisibility(hfCategoryId.Value, hfDocumentId.Value);
            if (activity.IsEnableDelete)
            {
                LinkButton imDownload = (LinkButton)sender;
                GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);
                int transID = Convert.ToInt32(row.Cells[0].Text);

                String sQuery = "SELECT EntryBy FROM dbo.DocumentData WHERE TransID =" + transID;
                _dataManager = new DataManager();

                DataTable dtUser = _dataManager.GetDataTable(sQuery);
                if (dtUser.Rows.Count > 0 &&
                    dtUser.Rows[0]["EntryBy"].ToString() == _user.GetCookie(CookieKey.UserId.ToString()))
                {
                    String sUpdateQuery = "UPDATE dbo.DocumentData SET IsActive=0 WHERE TransId=" + transID;
                    DataManager dataManager = new DataManager();

                    dataManager.ExecuteNonQuery(sUpdateQuery);
                    DisplayMessage("Attachment file has been deleted.");
                    BindGridViewDocumentData();
                }
                else
                {
                    DisplayMessage("Only document owner can delete his attachment file.");
                }
            }
            else
            {
                DisplayMessage("Sorry! You don't have permission to delete uploaded attachment.");
            }
        }

        protected void BindGridViewDocumentData()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                    _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                DataTable dtAttachemnt = _dataManager.GetDataTable("SP_SELECT_DOCUMENT_DATA", parameters);
                FillList.PopulateGridView(dtAttachemnt, gvAttachment);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."),
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion


        #region Validation Check Related Methods

        protected Boolean CheckSaveValidation()
        {
            Boolean _result = true;
            
            if (ddlCompany.SelectedValue == "0")
            {
                _errorMessage = "Please select company";
                ddlCompany.Focus();
                return false;
            }
            else if (ddlExpenseType.SelectedValue == "0")
            {
                _errorMessage = "Please select item category";
                ddlExpenseType.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(hfIsPoRequired.Value) || hfIsPoRequired.Value=="0")
            {
                _errorMessage = "Please Add any valid PO";
                return false;
            }
            else if (String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                _errorMessage = "Please click Create button to generate Tracking Number.";
                return false;
            }
            else if (txtBillNo.Enabled == true && String.IsNullOrEmpty(txtBillNo.Text))
            {
                _errorMessage = "Please enter bill number";
                txtBillNo.Focus();
                return false;
            }
            else if (txtBillAmount.Enabled == true && String.IsNullOrEmpty(txtBillAmount.Text))
            {
                _errorMessage = "Please enter bill amount";
                txtBillAmount.Focus();
                return false;
            }
            //else if (txtDiscountAmount.Enabled == true && String.IsNullOrEmpty(txtDiscountAmount.Text))
            //{
            //    _errorMessage = "Please enter discount amount";
            //    txtDiscountAmount.Focus();
            //    return false;
            //}
            else if (Convert.ToInt32(hfSubmitCount.Value)>0)
            {
                _errorMessage = "The document already submitted.";
                return false;
            }

            return _result;
        }

        protected Boolean CheckforRemarks()
        {
            Boolean _result = true;
            if (String.IsNullOrEmpty(txtRemarksBoss.Text))
            {
                _errorMessage = "Please write remarks and try again";
                txtRemarksBoss.Focus();
                _result = false;
            }

            return _result;
        }

        protected Boolean CheckValidationForRevert()
        {
            Boolean _result = true;
            if (ddlRevertTo.SelectedValue == "0")
            {
                _errorMessage = "Please select any role to reject";
                ddlCompany.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(txtRemarksBoss.Text))
            {
                _errorMessage = "Please write remarks and try again";
                txtRemarksBoss.Focus();
                _result = false;
            }

            return _result;
        }

        protected Boolean CheckUploadValidation()
        {
            Boolean _result = true;
            if (ddlDocumentType.SelectedValue == "0")
            {
                _errorMessage = "Please select document type";
                ddlDocumentType.Focus();
                return false;
            }
            else if (!FileUpload1.HasFile)
            {
                _errorMessage = "Please select any file first.";
                return false;
            }

            return _result;
        }
        #endregion


        #region Document Save and Upload Related Methods

        protected void GenerateElectronicSign(String sFileData,string sDraftPath)
        {
            GenerateWatermark generate = new GenerateWatermark();
            generate.AsDraft(sFileData, sDraftPath);
        }

        protected void ClearAllControls()
        {
            hfDocumentId.Value = String.Empty;
            ddlCompany.SelectedIndex = -1;
            ddlExpenseType.SelectedIndex = -1;
            txtRemarks.Text = String.Empty;
            txtBillNo.Text = String.Empty;
            txtBillAmount.Text = String.Empty;
            txtDiscountAmount.Text = String.Empty;
        }

        #endregion


        #region Button Click Related Methods

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfDocumentId.Value != "0")
                {
                    DisplayMessage("Please submit the document then create a new document.");
                    return;
                }
                if (ddlCompany.SelectedValue == "0")
                {
                    DisplayMessage("Please select company");
                    ddlCompany.Focus();
                    return;
                }
                else if (ddlExpenseType.SelectedValue == "0")
                {
                    DisplayMessage("Please select item category");
                    ddlExpenseType.Focus();
                    return;
                }
                else
                {
                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[19]
                    {
                    _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@RefDocumentID", SqlDbType.NVarChar, 500, hfRefDocumentID.Value),
                    _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                    _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpenseType.SelectedValue),
                    _dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                    _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarks.Text),
                    _dataManager.MakeInParam("@PartyBillNo", SqlDbType.NVarChar, 500, txtBillNo.Text),
                    _dataManager.MakeInParam("@BillAmount", SqlDbType.NVarChar, 500, txtBillAmount.Text),
                    _dataManager.MakeInParam("@DiscountAmount", SqlDbType.NVarChar, 500, txtDiscountAmount.Text),
                    _dataManager.MakeInParam("@FinalAmount", SqlDbType.NVarChar, 500, txtFinalAmount.Text),

                    _dataManager.MakeInParam("@BillDate", SqlDbType.NVarChar, 500, txtBillDate.Text),
                    _dataManager.MakeInParam("@VatAmount", SqlDbType.NVarChar, 500, txtVatAmount.Text),
                    _dataManager.MakeInParam("@VATChallanNo", SqlDbType.NVarChar, 500, txtVATChallanNo.Text),
                    _dataManager.MakeInParam("@VATDate", SqlDbType.NVarChar, 500, txtVATDate.Text),
                    _dataManager.MakeInParam("@Status", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "INSERT") ,
                    _dataManager.MakeInParam("@ProcessId", SqlDbType.NVarChar, 500, "1"),
                     _dataManager.MakeInParam("@TotalMrrAmt", SqlDbType.NVarChar, 500, txtTotalMrrAmt.Text)
                    };
                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_DOCUMENT_INITIAL", parameters);

                    if (_dtReturn.Rows.Count > 0)
                    {
                        hfDocumentId.Value = _dtReturn.Rows[0]["DocumentID"].ToString();
                        txtBillRefNo.Text = _dtReturn.Rows[0]["BillRefNo"].ToString();

                        DisplayMessage("Document has been created successfully! Document tracking no.: " + txtBillRefNo.Text.Trim());
                        TrackingNoGeneratedMode();
                    }
                    else
                    {
                        hfDocumentId.Value = String.Empty;
                        DisplayMessage("Document create failed. Please try again");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."),
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnUploadDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUploadValidation())
                {
                    String sExtension = Path.GetExtension(FileUpload1.FileName);
                    String sContentType = FileUpload1.PostedFile.ContentType;
                    String sTitle = Path.GetFileName(FileUpload1.FileName);

                    String sDraftFolder = ConfigurationManager.AppSettings["DraftFolder"].ToString();
                    String sApproveFolder = ConfigurationManager.AppSettings["ApproveFolder"].ToString();

                    String sDraftPath = Server.MapPath("~/" + sDraftFolder + "/" );
                    String sApprovePath = Server.MapPath("~/" + sApproveFolder + "/" );

                   
                   



                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[10]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value.ToString()),
                        _dataManager.MakeInParam("@Extension", SqlDbType.NVarChar, 500, sExtension),
                        _dataManager.MakeInParam("@Title", SqlDbType.NVarChar, 500, sTitle),
                        _dataManager.MakeInParam("@TypeID", SqlDbType.NVarChar, 500, ddlDocumentType.SelectedValue.ToString()),
                        _dataManager.MakeInParam("@ContentType", SqlDbType.NVarChar, 500, sContentType),
                        _dataManager.MakeInParam("@DraftPath", SqlDbType.NVarChar, 500, sDraftPath),
                        _dataManager.MakeInParam("@ApprovePath", SqlDbType.NVarChar, 500, sApprovePath),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtFileRemarks.Text),
                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "UPLOAD")
                    };

                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_FILE_UPLOADER", parameters);
                    if (_dtReturn.Rows.Count > 0 && _dtReturn.Rows[0]["Result"].ToString().Length > 10)
                    {
                        DisplayMessage("File has been uploaded Successfully!");

                        
                        String sFilePath =Server.MapPath("~/" + sDraftFolder + "/" + _dtReturn.Rows[0]["Result"].ToString());
                        FileUpload1.SaveAs(sFilePath);

                        
                        String sApproveFilePath = Server.MapPath("~/" + sApproveFolder + "/" + _dtReturn.Rows[0]["Result"].ToString());
                        FileUpload1.SaveAs(sApproveFilePath);

                        String sFileId = _dtReturn.Rows[0]["FileId"].ToString();

                        if (Convert.ToBoolean(_dtReturn.Rows[0]["SignatureRequired"].ToString()))
                        {
                            String sDraftWatermark = Server.MapPath("~/" + sDraftFolder + "/draft.png");

                            GenerateElectronicSign(sFileId, sDraftWatermark);
                        }

                        LoadDocumentAfterUpload();
                    }
                    else
                    {
                        DisplayMessage("File upload failed. Please try again");
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnSubmitDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckSaveValidation())
                {
                    DisplayMessage(_errorMessage);
                    return;
                }

                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[22]
                {
                    _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                    _dataManager.MakeInParam("@RefDocumentID", SqlDbType.NVarChar, 500, hfRefDocumentID.Value),

                    _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                    _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpenseType.SelectedValue),
                    _dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                    _dataManager.MakeInParam("@PoID", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@PoNo", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@SupplierName", SqlDbType.NVarChar, 500, string.Empty),

                    _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarks.Text),

                    _dataManager.MakeInParam("@PartyBillNo", SqlDbType.NVarChar, 500, txtBillNo.Text),
                    _dataManager.MakeInParam("@BillAmount", SqlDbType.NVarChar, 500, txtBillAmount.Text),
                    _dataManager.MakeInParam("@DiscountAmount", SqlDbType.NVarChar, 500, txtDiscountAmount.Text),
                    _dataManager.MakeInParam("@FinalAmount", SqlDbType.NVarChar, 500, txtFinalAmount.Text),

                    _dataManager.MakeInParam("@BillDate", SqlDbType.NVarChar, 500, txtBillDate.Text),
                    _dataManager.MakeInParam("@VatAmount", SqlDbType.NVarChar, 500, txtVatAmount.Text),
                    _dataManager.MakeInParam("@VATChallanNo", SqlDbType.NVarChar, 500, txtVATChallanNo.Text),
                    _dataManager.MakeInParam("@VATDate", SqlDbType.NVarChar, 500, txtVATDate.Text),

                    _dataManager.MakeInParam("@Status", SqlDbType.NVarChar, 500, "1"),
                    _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "UPDATE"),
                    _dataManager.MakeInParam("@ProcessId", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@TotalMrrAmt", SqlDbType.NVarChar, 500, txtTotalMrrAmt.Text)
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_DOCUMENT_INITIAL", parameters);
                if (Convert.ToBoolean(_dtReturn.Rows[0]["IsSuccess"].ToString()))
                {
                    hfSubmitCount.Value = Convert.ToString(Convert.ToInt32(hfSubmitCount.Value) + 1);
                    DisplayMessage(_dtReturn.Rows[0]["Message"].ToString());

                    Response.Redirect("~/UI/OwnDocument.aspx", false);
                }
                else
                {
                    DisplayMessage("Document save failed. Please try again");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void txtBillAmount_TextChanged(object sender, EventArgs e)
        {
            Amount_Changed();
        }

        protected void txtDiscountAmount_TextChanged(object sender, EventArgs e)
        {
            Amount_Changed();
        }

        protected void txtVatAmount_TextChanged(object sender, EventArgs e)
        {
            Amount_Changed();
        }
        protected void Amount_Changed()
        {
            try
            {
                Double dBillAmount = 0;
                Double dDiscountAmount = 0;
                Double dVatAmount = 0;

                if (!String.IsNullOrEmpty(txtBillAmount.Text))
                {
                    dBillAmount = Convert.ToDouble(txtBillAmount.Text);
                }
             
              
                if (!String.IsNullOrEmpty(txtDiscountAmount.Text))
                {
                    dDiscountAmount = Convert.ToDouble(txtDiscountAmount.Text);
                }

                if (!String.IsNullOrEmpty(txtVatAmount.Text))
                {
                    dVatAmount = Convert.ToDouble(txtVatAmount.Text);
                }

                Double dFinalAmount = dBillAmount - dDiscountAmount+ dVatAmount;

                txtFinalAmount.Text = dFinalAmount.ToString();

            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void LoadDocumentAfterUpload()
        {
            DataSet dsDocument = LoadDocumentInformation();

            if (dsDocument.Tables[1].Rows.Count > 0)
            {
                gvComment.DataSource = dsDocument.Tables[1];
                gvComment.DataBind();
            }
            else
            {
                gvComment.DataSource = null;
            }

            if (dsDocument.Tables[2].Rows.Count > 0)
            {
                gvAttachment.DataSource = dsDocument.Tables[2];
                gvAttachment.DataBind();
            }
            else
            {
                gvAttachment.DataSource = null;
                gvAttachment.DataBind();
            }
        }

        #endregion


        #region Document Workflow Action Related Methos

        protected void LoadDropDownListRevertTo()
        {
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT FlowID AS ValueField,FlowName AS DisplayField FROM dbo.Sys_Flowpath    ");
                sQuery.Append("WHERE CategoryID=" + hfCategoryId.Value + " AND SerialNo < (   ");
                sQuery.Append("SELECT MAX(F.SerialNo) FROM dbo.DocumentInfo D    ");
                sQuery.Append("INNER JOIN dbo.Sys_Flowpath F ON D.CategoryID=F.CategoryID AND D.FlowId=F.FlowID     ");
                sQuery.Append("WHERE DocumentID=" + hfDocumentId.Value + ") ORDER BY SerialNo DESC   ");

                _dataManager = new DataManager();
                DataTable dtRoleList = _dataManager.GetDataTable(sQuery.ToString());
                FillList.PopulateDropDownList(dtRoleList, ddlRevertTo, "Select Role to Revert");
            }
            catch
            {

            }
        }

        protected Boolean UpdateDocumentInformation(String sAction)
        {
            Boolean isSuccess = true;
            try
            {
                string sVoucherDate =string.Empty;

                if (!String.IsNullOrEmpty(txtVoucherDate.Text))
                { sVoucherDate = txtVoucherDate.Text; }

                    _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[25]
                {
                    _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                    _dataManager.MakeInParam("@BillRefNo", SqlDbType.NVarChar, 500, txtBillRefNo.Text),
                    _dataManager.MakeInParam("@RefDocumentID", SqlDbType.NVarChar, 500, "0"),

                    _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                    _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpenseType.SelectedValue),
                    _dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                    _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text),

                    _dataManager.MakeInParam("@PartyBillNo", SqlDbType.NVarChar, 500, txtBillNo.Text),
                    _dataManager.MakeInParam("@PartyBillDate", SqlDbType.NVarChar, 500, txtBillDate.Text),
                    _dataManager.MakeInParam("@BillAmount", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@DiscountAmount", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@FinalAmount", SqlDbType.NVarChar, 500, "0"),               

                    _dataManager.MakeInParam("@VatAmount", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@VATChallanNo", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@VATDate", SqlDbType.NVarChar, 500, string.Empty),

                    _dataManager.MakeInParam("@ProposedAmount", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@SupplierName", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@ExpiredDate", SqlDbType.NVarChar, 500, string.Empty),

                    _dataManager.MakeInParam("@VoucherNo", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@VoucherDate", SqlDbType.NVarChar, 500, string.Empty),
                    _dataManager.MakeInParam("@PaymentAmount", SqlDbType.NVarChar, 500, "0"),
                    _dataManager.MakeInParam("@Status", SqlDbType.NVarChar, 500, "1"),
                    _dataManager.MakeInParam("@IsClosed", SqlDbType.NVarChar, 500, cbBillClosed.Checked),
                 
                    _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sAction)
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_DOCUMENT_FLOW", parameters);
                if (_dtReturn.Rows.Count > 0)
                {
                    isSuccess = Convert.ToBoolean(_dtReturn.Rows[0]["IsSuccess"].ToString());

                    BindGridViewDocumentData();
                    DisplayMessage(_dtReturn.Rows[0]["Message"].ToString());
                }
                else
                {
                    hfDocumentId.Value = String.Empty;
                    DisplayMessage("Document save failed. Please try again");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), "UpdateDocumentInformation", ex);
            }

            return isSuccess;
        }

        protected void SendEmailToDocumentOwner()
        {
            try
            {
                String sQuery =
                    "SELECT U.Email FROM Sys_Users U  INNER JOIN dbo.DocumentInfo D ON U.UserID=D.EntryBy  WHERE D.DocumentID=" +
                    Request.QueryString["DocumentID"];

                _dataManager = new DataManager();
                DataTable dtUser = _dataManager.GetDataTable(sQuery);

                if (!String.IsNullOrEmpty(dtUser.Rows[0]["Email"].ToString()))
                {
                    EmailSender.Send("saaronno@gmail.com", "Document has been rejected");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."),
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnSaveDocument_Click(object sender, EventArgs e)
        {
            try
            {
               
                    Boolean isSuccess = UpdateDocumentInformation("DRAFT");
                   

                    if (isSuccess)
                    {

                      DisplayMessage("Data is Draft Saved Successfully.");
                    //Response.Redirect("~/UI/OwnDocument.aspx", false);
                    }
               
                    else
                    {
                         DisplayMessage("Data is not Saved. Please try again.");
                    }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        

        protected void btnWorkflowForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckSaveValidation() && CheckforRemarks())
                {
                    Boolean isSuccess = UpdateDocumentInformation("F");
                    if (hfIsApprover.Value == "1")
                    {

                        String sApproveFolder = ConfigurationManager.AppSettings["ApproveFolder"].ToString();

                        String sApprovetWatermark = Server.MapPath("~/" + sApproveFolder + "/approve.png");

                        GenerateWatermark generate = new GenerateWatermark();
                        generate.AsApprove(hfDocumentId.Value, sApprovetWatermark);
                    }

                    if (isSuccess)
                    {
                        Response.Redirect("~/UI/Dashborad.aspx", false);
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValidationForRevert())
                {
                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[5]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                        _dataManager.MakeInParam("@RoleID", SqlDbType.NVarChar, 500, ddlRevertTo.SelectedValue),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text.ToString()),
                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "R")
                    };
                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_WORKFLOW_ACTION", parameters);

                    if (_dtReturn.Rows.Count > 0)
                    {
                        DisplayMessage(_dtReturn.Rows[0]["Message"].ToString().Trim());
                        BindGridViewDocumentData();

                        SendEmailToDocumentOwner();

                        Response.Redirect("Dashborad.aspx", false);
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowDecline_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckforRemarks())
                {
                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[5]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                        _dataManager.MakeInParam("@RoleID", SqlDbType.NVarChar, 500,string.Empty),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text.ToString()),
                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "D")
                    };
                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_WORKFLOW_ACTION", parameters);

                    if (_dtReturn.Rows.Count > 0)
                    {
                        DisplayMessage(_dtReturn.Rows[0]["Message"].ToString().Trim());
                        BindGridViewDocumentData();

                        Response.Redirect("~/UI/Dashborad.aspx", false);
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowBackToList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/Dashborad.aspx", false);
        }

        #endregion


        #region Reference Document Related

        protected void BindGridViewOwnDocument()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[3]
                {
                    _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtSearchRefNo.Text),
                    _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "TRACKING")
                };
                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_LIST_TRACKING", parameters);
                FillList.PopulateGridView(dtDocuments, gvDraftDocuments);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."),
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void gvDraftDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            modalExtenderRef.Show();
            gvDraftDocuments.PageIndex = e.NewPageIndex;
            BindGridViewOwnDocument();
            modalExtenderRef.Show();
        }

        protected void btnSearchRefNo_Click(object sender, EventArgs e)
        {
            modalExtenderRef.Show();
            if (String.IsNullOrEmpty(txtSearchRefNo.Text))
            {
                txtSearchRefNo.Focus();
                DisplayMessage("Please enter document reference no.");
            }
            else
            {
                BindGridViewOwnDocument();
            }
        }

        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            modalExtenderRef.Show();
            txtSearchRefNo.Text = String.Empty;
            BindGridViewOwnDocument();
            modalExtenderRef.Show();
        }

        protected void btnSelectAsset_Click(object sender, EventArgs e)
        {
            string sRefNumber = ((System.Web.UI.WebControls.Button)sender).CommandArgument;
            string sDocumnetId = ((System.Web.UI.WebControls.Button)sender).CommandName;
            //txtRefTracking.Text = sRefNumber;
            hfRefDocumentID.Value = sDocumnetId;
        }

        #endregion


        #region Bind Document Details GridView

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
            dtDetails.Columns.Add("ChallanNo", typeof(String));
            dtDetails.Columns.Add("SupplierName", typeof(String));
            dtDetails.Columns.Add("CategoryName", typeof(String));
            dtDetails.Columns.Add("ReportPath", typeof(String));
            dtDetails.Columns.Add("MrrAmt", typeof(String));
            dtDetails.Columns.Add("TotalMrrAmt", typeof(String));

            dtDetails.Rows.Add(dtDetails.NewRow());

            ViewState["DetailsData"] = dtDetails;
        }

        protected void BindGridViewDocumentDetails()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[14]
                {     _dataManager.MakeInParam("@RowId", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@DocId", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                      _dataManager.MakeInParam("@Type", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@RefId", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@Date", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@ChallanNo", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@ChallanDate", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@SupplierName", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@MrrAmt", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@CategoryName", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@ReportPath", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };
                DataTable dtDetails = _dataManager.GetDataTable("SP_SYS_DOCUMENT_DETAILS", parameters);

                hfIsPoRequired.Value = "0";
                //Bind Requisition GridView
                DataTable dtPO = FilterDataTableByType(dtDetails, "PO");
                if (dtPO.Rows.Count > 0)
                {
                    hfIsPoRequired.Value = "1";
                    gvPurchaseOrder.DataSource = dtPO;
                    gvPurchaseOrder.DataBind();
                }
                else
                {
                    gvPurchaseOrder.DataSource = (DataTable)ViewState["DetailsData"];
                    gvPurchaseOrder.DataBind();
                }

                //Bind PI GridView
                DataTable dtPI = FilterDataTableByType(dtDetails, "PI");
                if (dtPI.Rows.Count > 0)
                {
                    gvProformaInvoice.DataSource = dtPI;
                    gvProformaInvoice.DataBind();
                }
                else
                {
                    gvProformaInvoice.DataSource = (DataTable)ViewState["DetailsData"];
                    gvProformaInvoice.DataBind();
                }

                //Bind LC GridView
                DataTable dtLC = FilterDataTableByType(dtDetails, "LC");
                if (dtLC.Rows.Count > 0)
                {
                    gvLC.DataSource = dtLC;
                    gvLC.DataBind();
                }
                else
                {
                    gvLC.DataSource = (DataTable)ViewState["DetailsData"];
                    gvLC.DataBind();
                }


                //Bind MRR GridView
                txtTotalMrrAmt.Text = "0";

                DataTable dtMRR = FilterDataTableByType(dtDetails, "MR");
                if (dtMRR.Rows.Count > 0)
                {
                    txtTotalMrrAmt.Text = dtMRR.Rows[0]["TotalMrrAmt"].ToString();
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
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
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

        #endregion


        #region Delete Details Data From GridView

        protected void glbPODelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            String sRowId = row.Cells[1].Text.ToString();

            DeleteDocumentDetails(sRowId);
            BindGridViewTypeWise("PO");
        }

        protected void glbPIDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            String sRowId = row.Cells[1].Text.ToString();

            DeleteDocumentDetails(sRowId);
            BindGridViewTypeWise("PI");
        }

        protected void glbLCDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            String sRowId = row.Cells[1].Text.ToString();

            DeleteDocumentDetails(sRowId);
            BindGridViewTypeWise("LC");
        }

        protected void glbMrrDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            String sRowId = row.Cells[1].Text.ToString();

            DeleteDocumentDetails(sRowId);
            BindGridViewTypeWise("MR");
        }

        protected void glbChallanDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            String sRowId = row.Cells[1].Text.ToString();

            DeleteDocumentDetails(sRowId);
            BindGridViewTypeWise("CL");
        }


        protected void DeleteDocumentDetails(String sRowId)
        {
            try
            {
                String sQuery = "DELETE FROM DocumentDetails WHERE RowId=" + sRowId + "";

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion


        #region Document Details Popup Related Mathods

        #region Call Popup Window From Details GridView Header

        protected  void glbShowPoPopup_Click(object sender, EventArgs e)
        {

            cbIsCategory.Checked = true;

            if (!String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                ClearControlDetailsPopup();
                phfType.Value = "PO";
                PanelHeaderName.InnerText = "Purchase Order";
                btnAddToTracking.Text = "Add Purchase Order";
            }
            else
            {
                DisplayMessage("Please Create a Tracking Number First, Then Try Again.");
            }
        }

        protected async void glbShowPiPopup_Click(object sender, EventArgs e)
        {
            cbIsCategory.Checked = true;

            if (!String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                ClearControlDetailsPopup();

                phfType.Value = "PI";
                PanelHeaderName.InnerText = "Proforma Invoice";
                btnAddToTracking.Text = "Add Proforma Invoice";

                String sFirstPoNo = gvPurchaseOrder.Rows[0].Cells[2].Text.Replace("&nbsp;", "");
                if (!String.IsNullOrEmpty(sFirstPoNo))
                {
                    divTextBoxDetails.Visible = false;
                    String sAddedReqId = GetSelectedIdByGridView(gvPurchaseOrder).Replace("&nbsp;", "0");
                    DataTable dataTable = await ApiClient.GetProformaInvoiceByPoId(sAddedReqId);
                    BindGridViewPI_Details(dataTable);
                }
            }
            else
            {
                DisplayMessage("Please Create a Tracking Number First, Then Try Again.");
            }
        }

        protected async void glbShowLcPopup_Click(object sender, EventArgs e)
        {
            cbIsCategory.Checked = true;

            if (!String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                ClearControlDetailsPopup();

                phfType.Value = "LC";
                PanelHeaderName.InnerText = "BTB Letter of Credit";
                btnAddToTracking.Text = "Add LC";

                String sFirstPiNo = gvProformaInvoice.Rows[0].Cells[2].Text.Replace("&nbsp;", "");
                if (!String.IsNullOrEmpty(sFirstPiNo))
                {
                    divTextBoxDetails.Visible = false;
                    String sAddedPiId = GetSelectedIdByGridView(gvProformaInvoice).Replace("&nbsp;", "0");
                    DataTable dataTable = await ApiClient.GetLetterOfCreditByPiId(sAddedPiId);
                    BindGridViewLC_Details(dataTable);
                }
            }
            else
            {
                DisplayMessage("Please Create a Tracking Number First, Then Try Again.");
            }
        }

        protected async void glbShowMrrPopup_Click(object sender, EventArgs e)
        {
            cbIsCategory.Checked = true;

            if (!String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                ClearControlDetailsPopup();

                phfType.Value = "MR";
                PanelHeaderName.InnerText = "Material Receipt";
                btnAddToTracking.Text = "Add MRR";

                String sFirstPoNo = gvPurchaseOrder.Rows[0].Cells[2].Text.Replace("&nbsp;", "");
                if (!String.IsNullOrEmpty(sFirstPoNo))
                {
                    divTextBoxDetails.Visible = false;
                    String sPoId = GetSelectedIdByGridView(gvPurchaseOrder).Replace("&nbsp;", "0");
                    String sPiId = GetSelectedIdByGridView(gvProformaInvoice).Replace("&nbsp;", "0");
                    String sCatId = ddlExpenseType.SelectedValue;

					DataTable dataTable = await ApiClient.GetMaterialReceiveByPoId(sPoId, sPiId, sCatId);
                    BindGridViewMR_Details(dataTable);
                }
            }
            else
            {
                DisplayMessage("Please Create a Tracking Number First, Then Try Again.");
            }
        }

        protected async void glbShowChallanPopup_Click(object sender, EventArgs e)
        {
            cbIsCategory.Checked = true;

            if (!String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                ClearControlDetailsPopup();

                phfType.Value = "CL";
                PanelHeaderName.InnerText = "Challan";
                btnAddToTracking.Text = "Add Challan";
            }
            else
            {
                DisplayMessage("Please Create a Tracking Number First, Then Try Again.");
            }
        }
       
        #endregion

        #region Bind Details Popup GridView

        protected void BindGridViewRE_Details(DataTable dtResult)
        {
            DataTable NewDataTable = new DataTable();
            NewDataTable.Columns.Add("Id", typeof(string));
            NewDataTable.Columns.Add("RefNo", typeof(string));
            NewDataTable.Columns.Add("Date", typeof(string));
            NewDataTable.Columns.Add("Supplier", typeof(string));
            NewDataTable.Columns.Add("Category", typeof(string));
            NewDataTable.Columns.Add("MrrAmt", typeof(string));

            foreach (DataRow row in dtResult.Rows)
            {
                DataRow newRow = NewDataTable.NewRow();
                newRow["Id"] = row["ID"].ToString().Replace("&nbsp;", "");
                newRow["RefNo"] = row["REQU_NO"].ToString().Replace("&nbsp;", "");
                newRow["Date"] = row["REQUISITION_DATE"].ToString().Replace("&nbsp;", "");
                newRow["Category"] = row["ITEM_CATEGORY_NAME"].ToString().Replace("&nbsp;", "");
                newRow["MrrAmt"] = row["MrrAmt"].ToString().Replace("&nbsp;", "");
                NewDataTable.Rows.Add(newRow);
            }
            gvDetails.Columns[4].Visible = false;
            gvDetails.Columns[5].Visible = false;
            gvDetails.Columns[6].Visible = false;
            gvDetails.Columns[7].Visible = false;

            gvDetails.DataSource = NewDataTable;
            gvDetails.DataBind();


        }

        protected void BindGridViewPO_Details(DataTable dtResult)
        {
            DataTable NewDataTable = new DataTable();
            NewDataTable.Columns.Add("Id", typeof(string));
            NewDataTable.Columns.Add("RefNo", typeof(string));
            NewDataTable.Columns.Add("Date", typeof(string));
            NewDataTable.Columns.Add("Supplier", typeof(string));
            NewDataTable.Columns.Add("Category", typeof(string));
            NewDataTable.Columns.Add("ReportPath", typeof(string));

            foreach (DataRow row in dtResult.Rows)
            {
                DataRow newRow = NewDataTable.NewRow();
                newRow["Id"] = row["ID"].ToString().Replace("&nbsp;", "");
                newRow["RefNo"] = row["WO_NUMBER"].ToString().Replace("&nbsp;", "");
                newRow["Date"] = row["WO_DATE"].ToString().Replace("&nbsp;", "");
                newRow["Supplier"] = row["SUPPLIER_NAME"].ToString().Replace("&nbsp;", "");
                newRow["ReportPath"] = "~/UI/Reports/html_po.aspx?id=" + row["ID"].ToString().Replace("&nbsp;", "");
                NewDataTable.Rows.Add(newRow);
            }

            gvDetails.Columns[4].Visible = false;
            gvDetails.Columns[5].Visible = true;
            gvDetails.Columns[6].Visible = false;
            gvDetails.Columns[7].Visible = false;

            gvDetails.DataSource = NewDataTable;
            gvDetails.DataBind();
           
        }

        private void BindGridViewPI_Details(DataTable dtResult)
        {
            DataTable NewDataTable = new DataTable();
            NewDataTable.Columns.Add("Id", typeof(string));
            NewDataTable.Columns.Add("RefNo", typeof(string));
            NewDataTable.Columns.Add("Date", typeof(string));
            NewDataTable.Columns.Add("Supplier", typeof(string));
            NewDataTable.Columns.Add("Category", typeof(string));
            NewDataTable.Columns.Add("ReportPath", typeof(string));

            foreach (DataRow row in dtResult.Rows)
            {
                DataRow newRow = NewDataTable.NewRow();
                newRow["Id"] = row["ID"].ToString().Replace("&nbsp;", "");
                newRow["RefNo"] = row["PI_NUMBER"].ToString().Replace("&nbsp;", "");
                newRow["Date"] = row["PI_DATE"].ToString().Replace("&nbsp;", "");
                newRow["Supplier"] = "";
                newRow["ReportPath"] = "~/UI/Reports/html_pi.aspx?id=" + row["ID"].ToString().Replace("&nbsp;", "");
                NewDataTable.Rows.Add(newRow);
            }
            gvDetails.Columns[4].Visible = false;
            gvDetails.Columns[5].Visible = false;
            gvDetails.Columns[6].Visible = false;
            gvDetails.Columns[7].Visible = false;

            gvDetails.DataSource = NewDataTable;
            gvDetails.DataBind();

           
        }

        private void BindGridViewLC_Details(DataTable dtResult)
        {
            DataTable NewDataTable = new DataTable();
            NewDataTable.Columns.Add("Id", typeof(string));
            NewDataTable.Columns.Add("RefNo", typeof(string));
            NewDataTable.Columns.Add("Date", typeof(string));
            NewDataTable.Columns.Add("Supplier", typeof(string));
            NewDataTable.Columns.Add("Category", typeof(string));
            NewDataTable.Columns.Add("ReportPath", typeof(string));

            foreach (DataRow row in dtResult.Rows)
            {
                DataRow newRow = NewDataTable.NewRow();
                newRow["Id"] = row["ID"].ToString().Replace("&nbsp;", "");
                newRow["RefNo"] = row["LC_NUMBER"].ToString().Replace("&nbsp;", ""); ;
                newRow["Date"] = row["LC_DATE"].ToString().Replace("&nbsp;", ""); ;
                newRow["Supplier"] = "";
                newRow["ReportPath"] = "~/UI/Reports/html_lc.aspx?id=" + row["ID"].ToString().Replace("&nbsp;", "");
                NewDataTable.Rows.Add(newRow);
            }
            gvDetails.Columns[4].Visible = false;
            gvDetails.Columns[5].Visible = false;
            gvDetails.Columns[6].Visible = false;
            gvDetails.Columns[7].Visible = false;

            gvDetails.DataSource = NewDataTable;
            gvDetails.DataBind();
            
        }

        private void BindGridViewMR_Details(DataTable dtResult)
        {
            DataTable NewDataTable = new DataTable();
            NewDataTable.Columns.Add("Id", typeof(string));
            NewDataTable.Columns.Add("RefNo", typeof(string));
            NewDataTable.Columns.Add("Date", typeof(string));
            NewDataTable.Columns.Add("ChallanNo", typeof(string));
            NewDataTable.Columns.Add("Supplier", typeof(string));
            NewDataTable.Columns.Add("Category", typeof(string));
            NewDataTable.Columns.Add("ReportPath", typeof(string));
            NewDataTable.Columns.Add("MrrAmt", typeof(string));

            foreach (DataRow row in dtResult.Rows)
            {
                DataRow newRow = NewDataTable.NewRow();
                newRow["Id"] = row["ID"].ToString().Replace("&nbsp;", ""); ;
                newRow["RefNo"] = row["RECV_NUMBER"].ToString().Replace("&nbsp;", "");
                newRow["Date"] = row["RECEIVE_DATE"].ToString().Replace("&nbsp;", "");
                newRow["ChallanNo"] = row["CHALLAN_NO"].ToString().Replace("&nbsp;", "");
                newRow["Supplier"] = "";
                newRow["ReportPath"] = "~/UI/Reports/html_mrr.aspx?id=" + row["ID"].ToString().Replace("&nbsp;", "");
                newRow["MrrAmt"] = row["CONS_AMOUNT"].ToString().Replace("&nbsp;", "");
                NewDataTable.Rows.Add(newRow);
               
            }
            gvDetails.Columns[4].Visible = true;
            gvDetails.Columns[5].Visible = false;
            gvDetails.Columns[6].Visible = false;
            gvDetails.Columns[7].Visible = true;

            gvDetails.DataSource = NewDataTable;
            gvDetails.DataBind();

          
        }

        protected void BindGridViewCL_Details(DataTable dtResult)
        {
            DataTable NewDataTable = new DataTable();
            NewDataTable.Columns.Add("Id", typeof(string));
            NewDataTable.Columns.Add("RefNo", typeof(string));
            NewDataTable.Columns.Add("Date", typeof(string));
            NewDataTable.Columns.Add("Supplier", typeof(string));
            NewDataTable.Columns.Add("Category", typeof(string));
            NewDataTable.Columns.Add("ReportPath", typeof(string));

            foreach (DataRow row in dtResult.Rows)
            {
                DataRow newRow = NewDataTable.NewRow();
                newRow["Id"] = row["ID"].ToString().Replace("&nbsp;", "");
                newRow["RefNo"] = row["REQU_NO"].ToString().Replace("&nbsp;", "");
                newRow["Date"] = row["REQUISITION_DATE"].ToString().Replace("&nbsp;", "");
                newRow["Category"] = row["ITEM_CATEGORY_NAME"].ToString().Replace("&nbsp;", "");
                newRow["ReportPath"] = "/reports/html_po.aspx?id=" + row["ID"].ToString().Replace("&nbsp;", "");
                NewDataTable.Rows.Add(newRow);
            }

            gvDetails.Columns[4].Visible = false;
            gvDetails.Columns[5].Visible = false;
            gvDetails.Columns[6].Visible = true;
            gvDetails.Columns[7].Visible = false;

            gvDetails.DataSource = NewDataTable;
            gvDetails.DataBind();
           
        }
        #endregion

        #endregion


        #region Details Popup Related
        protected async void plbSearchDetails_OnClick(object sender, EventArgs e)
        {
            string CatgId = "0";
            if (cbIsCategory.Checked)
            { CatgId = ddlExpenseType.SelectedValue; }

            CatgId = (String.IsNullOrEmpty(CatgId) ? "0" : CatgId);

            modalExtenderDetails.Show();
            if (!String.IsNullOrEmpty(ptxtSearchWith.Text))
            {
                DataTable dtResult;
                switch (phfType.Value)
                {
                    case "PO":
                        dtResult = await ApiClient.GetPurchaseOrderInfo(CatgId,ptxtSearchWith.Text);
                        //if (dtResult.Rows.Count > 0)
                        //{
                            BindGridViewPO_Details(dtResult);
                        //}
                        break;
                    case "PI":
                        dtResult = await ApiClient.GetProformaInvoiceInfo(CatgId, ptxtSearchWith.Text);
                        //if (dtResult.Rows.Count > 0)
                        //{
                            BindGridViewPI_Details(dtResult);
                        //}
                        break;
                    case "LC":
                        dtResult = await ApiClient.GetLetterOfCreditInfo(CatgId, ptxtSearchWith.Text);
                        //if (dtResult.Rows.Count > 0)
                        //{
                            BindGridViewLC_Details(dtResult);
                        //}
                        break;
                    case "MR":
                        dtResult = await ApiClient.GetMaterialReceiveInfo(CatgId, ptxtSearchWith.Text);
                        //if (dtResult.Rows.Count > 0)
                        //{
                            BindGridViewMR_Details(dtResult);
                        //}
                        break;
                    case "CL":
                        dtResult = await ApiClient.GetRequistionInfo(CatgId, ptxtSearchWith.Text);
                        //if (dtResult.Rows.Count > 0)
                        //{
                            BindGridViewCL_Details(dtResult);
                        //}
                        break;

                    default: break;
                }
            }
            else
            {
                DisplayMessage("Please enter anything to search");
            }
        }

        protected String GetSelectedIdByGridView(System.Web.UI.WebControls.GridView gridView)
        {
            String sSelectedId = String.Empty;

            foreach (GridViewRow gvRow in gridView.Rows)
            {
                sSelectedId = sSelectedId + gvRow.Cells[2].Text.ToString() + ",";
            }

            return sSelectedId.TrimEnd(',');
        }

        protected void btnAddToTracking_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvRow in gvDetails.Rows)
                {
                    if (((CheckBox)gvRow.FindControl("gvCbSelect")).Checked)
                    {
                        String sSupplierName = HttpUtility.HtmlDecode((phfType.Value.ToString() == "PO" ? gvRow.Cells[5].Text : ""));
                        String sCategoryName = HttpUtility.HtmlDecode((phfType.Value.ToString() == "RE" ? gvRow.Cells[6].Text : ""));
                        String sRefNo = HttpUtility.HtmlDecode(((HyperLink)gvRow.FindControl("gHlReportPath")).Text.ToString());
                        String sReportPath = HttpUtility.HtmlDecode(((HyperLink)gvRow.FindControl("gHlReportPath")).NavigateUrl.ToString());
                        String sChallanNo = HttpUtility.HtmlDecode((phfType.Value.ToString() == "MR" ? gvRow.Cells[4].Text : ""));
                        String sChallanDate = HttpUtility.HtmlDecode((phfType.Value.ToString() == "MR" ? gvRow.Cells[3].Text : "null"));
                        String sMrrAmt = HttpUtility.HtmlDecode((phfType.Value.ToString() == "MR" ? gvRow.Cells[7].Text : ""));
                        _dataManager = new DataManager();
                        SqlParameter[] parameters = new SqlParameter[14]
                        {
                             _dataManager.MakeInParam("@RowId", SqlDbType.NVarChar, 500, "0"),
                            _dataManager.MakeInParam("@DocId", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                            _dataManager.MakeInParam("@Type", SqlDbType.NVarChar, 500, phfType.Value.ToString()),
                            _dataManager.MakeInParam("@RefId", SqlDbType.NVarChar, 500, gvRow.Cells[1].Text),
                            _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, sRefNo),
                            _dataManager.MakeInParam("@Date", SqlDbType.NVarChar, 500, gvRow.Cells[3].Text),
                            _dataManager.MakeInParam("@ChallanNo", SqlDbType.NVarChar, 500, sChallanNo),
                            _dataManager.MakeInParam("@ChallanDate", SqlDbType.NVarChar, 500, sChallanDate),
                            _dataManager.MakeInParam("@SupplierName", SqlDbType.NVarChar, 500, sSupplierName),
                            _dataManager.MakeInParam("@MrrAmt", SqlDbType.NVarChar, 500, sMrrAmt),
                            _dataManager.MakeInParam("@CategoryName", SqlDbType.NVarChar, 500, sCategoryName),
                            _dataManager.MakeInParam("@ReportPath", SqlDbType.NVarChar, 500, sReportPath),
                            _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                            _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "SAVE")
                        };

                        DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_DOCUMENT_DETAILS", parameters);
                        if (_dtReturn.Rows.Count > 0)
                        {                               
                            modalExtenderDetails.Show();
                        }
                    }
                }
                
                modalExtenderDetails.Hide();
                BindGridViewTypeWise(phfType.Value.ToString());
            }
            catch (Exception ex)
            {

            }
        }


        //protected void glbSaveDetails_Click(object sender, EventArgs e)
        //{
        //    LinkButton button = (LinkButton)sender;
        //    GridViewRow gvRow = (GridViewRow)button.Parent.Parent;

        //    String sSupplierName = (phfType.Value.ToString() == "PO" ? gvRow.Cells[4].Text : "");
        //    String sCategoryName = (phfType.Value.ToString() == "RE" ? gvRow.Cells[5].Text : "");

        //    _dataManager = new DataManager();
        //    SqlParameter[] parameters = new SqlParameter[9]
        //    {
        //        _dataManager.MakeInParam("@DocId", SqlDbType.NVarChar, 500, hfDocumentId.Value),
        //        _dataManager.MakeInParam("@Type", SqlDbType.NVarChar, 500, phfType.Value.ToString()),
        //        _dataManager.MakeInParam("@RefId", SqlDbType.NVarChar, 500, gvRow.Cells[1].Text),
        //        _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, gvRow.Cells[2].Text),
        //        _dataManager.MakeInParam("@Date", SqlDbType.NVarChar, 500, gvRow.Cells[3].Text),
        //        _dataManager.MakeInParam("@SupplierName", SqlDbType.NVarChar, 500, sSupplierName),
        //        _dataManager.MakeInParam("@CategoryName", SqlDbType.NVarChar, 500, sCategoryName),
        //        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
        //        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "SAVE")
        //    };

        //    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_DOCUMENT_DETAILS", parameters);
        //    if (_dtReturn.Rows.Count > 0)
        //    {
        //        if (Convert.ToInt32(_dtReturn.Rows[0]["Type"]) > 0)
        //        {
        //            BindGridViewTypeWise(phfType.Value.ToString());
        //        }
        //        DisplayMessage(_dtReturn.Rows[0]["Message"].ToString());
        //        modalExtenderDetails.Show();
        //    }
        //}

        protected void BindGridViewTypeWise(String sType)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[14]
                {     _dataManager.MakeInParam("@RowId", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@DocId", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                      _dataManager.MakeInParam("@Type", SqlDbType.NVarChar, 500, phfType.Value.ToString()),
                      _dataManager.MakeInParam("@RefId", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@Date", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@ChallanNo", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@ChallanDate", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@SupplierName", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@MrrAmt", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@CategoryName", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@ReportPath", SqlDbType.NVarChar, 500, string.Empty),
                      _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, string.Empty),                      
                      _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD_TYPE_WISE")
                };
                
                DataTable dtDetails = _dataManager.GetDataTable("SP_SYS_DOCUMENT_DETAILS", parameters);
                if (dtDetails.Rows.Count == 0)
                {
                    dtDetails = (DataTable)ViewState["DetailsData"];
                }

                switch (sType)
                {
                    case "PO":
                        if (dtDetails.Rows.Count > 0)
                        { hfIsPoRequired.Value = "1"; }
                        else { hfIsPoRequired.Value = "0"; }
                        gvPurchaseOrder.DataSource = dtDetails;
                        gvPurchaseOrder.DataBind();
                        break;
                    case "PI":
                        gvProformaInvoice.DataSource = dtDetails;
                        gvProformaInvoice.DataBind();
                        break;
                    case "LC":
                        gvLC.DataSource = dtDetails;
                        gvLC.DataBind();
                        break;
                    case "MR":
                        txtTotalMrrAmt.Text = "0";

                        if (dtDetails.Rows.Count > 0)
                        {
                            txtTotalMrrAmt.Text = dtDetails.Rows[0]["TotalMrrAmt"].ToString();

                        }
                        gvMRR.DataSource = dtDetails;
                        gvMRR.DataBind();
                        break;
                    //case "CL":
                    //    gvChallan.DataSource = dtDetails;
                    //    gvChallan.DataBind();
                    //    break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void ClearControlDetailsPopup()
        {
            modalExtenderDetails.Show();
            ptxtSearchWith.Text = String.Empty;
            divTextBoxDetails.Visible = true;
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }
        #endregion


        #region Display Message
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }


        #endregion


        protected void btnLoadRefNo_Click(object sender, EventArgs e)
        {
            modalExtenderRef.Show();
        }

        protected void gvCbSelect_CheckedChanged(object sender, EventArgs e)
        {
            modalExtenderRef.Show();
            CheckBox myClickedBox = (CheckBox)sender;
            GridViewRow clickedRow = (GridViewRow)myClickedBox.NamingContainer;

            // Store the current scroll position in a session variable
            Session["ScrollPosition"] = clickedRow.RowIndex;

            if (myClickedBox.Checked)
            {
                clickedRow.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                clickedRow.BackColor = System.Drawing.Color.White;
            }
        }

    }
}