using System;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using CWF_Security;

public partial class CreateFile : System.Web.UI.Page
{
    private SecurityCredentials SecCred; 
    public string DatabaseName;
    public string attachID;
    public string ID_FieldName;
    public string ID_FieldValue;
    public string tableName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SecCred = new SecurityCredentials();
            if (!IsPostBack)
            {
                //ID_FieldName = Request["ID_FieldName"];
                //ID_FieldValue = Request["ID_FieldValue"];
                DatabaseName = Request["DatabaseName"];
                tableName = Request["tableName"];
                attachID = Request["ID"];                
            }

            //Connect();
            CreateAttachment2();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            Response.End();
            return;
        }
    }
   
    private void CreateAttachment2()
    {
        SqlConnection sqlConn = new SqlConnection();

        try
        {            
            sqlConn.ConnectionString = "server=" + SecCred.Server + ";database=" + DatabaseName + ";UID=" + SecCred.UserIDWrite + ";PWD=" + SecCred.PasswordWrite + ";";
            sqlConn.Open();

            string SQL = "SELECT * FROM " + tableName + " WHERE ID = " + attachID;
            SqlCommand sqlComm = new SqlCommand(SQL, sqlConn);
            SqlDataReader sdr = sqlComm.ExecuteReader();
            if (sdr.HasRows == false)
            {
                Response.Write("No Attachment Found!");
                Response.End();
            }
            sdr.Read();
            byte[] bits = (byte[])sdr["Image"];
            string filename = sdr["filename"].ToString();
            sdr.Close();

            string contentType = MimeHelper.GetMimeType(filename);

            //Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Type", contentType);
            Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
            Response.OutputStream.Write(bits, 0, bits.Length);
            Response.Flush();            
            Response.End();          
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            Response.End();
            return;
        }
        finally
        {
            sqlConn.Close();
        }
    }

    public static class MimeHelper
    {
        private static readonly object Locker = new object();
        private static object mimeMapping = null;
        private static readonly MethodInfo GetMimeMappingMethodInfo;
        static MimeHelper()
        {
            Type mimeMappingType = Assembly.GetAssembly(typeof(HttpRuntime)).GetType("System.Web.MimeMapping");
            if (mimeMappingType == null)
                throw new SystemException("Couldnt find MimeMapping type");
            GetMimeMappingMethodInfo = mimeMappingType.GetMethod("GetMimeMapping",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (GetMimeMappingMethodInfo == null)
                throw new SystemException("Couldnt find GetMimeMapping method");
            if (GetMimeMappingMethodInfo.ReturnType != typeof(string))
                throw new SystemException("GetMimeMapping method has invalid return type");
            if (GetMimeMappingMethodInfo.GetParameters().Length != 1
                && GetMimeMappingMethodInfo.GetParameters()[0].ParameterType != typeof(string))
                throw new SystemException("GetMimeMapping method has invalid parameters");
        }

        public static string GetMimeType(string filename)
        {
            lock (Locker)
                return (string)GetMimeMappingMethodInfo.Invoke(mimeMapping,
                    new object[] { filename });
        }

    }
}
