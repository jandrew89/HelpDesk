using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using CWF_Corporate;

public class GridViewTemplate_LinkButton : ITemplate
{
    string ColumnText;
    string OverrideColumnText;
    string ClientClick;
    string FieldName;
    string HeaderName;

    public GridViewTemplate_LinkButton(string headerName, string fieldName)
    {
        this.FieldName = fieldName;
        this.HeaderName = headerName;
    }

    public GridViewTemplate_LinkButton(string headerName, string fieldName, string clientClick)
    {
        this.ClientClick = clientClick;
        this.FieldName = fieldName;
        this.HeaderName = headerName;
    }

    public GridViewTemplate_LinkButton(string headerName, string fieldName, string clientClick, string OverrideFieldColumnText)
    {
        this.ColumnText = OverrideFieldColumnText;
        this.ClientClick = clientClick;
        this.FieldName = fieldName;
        this.HeaderName = headerName;
    }

    public GridViewTemplate_LinkButton(string headerName, string fieldName, string clientClick, string OverrideFieldColumnText, string OverrideColumnText)
    {
        this.ColumnText = OverrideFieldColumnText;
        this.OverrideColumnText = OverrideColumnText;
        this.ClientClick = clientClick;
        this.FieldName = fieldName;
        this.HeaderName = headerName;
    }

    public void InstantiateIn(Control objContainer)
    {
        LinkButton lnkbtn = new LinkButton();
        lnkbtn.DataBinding += new EventHandler(lnkbtn_DataBinding);
        objContainer.Controls.Add(lnkbtn);
    }

    private void lnkbtn_DataBinding(object sender, EventArgs e)
    {
        string Activity = "Edit";

        LinkButton lnkbtn = (LinkButton)sender;
        lnkbtn.CausesValidation = false;
        GridViewRow row = (GridViewRow)lnkbtn.NamingContainer;

        if (!string.IsNullOrEmpty(ColumnText))
        {
            lnkbtn.Text = DataBinder.Eval(row.DataItem, ColumnText).ToString();
            lnkbtn.ToolTip = "Click to " + Activity + " " + HeaderName + " '" + DataBinder.Eval(row.DataItem, ColumnText).ToString().Trim() + "'";
        }
        else
        {
            lnkbtn.Text = DataBinder.Eval(row.DataItem, FieldName).ToString();
            lnkbtn.ToolTip = "Click to " + Activity + " " + HeaderName + " " + DataBinder.Eval(row.DataItem, FieldName).ToString().Trim();
        }

        if ((!string.IsNullOrEmpty(OverrideColumnText)) && (lnkbtn.Text != ""))
        {
            lnkbtn.Text = OverrideColumnText;
            lnkbtn.ToolTip = "Click to " + Activity + " " + OverrideColumnText;
        }

        lnkbtn.ID = DataBinder.Eval(row.DataItem, FieldName).ToString();
        lnkbtn.OnClientClick = ClientClick + "('" + DataBinder.Eval(row.DataItem, FieldName).ToString() + "')";
    }

}

public class GridViewTemplate_LabelUser : ITemplate
{
    string FieldName;
    string HeaderName;
    Authentication _AU;

    public GridViewTemplate_LabelUser(string headerName, string fieldName, Authentication AU)
    {
        this.FieldName = fieldName;
        this.HeaderName = headerName;
        _AU = AU;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.DataBinding += new EventHandler(lbl_DataBinding);
        objContainer.Controls.Add(lbl);
    }

    private void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow row = (GridViewRow)lbl.NamingContainer;

        lbl.ID = HeaderName + "_" + DataBinder.Eval(row.DataItem, FieldName).ToString();
        lbl.Text = UserDisplayName(DataBinder.Eval(row.DataItem, FieldName).ToString());
    }

    private string UserDisplayName(string UserID)
    {
        if (string.IsNullOrEmpty(UserID))
            return "";                
        
        Profile p = _AU.GetUserProfile(UserID);
        if (!string.IsNullOrEmpty(p.friendlyName))
            return p.friendlyName;

        return UserID;
    }

}

public class GridViewTemplate_LabelDate : ITemplate
{
    string FieldName;
    string HeaderName;
    CultureInfo culture;

    public GridViewTemplate_LabelDate(string headerName, string fieldName, CultureInfo cultureInfo)
    {
        this.FieldName = fieldName;
        this.HeaderName = headerName;
        this.culture = cultureInfo;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.DataBinding += new EventHandler(lbl_DataBinding);
        objContainer.Controls.Add(lbl);
    }

    private void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow row = (GridViewRow)lbl.NamingContainer;

