using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for JavaScript
/// </summary>
public class myClientScript
{
	public myClientScript()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string js_body_onLoad(string methodName)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<body onload=\"" + methodName + "();\"\n>"); 
        return sb.ToString();
    }

    public static string js_keyPress()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<script for=window event=onKeyPress>\nfnEsc(event);\n</script>\n\n");
        //sb.Append("function keyPress(event)\n{ fnEsc(event)}\n\n</script>");         
        return sb.ToString();
    }    

    public static string js_body_onUnLoad()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script for=window event=onunload>\n");
        sb.Append("window.returnValue=\"\";\n");
        sb.Append("</script>\n\n");
        return sb.ToString();
    }

    public static string js_warnOverwrite(string hdnOverwrite_ClientID, string darkBackgroundLayer_ClientID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("function warnOverwrite() \n{\n");
        sb.Append("\tdocument.getElementById(\"" + darkBackgroundLayer_ClientID + "\").style.display = \"\";\n");
        sb.Append("\tretval = confirm(\"Template already exists - overwrite?\");\n");
        sb.Append("\tdocument.getElementById(\"" + darkBackgroundLayer_ClientID + "\").style.display = \"none\";\n");
        sb.Append("\tif (retval == true)\n\t{\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnOverwrite_ClientID + "\").value = \"1\";\n");        
        sb.Append("\t\tdocument.aspnetForm.submit();\n");
        sb.Append("\t}\n");
        sb.Append("\telse\n\t{\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnOverwrite_ClientID + "\").value = \"\";\n\t}");
        sb.Append("\n}\n");
        return sb.ToString();        
    }

    public static string js_warnBOMOverwrite(string hdnOverwrite_ClientID, string hdnBillType_ClientID, string billType, string darkBackgroundLayer_ClientID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("function warnOverwrite() \n{\n");
        sb.Append("\tdocument.getElementById(\"" + darkBackgroundLayer_ClientID + "\").style.display = \"\";\n");
        sb.Append("\tretval = confirm(\"Part already exists - overwrite current BOM?\");\n");
        sb.Append("\tdocument.getElementById(\"" + darkBackgroundLayer_ClientID + "\").style.display = \"none\";\n");
        sb.Append("\tif (retval == true)\n\t{\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnOverwrite_ClientID + "\").value = \"1\";\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnBillType_ClientID + "\").value = \"" + billType + "\";\n");
        sb.Append("\t\tdocument.aspnetForm.submit();\n");
        sb.Append("\t}\n");
        sb.Append("\telse\n\t{\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnOverwrite_ClientID + "\").value = \"\";\n\t}");
        sb.Append("\n}\n");
        return sb.ToString();
    }

    public static string js_promptAddAnyway(string hdnAddAnywayClientID, string hdnAddAnywayDescriptionClientID, string darkBackgroundLayer_ClientID)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("function addAnyway() \n{\n");
        sb.Append("\tdocument.getElementById(\"" + darkBackgroundLayer_ClientID + "\").style.display = \"\";\n");
        sb.Append("\tretval = confirm(\"Part not found in JDE - add anyway?\");\n");
        sb.Append("\tdocument.getElementById(\"" + darkBackgroundLayer_ClientID + "\").style.display = \"none\";\n");
        sb.Append("\tif (retval == true)\n\t{\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnAddAnywayClientID + "\").value = \"1\";\n");
        sb.Append("\t\tretval = prompt(\"Item Description\",\"\");\n");
        sb.Append("\tif (retval != null)\n\t");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnAddAnywayDescriptionClientID + "\").value = retval;\n");
        sb.Append("\t\tdocument.aspnetForm.submit();\n");
        sb.Append("\t}\n");
        sb.Append("\telse\n\t{\n");
        sb.Append("\t\tdocument.forms(\"aspnetForm\").elements(\"" + hdnAddAnywayClientID + "\").value = \"\";\n\t}");
        sb.Append("\n}\n");
        return sb.ToString();
    }
}
