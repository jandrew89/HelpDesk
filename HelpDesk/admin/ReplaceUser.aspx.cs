using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace QA
{
    public partial class clsReplaceUser : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin)
            {
                Response.Write("Admin rights are required!");
                Response.End();
            }

            txtName.Attributes.Add("autocomplete", "off");
            txtNewUser.Attributes.Add("autocomplete", "off");
            
            if (Request["__EVENTTARGET"] == "Replace")
                ReplaceUser();

            if (Request["__EVENTTARGET"] == "Confirmed")
            {
                ReplaceUser();
            }
          
            ConfigurePage();
            txtName.Focus();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }

        private void ConfigurePage()
        {            
            Master.Title = _appFriendlyName + " - Replace User";
            Master.HeaderText = Master.Title;
            Master.MyToolbar.LoadToolbarItem("", "javascript:Return();", "GoRtlHS.png", "Return", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:Replace();", "save16.png", "Replace", "");
        }

        private void ReplaceUser()
        {           
            bool currentUserMissing = false;
            bool newUserMissing = false;
            string currentUserID = "";
            string newUserID = "";
            string errorMsg = "";

            validateUser(txtName.Text, out currentUserID, out currentUserMissing, out errorMsg);
            if (errorMsg.Length > 0)
            {
                SetErrorMsg(errorMsg, hdnErrorMsg);
                return;
            }

            validateUser(txtNewUser.Text, out newUserID, out newUserMissing, out errorMsg);
            if (errorMsg.Length > 0)
            {
                SetErrorMsg(errorMsg, hdnErrorMsg);
                return;
            }            

            if (currentUserID.Length > 0 && newUserID.Length > 0)
            {
                workflow2.ReplaceUser(currentUserID, newUserID);
            }
            else
            {
                if (Request["__EVENTARGUMENT"] != "confirmed")
                {
                    string tmp = "";
                    if (currentUserMissing)
                        tmp = "Current User " + txtName.Text + " not recognized!";
                    if (newUserMissing)
                        tmp += " New User " + txtNewUser.Text + " not recognized!";
                    registerStartUpScript("startUp", "fnConfirm('" + tmp + " - Continue?', 'confirmed');");
                    return;
                }
                else
                {
                    if (currentUserMissing)
                        currentUserID = txtName.Text;
                    if (newUserMissing)
                        newUserID = txtNewUser.Text;
                    workflow2.ReplaceUser(currentUserID, newUserID);
                }
            }

            Response.Redirect("../admin.aspx?BP=" + _BP);
        }

        public void registerStartUpScript(string scriptKey, string script)
        {
            ClientScriptManager csm = Page.ClientScript;            
            csm.RegisterStartupScript(this.GetType(), scriptKey, script, true);
        }

        #region //******************************** WEB METHODS *******************************/
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetUserNames(string prefixText, int count)
        {
            return QA.Classes.GetNames.GetADUserNames(prefixText, count);
        }
        #endregion
    }
}