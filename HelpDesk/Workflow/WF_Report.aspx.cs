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

namespace WF
{
    public partial class _Report : WF.BaseClass_WF_Page
    {
        public int _tableWidth = 600;        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScript.GetPostBackClientHyperlink(new Control(), string.Empty);
            
            if (!IsPostBack)
            {                
                
            }
            
            Connect();
            ConfigurePage();            
        }
       
        private void ConfigurePage()
        {            
            Master.HeaderText = wf._appFriendlyName + " - Workflows - " + wf._BP;
            if (Request["showCancel"] != null)
            {
                Master.MyToolbar.LoadToolbarItem("javascript:fnClose();", "", "Undo16.png", "Return", "", "Return");
            }
        }

        //private void Cancel()
        //{
        //    ClientScriptManager csm = Page.ClientScript;
        //    string tmp1 = "fnClose(); ";
        //    csm.RegisterStartupScript(GetType(), "startup1", tmp1, true);
        //}

        private void Connect()
        {
            phControls.Controls.Clear();
            DataTable dt;
            if (Request["templateID"] != null)
                dt = wf.workflow2.GetTemplate(Request["templateID"]);
            else
                dt = wf.workflow2.GetTemplateList(wf._BP);
            foreach(DataRow dr2 in dt.Rows)
            {
                Label lbl = new Label();
                lbl.Font.Bold = true;
                lbl.Font.Name = "Tahoma";
                lbl.Font.Size = FontUnit.Point(11);
                lbl.Text = dr2["Description"].ToString();
                phControls.Controls.Add(lbl);

                Table tb1 = wf.drawSteps((int)dr2["ID"], true, _tableWidth, false);
                phControls.Controls.Add(tb1);
            
                HtmlGenericControl gc1 = new HtmlGenericControl();
                gc1.InnerHtml = "<br />";
                phControls.Controls.Add(gc1);
            }
        }     
    }
}