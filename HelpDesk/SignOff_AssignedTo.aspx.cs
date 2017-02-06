using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace QA
{
    public partial class SignOff_AssignedTo : BaseClass
    {        
        private DataTable dtStepUser;
        private DataTable dtStep;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SetStatus("", lblMsg);
            hdnErrorMsg.Value = "";

            if (workflowID == "0")
            {
                Response.Write("Workflow Number is required");
                Response.End();
            }

            _stepUserID = Request["stepUserID"] + "";
            if (_stepUserID.Length == 0)
            {
                Response.Write("Step user id is required");
                Response.End();
            }

            dtStepUser = workflow2.GetStepUser(_stepUserID, false, false);
            if (dtStepUser.Rows.Count == 0)
            {
                Response.Write("Step user id not found");
                Response.End();
            }

            dtStep = workflow2.GetStep((int)dtStepUser.Rows[0]["stepID"], false);
            _stepID = dtStep.Rows[0]["ID"].ToString();

            if (Request["__EVENTTARGET"] == "Save")
                fnSignOff();

            if (Request["__EVENTTARGET"] == "Cancel")
                Cancel();

            if (!IsPostBack)
            {
                init();
                txtComments.Focus();
            }
        
            ConfigurePage();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }    

        private void ConfigurePage()
        {            
            string stepDescription = "";
            if (dtStepUser.Rows.Count > 0)
            {
                DataTable dt = workflow2.GetStep((int)dtStepUser.Rows[0]["stepID"], false);
                stepDescription = " - " + dt.Rows[0]["description"].ToString();
            }

            Master.HeaderText = _appFriendlyName + " (Sign Off)" + stepDescription + " - " + dtStepUser.Rows[0]["Name"].ToString();
            Master.MyToolbar.LoadToolbarItem("javascript:Cancel();", "", "GoRtlHS.png", "Return", "", "Return");
            
            if (dtStepUser.Rows.Count > 0)
            {
                Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
                Master.MyToolbar.LoadToolbarItem("javascript:fnSave()", "", "save16.png", "Save", "", "Save");                
            }
        }

        private void init()
        {
            DataTable dt = tools.GetMaintenanceUsers(_BP, "isMaintElectrical,isMaintMechanical,isMaintOther");
            
            DataRow newRow = dt.NewRow();
            for(int i = dt.Rows.Count - 1; i>=0; i--)
            {
                if (dt.Rows[i]["userID"].ToString().ToLower() == "unassigned01")
                    dt.Rows.Remove(dt.Rows[i]);
            }

            newRow["friendlyName"] = "";
            newRow["userID"] = "";
            dt.Rows.InsertAt(newRow, 0);

            ddl_Reassign.DataSource = dt;
            ddl_Reassign.DataTextField = "friendlyName";
            ddl_Reassign.DataValueField = "userID";
            ddl_Reassign.DataBind();

            ddl_AddUser.DataSource = dt;
            ddl_AddUser.DataTextField = "friendlyName";
            ddl_AddUser.DataValueField = "userID";
            ddl_AddUser.DataBind();

            if (dtStepUser.Rows.Count > 0)
            {
                string comments = dtStepUser.Rows[0]["Comments"].ToString().Replace("\r\n", "<br />");
                lblComments.Text = comments;
            }            

            if(dtStepUser.Rows[0]["userName"].ToString().ToLower() == "unassigned01")
            {
                ddl_AddUser.Enabled = false;
                cbComplete.Enabled = false;
                txtTimeWorked.Enabled = false;
            }
            else
            {
                bool lastSignOff = true;
                DataTable dtUsers = workflow2.GetStepUsers(int.Parse(_stepID), false, true);
                foreach(DataRow dr in dtUsers.Rows)
                {
                    if ((int)dr["ID"] != int.Parse(_stepUserID) && dr["SignedOff"].ToString() == "0")
                        lastSignOff = false;
                }
                if(!lastSignOff)
                {
                    ddl_Reassign.Enabled = false;
                }
            }
        }

        //private void SignOffStepUsers()
        //{
        //    DataTable dtUsers = workflow2.GetStepUsers((int)dtStep.Rows[0]["ID"], false, true);
        //    foreach(DataRow dr in dtUsers.Rows)
        //    {
        //        dr["SignedOff"] = 1;
        //    }
        //    workflow2.updateUsers(dtUsers, false);
        //}

        public void insertStep()
        {
            int newStepID = workflow2.InsertStep(int.Parse(workflowID), "Re-Assigned", dtStep.Rows[0]["StepNumber"].ToString(), false, false);
            DataTable dtNewStep = workflow2.GetStep(newStepID, false);
            dtNewStep.Rows[0]["VirtualStep"] = true;
            dtNewStep.Rows[0]["StartTime"] = DateTime.Now;
            dtNewStep.Rows[0]["detailURL"] = "signOff_AssignedTo.aspx";
            dtNewStep.Rows[0]["EmailReminderFrequency"] = dtStep.Rows[0]["EmailReminderFrequency"];
            dtNewStep.Rows[0]["PastDueLimit"] = dtStep.Rows[0]["PastDueLimit"];
            workflow2.UpdateStep(dtNewStep, false);

            DataTable dt = workflow2.GetStepUser("-1", false, true);
            DataRow newRow = dt.NewRow();
            newRow["stepID"] = newStepID;
            newRow["userName"] = ddl_Reassign.SelectedValue;
            newRow["fieldName"] = "Reassigned";
            newRow["responseRequired"] = 1;
            newRow["Rejected"] = 0;
            newRow["SignedOff"] = 0;
            newRow["DateTimeStamp"] = DateTime.Now.ToShortDateString();
            newRow["LastModifiedBy"] = userID;
            newRow["RoleBasedUser"] = 0;
            newRow["LastEmailed"] = DateTime.Now;            
            dt.Rows.Add(newRow);

            workflow2.updateUsers(dt, false);
        }
                
        private void fnSignOff()
        {
            decimal timeWorked = 0;
            if (txtTimeWorked.Enabled && (txtTimeWorked.Text.Length > 0 || ddl_Reassign.SelectedValue.Length > 0 || cbComplete.Checked))
            {            
                if ((txtTimeWorked.Text.Length == 0) || !decimal.TryParse(txtTimeWorked.Text, out timeWorked))
                {
                    hdnErrorMsg.Value = "Please enter a valid time worked!";
                    return;
                }               
            }

            string comments = "";
            hdnErrorMsg.Value += addComment(ref comments, dtStepUser.Rows[0]["Comments"].ToString(), "");
            hdnErrorMsg.Value += addComment(ref comments, txtComments.Text, userFriendlyName);            
            if (hdnErrorMsg.Value.Length > 0)
                return;

            if (cbComplete.Checked)
            {
                if (txtComments.Text.Length < 3)
                {
                    hdnErrorMsg.Value = "Please enter a comment!";
                    return;
                }
                dtStepUser.Rows[0]["SignedOff"] = 1;
            }

            if(ddl_Reassign.SelectedValue.Length > 0)
            {
                hdnErrorMsg.Value += addComment(ref comments, "Work order reassigned", userFriendlyName);
                if (hdnErrorMsg.Value.Length > 0)
                    return;

                insertStep();
                
                dtStepUser.Rows[0]["SignedOff"] = 1;                
            }
            else if (ddl_AddUser.SelectedValue.Length > 0)
            {
                DataTable dt = workflow2.GetStepUser("-1", false, true);
                DataRow newRow = dt.NewRow();
                newRow["stepID"] = dtStep.Rows[0]["ID"];
                newRow["userName"] = ddl_AddUser.SelectedValue;
                newRow["fieldName"] = "Reassigned";
                newRow["responseRequired"] = 1;
                newRow["Rejected"] = 0;
                newRow["SignedOff"] = 0;
                newRow["DateTimeStamp"] = DateTime.Now;
                newRow["LastEmailed"] = DateTime.Now;
                newRow["LastModifiedBy"] = userID;
                newRow["RoleBasedUser"] = 0;
                dt.Rows.Add(newRow);
                workflow2.updateUsers(dt, false);
            }            

            dtStepUser.Rows[0]["Comments"] = comments; 
            dtStepUser.Rows[0]["DateTimeStamp"] = DateTime.Now;
            dtStepUser.Rows[0]["lastModifiedBy"] = userID;
            dtStepUser.Rows[0]["Rejected"] = 0;            
            //dtStepUser.Rows[0]["userName"] = 
            workflow2.updateUsers(dtStepUser, false);

            if (workflow2.StepComplete(int.Parse(_stepID), 0))
            {
                dtStep.Rows[0]["EndTIme"] = DateTime.Now;
                workflow2.fnUpdateStep(dtStep, "");

                DataTable dtA = workflow2.GetCurrentActiveSteps(int.Parse(workflowID));
                if (dtA.Rows.Count == 0)
                    workflow2.fnUpdateHeaderStatus(int.Parse(workflowID), CWF_WorkflowRouting.Routing2.DBStatus.Complete, false);
            }

            if (timeWorked > 0)
            {
                DataTable dtTime = tools.getTimeWorked("0");
                DataRow newRow = dtTime.NewRow();
                newRow["HeaderID"] = myHeaderDT.Rows[0]["ID"];
                newRow["TimeWorked"] = timeWorked;
                newRow["UserID"] = dtStepUser.Rows[0]["userName"];
                newRow["DateTimeStamp"] = DateTime.Now;
                dtTime.Rows.Add(newRow);
                tools.updateTimeWorked(dtTime);
            }

            if(cbComplete.Checked)
            {
                //workflow2.fnUpdateHeaderStatus(int.Parse(workflowID), CWF_WorkflowRouting.Routing2.DBStatus.Complete, false);
            }

            //if (ddl_Reassign.SelectedValue.Length > 0)
              //  SignOffStepUsers();

            Cancel();
        }

        private void Cancel()
        {
            Response.Redirect(_appPath + "/mainForm.aspx?BP=" + _BP + "&workflowID=" + workflowID);
        }       
    }
}
