using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace QA
{
    public partial class FindUser : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isAdmin)
            {
                Response.Write("Admin rights are required!");
                Response.End();
            }

            txtName.Attributes.Add("autocomplete", "off");

            if (Request["__EVENTTARGET"] == "Search")
                Search();
          
            ConfigurePage();
            txtName.Focus();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
         
        }

        private void ConfigurePage()
        {            
            Master.Title = _appFriendlyName + "&nbsp;-&nbsp;Find User in Templates";
            Master.HeaderText = Master.Title;
            Master.MyToolbar.LoadToolbarItem("", "javascript:Return();", "GoRtlHS.png", "Return", "");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:Search();", "search16.png", "Search", "");
        }

        private void Search()
        {

            bool currentUserMissing = false;            
            string currentUserID = "";            
            string errorMsg = "";

            validateUser(txtName.Text, out currentUserID, out currentUserMissing, out errorMsg);
            if (errorMsg.Length > 0)
            {
                SetErrorMsg(errorMsg, hdnErrorMsg);
                return;
            }
            if (currentUserMissing)
                currentUserID = txtName.Text.Trim();
            Response.Redirect("FindUserResults.aspx?BP=0&userID=" + currentUserID);
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