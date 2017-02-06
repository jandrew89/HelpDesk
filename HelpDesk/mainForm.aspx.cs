using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Globalization;
using System.Text;
using System.IO;
using System.Drawing;

using CWF_WorkflowRouting;
using CWF_HelpDesk;
using CWF_SQLCookies;

namespace QA
{
    public partial class mainForm : BaseClass
    {
        WF.WorkflowSteps2 uctemplate;
     
        public string queryString = "";
        public SQLCookies TheCookies;
        public DataTable dtTemplateList;
        private bool _editRequest = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            TheCookies = new SQLCookies(URL, userID, SecCred);

            if ((_isOwner || _isAdmin) && _DBStatus == Routing2.DBStatus.NotStarted)
                _editRequest = true;
            
            //imgBtn_GetEquipment.ImageUrl = "images/new add16.png";
            //imgBtn_GetEquipment.Attributes.Add("onclick", "fnGetEquipment(); return false;");

            ddl_Department.Attributes.Add("onchange", "fnDeptChanged();");

            queryString = "BP=" + _BP + "&WorkFlowID=" + workflowID;
            if (Request["returnToList"] != null)
                queryString += "&returnToList=" + Request["returnToList"];

            if (!IsPostBack)
            {
                initForm();

                if (Request["ResetWorkflow"] != null)
                {
                    workflow2.ResetWorkflow((int)myHeaderDT.Rows[0]["WFID"]);
                    tools.DeleteTimeWorked(myHeaderDT.Rows[0]["ID"].ToString());
                    Response.Redirect("mainForm.aspx?" + queryString);
                }

                GetRequestHeaderData();
            }

            if (Request["__EVENTTARGET"] == "Department")
            {
                initEquipment();
            }

            if (Request["__EVENTTARGET"] == "Delete")
            {
                if (int.Parse(workflowID) > 0)
                    workflow2.DeleteWorkflowHeader(int.Parse(workflowID));
                tools.deleteHeader(workflowID);

                DataTable dt = tools.getHeader(workflowID, true);
                if (dt.Rows.Count > 0)
                    Response.Redirect("mainForm.aspx?BP=" + _BP + "&WorkflowID=" + dt.Rows[dt.Rows.Count - 1]["WFID"].ToString());

                Response.Redirect("default.aspx?BP=" + _BP);
            }

            if (Request["__EVENTTARGET"] == "CopyWF")
            {
                fnCopyTemplateToWorkflow(Request["__EVENTARGUMENT"], ddlWorkflowTemplates.SelectedItem.Text);
            }
           
            if (Request["__EVENTTARGET"] == "newUser")
            {
                newUser();
            }

            getTemplateList();

            if (Request["__EVENTTARGET"] == "newEquipment")
            {
                if(ddl_Equipment.Items.FindByValue(Request["__EVENTARGUMENT"]) == null)
                {
                    ddl_Equipment.Items.Add(new ListItem(Request["__EVENTARGUMENT"], Request["__EVENTARGUMENT"]));
                    ddl_Equipment.SelectedValue = Request["__EVENTARGUMENT"];
                }
            }

            if (Request["__EVENTTARGET"] == "Submit")
            {
                StartRequest();
            }

            if (Request["__EVENTTARGET"] == "CancelWF")
            {
                workflow2.CancelWorkflow(int.Parse(workflowID), Request["__EVENTARGUMENT"] + "");

                string stepEmail = workflow2.fnGetAllStepUsersEmail(int.Parse(workflowID));
                workflow2.SendEmail(int.Parse(workflowID), 0, userEmail, stepEmail, Request["__EVENTARGUMENT"] + "");
                Response.Redirect("mainForm.aspx?" + Request.QueryString);
            }

            //if (Request["__EVENTTARGET"] == "Save")
            //{
            //    bool retval = SaveRequest();
            //    if (retval)
            //        Response.Redirect("mainForm.aspx?" + queryString);
            //}