        //lbl.ID = DataBinder.Eval(row.DataItem, FieldName).ToString();
        if (DataBinder.Eval(row.DataItem, FieldName) != DBNull.Value)
            lbl.Text = getFormattedDate((DateTime)DataBinder.Eval(row.DataItem, FieldName));
    }

    private string getFormattedDate(DateTime date)
    {
        if (date == DateTime.MinValue)
            return "";

        return date.ToString("d", culture);
    }

    private string getFormattedDateTime(DateTime date)
    {
        if (date == DateTime.MinValue)
            return "";

        return date.ToString(culture);
    }


}

public class GridViewTemplate_HTML : ITemplate
{
    string FieldName;

    public GridViewTemplate_HTML(string fieldName)
    {
        this.FieldName = fieldName;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.DataBinding += new EventHandler(lbl_DataBinding);
        objContainer.Controls.Add(lbl);
    }

    private void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow row = (GridViewRow)lbl.NamingContainer;

        lbl.ID = FieldName;
        lbl.Text = "<div>" + DataBinder.Eval(row.DataItem, FieldName).ToString() + "</ div>";
    }
}

public class GridViewTemplate_Hyperlink : ITemplate
{
    string _hypID;

    public GridViewTemplate_Hyperlink(string hypID)
    {
        this._hypID = hypID;
    }

    public void InstantiateIn(Control objContainer)
    {
        HyperLink hyp = new HyperLink();
        hyp.ID = _hypID;
        hyp.TabIndex = -1;
        hyp.ForeColor = System.Drawing.Color.Black;
        objContainer.Controls.Add(hyp);
    }
}

public class GridViewTemplate_Anchor : ITemplate
{
    string _hypID;
    string _description;
    string _toolTip = "";

    public GridViewTemplate_Anchor(string ID, string description)
    {
        this._hypID = ID;
        _description = description;
    }

    public GridViewTemplate_Anchor(string ID, string description, string toolTip)
    {
        this._hypID = ID;
        _description = description;
        _toolTip = toolTip;
    }

    public void InstantiateIn(Control objContainer)
    {
        HtmlAnchor hyp = new HtmlAnchor();
        hyp.ID = _hypID;
        hyp.InnerText = _description;
        hyp.Attributes.Add("class", "SP_dataLabel_Header");
        objContainer.Controls.Add(hyp);
    }
}

public class GridViewTemplate_Label : ITemplate
{
    string _labelID;

    public GridViewTemplate_Label(string labelID)
    {
        this._labelID = labelID;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.ID = _labelID;
        lbl.CssClass = "SP_dataLabel";
        objContainer.Controls.Add(lbl);
    }
}

public class GridViewTemplate_LabelPlusTextBox : ITemplate
{
    string _labelID;

    public GridViewTemplate_LabelPlusTextBox(string labelID)
    {
        this._labelID = labelID;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.ID = _labelID;
        lbl.CssClass = "SP_dataLabel";
        lbl.Visible = false;
        objContainer.Controls.Add(lbl);
        TextBox txtBox = new TextBox();
        txtBox.Visible = false;
        txtBox.Attributes.Add("style", "text-align:right");
        txtBox.Width = 35;
        txtBox.CssClass = "SP_dataLabel";
        txtBox.ID = "txt" + _labelID;
        objContainer.Controls.Add(txtBox);
    }
}

public class GridViewTemplate_FooterLabel : ITemplate
{
    string _labelID;

    public GridViewTemplate_FooterLabel(string labelID)
    {
        this._labelID = labelID;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.ID = _labelID;
        lbl.CssClass = "SP_dataLabel";
        objContainer.Controls.Add(lbl);
        lbl = new Label();
        lbl.Text = "<BR>";
        objContainer.Controls.Add(lbl);
        lbl = new Label();
        lbl.ID = _labelID + "_Pages";
        lbl.CssClass = "SP_dataLabel";
        objContainer.Controls.Add(lbl);
    }
}

public class GridViewTemplate_WF_CheckBox : ITemplate
{
    bool _isHeader;

    public GridViewTemplate_WF_CheckBox(bool isHeader)
    {
        _isHeader = isHeader;
    }

    public void InstantiateIn(Control objContainer)
    {
        CheckBox cb = new CheckBox();
        cb.ID = "chkPrint";
        cb.CssClass = "SP_dataLabel";
        //cb.ClientIDMode = ClientIDMode.Static;
        cb.TabIndex = -1;
        if (_isHeader)
            cb.Attributes.Add("onclick", "javascript:selectAllCheckboxes(this, 'Print')");
        objContainer.Controls.Add(cb);
    }
}

public class GridViewTemplate_LinkButton_WF : ITemplate
{
    private string _target = "";
    private string _server = "";
    private string _args = "";

    public GridViewTemplate_LinkButton_WF(string target, string server, string args)
    {
        _target = target;
        _server = server;
        _args = args;
    }

