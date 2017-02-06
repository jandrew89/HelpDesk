using System;
using System.Data;
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
using CWF_WorkflowRouting;
using CWF_HelpDesk;

namespace WF
{
    public partial class WorkflowSteps2 : BaseClass_WF_UC 
    {
        public int _tableWidth = 600;     
     
        public string _AutoLoadTemplateName = "";
        public int _TemplateAppendStep = 0;        
        public bool _manualMenuSetup = false;        

        public string _border = "0";
        public bool usesAD = true;

        public delegate void UCMessage(string value);
        public event UCMessage workflowMessage;

        WF_PopUpMenu menu1 = new WF_PopUpMenu("myMenu1");
        WF_PopUpMenu menu2 = new WF_PopUpMenu("myMenu2");

        protected void Page_Load(object sender, EventArgs e)
        {
            lblHeaderLabel.Text = "Workflow";

            if (Request["__EVENTTARGET"] == "DeleteStep")
            {
                DeleteStep(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "ResetStep")
            {
                ResetStep(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "MoveUp")
            {
                MoveUp(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "MoveDown")
            {
                MoveDown(Request["__EVENTARGUMENT"]);
            }

            if (Request["__EVENTTARGET"] == "Email")
            {
                Email(Request["__EVENTARGUMENT"]);
            }

            //if (Request["__EVENTTARGET"] == "addComment")
            //{
            //    addComment(Request["__EVENTARGUMENT"]);                
            //}

            buildMenus();
            loadData();
            displayMenus();

            if (_AutoLoadTemplateName != "")
                loadTemplate(_AutoLoadTemplateName);
        }
      
        private void buildMenus()
        {
            WF_PopUpMenuItem item1 = new WF_PopUpMenuItem();            

            //****************************** menu1 *************************************
            item1 = new WF_PopUpMenuItem();
            item1.Text = "Insert Step";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Add16.png";
            item1.hREF = "javascript:fnMenuInsertStep()";            
            menu1.menuItems.Add(item1);

            //item1 = new FE_PopUpMenuItem();
            //item1.isSeparator = true;
            //menu1.menuItems.Add(item1);

            //item1 = new FE_PopUpMenuItem();
            //item1.Text = wf.TranslatePage("Templates");
            //item1.ImageURL = item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Settings16.png";
            //item1.hREF = "javascript:fnTemplates()";
            //menu1.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu1.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Cancel";
            menu1.menuItems.Add(item1);

            //****************************** menu2 *************************************
            item1 = new WF_PopUpMenuItem();
            item1.Text = "Modify";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Edit16.png";
            item1.hREF = "javascript:fnMenuModifyStep()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Insert";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Add16.png";
            item1.hREF = "javascript:fnMenuInsertStep()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Reset";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/refresh16.png";
            item1.hREF = "javascript:fnMenuResetStep()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Send Mail";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Send (Email)16.png";
            item1.hREF = "javascript:fnMenuEmail()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Delete";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Delete16.png";
            item1.hREF = "javascript:fnMenuDeleteStep()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Move Up";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Move Up (Arrow)16.png";
            item1.hREF = "javascript:fnMenuMoveUp()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Move Down";
            item1.ImageURL = "http://" + wf._webServer + wf._appPath + "/workflow/images/Move Down (Arrow)16.png";
            item1.hREF = "javascript:fnMenuMoveDown()";
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.isSeparator = true;
            menu2.menuItems.Add(item1);

            item1 = new WF_PopUpMenuItem();
            item1.Text = "Cancel";
            menu2.menuItems.Add(item1);
        }

        private void displayMenus()
        {
            if (wf._isMailMessage)
            {
                //panelComment.Visible = false;
                return;
            }

            List<WF_PopUpMenu> menus = new List<WF_PopUpMenu>();
            menu1.appImagesPath = wf._webServer + wf._appPath + "/workflow/images";
            menu2.appImagesPath = wf._webServer + wf._appPath + "/workflow/images";
            menus.Add(menu1);
            menus.Add(menu2);

            string retval = WF_PopUp.writeMenus(this.Page, menus);
            Literal l = new Literal();
            l.Text = retval;
            phMenu1.Controls.Add(l);
        }
      
        public void loadData()
        {
            panelSteps.Controls.Clear();            
            DataTable StepTable = wf.workflow2.GetSteps(Convert.ToInt32(wf._workflowID), false);
            
            // Only default menu items if not being setup manually by host        
            if (!_manualMenuSetup)
            {
                foreach (WF_PopUpMenuItem item in menu2)
                    item.Enabled = wf._enableStepMenu;               
            }
            
            Table tb1 = wf.drawSteps(int.Parse(wf._workflowID), false, _tableWidth, wf._enableStepMenu);                                    
            panelSteps.Controls.Add(tb1);                                  
        }        

        public void loadTemplate(string templateName)
        {            
            if (Convert.ToInt32(wf._workflowID) < 1)
                return;

            wf.workflow2.CopyTemplateToWorkflow(wf._BP, templateName, Convert.ToInt32(wf._workflowID), _TemplateAppendStep);
            loadData();
            workflowMessage("0");
        }

        //private void addComment(string stepUsersID)
        //{          
        //    DataTable dtStepUser = wf.workflow2.GetStepUser(stepUsersID, false, true);
        //    if (dtStepUser.Rows.Count > 0 && txtComments.Text.Length > 0)
        //    {
        //        string userName = wf._userID;
        //        FE_AD.ADProfile pro = wf.workflow2.fnGetADUserProfile(wf._userID);
        //        if (pro.displayName.Length > 0)
        //            userName = pro.displayName;
        //        if (dtStepUser.Rows[0]["Comments"].ToString().Length > 0)
        //            dtStepUser.Rows[0]["Comments"] += "\r\n";
        //        dtStepUser.Rows[0]["Comments"] += txtComments.Text + " - " + userName + " " + DateTime.Now.ToString();
        //        wf.workflow2.updateUsers(dtStepUser, false);
        //        txtComments.Text = "";
        //        loadData();
        //    }
        //}
      
        #region //*************** PROPERTIES *****************/       
        public string AutoLoadTemplateName
        {
            get
            { return _AutoLoadTemplateName; }
            set
            { _AutoLoadTemplateName = value; }
        }

        public bool mnuTemplates
        {
            get
            { return menu1.menuItems[2].Enabled; }
            set
            {
                menu1.menuItems[2].Enabled = value;
            }
        }

        public bool mnuModifyStep
        {
            get
            { return menu2.menuItems[0].Enabled; }
            set
            {
                menu2.menuItems[0].Enabled = value;
                menu1.menuItems[0].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public bool mnuInsertStep
        {
            get
            { return menu2.menuItems[1].Enabled; }
            set
            {
                menu2.menuItems[1].Enabled = value;
                menu1.menuItems[0].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public bool mnuResetStep
        {
            get
            { return menu2.menuItems[2].Enabled; }
            set
            {
                menu2.menuItems[2].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public bool mnuEMailStep
        {
            get
            { return menu2.menuItems[4].Enabled; }
            set
            {
                menu2.menuItems[4].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public bool mnuDeleteStep
        {
            get
            { return menu2.menuItems[6].Enabled; }
            set
            {
                menu2.menuItems[6].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public bool mnuMoveUpStep
        {
            get
            { return menu2.menuItems[8].Enabled; }
            set
            {
                menu2.menuItems[8].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public bool mnuMoveDownStep
        {
            get
            { return menu2.menuItems[9].Enabled; }
            set
            {
                menu2.menuItems[9].Enabled = value;
                _manualMenuSetup = true;
            }
        }

        public int TemplateAppendStep
        {
            get
            { return _TemplateAppendStep; }
            set
            { _TemplateAppendStep = value; }
        }      
        #endregion

        #region //************************* EVENTS ****************************/
        //protected void btnModifyComment_Click(object sender, EventArgs e)
        //{            
        //    addComment(hdnStepUserID.Value);
        //}

        private void DeleteStep(string ID)
        {
            wf.workflow2.DeleteStep(int.Parse(ID), false);            
            loadData();
        }

        private void ResetStep(string ID)
        {
            wf.workflow2.ResetSteps(int.Parse(wf._workflowID), int.Parse(ID));            
            workflowMessage("0");
            loadData();
        }

        private void Email(string ID)
        {
            wf.workflow2.ReSendEmail(int.Parse(wf._workflowID), int.Parse(ID));            
            loadData();
        }

        private void MoveUp(string ID)
        {
            wf.workflow2.MoveStep(wf._workflowID, ID, false, false);            
            loadData();
        }

        private void MoveDown(string ID)
        {
            wf.workflow2.MoveStep(wf._workflowID, ID, true, false);            
            loadData();
        }
              
        #endregion

        #region //******************************** WEB METHODS *******************************/     
        public static string updateComment(string stepUserID, string commentToAdd, string BP, string userID, bool usesAD)
        {            
            try
            {
               
                CWF_Security.SecurityCredentials SecCred = new SecurityCredentials();
                CWF_Corporate.Authentication AU = new CWF_Corporate.Authentication(SecCred, usesAD);
                CWF_HelpDesk.Workflow2 workflow2 = new CWF_HelpDesk.Workflow2(SecCred, BP, userID, null, "", 700, usesAD);
                
                DataTable dtStepUser = workflow2.GetStepUser(stepUserID, false, true);
                if (dtStepUser.Rows.Count > 0)
                {
                    string userName = userID;
                    CWF_Corporate.Profile pro = AU.GetUserProfile(userID);
                    if (pro.friendlyName.Length > 0)
                        userName = pro.friendlyName;

                    string comments = "";
                    string retval = QA.BaseClass.addComment(ref comments, dtStepUser.Rows[0]["Comments"].ToString(), "");
                    retval += QA.BaseClass.addComment(ref comments, commentToAdd, userName);
                   
                    if (retval.Length == 0)
                    {
                        dtStepUser.Rows[0]["Comments"] = comments;
                        workflow2.updateUsers(dtStepUser, false);
                        return comments;
                    }
                    else
                        return retval;
                }
                return "Error:No step user record found!";
            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }
        }      
        #endregion
    }
}
