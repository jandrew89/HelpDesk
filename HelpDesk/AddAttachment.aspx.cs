using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


namespace QA
{
    public partial class Attachment : BaseClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Master.MyStyleSheet = "osx2_1.css";
            Master.HeaderText = "Workflow - " + _appFriendlyName;
            Master.MyToolbar.LoadToolbarItem("javascript:Cancel();", "GoRtlHS.png", "Return");                        
        }       
        
        public string getQueryString()
        {
            //string retval = "workflowID=" + workflowID;
            //if (Request["edit"] != null)
            //    retval += "&Edit=1";
            return Request.QueryString.ToString(); // retval;
        }

        protected void AsyncFileUpload1_UploadedComplete(object sender,  AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            if (AsyncFileUpload1.HasFile)
            {                
                saveAttachment(myHeaderDT.Rows[0]["ID"].ToString(), Path.GetFileName(e.FileName), AsyncFileUpload1.FileBytes, userFriendlyName);                
            }
        }

        public void saveAttachment(string applicationID, string filename, byte[] byteArray, string userID)
        {            
            try
            {
                OpenConn();
                filename = filename.Replace('#', '-');
                SqlDataAdapter SQL_DA = new SqlDataAdapter();
                string sql = "SELECT * FROM Attachments WHERE applicationID='" + applicationID + "' AND filename='" + filename + "'";
                SQL_DA.SelectCommand = new SqlCommand(sql, sqlConn);
                SqlCommandBuilder custCB = new SqlCommandBuilder(SQL_DA);
                DataTable DT = new DataTable();
                SQL_DA.Fill(DT);
                DataRow newRow;
                if (DT.Rows.Count == 0)
                    newRow = DT.NewRow();
                else
                    newRow = DT.Rows[0];
                newRow["applicationID"] = applicationID;
                newRow["filename"] = filename;
                newRow["enteredBy"] = userFriendlyName;
                //newRow["userName"] = userName;
                newRow["Image"] = byteArray;
                newRow["dateEntered"] = DateTime.Now;
                if (DT.Rows.Count == 0)
                    DT.Rows.Add(newRow);
                SQL_DA.Update(DT);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
            finally
            {
                CloseConn();
            }
        }
    }
}