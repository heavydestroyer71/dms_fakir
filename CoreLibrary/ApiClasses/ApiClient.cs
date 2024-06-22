using System;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class ApiClient
    {

        #region Category
        public static async Task<DataTable> GetCategoryInfo(String CatgId)
        {
            return await RequestDataTable(ApiEndpoint.CategoryInfo(CatgId));
        }
        #endregion

        #region Requistion
        public static async Task<DataTable> GetRequistionInfo(String CatgId, String requistionNo)
        {
            return await RequestDataTable(ApiEndpoint.RequisitionInfo(CatgId, requistionNo));
        }
        #endregion

        #region Purchase Order
        public static async Task<DataTable> GetPurchaseOrderInfo(String CatgId, String poNo)
        {
            return await RequestDataTable(ApiEndpoint.PurchaseOrderInfo(CatgId, poNo));
        }

        public static async Task<DataTable> GetPurchaseOrderByReqId(String reqId)
        {
            return await RequestDataTable(ApiEndpoint.PurchaseOrderByReqId(reqId));
        }
        #endregion

        #region Proforma Invoice
        public static async Task<DataTable> GetProformaInvoiceInfo(String CatgId, String piNo)
        {
            return await RequestDataTable(ApiEndpoint.ProformaInvoiceInfo(CatgId, piNo));
        }

        public static async Task<DataTable> GetProformaInvoiceByPoId(String poId)
        {
            return await RequestDataTable(ApiEndpoint.ProformaInvoiceByPoId(poId));
        }
        #endregion

        #region Letter Of Credit
        public static async Task<DataTable> GetLetterOfCreditInfo(String CatgId, String lcNo)
        {
            return await RequestDataTable(ApiEndpoint.LetterOfCreditInfo(CatgId, lcNo));
        }

        public static async Task<DataTable> GetLetterOfCreditByPiId(String piId)
        {
            return await RequestDataTable(ApiEndpoint.LetterOfCreditByPiId(piId));
        }
        #endregion

        #region Material Receive
        public static async Task<DataTable> GetMaterialReceiveInfo(String CatgId, String mrrNo)
        {
            return await RequestDataTable(ApiEndpoint.MaterialReceiveInfo(CatgId, mrrNo));
        }

        public static async Task<DataTable> GetMaterialReceiveByPoId(String poId, String piId)
        {
            return await RequestDataTable(ApiEndpoint.MaterialReceiveByPoId(poId, piId));
        }
        #endregion


        #region Private mathods for API call and convert Json Array to DataTable
        private static async Task<DataTable> RequestDataTable(String endpointAddress)
        {
            DataTable dtApiResult = new DataTable();

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(endpointAddress);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        JObject jsonObject = JObject.Parse(jsonResult);
                        JArray resultSetArray = (JArray)jsonObject["resultset"];

                        dtApiResult = ConvertJArrayToDataTable(resultSetArray);
                    }
                    else
                    {
                        // Handle the error if necessary
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorTracking.SaveError("", "ApiClient", "RequestDataTable", ex);
            }
            catch (Exception ex)
            {
                ErrorTracking.SaveError("", "ApiClient", "RequestDataTable", ex);
            }

            return dtApiResult;
        }

        private static DataTable ConvertJArrayToDataTable(JArray array)
        {
            DataTable dataTable = new DataTable();

            if (array.Count > 0)
            {
                JObject firstRow = (JObject)array[0];
                foreach (JProperty property in firstRow.Properties())
                {
                    dataTable.Columns.Add(property.Name, typeof(object));
                }
            }

            foreach (JObject item in array)
            {
                DataRow row = dataTable.NewRow();
                foreach (JProperty property in item.Properties())
                {
                    row[property.Name] = property.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        #endregion
    }
}
