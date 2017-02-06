using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using CWF_Security;
using CWF_Corporate;
using CWF_HelpDesk;
using CWF_SQLCookies;
using CWF_WorkflowRouting;

namespace QA
{
    public class BaseClass : System.Web.UI.Page
    {
        protected WebSecure WSSecurity;
        protected SecurityCredentials SecCred;
        protected string URL = "/HelpDesk/search.aspx";

        protected string userID = "";
        protected string userFriendlyName = "";
        protected string userEmail = "";

        protected Tools2 tools;
        protected Workflow2 workflow2;
        protected Authentication AU;
        protected bool _usesAD = true;
        protected DataTable myHeaderDT;
               
        protected string _dbName = "";        
        protected string _appFriendlyName = "";
        protected string _webServer = "";
        protected string _webConfig_WebServer = "";
        protected bool _isMailMessage = false;
        
        protected bool _debug = false;

        protected string _BP = "";
        protected string workflowID = "";                
        protected string PageTitle = "";
        protected string _appPath = "";
        
        protected bool _isAdmin = false;
        protected bool _isOwner = false;
        
        protected string _ServerModeOptions = "";               
        
        protected string _stepID = "0";
        protected string _stepUserID = "";
        protected Routing2.DBStatus _DBStatus;
        
        protected SqlConnection sqlConn;
        
        public SQLCookies myCook;        
        protected CultureInfo _culture;
        protected NumberFormatInfo _nfi;

        protected override void OnInit(EventArgs e)
        {
            //_debug = true;                        

            SecCred = new SecurityCredentials();
            
            if (System.Configuration.ConfigurationManager.AppSettings["usesAD"] != null)
                _usesAD = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["usesAD"]);

            AU = new Authentication(SecCred, _usesAD);

            if (_usesAD)
            {
                WSSecurity = new WebSecure(Response, Request, Session);
                userID = WSSecurity.AuthenticateUser();
                if (userID.Length > 0)
                {
                    Profile p = AU.GetUserProfile(userID);
                    userFriendlyName = p.friendlyName;
                    userEmail = p.email;
                }
            }
            else
            {
                userID = AU.getPortalCookie(Request, Response);
                if (userID.Length == 0)
                {
                    if (!Request.ServerVariables["PATH_INFO"].Contains("Login"))
                        Response.Redirect("Login/default.aspx");
                }
            }
            
            _webServer = this.Request.Url.Authority;
            _webConfig_WebServer = System.Configuration.ConfigurationManager.AppSettings["WebServer"];
            _appPath = Request.ApplicationPath;

            _BP = Request["BP"] + "";
            if (_BP == "")
            {
                _BP = "1100";
            }

            workflowID = Request["workflowID"] + "";
            if (workflowID.Length == 0 || workflowID == "-1")
                workflowID = "0";            

            _stepID = Request["stepID"];
            _stepUserID = Request["stepUserID"] + "";

            _dbName = System.Configuration.ConfigurationManager.AppSettings["dbName"];            
            _appFriendlyName = System.Configuration.ConfigurationManager.AppSettings["AppFriendlyName"];
            
            tools = new Tools2(_BP, userID, SecCred, _usesAD);

            _culture = UserCulture();
            _nfi = (NumberFormatInfo)_culture.NumberFormat.Clone();
            _nfi.CurrencySymbol = "$";

            string filename = Server.MapPath("~/CSS/osx2_1.css");
            StreamReader sr = File.OpenText(filename);
            string s = sr.ReadToEnd();

            workflow2 = new Workflow2(SecCred, _BP, userID, _culture, s, 700, _usesAD);    
            

            dbServerModeStruct sms = SecCred.GetServerMode(Request.Url.Host);
            _ServerModeOptions = sms.Options;

            if (System.Configuration.ConfigurationManager.AppSettings["RoutingDebugStatus"] != null && sms.Mode != "P")
            {
                workflow2.debug = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["RoutingDebugStatus"]);
                workflow2.debugUsers = System.Configuration.ConfigurationManager.AppSettings["RoutingDebugEmail"];                
            }

            _DBStatus = Routing2.DBStatus.NotStarted;                       
            
            myHeaderDT = tools.getHeader(workflowID, false);            
            if (myHeaderDT.Rows.Count > 0)
            {
                if (myHeaderDT.Rows[0]["BP"].ToString() != _BP)
                    _BP = myHeaderDT.Rows[0]["BP"].ToString();

                tools.BP = _BP;
                workflow2._BP = _BP;

                _isOwner = tools.IsOwner(workflowID);
                _DBStatus = workflow2.WorkflowStatusEnum((int)myHeaderDT.Rows[0]["Status"]);                
            }            