            //' //************ INSERT USER CONTROL/SUPPORT FOR ROUTING STEPS *******************/            
            uctemplate = (WF.WorkflowSteps2)LoadControl("~/workflow/WorkflowSteps2.ascx");
            WorkflowStepDisplay.Controls.Add(uctemplate);
            uctemplate.workflowMessage += messageFromWorkflow;

            if (Request["__EVENTTARGET"] == "DeleteAttachment")
            {
                if (Request["__EVENTARGUMENT"] != null)
                {
                    tools.deleteAttachment(Request["__EVENTARGUMENT"]);
                }
            }

            if (Request["__EVENTTARGET"] == "AddAttachment")
                addAttachment();

            initAttachments();
            
            initTimeWorked();

            addPopUpToolbar_User();

            ConfigurePage();            
        }

        private void ConfigurePage()
        {
            Master.Title = _appFriendlyName;
            Master.HeaderText = tools.getPlantName(_BP) + " " + _appFriendlyName + " - " + myHeaderDT.Rows[0]["FriendlyName"];

            Master.MyToolbar.LoadToolbarItem("default.aspx?BP=" + _BP, "Search16.png", "Search Page");
            Master.MyToolbar.LoadToolbarItem("", "javascript:ReturnToList('" + Request["returnToList"] + "=1');", "GoRtlHS.png", "Return to List", "", "Return to List");

            if (_DBStatus == Workflow2.DBStatus.NotStarted)
            {
                //Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
                //Master.MyToolbar.LoadToolbarItem("", "javascript:Edit();", "EditTableHS.png", "Edit", "");

                Master.MyToolbar.LoadToolbarItem("", "javascript:Submit();", "Send (Email) Forward16.png", "Submit", "");
            }

            if (_isAdmin && (_DBStatus == Workflow2.DBStatus.Circulating)) // || _DBStatus == Workflow2.DBStatus.NotStarted))
            {
                Master.MyToolbar.LoadToolbarItem("", "javascript:Delete();", "Delete16.png", "Delete", "");
            }

            Master.MyToolbar.LoadToolbarItem("", "javascript:AddAttachment();", "AttachmentHS.png", "Attach File", "");

            if (_isAdmin || _isOwner)
            {
                if (_DBStatus == Routing2.DBStatus.Circulating || _DBStatus == Routing2.DBStatus.Rejected || _DBStatus == Routing2.DBStatus.Cancelled)
                {
                    Master.MyToolbar.LoadToolbarItem("", "javascript:ResetWorkflow();", "RepeatHS.png", "Reset Workflow", "");
                }

                if ((_DBStatus == Routing2.DBStatus.Circulating || _DBStatus == Routing2.DBStatus.Rejected) && _DBStatus != Routing2.DBStatus.Cancelled)
                {
                    Master.MyToolbar.LoadToolbarItem("", "javascript:CancelWF();", "Cancel16.png", "Cancel Workflow", "");
                }
            }
        }
        
        private void initForm()
        {
            DataTable dt1 = tools.getDepartments(_BP);
            ddl_Department.DataSource = dt1;
            ddl_Department.DataTextField = "friendlyName";
            ddl_Department.DataValueField = "Name";
            ddl_Department.DataBind();

            dt1 = tools.GetMaintenanceUsers(_BP, "isMaintElectrical,isMaintMechanical,isMaintOther");
            //DataRow newRow = dt1.NewRow();
            //newRow["friendlyName"] = "Select a Maintenance Person";
            //newRow["userID"] = "";
            //dt1.Rows.InsertAt(newRow, 0);

            ddl_AssignedTo.DataSource = dt1;
            ddl_AssignedTo.DataTextField = "friendlyName";
            ddl_AssignedTo.DataValueField = "userID";
            ddl_AssignedTo.DataBind();


            initEquipment();

            
        }    
    
