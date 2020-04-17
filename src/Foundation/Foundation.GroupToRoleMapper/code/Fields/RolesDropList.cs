using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Applications.ContentEditor;

namespace Sitecore.GroupToRoleMapper.Fields
{
    public class RolesDropList : LookupEx
    {
        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull(output, "output");
            var values = GetValueList();

            output.Write($"<select {GetControlAttributes()}>");
            output.Write("<option value=''></option>");
            var valueExistsInList = false;
            foreach (var value in values)
            {
                if (IsSelected(value))
                    valueExistsInList = true;
                output.Write($"<option value='{value}' {(IsSelected(value) ? "selected='selected'" : string.Empty)}>{value}</option>");
            }

            var valueNotInSelection = !string.IsNullOrEmpty(Value) && !valueExistsInList;
            if (valueNotInSelection)
            {
                output.Write($"<optgroup label='{Translate.Text("Value not in the selection list.")}'>");
                output.Write($"<option value='{Value}' selected='selected'>{Value}</option>");
                output.Write($"</optgroup>");
            }
            output.Write("</select>");

            if (valueNotInSelection)
                output.Write($"<div style='color:#999999;padding:2px 0px 0px 0px'>{Translate.Text("The field contains a value that is not in the selection list.")}</div>");
        }

        private bool IsSelected(string value)
        {
            Assert.ArgumentNotNull(value, "value");
            return Value == value;
        }

        private static IEnumerable<string> GetValueList()
        {
            var roles = RolesInRolesManager.GetAllRoles();

            return roles.Select(r => r.Name).Append(Constants.AdminText).ToArray();
        }
    }
}