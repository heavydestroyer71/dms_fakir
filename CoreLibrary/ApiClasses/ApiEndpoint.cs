using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class ApiEndpoint
    {
        #region Private Properties of API EndPoint Url
        // private static String sBaseUrl = "http://192.168.100.4/fakirfashion_erp/logic-api/index.php/api/dms/Dms/";

      
        //private static String sRequisitionUrl = "purchase_requisition_details/catg_id/@catg_id@/requ_no/";
        //private static String sPurchaseOrderUrl = "purchase_order_details/catg_id/@catg_id@/order_no/";
        //private static String sProformaInvoiceUrl = "Pi_info/catg_id/@catg_id@/pi_no/";
        //private static String sLetterOfCreditUrl = "Lc/catg_id/@catg_id@/lc_number/";
        //private static String sMaterialReceiveUrl = "Mrr/catg_id/@catg_id@/mrr_no/";
        //private static String sCategoryUrl = "item_category_list/catg_id/";


        //private static String sPurchaseOrderByReqUrl = "purchase_order_details/order_no/0/requ_id/";
        //private static String sProformaInvoiceByPoUrl = "Pi_info/pi_no/0/work_order/";
        //private static String sLetterOfCreditByPiUrl = "Lc/lc_number/0/pi_id/";
        #endregion

        #region Public Properties of API EndPoint Url

        public static String CategoryInfo(String CatgId)
        {
            String sCategoryUrl = "item_category_list/catg_id/";
            String sFullUrl = sBaseUrl() + sCategoryUrl + CatgId;
            return sFullUrl;
        }

        public static String RequisitionInfo(String CatgId, String ReqId)
        {
            
            String sRequisitionUrl = "purchase_requisition_details/catg_id/@catg_id@/requ_no/";
            sRequisitionUrl = sRequisitionUrl.Replace("@catg_id@", CatgId);

            String sFullUrl = sBaseUrl() + sRequisitionUrl + ReqId;
            return sFullUrl;
        }
        public static String PurchaseOrderInfo(String CatgId, String PoId)
        {
          String sPurchaseOrderUrl = $"purchase_order_details/catg_id/{CatgId}/order_no/{PoId}";

          //sPurchaseOrderUrl = sPurchaseOrderUrl.Replace("@catg_id@", CatgId);

            String sFullUrl = sBaseUrl() + sPurchaseOrderUrl;
            return sFullUrl;
        }
        public static String ProformaInvoiceInfo(String CatgId, String PiId)
        {
         String sProformaInvoiceUrl = "Pi_info/catg_id/@catg_id@/pi_no/";

        sProformaInvoiceUrl = sProformaInvoiceUrl.Replace("@catg_id@", CatgId);
            String sFullUrl = sBaseUrl() + sProformaInvoiceUrl + PiId;
            return sFullUrl;
        }
        public static String LetterOfCreditInfo(String CatgId, String LcId)
        {

            String sLetterOfCreditUrl = "Lc/catg_id/@catg_id@/lc_number/";
            sLetterOfCreditUrl = sLetterOfCreditUrl.Replace("@catg_id@", CatgId);
            String sFullUrl = sBaseUrl() + sLetterOfCreditUrl + LcId;
            return sFullUrl;
        }
        public static String MaterialReceiveInfo(String CatgId, String MrrId)
        {
            String sMaterialReceiveUrl = "Mrr/catg_id/@catg_id@/mrr_no/";
            sMaterialReceiveUrl = sMaterialReceiveUrl.Replace("@catg_id@", CatgId);
            String sFullUrl = sBaseUrl() + sMaterialReceiveUrl + MrrId;
            return sFullUrl;
        }


        public static String PurchaseOrderByReqId(String ReqId)
        {
            String sPurchaseOrderByReqUrl = "purchase_order_details/order_no/0/requ_id/";
            String sFullUrl = sBaseUrl() + sPurchaseOrderByReqUrl + ReqId;
            return sFullUrl;
        }
        public static String ProformaInvoiceByPoId(String PoId)
        {
            String sProformaInvoiceByPoUrl = "Pi_info/pi_no/0/work_order/";
            String sFullUrl = sBaseUrl() + sProformaInvoiceByPoUrl + PoId;
            return sFullUrl;

        }
        public static String LetterOfCreditByPiId(String PiId)
        {
            String sLetterOfCreditByPiUrl = "Lc/lc_number/0/pi_id/";
            String sFullUrl = sBaseUrl() + sLetterOfCreditByPiUrl + PiId;
            return sFullUrl;

        }

		public static String getCategoryByWO(String wo_order)
		{
			String url = "item_category_by_po_number/wo_number/";
			String sFullUrl = sBaseUrl() + url + wo_order;
			return sFullUrl;

		}
		public static String MaterialReceiveByPoId(String PoId,String PiId)
        {
            String sPoPart = "Mrr/mrr_no/0/parchase_order_ids/";
            String sPiPart = "/pi_ids/";
            PoId = (String.IsNullOrEmpty(PoId) ? "0" : PoId);
            PiId = (String.IsNullOrEmpty(PiId) ? "0" : PiId);


            String sFullUrl = sBaseUrl() + sPoPart + PoId+ sPiPart+ PiId;
            return sFullUrl;
        }
        #endregion

        private static String sBaseUrl()
        {
            String sBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
            return sBaseUrl;
        }

        
    }
}