    public void InstantiateIn(Control objContainer)
    {
        HtmlAnchor hyp = new HtmlAnchor();
        hyp.DataBinding += new EventHandler(lnkbtn_DataBinding);
        objContainer.Controls.Add(hyp);
    }

    private void lnkbtn_DataBinding(object sender, EventArgs e)
    {
        HtmlAnchor hyp = (HtmlAnchor)sender;
        GridViewRow row = (GridViewRow)hyp.NamingContainer;

        string fn = DataBinder.Eval(row.DataItem, "friendlyName").ToString();
        string WFID = DataBinder.Eval(row.DataItem, "WFID").ToString();
        string BP = DataBinder.Eval(row.DataItem, "BP").ToString();
        hyp.Target = _target;
        hyp.ID = "lnkbtn_" + WFID;
        hyp.InnerHtml = fn;
        hyp.Attributes.Add("class", "SP_dataLabel");
        hyp.HRef = "http://" + _server + "/mainForm.aspx?BP=" + BP + "&workflowID=" + WFID + "&" + _args;
        hyp.Attributes.Add("onclick", "javascript:fnLink('" + BP + "','" + WFID + "');");
        hyp.Title = "Click to view item ...";
        hyp.Style.Add("color", "black");
    }
}

public class GridViewTemplate_Boolean : ITemplate
{
    string FieldName;
    DisplayValue dv;
    bool _useColors;
    bool _isExcel;

    public GridViewTemplate_Boolean(string fieldName, DisplayValue dv, bool useColors, bool isExcel)
    {
        _useColors = useColors;
        this.FieldName = fieldName;
        this.dv = dv;
        _isExcel = isExcel;
    }

    private void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow row = (GridViewRow)lbl.NamingContainer;
        string v = "";
        bool b = false;
        if (DataBinder.Eval(row.DataItem, FieldName) != DBNull.Value)
        {
            b = bool.Parse(DataBinder.Eval(row.DataItem, FieldName).ToString());
            switch (dv)
            {
                case DisplayValue.trueFalse:
                    if (b)
                        v = "&nbsp;True&nbsp;";
                    else
                        v = "&nbsp;False&nbsp;";
                    break;

                case DisplayValue.yesNo:
                    if (b)
                        v = "&nbsp;Yes&nbsp;";
                    else
                        v = "&nbsp;No&nbsp;";
                    break;

                case DisplayValue.zeroOne:
                    if (b)
                        v = "&nbsp;0&nbsp;";
                    else
                        v = "&nbsp;1&nbsp;";
                    break;
            }
        }
        lbl.Text = v;

        if (_useColors && v.Length > 0)
        {
            if (!_isExcel)
            {
                lbl.ForeColor = System.Drawing.Color.White;
                if (b)
                    lbl.BackColor = System.Drawing.Color.Green;
                else
                    lbl.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                if (b)
                    lbl.ForeColor = System.Drawing.Color.Green;
                else
                    lbl.ForeColor = System.Drawing.Color.Red;
                lbl.Font.Bold = true;
            }
        }
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.DataBinding += new EventHandler(lbl_DataBinding);
        lbl.CssClass = "SP_dataLabel";
        objContainer.Controls.Add(lbl);
    }

    public enum DisplayValue
    {
        yesNo,
        trueFalse,
        zeroOne
    }
}

public class GridViewTemplate_Status : ITemplate
{
    CWF_HelpDesk.Tools2 myTools;
    CWF_HelpDesk.Workflow2 myWF;

    public GridViewTemplate_Status(CWF_HelpDesk.Tools2 tools, CWF_HelpDesk.Workflow2 workflow2)
    {
        myTools = tools;
        myWF = workflow2;
    }

    public void InstantiateIn(Control objContainer)
    {
        Label lbl = new Label();
        lbl.DataBinding += new EventHandler(lbl_DataBinding);
        objContainer.Controls.Add(lbl);
    }

    private void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow row = (GridViewRow)lbl.NamingContainer;

        lbl.ID = "lblStatus";
        int WFID = (int)DataBinder.Eval(row.DataItem, "WFID");
        //int Status = (int)DataBinder.Eval(row.DataItem, "Status");
        //string friendlyName = DataBinder.Eval(row.DataItem, "friendlyName").ToString();
        lbl.Text = getStatus(WFID, myTools, myWF);
    }

    public static string getStatus(int WFID, CWF_HelpDesk.Tools2 tools, CWF_HelpDesk.Workflow2 workflow2)
    {
        CWF_HelpDesk.Workflow2.DBStatus DBStatus = workflow2.WorkflowStatus(WFID);
        return workflow2.WorkflowStatusDescription(DBStatus);
    }
}