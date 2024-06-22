using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CoreLibrary
{
    public class SectionVisibility
    {
        public String CategoryName { get; private set; }
        public Int32 RoleId { get; private set; }

        public Boolean IsEnableRE { get; private set; }
        public Boolean IsEnablePO { get; private set; }
        public Boolean IsEnablePI { get; private set; }
        public Boolean IsEnableLC { get; private set; }
        public Boolean IsEnableMR { get; private set; }
        public Boolean IsEnableCL { get; private set; }


        public Boolean IsEnableBill { get; private set; }
        public Boolean IsEnableAmount { get; private set; }
        public Boolean IsEnableDiscount { get; private set; }
        public Boolean IsEnableAccounts { get; private set; }
        public Boolean IsVisibleUploader { get; private set; }
        public Boolean IsEnableDownload { get; private set; }
        public Boolean IsEnableDelete { get; private set; }

        public Boolean IsInitialPath { get; private set; }
        public Boolean IsApprover { get; private set; }
        public Boolean IsCloser { get; private set; }


        public SectionVisibility(String CategoryId, String DocumentId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                 dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, CategoryId),
                 dataManager.MakeInParam("@DocumentId", SqlDbType.NVarChar, 500, DocumentId)
            };

            DataTable dtPermission = dataManager.GetDataTable("SP_SYS_CONTROL_STATUS", parameters);
            if (dtPermission.Rows.Count > 0)
            {
                CategoryName = dtPermission.Rows[0]["CategoryName"].ToString();
                RoleId = Convert.ToInt32(dtPermission.Rows[0]["RoleID"].ToString());

                IsEnableRE = Convert.ToBoolean(dtPermission.Rows[0]["IsRE"].ToString());
                IsEnablePO = Convert.ToBoolean(dtPermission.Rows[0]["IsPO"].ToString());
                IsEnablePI = Convert.ToBoolean(dtPermission.Rows[0]["IsPI"].ToString());
                IsEnableLC = Convert.ToBoolean(dtPermission.Rows[0]["IsLC"].ToString());
                IsEnableMR = Convert.ToBoolean(dtPermission.Rows[0]["IsMR"].ToString());
                IsEnableCL = Convert.ToBoolean(dtPermission.Rows[0]["IsCL"].ToString());

                IsEnableBill = Convert.ToBoolean(dtPermission.Rows[0]["IsBill"].ToString());
                IsEnableAmount = Convert.ToBoolean(dtPermission.Rows[0]["IsAmount"].ToString());
                IsEnableDiscount = Convert.ToBoolean(dtPermission.Rows[0]["IsDiscount"].ToString());

                IsEnableAccounts = Convert.ToBoolean(dtPermission.Rows[0]["IsAccounts"].ToString());

                IsVisibleUploader = Convert.ToBoolean(dtPermission.Rows[0]["IsCanUpload"].ToString());
                IsEnableDownload = Convert.ToBoolean(dtPermission.Rows[0]["IsCanDownload"].ToString());
                IsEnableDelete = Convert.ToBoolean(dtPermission.Rows[0]["IsCanDelete"].ToString());

                IsInitialPath = Convert.ToBoolean(dtPermission.Rows[0]["IsInitialPath"].ToString());
                IsApprover = Convert.ToBoolean(dtPermission.Rows[0]["IsApprover"].ToString());
                IsCloser = Convert.ToBoolean(dtPermission.Rows[0]["IsCloser"].ToString());
            }
        }
    }
}