        private void initTimeWorked()
        {
            DataTable dt = tools.getTimeWorked(myHeaderDT.Rows[0]["ID"].ToString());
            if (dt.Rows.Count > 0)
            {
                Table tbl = new Table();
                tbl.Width = Unit.Percentage(100);
                foreach (DataRow dr in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();

                    tc = new TableCell();
                    tc.Wrap = false;
                    HtmlGenericControl gc = new HtmlGenericControl();
                    gc.InnerHtml = "<div class='SP_dataLabel'>" + dr["timeWorked"].ToString() + "</div>";
                    tc.Controls.Add(gc);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Wrap = false;
                    gc = new HtmlGenericControl();
                    gc.InnerHtml = "<div class='SP_dataLabel'>" + dr["friendlyName"].ToString() + "</div>";
                    tc.Controls.Add(gc);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    gc = new HtmlGenericControl();
                    gc.InnerHtml = "<div class='SP_dataLabel'>" + getFormattedDate(dr["DateTimeStamp"]) + "</div>";
                    tc.Controls.Add(gc);
                    tr.Cells.Add(tc);

                    tbl.Rows.Add(tr);
                }
                panelTimeWorked.Controls.Add(tbl);
            }
        }

        private void initEquipment()
        {
            DataTable dt = tools.GetEquipment(_BP, ddl_Department.SelectedValue);
            ddl_Equipment.DataSource = dt;
            ddl_Equipment.DataTextField = "FriendlyName";
            ddl_Equipment.DataValueField = "FriendlyName";
            ddl_Equipment.DataBind();
        }

        public void messageFromWorkflow(string value)
        {
            switch (value)
            {
                case "0": //'reload data
                    Master.MyToolbar.ClearToolbarItems();
                    GetRequestHeaderData();
                    ConfigurePage();
                    break;

                case "1":
                case "2":
                    Response.Redirect("mainForm.aspx?" + queryString);
                    break;
            }
        }

        private void StartRequest()
        {
            bool retval = SaveRequest();
            if (!retval)
                return;

            if (dtTemplateList.Rows.Count == 1)
            {
                fnCopyTemplateToWorkflow("1", dtTemplateList.Rows[0]["Description"].ToString());
            }
            else
            {
                hdnErrorMsg.Value = "Please select a workflow template!";
                return;
            }

            //********* set lastemailed to something other than null, so the engine won't attempt to email user *******//
            int[] steps = workflow2.GetStepsByDescription(int.Parse(workflowID), "Assigned To", false);
            DataTable dtStepUsers = workflow2.GetStepUsers(steps[0], false, true);
            foreach (DataRow dr in dtStepUsers.Rows)
            {
                dr["LastEmailed"] = DateTime.Now;
            }

            //have to use this instead of StartWorkflow because I don't want the second step to get emailed
            workflow2.updateUsers(dtStepUsers, false);
            workflow2.fnUpdateHeaderStatus(int.Parse(workflowID), _DBStatus, true);
            workflow2.fnWorkflow(int.Parse(workflowID));

            //workflow2.StartWorkflow((int)myHeaderDT.Rows[0]["WFID"]);
            Response.Redirect("mainForm.aspx?" + queryString);
        }

