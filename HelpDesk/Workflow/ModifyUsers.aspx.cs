using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Services;

using CWF_Corporate;
using CWF_WorkflowRouting;

namespace WF
{
    public partial class ModifyUsers : WF.BaseClass_WF_Page
    {
        public GridView gv;
        private bool _isTemplate = false;
        private DataTable dtStep;

        public ModifyUsers()
        {
            Init += new EventHandler(ModifyUsers_Init);
        }

        public string _border = "0";

        void ModifyUsers_Init(object sender, EventArgs e)
        {
            _isTemplate = bool.Parse(Request["Template"]);
            buildGridViews();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            wf.SetStatus("", BlockLabel);
            Master.BaseTarget = "_self";

            ddl_StepTitles.Attributes.Add("onchange", "fnTitleChanged()");

            if (Request["__EVENTTARGET"] == "Return")
            {
                Exit();
            }

            if (Request["__EVENTTARGET"] == "Save")
            {
                SaveUsers();
            }

            dtStep = wf.workflow2.GetStep(Convert.ToInt32(wf._stepID), _isTemplate);
            
            if (!IsPostBack)
            {                
                txtStepTitle.Text = dtStep.Rows[0]["description"].ToString();
                
                txtPastDueLimit.Text = dtStep.Rows[0]["PastDueLimit"].ToString();
                txtEmailReminder.Text = dtStep.Rows[0]["EmailReminderFrequency"].ToString();

                DataTable dtRejectOptions = wf.workflow2.GetRejectOptions();
                ddl_Reject.DataSource = dtRejectOptions;
                ddl_Reject.DataTextField = "Description";
                ddl_Reject.DataValueField = "Name";
                ddl_Reject.DataBind();
                if (ddl_Reject.Items.FindByValue(dtStep.Rows[0]["RejectAction"].ToString()) != null)
                    ddl_Reject.SelectedValue = dtStep.Rows[0]["RejectAction"].ToString();

                DataTable dtFinalStepOptions = wf.workflow2.GetFinalStepOptions();
                DataRow newRow = dtFinalStepOptions.NewRow();
                newRow["Name"] = "";
                newRow["Description"] = "";
                dtFinalStepOptions.Rows.InsertAt(newRow, 0);
                ddl_FinalStepOptions.DataSource = dtFinalStepOptions;
                ddl_FinalStepOptions.DataTextField = "Description";
                ddl_FinalStepOptions.DataValueField = "Name";
                ddl_FinalStepOptions.DataBind();
                if (ddl_FinalStepOptions.Items.FindByValue(dtStep.Rows[0]["FinalStepAction"].ToString()) != null)
                    ddl_FinalStepOptions.SelectedValue = dtStep.Rows[0]["FinalStepAction"].ToString();

                DataTable dtRuntimeUsers = wf.workflow2.GetRuntimeUsers();
                ddl_FieldName.DataSource = dtRuntimeUsers;
                ddl_FieldName.DataTextField = "Description";
                ddl_FieldName.DataValueField = "Name";
                ddl_FieldName.DataBind();

                DataTable dtDetailURL = wf.workflow2.GetDetailURL();              
                ddl_StepTitles.DataSource = dtDetailURL;
                ddl_StepTitles.DataTextField = "StepTitle";
                ddl_StepTitles.DataValueField = "ID";
                ddl_StepTitles.DataBind();
                ListItem li = ddl_StepTitles.Items.FindByText(dtStep.Rows[0]["Description"].ToString());
                if (li != null)
                {
                    ddl_StepTitles.SelectedValue = li.Value;
                    txtStepTitle.Visible = false;
                }
                else
                {
                    txtStepTitle.Text = dtStep.Rows[0]["Description"].ToString();
                    txtStepTitle.Visible = true;
                    DataView dv1 = new DataView(dtDetailURL);
                    dv1.RowFilter = "StepTitle like '%User defined%'";
                    if (dv1.Count > 0)
                        ddl_StepTitles.SelectedValue = dv1[0]["ID"].ToString();
                }
                
                DataTable dtSteps = wf.workflow2.GetSteps(int.Parse(wf._workflowID), _isTemplate);
                DataView dv = new DataView(dtSteps);
                dv.RowFilter = "ID=" + wf._stepID;
                dv.Delete(0);
                newRow = dtSteps.NewRow();
                newRow["description"] = "";
                newRow["ID"] = 0;
                dtSteps.Rows.InsertAt(newRow, 0);
                ddl_ParentSteps.DataTextField = "Description";
                ddl_ParentSteps.DataValueField = "ID";
                ddl_ParentSteps.DataSource = dtSteps;
                ddl_ParentSteps.DataBind();
                if (dtStep.Rows[0]["parentID"] != DBNull.Value)
                {
                    if (ddl_ParentSteps.Items.FindByValue(dtStep.Rows[0]["ParentID"].ToString()) != null)
                        ddl_ParentSteps.SelectedValue = dtStep.Rows[0]["parentID"].ToString();
                }

                if (dtStep.Rows[0]["groupStep"].ToString() == "0")
                    chkGroup.Checked = false;
                else
                    chkGroup.Checked = true;

                initForm();

                if (Request["refreshPage"] != null)
                    hdnRefreshHost.Value = "true";
            }
            //********** this code causes the link add user button to post when the ENTER button is hit ******************/
            string tmp = "document.getElementById('" + lnkBtnAdd.ClientID + "').focus(); ";
            //tmp += "document.getElementById('" + lnkBtnAdd.ClientID + "').click();";
            tmp += "__doPostBack('" + lnkBtnAdd.UniqueID + "','');";
            txtAdduser.Attributes.Add("onkeyup", "if (event.keyCode==13){ " + tmp + " return true; }");
            //************************************************************************************//

            //addPopUpToolbar_newValue();

            ConfigurePage();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] == "titleChanged")
            {
                initForm();
            }
           
