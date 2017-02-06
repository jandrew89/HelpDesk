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


namespace QA
{
    public partial class FindUserResults : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin)
            {
                Response.Write("Admin rights are required!");
                Response.End();
            }
                      
            if (!IsPostBack)
            {
                Connect();                             
            }

            ConfigurePage();            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }       
      
        private void ConfigurePage()
        {
            string userName = Request["userID"];
            string FN, eMail;
            workflow2.getProfileAttributes(userName, out FN, out eMail);            
            if (FN.Length > 0)
                userName = FN;
            Master.HeaderText = _appFriendlyName + "&nbsp;-&nbsp;" + "Find User in Templates&nbsp;-&nbsp;" + userName;
            Master.MyToolbar.LoadToolbarItem("javascript:Return();", "GoRtlHS.png", "Return");
        }

        private void Connect()
        {            
            DataTable dt = workflow2.GetUserInWorkflow(Request["userID"], false);
            GV.DataSource = dt;
            GV.DataBind();
        }
      
        #region //************************** EVENTS *****************************/
        protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int WFID = (int)DataBinder.Eval(e.Row.DataItem, "WFID");
                string BP = (string)DataBinder.Eval(e.Row.DataItem, "BP");
                string templateName = (string)DataBinder.Eval(e.Row.DataItem, "templateName");
                HyperLink hyp = (HyperLink)e.Row.FindControl("hypTemplate");
                hyp.Text = templateName;
                hyp.NavigateUrl = "WF_Manager.aspx?BP=" + BP + "&workflowID=" + WFID;
            }
        }       
        #endregion
    }
}