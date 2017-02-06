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
    public partial class users : BaseClass
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
            Master.MyToolbar.LoadToolbarItem("javascript:NewUser();", "User Add16.png", "New User");
        }

        public string ddlLocations()
        {
            DataTable dt = tools.getLocations();
            string tmp = "<li class=\"dropdown hover\"><a class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-globe fa-lg\"></i>" + " Locations "+"<span class=\"caret\"></span></a>";
                   tmp += "<ul class=\"dropdown-menu hover\">";
                    //Still need links
                   tmp += "<li><a href=\"#\">" + "All Users" + "</a></li>";
                   tmp += "<li><a href=\"#\">" + "Global Admin" + "</a></li>";

            foreach (DataRow dr in dt.Rows)
            {
                tmp += "<li><a href=\"/admin/users.aspx?BP="+ dr["BP"] +"\">" + dr["Location"] + "</a></li>";
            }

            tmp += "</ul></li>";
            return tmp;
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

                newRow = dt.NewRow();
                newRow["Location"] = "All Users";
                newRow["BP"] = "";
                dt.Rows.InsertAt(newRow, 0);
            }

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
            
            bool dataChanged = false;
            DataTable dt = tools.getUsers(ddl_Locations.SelectedValue, false);
            if (dt.Rows.Count > 0)
            {            
                foreach (DataRow dr in dt.Rows)
                {             
                    if (dr["friendlyName"].ToString() == dr["userID"].ToString())
                    {
                        //must no longer be in AD - let's just go ahead and make inactive
                        DataTable dtUser = tools.getUserByName(dr["userID"].ToString());
                        dtUser.Rows[0]["isActive"] = 0;
                        dtUser.Rows[0]["friendlyname"] = "";
                        tools.updateUser(dtUser);
                        dataChanged = true;
                    }
                }
                if(dataChanged)
                    dt = tools.getUsers(ddl_Locations.SelectedValue, false);
       
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
                    ((LinkButton)e.Row.Cells[1].Controls[0]).OnClientClick = "return DeleteUser();"; // confirm('Are you sure you want to delete?');
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
            DataTable dt = tools.getUser(GV.DataKeys[row].Values["ID"].ToString());

            CheckBox cbActive = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkIsActive");            
            CheckBox cbAdmin = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkIsAdmin");
            //CheckBox cb1 = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkisMaintElectrical");
            //CheckBox cb2 = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkisMaintMechanical");
            //CheckBox cb3 = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkisMaintOther");
            
            dt.Rows[0]["isActive"] = cbActive.Checked;            
            dt.Rows[0]["IsAdmin"] = cbAdmin.Checked;
            //dt.Rows[0]["isMaintElectrical"] = cb1.Checked;
            //dt.Rows[0]["isMaintMechanical"] = cb2.Checked;
            //dt.Rows[0]["isMaintOther"] = cb3.Checked;            
                                                
            tools.updateUser(dt);
            GV.EditIndex = -1;
            Connect();
        }

        protected void GV_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            tools.DeleteUser(e.Keys[0].ToString());
            Connect();
        }
        #endregion
    }
}