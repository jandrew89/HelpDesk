using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace QA
{
    public partial class SignOff : BaseClass
    {        
        private DataTable dtStepUser;
        private DataTable dtStep;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SetStatus("", lblMsg);

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

            dtStepUser = workflow2.GetStepUser(_stepUserID, false, true);
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
                init();
        
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

            Master.HeaderText = _appFriendlyName + " (Sign Off)" + stepDescription;
            Master.MyToolbar.LoadToolbarItem("javascript:Cancel();", "", "GoRtlHS.png", "Return", "", "Return");
            if (dtStepUser.Rows.Count > 0)
            {
                Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
                Master.MyToolbar.LoadToolbarItem("javascript:fnSave()", "", "save16.png", "Save", "", "Save");                
            }
        }

        private void init()
        {
            
            if (dtStepUser.Rows.Count > 0)
            {
                txtComments.Text = dtStepUser.Rows[0]["Comments"].ToString();            
            }            
        }
        
        private void fnSignOff()
        {
            if (rbSignoffToggle.SelectedValue == "2")
            {
                if (txtComments.Text.Length == 0)
                {
                    SetStatus("Please enter a comment!", lblMsg);
                    return;
                }
            }

            dtStepUser.Rows[0]["Comments"] = txtComments.Text;
            dtStepUser.Rows[0]["DateTimeStamp"] = DateTime.Now;
            dtStepUser.Rows[0]["lastModifiedBy"] = userID;
            dtStepUser.Rows[0]["Rejected"] = 0;

            if (rbSignoffToggle.SelectedValue == "2")
                dtStepUser.Rows[0]["Rejected"] = 1;
            else
                dtStepUser.Rows[0]["SignedOff"] = Convert.ToByte(rbSignoffToggle.SelectedValue);
            if (userID.ToLower() != dtStepUser.Rows[0]["UserName"].ToString().ToLower())
                dtStepUser.Rows[0]["Impersonation"] = userID;
            workflow2.updateUserData(dtStepUser);

            if (rbSignoffToggle.SelectedValue == "2")
                SendRejectEmail(txtComments.Text);
            else
                CheckFinalStepAction(dtStep);

            Cancel();
        }

        private void Cancel()
        {
            Response.Redirect(_appPath + "/mainForm.aspx?BP=" + _BP + "&workflowID=" + workflowID);
        }       
    }
}
