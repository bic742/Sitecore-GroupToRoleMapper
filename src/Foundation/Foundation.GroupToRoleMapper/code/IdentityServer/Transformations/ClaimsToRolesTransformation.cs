using System.Linq;
using System.Security.Claims;
using Foundation.GroupToRoleMapper.Extensions;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Services;

namespace Sitecore.GroupToRoleMapper.IdentityServer.Transformations
{
    public class ClaimsToRolesTransformation : Transformation
    {
        public override void Transform(ClaimsIdentity identity, TransformationContext context)
        {
            var groups = identity.FindAll("groups");

            var masterDatabase = Factory.GetDatabase("master");
            var rootTransformationItem = masterDatabase.GetItem(Settings.GetSetting(Constants.Settings.RoleTransformationsRootItemId, "{D65C7354-4B9F-409B-A621-2B023D435E58}"));

            if (rootTransformationItem == null)
            {
                return;
            }

            var transformations = rootTransformationItem.Children.Where(rti => rti.IsDerived(Templates.GroupToRoleTransformations.Id)).ToList();

            Log.Info($"Processing groups-to-role for user: {string.Join(" | ", groups.Select(g => g.Value))}", this);

            foreach (var transformation in transformations)
            {
                var source = transformation.Fields[Templates.GroupToRoleTransformations.Fields.GroupName]?.Value.ToLowerInvariant();
                var targetRole = transformation.Fields[Templates.GroupToRoleTransformations.Fields.SitecoreRole]?.Value;

                if (groups.Any(g => g.Value.Equals(source, System.StringComparison.InvariantCultureIgnoreCase)))
                {
                    var claim = targetRole.Equals(Constants.AdminText) 
                        ? new Claim(Constants.AdminClaim, bool.TrueString.ToLower()) 
                        : new Claim("role", targetRole);

                    identity.AddClaim(claim);
                }
            }
        }
    }
}