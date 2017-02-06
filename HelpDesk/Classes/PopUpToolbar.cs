using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PopUpToolbar
{
    public class PopUpToolbar
    {
        private string _webServer = "";
        private string _appPath = "";
        private string _title = "";
        private string _titleID = ""; //"lblPopUpToolbarTitle";

        public List<PopUpToolbarItem> toolbarColl = new List<PopUpToolbarItem>();

        public PopUpToolbar(string webserver, string appPath, string title, string titleID)
        {
            _webServer = webserver;
            _appPath = appPath;
            _title = title;
            //if(titleID.Length > 0)
                _titleID = titleID;
        }        

        public void LoadToolbarItem(toolbarDelegate myDelegate)
        {
            PopUpToolbarItem tbi = new PopUpToolbarItem();
            tbi.tbDelegate = myDelegate;
            toolbarColl.Add(tbi);
        }

        public void LoadToolbarItem(string url, string image, string imageText)
        {
            LoadToolbarItem(url, "", image, imageText, "", "");
        }

        public void LoadToolbarItem(string url, string script, string image, string imageText, string target)
        {
            LoadToolbarItem(url, script, image, imageText, target, "");
        }

        public void LoadToolbarItem(string url, string script, string image, string imageText, string target, string tooltip)
        {
            PopUpToolbarItem tbi = new PopUpToolbarItem();
            tbi.url = url;
            tbi.image = image;
            tbi.script = script;
            tbi.imgText = imageText;
            tbi.target = target;
            tbi.toolTip = tooltip;
            toolbarColl.Add(tbi);
        }

        public string DisplayDynamicToolbar()
        {
            StringBuilder sb = new StringBuilder();
            if (_title.Length > 0 || _titleID.Length > 0)
            {
                sb.Append("<table style=\"width:100%\" class=\"SP_H2\" cellpadding=\"3\"><tr><td style=\"white-space:nowrap\">");

                Label lbl = new Label();
                lbl.ID = _titleID; // "lblPopUpToolbarTitle";
                if (_title.Length > 0)
                    lbl.Text = _title;
                lbl.ClientIDMode = ClientIDMode.Static;
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                lbl.RenderControl(htw);
                sb.Append(sw.ToString());
                sb.Append("</td></tr></table>");
            }

            string output = "<div class=\"toolbar1\">";
            foreach (PopUpToolbarItem tbi in toolbarColl)
            {
                if (tbi.tbDelegate != null)
                {
                    output += tbi.tbDelegate.Invoke();
                    continue;
                }

                string target = "";
                string url = "javascript:;";
                string script = "";
                string text = tbi.imgText;
                string hover = "";

                if (text.Length > 0)
                    hover = " onmouseover=\"document.getElementById('" + text + "').className='AhoverOver'\" onmouseout=\"document.getElementById('" + text + "').className='AhoverOut'\" ";

                if (tbi.target != "")
                    target = "target=" + tbi.target;

                if (tbi.url != "")
                    url = tbi.url;

                if (tbi.script != "")
                    script = " onclick=\"" + tbi.script + "\" ";

                //if (text.Length > 0)
                //  text = "&nbsp;" + text;

                output += "<a id=\"" + text + "\" " + hover + target + " Title=\"" + tbi.toolTip + "\" style=\"float:left;margin-top:2px;margin-right:2px;margin-left:2px;\" href=\"" + url + "\"" + script + ">";
                if (tbi.image.Length > 0)
                    output += "<img border=\"0\" align=\"absmiddle\" src=\"http://" + _webServer + _appPath + "/images/" + tbi.image + "\" alt=\"" + tbi.toolTip + "\" />&nbsp;";
                output += "<font style=\"font-size:8pt;font-weight:normal;white-space:nowrap\">" + text + "</font></a>";
            }
            output += "</div>";
            sb.Append(output);
            return sb.ToString();
        }

        public void ClearToolbarItems()
        {
            toolbarColl.Clear();
        }
    }

    public class PopUpToolbarItem
    {
        public string url = "";
        public string script = "";
        public string image = "";
        public string imgText = "";
        public string target = "";
        public string toolTip = "";
        public toolbarDelegate tbDelegate = null;
    }
}