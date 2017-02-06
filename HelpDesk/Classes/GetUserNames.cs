using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Classes
{
    public class GetNames
    {

        public GetNames()
        {

        }

        public static string[] GetADUserNames(string prefixText, int count)
        {
            List<string> names = new List<string>();
            try
            {
                int n = 0;
                CWF_Security.SecurityCredentials secCred = new CWF_Security.SecurityCredentials();
                CWF_Corporate.Authentication AU = new CWF_Corporate.Authentication(secCred, false);
                List<CWF_Corporate.Profile> adList = AU.GetUserProfiles("cn=" + prefixText + "*");
                foreach (CWF_Corporate.Profile p in adList)
                {
                    if (n++ < count)
                        names.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(p.friendlyName, p.userName));
                    else
                        break;
                }
                return names.ToArray();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                //Response.End();            
                names.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(prefixText, ""));
                names.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(ex.Message, ""));
                return names.ToArray();
            }
        }
    }
}