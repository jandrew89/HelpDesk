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
    public partial class Departments : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddl_Locations.Attributes.Add("onchange", "fnLocationChanged()");
            
            if (Request["__EVENTTARGET"] == "locationChanged")
            {
                locationChanged();
            }  
          
            if (Request["__EVENTTARGET"] == "newDepartment")
            {
                newDepartment();
                txtDepartment.Text = "";
                txtDescription.Text = "";
            }

            if (!IsPostBack)
            {
                buildLocations();
                Connect();                             
            }

            addPopUpToolbar_Department();

            ConfigurePage();            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }       
      
        private void ConfigurePage()
        {
            
            Master.HeaderText = _appFriendlyName + " - " + "Administration - Departments";
            Master.MyToolbar.LoadToolbarItem("javascript:Return();", "GoRtlHS.png", "Return");
            Master.MyToolbar.LoadToolbarItem(new toolbarDelegate(ddlLocations));
            Master.MyToolbar.LoadToolbarItem("",  "javascript:fnAddDepartment('New Department');", "User Add16.png", "New Department", "");
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
                tmp += "<li><a href=\"/admin/Departments.aspx?BP=" + dr["BP"] + "\">" + dr["Location"] + "</a></li>";
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

        public void locationChanged()
        {
            Connect();
        }

        private void Connect()
        {
            GV.Visible = false;            
            DataTable dt = tools.getDepartments(Request["BP"]);
            if (dt.Rows.Count > 0)
            {
                GV.DataSource = dt;
                GV.DataBind();
                GV.Visible = true;
            }
        }

        private void newDepartment()
        {
            //string[] tmp = Request["__EVENTARGUMENT"].Split(';');
            if(txtDepartment.Text.Length < 2)
            {
                hdnErrorMsg.Value = "Please enter a department!";                
                return;
            }

            DataTable dt = tools.getDepartment(ddl_Locations.SelectedValue, txtDepartment.Text);
            if (dt.Rows.Count > 0)
            {
                hdnErrorMsg.Value = "This name already exists!";
                return;
            }
            DataRow newRow = dt.NewRow();
            newRow["BP"] = ddl_Locations.SelectedValue;
            newRow["Name"] = txtDepartment.Text;
            newRow["Description"] = txtDescription.Text;            
            dt.Rows.Add(newRow);
            tools.updateDepartment(dt);
            Connect();
        }

        private void addPopUpToolbar_Department()
        {
            PopUpToolbar.PopUpToolbar pt = new PopUpToolbar.PopUpToolbar(_webServer, _appPath, "", "lblPopUpToolbarTitle");
            pt.LoadToolbarItem("", "javascript:fnHideToolBarPopUp_Department(); return false;", "GoRtlHS.png", "Return", "");
            pt.LoadToolbarItem("", "", "vert_div.gif", "", "");
            pt.LoadToolbarItem("", "javascript:fnNewDepartment()", "save16.png", "Save", "");
            HtmlGenericControl gc = new HtmlGenericControl();
            gc.InnerHtml = pt.DisplayDynamicToolbar();
            popUp_Department.Controls.AddAt(0, gc);
        }

        #region //************************** EVENTS *****************************/
        protected void GV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                TextBox txtSuper = (TextBox)e.Row.FindControl("txtSupervisor");
                AjaxControlToolkit.AutoCompleteExtender ac1 = (AjaxControlToolkit.AutoCompleteExtender)e.Row.FindControl("AutoCompleteExtender1");
                ac1.TargetControlID = txtSuper.ID;              
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Controls.Count > 0)
                    ((LinkButton)e.Row.Cells[1].Controls[0]).OnClientClick = "return DeleteDepartment();"; // confirm('Are you sure you want to delete?');";
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
            TextBox txtName = (TextBox)GV.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtDescription = (TextBox)GV.Rows[e.RowIndex].FindControl("txtDescription");
            
            string ID = e.Keys["ID"].ToString();
            DataTable dtDepartment = tools.getDepartment(int.Parse(ID));
            
            DataTable dt = tools.getDepartment(e.Keys["BP"].ToString(), txtName.Text);
            if(dt.Rows.Count > 0)
            {                
                if (dt.Rows[0]["ID"].ToString() != ID)
                {
                    hdnErrorMsg.Value = "This name already exists!";
                    return;
                }
            }
            
            dtDepartment.Rows[0]["Name"] = txtName.Text;
            dtDepartment.Rows[0]["Description"] = txtDescription.Text;
            dtDepartment.Rows[0]["Supervisor"] = hdnSupervisor.Value;
            //Response.Write("S:" + hdnSupervisor.Value);
            tools.updateDepartment(dtDepartment);
            GV.EditIndex = -1;
            hdnSupervisor.Value = "";
            Connect();
        }

        protected void GV_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            tools.deleteDepartment((int)e.Keys["ID"]);
            Connect();
        }
        #endregion

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