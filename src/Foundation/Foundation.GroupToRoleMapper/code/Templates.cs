using Sitecore.Data;

namespace Sitecore.GroupToRoleMapper
{
    public static class Templates
    {
        public static class GroupToRoleTransformations
        {
            public static readonly ID Id = new ID("{B3C85F34-DF92-42E8-B37D-F03CC2BA1F74}");

            public static class Fields
            {
                public static readonly ID GroupName = new ID("{E99E4112-7BE5-4537-8522-7F7ED3E13CFB}");

                public static readonly ID SitecoreRole = new ID("{51C4A445-8BB4-4990-B8AA-CB8CC75294F2}");
            }
        }
    }
}