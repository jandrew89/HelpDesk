using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

public class FE_PopUpMenu
{
    private string _ID = "";
    private string _menuColor = "#DBDBDB";
    private string _backGroundColor = "#B0C4DE";
    public List<FE_PopUpMenuItem> menuItems;    
    
	public FE_PopUpMenu(string ID)
    {
        _ID = ID;
        menuItems = new List<FE_PopUpMenuItem>();
    }

    public IEnumerator<FE_PopUpMenuItem> GetEnumerator()
    {
        return this.menuItems.GetEnumerator();
    }

    #region //********************* PROPERTIES ***************************/
    public int Count { get { return menuItems.Count; } }

    public void AddMenuItem(FE_PopUpMenuItem item)
    {
        menuItems.Add(item);
    }

    public void ClearMenuItems()
    {
        menuItems.Clear();
    }    

    public string menuColor
    {
        get { return _menuColor; }
        set
        {
            _menuColor = value;
        }
    }

    public string backgroundColor
    {
        get { return _backGroundColor; }
        set
        {
            _backGroundColor = value;
        }
    }

    public string ID
    {
        get { return _ID; }
        set
        {
            _ID = value;
        }
    }
    #endregion
}

public class FE_PopUpMenuItem
{
    private string _hREF = "";
    private string _script = "";
    private string _Text = "";
    private string _Target = "";
    private string _ImageURL = "";
    private bool _isSeparator = false;
    private bool _enabled = true;
    private bool _isVariable = false;
    private string _variableName = "";

    public FE_PopUpMenuItem()
    {
    }

    public FE_PopUpMenuItem(string hREF, string Text, string Target, string imageURL)
    {
        _hREF = hREF;
        _Text = Text;
        _Target = Target;
        _ImageURL = imageURL;    
    }

    public FE_PopUpMenuItem(string hREF, string Text, string Target, string imageURL, bool isSeparator)
    {
        _hREF = hREF;
        _Text = Text;
        _Target = Target;
        _ImageURL = imageURL;
        _isSeparator = isSeparator;
    }

    public FE_PopUpMenuItem(string hREF, string Text, string Target, string imageURL, bool isSeparator, bool enabled)
    {
        _hREF = hREF;
        _Text = Text;
        _Target = Target;
        _ImageURL = imageURL;
        _isSeparator = isSeparator;
        _enabled = enabled;
    }   

    #region //****************** PROPERTIES ***********************/
    public string hREF
    {
        get { return _hREF; }
        set
        {
            _hREF = value;
        }
    }

    public string script
    {
        get { return _script; }
        set
        {
            _script = value;
        }
    }

    public string Text
    {
        get { return _Text; }
        set
        {
            _Text = value;
        }
    }

    public string Target
    {
        get { return _Target; }
        set
        {
            _Target = value;
        }
    }

    public string ImageURL
    {
        get { return _ImageURL; }
        set
        {
            _ImageURL = value;
        }
    }

    public bool isSeparator
    {
        get { return _isSeparator; }
        set
        {
            _isSeparator = value;
        }
    }

    public bool isVariable
    {
        get { return _isVariable; }
        set
        {
            _isVariable = value;
        }
    }

    public string variableName
    {
        get { return _variableName; }
        set
        {
            _variableName = value;
        }
    }

    public bool Enabled
    {
        get { return _enabled; }
        set
        {
            _enabled = value;
        }
    }
    #endregion
}

public class FE_PopUp
{
    //public FE_PopUp()
    //{ }    

