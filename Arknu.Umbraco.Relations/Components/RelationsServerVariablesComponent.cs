using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Arknu.Umbraco.Relations.Controllers;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;

namespace Arknu.Umbraco.Relations.Components
{
    public class RelationsServerVariablesComponent : IComponent
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public RelationsServerVariablesComponent(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        public void Initialize()
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        public void Terminate()
        {
            ServerVariablesParser.Parsing -= ServerVariablesParser_Parsing;
        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> dictionary)
        {
            var umbracoContext = _umbracoContextAccessor.UmbracoContext;
            if (umbracoContext == null) throw new InvalidOperationException("Umbraco context is null");

            var urlHelper = new UrlHelper(umbracoContext.HttpContext.Request.RequestContext);
            var url = urlHelper.GetUmbracoApiServiceBaseUrl<RelationsApiController>(it => it.GetRelations(1, "test"));

            dictionary.Add("arknuRelations", new Dictionary<string, object>
            {
                {"relationsApiBase", url}
            });

        }
    }
}
