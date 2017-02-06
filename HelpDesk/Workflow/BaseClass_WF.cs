using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using CWF_Security;
using CWF_SQLCookies;
using CWF_WorkflowRouting;
using CWF_HelpDesk;
using CWF_Corporate;

namespace WF
{
    public class BaseClass_WF_Page : Page
    {
        public BaseClassWF wf;        

        protected override void OnInit(EventArgs e)
        {
            WebSecure WSSecurity = new WebSecure(Response, Request, Session);
            
            wf = new BaseClassWF(Request, WSSecurity, Server, Response);            

            base.OnInit(e);            
        }                     
    }

    public class BaseClass_WF_UC : UserControl
    {
        public BaseClassWF wf;        

        protected override void OnInit(EventArgs e)
        {
            WebSecure WSSecurity = new WebSecure(Response, Request, Session);
                        
            wf = new BaseClassWF(Request, WSSecurity, Server, Response);
            base.OnInit(e);
        }
    }

    public class BaseClassWF
    {        
        public SecurityCredentials SecCred;
        protected WebSecure _WSSecurity;

        public bool _debug = false;
        public string _userID = "";
        public bool _usesAD = true;

        public Workflow2 workflow2;
        public Authentication AU;
        public SQLCookies LangCookies;
        public CultureInfo culture;
        private HttpServerUtility Server;

        public string _workflowID = "0";
        
        public string _stepID = "0";
        public string _databaseName = "";        
        public string _appFriendlyName = "";
        
        public string _BP = "";                
        public string _webServer = "";
        public bool _isMailMessage = false;
        public string _appPath = "";

        public bool _isOwner = false;
        public bool _isAdmin = false;
        public bool _isSignOffVisible = false;
        public Workflow2.DBStatus _DBStatus;        
        
        public bool _enableTemplateMenu = false;
        public bool _enableStepMenu = false;

        public BaseClassWF(HttpRequest Request, WebSecure WSSecurity, HttpServerUtility httpServer, HttpResponse Response)
        {
            SecCred = new SecurityCredentials();

            if (System.Configuration.ConfigurationManager.AppSettings["usesAD"] != null)
                _usesAD = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["usesAD"]);

            AU = new Authentication(SecCred, _usesAD);

            if (_usesAD)
            {
                _WSSecurity = WSSecurity;
                _userID = _WSSecurity.AuthenticateUser();
            }
            else
            {
                _userID = AU.getPortalCookie(Request, Response);
            }            
            
            Server = httpServer;

            _webServer = Request.Url.Authority;
            SecCred.WebServer = _webServer;
            
            _BP = Request["BP"];
                                    
            _stepID = Request["stepID"];
            if (Request["mailMessage"] != null)
                _isMailMessage = true;            
            
            _appFriendlyName = System.Configuration.ConfigurationManager.AppSettings["AppFriendlyName"];
            _databaseName = System.Configuration.ConfigurationManager.AppSettings["dbName"];
            //tools = new Tools2(_BP, userID, SecCred, _usesAD);

            culture = new CultureInfo("en-US");
            try
            {
                if (Request.UserLanguages != null)
                    culture = CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);
            }
            catch
            { }           

            _appPath = Request.ApplicationPath;
            _workflowID = Request["workflowID"] + "";
            if (_workflowID.Length == 0 || _workflowID == "-1")
                _workflowID = "0";

            string filename = Server.MapPath("~/CSS/osx2_1.css");
            StreamReader sr = File.OpenText(filename);
            string s = sr.ReadToEnd();

            workflow2 = new Workflow2(SecCred, _BP, _userID, culture, s, 700, _usesAD);            

