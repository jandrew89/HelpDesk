using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CWF_HelpDesk;

public delegate string toolbarDelegate();
public delegate string titleBarDelegate();

public partial class Support_MasterPages_Tabs : System.Web.UI.MasterPage
{
    //private string _MyStyleSheet = "";
    private string _HeaderText = "";
    public ToolbarSystem MyToolbar;
    public string webServer = "";
    public string _appPath = "";
    //private bool _embedStyleSheet = false;
    private string _title = "";
    private string _formTarget = "";
    private string _baseTarget = "";
    private titleBarDelegate _tbd;

    protected bool _isAdmin = false;
    protected Tools2 tools;

    protected override void OnInit(EventArgs e)
    {
        webServer = this.Request.Url.Authority;
        _appPath = Request.ApplicationPath;
        MyToolbar = new ToolbarSystem(webServer, _appPath);

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Title1.Text = _title;
        if (_formTarget.Length > 0)
            aspnetForm.Target = _formTarget;
        if (_baseTarget.Length > 0)
            Header_Base.Attributes.Add("target", _baseTarget);
    }

    //public string GetStyleSheet()
    //{
    //    string retval = "";
    //    //if (_MyStyleSheet.Length > 0)
    //    //{
    //        //if (_embedStyleSheet == false)
    //        //{
    //        //    return "<link rel=\"stylesheet\" href=\"http://" + webServer + "/support/MasterPages/" + _MyStyleSheet + "\" type=\"text/css\" media=\"screen\" charset=\"utf-8\" />";
    //        //    //HtmlLink link = new HtmlLink();
    //        //    //link.Attributes.Add("type", "text/css");
    //        //    //link.Attributes.Add("rel", "stylesheet");
    //        //    //link.Attributes.Add("href", "http://" + webServer + "/support/masterpages/" + _MyStyleSheet);
    //        //    //this.Head1.Controls.Add(link);
    //        //}
    //        //else
    //        //{
    //        //    string filename = Server.MapPath("/support/masterpages/" + _MyStyleSheet);

    //        //    StreamReader sr = File.OpenText(filename);
    //        //    string s = sr.ReadToEnd();                
    //        //    return "<style type=\"text/css\">" + s + "</style>";                
    //        //}
    //  //  }
    //    return retval;
    //}

    public string ShowToolbar()
    {
        string output = "";

        if (_HeaderText != "")
        {
            output = "<nav class=\"navbar navbar-inverse\"><div class=\"container-fluid\"><div class=\"navbar-header\"><a class=\"navbar-brand hover\">" + _HeaderText + "</a></div>";
            if(_tbd != null)
                output += _tbd.Invoke();
            
            output += "<ul class=\"nav navbar-nav navbar-right\">";
            output += "<li class=\"dropdown\"><a class=\"dropdown-toggle\" role=\"button\" aria-expanded=\"false\" href=\"#\" data-toggle=\"dropdown\">" + "Jason Christman " + "<span class=\"caret\"></span></a>";
            output += "<ul class=\"dropdown-menu hover\">";
            output += "<li><a href=\"#\">" + "All Users" + "</a></li>";
            output += "<li><a href=\"#\">" + "All Users" + "</a></li>";
            output += "<li><a href=\"#\">" + "Users" + "</a></li>";
            if (true)
            {
                output += "<li role=\"separator\" class=\"divider\"></li>";
                output += "<li class=\"dropdown-submenu\">";
                output += "<a tabindex=\"-1\" class=\"test\" href=\"/admin.aspx\">" + "Administraion" + "</a>";
                output += "<ul class=\"dropdown-menu\">";
                output += "<li><a tabindex=\"-1\" href=\"/admin/ReplaceUser.aspx\">" + "Replace Users" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/admin/users.aspx?BP=<%=_BP%>\">" + "Users" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/workflow/WF_Manager.aspx?BP=<%=_BP%>\">" + "Workflows" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/workflow/FindUser.aspx?BP=0\">" + "Find User" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/admin/sites.aspx\">" + "Sites" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/admin/Departments.aspx?BP=<%=_BP%>\">" + "Departments" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/admin/Maintenance.aspx?BP=<%=_BP%>\">" + "Maintenance" + "</a></li>";
                output += "<li><a tabindex=\"-1\" href=\"/admin/Equipment.aspx?BP=<%=_BP%>\">" + "Equipment" + "</a></li>";
                output += "</ul></li>";
            }
            output += "</ul></li>";
            output += "<li><a href=\"#\"class=\"dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-bars fa-lg\"></i></a>";
            output += "<ul class=\"dropdown-menu hover\">";
            output += "<li><a href=\"#\">" + "Replace Users" + "</a></li>";
            output += "<li><a href=\"#\">" + "Users" + "</a></li>";
            output += "<li><a href=\"#\">" + "Workflows" + "</a></li>";
            output += "<li><a href=\"#\">" + "Find User" + "</a></li>";
            output += "<li><a href=\"#\">" + "Sites" + "</a></li>";
            output += "<li><a href=\"#\">" + "Departments" + "</a></li>";
            output += "<li><a href=\"#\">" + "Maintenance" + "</a></li>";
            output += "<li><a href=\"#\">" + "Equipment" + "</a></li>";
            output += "</ul></li>";
            output += "</ul></div></div></nav>";
        }
        if (MyToolbar.toolbarColl.Count > 0)
            output += MyToolbar.DisplayDynamicToolbar();

        return output;
    }

