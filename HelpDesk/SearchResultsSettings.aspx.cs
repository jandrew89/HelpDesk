using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CWF_SQLCookies;


namespace QA
{
    public partial class SearchResultsSettings : BaseClass
    {
        private SelectedColumns.Search ColsSearch;
        private SelectedColumns.Excel ColsExcel;

        protected void Page_Load(object sender, EventArgs e)
        {
            ColsSearch = new SelectedColumns.Search(myCook.GetCookieSQL("SearchResultsSearch"));
            ColsExcel = new SelectedColumns.Excel(myCook.GetCookieSQL("SearchResultsExcel"));

            if (Request["__EVENTTARGET"] == "Return")
            {
                Response.Redirect(getPageName());
            }

            if (Request["__EVENTTARGET"] == "LoadDefaults")
            {
                myCook.SetCookieSQL("SearchResultsSearch", "");
                myCook.SetCookieSQL("SearchResultsExcel", "");               
                Response.Redirect("SearchResultsSettings.aspx?" + Request.QueryString);
            }

            setColumnSelectFields();
            ConfigurePage();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // This runs after Page_Load
            // In order to use or override viewstate some things need to run in here
            // Viewstate automatically fills the values for the dynamic controls
            // inside of the grid after the page_load event
            if (Request["__EVENTTARGET"] == "Save")
            {
                fnSave();
                Response.Redirect(getPageName());
            }
        }

        private void ConfigurePage()
        {
            Master.Title = "Search Results Settings";
            Master.HeaderText = "Search Results Settings";

            string pageName = getPageName();

            Master.MyToolbar.LoadToolbarItem("", "javascript:fnReturn();", "GoRtlHS.png", "Return", "", "Return to Search Results");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnSave();", "save16.png", "Save", "", "Save settings and display Search Results");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnLoadDefaults();", "Make Favorite16.png", "Load Defaults", "", "Load default settings");            
        }

        private void setColumnSelectFields()
        {
            lblColumnSelect.Visible = true;

            int offset = 0;
            tblSearch.Rows.Clear();
            List<SelectedColumns.Column> colsSearch = ColsSearch.getAllColumns();
            foreach (SelectedColumns.Column c in colsSearch)
            {
                buildTableRow(tblSearch, c, offset++);
            }

            offset = 0;
            tblExcel.Rows.Clear();
            List<SelectedColumns.Column> colsExcel = ColsExcel.getAllColumns();
            foreach (SelectedColumns.Column c in colsExcel)
            {
                buildTableRow(tblExcel, c, offset++);
            }            
        }

        private void fnSave()
        {
            
            for (int i = 0; i < tblSearch.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)tblSearch.Rows[i].FindControl(tblSearch.ID + "_cbID_" + i);
                SelectedColumns.Column c = ColsSearch.getColumn((string)ViewState[cb.ID]);
                if (c != null)
                    c.selected = cb.Checked;
            }
            ColsSearch.reOrder();
            myCook.SetCookieSQL("SearchResultsSearch", ColsSearch.getCookie());

            for (int i = 0; i < tblExcel.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)tblExcel.Rows[i].FindControl(tblExcel.ID + "_cbID_" + i);
                SelectedColumns.Column c = ColsExcel.getColumn((string)ViewState[cb.ID]);
                if (c != null)
                    c.selected = cb.Checked;
            }
            ColsExcel.reOrder();
            myCook.SetCookieSQL("SearchResultsExcel", ColsExcel.getCookie());
                       
            setColumnSelectFields();
        }

        private string getPageName()
        {
            string[] names = { "pageName" };
            return Request["pageName"]; // +"?" + removeFrom_QueryString(names);
        }

        private void buildTableRow(Table tbl, SelectedColumns.Column c, int offset)
        {
            TableRow tr = new TableRow();
            tr.CssClass = "SP_Row";

            TableCell tc = new TableCell();
            if (offset == 0)
            {
                tc.CssClass = "SP_itemLabelCell";
                Label lbl1 = new Label();
                switch (tbl.ID)
                {
                    case "tblSearch":
                        lbl1.Text = "Search";
                        break;
                    case "tblExcel":
                        lbl1.Text = "Excel";
                        break;                    
                }
                lbl1.CssClass = "SP_itemLabel2";
                tc.Controls.Add(lbl1);
            }
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.CssClass = "SP_dataLabelCell";
            CheckBox cb = new CheckBox();
            cb.CssClass = "SP_dataLabel";
            if (!string.IsNullOrEmpty(c.HeaderText))
                cb.Text = c.HeaderText;
            else
                cb.Text = c.dbFieldName;

            cb.ID = tbl.ID + "_cbID_" + offset;
            ViewState[cb.ID] = c.dbFieldName;

            cb.Checked = c.selected;
            cb.Enabled = !c.required;

            tc.Controls.Add(cb);
            tr.Cells.Add(tc);

            if (c.selected)
            {
                tc = new TableCell();
                tc.CssClass = "SP_dataLabelCell";

                ImageButton ib;
                ib = new ImageButton();
                ib.CssClass = "SP_dataLabel";
                ib.ImageUrl = "images/Move Up (Arrow)16.png";
                ib.CommandArgument = c.dbFieldName;
                ib.CommandName = "Up";
                switch (tbl.ID)
                {
                    case "tblSearch":
                        ib.Command += moveSearch_Click;
                        break;
                    case "tblExcel":
                        ib.Command += moveExcel_Click;
                        break;                   
                }
                ib.ID = tbl.ID + "_ibID_Up_" + c.dbFieldName;

                tc.Controls.Add(ib);
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.CssClass = "SP_dataLabelCell";

                ib = new ImageButton();
                ib.CssClass = "SP_dataLabel";
                ib.ImageUrl = "images/Move Down (Arrow)16.png";
                ib.CommandArgument = c.dbFieldName;
                ib.CommandName = "Down";
                switch (tbl.ID)
                {
                    case "tblSearch":
                        ib.Command += moveSearch_Click;
                        break;
                    case "tblExcel":
                        ib.Command += moveExcel_Click;
                        break;                    
                }
                ib.ID = tbl.ID + "_ibID_Down_" + c.dbFieldName;

                tc.Controls.Add(ib);
                tr.Cells.Add(tc);
            }
            else
            {
                tc = new TableCell();
                tc.ColumnSpan = 2;
                tc.CssClass = "SP_dataLabelCell";
                tr.Cells.Add(tc);
            }

            tbl.Rows.Add(tr);
        }

        protected void moveSearch_Click(object sender, CommandEventArgs e)
        {
            SelectedColumns.Column c = ColsSearch.getColumn((string)e.CommandArgument);
            if (e.CommandName == "Up")
                ColsSearch.MoveUp(c);
            else
                ColsSearch.MoveDown(c);
            myCook.SetCookieSQL("SearchResultsSearch", ColsSearch.getCookie());
            setColumnSelectFields();
        }

        protected void moveExcel_Click(object sender, CommandEventArgs e)
        {
            SelectedColumns.Column c = ColsExcel.getColumn((string)e.CommandArgument);
            if (e.CommandName == "Up")
                ColsExcel.MoveUp(c);
            else
                ColsExcel.MoveDown(c);
            myCook.SetCookieSQL("SearchResultsExcel", ColsExcel.getCookie());
            setColumnSelectFields();
        }


    }
}