        private bool SaveRequest()
        {
            //initEquipment();

            //if (Request.Form[ddl_Equipment.UniqueID] != null && Request.Form[ddl_Equipment.UniqueID].Length > 0)
            //{
            //    //Response.Write("v:" + Request.Form[ddl_Equipment.UniqueID]);
            //    string newOption = Request.Form[ddl_Equipment.UniqueID].Trim();
            //    if (ddl_Equipment.Items.FindByValue(newOption) == null)
            //    {
            //        ddl_Equipment.Items.Add(new ListItem(newOption, newOption));
            //        ddl_Equipment.SelectedValue = newOption;
            //    }
            //}

            if (!fnValidateFields())
                return false;
                        
            myHeaderDT.Rows[0]["Department"] = ddl_Department.SelectedValue;
            myHeaderDT.Rows[0]["Equipment"] = ddl_Equipment.SelectedValue;
            myHeaderDT.Rows[0]["Response"] = Request.Form["rbResponse"].ToString();
            myHeaderDT.Rows[0]["Type"] = Request.Form["rbTypes"].ToString();
            myHeaderDT.Rows[0]["Condition"] = Request.Form["rbCondition"].ToString();
            myHeaderDT.Rows[0]["Comment"] = txtComments.Text;
            myHeaderDT.Rows[0]["Originator"] = txtOriginator.Text;
            myHeaderDT.Rows[0]["LastUpdatedBy"] = userID;
            myHeaderDT.Rows[0]["LastModifiedDate"] = DateTime.Now;
            myHeaderDT.Rows[0]["AssignedTo"] = ddl_AssignedTo.SelectedValue;
            
            myHeaderDT = tools.updateHeader(myHeaderDT);

            tools.addEquipment(myHeaderDT.Rows[0]["BP"].ToString(), "", myHeaderDT.Rows[0]["Department"].ToString(), "",  myHeaderDT.Rows[0]["Equipment"].ToString(), "");
          
            return true;
        }        

