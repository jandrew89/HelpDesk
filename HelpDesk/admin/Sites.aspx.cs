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
    public partial class Sites : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!tools.isAdmin("9900"))
            {
                Response.Write("Global admin rights are required!");
                Response.End();
            }

            if (Request["__EVENTTARGET"] == "newSite")
            {
                newSite();
            }

            if (!IsPostBack)
            {
                Connect();                             
            }

            ConfigurePage();            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }       
      
        private void ConfigurePage()
        {
            Master.HeaderText = _appFriendlyName + " - " + "Administration - Sites";
            Master.MyToolbar.LoadToolbarItem("javascript:Return();", "GoRtlHS.png", "Return");
            Master.MyToolbar.LoadToolbarItem("", "", "vert_div.gif", "", "");
            Master.MyToolbar.LoadToolbarItem("javascript:NewLocation();", "User Add16.png", "New Location");
        }

        private void Connect()
        {
            GV.Visible = false;            
            DataTable dt = tools.getLocations();
            if (dt.Rows.Count > 0)
            {
                GV.DataSource = dt;
                GV.DataBind();
                GV.Visible = true;
            }
        }

        private void newSite()
        {
            string[] tmp = Request["__EVENTARGUMENT"].Split(';');

            DataTable dt = tools.getLocation(tmp[0]);
            if (dt.Rows.Count > 0)
                return;

            dt = tools.getLocationByName(tmp[1]);
            if (dt.Rows.Count > 0)
                return;

            DataRow newRow = dt.NewRow();
            newRow["BP"] = tmp[0];
            newRow["Location"] = tmp[1];
            newRow["Active"] = 1;
            newRow["CreateNew"] = 1;
            dt.Rows.Add(newRow);
            tools.updateLocations(dt);
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
                    ((LinkButton)e.Row.Cells[1].Controls[0]).OnClientClick = "return DeleteSite();"; // confirm('Are you sure you want to delete?');";
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
            DataTable dt = tools.getLocation(e.Keys["BP"].ToString());

            CheckBox cbActive = (CheckBox)GV.Rows[e.RowIndex].FindControl("chkIsActive");            
            CheckBox cbCreateNew = (CheckBox)GV.Rows[e.RowIndex].FindControl("cbCreateNew");            
            TextBox txtLocation = (TextBox)GV.Rows[e.RowIndex].FindControl("txtLocation");

            dt.Rows[0]["Active"] = cbActive.Checked;            
            dt.Rows[0]["CreateNew"] = cbCreateNew.Checked;            
            dt.Rows[0]["Location"] = txtLocation.Text;

            tools.updateLocations(dt);
            GV.EditIndex = -1;
            Connect();
        }

        protected void GV_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = tools.GetHeaderField(e.Keys["BP"].ToString(), "WFID");
            if (dt.Rows.Count == 0)
            {                
                tools.deleteLocation(e.Keys[0].ToString());
            }
            else
            {
                SetErrorMsg("Records exist for location - " + e.Keys["BP"].ToString(), hdnErrorMsg);
            }
            Connect();
        }
        #endregion
    }
}