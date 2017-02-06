using QA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace LabRequest
{
    public partial class LoginDefault : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lbHelp.Visible = false;
            ConfigurePage();
            txtUserID.Focus();                        
        }

        private void ConfigurePage()
        {            
            //Master.Title = "User Login";
            //Master.HeaderText = Master.Title;            
        }

        #region //************** EVENTS *************************/
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtPWD.Value.Length > 0 && txtUserID.Value.Length > 0)
            {
                CWF_Corporate.Authentication AU = new CWF_Corporate.Authentication(SecCred, false);
                if(AU.ValidateUser(txtUserID.Value, txtPWD.Value))
                {
                        AU.setPortalCookie(txtUserID.Value, Response);
                        Response.Redirect("../default.aspx");                    
                }
                lblMsg.InnerText = "Sorry, but we were unable to verify your user name!";
                //lbHelp.Visible = true;
            }
            else
                lblMsg.InnerText = "Please enter user name and password!";            
            
            txtPWD.Value = "";
        }             
        #endregion
    }
}