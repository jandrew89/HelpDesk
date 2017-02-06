using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;

/// <summary>
/// Summary description for TableExport
/// </summary>
public class TableExport
{
    public TableExport(string fileName, Table table)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);        
        
        PrepareControlForExport(table);        
       
        table.RenderControl(htw);
        HttpContext.Current.Response.Write(sw.ToString());
        HttpContext.Current.Response.End();
    }

    //' Replace any of the contained controls with literals
    private void PrepareControlForExport(Control control)
    {
        int i = 0;
        while (i < control.Controls.Count)
        {
            Control current = control.Controls[i];

            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((LinkButton)current).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((ImageButton)current).AlternateText));
            }
            else if (current is Button || current is Image)
            {
                control.Controls.Remove(current);
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((HyperLink)current).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((DropDownList)current).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((CheckBox)current).Checked.ToString()));
            }
            else if (current is TextBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((TextBox)current).Text));
            }
            else if (current is HiddenField)
            {
                control.Controls.Remove(current);
                //TODO: Warning!!!, inline IF is not supported ?
            }
            else if (current is HtmlAnchor)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((HtmlAnchor)current).InnerText));
            }

            if (current.HasControls())
                PrepareControlForExport(current);

            ++i;
        }
    }
}