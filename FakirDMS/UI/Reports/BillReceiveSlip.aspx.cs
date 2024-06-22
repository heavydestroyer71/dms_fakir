using CoreLibrary;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Drawing;
using System.IO;
using QRCoder;

namespace FokirDMS.UI.Reports
{
    public partial class BillReceiveSlip : System.Web.UI.Page
    {

        Cookie _user = new Cookie();
        String _validationMessage = String.Empty;
        DataManager _dataManager = new DataManager();

        public string gTrackingNo = "";
        public string gCompany = "";
        public string gSupplierName = "";
        public string gPoNo = "";
        public string gInvoiceNo = "";
        public string gInvoiceAmount = "";
        public string gReceivedDate = "";
        public string gDocReceiptList = "";
        public string gReceivedBy = "";
        public string gQrCode = "";
        public string gExpenseType = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            #region Check User Login Status
            if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) || _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }
            #endregion

            if (!string.IsNullOrEmpty(Request.QueryString["DocumentID"]))
            {
                string DocumentId = "0";
                DocumentId = Request.QueryString["DocumentID"];
                LoadBillReceiptSlip(DocumentId);

            }


            if (!IsPostBack)
            {
                //string content ="Tracking#: "+ gTrackingNo + ", PO#: " + gPoNo + ", Supplier: "+ gSupplierName; // Change this to the content you want to encode
                string content = gTrackingNo; 
                int width = 150; // Adjust width as needed
                int height = 150; // Adjust height as needed

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                // Adjusting scale based on desired width and height
                int scale = Math.Min(width, height) / qrCodeData.ModuleMatrix.Count;


                Bitmap qrCodeImage = qrCode.GetGraphic(scale); // Adjust scale as needed

                // Convert Bitmap to byte array
                byte[] byteArray;
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = stream.ToArray();
                }

                // Convert byte array to Base64 string
                string base64Image = Convert.ToBase64String(byteArray);
                string imageUrl = "data:image/png;base64," + base64Image;

                gQrCode = imageUrl;
            }




        }


        protected void LoadBillReceiptSlip(string DocumentId)
        {
            try
            {

                gTrackingNo = "";
                gCompany = "";
                gSupplierName = "";
                gPoNo = "";
                gInvoiceNo = "";
                gInvoiceAmount = "";
                gReceivedDate = "";
                gDocReceiptList = "";
                gExpenseType = "";
                gReceivedBy = "";


                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                  _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, DocumentId)
                };

                DataSet ds = _dataManager.GetDataSet("SP_RPT_BILL_RECEIVE_COPY", parameters);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gTrackingNo = ds.Tables[0].Rows[0]["TrackingNo"].ToString();
                    gCompany = ds.Tables[0].Rows[0]["Company"].ToString();
                    gSupplierName = ds.Tables[0].Rows[0]["SupplierName"].ToString();
                    gPoNo = ds.Tables[0].Rows[0]["PoNo"].ToString();
                    gInvoiceNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    gInvoiceAmount = ds.Tables[0].Rows[0]["InvoiceAmount"].ToString();
                    gReceivedDate = ds.Tables[0].Rows[0]["ReceivedDate"].ToString();
                    gDocReceiptList = ds.Tables[0].Rows[0]["DocReceiptList"].ToString();
                    gExpenseType = ds.Tables[0].Rows[0]["ExpenseType"].ToString();
                    gReceivedBy = ds.Tables[0].Rows[0]["ReceivedBy"].ToString();
                   
                }

               
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\\n \\nError: " + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }


    }
}