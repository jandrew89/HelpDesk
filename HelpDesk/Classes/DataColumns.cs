using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nsDataColumns
{
    public class DataColumns
    {
        public DataTable SetWSDataColumns(DataTable dtSource, string dbExtension)
        {
            Dictionary<string, string> excludeColumns = new Dictionary<string,string>();
            excludeColumns.Add("Original_HP", "");
            excludeColumns.Add("Original_RPM", "");
            excludeColumns.Add("Original_Volts", "");
            excludeColumns.Add("BP1", "");
            excludeColumns.Add("PN1", "");
            excludeColumns.Add("REV1", "");

            string tableName1 = "WindSpecs_0" + dbExtension;
            string tableName2 = "WindSpecs_40" + dbExtension;

            DataTable dt = initDTColumns(dtSource, excludeColumns);
            foreach (DataRow dr in dt.Rows)
            {
                dr["TableName"] = tableName1;

                switch (dr["Name"].ToString().ToUpper())
                {
                    case "BP":
                    case "PN":
                    case "REV":
                        dr["Required"] = 1;
                        dr["Selected"] = 1;
                        break;

                    case "ENA":
                        dr["friendlyName"] = "End Turn A";
                        break;

                    case "ENB":
                        dr["friendlyName"] = "End Turn B";
                        break;

                    case "HVO":
                        dr["friendlyName"] = "Volts";
                        break;

                    case "MGP":
                        dr["friendlyName"] = "Wire";
                        dr["TableName"] = tableName2;
                        break;

                    case "NOM":
                        dr["friendlyName"] = "Resistance Nominal";
                        dr["TableName"] = tableName2;
                        break;

                    case "PHA":
                        dr["friendlyName"] = "Phase";
                        break;

                    case "QTM":
                        dr["friendlyName"] = "Qty";
                        dr["TableName"] = tableName2;
                        break;

                    case "RS1":
                        dr["friendlyName"] = "Resistance Min";
                        dr["TableName"] = tableName2;
                        break;

                    case "RS2":
                        dr["friendlyName"] = "Resistance Max";
                        dr["TableName"] = "WindSpecs_40";
                        break;

                    case "SIZ":
                        dr["friendlyName"] = "Wire Size";
                        dr["TableName"] = tableName2;
                        break;

                    case "SLF":
                        dr["friendlyName"] = "Slot Fill";
                        break;

                    case "SLN":
                        dr["friendlyName"] = "Stack Length";
                        break;

                    case "TURNS1":
                    case "TURNS2":
                    case "TURNS3":
                    case "TURNS4":
                    case "TURNS5":
                    case "TURNS1B":
                    case "TURNS2B":
                    case "TURNS3B":
                    case "TURNS4B":
                    case "TURNS5B":
                    case "TURNS1C":
                    case "TURNS2C":
                    case "TURNS3C":
                    case "TURNS4C":
                    case "TURNS5C":
                        dr["TableName"] = tableName2;
                        break;

                    case "TWN":
                        dr["friendlyName"] = "Winding";
                        dr["TableName"] = tableName2;
                        break;

                    case "SPN":
                        dr["friendlyName"] = "Span";
                        dr["TableName"] = tableName2;
                        break;

                    case "WGT":
                        dr["friendlyName"] = "Weight";
                        dr["TableName"] = tableName2;
                        break;
                }
            }
            return dt;
        }

        public DataTable SetTSDataColumns(DataTable dtSource, string dbExtension)
        {
            Dictionary<string, string> excludeColumns = new Dictionary<string, string>();            
            excludeColumns.Add("BP1", "");
            excludeColumns.Add("PN1", "");
            excludeColumns.Add("REV1", "");

            string tableName1 = "TestSpecs_0" + dbExtension;
            string tableName2 = "TestSpecs_20" + dbExtension;

            DataTable dt = initDTColumns(dtSource, excludeColumns);
            foreach (DataRow dr in dt.Rows)
            {
                dr["TableName"] = tableName1;

                switch (dr["Name"].ToString().ToUpper())
                {
                    case "BP":
                    case "PN":
                    case "REV":
                        dr["Required"] = 1;
                        dr["Selected"] = 1;
                        break;

                    case "ENA":
                        dr["friendlyName"] = "End Turn A";
                        break;

                    case "ENB":
                        dr["friendlyName"] = "End Turn B";
                        break;

                    case "HVO":
                        dr["friendlyName"] = "Volts";
                        break;

                    case "MGP":
                        dr["friendlyName"] = "Wire";                        
                        break;

                    case "NOM":
                        dr["friendlyName"] = "Resistance Nominal";                        
                        break;

                    case "PHA":
                        dr["friendlyName"] = "Phase";
                        break;

                    case "QTM":
                        dr["friendlyName"] = "Qty";                        
                        break;

                    case "RS1":
                        dr["friendlyName"] = "Resistance Min";                        
                        break;

                    case "RS2":
                        dr["friendlyName"] = "Resistance Max";                        
                        break;

                    case "SIZ":
                        dr["friendlyName"] = "Wire Size";                        
                        break;

                    case "SLF":
                        dr["friendlyName"] = "Slot Fill";
                        break;

                    case "SLN":
                        dr["friendlyName"] = "Stack Length";
                        break;

                    case "LD":
                    case "TP":
                    case "VLT":
                    case "AMP":
                    case "WAT":
                    case "RMX":
                    case "TLB":
                    case "HTZ":
                    case "REM":                    
                        dr["TableName"] = tableName2;
                        break;                  
                }
            }
            return dt;
        }

        private DataTable initDTColumns(DataTable dtSource, Dictionary<string, string> excludedValues)
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Name");
            dt1.Columns.Add("Type");
            dt1.Columns.Add("Required", typeof(bool));
            dt1.Columns.Add("Selected", typeof(bool));
            dt1.Columns.Add("FriendlyName");
            dt1.Columns.Add("TableName");            

            for (int i = 0; i < dtSource.Columns.Count; i++)
            {
                if(!excludedValues.ContainsKey(dtSource.Columns[i].ColumnName))                
                {
                    DataRow dr = dt1.NewRow();
                    dr["Name"] = dtSource.Columns[i].ColumnName;
                    dr["Type"] = dtSource.Columns[i].DataType;
                    dr["Required"] = 0;
                    dr["Selected"] = 0;
                    dr["FriendlyName"] = dr["Name"];
                    //dr["TableName"] = "WindSpecs_0";
                    dt1.Rows.Add(dr);
                }
            }
            return dt1;
        }
    }  
}