using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;

namespace SelectedColumns
{
    public class Search : Columns
    {
        public Search(string cookieColumns)
        {
            addColumn("Location", "BP", "Location", true, true, HorizontalAlign.Left, 0);
            addColumn("friendlyName", "friendlyName", "Number", true, true, HorizontalAlign.Left, 0);
            addColumn("Status", "Status", "Status", true, true, HorizontalAlign.Left, 0);
            addColumn("AssignedTo", "AssignedTo", "Assigned To", true, true, HorizontalAlign.Left, 0);
            addColumn("Department", "Department", "Department", true, true, HorizontalAlign.Right, 0);
            addColumn("Equipment", "Equipment", "Equipment", true, true, HorizontalAlign.Left, 0);
            addColumn("Comment", "", "Service Required", false, true, HorizontalAlign.Left, 0);
            addColumn("Originator", "Originator", "Originator", false, true, HorizontalAlign.Left, 0);
            addColumn("Type", "Type", "Type", true, true, HorizontalAlign.Left, 0);
            addColumn("Response", "Response", "Response", true, true, HorizontalAlign.Left, 0);
            addColumn("Condition", "Condition", "Condition", true, true, HorizontalAlign.Left, 0);
            addColumn("CreatedBy", "CreatedBy", "Created By", true, true, HorizontalAlign.Left, 0);
            addColumn("DateCreated", "DateCreated", "Date Created", true, true, HorizontalAlign.Right, 0);            
            addColumn("StartedDateTime", "StartedDateTime", "Start Date", true, true, HorizontalAlign.Right, 0);
            //addColumn("onTime", "onTime", "On Time", false, false, HorizontalAlign.Center, 0);
            addColumn("DaystoComplete", "DaystoComplete", "Days to Complete", false, false, HorizontalAlign.Right, 0);
            addColumn("Comments", "", "Workflow Comments", false, false, HorizontalAlign.Left, 0);
            addColumn("StepDescription", "StepDescription", "Step Description", false, false, HorizontalAlign.Left, 0);
            addColumn("stepUsers", "stepUsers", "Step Users", true, true, HorizontalAlign.Left, 0);
            addColumn("DaysInQueue", "DaysInQueue", "Days In Queue", false, false, HorizontalAlign.Right, 0);

            if (cookieColumns.Length > 0)
            {
                // Turn off all columsn except required ones so the cookies drives which columns are selected
                ResetColumnSelection();

                int index = 0;
                string[] cc = cookieColumns.Split('~');
                foreach (string s in cc)
                {
                    Column c = lstColumns.Find(delegate(Column l) { return l.dbFieldName == s; });
                    if (c != null)
                    {
                        c.selected = true;
                        lstColumns.Remove(c);
                        lstColumns.Insert(index++, c);
                    }
                }
                reOrder();
            }
        }
    }


    public class Excel : Columns
    {
        public Excel(string cookieColumns)
        {
            addColumn("Location", "BP", "Location", true, true, HorizontalAlign.Left, 0);
            addColumn("friendlyName", "friendlyName", "Number", true, true, HorizontalAlign.Left, 0);
            addColumn("Status", "Status", "Status", true, true, HorizontalAlign.Left, 0);
            addColumn("AssignedTo", "AssignedTo", "Assigned To", true, true, HorizontalAlign.Left, 0);
            addColumn("Department", "Department", "Department", true, true, HorizontalAlign.Left, 0);
            addColumn("Equipment", "Equipment", "Equipment", true, true, HorizontalAlign.Left, 0);
            addColumn("Comment", "", "Service Required", false, true, HorizontalAlign.Left, 0);
            addColumn("Originator", "Originator", "Originator", false, true, HorizontalAlign.Left, 0);
            addColumn("Type", "Type", "Type", true, true, HorizontalAlign.Left, 0);
            addColumn("Response", "Response", "Response", true, true, HorizontalAlign.Left, 0);
            addColumn("Condition", "Condition", "Condition", true, true, HorizontalAlign.Left, 0);
            addColumn("CreatedBy", "CreatedBy", "Created By", true, true, HorizontalAlign.Left, 0);
            addColumn("DateCreated", "DateCreated", "Date Created", true, true, HorizontalAlign.Right, 0);
            addColumn("StartedDateTime", "StartedDateTime", "Start Date", true, true, HorizontalAlign.Right, 0);
            //addColumn("onTime", "onTime", "On Time", false, false, HorizontalAlign.Center, 0);
            addColumn("DaystoComplete", "DaystoComplete", "Days to Complete", false, false, HorizontalAlign.Right, 0);
            addColumn("Comments", "", "Workflow Comments", false, false, HorizontalAlign.Left, 0);
            addColumn("StepDescription", "StepDescription", "Step Description", false, false, HorizontalAlign.Left, 0);
            addColumn("stepUsers", "stepUsers", "Step Users", false, false, HorizontalAlign.Left, 0);
            addColumn("DaysInQueue", "DaysInQueue", "Days In Queue", false, false, HorizontalAlign.Right, 0);

            if (cookieColumns.Length > 0)
            {
                // Turn off all columsn except required ones so the cookies drives which columns are selected
                ResetColumnSelection();

                int index = 0;
                string[] cc = cookieColumns.Split('~');
                foreach (string s in cc)
                {
                    Column c = lstColumns.Find(delegate(Column l) { return l.dbFieldName == s; });
                    if (c != null)
                    {
                        c.selected = true;
                        lstColumns.Remove(c);
                        lstColumns.Insert(index++, c);
                    }
                }
                reOrder();
            }
        }
    }


    public class Columns
    {
        protected List<Column> lstColumns;

        public Columns()
        {
            lstColumns = new List<Column>();

        }