    public void fillTitle(titleBarDelegate tbd)
    {
        _tbd = tbd;

    }

    #region //PROPERTIES *********************
    //public bool EmbedStyleSheet
    //{
    //    get
    //    {
    //        return _embedStyleSheet;
    //    }
    //    set
    //    {
    //        _embedStyleSheet = value;
    //    }
    //}

    //public string MyStyleSheet
    //{
    //    get
    //    {
    //        return _MyStyleSheet;
    //    }
    //    set
    //    {
    //        _MyStyleSheet = value;
    //    }
    //}

    public string HeaderText
    {
        get
        {
            return _HeaderText;
        }
        set
        {
            _HeaderText = value;
        }
    }

    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            _title = value;
        }
    }

    public string FormTarget
    {
        get
        {
            return _formTarget;
        }
        set
        {
            _formTarget = value;
        }
    }

    public string BaseTarget
    {
        get
        {
            return _baseTarget;
        }
        set
        {
            _baseTarget = value;
        }
    }
    #endregion  
}

public class ToolbarItem
{
    public string url = "";
    public string script = "";
    public string image = "";
    public string imgText = "";
    public string target = "";
    public string toolTip = "";
    public string ddlText = "";
    public toolbarDelegate tbDelegate = null;
}

public class ToolbarSystem
{
    private string _webServer = "";
    private string _appPath = "";

    public List<ToolbarItem> toolbarColl = new List<ToolbarItem>();

    public ToolbarSystem(string webserver, string appPath)
    {
        _webServer = webserver;
        _appPath = appPath;
    }

    public void LoadToolbarItem(toolbarDelegate myDelegate)
    {
        ToolbarItem tbi = new ToolbarItem();
        tbi.tbDelegate = myDelegate;
        toolbarColl.Add(tbi);
    }

    public void LoadToolbarItem(string url, string image, string imageText)
    {
        LoadToolbarItem(url, "", image, imageText, "", "", "");
    }

    public void LoadToolbarItem(string url, string script, string image, string imageText, string target)
    {
        LoadToolbarItem(url, script, image, imageText, target, "", "");
    }

    public void LoadToolbarItem(string url, string script, string image, string imageText, string target, string tooltip)
    {
        LoadToolbarItem(url, script, image, imageText, target, tooltip, "");
    }