        private void GetRequestHeaderData()
        {     
       
            lblFriendlyName.Text = myHeaderDT.Rows[0]["FriendlyName"].ToString();

            lblStatus.Text = workflow2.WorkflowStatusDescription(_DBStatus);
            if (((int)myHeaderDT.Rows[0]["Status"] == (int)Workflow2.DBStatus.Rejected) || ((int)myHeaderDT.Rows[0]["Status"] == (int)Workflow2.DBStatus.Cancelled))
            {
                lblStatus.ForeColor = Color.Red;
                if ((int)myHeaderDT.Rows[0]["Status"] == (int)Workflow2.DBStatus.Cancelled)
                    lblStatus.Text += " - " + myHeaderDT.Rows[0]["CancelledComments"].ToString();
            }

            lblLocation.Text = tools.getPlantName(myHeaderDT.Rows[0]["BP"].ToString());

            //initialize these with the current values just in case they are not found in the ddl.
            //lblDepartment.Text = myHeaderDT.Rows[0]["Department"].ToString();
            //lblEquipment.Text = myHeaderDT.Rows[0]["Equipment"].ToString();
            //lblType.Text = myHeaderDT.Rows[0]["Type"].ToString();
            //lblResponse.Text = myHeaderDT.Rows[0]["Response"].ToString();
            //lblCondition.Text = myHeaderDT.Rows[0]["Condition"].ToString();
            //lblAssignedTo.Text = myHeaderDT.Rows[0]["AssignedToFriendlyName"].ToString();

            lblCreatedBy.Text = myHeaderDT.Rows[0]["CreatedByFriendlyName"].ToString();
            lblDateCreated.Text = ((DateTime)myHeaderDT.Rows[0]["dateCreated"]).ToString("d", _culture);
            
            //txtRequestedDate.Text = ((DateTime)myHeaderDT.Rows[0]["RequestedDate"]).ToString("d", _culture);
            //lblRequestedDate.Text = txtRequestedDate.Text;                                 

            string lmd = myHeaderDT.Rows[0]["LastModifiedDate"].ToString();
            if (lmd.Length > 0)
                lmd = DateTime.Parse(lmd).ToString("d", _culture);
            lblTimeStamp.Text = "Last updated by" + " " + myHeaderDT.Rows[0]["LastUpdatedByFriendlyName"].ToString() + " : " + lmd;

            if (myHeaderDT.Rows[0]["Type"] != DBNull.Value)
            {
                switch (myHeaderDT.Rows[0]["type"].ToString())
                {
                    case "E":
                        rbElectrical1.Checked = true;
                        break;
                    case "M":
                        rbMechanical1.Checked = true;
                        break;
                    case "O":
                        rbOther1.Checked = true;
                        break;
                }
                //if (rblType.Items.FindByValue(myHeaderDT.Rows[0]["Type"].ToString()) != null)
                //{
                //    rblType.SelectedValue = myHeaderDT.Rows[0]["Type"].ToString();
                //    //lblType.Text = rblType.SelectedItem.Text;
                //}
            }

            if (myHeaderDT.Rows[0]["Response"] != DBNull.Value)
            {
                switch (myHeaderDT.Rows[0]["response"].ToString())
                {
                    case "R":
                        rbRoutine.Checked = true;
                        break;
                    case "S":
                        rbScheduled.Checked = true;
                        break;
                    case "E":
                        rbEmergency.Checked = true;
                        break;
                }
                //    if (rblResponse.Items.FindByValue(myHeaderDT.Rows[0]["Response"].ToString()) != null)
                //    {
                //        rblResponse.SelectedValue = myHeaderDT.Rows[0]["Response"].ToString();
                //        //lblResponse.Text = rblResponse.SelectedItem.Text;
                //    }
            }

            if(myHeaderDT.Rows[0]["Condition"] != DBNull.Value)
            {
                switch (myHeaderDT.Rows[0]["condition"].ToString())
                {
                    case "D":
                        rbDown.Checked = true;
                        break;
                    case "R":
                        rbRunning.Checked = true;
                        break;
                  
                }
                //if (rblCondition.Items.FindByValue(myHeaderDT.Rows[0]["Condition"].ToString()) != null)
                //{
                //    rblCondition.SelectedValue = myHeaderDT.Rows[0]["Condition"].ToString();
                //    //lblCondition.Text = rblCondition.SelectedItem.Text;
                //}
            }

            if (myHeaderDT.Rows[0]["Department"] != DBNull.Value)
            {
                if (ddl_Department.Items.FindByValue(myHeaderDT.Rows[0]["Department"].ToString()) != null)
                {
                    ddl_Department.SelectedValue = myHeaderDT.Rows[0]["Department"].ToString();
                    //lblDepartment.Text = ddl_Department.SelectedItem.Text;
                }               
            }

            if (myHeaderDT.Rows[0]["Equipment"] != DBNull.Value)
            {
                if (ddl_Equipment.Items.FindByValue(myHeaderDT.Rows[0]["Equipment"].ToString()) != null)
                {
                    ddl_Equipment.SelectedValue = myHeaderDT.Rows[0]["Equipment"].ToString();
                    //lblEquipment.Text = ddl_Equipment.SelectedItem.Text;
                }
                //else
                //{
                //    tools.normalizeEquipment();
                //    initEquipment();
                //    ddl_Equipment.SelectedValue = myHeaderDT.Rows[0]["Equipment"].ToString();
                //    lblEquipment.Text = ddl_Equipment.SelectedItem.Text;
                //}
            }

            if (myHeaderDT.Rows[0]["AssignedTo"] != DBNull.Value)
            {
                if (ddl_AssignedTo.Items.FindByValue(myHeaderDT.Rows[0]["AssignedTo"].ToString()) != null)
                {
                    ddl_AssignedTo.SelectedValue = myHeaderDT.Rows[0]["AssignedTo"].ToString();
                    //lblAssignedTo.Text = ddl_AssignedTo.SelectedItem.Text;
                }
                else
                    ddl_AssignedTo.SelectedValue = "unassigned01";
            }
            else
                ddl_AssignedTo.SelectedValue = "unassigned01";

            lblComments.Text = myHeaderDT.Rows[0]["Comment"].ToString();
            txtComments.Text = myHeaderDT.Rows[0]["Comment"].ToString();

            //lblOriginator.Text = myHeaderDT.Rows[0]["Originator"].ToString();
            txtOriginator.Text = myHeaderDT.Rows[0]["Originator"].ToString();

            if (_editRequest)
            {
                ddl_Department.Visible = true;
                ddl_Equipment.Visible = true;
                //rblResponse.Visible = true;
                //rblType.Visible = true;
                //rblCondition.Visible = true;
                //lblRequired_Comment.Visible = true;
                //lblRequired_Condition.Visible = true;
                //lblRequired_Equipment.Visible = true;
                //lblRequired_Response.Visible = true;
                //lblRequired_Type.Visible = true;
                ddl_AssignedTo.Visible = true;
                txtComments.Visible = true;
                txtOriginator.Visible = true;
                //imgBtn_GetEquipment.Visible = true;
                //lblRequired_Originator.Visible = true;
                lblRequiredFields.Visible = true;
                //lblRequired_AssginedTo.Visible = true;
            }
            else
            {
                //lblAssignedTo.Visible = true;
                lblComments.Visible = true;
                //lblDepartment.Visible = true;
                //lblEquipment.Visible = true;
                //lblType.Visible = true;
                //lblResponse.Visible = true;
                //lblCondition.Visible = true;
                //lblOriginator.Visible = true;
            }
        }

