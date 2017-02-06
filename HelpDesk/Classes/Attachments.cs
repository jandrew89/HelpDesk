using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

using CWF_Security;

namespace FE_Attachments
{
    public class Attachments
    {
        private SqlConnection sqlConn;
        private SecurityCredentials secCred;
        private string _dbName;

        public Attachments(string dbName)
        {
            secCred = new SecurityCredentials();
            Init(dbName);
        }

        public Attachments(string dbName, SecurityCredentials SecCred)
        {
            secCred = SecCred;
            Init(dbName);
        }

        private void Init(string dbName)
        {
            _dbName = dbName;
            sqlConn = new SqlConnection();
            sqlConn.ConnectionString = secCred.ConnectionStringSQL(_dbName);
        }

        #region //******************************* ATTACHMENTS ********************************/
        public DataTable getAttachments(string tableName, string fieldName, string fieldValue)
        {
            try
            {
                OpenConn();
                string SQL = "Select * from Attachments where " + fieldName + "='" + fieldValue + "'";
                DataTable dt = new DataTable();
                SqlDataAdapter adap = new SqlDataAdapter(SQL, sqlConn);
                adap.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConn();
            }
        }

        public bool hasDocID(string tableName)
        {
            try
            {
                OpenConn();
                string SQL = "Select * from Attachments where ID=-1";
                DataTable dt = new DataTable();
                SqlDataAdapter adap = new SqlDataAdapter(SQL, sqlConn);
                adap.Fill(dt);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToLower() == "docid")
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConn();
            }
        }

        public void saveAttachment(string tableName, string fieldName, string fieldValue, string filename, byte[] byteArray, string userID, string userName, string docID, string comments)
        {
            DataTable DT = new DataTable();
            try
            {
                OpenConn();
                filename = filename.Replace('#', '-');
                SqlDataAdapter SQL_DA = new SqlDataAdapter();
                string sql = "SELECT * FROM Attachments WHERE " + fieldName + " = '" + fieldValue + "' AND filename = '" + filename + "'";
                SQL_DA.SelectCommand = new SqlCommand(sql, sqlConn);
                SqlCommandBuilder custCB = new SqlCommandBuilder(SQL_DA);
                SQL_DA.Fill(DT);
                DataRow newRow;                
                if (DT.Rows.Count == 0)
                    newRow = DT.NewRow();
                else
                    newRow = DT.Rows[0];
                newRow[fieldName] = fieldValue;
                newRow["filename"] = filename;
                newRow["userID"] = userID;
                newRow["comments"] = comments;
                newRow["userName"] = userName;
                newRow["docImage"] = byteArray;
                newRow["changeDate"] = DateTime.Now;
                if (docID.Length > 0)
                    newRow["docID"] = docID;
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

        public void deleteAttachment(string tableName, string fieldName, string fieldValue, string fileName)
        {
            try
            {
                OpenConn();
                string SQL = "Delete from Attachments where " + fieldName + "='" + fieldValue + "' AND Filename='" + fileName + "'";
                SqlCommand sqlComm = new SqlCommand(SQL, sqlConn);
                sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConn();
            }
        }
        #endregion

        protected void OpenConn()
        {
            sqlConn.Open();
        }

        protected void CloseConn()
        {
            sqlConn.Close();
        }
    }
}