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
    public partial class _Manager : WF.BaseClass_WF_Page
    {
        public int _tableWidth = 600;
        WF_PopUpMenu menu2 = new WF_PopUpMenu("myMenu2");

        protected void Page_Load(object sender, EventArgs e)
        {
            wf.SetStatus("", lblMsg);

            if (!wf._isAdmin)
            {
                Response.Write("Admin rights are required!");
                Response.End();
            }

            if (Request["__EVENTTARGET"] == "templateChanged")
            {
                templateChanged();
            }

            if (Request["__EVENTTARGET"] == "locationChanged")
            {
                locationChanged();
            }

            ddl_Templates.Attributes.Add("onchange", "fnTemplateChanged()");
            ddl_Location.Attributes.Add("onchange", "fnLocationChanged()");

            if (!IsPostBack)
            {                
                txtLocation.Text = wf._BP;
                getTemplateList();
                if(ddl_Templates.Items.Count > 0)
                    txtNewTemplateName.Text = ddl_Templates.SelectedItem.Text;

                initLocation();
            }            

            if (Request["__EVENTTARGET"] == "insertStep")
            {            
                insertStep(Request["__EVENTARGUMENT"], ddl_Templates.SelectedValue);
            }

            if (Request["__EVENTTARGET"] == "modifyStep")
            {
                modifyStep(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "deleteStep")
            {
                deleteStep(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "moveUp")
            {
                moveUp(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "moveDown")
            {
                moveDown(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "deleteTemplate")
            {
                deleteTemplate();
            }

            if (Request["__EVENTTARGET"] == "newTemplate")
            {
                newTemplate(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "copyTemplate")
            {
                copyTemplate();
            }

            Connect();
            ConfigurePage();
            buildMenus();
            displayMenus();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }       
      
        private void ConfigurePage()
        {
            Master.HeaderText = wf._appFriendlyName + " - Workflows - " + wf.workflow2.getFriendlyBPName(wf._BP);
            Master.MyToolbar.LoadToolbarItem("", "javascript:Return();", "GoRtlHS.png", "Return", "");
            Master.MyToolbar.LoadToolbarItem(new toolbarDelegate(ddlLocation));
            Master.MyToolbar.LoadToolbarItem(new toolbarDelegate(ddlTemplate));
            Master.MyToolbar.LoadToolbarItem("", "javascript:newTemplate();", "User Add16.png", "New Template", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:copyTemplate();", "tool-copy.gif", "Copy Template", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:deleteTemplate();", "delete16.png", "Delete Template", "");
            Master.MyToolbar.LoadToolbarItem("WF_Report.aspx?BP=" + wf._BP, "", "tool-preview.gif", "Report", "_blank");
        }

        public string ddlLocation()
        {
            DataTable dt = wf.workflow2.GetTemplateLocations(wf._userID);
            string tmp = "<li class=\"dropdown hover\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-globe fa-lg\"></i>" + " Locations " + "<span class=\"caret\"></span></a>";
            tmp += "<ul class=\"dropdown-menu hover\">";
       

            foreach (DataRow dr in dt.Rows)
            {
                tmp += "<li><a href=\"/workflow/WF_Manager.aspx?BP=" + dr["BP"] + "\">" + dr["friendlyBPName"] + "</a></li>";
            }

            tmp += "</ul></li>";
            return tmp;
        }

        public string ddlTemplate()
        {
            DataTable dt = wf.workflow2.GetTemplateList(wf._BP);
            string tmp = "<li class=\"dropdown hover\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-file-text fa-lg\"></i>" + " Template Name " + "<span class=\"caret\"></span></a>";
            tmp += "<ul class=\"dropdown-menu hover\">";


            foreach (DataRow dr in dt.Rows)
            {
                tmp += "<li><a href=\"/workflow/WF_Manager.aspx?BP=" + dr["ID"] + "\">" + dr["Description"] + "</a></li>";
            }

            tmp += "</ul></li>";
            return tmp;
        }

        private void getTemplateList()
        {
            DataTable dt = wf.workflow2.GetTemplateList(wf._BP);
            ddl_Templates.DataSource = dt;
            ddl_Templates.DataTextField = "Description";
            ddl_Templates.DataValueField = "ID";
            ddl_Templates.DataBind();
            if (ddl_Templates.Items.FindByValue(wf._workflowID) != null)
                ddl_Templates.SelectedValue = wf._workflowID;
            else if(ddl_Templates.Items.Count > 0)
                Response.Redirect("WF_Manager.aspx?BP=" + wf._BP + "&workflowID=" + ddl_Templates.SelectedValue);
        }

        private void initLocation()
        {
            DataTable dt = wf.workflow2.GetTemplateLocations(wf._userID);
            ddl_Location.DataSource = dt;
            ddl_Location.DataTextField = "friendlyBPName";
            ddl_Location.DataValueField = "BP";
            ddl_Location.DataBind();
            if (ddl_Location.Items.FindByValue(wf._BP) != null)
                ddl_Location.SelectedValue = wf._BP;
            else if (dt.Rows.Count > 0)
                Response.Redirect("WF_Manager.aspx?BP=" + dt.Rows[0]["BP"]);
        }

        private void Connect()
        {
            panelSteps.Controls.Clear();
            if (ddl_Templates.Items.Count > 0)
            {
                Table tb1 = wf.drawSteps(int.Parse(ddl_Templates.SelectedValue), true, _tableWidth, true);
                panelSteps.Controls.Add(tb1);
            }
        }
      
        private void modifyStep(string stepID)
        {
            Response.Redirect("ModifyUsers.aspx?Template=true&BP=" + wf._BP + "&workflowID=" + ddl_Templates.SelectedValue + "&stepID=" + stepID);
        }

        private void insertStep(string stepID, string workflowID)
        {           
            string stepNumber = "0";
            DataTable dt = wf.workflow2.GetStep(Convert.ToInt32(stepID), true);
            if (dt.Rows.Count > 0)
                stepNumber = dt.Rows[0]["StepNumber"].ToString();
            int newStepID = wf.workflow2.InsertStep(int.Parse(workflowID), "New Step", stepNumber, false, true);
            Response.Redirect("ModifyUsers.aspx?Template=true&BP=" + wf._BP + "&workflowID=" + workflowID + "&stepID=" + newStepID);
        }

        private void deleteStep(string stepID)
        {
            wf.workflow2.DeleteStep(Convert.ToInt32(stepID), true);
            Connect();
        }

        private void moveUp(string stepID)
        {
            wf.workflow2.MoveStep(wf._workflowID, stepID, false, true);
            Connect();
        }

        private void moveDown(string stepID)
        {
            wf.workflow2.MoveStep(wf._workflowID, stepID, true, true);
            Connect();
        }

        private void deleteTemplate()
        {
            wf.workflow2.DeleteTemplate(ddl_Templates.SelectedValue);
            getTemplateList();
            Connect();
        }

        private void newTemplate(string templateName)
        {
            DataTable dt = wf.workflow2.GetTemplate(wf._BP, templateName);
            if (dt.Rows.Count > 0)
            {
                wf.SetStatus("This name already exists!", lblMsg);
                return;
            }

            string newTemplateID = wf.workflow2.CopyWorkflowToTemplate(wf._BP, -1, templateName);            
            insertStep("0", newTemplateID);                        
        }

        private void copyTemplate()
        {
            if (txtLocation.Text.Length == 0)
            {
                wf.SetStatus("Please select a location!", lblMsg);
                return;
            }

            DataTable dt = wf.workflow2.GetTemplate(txtLocation.Text, txtNewTemplateName.Text);
           if (dt.Rows.Count > 0)
           {
               wf.SetStatus("This name already exists!", lblMsg);
               return;
           }

            string newTemplateID = wf.workflow2.CopyTemplateWorkflowToNewTemplateWorkflow(ddl_Templates.SelectedValue, txtNewTemplateName.Text, txtLocation.Text);
            Response.Redirect("WF_Manager.aspx?BP=" + txtLocation.Text + "&workflowID=" + newTemplateID);
        }

        private void displayMenus()
        {
            List<WF_PopUpMenu> menus = new List<WF_PopUpMenu>();
            menu2.appImagesPath = wf._webServer + wf._appPath + "/workflow/images";
            menus.Add(menu2);

            string retval = WF_PopUp.writeMenus(this.Page, menus);
            Literal l = new Literal();
            l.Text = retval;
            phMenu1.Controls.Add(l);
        }

        private void buildMenus()
        {
            WF_PopUpMenuItem item1 = new WF_PopUpMenuItem();
         
            //****************************** menu2 *************************************
            item1 = new WF_PopUpMenuItem();
            item1.Text = "Modify";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/images/Edit16.png";
            item1.hREF = "javascript:fnMenuModifyStep()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Insert";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/images/Add16.png";
            item1.hREF = "javascript:fnMenuInsertStep()";
            menu2.menuItems.Add(item1);           

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);          

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Delete";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/images/Delete16.png";
            item1.hREF = "javascript:fnMenuDeleteStep()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Move Up";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/images/Move Up (Arrow)16.png";
            item1.hREF = "javascript:fnMenuMoveUp()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Move Down";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/images/Move Down (Arrow)16.png";
            item1.hREF = "javascript:fnMenuMoveDown()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Cancel";
            menu2.menuItems.Add(item1);
        }
    
        #region //***************************************** EVENTS ************************************/
        public void templateChanged()
        {
            Response.Redirect("WF_Manager.aspx?BP=" + wf._BP + "&workflowID=" + ddl_Templates.SelectedValue);
        }

        public void locationChanged()
        {
            Response.Redirect("WF_Manager.aspx?BP=" + ddl_Location.SelectedValue);
        }        
        #endregion       
    }
}