        public List<Column> getAllColumns()
        {
            return lstColumns;
        }

        public List<Column> getSelectedColumns()
        {
            List<Column> sList = new List<Column>();
            foreach (Column l in lstColumns)
                if (l.selected)
                    sList.Add(l);
            return sList;
        }

        public Column getColumn(string dbFieldName)
        {
            Column c = lstColumns.Find(delegate(Column l) { return l.dbFieldName == dbFieldName; });
            return c;
        }

        public Column getSelectedColumn(string dbFieldName)
        {
            Column c = lstColumns.Find(delegate(Column l) { return l.dbFieldName == dbFieldName; });
            if (c.selected)
                return c;
            return null;
        }

        public Column getSelectedColumnName(string HeaderText)
        {
            Column c = lstColumns.Find(delegate(Column l) { return l.HeaderText == HeaderText; });
            if (c == null)
                return null;
            if (c.selected)
                return c;
            return null;
        }

        public List<Column> getSelectedColumns(string dbFieldName)
        {
            List<Column> newList = new List<Column>();
            List<Column> selColumns = getSelectedColumns();
            foreach (Column c in selColumns)
            {
                if (c.dbFieldName.Contains(dbFieldName))
                    newList.Add(c);
            }
            return newList;
        }

        public ListItem getListItem(Column c)
        {
            ListItem i = new ListItem(c.HeaderText, c.dbFieldName);
            i.Selected = c.selected;
            return i;
        }

        public string getCookie()
        {
            string retval = "";
            List<Column> lst = getSelectedColumns();
            foreach (Column c in lst)
                retval += c.dbFieldName + "~";
            if (retval.Length > 0)
                retval = retval.Substring(0, retval.Length - 1);
            return retval;
        }

        public void MoveUp(Column c)
        {
            int offset = 0;
            int i = 0;
            foreach (Column u in lstColumns)
            {
                if (u == c)
                {
                    offset = i;
                    break;
                }
                i++;
            }
            if (offset > 0)
            {
                Column x = getColumn(c.dbFieldName);
                lstColumns.Remove(x);
                lstColumns.Insert(offset - 1, x);
            }
        }

        public void MoveDown(Column c)
        {
            int offset = 0;
            int i = 0;
            foreach (Column u in lstColumns)
            {
                if (u == c)
                {
                    offset = i;
                    break;
                }
                i++;
            }
            if (offset < lstColumns.Count)
            {
                if (offset < lstColumns.Count - 1)
                {
                    if (!lstColumns[offset + 1].selected)
                        return;
                }
                Column x = getColumn(c.dbFieldName);
                lstColumns.Remove(x);
                lstColumns.Insert(offset + 1, x);
            }
        }

        public void reOrder()
        {
            List<Column> newList = new List<Column>();
            foreach (Column c in lstColumns)
            {
                if (c.selected)
                {
                    Column c1 = new Column(c.dbFieldName, c.dbSortFieldName, c.HeaderText, c.required, c.horizontalAlign, c.maxLength);
                    c1.selected = c.selected;
                    newList.Add(c1);
                }
            }
            int index = 0;
            foreach (Column c in newList)
            {
                Column x = getColumn(c.dbFieldName);
                lstColumns.Remove(x);
                lstColumns.Insert(index++, c);
            }
        }

        protected Column addColumn(string dbField, string dbSortField, string HeaderText, bool required, bool selected, HorizontalAlign ha, int MaxLength)
        {
            Column f = new Column(dbField, dbSortField, HeaderText, required, ha, MaxLength);
            f.selected = selected;
            lstColumns.Add(f);
            return f;
        }

        public void removeColumn(Column c)
        {
            lstColumns.Remove(c);
        }

        public void ResetColumnSelection()
        {
            // Turn off all columns except required ones
            foreach (Column l in lstColumns)
                if (l.required)
                    l.selected = true;
                else
                    l.selected = false;
        }

    }

    public class Column
    {
        private string _dbFieldName = "";
        private string _dbSortFieldName = "";
        private string _HeaderText = "";
        private string _toolTip = "";
        //private bool _template = false;
        private string _methodName = "";
        private bool _required = false;
        private bool _selected = false;
        private HorizontalAlign _ha;
        private int _maxLength = 0;
        private bool _wrap = false;
        private string _dataFormat = "";

        public Column(string dbFieldName, string dbSortFieldName, string HeaderText, bool required, HorizontalAlign ha, int MaxLength)
        {
            _dbFieldName = dbFieldName;
            _dbSortFieldName = dbSortFieldName;
            _HeaderText = HeaderText;
            _required = required;
            _ha = ha;
            _maxLength = MaxLength;
        }

        public string dbFieldName
        {
            get
            { return _dbFieldName; }
        }

        public string dbSortFieldName
        {
            get
            { return _dbSortFieldName; }
            set
            { _dbSortFieldName = value; }
        }

        public string HeaderText
        {
            get
            { return _HeaderText; }
        }

        public string methodName
        {
            get
            { return _methodName; }
        }

        public bool required
        {
            get
            { return _required; }
        }

        public bool selected
        {
            set
            { _selected = value; }
            get
            { return _selected; }
        }

        public HorizontalAlign horizontalAlign
        {
            get
            { return _ha; }
        }

        public int maxLength
        {
            set
            { _maxLength = value; }
            get
            { return _maxLength; }
        }

        public string toolTip
        {
            get
            { return _toolTip; }
        }

        public bool wrap
        {
            set
            { _wrap = value; }
            get
            { return _wrap; }
        }

        public string dataFormat
        {
            set
            { _dataFormat = value; }
            get
            { return _dataFormat; }
        }
    }
   
}