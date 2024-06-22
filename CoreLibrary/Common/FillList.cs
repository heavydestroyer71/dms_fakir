using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;


namespace CoreLibrary
{
    public static class FillList
    {
        public static void PopulateDropDownList(DataTable DropDownListData, DropDownList DropDownListId, String DisplayField, String ValueField)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            foreach (DataRow myRow in DropDownListData.Rows)
            {
                DataRow dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow[ValueField];
                dRow["ItemText"] = myRow[DisplayField];
                dtDropDown.Rows.Add(dRow);
            }
            DropDownListId.DataSource = dtDropDown;
            DropDownListId.DataTextField = "itemText";
            DropDownListId.DataValueField = "itemValue";
            DropDownListId.DataBind();
        }

        public static void PopulateDropDownList(DataTable DropDownListData, DropDownList DropDownListId, String DisplayField, String ValueField, Boolean IsExtraTop, String TopText, String TopValue)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            if (IsExtraTop == true)
            {
                DataRow dRow = dtDropDown.NewRow();
                try
                {
                    dRow["ItemValue"] = TopValue;
                }
                catch (Exception)
                {
                    dRow["ItemValue"] = "-1";
                }

                try
                {
                    dRow["itemText"] = TopText;
                }
                catch (Exception)
                {
                    dRow["itemText"] = "";
                }
                dtDropDown.Rows.Add(dRow);
            }

            // add list contents from the Dataset
            foreach (DataRow myRow in DropDownListData.Rows)
            {
                DataRow dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow[ValueField];
                dRow["ItemText"] = myRow[DisplayField];
                dtDropDown.Rows.Add(dRow);
            }
            DropDownListId.DataSource = dtDropDown;
            DropDownListId.DataTextField = "itemText";
            DropDownListId.DataValueField = "itemValue";
            DropDownListId.DataBind();
        }

        public static void PopulateDropDownList(DataTable DropDownListData, DropDownList DropDownListId, String TopText)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            DataRow dRow = dtDropDown.NewRow();
            dRow["ItemValue"] = "0";
            dRow["itemText"] = TopText;
            dtDropDown.Rows.Add(dRow);

            foreach (DataRow myRow in DropDownListData.Rows)
            {
                dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow["ValueField"];
                dRow["ItemText"] = myRow["DisplayField"];
                dtDropDown.Rows.Add(dRow);
            }
            DropDownListId.DataSource = dtDropDown;
            DropDownListId.DataTextField = "itemText";
            DropDownListId.DataValueField = "itemValue";
            DropDownListId.DataBind();
        }

        public static void PopulateGridView(DataTable GridViewData, GridView GridViewId)
        {
            if (GridViewData.Rows.Count == 0)
            {
                GridViewData = new DataTable();
            }

            GridViewId.DataSource = GridViewData;
            GridViewId.DataBind();
        }

        public static StringWriter ExportToExcel(GridView gridView)
        {
                       
            
            StringWriter sw = new StringWriter();

           // sw.Write("");

            //sw.Write("<div><table cellspacing=@0@  border=@1@ style=@border-collapse:collapse;@>".Replace("@", "\""));

            sw.Write(ExportRowData(gridView.HeaderRow, true));

            GridViewRowCollection rows = gridView.Rows;
            System.Collections.Generic.List<GridViewRow> allRows = new System.Collections.Generic.List<GridViewRow>();

            for (int pageIndex = 0; pageIndex < gridView.PageCount; pageIndex++)
            {
                gridView.PageIndex = pageIndex;

                foreach (GridViewRow row in gridView.Rows)
                {
                    allRows.Add(row);
                }
            }


            foreach (GridViewRow row in allRows)
            {
                sw.Write(ExportRowData(row, false));
            }


            //foreach (GridViewRow row in gridView.Rows)
            //{

            //    sw.Write(ExportRowData(row, false));


            //}

            //sw.Write("</table></div>");

            return sw;
            
        }

        private static StringWriter ExportRowData(GridViewRow gvr, bool IsHeader)
        {
            
                StringWriter sw = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter((TextWriter)sw);
                
                    string str = "<div><table cellspacing=@0@ rules=@all@ border=@1@ style=@border-collapse:collapse;@>".Replace("@", "\"");
                    sw.Write(str);
               
                sw.Write("<tr>");
                if (IsHeader)
                {
                    
                sw.Write("<br/>");
                    for (int index = 1; index < gvr.Cells.Count-1; ++index)
                    {
                        sw.Write("<th scope=\"col\">");
                        sw.Write(gvr.Cells[index].Text);
                        sw.Write("</th>");
                    }
                }
                else
                {
                     HyperLink hl = null;
                     string Data = "";

                    for (int index = 1; index < gvr.Cells.Count-1; ++index)
                    {
                        sw.Write("<td >");

                        if(index==1) 
                        {
                        hl = gvr.FindControl("lblReturnPath") as HyperLink;
                        Data = hl.Text;
                        }
                        else if (index == 2)
                        {
                        hl = gvr.FindControl("lblRefReturnPath") as HyperLink;
                        Data = hl.Text;
                        }
                        else { Data = gvr.Cells[index].Text; }

                        sw.Write(Data);

                        sw.Write("</td>");
                    }
                }


            sw.Write("</tr>");
                
            sw.Write("</table></div>");
          

            return sw;


        }

    }
}