    public void LoadToolbarItem(string url, string script, string image, string imageText, string target, string tooltip, string ddlText)
    {
        ToolbarItem tbi = new ToolbarItem();
        tbi.url = url;
        tbi.image = image;
        tbi.script = script;
        tbi.imgText = imageText;
        tbi.target = target;
        tbi.toolTip = tooltip;
        tbi.ddlText = ddlText;
        toolbarColl.Add(tbi);
    }

    public string DisplayDynamicToolbar()
    {
        string output = "<ul class=\"nav nav-pills\">";
        foreach (ToolbarItem tbi in toolbarColl)
        {
            if (tbi.tbDelegate != null)
            {
                output += tbi.tbDelegate.Invoke();
                continue;
            }
            string ddlText = "";
            string target = "";
            string url = "javascript:;";
            string script = "";
            string text = tbi.imgText;
            string hover = "";

            if (text.Length > 0)
                //hover = " onmouseover=\"document.getElementById('" + text + "').className='AhoverOver'\" onmouseout=\"document.getElementById('" + text + "').className='AhoverOut'\" ";

            if (tbi.target != "")
                target = "target=" + tbi.target;

            if (tbi.url != "")
                url = tbi.url;

            if (tbi.script != "")
                script = " onclick=\"" + tbi.script + "\" ";

            if (tbi.ddlText != "")
            {
                output = ddlText;
            }
            else
            {
                if (text.Length > 0)
                output += "<li><a id=\"" + text + "\" " + hover + target + " Title=\"" + tbi.toolTip + "\" href=\"" + url + "\"" + script + "/>";
                else
                output += "<li id=\"" + text + "\" " + hover + target + " Title=\"" + tbi.toolTip + "\" href=\"" + url + "\"" + script + "/>";

                if (tbi.image.Length > 0)
                    output += "<img border=\"0\" align=\"absmiddle\" src=\"http://" + _webServer + _appPath + "/images/" + tbi.image + "\" alt=\"" + tbi.toolTip + "\" />&nbsp;";
                output += text + "</a></li>";
            }
        }
        output += "</ul>";
        return output;
    }

    //public string DisplayDynamicToolbar1()
    //{
    //    string output = "<div class=\"toolbar1\"><div>"; 
    //    foreach (ToolbarItem tbi in toolbarColl)
    //    {            

    //        if (tbi.tbDelegate != null)
    //        {
    //            output += tbi.tbDelegate.Invoke();
    //            continue;
    //        }

    //        string target = "";
    //        string url = "javascript:;";
    //        string script = "";
    //        string text = tbi.imgText;
    //        string hover = "";

    //        if (text.Length > 0)
    //            hover = " onmouseover=\"document.getElementById('" + text + "').className='AhoverOver'\" onmouseout=\"document.getElementById('" + text + "').className='AhoverOut'\" ";

    //        if (tbi.target != "")
    //            target = "target=" + tbi.target;

    //        if (tbi.url != "")
    //            url = tbi.url;

    //        if (tbi.script != "")
    //            script = " onclick=\"" + tbi.script + "\" ";

    //        //if (text.Length > 0)
    //        //  text = "&nbsp;" + text;

    //        output += "<a id=\"" + text + "\" " + hover + target + " Title=\"" + tbi.toolTip + "\" style=\"float:left;margin-top:2px;margin-right:2px;margin-left:3px;\" href=\"" + url + "\"" + script + ">" +
    //                "<img border=\"0\" align=\"absmiddle\" src=\"http://" + _webServer + "/forecasting/images/" + tbi.image + "\" alt=\"" + tbi.toolTip + "\" />" + "<font style=\"font-size:8pt;font-weight:normal;white-space:nowrap\">" +
    //                "&nbsp;" + text + "</font></a>";
    //    }
    //    output += "</div></div>";
    //    return output;
    //}

    public void ClearToolbarItems()
    {
        toolbarColl.Clear();
    }

}