            dbServerModeStruct sms = SecCred.GetServerMode(Request.Url.Host);
            if (System.Configuration.ConfigurationManager.AppSettings["RoutingDebugStatus"] != null && sms.Mode != "P")
            {
                workflow2.debug = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["RoutingDebugStatus"]);
                workflow2.debugUsers = System.Configuration.ConfigurationManager.AppSettings["RoutingDebugEmail"] + "";
                //if (!_webServer.Contains("localhost"))
                  //  workflow2.debugEmailCopyCreator = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["RoutingDebugEmailCopyCreator"]);
            }
                         
            _isAdmin = workflow2.isAdmin();
            
            _DBStatus = workflow2.WorkflowStatus(int.Parse(_workflowID));            
            
            switch (_DBStatus)
            {
                case Routing2.DBStatus.Circulating:
                case Routing2.DBStatus.NotStarted:
                case Routing2.DBStatus.Rejected:
                    if ((_isAdmin || workflow2.enablePopUpMenus(int.Parse(_workflowID))) && !_isMailMessage)
                    {
                        _enableTemplateMenu = true;
                        _enableStepMenu = true;
                    }
                    break;
            }

            if (_DBStatus == Routing2.DBStatus.Circulating || _DBStatus == Routing2.DBStatus.Rejected)
                _isSignOffVisible = true;
            
            //myTranslation = new AppTranslations();
            LangCookies = new SQLCookies("Language", _userID, SecCred);                            
        }

        //public string GetEmailBody(int workflowID)
        //{
        //    string filename = Server.MapPath("~/CSS/osx2_1.css");
        //    StreamReader sr = File.OpenText(filename);
        //    string s = sr.ReadToEnd();

        //    FE_ECN2.DrawWorkflowBody drawBody = new FE_ECN2.DrawWorkflowBody(workflow2);
        //    drawBody.pTranslatePage = TranslatePage;
        //    return drawBody.DrawBody(workflowID); //, 700, s);
        //}

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

        public BoundField buildBoundField(string header, string dataField, int width, bool wrap)
        {
            BoundField bf = new BoundField();
            bf.DataField = dataField;

            bf.HeaderText = header;
            bf.HeaderStyle.Font.Name = "Tahoma";
            bf.HeaderStyle.Wrap = false;
            bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;

            bf.ItemStyle.Font.Size = FontUnit.Point(8);
            bf.ItemStyle.CssClass = "SP_dataLabel";
            if (width > 0)
                bf.ItemStyle.Width = Unit.Percentage(width);
            bf.ItemStyle.Wrap = wrap;
            return bf;
        }
       
        public Dictionary<string, string> parseDictionaryString(string myValues)
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
                //BlockLabel.Text = "&nbsp;&nbsp; <font color=#000000>(" + vMsg + " .... )</font>";
                BlockLabel.Text = vMsg + " ....";
                BlockLabel.Visible = true;
            }
            else
            {
                BlockLabel.Text = "";
                BlockLabel.Visible = false;
            }
        }

        public int getIntFromHex(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }      

        public string getPlantName(string bp)
        {
            if (bp == "0")
                return "All Plants";
            //string retval = "";
            return workflow2.getFriendlyBPName(bp);
            //CWF_Corporate.Plants p = new CWF_Corporate.Plants();
            //DataTable d = p.BranchPlant(bp);
            //if (d.Rows.Count > 0)
            //    retval = d.Rows[0]["Name"].ToString();
            //return retval;
        }

        #region //********************************** Make/Draw Steps *************************/
        public Table drawSteps(int workflowID, bool isTemplate, int tableWidth, bool enableContextMenu)
        {
            CWF_HelpDesk.DrawWorkflowBody drawBody = new CWF_HelpDesk.DrawWorkflowBody(workflow2);            
            
            bool showCommentLink = false;
            if (!isTemplate && _DBStatus == Routing2.DBStatus.Circulating)
                showCommentLink = true;            
            bool showSignOffLink = false;
            if (!isTemplate && _isSignOffVisible)
                showSignOffLink = true;
            Table tb1 = drawBody.DrawSteps(workflowID, tableWidth, isTemplate, enableContextMenu, showSignOffLink, showCommentLink);
            return tb1;
        }        

        #region //**************************************** FIND CONTROLS ************************************/
        public List<Control> FindControls(Control root, string startsWith)
        {
            List<Control> controls = new List<Control>();
            FindControlsRecursive(ref controls, root, startsWith);
            return controls;

        }

        public void FindControlsRecursive(ref List<Control> controls, Control Root, string startsWith)
        {
            foreach (Control Ctl in Root.Controls)
            {
                if (Ctl != null)
                {
                    //System.Diagnostics.Debug.WriteLine(Ctl.UniqueID + ":" + Ctl.ID + ":" + Ctl.ClientID);
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
        #endregion
        #endregion
    }

    //public class GridViewTemplate_Comments : ITemplate
    //{
    //    string _userID = "";
    //    string _commentLabel = "Comment";

    //    public GridViewTemplate_Comments(string userID, string commentLabel)
    //    {
    //        _userID = userID;
    //        _commentLabel = commentLabel;
    //    }

    //    private void OnDataBinding(object sender, EventArgs e)
    //    {
    //        Control ctrl = (Control)sender;
    //        IDataItemContainer data_item_container = (IDataItemContainer)ctrl.NamingContainer;

    //        HyperLink hyp = (HyperLink)sender;
    //        //int stepUserID = (int)DataBinder.Eval(data_item_container.DataItem, "ID");                        
    //        string id = DataBinder.Eval(data_item_container.DataItem, "id").ToString();
    //        string tmp = "javascript:fnAddComment(" + id + ");"; //,'" + _userID + "');";
    //        hyp.ID = "hypComments_" + id;
    //        hyp.NavigateUrl = tmp;
    //    }

    //    public void InstantiateIn(Control objContainer)
    //    {
    //        HyperLink hyp = new HyperLink();
    //        //hyp.ID = _hypID;
    //        hyp.Text = _commentLabel; // "Comment";
    //        hyp.CssClass = "SP_dataLabel";
    //        hyp.ForeColor = System.Drawing.Color.Black;
    //        hyp.DataBinding += new EventHandler(OnDataBinding);
    //        objContainer.Controls.Add(hyp);
    //    }
    //}

}
