using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace IdentityServerWithAspNetIdentity.Mvc
{
    public class FeaturesLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Features/{1}/{0}.cshtml", 
                "/Features/Shared/{0}.cshtml"
            };
        }
    }
}
