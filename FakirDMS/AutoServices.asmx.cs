using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace FokirDMS
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService()]

    public class AutoServices : WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #region Web Service based on Database
        [WebMethod]
        public String[] GetEmployeeInfo(String prefixText)
        {
            List<String> listProducts = new List<String>();
            DataTable _objdt = new DataTable();
            _objdt = GetEmployeeInfoFromDatabase(prefixText);
            if (_objdt.Rows.Count > 0)
            {
                for (int i = 0; i < _objdt.Rows.Count; i++)
                {
                    listProducts.Add(_objdt.Rows[i]["UserName"].ToString());
                }
            }
            return listProducts.ToArray();
        }

        [WebMethod]
        public String[] GetSupplierInfo(String prefixText)
        {
            List<String> listSuppler = new List<String>();
            DataTable _objdt = new DataTable();
            _objdt = GetSupplierInfoFromDatabase(prefixText);
            if (_objdt.Rows.Count > 0)
            {
                for (int i = 0; i < _objdt.Rows.Count; i++)
                {
                    listSuppler.Add(_objdt.Rows[i]["Supplier"].ToString());
                }
            }
            return listSuppler.ToArray();
        }

        public DataTable GetEmployeeInfoFromDatabase(String prefixText)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("SELECT LoginID+' :: '+ UserName+', '+DG.LookupText+', '+D.LookupText AS UserName  ");
            sQuery.Append("FROM Sys_Users U   ");
            sQuery.Append("LEFT JOIN dbo.Sys_Lookup D ON D.LookupTypeId=2 AND U.DepartmentID=D.LookupValue   ");
            sQuery.Append("LEFT JOIN dbo.Sys_Lookup DG ON DG.LookupTypeId=3 AND U.DesignationId=DG.LookupValue    ");
            sQuery.Append("WHERE U.LoginID LIKE '%" + prefixText + "%'  OR U.UserName LIKE '%" + prefixText + "%' AND  U.IsActive=1  ");

            DataManager dataManager = new DataManager();
            return dataManager.GetDataTable(sQuery.ToString());
        }

        public DataTable GetSupplierInfoFromDatabase(String prefixText)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("select distinct SupplierName as Supplier from DocumentDetails  ");
            sQuery.Append("where rtrim(ltrim(isnull(SupplierName,'')))<>'' and SupplierName LIKE '%" + prefixText + "%' ");

            DataManager dataManager = new DataManager();
            return dataManager.GetDataTable(sQuery.ToString());
        }

        #endregion
    }
}
