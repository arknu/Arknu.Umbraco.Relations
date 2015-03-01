using Arknu.Umbraco.Relations.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;
using Umb = Umbraco.Web;

namespace Arknu.Umbraco.Relations
{
    public class Events : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Umb.UI.JavaScript.ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;

        }

        void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> dictionary)
        {
            if (HttpContext.Current == null) throw new InvalidOperationException("HttpContext is null");
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));
            
            var url = urlHelper.GetUmbracoApiServiceBaseUrl<RelationsApiController>("GetRelations");

            dictionary.Add("arknuRelations", new Dictionary<string, object>
            {
                {"relationsApiBase", url}
            });  

        }
    }
}