            PageTitle = _appFriendlyName;
            
            _isAdmin = tools.isAdmin(_BP);

            //if (Request["Edit"] + "" == "1" || Request["Edit"] + "" == "True" && (_isOwner || _isAdmin))// _hasModifyRights)
              //  _editRequest = true;
                 
            myCook = new SQLCookies(_appFriendlyName, userID, SecCred);            
            
            sqlConn = new SqlConnection();
            sqlConn.ConnectionString = SecCred.ConnectionStringSQL(_dbName);            
            
            base.OnInit(e);
        }       
                          

        #region Error Handling
        protected void SetErrorMsg(string Msg, HiddenField hdnErrorMsg)
        {
            // Display Error message on client side in message box
            hdnErrorMsg.Value = Msg;
        }
        #endregion

        #region //*********************** WORKFLOW STEP SUPPORT ********************************/       
        public void deleteAllStepUsers(int stepID)
        {
            DataTable dtStepUsers = workflow2.GetStepUsers(stepID, false, true);
            foreach (DataRow dr in dtStepUsers.Rows)
            {
                workflow2.DeleteStepUser(dr["ID"].ToString(), false);
            }
        }

        public void addWorkflowUser(int StepID, string userName, bool responseRequired)
        {
            DataTable dt = workflow2.GetStepUser("-1", false, true);
            DataRow newRow = dt.NewRow();
            newRow["stepID"] = StepID;
            newRow["userName"] = userName;
            newRow["responseRequired"] = responseRequired;
            newRow["Rejected"] = 0;
            newRow["SignedOff"] = 0;
            newRow["DateTimeStamp"] = DateTime.Now.ToShortDateString();
            newRow["LastModifiedBy"] = userID;
            newRow["RoleBasedUser"] = 0;
            dt.Rows.Add(newRow);
            workflow2.updateUsers(dt, false);
        }

        public bool GetTemplateSteps(string containerName)
        {
            DataTable dt = GetTemplateTreeSteps(containerName);
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }

        private DataTable GetTemplateTreeSteps(string name)
        {
            DataTable dt = new DataTable();
            int[] stepCG = workflow2.GetStepsByDescription((int)myHeaderDT.Rows[0]["WFID"], name, false);
            if (stepCG.GetUpperBound(0) >= 0)
            {
                return workflow2.GetTreeSteps(stepCG[0], false);
            }
            else
                return dt;
        }

        public void addToWorkflow(string placeHolderSourceName, string placeholderDestinationName, string templateName)
        {
            DataTable dt1 = GetTemplateTreeSteps(placeHolderSourceName);

            int[] placeHolder = workflow2.GetStepsByDescription((int)myHeaderDT.Rows[0]["WFID"], placeholderDestinationName, false);
            DataTable dtTemplate = workflow2.GetTemplate(_BP, templateName);
            if (placeHolder.GetUpperBound(0) >= 0 && dtTemplate.Rows.Count > 0)
            {
                DataTable dt = workflow2.GetStep(placeHolder[0], false);
                workflow2.CopyTemplateToWorkflow((int)dtTemplate.Rows[0]["ID"], (int)myHeaderDT.Rows[0]["WFID"], (int)dt.Rows[0]["StepNumber"] + 1, placeHolder[0]);
            }
        }

        public void ClearWorkflowContainerSteps(string stepDescription)
        {
            int[] stepCG = workflow2.GetStepsByDescription((int)myHeaderDT.Rows[0]["WFID"], stepDescription, false);
            foreach (int i in stepCG)
            {
                DataTable dtAG = workflow2.GetTreeSteps(i, false);
                foreach (DataRow dr in dtAG.Rows)
                    workflow2.DeleteStep((int)dr["ID"], false);
            }
        }

        public void SendRejectEmail(string comments)
        {            
            string FN;
            string ccTo;
            workflow2.getProfileAttributes(myHeaderDT.Rows[0]["createdBy"].ToString(), out FN, out ccTo);            

            if (workflow2.debug)
                ccTo = workflow2.debugUsers;

            if (ccTo.Length > 0)
                workflow2.SendRejectEmail(int.Parse(workflowID), int.Parse(_stepID), ccTo, userFriendlyName, comments);
        }

        public void CheckFinalStepAction(DataTable dtStep)
        {
            if (workflow2.WorkflowStatus(int.Parse(workflowID)) == CWF_WorkflowRouting.Routing2.DBStatus.Complete)
            {
                if (dtStep.Rows[0]["FinalStepAction"] != DBNull.Value)
                {
                    switch (dtStep.Rows[0]["FinalStepAction"].ToString().ToLower())
                    {
                        case "emailall":
                            workflow2.SendCompletedEmail_AllUsers(int.Parse(workflowID));
                            break;

                        case "emailowner":
                            workflow2.SendCompletedEmail_CreatorOnly(int.Parse(workflowID));
                            break;
                    }
                }
            }
        }

        public static string addComment(ref string comments, string comment, string UserID)
        {
            if (comments.Length > 0 && comment.Length > 0)
                comments += "\r\n";
            if (comment.Length > 0)
            {
                comments += comment;
                if (UserID.Length > 0)
                    comments += " - (" + UserID + " - " + DateTime.Now.ToString() + ")";
            }

            if (comments.Length > 8000)
            {
                return "Comment max length has been reached!\nPlease contact IT";                
            }
            return "";
        }
        #endregion

        #region //********************** CULTURE *******************************/
        public CultureInfo UserCulture()
        {
            CultureInfo retval = new CultureInfo("en-US");
            try
            {
                if (Request.UserLanguages != null)
                    retval = CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);
            }
            catch
            {
            }
            return retval;
        }

        public static CultureInfo UserCulture(string Name)
        {
            CultureInfo retval = new CultureInfo("en-US");
            try
            {
                retval = CultureInfo.CreateSpecificCulture(Name);
            }
            catch
            {
            }
            return retval;
        }

        public string getFormattedDate(object date)
        {
            if (date == null)
                return "";

            DateTime datetime = DateTime.Now;
            if (!DateTime.TryParse(date.ToString(), out datetime))
                return "";

            return datetime.ToString("d", _culture);
        }

        public string getFormattedDateTime(object date)
        {
            if (date == null)
                return "";

            DateTime datetime = DateTime.Now;
            if (!DateTime.TryParse(date.ToString(), out datetime))
                return "";

            return datetime.ToString(_culture);
        }
        #endregion

        public DataRow cloneRow(DataTable table, DataRow clonedRow)
        {
            ArrayList keys = new ArrayList(table.PrimaryKey);
            DataRow newRow = table.NewRow();
            foreach (DataColumn col in table.Columns)
            {
                if (!keys.Contains(col))
                    newRow[col.ColumnName] = clonedRow[col.ColumnName];
            }
            return newRow;
        }

        #region //************************ Common methods ******************************/

        #endregion       

        #region //****************************** LANGUAGE ************************************/
       

       
        #endregion

        public void validateUser(string userID, out string userName, out bool isMissing, out string errorMsg)
        {
            isMissing = false;
            userName = "";
            errorMsg = "";
            string myUserID = userID.Trim();
            if (myUserID.Length == 0)
            {
                errorMsg = "Name not recognized!";
                return;
            }
            var pro = tools.GetUserName(myUserID);
            if (pro == userID)
            {
                errorMsg = myUserID + " not recognized. No spaces/commas allowed in unrecognized names!";
                isMissing = true;
            }
            else
                userName = pro;
        }

        //public string getPlantName(string bp)
        //{
        //    if (bp == "0")
        //        return "All Plants";
        //    string retval = "";
        //    CWF_Corporate.Plants p = new CWF_Corporate.Plants();
        //    DataTable d = p.BranchPlant(bp);
        //    if (d.Rows.Count > 0)
        //        retval = d.Rows[0]["Name"].ToString();
        //    return retval;
        //}

        //public string getPlantName()
        //{
        //    return getPlantName(_BP);
        //}

        public void BuildBP(DropDownList ddl_BP)
        {
            //CWF_Corporate.Plants plants = new CWF_Corporate.Plants(SecCred);
            //DataTable dt = plants.BranchPlantsBOM();
            DataTable dt = getLocations();
            foreach (DataRow dr in dt.Rows)
            {
                ListItem lt = new ListItem();
                lt.Text = dr["Name"].ToString();
                lt.Value = dr["BP"].ToString();
                if (_BP == dr["BP"].ToString())
                    lt.Selected = true;
                ddl_BP.Items.Add(lt);
            }
        }

        public DataTable getLocations()
        {
            try
            {
                OpenConn();
                DataTable dt = new DataTable();
                dt.Columns.Add("Location");
                dt.Columns.Add("BP");
                string SQL = "Select * FROM Sites WHERE Active <> 0";
                SqlDataReader sdr;
                SqlCommand sqlComm = new SqlCommand(SQL, sqlConn);
                sdr = sqlComm.ExecuteReader();
                while (sdr.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["Location"] = sdr["Location"].ToString();
                    dr["BP"] = sdr["BP"].ToString();
                    dt.Rows.Add(dr);
                }
                sdr.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConn();
            }
        }

        public bool BPisValid()
        {
            DataTable dt = getLocations();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["BP"].ToString() == _BP)
                    return true;
            }
            return false;
        }

        public static Dictionary<string, string> parseDictionaryString(string myValues)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string[] t = myValues.Split('~');
            foreach (string s in t)
            {
                string[] s1 = s.Split('=');
                if (s1.GetUpperBound(0) > 0)
                    values.Add(s1[0], s1[1]);
            }
            return values;
        }

        public void SetStatus(string vMsg, Label BlockLabel)
        {
            if (vMsg.Length > 0)
            {
                int x = 75;
                if (vMsg.Length < x)
                    x = vMsg.Length;
                vMsg = vMsg.Substring(0, x);
                
                //if (vMsg.Length > 1)
                BlockLabel.BackColor = System.Drawing.Color.Red;
                //else

                BlockLabel.ForeColor = System.Drawing.Color.White;
                BlockLabel.Text = vMsg + " .... ";
            }
            else
            {
                BlockLabel.Text = "";
                BlockLabel.BackColor = System.Drawing.Color.Transparent;
            }
        }

        public void SetStatus(string vMsg, Label BlockLabel, System.Drawing.Color forecolor)
        {
            if (vMsg.Length > 0)
            {
                int x = 75;
                if (vMsg.Length < x)
                    x = vMsg.Length;
                vMsg = vMsg.Substring(0, x);

                BlockLabel.ForeColor = forecolor;
                BlockLabel.Text = vMsg + " .... ";
            }
            else
            {
                BlockLabel.Text = "";
            }
        }

        public int getIntFromHex(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }

        #region //******************************** GRID VIEW SUPPORT **********************************/       
        public TemplateField makeLabelTemplate(bool makeFooter, SelectedColumns.Column col, ITemplate template)
        {
            TemplateField tempField = new TemplateField();
            tempField.HeaderText = col.HeaderText;
            if (col.dbSortFieldName.Length > 0)
            {
                tempField.SortExpression = col.dbSortFieldName;
                tempField.HeaderTemplate = new GridViewTemplate_Anchor("lbSort_" + col.dbSortFieldName, col.HeaderText, col.toolTip);
            }
            tempField.HeaderStyle.Wrap = col.wrap;
            tempField.HeaderStyle.HorizontalAlign = col.horizontalAlign;
            tempField.HeaderStyle.Font.Bold = false;
            tempField.HeaderStyle.CssClass = "SP_dataLabel_Header";

            tempField.ItemStyle.Wrap = col.wrap; // false;
            tempField.ItemStyle.CssClass = "SP_dataLabel";
            tempField.ItemStyle.HorizontalAlign = col.horizontalAlign;
            tempField.ItemTemplate = template; // new GridViewTemplate_Label(col.dbFieldName);

            if (makeFooter)
            {
                tempField.FooterStyle.CssClass = "SP_dataLabel";
                tempField.FooterStyle.Wrap = false;
                tempField.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                tempField.FooterTemplate = new GridViewTemplate_FooterLabel("sum" + col.dbFieldName);
            }
            return tempField;
        }

        public BoundField buildBoundField(string header, string dataField, int width, bool wrap, string dataFormat)
        {
            BoundField bf = new BoundField();
            bf.DataField = dataField;

            bf.HeaderText = header;
            //bf.HeaderStyle.Font.Name = "Tahoma";
            bf.HeaderStyle.CssClass = "SP_dataLabel_Header";
            bf.HeaderStyle.Wrap = false;
            bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;

            bf.ItemStyle.Font.Size = FontUnit.Point(8);
            bf.ItemStyle.CssClass = "SP_dataLabel";
            if (width > 0)
                bf.ItemStyle.Width = Unit.Percentage(width);
            bf.ItemStyle.Wrap = wrap;
            if (dataFormat.Length > 0)
                bf.DataFormatString = dataFormat;
            return bf;
        }

        public BoundField buildBoundField(SelectedColumns.Column col, int width)
        {
            BoundField bf = new BoundField();
            bf.DataField = col.dbFieldName;

            bf.HeaderText = col.HeaderText;
            bf.HeaderStyle.CssClass = "SP_dataLabel_Header";
            bf.HeaderStyle.Wrap = false;
            bf.HeaderStyle.HorizontalAlign = col.horizontalAlign;

            bf.ItemStyle.Font.Size = FontUnit.Point(8);
            bf.ItemStyle.CssClass = "SP_dataLabel";
            if (width > 0)
                bf.ItemStyle.Width = Unit.Percentage(width);
            bf.ItemStyle.Wrap = col.wrap;
            bf.ItemStyle.HorizontalAlign = col.horizontalAlign;
            if (col.dataFormat.Length > 0)
                bf.DataFormatString = col.dataFormat;
            return bf;
        }

        public void GV_RowCreated2(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView gv = (GridView)sender;
                List<Control> controls = FindControls(e.Row, "lbSort_");
                foreach (Control c in controls)
                {
                    string[] tmp = c.UniqueID.Split('_');
                    ((HtmlAnchor)c).Attributes.Add("href", "javascript:fnSort('" + gv.UniqueID + "','Sort$" + tmp[1] + "')");
                }
                addSortImage(gv, e.Row);
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');
                SetPagerButtonStates((GridView)sender, e.Row, strSortExpression[0]);
            }
        }

        public void SetPagerButtonStates(GridView gridView, GridViewRow gvPagerRow, string sortExpression)
        {
            int pageIndex = gridView.PageIndex;
            int pageCount = gridView.PageCount;

            ImageButton imgFirst = (ImageButton)gvPagerRow.FindControl("imgFirst");
            ImageButton imgPrevious = (ImageButton)gvPagerRow.FindControl("imgPrevious");
            ImageButton imgNext = (ImageButton)gvPagerRow.FindControl("imgNext");
            ImageButton imgLast = (ImageButton)gvPagerRow.FindControl("imgLast");

            imgFirst.Enabled = imgPrevious.Enabled = (pageIndex != 0);
            imgNext.Enabled = imgLast.Enabled = (pageIndex < (pageCount - 1));

            DropDownList ddlPageSelector = (DropDownList)gvPagerRow.FindControl("ddlPageSelector");
            ddlPageSelector.Attributes.Add("onchange", "fnPageSelect(this);");
            ddlPageSelector.Items.Clear();

            DataTable dt = (DataTable)gridView.DataSource;
            for (int i = 1; i <= gridView.PageCount; i++)
            {
                string v = i.ToString();
                if (dt != null)
                {
                    string s = "";
                    System.Type st = dt.Columns[sortExpression].DataType;
                    if (st == System.Type.GetType("System.Decimal"))
                    {
                        //Response.Write(st.ToString());
                        s = string.Format(_culture, "{0:N0}", dt.Rows[(i - 1) * gridView.PageSize][sortExpression]);
                    }
                    else
                    {
                        s = dt.Rows[(i - 1) * gridView.PageSize][sortExpression].ToString();
                        if (s.Trim().Length > 15)
                            s = s.Substring(0, 15) + "..";
                    }
                    v = s + " (" + i.ToString() + ")";
                }
                ddlPageSelector.Items.Add(new ListItem(v, i.ToString()));
            }

            ddlPageSelector.SelectedIndex = pageIndex;
            gridView.SelectedIndex = pageIndex;
        }

        public int GetSortColumnIndex(GridView GV, string sortExpression)
        {
            // Iterate through the Columns collection to determine the index of the column being sorted.
            foreach (DataControlField field in GV.Columns)
            {
                if (field.SortExpression == sortExpression)
                {
                    return GV.Columns.IndexOf(field);
                }
            }
            return -1;
        }

        public void addSortImage(GridView gv, GridViewRow headerRow)
        {
            string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');
            int sortColumnIndex = GetSortColumnIndex(gv, strSortExpression[0]);
            if (sortColumnIndex != -1)
            {
                Image sortImage = new Image();
                if (strSortExpression[1] == "ASC")
                {
                    sortImage.ImageUrl = "images/Move Up (Arrow)16.png";
                    sortImage.AlternateText = "Ascending Order";
                }
                else
                {
                    sortImage.ImageUrl = "images/Move Down (Arrow)16.png";
                    sortImage.AlternateText = "Descending Order";
                }
                headerRow.Cells[sortColumnIndex].Controls.Add(sortImage);
            }
        }
        #endregion

        private List<Control> FindControls(Control root, string startsWith)
        {
            List<Control> controls = new List<Control>();
            FindControlsRecursive(ref controls, root, startsWith);
            return controls;

        }

        public void FindControlsRecursive(ref List<Control> controls, Control Root, string startsWith)
        {
            //if (Root.ID == Id)
            //  return Root;
            foreach (Control Ctl in Root.Controls)
            {
                if (Ctl != null)
                {
                    System.Diagnostics.Debug.WriteLine(Ctl.UniqueID + ":" + Ctl.ID + ":" + Ctl.ClientID);
                    //FindControlRecursive(Ctl, Id);
                    if (Ctl.UniqueID.Contains(startsWith))
                        controls.Add(Ctl);
                    else
                        FindControlsRecursive(ref controls, Ctl, startsWith);
                }
            }
        }

        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;
            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }
            return null;
        }

        public string removeFrom_QueryString(string[] names)
        {
            bool useValue;
            string retval = "";
            foreach (string s in Request.QueryString.AllKeys)
            {
                useValue = true;
                foreach (string s1 in names)
                {
                    if (s.ToUpper() == s1.ToUpper())
                        useValue = false;
                }
                if (useValue)
                    retval += "&" + s + "=" + Request.QueryString[s];
            }
            retval = retval.Substring(1);
            return retval;
        }

        #region //**************************** METRICS SUPPORT *************************************/
        public PlaceHolder buildStatistics(DataTable dt, int rollingDays, string BP, string stepUsers)
        {
            GridView gv = MakeGrid(0);

            SelectedColumns.Column col = new SelectedColumns.Column("onTimePercent", "", "On Time(%)", false, HorizontalAlign.Right, 0);
            col.dataFormat = "{0:N0}";
            gv.Columns.Add(buildBoundField(col, 0));

            col = new SelectedColumns.Column("pastDue", "", "Past Due", false, HorizontalAlign.Right, 0);
            gv.Columns.Add(buildBoundField(col, 0));

            col = new SelectedColumns.Column("dueToday", "", "Due Today", false, HorizontalAlign.Right, 0);
            gv.Columns.Add(buildBoundField(col, 0));

            col = new SelectedColumns.Column("dueThisWeek", "", "Due This Week", false, HorizontalAlign.Right, 0);
            gv.Columns.Add(buildBoundField(col, 0));

            col = new SelectedColumns.Column("avgDaysToComplete", "", "Avg Days to Complete", false, HorizontalAlign.Right, 0);
            col.dataFormat = "{0:N2}";
            gv.Columns.Add(buildBoundField(col, 0));

            col = new SelectedColumns.Column("numberCompleted", "", "Number Completed", false, HorizontalAlign.Right, 0);
            gv.Columns.Add(buildBoundField(col, 0));

            col = new SelectedColumns.Column("avgStepDaysToComplete", "", "Avg Days to Sign Off", false, HorizontalAlign.Right, 0);
            col.dataFormat = "{0:N0}";
            gv.Columns.Add(buildBoundField(col, 0));

            gv.DataSource = dt;
            gv.DataBind();

            PlaceHolder phMetrics = new PlaceHolder();

            phMetrics.Controls.Add(gv);
            return phMetrics;
        }

        public GridView MakeGrid(int width)
        {
            GridView gv = new GridView();
            if (width > 0)
                gv.Width = Unit.Percentage(width);
            gv.AutoGenerateColumns = false;
            gv.CellPadding = 4;
            gv.BorderWidth = Unit.Pixel(1);
            //gv.RowDataBound += new GridViewRowEventHandler(GV_AffectedItems_RowDataBound_ViewOnly);
            gv.RowStyle.BackColor = System.Drawing.Color.FromArgb(getIntFromHex("eb"), getIntFromHex("f3"), getIntFromHex("ff"));
            gv.HeaderStyle.CssClass = "SP_dataLabel_Header";
            return gv;
        }
        
        public void parseUsers(string users, out string SQLFilter, out string friendlyNames)
        {
            SQLFilter = "";
            friendlyNames = "";
            string tmp = "";
            string userNames = "";
            string[] tmp1 = users.Split(',');
            foreach (string s in tmp1)
            {
                tmp += "'" + s + "',";
                userNames += tools.GetUserName(s) + ",";
            }
            tmp = tmp.Substring(0, tmp.Length - 1);
            userNames = userNames.Substring(0, userNames.Length - 1);
            SQLFilter = tmp;
            friendlyNames = userNames;
        }
        #endregion

        protected void OpenConn()
        {
            sqlConn.Open();
        }

        protected void CloseConn()
        {
            sqlConn.Close();
        }
    }
}
