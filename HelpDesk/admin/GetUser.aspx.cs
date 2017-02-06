using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace QA
{
    public partial class GetUser : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {                 
            if (Request["__EVENTTARGET"] == "NewUser")
                AddUser();

            if (!IsPostBack)
            {
                buildLocations();
            }

            if (ddl_Locations.Items.Count == 0)
            {
                Response.Write("Admin rights are required!");
                Response.End();
            }

            ConfigurePage();
            txtName.Focus();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }

        private void ConfigurePage()
        {            
            Master.Title = _appFriendlyName + " Administration - Add User";
            Master.HeaderText = Master.Title;
            Master.MyToolbar.LoadToolbarItem("", "javascript:Return();", "GoRtlHS.png", "Return", "");
        }

        private void buildLocations()
        {
            bool addGlobal;
            DataTable dt = tools.getLocationsByUser(userID, out addGlobal);
            if (addGlobal)
            {
                DataRow newRow = dt.NewRow();
                newRow["Location"] = "Global Admins";
                newRow["BP"] = "9900";
                dt.Rows.InsertAt(newRow, 0);              
            }

            ddl_Locations.DataSource = dt;
            ddl_Locations.DataTextField = "Location";
            ddl_Locations.DataValueField = "BP";
            ddl_Locations.DataBind();

            if (ddl_Locations.Items.FindByValue(_BP) != null)
                ddl_Locations.SelectedValue = _BP;
        }

        private void AddUser()
        {            
            DataTable dt = tools.getUser(ddl_Locations.SelectedValue, Request["__EVENTARGUMENT"], false);
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["userID"] = Request["__EVENTARGUMENT"];
                dr["BP"] = ddl_Locations.SelectedValue;
                dr["isActive"] = true;
                //dr["Creator"] = false;
                dr["isAdmin"] = false;
                //dr["isMaintElectrical"] = false;
                //dr["isMaintMechanical"] = false;
                //dr["isMaintOther"] = false;
                dt.Rows.Add(dr);
                tools.updateUser(dt);
            }
            Response.Redirect("users.aspx?BP=" + _BP);
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