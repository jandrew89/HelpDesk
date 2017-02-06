using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeLib
{
    /// <summary>
    /// Summary description for MyTree
    /// </summary>
    [ToolboxData("<{0}:WebCustomControl1 runat=server></{0]:WebcustomControl1>")]
    public class MyTree : TreeView
    {        
        protected override TreeNode CreateNode()
        {
            return new myTreeNode(); // base.CreateNode();
        }                
    }
    

    public class myTreeNode : TreeNode
    {
        private bool _showToolTip = false;
        private string _ID = "";
        //private DataRow _dr;
        private string _BP = "";
        private string _productType = "";
        private string _productLine = "";
        private string _productCategory = "";
        private string _description = "";
        private string _mfType = "";
        private bool _showMyCheckBox = false;
        private string _requestID = "";
        private string _customArgs = "";
        private string _PRC = "";
        private bool _usePRC = false;
        private bool _populateOnDemand = false;

        public bool populateOnDemand
        {
            get
            { return _populateOnDemand; }
            set
            { _populateOnDemand = value; }
        }

        public bool usePRC
        {
            get
            { return _usePRC; }
            set
            { _usePRC = value; }
        }

        public string PRC
        {
            get
            { return _PRC; }
            set
            { _PRC = value; }
        }

        public string requestID
        {
            get
            { return _requestID; }
            set
            { _requestID = value; }
        }

        public bool showMyCheckBox
        {
            get
            { return _showMyCheckBox; }
            set
            { _showMyCheckBox = value; }
        }

        public bool showToolTip
        {
            get
            { return _showToolTip; }
            set
            { _showToolTip = value; }
        }

        public string ID
        {
            get
            { return _ID; }
            set
            { _ID = value; }
        }

        public string description
        {
            get
            { return _description; }
            set
            { _description = value; }
        }

        public string productType
        {
            get
            { return _productType; }
            set
            { _productType = value; }
        }

        public string BP
        {
            get
            { return _BP; }
            set
            { _BP = value; }
        }

        public string productLine
        {
            get
            { return _productLine; }
            set
            { _productLine = value; }
        }

        public string productCategory
        {
            get
            { return _productCategory; }
            set
            { _productCategory = value; }
        }

        public string mfType
        {
            get
            { return _mfType; }
            set
            { _mfType = value; }
        }

        public string customArgs
        {
            get
            { return _customArgs; }
            set
            { _customArgs = value; }
        }

        protected override void RenderPostText(HtmlTextWriter writer)
        {


            if (_showMyCheckBox)
            {
                string boxChecked = "";
                if (this.Checked)
                    boxChecked = " checked ";
                writer.Write("<input type=\"checkbox\"" + boxChecked + " id=\"cb" + _ID + "\" onclick=\"fnTVCheck(this," + _requestID + ",'" + _productType + "','" + _productLine + "','" + _productCategory + "')\">&nbsp;");
            }

            string link = "";
            if (populateOnDemand)
            {
                link = " href=\"javascript:fnPopulateOnDemand('" + _customArgs + "');\"";
                writer.Write("<a ID=\"" + ID + "\" style=\"color:Black;text-decoration:none\"" + link + ">" + _description + "</a>");
            }
            else
            {                
                if (!_usePRC)
                    link = " href=\"javascript:fnFlatten(" + _ID + ",'" + _BP + "','" + _productType + "','" + _productLine + "','" + _productCategory +
                            "','" + _mfType + "','" + _customArgs + "','');\"";
                else
                    link = " href=\"javascript:fnFlatten(" + _ID + ",'" + _BP + "','" + _PRC + "','" + _mfType + "','" + _customArgs + "','');\"";

                if (_ID.Length > 0)
                    writer.Write("<a ID=\"" + ID + "\" style=\"color:Black;text-decoration:none\" title=\"Right-click for options\"" + link + ">" + _description + "</a>");
                else
                    writer.Write(_description);
            }

            base.RenderPostText(writer);
        }
    }
}