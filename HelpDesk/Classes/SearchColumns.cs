using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web;

namespace nsAppColumns
{
    public class AppColumns : Columns
    {       
        public AppColumns(DataTable dtColumns, string cookieColumns)
        {
            foreach (DataRow dr in dtColumns.Rows)
            {
                addColumn(dr["Name"].ToString(), dr["Name"].ToString(), dr["FriendlyName"].ToString(), (bool)dr["Required"], (bool)dr["Selected"], HorizontalAlign.Left, 0, dr["tableName"].ToString());
            }            

            if (cookieColumns.Length > 0)
            {
                ResetColumnSelection();

                int index = 0;
                string[] cc = cookieColumns.Split('~');
                foreach (string s in cc)
                {
                    Column c = lstColumns.Find(delegate (Column l) { return l.dbFieldName == s; });
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

    //public class Excel : Columns
    //{
    //    public Excel(DataTable dtColumns, string cookieColumns)
    //    {
    //        foreach (DataRow dr in dtColumns.Rows)
    //        {
    //            addColumn(dr["Name"].ToString(), dr["Name"].ToString(), dr["FriendlyName"].ToString(), (bool)dr["Required"], (bool)dr["Selected"], HorizontalAlign.Left, 0, dr["TableName"].ToString());
    //        }

    //        if (cookieColumns.Length > 0)
    //        {
    //            ResetColumnSelection();

    //            int index = 0;
    //            string[] cc = cookieColumns.Split('~');
    //            foreach (string s in cc)
    //            {
    //                Column c = lstColumns.Find(delegate (Column l) { return l.dbFieldName == s; });
    //                if (c != null)
    //                {
    //                    c.selected = true;
    //                    lstColumns.Remove(c);
    //                    lstColumns.Insert(index++, c);
    //                }
    //            }
    //            reOrder();
    //        }
    //    }
    //}


    public class Columns
    {
        protected List<Column> lstColumns;

        public Columns()
        {
            lstColumns = new List<Column>();

        }

        public void convertOldCookie(string cookie)
        {
            if (cookie.Length > 0)
            {
                string[] tmp = cookie.Split(',');
                foreach (string s in tmp)
                {
                    Column c = getColumn(s);
                    if (c != null)
                    {
                        c.selected = true;
                    }
                }
            }
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

        public void ResetColumnSelection()
        {
            // Turn off all columns except required ones
            foreach (Column l in lstColumns)
                if (l.required)
                    l.selected = true;
                else
                    l.selected = false;
        }

        public Column getColumn(string dbFieldName)
        {
            Column c = lstColumns.Find(delegate(Column l) { return l.dbFieldName == dbFieldName; });
            return c;
        }

        public Column getSelectedColumn(string dbFieldName)
        {
            Column c = lstColumns.Find(delegate(Column l) { return l.dbFieldName == dbFieldName; });
            //if (c == null)
            //  throw new Exception(dbFieldName);
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
                    Column c1 = new Column(c.dbFieldName, c.dbSortFieldName, c.HeaderText, c.required, c.horizontalAlign, c.maxLength, c.dbTableName);
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

        public Column addColumn(string dbField, string dbSortField, string HeaderText, bool required, bool selected, HorizontalAlign ha, int MaxLength, string dbTableName)
        {
            Column f = new Column(dbField, dbSortField, HeaderText, required, ha, MaxLength, dbTableName);
            f.selected = selected;
            lstColumns.Add(f);
            return f;
        }

        //public void removeColumn(string dbField)
        //{
        //    Column f = getColumn(dbField);
        //    if (f != null)
        //        lstColumns.Remove(f);
        //}

        public void removeColumn(Column c)
        {
            lstColumns.Remove(c);
        }

    }

    public class Column
    {
        private string _dbFieldName = "";
        private string _dbSortFieldName = "";
        private string _HeaderText = "";
        private string _dbTableName = "";
        private string _methodName = "";
        private bool _required = false;
        private bool _selected = false;
        private HorizontalAlign _ha;
        private int _maxLength = 0;

        public Column(string dbFieldName, string dbSortFieldName, string HeaderText, bool required, HorizontalAlign ha, int MaxLength, string dbTableName)
        {
            _dbFieldName = dbFieldName;
            _dbSortFieldName = dbSortFieldName;
            _HeaderText = HeaderText;
            _required = required;
            _ha = ha;
            _maxLength = MaxLength;
            _dbTableName = dbTableName;
        }

        public string dbTableName
        {
            get
            { return _dbTableName; }
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
    }

}