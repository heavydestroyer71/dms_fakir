using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web.Security;
using System.Xml.Linq;


namespace CoreLibrary
{
    public static class PopulateLists
    {
        #region Category, Role and UserGroup Related
        public static DataTable GetCategorys()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Category")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetFlowsByCategory(String CategoryId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, CategoryId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "FlowByCategory")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
        #endregion
        
        #region Document, Expense, Currency and Status Related
        public static DataTable GetAttachmentTypes()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "AttachmentType")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetExpenseTypes()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "ExpenseType")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetCurrencys()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Currency")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetStatuses()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Status")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
        #endregion

        #region LookupType and Lookup Related
        public static DataTable GetLookupTypes()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LookupType")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
        #endregion

        #region TeamWork Related
        public static DataTable GetTeamMemberList(String sSupervisorId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, sSupervisorId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "TeamMemberBySupervisorId")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
        #endregion

        #region Company, Department, Location Related
        public static DataTable GetCompanies()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Company")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetCompaniesByUser_Category(String sUserId, String sCategoryId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[3]
            {
                dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, sUserId),
                dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, sCategoryId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "CompanyByUser_Category")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetDepartments()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Department")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetLocations()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Location")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetDesignations()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Designation")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
        #endregion

        #region Storage (Room, Rack, Shelf & Box) Related
        public static DataTable GetRooms()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Room")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
		public static DataTable GetUsersbyFlow(int flowId, int userId, string username)
		{
			DataManager dataManager = new DataManager();
			SqlParameter[] parameters = new SqlParameter[3]
			{
				dataManager.MakeInParam("@FlowId", SqlDbType.NVarChar, 500, flowId),
				dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, userId),
				dataManager.MakeInParam("@UserName", SqlDbType.NVarChar, 500, username)
			};
			return dataManager.GetDataTable("GET_USER_LIST", parameters);
		}

		public static DataTable GetRoomWiseRack(String RoomId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                dataManager.MakeInParam("@ParentId", SqlDbType.NVarChar, 500, RoomId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "RoomWiseRack")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetRackWiseShelf(String RackId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                dataManager.MakeInParam("@ParentId", SqlDbType.NVarChar, 500, RackId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "RackWiseShelf")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }

        public static DataTable GetShelfWiseBox(String ShelfId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                dataManager.MakeInParam("@ParentId", SqlDbType.NVarChar, 500, ShelfId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "ShelfWiseBox")
            };
            return dataManager.GetDataTable("SP_SYS_POPULATE_LIST", parameters);
        }
        #endregion
    }
}