using System;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace QA
{
    public partial class Equipment : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddl_Locations.Attributes.Add("onchange", "fnLocationChanged()");
            ddl_Department.Attributes.Add("onchange", "fnEquipmentChanged()");

            if (Request["__EVENTTARGET"] == "locationChanged")
            {
                Connect();
            }

            if (Request["__EVENTTARGET"] == "equipmentChanged")
            {
                Connect();
            } 
                 
            if (!IsPostBack)
            {
                init();
                Connect();
            }         

            if (Request["__EVENTTARGET"] == "Sync")
            {
                int count = tools.UpdateJDEEquipment(ddl_Locations.SelectedValue);
                hdnErrorMsg.Value = "Updated " + count + " records.";
            }

            addPopUpToolbar_Equipment();

            ConfigurePage();            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] == "newPA")
            {
                newPA();
            }            
        }       
      
        private void ConfigurePage()
        {
            Master.HeaderText = _appFriendlyName + " - " + "Administration - Equipment";
            Master.MyToolbar.LoadToolbarItem("javascript:Return();", "GoRtlHS.png", "Return");
            Master.MyToolbar.LoadToolbarItem(new toolbarDelegate(ddlLocations));
            Master.MyToolbar.LoadToolbarItem(new toolbarDelegate(ddlDepartments));
            Master.MyToolbar.LoadToolbarItem("",  "javascript:Sync();", "User Add16.png", "Download from JDE", "");
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnShowPA();", "User Add16.png", "New Equipment", "");
        }

        public string ddlLocations()
        {
            DataTable dt = tools.getLocations();
            string tmp = "<li class=\"dropdown hover\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-globe fa-lg\"></i>" + " Locations " + "<span class=\"caret\"></span></a>";
            tmp += "<ul class=\"dropdown-menu hover\">";
     
            foreach (DataRow dr in dt.Rows)
            {
                tmp += "<li><a onclick=\"fnLocationChanged()\">" + dr["Location"] + "</a></li>";
            }

            tmp += "</ul></li>";
            return tmp;
        }

        public string ddlDepartments()
        {
            DataTable dt = tools.getDepartments(_BP);
            string tmp = "<li class=\"dropdown hover\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-globe fa-lg\"></i>" + " Departments " + "<span class=\"caret\"></span></a>";
            tmp += "<ul class=\"dropdown-menu hover\">";
            //Still need links


            foreach (DataRow dr in dt.Rows)
            {
                tmp += "<li><a onclick=fnEquipmentChanged()"+ dr["Name"] + "\">" + dr["friendlyName"] + "</a></li>";
            }

            tmp += "</ul></li>";
            return tmp;
        }

        private void init()
        {            
            DataTable dt = tools.getLocations();
            ddl_Locations.DataSource = dt;
            ddl_Locations.DataTextField = "Location";
            ddl_Locations.DataValueField = "BP";
            ddl_Locations.DataBind();

            dt = tools.getDepartments(_BP);
            ddl_Department.DataSource = dt;
            ddl_Department.DataTextField = "friendlyName";
            ddl_Department.DataValueField = "Name";
            ddl_Department.DataBind();
        }
        
        private void Connect()
        {
            //GV.Visible = false;
            DataTable dt = tools.GetEquipment(ddl_Locations.SelectedValue, ddl_Department.SelectedValue);
            //if (dt.Rows.Count > 0)
            //{
                GV.DataSource = dt;
                GV.DataBind();
                //GV.Visible = true;
            //}            
        }

        #region //****************************************** New Equipment ************************************/
        private void addPopUpToolbar_Equipment()
        {
            PopUpToolbar.PopUpToolbar pt = new PopUpToolbar.PopUpToolbar(_webServer, _appPath, "Add new equipment", "");
            pt.LoadToolbarItem("", "javascript:fnHidePA(); return false;", "GoRtlHS.png", "Return", "");
            pt.LoadToolbarItem("", "", "vert_div.gif", "", "");
            pt.LoadToolbarItem("", "javascript:fnSavePA()", "save16.png", "Save", "");
            HtmlGenericControl gc = new HtmlGenericControl();
            gc.InnerHtml = pt.DisplayDynamicToolbar();
            panelPA.Controls.AddAt(0, gc);

            DataTable dt = tools.getDepartments(_BP);
            ddl_AddDepartment.DataSource = dt;
            ddl_AddDepartment.DataTextField = "friendlyName";
            ddl_AddDepartment.DataValueField = "Name";
            ddl_AddDepartment.DataBind();

            lblAddLocation.Text = ddl_Locations.SelectedItem.Text;
        }

        private void newPA()
        {
            string retval = tools.addEquipment(_BP, txtAddUnit.Text,Request.Form[ddl_AddDepartment.UniqueID], txtAddMfg.Text, txtAddDescription.Text, txtAddModel.Text);
            txtAddDescription.Text = "";
            txtAddMfg.Text = "";
            txtAddModel.Text = "";
            txtAddUnit.Text = "";
            if (retval.Length > 0)
                hdnErrorMsg.Value = retval;
        }
        #endregion

        #region //************************** EVENTS *****************************/
        protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddl_Dept = (DropDownList)e.Row.FindControl("ddl_Department");
                DataTable dt = tools.getDepartments(_BP);
                ddl_Dept.DataSource = dt;
                ddl_Dept.DataTextField = "friendlyName";
                ddl_Dept.DataValueField = "Name";
                ddl_Dept.DataBind();
                ddl_Dept.SelectedValue = ddl_Department.SelectedValue;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Controls.Count > 0)
                    ((LinkButton)e.Row.Cells[1].Controls[0]).OnClientClick = "return DeleteEquipment();";
            }
        }

        protected void GV_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GV.EditIndex = e.NewEditIndex;
            Connect();            
        }

        protected void GV_RowCancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GV.EditIndex = -1;
            Connect();
        }

        protected void GV_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string ID = e.Keys["ID"].ToString();            

            DataTable dt = tools.GetEquipment((int)e.Keys["ID"]);
            if (dt.Rows.Count > 0)
            {
                TextBox txtUnit = (TextBox)GV.Rows[e.RowIndex].FindControl("txtUnit");
                TextBox txtModel = (TextBox)GV.Rows[e.RowIndex].FindControl("txtModel");
                TextBox txtMfg = (TextBox)GV.Rows[e.RowIndex].FindControl("txtMfg");
                TextBox txtDescription = (TextBox)GV.Rows[e.RowIndex].FindControl("txtDescription");
                DropDownList ddl_Dept = (DropDownList)GV.Rows[e.RowIndex].FindControl("ddl_Department");

                dt.Rows[0]["Unit"] = txtUnit.Text;
                dt.Rows[0]["Model"] = txtModel.Text;
                dt.Rows[0]["Mfg"] = txtMfg.Text;
                dt.Rows[0]["Description"] = txtDescription.Text;
                dt.Rows[0]["Department"] = ddl_Dept.SelectedValue;

                tools.updateEquipment(dt);
            }
            GV.EditIndex = -1;
            
            Connect();
        }

        protected void GV_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            tools.deleteEquipment((int)e.Keys["ID"]);
            Connect();
        }
        #endregion

        #region //******************************** WEB METHODS *******************************/
     
        #endregion
    }
}