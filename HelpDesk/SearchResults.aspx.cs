using System;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Web.Services;

using CWF_SQLCookies;
using CWF_WorkflowRouting;
using CWF_HelpDesk;

namespace QA
{
    public partial class _searchResults : BaseClass
    {
        private SQLCookies searchPageCookies;

        public Dictionary<string, string> queryString = new Dictionary<string, string>();
        private bool myActiveList = false;
        private bool joinUsers = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            string searchPageCookieURL = _appFriendlyName + @"/SearchPage";
            searchPageCookies = new SQLCookies(searchPageCookieURL, userID, SecCred);

            setUpDictionary();

            if (!IsPostBack)
            {
                ViewState["SortExpression"] = "WFID ASC";

                // Use Sort stored in cookies
                if (!string.IsNullOrEmpty(myCook.GetCookieSQL("SearchResultsSortField")))
                    ViewState["SortExpression"] = myCook.GetCookieSQL("SearchResultsSortField") + " " + myCook.GetCookieSQL("SearchResultsSortOrder");

                initData(false);
            }

            if (Request["__EVENTTARGET"] == "DownloadToExcel")
                ExportTableExcel();

            ConfigurePage();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] == "PageSelect")
            {
                PageSelect();
            }
        }

        private void ConfigurePage()
        {
            Master.Title = _appFriendlyName;
            Master.HeaderText = _appFriendlyName + "<br><font size=3>" + Master.HeaderText + "</font>";

            Master.MyToolbar.LoadToolbarItem("default.aspx", "javascript:Search();", "Search16.png", "Search", "");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("javascript:DownloadToExcel();", "", "excel.gif", "Download to Excel", "");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("SearchResultsSettings.aspx?pageName=SearchResults.aspx", "", "Settings16.png", "Columns", "");
        }

        private void extendResults(DataTable dt)
        {
            dt.Columns.Add("Location");
            dt.Columns.Add("AssignedToFriendlyName");
            dt.Columns.Add("Type_Friendly");
            dt.Columns.Add("Response_Friendly");
            dt.Columns.Add("Condition_Friendly");
            dt.Columns.Add("StepDescription");
            dt.Columns.Add("StepUsers");
            dt.Columns.Add("StepStartTime", typeof(DateTime));
            dt.Columns.Add("DaysInQueue");

            foreach (DataRow dr in dt.Rows)
            {
                dr["DaysInQueue"] = DBNull.Value;

                Workflow2.DBStatus dbStatus = workflow2.WorkflowStatusEnum((int)dr["Status"]);
                if (dbStatus == Routing2.DBStatus.Circulating || dbStatus == Routing2.DBStatus.Rejected)
                {
                    List<Routing2.clsStepStatus> lstExtStatus = workflow2.WorkflowStatus(dr["WFID"].ToString(), (int)dr["Status"]);
                    if (lstExtStatus.Count > 0)
                    {
                        foreach (Routing2.clsStepStatus kvp in lstExtStatus)
                        {
                            dr["StepDescription"] += "<nobr>" + kvp.StepDescription + "</nobr><BR>";
                            dr["StepStartTime"] = kvp.StartTime;

                            int i;
                            if (int.TryParse(kvp.DaysInQueue, out i))
                                dr["DaysInQueue"] = i;

                            DataTable dtStepUsers = workflow2.GetStepUsers(kvp.StepID, false, false);
                            foreach (DataRow dr1 in dtStepUsers.Rows)
                            {
                                if (!dr["StepUsers"].ToString().Contains(dr1["Name"].ToString()))
                                    dr["StepUsers"] += "<nobr>" + dr1["Name"] + "</nobr><br>";
                            }
                        }
                    }
                }

                if (joinUsers && !string.IsNullOrEmpty(dr["userName"].ToString()))
                    dr["AssignedToFriendlyName"] = tools.GetUserName(dr["userName"].ToString());
                dr["Type_Friendly"] = CWF_HelpDesk.WOType.getWOType(dr["Type"].ToString());
                dr["Response_Friendly"] = CWF_HelpDesk.Response.getReponse(dr["Response"].ToString());
                dr["Condition_Friendly"] = CWF_HelpDesk.Condition.getCondition(dr["Condition"].ToString());
                dr["Location"] = tools.getPlantName(dr["BP"].ToString());
            }
        }

        private void initData(bool useExcel)
        {
            DataTable dt = init();
            SetupGridView(useExcel);
            GV.DataSource = dt;
            GV.DataBind();
        }

        public void setUpDictionary()
        {
            string query = "";
            bool usingQSArgs = false;

            if (Request["storedQueryID"] != null)
            {
                StoredQueries StoredQ = new StoredQueries(URL, userID, SecCred);
                query = StoredQ.getStoredQueryString(Request["storedQueryID"]);
                query = query.Replace("[TODAY]", DateTime.Now.ToShortDateString());
            }
            else
            {
                if (Request.QueryString.Count == 0)
                    query = searchPageCookies.getCookieString().Replace(";", "&");
                else
                {
                    query = Request.QueryString.ToString();
                    usingQSArgs = true;
                }
            }

            if (query.Length > 0)
            {
                string[] tmp = query.Split('&');
                foreach (string s in tmp)
                {
                    if (s.Contains("=") == true)
                    {
                        string[] tmp1 = s.Split('=');
                        if (usingQSArgs == false)
                            queryString.Add(tmp1[0], tmp1[1]);
                        else
                        {
                            //get the value from request because it handles special characters correctly!!!!!!
                            string tmp2 = Request.QueryString[tmp1[0]];
                            tmp2 = tmp2.Replace("[TODAY]", DateTime.Now.ToShortDateString());
                            queryString.Add(tmp1[0], tmp2);
                        }
                    }
                }
            }
        }

        public DataTable init()
        {
            string tmp = "";
            string status = "";

            string SQLWhere = "1=1 ";

            if (queryString.ContainsKey("MyActiveList"))
            {
                myActiveList = true;
                SQLWhere = workflow2.getActiveWorkflows(userID);
                searchPageCookies.SetCookieSQL("MyActiveList", "1");
                Master.HeaderText = "My Active Items";
            }
            else
            {
                if (queryString.ContainsKey("MyList"))
                {
                    SQLWhere += "AND (createdBy Like '%" + userID + "%' OR RequestedBy LIKE '%" + userID + "%' OR RequestedBy LIKE '%" + userFriendlyName + "%')";
                    searchPageCookies.SetCookieSQL("MyList", "1");
                    Master.HeaderText = "My Items;";
                }

                if (queryString.ContainsKey("WFStep"))
                {
                    SQLWhere = workflow2.GetActiveSteps_SQL_WHERE(_BP, queryString["WFStep"], "");
                    Master.HeaderText = "Step=" + queryString["WFStep"];
                    searchPageCookies.SetCookieSQL("WFStep", queryString["WFStep"]);
                    joinUsers = true;
                }

                setField(ref SQLWhere, "BP", "Header.BP", true, false, true, false, "");
                setField(ref SQLWhere, "Department", "Department", true, false, true, true, "");

                if (setField(ref SQLWhere, "WFComments", "Comments", true, true, true, true, ""))
                    joinUsers = true;

                setField(ref SQLWhere, "WFNumber", "Header.FriendlyName", true, false, false, true, "");
                setField(ref SQLWhere, "Equipment", "Equipment", true, true, true, true, "");
                setField(ref SQLWhere, "ServiceRequired", "Comment", true, true, true, true, "");
                if (queryString.ContainsKey("CreatedBy"))
                {
                    tmp = getUserName(queryString["CreatedBy"]);
                    setField(ref SQLWhere, "CreatedBy", "CreatedBy", true, true, true, true, tmp);
                }

                if (queryString.ContainsKey("AssignedTo"))
                {
                    tmp = getUserName(queryString["AssignedTo"]);
                    setField(ref SQLWhere, "AssignedTo", "userName", true, false, true, true, tmp);
                    joinUsers = true;
                }

                if (queryString.ContainsKey("Status"))
                {
                    status = queryString["Status"];
                    switch (status)
                    {
                        case "0":
                            tmp = "Not Started";
                            setField(ref SQLWhere, "Status", "Status", false, false, true, true, tmp);
                            break;

                        case "1":
                            tmp = "Circulating";
                            setField(ref SQLWhere, "Status", "Status", false, false, true, true, tmp);
                            break;

                        case "2":
                            tmp = "Complete";
                            setField(ref SQLWhere, "Status", "Status", false, false, true, true, tmp);
                            break;

                        case "3":
                            tmp = "Rejected";
                            setField(ref SQLWhere, "Status", "Status", false, false, true, true, tmp);
                            break;

                        case "4":
                            tmp = "Cancelled";
                            setField(ref SQLWhere, "Status", "Status", false, false, true, true, tmp);
                            break;
                    }
                }
                //else
                //    //SQLWhere += " AND CompleteDate is null ";
                //    SQLWhere += " AND Status = 1 ";       
            }

            string SQLSelect = "SELECT distinct BP,FriendlyName, WFID,Department, Equipment, Type, Response, Condition, Originator, Comment, Status, Createdby, dateCreated, " +
                                "CompleteDate, StartedDateTime ";

            SQLSelect += ",DATEPART(day, WF_Header.CompleteDate - WF_Header.DateCreated) AS daysToComplete ";

            if (joinUsers)
                SQLSelect += ",Comments,userName ";

            string SQLFrom = "FROM Header INNER JOIN WF_Header ON WFID = WF_Header.ID  ";

            SQLFrom += " INNER JOIN WF_Step ON WF_Step.HeaderID = WF_Header.ID ";

            if (joinUsers)
                SQLFrom += "INNER JOIN WF_StepUsers ON StepID = WF_Step.ID ";

            if (myActiveList)
            {
                SQLSelect = "SELECT CASE WHEN ResponseRequired = 0 THEN 'False' WHEN ResponseRequired = 1 then 'True' END as ResponseRequired," + SQLSelect.Replace("SELECT", "");
            }

            string SQL = SQLSelect + SQLFrom + " WHERE " + SQLWhere;

            if (_debug)
                Response.Write(SQL);

            SqlDataAdapter SQL_DA = new SqlDataAdapter();
            SQL_DA.SelectCommand = new SqlCommand(SQL, sqlConn);
            DataTable dt = new DataTable();
            try
            {
                SQL_DA.Fill(dt);
                extendResults(dt);
                DataView dv = new DataView(dt);
                dv.Sort = ViewState["SortExpression"].ToString();
                dt = dv.ToTable();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + "<br>" + SQL);
                Response.End();
            }
            return dt;
        }

        private string setDateField(string urlName, string dbName, bool ToDate)
        {
            string retval = "";
            if (queryString.ContainsKey(urlName) == false || queryString[urlName].Length == 0)
                return retval;

            DateTime CheckDate = DateTime.Now;
            string strDate = queryString[urlName];
            if (!DateTime.TryParse(strDate, _culture, DateTimeStyles.None, out CheckDate))
                return retval;

            retval = " AND ";
            retval += dbName;

            if (ToDate)
                retval += " <= '" + CheckDate.ToString("MM/dd/yyyy") + " 23:59:59'";
            else
                retval += " >= '" + CheckDate.ToString("MM/dd/yyyy") + " 00:00:00'";

            searchPageCookies.SetCookieSQL(urlName, queryString[urlName]);

            return retval;
        }

        private bool setField(ref string SQLWhere, string urlName, string dbName, bool varchar, bool like, bool addToCookies, bool addtoHeaderText, string HeaderTextOverride)
        {
            if (queryString.ContainsKey(urlName) == false || queryString[urlName].Length == 0)
                return false;

            string retval = "";
            if (SQLWhere.Length > 0)
                retval = " AND ";

            if (like)
            {
                string tmp1 = "";
                string[] tmp = queryString[urlName].Split('+', ' ');
                foreach (string s in tmp)
                {
                    string s1 = s.Replace("'", "''");
                    tmp1 += dbName + " LIKE '%" + s1 + "%' AND ";
                }
                if (tmp1.Length > 0)
                {
                    tmp1 = tmp1.Substring(0, tmp1.Length - 4);
                    retval += tmp1;
                }
            }
            else
            {
                retval += dbName;

                string t = queryString[urlName].Replace("'", "''");
                t = t.Replace('+', ' ');

                if (t.StartsWith("<") || t.StartsWith(">"))
                {
                    retval += t.Substring(0, 1);
                    if (t.Substring(1, 1) == "=")
                    {
                        retval += t.Substring(1, 1);
                        t = t.Substring(2);
                    }
                    else
                        t = t.Substring(1);
                }
                else
                    retval += "= ";

                if (varchar)
                    retval += "'" + t + "'";
                else
                    retval += t;
            }

            if (addToCookies)
                searchPageCookies.SetCookieSQL(urlName, queryString[urlName]);

            if (addtoHeaderText)
            {
                if (HeaderTextOverride.Length == 0)
                    Master.HeaderText += urlName + " = " + queryString[urlName] + " ; ";
                else
                    Master.HeaderText += urlName + " = " + HeaderTextOverride + " ; ";
            }

            SQLWhere += retval;
            return true;
        }

        public string getUserName(object name)
        {
            return tools.GetUserName(name.ToString());
        }

        private void SetupGridView(bool useExcel)
        {
            ITemplate template;
            BoundField bf;

            GV.Columns.Clear();

            if (myActiveList)
            {
                GV.Columns.Add(buildBoundField("Response Required", "responserequired", 0, true, ""));
            }

            List<SelectedColumns.Column> lstSelColumns = null;
            if (useExcel)
            {
                SelectedColumns.Excel ColsSearch = new SelectedColumns.Excel(myCook.GetCookieSQL("SearchResultsExcel"));
                lstSelColumns = ColsSearch.getSelectedColumns();
            }
            else
            {
                SelectedColumns.Search ColsSearch = new SelectedColumns.Search(myCook.GetCookieSQL("SearchResultsSearch"));
                lstSelColumns = ColsSearch.getSelectedColumns();
            }

            foreach (SelectedColumns.Column c in lstSelColumns)
            {

                switch (c.dbFieldName)
                {
                    case "friendlyName":
                        string args = "";
                        string location = "";
                        template = new GridViewTemplate_LinkButton_WF(location, _webServer + _appPath, args);
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "DateCreated":
                    case "StartedDateTime":
                    case "StepStartTime":
                        template = new GridViewTemplate_LabelDate(c.HeaderText, c.dbFieldName, _culture);
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "CreatedBy":
                    case "StartedBy":
                        template = new GridViewTemplate_LabelUser(c.HeaderText, c.dbFieldName, AU);
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "Status":
                        template = new GridViewTemplate_Status(tools, workflow2);
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "onTime":
                        template = new GridViewTemplate_Boolean(c.dbFieldName, GridViewTemplate_Boolean.DisplayValue.yesNo, true, false);
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "AssignedTo":
                        if (joinUsers)
                        {
                            template = new GridViewTemplate_HTML("AssignedToFriendlyName");
                            GV.Columns.Add(makeLabelTemplate(false, c, template));
                        }
                        break;

                    case "Type":
                        template = new GridViewTemplate_HTML("Type_Friendly");
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "Condition":
                        template = new GridViewTemplate_HTML("Condition_Friendly");
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "Response":
                        template = new GridViewTemplate_HTML("Response_Friendly");
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "Comment":
                    case "StepDescription":
                    case "stepUsers":
                    case "DaysInQueue":
                        template = new GridViewTemplate_HTML(c.dbFieldName);
                        c.wrap = true;
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    case "Comments":
                        if (joinUsers)
                        {
                            template = new GridViewTemplate_HTML(c.dbFieldName);
                            c.wrap = true;
                            GV.Columns.Add(makeLabelTemplate(false, c, template));
                        }
                        break;

                    case "Department":
                    case "Equipment":
                    case "Originator":
                        template = new GridViewTemplate_HTML(c.dbFieldName);
                        GV.Columns.Add(makeLabelTemplate(false, c, template));
                        break;

                    default:
                        bf = buildBoundField(c, 0);
                        if (c.HeaderText == "Comment")
                            bf.ItemStyle.Wrap = true;
                        GV.Columns.Add(bf);
                        break;
                }
            }
        }

        private void ExportTableExcel()
        {
            GV.PageSize = 10000;

            initData(true);

            GridViewExport gvx = new GridViewExport("MaintHelpDesk_Search.XLS", GV);
            Response.End();
        }

        #region //************************** EVENTS *****************************/
        protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void GV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV.PageIndex = e.NewPageIndex;
            initData(false);
        }

        private void PageSelect()
        {
            string index = Request["__EVENTARGUMENT"];
            GV.PageIndex = int.Parse(index) - 1;
            initData(false);
        }

        protected void GV_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GV_RowCreated2(sender, e);
        }

        protected void GV_Sorting(Object sender, GridViewSortEventArgs e)
        {
            string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');
            // If the sorting column is the same as the previous one,  
            // then change the sort order. 
            if (strSortExpression[0] == e.SortExpression)
            {
                if (strSortExpression[1] == "ASC")
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
                }
            }
            // If sorting column is another column,   
            // then specify the sort order to "Ascending". 
            else
            {
                ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
            }

            // Save Sort order in cookies
            strSortExpression = ViewState["SortExpression"].ToString().Split(' ');
            myCook.SetCookieSQL("SearchResultsSortField", strSortExpression[0]);
            myCook.SetCookieSQL("SearchResultsSortOrder", strSortExpression[1]);

            initData(false);
        }

        //int GetSortColumnIndex(GridView GV, string sortExpression)
        //{
        //    foreach (DataControlField field in GV.Columns)
        //    {
        //        if (field.SortExpression == sortExpression)
        //        {
        //            return GV.Columns.IndexOf(field);
        //        }
        //    }
        //    return -1;
        //}

        //private void addSortImage(GridView gv, GridViewRow headerRow)
        //{
        //    string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');
        //    int sortColumnIndex = GetSortColumnIndex(gv, strSortExpression[0]);
        //    if (sortColumnIndex != -1)
        //    {
        //        Image sortImage = new Image();
        //        if (strSortExpression[1] == "ASC")
        //        {
        //            sortImage.ImageUrl = "images/Move Up (Arrow)16.png";
        //            sortImage.AlternateText = "Ascending Order";
        //        }
        //        else
        //        {
        //            sortImage.ImageUrl = "images/Move Down (Arrow)16.png";
        //            sortImage.AlternateText = "Descending Order";
        //        }
        //        headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
        //    }
        //}

        //public void SetPagerButtonStates(GridView gridView, GridViewRow gvPagerRow, string sortExpression)
        //{
        //    int pageIndex = gridView.PageIndex;
        //    int pageCount = gridView.PageCount;

        //    ImageButton imgFirst = (ImageButton)gvPagerRow.FindControl("imgFirst");
        //    ImageButton imgPrevious = (ImageButton)gvPagerRow.FindControl("imgPrevious");
        //    ImageButton imgNext = (ImageButton)gvPagerRow.FindControl("imgNext");
        //    ImageButton imgLast = (ImageButton)gvPagerRow.FindControl("imgLast");

        //    imgFirst.Enabled = imgPrevious.Enabled = (pageIndex != 0);
        //    imgNext.Enabled = imgLast.Enabled = (pageIndex < (pageCount - 1));

        //    DropDownList ddlPageSelector = (DropDownList)gvPagerRow.FindControl("ddlPageSelector");
        //    ddlPageSelector.Attributes.Add("onchange", "fnPageSelect(this);");
        //    ddlPageSelector.Items.Clear();

        //    DataTable dt = (DataTable)gridView.DataSource;
        //    for (int i = 1; i <= gridView.PageCount; i++)
        //    {
        //        string v = i.ToString();
        //        if (dt != null)
        //        {
        //            string s = "";
        //            s = dt.Rows[(i - 1) * gridView.PageSize][sortExpression].ToString();
        //            if (s.Trim().Length > 15)
        //                s = s.Substring(0, 15) + "..";
        //            v = s + " (" + i.ToString() + ")";
        //        }
        //        ddlPageSelector.Items.Add(new ListItem(v, i.ToString()));
        //    }

        //    ddlPageSelector.SelectedIndex = pageIndex;
        //    gridView.SelectedIndex = pageIndex;
        //}

        #endregion

    }
}