    private static string writetMenu(FE_PopUpMenu menu, Page clientPage)
    {
        ClientScriptManager csm = clientPage.ClientScript;
        
        StringBuilder sb = new StringBuilder();
        sb.Append("<div id=\"" + menu.ID + "\"style=\"position:absolute;display:none;background-color:" + menu.menuColor + ";");
        sb.Append("border-left-width:1px; border-top-width:1px; border-right-width:2px; border-bottom-width:2px;");
        sb.Append("border-right-color:Gray; border-bottom-color:Gray; border-style:solid; border-top-color:White; border-left-color:White\">\n");
        sb.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\">\n");
        foreach (FE_PopUpMenuItem item in menu.menuItems)
        {
            sb.Append("<tr>\n");
            if (item.isSeparator)
            {
                sb.Append("<td colspan=\"1\" style=\"background-image:url(images/ig_menuSep.gif);background-repeat:repeat-x;\"></td>\n");
            }
            else if (item.isVariable)
            {
                if (item.variableName.Length == 0)
                    throw new Exception("Menu items marked as variable must have a variable name!");

                sb.Append("<td align=\"center\"><span style=\"font-size:11px; width:100%; font-weight:bold; height:100%; font-family:Verdana\" id=\"" + item.variableName + "\"></span></td>\n");                
            }
            else
            {                
                sb.Append("<td style=\"white-space:nowrap; border:1px Solid " + menu.menuColor + "\">\n");
                sb.Append("<a onmouseover=\"javascript:tdOver(this)\" onmouseout=\"javascript:tdOut(this)\" ");
                if (item.hREF.Length > 0 && item.Enabled)
                {
                    sb.Append("href=\"" + item.hREF + "\"");
                    if(item.script.Length > 0)
                        csm.RegisterClientScriptBlock(clientPage.GetType(), item.Text, item.script, true);
                }

                sb.Append("style=\"font-size:11px; width:100%; height:100%; display:block; font-family:Tahoma; text-decoration:none; color:Black\"");                
                if (item.Target.Length > 0)
                    sb.Append(" target=\"" + item.Target + "\"");
                sb.Append(">");
                string img = item.ImageURL;
                if (img.Length == 0)
                    img = "images/pixel_trans.gif";

                sb.Append("<img border=\"0\" style=\"vertical-align:middle\" width=\"16px\" height=\"16px\" src=\"" + img + "\">&nbsp;&nbsp;");
                sb.Append(item.Text);
                sb.Append("</a></td>\n");
            }
            sb.Append("</tr>\n");
        }
        sb.Append("</table></div>\n");        
        return sb.ToString();        
    }

    public static string writeMenus(Page clientPage, List<FE_PopUpMenu> menus, string containerID)
    {
        if(menus.Count == 0)
            return "";

        string myMenuList = "";
        string retval = "";

        FE_PopUpMenu menu1 = menus[0];

        foreach (FE_PopUpMenu m in menus)
        {
            myMenuList += m.ID + ",";
            retval += writetMenu(m, clientPage);
        }
            
        if(myMenuList.Length > 0)
            myMenuList = myMenuList.Substring(0, myMenuList.Length - 1);
        
        StringBuilder sb = new StringBuilder();
        sb.Append("document.onmouseup = fnHideMenu;\n\n");

        sb.Append("function tdOver(x) {\n");
        sb.Append("x.style.color = \"#FFFFFF\";\n");
        sb.Append("x.parentNode.style.border = \"1px Solid #000000\";\n");
        sb.Append("x.parentNode.style.backgroundColor = \"" + menu1.backgroundColor + "\"; }\n\n");                

        sb.Append("function tdOut(x) {\n");
        sb.Append("x.style.color = \"#000000\";\n");
        sb.Append("x.parentNode.style.border = \"1px Solid " + menu1.menuColor + "\";\n");
        sb.Append("x.parentNode.style.backgroundColor = \"" + menu1.menuColor + "\"; }\n\n");

        sb.Append("function fnShowMenu(myObject, menuName) {\n");
        sb.Append("var pos = findPos(myObject); \n");
        sb.Append("var posLeft = pos[0]; \n var posTop = pos[1]; \n");
        sb.Append("var tblMenu = document.getElementById(menuName);\n");
        sb.Append("tblMenu.style.left = posLeft;\n");
        sb.Append("tblMenu.style.top = posTop;\n");
        sb.Append("tblMenu.style.display = \"block\";\n");
        sb.Append("var menuHeight = parseInt(tblMenu.offsetHeight, 10);\n");
        sb.Append("var iPosTop = parseInt(posTop);\n");
        sb.Append("var myWidth = 0; var myHeight = 0; var dim = getWindowSize(); myHeight = dim[1]; \n");
        sb.Append("var clientHeight = parseInt(myHeight); \n");
        //sb.Append("var absoluteTop = iPosTop - parseInt($(\"" + containerID + "\").scrollTop); \n");
        if (containerID.Length > 0)
            sb.Append("var absoluteTop = iPosTop - $(" + containerID + ").scrollTop(); \n"); // parseInt($(" + containerID + ").scrollTop); \n");
        else
            sb.Append("var absoluteTop = iPosTop; \n");
        //sb.Append("alert(iPosTop); ");
        sb.Append("if(absoluteTop + menuHeight > clientHeight) { iPosTop = clientHeight - menuHeight; } else iPosTop = absoluteTop;");        
        sb.Append("tblMenu.style.left = posLeft + 20 + 'px';\n");
        sb.Append("tblMenu.style.top = iPosTop + 'px';\n");
        //sb.Append("alert(absoluteTop + ':' + clientHeight + ':' + iPosTop + ':' + menuHeight);");
        sb.Append("event.returnValue = false; } \n\n");     
    
        sb.Append("function fnHideMenu() { \n");
        sb.Append("var myMenus = \"" + myMenuList + "\";\n");
        sb.Append("var menus = myMenus.split(\",\");\n");
        sb.Append("for (var x = 0; x < menus.length; x++) {\n");
        sb.Append("var tblMenu = document.getElementById(menus[x]);\n");
        sb.Append("tblMenu.style.display = \"none\"; } \n}\n\n");

        sb.Append("function findPos(obj) {\n");
        sb.Append("var curleft = curtop = 0;\n");
        sb.Append("if (obj.offsetParent) { ");
        sb.Append(" do { curleft += obj.offsetLeft; curtop += obj.offsetTop; } while (obj = obj.offsetParent); } \n\n");
        sb.Append("return [curleft, curtop]; } \n\n");
    

        sb.Append("function getWindowSize() { \n");
        sb.Append("if (typeof (window.innerWidth) == 'number') { \n");
        sb.Append("//Non-IE \n");
        sb.Append("myWidth = window.innerWidth; \n");
        sb.Append("myHeight = window.innerHeight; \n");
        sb.Append("} else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) { \n");
        sb.Append("//IE 6+ in 'standards compliant mode' \n");
        sb.Append("myWidth = document.documentElement.clientWidth; \n");
        sb.Append("myHeight = document.documentElement.clientHeight; \n");
        sb.Append("} else if (document.body && (document.body.clientWidth || document.body.clientHeight)) { \n");
        sb.Append("//IE 4 compatible \n");
        sb.Append("myWidth = document.body.clientWidth; \n");
        sb.Append("myHeight = document.body.clientHeight; \n");
        sb.Append("}");
        sb.Append("//window.alert('Width = ' + myWidth); \n");
        sb.Append("//window.alert('Height = ' + myHeight); \n");
        sb.Append("return [myWidth, myHeight]; } ");
        
        ClientScriptManager csm = clientPage.ClientScript;
        csm.RegisterClientScriptBlock(clientPage.GetType(), menu1.ID, sb.ToString(), true);
        return retval;
    }

    //****************************** SAMPLE CODE *******************************/
    //private void buildMenus()
    //{
    //    List<FE_PopUpMenu> menus = new List<FE_PopUpMenu>();

    //    FE_PopUpMenu m = new FE_PopUpMenu("myMenu1");

    //    FE_PopUpMenuItem item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Modify";
    //    item1.ImageURL = "/support/img/Edit16.png";
    //    item1.hREF = "javascript:fnMenuModifyStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Insert";
    //    item1.ImageURL = "/support/img/Add16.png";
    //    item1.hREF = "javascript:fnMenuInsertStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Reset";
    //    item1.ImageURL = "/support/img/refresh16.png";
    //    item1.hREF = "javascript:fnMenuresetStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.isSeparator = true;
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Send Mail";
    //    item1.ImageURL = "/support/img/Send (Email)16.png";
    //    item1.hREF = "javascript:fnMenuEmail()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.isSeparator = true;
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Delete";
    //    item1.ImageURL = "/support/img/Delete16.png";
    //    item1.hREF = "javascript:fnMenuDeleteStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.isSeparator = true;
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Move Up";
    //    item1.ImageURL = "/support/img/Move Up (Arrow)16.png";
    //    item1.hREF = "javascript:fnMenuMoveUp()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Move Down";
    //    item1.ImageURL = "/support/img/Move Down (Arrow)16.png";
    //    item1.hREF = "javascript:fnMenuMoveDown()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.isSeparator = true;
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Cancel";
    //    m.menuItems.Add(item1);

    //    menus.Add(m);

    //    m = new FE_PopUpMenu("myMenu2");

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Modify";
    //    item1.ImageURL = "/support/img/Edit16.png";
    //    item1.hREF = "javascript:fnMenuModifyStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Insert";
    //    item1.ImageURL = "/support/img/Add16.png";
    //    item1.hREF = "javascript:fnMenuInsertStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Reset";
    //    item1.ImageURL = "/support/img/refresh16.png";
    //    item1.hREF = "javascript:fnMenuresetStep()";
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.isSeparator = true;
    //    m.menuItems.Add(item1);

    //    item1 = new FE_PopUpMenuItem();
    //    item1.Text = "Send Mail";
    //    item1.ImageURL = "/support/img/Send (Email)16.png";
    //    item1.hREF = "javascript:fnMenuEmail()";
    //    m.menuItems.Add(item1);

    //    menus.Add(m);

    //    string retval = FE_PopUp.writeMenus(this.Page, menus);
    //    Literal l = new Literal();
    //    l.Text = retval;
    //    phMenu1.Controls.Add(l);
    //}        
}