            if (!IsPostBack)
            {
               
            }
        }

        private void ConfigurePage()
        {
            if (_isTemplate)
            {
                DataTable dt = wf.workflow2.GetTemplate(wf._workflowID);
                Master.HeaderText = "Workflow Template" + " (" + dt.Rows[0]["description"] + ")";
            }
            else
                Master.HeaderText = "Modify Users";

            Master.Title = "Modify Users";
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnReturn()", "GoRtlHS.png", "Return", "");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnSave()", "save16.png", "Save", "");
        }

        private void UpdateUsers()
        {
            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow gvr = gv.Rows[i];
                DataKey dk = gv.DataKeys[i];
                string ID = dk.Value.ToString();

                CheckBox cbInclude = (CheckBox)gvr.FindControl("cb_" + ID);
                if (cbInclude == null)
                {
                    continue;
                }
                if (!cbInclude.Checked)
                    wf.workflow2.DeleteStepUser(ID, _isTemplate);
                else
                {
                    DataTable dt = wf.workflow2.GetStepUser(ID, _isTemplate, true);
                    RadioButton rbRequired = (RadioButton)gvr.FindControl("rb_" + ID + "_REQUIRED");
                    int rr = 0;
                    if (rbRequired.Checked)
                        rr = 1;
                    dt.Rows[0]["ResponseRequired"] = rr;
                    wf.workflow2.updateUsers(dt, _isTemplate);
                }
            }
        }

        private void buildGridViews()
        {
            DataTable dt = wf.workflow2.GetStepUsers(Convert.ToInt32(wf._stepID), _isTemplate, false);
            gv = new GridView();

            gv.ID = "GV1";

            string[] keys = { "ID" };
            gv.DataKeyNames = keys;

            gv.PageSize = 10;
            gv.Width = Unit.Pixel(520);
            gv.AutoGenerateColumns = false;

            gv.HeaderStyle.Font.Name = "Tahoma";
            gv.HeaderStyle.Font.Size = FontUnit.Point(8);
            gv.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(wf.getIntFromHex("6B"), wf.getIntFromHex("69"), wf.getIntFromHex("6B"));
            gv.HeaderStyle.ForeColor = System.Drawing.Color.White;

            gv.RowStyle.Font.Name = "Tahoma";
            gv.RowStyle.Font.Size = FontUnit.Point(8);
            gv.RowStyle.BackColor = System.Drawing.Color.FromArgb(wf.getIntFromHex("eb"), wf.getIntFromHex("f3"), wf.getIntFromHex("ff"));

            gv.AlternatingRowStyle.Font.Name = "Tahoma";
            gv.AlternatingRowStyle.Font.Size = FontUnit.Point(8);
            gv.AlternatingRowStyle.BackColor = System.Drawing.Color.White;

            gv.CellPadding = 4;
            gv.BorderWidth = Unit.Pixel(1);

            gv.Columns.Add(wf.buildBoundField("User", "Name", 0, false));

            TemplateField tf = new TemplateField();
            tf.HeaderText = "Include";
            tf.ItemTemplate = new GridViewTemplate("CHECKBOX", "");
            gv.Columns.Add(tf);

            tf = new TemplateField();
            tf.HeaderText = "Optional";
            tf.ItemTemplate = new GridViewTemplate("", "OPTIONAL");
            gv.Columns.Add(tf);

            tf = new TemplateField();
            tf.HeaderText = "Required";
            tf.ItemTemplate = new GridViewTemplate("", "REQUIRED");
            gv.Columns.Add(tf);

            Panel_GV.Controls.Clear();
            Panel_GV.Controls.Add(gv);

            gv.DataSource = dt;
            gv.DataBind();
        }

        public bool SaveStepHeader(bool updateUsers)
        {
            DataTable dtStep = wf.workflow2.GetStep(int.Parse(wf._stepID), _isTemplate);
            if (txtStepTitle.Visible)
            {
                if (txtStepTitle.Text.Trim().Length == 0)
                {
                    wf.SetStatus("Please enter a title for this step!", BlockLabel);
                    return false;
                }
                dtStep.Rows[0]["Description"] = txtStepTitle.Text.Trim();               
            }
            else
            {
                ListItem li = ddl_StepTitles.SelectedItem;
                dtStep.Rows[0]["Description"] = li.Text;
            }

            dtStep.Rows[0]["GroupStep"] = 0;
            if (chkGroup.Checked == true)
                dtStep.Rows[0]["GroupStep"] = 1;
            if (ddl_Reject.SelectedValue.Length > 0)
                dtStep.Rows[0]["RejectAction"] = int.Parse(ddl_Reject.SelectedValue);

            if (ddl_FinalStepOptions.SelectedValue.Length > 0)
                dtStep.Rows[0]["FinalStepAction"] = ddl_FinalStepOptions.SelectedValue;

            DataTable dtDetailURL = wf.workflow2.GetDetailURL_byID(ddl_StepTitles.SelectedValue);
            dtStep.Rows[0]["detailURL"] = dtDetailURL.Rows[0]["detailURL"];                     

            if (ddl_ParentSteps.SelectedValue != "0")
                dtStep.Rows[0]["parentID"] = ddl_ParentSteps.SelectedValue;
            else
                dtStep.Rows[0]["parentID"] = DBNull.Value;

            int i;
            if(int.TryParse(txtPastDueLimit.Text.Trim(), out i))
                dtStep.Rows[0]["PastDueLimit"] = i;
            
            if (int.TryParse(txtEmailReminder.Text.Trim(), out i))
                dtStep.Rows[0]["EmailReminderFrequency"] = i;

            wf.workflow2.UpdateStep(dtStep, _isTemplate);

            SaveStepToolTip();

            if (updateUsers)
                UpdateUsers();

            hdnRefreshHost.Value = "true";
            return true;
        }

        private void SaveStepToolTip()
        {
            DataTable dtDetailURL = wf.workflow2.GetDetailURL_byID(ddl_StepTitles.SelectedValue);
            dtDetailURL.Rows[0]["HelpToolTip"] = txtHelpToolTip.Text;
            wf.workflow2.UpdateDetailURL(dtDetailURL);
        }

        private void SaveUsers()
        {            
            SaveStepHeader(true);                        
            Exit();
        }

        private void initForm()
        {
            txtStepTitle.Visible = false;

            DataTable dtDetailURL = wf.workflow2.GetDetailURL_byID(ddl_StepTitles.SelectedValue);
            if (dtDetailURL.Rows.Count > 0)
            {
                if (dtDetailURL.Rows[0]["StepTitle"].ToString().Contains("User defined"))
                    txtStepTitle.Visible = true;

                txtHelpToolTip.Text = dtDetailURL.Rows[0]["HelpToolTip"].ToString();
                lblDetailURL.Text = dtDetailURL.Rows[0]["detailURL"].ToString();
            }
        }
        
        private void Exit()
        {
            if (_isTemplate)
                Response.Redirect("WF_Manager.aspx?BP=" + wf._BP + "&workflowID=" + wf._workflowID);
            else
            {
                string sScript = "";
                if (hdnRefreshHost.Value == "true")
                    sScript = "<script>top.returnValue=true;window.close();</script>";
                else
                    sScript = "<script>top.returnValue=false;window.close();</script>";
                Response.Write(sScript);
            }
        }

        //#region //**************************** New Title ******************************/
        //private void addPopUpToolbar_newValue()
        //{
        //    Classes.PopUpToolbar pt = new Classes.PopUpToolbar(wf._webServer, wf._appPath, "Add new value", "");
        //    pt.LoadToolbarItem("", "javascript:fnHideToolBarPopUp_newValue(); return false;", "GoRtlHS.png", wf.TranslatePage("Return"), "");
        //    pt.LoadToolbarItem("", "", "vert_div.gif", "", "");
        //    pt.LoadToolbarItem("", "javascript:fnSaveNewValue()", "save16.png", wf.TranslatePage("Save"), "");
        //    HtmlGenericControl gc = new HtmlGenericControl();
        //    gc.InnerHtml = pt.DisplayDynamicToolbar();
        //    panelAddValue.Controls.AddAt(0, gc);
        //}
        //#endregion

        #region //********************** EVENTS *******************************/
        protected void lnkBtnAddFieldName_Click(object sender, EventArgs e)
        {
            SaveStepHeader(true);
            //updateUsers();
            if (ddl_FieldName.SelectedValue.Length > 0)
            {
                DataTable dt = wf.workflow2.GetStepUser(wf._stepID, ddl_FieldName.SelectedValue, _isTemplate, true);
                if (dt.Rows.Count == 0)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["stepID"] = wf._stepID;
                    newRow["userName"] = "";
                    newRow["fieldName"] = ddl_FieldName.SelectedValue;
                    newRow["responseRequired"] = 1;
                    newRow["SignedOff"] = 0;
                    newRow["Rejected"] = 0;
                    newRow["DateTimeStamp"] = DateTime.Now.ToShortDateString();
                    newRow["LastModifiedBy"] = wf._userID;

                    DataTable dtRunTimeUser = wf.workflow2.GetRuntimeUsers(ddl_FieldName.SelectedValue);
                    newRow["RoleBasedUser"] = (bool)dtRunTimeUser.Rows[0]["isRoleBased"];

                    dt.Rows.Add(newRow);
                    wf.workflow2.updateUsers(dt, _isTemplate);

                    hdnRefreshHost.Value = "true";
                    //buildGridViews();
                }
            }
            ddl_FieldName.SelectedIndex = -1;
            buildGridViews();
        }

        protected void lnkBtnAdd_Click(object sender, EventArgs e)
        {
            if (txtAdduser.Text.Length == 0 || hdnUserID.Value.Length == 0)
            {
                txtAdduser.Text = "";
                hdnUserID.Value = "";
                txtAdduser.Focus();
                return;
            }

            SaveStepHeader(false);

            Authentication AU = new Authentication(wf.SecCred, wf._usesAD);
            Profile p = AU.GetUserProfile(hdnUserID.Value);
            if (p.friendlyName == txtAdduser.Text)
            {
                DataTable dt = wf.workflow2.GetStepUser(wf._stepID, hdnUserID.Value, _isTemplate, false);
                if (dt.Rows.Count == 0)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["stepID"] = wf._stepID;
                    newRow["userName"] = hdnUserID.Value;
                    newRow["responseRequired"] = 1;
                    newRow["SignedOff"] = 0;
                    newRow["Rejected"] = 0;
                    newRow["DateTimeStamp"] = DateTime.Now.ToShortDateString();
                    newRow["LastModifiedBy"] = wf._userID;
                    newRow["RoleBasedUser"] = false;
                    dt.Rows.Add(newRow);
                    wf.workflow2.updateUsers(dt, _isTemplate);

                    hdnRefreshHost.Value = "true";
                    buildGridViews();
                }
            }
            txtAdduser.Text = "";
            txtAdduser.Focus();
            hdnUserID.Value = "";
        }
        #endregion

        #region //******************************** WEB METHODS *******************************/
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetUserNames(string prefixText, int count)
        {
            return QA.Classes.GetNames.GetADUserNames(prefixText, count);
        }
        #endregion
    }

    public class GridViewTemplate : ITemplate
    {
        bool _isCheckBox = false;
        string _fieldName = "";

        public GridViewTemplate(string templateType, string fieldName) //, string ID)
        {
            switch (templateType.ToUpper())
            {
                case "CHECKBOX":
                    _isCheckBox = true;
                    break;

                default:
                    _fieldName = fieldName;
                    break;
            }
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            IDataItemContainer data_item_container = (IDataItemContainer)ctrl.NamingContainer;

            string value = DataBinder.Eval(data_item_container.DataItem, "ResponseRequired").ToString();
            string ID = DataBinder.Eval(data_item_container.DataItem, "ID").ToString();

            if (_isCheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                cb.ID = "cb_" + ID;
                cb.Checked = true;
            }
            else
            {
                RadioButton rb = (RadioButton)sender;

                rb.ID = "rb_" + ID + "_" + _fieldName;
                rb.GroupName = ID; // user;
                if (_fieldName == "OPTIONAL")
                {
                    if (value == "0")
                        rb.Checked = true;
                }
                else
                {
                    if (value == "1")
                        rb.Checked = true;
                }
            }
        }

        public void InstantiateIn(System.Web.UI.Control Container)
        {
            if (_isCheckBox)
            {
                CheckBox cb = new CheckBox();
                cb.DataBinding += new EventHandler(OnDataBinding);
                Container.Controls.Add(cb);
            }
            else
            {
                RadioButton rb = new RadioButton();
                rb.DataBinding += new EventHandler(OnDataBinding);
                Container.Controls.Add(rb);
            }
        }
    }
}
