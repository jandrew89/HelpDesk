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
    public partial class Maintenance : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddl_Locations.Attributes.Add("onchange", "fnLocationChanged()");

            if (Request["__EVENTTARGET"] == "locationChanged")
            {
                locationChanged();
            }  

            if (!IsPostBack)
            {
                buildLocations();
                Connect();                             
            }

            if (ddl_Locations.Items.Count == 0)
            {
                Response.Write("Admin rights are required!");
                Response.End();
            }

            if (Request["__EVENTTARGET"] == "newUser")
            {
                newUser();
                txtUserID.Text = "";
                txtName.Text = "";
                cbActive.Checked = true;
                cbElectrical.Checked = false;
                cbMechanical.Checked = false;
                cbOther.Checked = false;
            }

            addPopUpToolbar();

            ConfigurePage();            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }       
      
        private void ConfigurePage()
        {
            Master.Title = _appFriendlyName + " User Administration";
            Master.HeaderText = Master.Title + " - " + tools.getPlantName(_BP);
            Master.MyToolbar.LoadToolbarItem("javascript:Return();", "GoRtlHS.png", "Return");
            Master.MyToolbar.LoadToolbarItem(new toolbarDelegate(ddlLocations));
            Master.MyToolbar.LoadToolbarItem("", "javascript:fnShowPopUp('New Maintenace User');", "User Add16.png", "New User", "");
        }

        public string ddlLocations()
        {
            DataTable dt = tools.getLocations();
            string tmp = "<li class=\"dropdown hover\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-globe fa-lg\"></i>" + " Locations " + "<span class=\"caret\"></span></a>";
            tmp += "<ul class=\"dropdown-menu hover\">";

            //tmp += "<li><a href=\"#\">" + "All Users" + "</a></li>";
            //tmp += "<li><a href=\"#\">" + "Global Admin" + "</a></li>";

            foreach (DataRow dr in dt.Rows)
            {
                tmp += "<li><a href" + dr["BP"] + "\">" + dr["Location"] + "</a></li>";
            }

            tmp += "</ul></li>";
            return tmp;
        }

        private void buildLocations()
        {            
            DataTable dt = tools.getLocations();            
            ddl_Locations.DataSource = dt;
            ddl_Locations.DataTextField = "Location";
            ddl_Locations.DataValueField = "BP";
            ddl_Locations.DataBind();
        }

        private void Connect()
        {
            GV.Visible = false;
            if (ddl_Locations.Items.Count == 0)
                return;

            lblPopUpLocation.Text = ddl_Locations.SelectedItem.Text;

            DataTable dt = tools.GetMaintenanceUsers(ddl_Locations.SelectedValue, "");
            if (dt.Rows.Count > 0)
            {                                                   
                dt.Columns.Add("Location");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Location"] = dr["BP"] + " - " + tools.getPlantName(dr["BP"].ToString());                  
                }

                GV.DataSource = dt;
                GV.DataBind();
                GV.Visible = true;
            }
        }

        private void addPopUpToolbar()
        {
            PopUpToolbar.PopUpToolbar pt = new PopUpToolbar.PopUpToolbar(_webServer, _appPath, "", "lblPopUpToolbarTitle");
            pt.LoadToolbarItem("", "javascript:fnHideToolBarPopUp(); return false;", "GoRtlHS.png", "Return", "");
            pt.LoadToolbarItem("", "", "vert_div.gif", "", "");
            pt.LoadToolbarItem("", "javascript:fnNewValue()", "save16.png", "Save", "");
            HtmlGenericControl gc = new HtmlGenericControl();
            gc.InnerHtml = pt.DisplayDynamicToolbar();
            popUp_Department.Controls.AddAt(0, gc);
        }

        private void newUser()
        {
            //string[] tmp = Request["__EVENTARGUMENT"].Split(';');
            if (txtUserID.Text.Length < 2)
            {
                hdnErrorMsg.Value = "Please enter a user ID!";
                return;
            }

            DataTable dt = tools.GetMaintenanceUser(ddl_Locations.SelectedValue, txtUserID.Text);
            if (dt.Rows.Count > 0)
            {
                hdnErrorMsg.Value = "This user already exists!";
                return;
            }
            DataRow newRow = dt.NewRow();
            newRow["BP"] = ddl_Locations.SelectedValue;
            newRow["UserID"] = txtUserID.Text;
            newRow["friendlyName"] = txtName.Text;
            newRow["isMaintElectrical"] = cbElectrical.Checked;
            newRow["isMaintMechanical"] = cbMechanical.Checked;
            newRow["isMaintOther"] = cbOther.Checked;
            dt.Rows.Add(newRow);
            tools.updateMaintenance(dt);
            Connect();
        }

        public void locationChanged()
        {
            Connect();
        }
      
        #region //************************** EVENTS *****************************/
        protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
              
              
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Controls.Count > 0)
                    ((LinkButton)e.Row.Cells[1].Controls[0]).OnClientClick = "return DeleteUser();"; // confirm('Are you sure you want to delete?');";
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
            int row = e.RowIndex;
            DataTable dt = tools.GetMaintenanceUser((int)GV.DataKeys[row].Values["ID"]);

            CheckBox cbActive = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkIsActive");                        
            CheckBox cb1 = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkisMaintElectrical");
            CheckBox cb2 = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkisMaintMechanical");
            CheckBox cb3 = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkisMaintOther");
            TextBox txtFN = (TextBox)GV.Rows[e.RowIndex].FindControl("txtFN");
            TextBox txtUserID = (TextBox)GV.Rows[e.RowIndex].FindControl("txtUserID");
            
            if(txtUserID.Text.Length ==0)
            {
                hdnErrorMsg.Value = "Please enter a user ID";
                return;
            }

            dt.Rows[0]["isActive"] = cbActive.Checked;
            dt.Rows[0]["friendlyName"] = txtFN.Text;
            dt.Rows[0]["userID"] = txtUserID.Text;
            dt.Rows[0]["isMaintElectrical"] = cb1.Checked;
            dt.Rows[0]["isMaintMechanical"] = cb2.Checked;
            dt.Rows[0]["isMaintOther"] = cb3.Checked;            
                                                
            tools.updateMaintenance(dt);
            GV.EditIndex = -1;
            Connect();
        }

        protected void GV_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            tools.deleteMaintenance((int)e.Keys["ID"]);
            Connect();
        }
        #endregion
    }
}