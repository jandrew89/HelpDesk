using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QA
{

    public partial class Admin : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!_isAdmin)
            {
                //Response.Write(TranslatePage("Admin rights are required") + "!");
                //Response.End();                
                ///lbDeleteID.Visible = false;
            }

            if (!IsPostBack)
            {
                //lblWorkflowIDLabel.Text = "Workflow ID";                
            }
            ConfigurePage();
        }

        private void ConfigurePage()
        {
            Master.Title = _appFriendlyName;
            Master.HeaderText = " " + _appFriendlyName + " - " + "Administration";
            Master.MyToolbar.LoadToolbarItem("javascript:Return();", "GoRtlHS.png", "Return");            
        }

        #region //************** EVENTS *************************/
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (txtWorkflowID.Text.Length == 0)
        //        return;
        //    int WorkflowID = 0;
        //    if (!int.TryParse(txtWorkflowID.Text, out WorkflowID))
        //    {
        //        SetErrorMsg("Request ID must be numeric.", hdnErrorMsg);
        //        return;
        //    }
        //    DataTable dt = tools.getHeader(WorkflowID.ToString(), true);
        //    if (dt.Rows.Count == 0)
        //    {
        //        SetErrorMsg("Request ID was not found.", hdnErrorMsg);
        //        return;
        //    }

        //    int workflowID = 0;
        //    int.TryParse(dt.Rows[0]["WFID"].ToString(), out workflowID);
        //    if (workflowID > 0)
        //        workflow2.DeleteAllWorkflow(workflowID);

        //    tools.deleteHeader(WorkflowID.ToString());

        //    SetStatus("Request has been deleted" + ".", lblMsg, System.Drawing.Color.Blue);

        //    txtWorkflowID.Text = "";            
        //}

       
        #endregion
    }
}