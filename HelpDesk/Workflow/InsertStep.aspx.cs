using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using CWF_WorkflowRouting;

namespace WF
{
    public partial class WFInsertStep : BaseClass_WF_Page
    {        
        public string _border = "0";     

        protected void Page_Load(object sender, EventArgs e)
        {
            wf.SetStatus("", BlockLabel);            
            Master.BaseTarget = "_self";

            ClientScriptManager csm = Page.ClientScript;
            csm.RegisterClientScriptBlock(this.GetType(), "onkeypress", "<body onKeyPress=fnEsc(event)>", false);          

            if (!IsPostBack)
            {
                if (wf._stepID != "0")
                {
                    DataTable dt = wf.workflow2.GetStep(Convert.ToInt32(wf._stepID), false);
                    lblStepDescription.Text = " -&nbsp;&nbsp; (" + dt.Rows[0]["Description"].ToString() + ")";
                }
            }
            
            ConfigurePage();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }

        private void ConfigurePage()
        {            
            Master.Title = "Insert Step";        
        }        

        public void InsertNewStep()
        {
            if (txtStepTitle.Text.Length == 0)
            {
                wf.SetStatus("Please enter a title for this step!", BlockLabel);
                return;
            }            

            bool insertBefore = true;
            if (rbAfter.Checked == true)
                insertBefore = false;

            string stepNumber = "0";
            DataTable dt = wf.workflow2.GetStep(Convert.ToInt32(wf._stepID), false);
            if (dt.Rows.Count > 0)
                stepNumber = dt.Rows[0]["StepNumber"].ToString();            

            int newStepID = wf.workflow2.InsertStep(int.Parse(wf._workflowID), txtStepTitle.Text, stepNumber, insertBefore, false);            
            Response.Redirect("ModifyUsers.aspx?Template=false&refreshPage=1&BP=" + wf._BP + "&workflowID=" + wf._workflowID + "&stepID=" + newStepID.ToString());            
        }
                
        #region //********************************** EVENTS *********************************/
        protected void SaveUsers_Click(object sender, EventArgs e)
        {
            InsertNewStep();            
        }        
        #endregion
    }    
}
