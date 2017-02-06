using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Services;

using CWF_Security;
using CWF_SQLCookies;
using CWF_WorkflowRouting;

namespace QA
{
    public partial class _Default : BaseClass
    {
        private SQLCookies searchPageCookies;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            searchPageCookies = new SQLCookies(_appFriendlyName + @"/SearchPage", userID, SecCred);
                    
            ddl_Location.Attributes.Add("onchange", "fnLocationChanged()");

            if (!IsPostBack)
            {
                initForm();

                string allCookies = searchPageCookies.getCookieString();
                allCookies = allCookies.Replace(";", "&");
                prePopulateForm(allCookies);
            }

            if (Request["__EVENTTARGET"] == "Search")
            {                
                string s = buildQuery();
                Response.Redirect("searchresults.aspx?" + s);
            }

            if (Request["__EVENTTARGET"] == "ClearForm")
                ClearForm();

            if (Request["__EVENTTARGET"] == "New")
                NewWF();

            //if (Request["__EVENTTARGET"] == "MyWorkfow")
            //{
            //    string s = buildQuery(true);
            //    Response.Redirect("searchresults.aspx?" + s);
            //}

            ConfigurePage();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] == "LocationChanged")
            {
                locationChanged();
            }
        }

        private void ConfigurePage()
        {
            Master.Title = "Search " + _appFriendlyName;
            string location = "All Locations";
            bool createNew = false;
            DataTable dt = tools.getLocation(ddl_Location.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                location = dt.Rows[0]["Location"].ToString();
                createNew = (bool)dt.Rows[0]["CreateNew"];
            }
            Master.HeaderText = _appFriendlyName + " - " + location;
           
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnReset();", "refresh16.png", "Reset Search Parameters", "");

            if (createNew)
            {
                Master.MyToolbar.LoadToolbarItem("", "javascript:CreateNewWorkfow();", "New16.png", "New Work Order" + " - " + location, "");
            }

            if (tools.isAdmin(_BP))
            {
                Master.MyToolbar.LoadToolbarItem("", "javascript:Admin();", "security16.png", "Administration", "");
            }

            if (!IsPostBack)
            {                
               // CalendarExtender1.Format = _culture.DateTimeFormat.ShortDatePattern;
               // CalendarExtender2.Format = _culture.DateTimeFormat.ShortDatePattern;
            }

            Form.Attributes["OnKeyPress"] = "fnCheckForEnter()";        
        }

   
   

        private void initForm()
        {           
            PopulateCreatedByList();

            populateDepartments();

            DataTable dt1 = tools.getLocations();
            if (dt1.Rows.Count > 1)
            {
                DataRow dr = dt1.NewRow();
                dr["Location"] = "All Locations";
                dr["BP"] = "";
                dt1.Rows.InsertAt(dr, 0);
            }

            ddl_Location.DataSource = dt1;
            ddl_Location.DataTextField = "Location";
            ddl_Location.DataValueField = "BP";

            if (tools.BPisValid(_BP))
                ddl_Location.SelectedValue = _BP;
            ddl_Location.DataBind();

            dt1 = workflow2.GetDistinctStepUsers(ddl_Location.SelectedValue); // tools.GetHeaderField(ddl_Location.SelectedValue, "AssignedTo");
            DataRow newRow = dt1.NewRow();
            newRow["friendlyName"] = "";
            newRow["userName"] = "";
            dt1.Rows.InsertAt(newRow, 0);

            ddl_AssignedTo.DataSource = dt1;
            ddl_AssignedTo.DataTextField = "friendlyName";
            ddl_AssignedTo.DataValueField = "userName";
            ddl_AssignedTo.DataBind();

            DataTable dtWFSteps = workflow2.GetAllActiveStepDescriptions(ddl_Location.SelectedValue);
            //if (dtWFSteps.Rows.Count > 0)
            //{
            //    DataRow dr = dtWFSteps.NewRow();
            //    dr["Description"] = "";
            //    dtWFSteps.Rows.InsertAt(dr, 0);
            //    ddl_WorkflowStep.DataSource = dtWFSteps;
            //    ddl_WorkflowStep.DataTextField = "Description";
            //    ddl_WorkflowStep.DataValueField = "Description";
            //    ddl_WorkflowStep.DataBind();
            //}
        }        

        private void populateDepartments()
        {
            DataTable dt1 = tools.GetHeaderField(_BP, "Department");
                DataRow dr = dt1.NewRow();
                dr["friendlyName"] = "";
                dr["Department"] = "";
                dt1.Rows.InsertAt(dr, 0);
            
            //ddl_Department.DataSource = dt1;
            //ddl_Department.DataTextField = "friendlyName";
            //ddl_Department.DataValueField = "Department";            
            //ddl_Department.DataBind();
        }

        private void PopulateCreatedByList()
        {
            try
            {
                ddlCreatedBy.Items.Clear();
                ddlCreatedBy.Items.Add(new ListItem("", ""));

                OpenConn();

                DataTable dt = new DataTable();
                string SQL = "Select Distinct CreatedBy FROM WF_Header WHERE CreatedBy is not null and CreatedBy <> '' order by CreatedBy";
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = new SqlCommand(SQL, sqlConn);
                sqlDA.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {                   
                    var Name = tools.GetUserName(dr["CreatedBy"].ToString());
                    ddlCreatedBy.Items.Add(new ListItem(Name, dr["CreatedBy"].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(" at PopulateCreatedByList " + ex.Message);
            }
            finally
            {
                CloseConn();
            }

        }       

        private void locationChanged()
        {
            _BP = ddl_Location.SelectedValue;
            searchPageCookies.SetCookieSQL("BP", ddl_Location.SelectedValue);            
        }

        private void ClearForm()
        {
            txtWorkflowID.Text = "";
            ddl_AssignedTo.SelectedIndex = -1;
            //txtEquipment.Text = "";
            //ddl_Department.SelectedIndex = -1;
            txtCreatedDateFrom.Text = "";
            txtCreatedDateTo.Text = "";
            ddlCreatedBy.SelectedIndex = -1;
            ddlStatus.SelectedValue = "1";
            hdnUserID.Value = "";
            txtComments.Text = "";
            //txtServiceRequired.Text = "";
            //ddl_WorkflowStep.SelectedIndex = -1;
        }

        private void prePopulateForm(string queryString)
        {
            //bool foundMatch = false;
            Dictionary<string, string> dQuery = new Dictionary<string, string>();

            string[] tmp = queryString.Split('&');
            foreach (string s in tmp)
            {
                if (s.Contains("=") == true)
                {
                    string[] tmp1 = s.Split('=');
                    dQuery.Add(tmp1[0], tmp1[1]);
                }
            }

            foreach (KeyValuePair<string, string> kvp in dQuery)
            {
                switch (kvp.Key)
                {
                    case "BP":
                        if (kvp.Value.Length > 1)
                            if (ddl_Location.Items.FindByValue(kvp.Value) != null)
                                ddl_Location.SelectedValue = kvp.Value;
                        break; 

                    case "WorkFlowID":
                        txtWorkflowID.Text = kvp.Value;
                       // foundMatch = true;
                        break;

                    case "AssignedTo":
                        if (ddl_AssignedTo.Items.FindByValue(kvp.Value) != null)
                        {
                            ddl_AssignedTo.SelectedValue = kvp.Value;
                         //   foundMatch = true;
                        }
                        break;

                    case "Equipment":
                        //txtEquipment.Text = kvp.Value;
                       // foundMatch = true;
                        break;  

                    case "CreatedDateFrom":
                        txtCreatedDateFrom.Text = kvp.Value;
                       // foundMatch = true;
                        break;

                    case "CreatedDateTo":
                        txtCreatedDateTo.Text = kvp.Value;
                      //  foundMatch = true;
                        break;

                    case "ServiceRequired":
                        //txtServiceRequired.Text = kvp.Value;
                      //  foundMatch = true;
                        break;

                    case "WFComments":
                        txtComments.Text = kvp.Value;
                     //   foundMatch = true;
                        break;

                    //case "Department":
                    //    if (ddl_Department.Items.FindByValue(kvp.Value) != null)
                    //    {
                    //        ddl_Department.SelectedValue = kvp.Value;
                    //   //     foundMatch = true;
                    //    }
                    //    break;

                    case "Status":
                        if (ddlStatus.Items.FindByValue(kvp.Value) != null)
                        {
                            ddlStatus.SelectedValue = kvp.Value;
                       //     foundMatch = true;
                        }
                        break;
                    
                    case "CreatedBy":
                        if (ddlCreatedBy.Items.FindByValue(kvp.Value) != null)
                        {
                            ddlCreatedBy.SelectedValue = kvp.Value;
                        //    foundMatch = true;
                        }
                        break;

                    //case "WFStep":
                    //    if (ddl_WorkflowStep.Items.FindByValue(kvp.Value) != null)
                    //        ddl_WorkflowStep.SelectedValue = kvp.Value;
                    //    break; 
                }
            }
            //if (!foundMatch)
            //    ddlStatus.SelectedValue = "1"; 
        }

        private string buildQuery()
        {
            searchPageCookies.ClearAllCookies();

            string s = ""; 
            
            if (txtWorkflowID.Text.Length > 0)
                return "&WFNumber=" + txtWorkflowID.Text;
            else
            {
                //if (ddl_WorkflowStep.SelectedValue.Length > 0)
                //    return "WFStep=" + ddl_WorkflowStep.SelectedValue;

                if (ddl_AssignedTo.SelectedValue.Length > 0)
                    s += "&AssignedTo=" + ddl_AssignedTo.SelectedValue;

                //if (!string.IsNullOrEmpty(txtEquipment.Text))
                //    s += "&Equipment=" + txtEquipment.Text.Trim();

                if (!string.IsNullOrEmpty(txtComments.Text))
                    s += "&WFComments=" + txtComments.Text.Trim();

                if (ddlPriority.SelectedValue.Length > 0)
                    s += "&Priority=" + ddlPriority.SelectedValue;

                //if (!string.IsNullOrEmpty(txtServiceRequired.Text))
                //    s += "&ServiceRequired=" + txtServiceRequired.Text.Trim();

                //if (ddl_Department.SelectedValue.Length > 0)
                //    s += "&Department=" + ddl_Department.SelectedValue;

                if (ddlStatus.SelectedValue.Length > 0)
                    s += "&Status=" + ddlStatus.SelectedValue;              

                if (ddlCreatedBy.SelectedValue.Length > 0)
                    s += "&CreatedBy=" + ddlCreatedBy.SelectedValue;
                
                switch (rblMyECN.SelectedValue)
                {
                    case "1":
                        s += "&MyList=1";
                        ddl_Location.SelectedIndex = 0;
                        break;

                    case "2":
                        s = "&MyActiveList=1";
                        ddl_Location.SelectedIndex = 0;
                        break;
                }
            }
           // if (s.Length == 0)
             //   s += "&Status=1";
            
            s += "&BP=" + ddl_Location.SelectedValue;
            
            s = s.Substring(1);
            return s;
        }       
        
        private void NewWF()
        {
            string newID = workflow2.MakeRev0(ddl_Location.SelectedValue, userID);            
            Response.Redirect("mainForm.aspx?Edit=1&BP=" + ddl_Location.SelectedValue + "&workflowID=" + newID);
        }

        #region //******************************** WEB METHODS *******************************/
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetUserNames(string prefixText, int count)
        {
            return QA.Classes.GetNames.GetADUserNames(prefixText, count);
        }
        #endregion 
    }
}