        private void initAttachments()
        {
            panelAttachments.Controls.Clear();
            DataTable dt = tools.getAttachments(myHeaderDT.Rows[0]["ID"].ToString());
            Table tbl = new Table();
            tbl.Width = Unit.Percentage(100);
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell tc = new TableCell();

                HyperLink hyp = new HyperLink();
                hyp.Target = "_blank";
                hyp.NavigateUrl = "http://" + _webServer + _appPath + "/attachments/CreateFile.aspx?DatabaseName=" + _dbName + "&tableName=Attachments&ID=" + dr["ID"].ToString();
                hyp.Text = dr["fileName"].ToString();
                hyp.Font.Underline = true;
                hyp.CssClass = "SP_dataLabel";
                hyp.ForeColor = System.Drawing.Color.Red;
                hyp.Font.Bold = true;               
                tc = new TableCell();
                tc.Wrap = false;                
                HyperLink img = new HyperLink();
                img.ImageUrl = "images/AttachmentHS.png";
                tc.Controls.Add(img);
                tc.Controls.Add(hyp);
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Wrap = false;
                HtmlGenericControl gc = new HtmlGenericControl();
                gc.InnerHtml = "<div class='SP_dataLabel'>" + dr["enteredBy"].ToString() + "</div>";
                tc.Controls.Add(gc);
                tr.Cells.Add(tc);

                tc = new TableCell();
                gc = new HtmlGenericControl();
                gc.InnerHtml = "<div class='SP_dataLabel'>" + getFormattedDate(dr["DateEntered"]) + "</div>";
                tc.Controls.Add(gc);
                tr.Cells.Add(tc);

                //if (_editRequest)
                {
                    LinkButton lb = new LinkButton();

                    lb.Text = "Delete";
                    lb.CssClass = "SP_dataLabel";
                    lb.CommandName = "delete";
                    lb.CommandArgument = dr["ID"].ToString();
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Underline = true;
                    lb.ID = "lb_" + dr["ID"].ToString();
                    lb.OnClientClick = "return DeleteAttachment('" + dr["ID"].ToString() + "','" + dr["filename"].ToString() + "')";
                    tc = new TableCell();
                    tc.Controls.Add(lb);
                    tr.Cells.Add(tc);
                }

                tbl.Rows.Add(tr);
            }
            panelAttachments.Controls.Add(tbl);
        }

        private bool fnValidateFields()
        {       
            if (string.IsNullOrEmpty(Request.Form["rbTypes"]))
            {
                hdnErrorMsg.Value = "Please make a selection for Type!";
                return false;
            }

            if (string.IsNullOrEmpty(Request.Form["rbReponse"]))
            {
                hdnErrorMsg.Value = "Please make a selection for Response!";
                return false;
            }

            if (string.IsNullOrEmpty(Request.Form["rbCondition"]))
            {
                hdnErrorMsg.Value = "Please make a selection for Condition!";
                return false;
            }

            if(ddl_AssignedTo.SelectedValue.Length == 0)
            {
                hdnErrorMsg.Value = "Please select a Maintenance Person!";
                return false;
            }
        
            if(ddl_Equipment.SelectedValue.Length == 0)
            {
                hdnErrorMsg.Value = "Please select equipment!";
                return false;
            }

            if(txtComments.Text.Length < 3)
            {
                hdnErrorMsg.Value = "Please enter service required!";
                return false;
            }

            if (txtOriginator.Text.Length < 2)
            {
                hdnErrorMsg.Value = "Please enter an originator!";
                return false;
            }

            //DateTime dtm;

            //if (!DateTime.TryParse(txtRequestedDate.Text, out dtm))
            //{
            //    hdnErrorMsg.Value = "Due date is not a valid date";
            //    return false;
            //}

            //if (dtm < DateTime.Today)
            //{
            //    hdnErrorMsg.Value = "Due date is not a future date";
            //    return false;
            //}            


            return true;
        }

        private void addPopUpToolbar_User()
        {
            PopUpToolbar.PopUpToolbar pt = new PopUpToolbar.PopUpToolbar(_webServer, _appPath, "", "lblPopUpToolbarTitle");
            pt.LoadToolbarItem("", "javascript:fnHideToolBarPopUp_User(); return false;", "GoRtlHS.png", "Return", "");
            pt.LoadToolbarItem("", "", "vert_div.gif", "", "");
            pt.LoadToolbarItem("", "javascript:fnNewUser()", "save16.png", "Save", "");
            HtmlGenericControl gc = new HtmlGenericControl();
            gc.InnerHtml = pt.DisplayDynamicToolbar();
            popUp_NewUser.Controls.AddAt(0, gc);
        }

        private void newUser()
        {
            if (hdnUserID.Value.Length == 0)
            {
                SetErrorMsg("Please enter a user" + "!", hdnErrorMsg);
                return;
            }
            switch (hdnUserType.Value)
            {
                case "Author":
                //    if (ddlAuthor.Items.Cast<ListItem>().Where(w => w.Value.ToUpper() == hdnUserID.Value.ToUpper()).FirstOrDefault() == null)
                //    {
                //        string userName = tools.GetUserName(hdnUserID.Value);
                //        ddlAuthor.Items.Add(new ListItem(userName, hdnUserID.Value));
                //        ddlAuthor.SelectedValue = hdnUserID.Value;
                //    }
                    break;               
            }
            txtName.Text = "";
            hdnUserID.Value = "";
            hdnUserType.Value = "";
        }

        private void addAttachment()
        {
            if (!SaveRequest())
                return;

            Response.Redirect("AddAttachment.aspx?" + Request.QueryString); // BP=" + _BP + "&workflowID=" + workflowID + "&" + tmp);
        }
        
        #region //************************* TEMPLATES ***********************************/
        private void getTemplateList()
        {
            dtTemplateList = workflow2.GetTemplateList(_BP);
            //DataRow dr = dtTemplateList.NewRow();
            //dr["ID"] = 0;
            //dr["Description"] = "";
            //dtTemplateList.Rows.InsertAt(dr, 0);
            ddlWorkflowTemplates.DataSource = dtTemplateList;
            ddlWorkflowTemplates.DataTextField = "Description";
            ddlWorkflowTemplates.DataValueField = "ID";
            ddlWorkflowTemplates.DataBind();
        }

        private void fnCopyTemplateToWorkflow(string action, string templateDescription)
        {
            int overwrite = 0;
            if (action == "2")
            {
                DataTable dt = workflow2.GetSteps((int)myHeaderDT.Rows[0]["WFID"], false);
                overwrite = dt.Rows.Count;
            }
            workflow2.CopyTemplateToWorkflow(_BP, templateDescription, (int)myHeaderDT.Rows[0]["WFID"], overwrite);
        }

        #endregion
    
        #region //******************************** WEB METHODS *******************************/
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetUserNames(string prefixText, int count)
        {
            return QA.Classes.GetNames.GetADUserNames(prefixText, count);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string updateComment(string stepUserID, string commentToAdd, string BP, string userID, bool useAD)
        {
            return WF.WorkflowSteps2.updateComment(stepUserID, commentToAdd, BP, userID, useAD);
        }
        #endregion
    }

}