using a = Arknu.Umbraco.Relations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Arknu.Umbraco.Relations.Controllers
{
    [PluginController("ArknuRelations")]
    public class RelationsApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<a.RelationType> GetRelationTypes()
        {
            var result = new List<a.RelationType>();

            var relations = ApplicationContext.Services.RelationService.GetAllRelationTypes();
            foreach (var r in relations)
            {
                result.Add(new a.RelationType()
                {
                    Id = r.Id,
                    Alias = r.Alias,
                    Name = r.Name
                });
            }

            return result;
        }

        public IEnumerable<a.Relation> GetRelations(int id, string type)
        {
            var result = new List<a.Relation>();

            var reltype = ApplicationContext.Services.RelationService.GetRelationTypeByAlias(type);

            IEnumerable<IRelation> relations = null;
            if (reltype.IsBidirectional)
            {
                relations = ApplicationContext.Services.RelationService.GetByParentOrChildId(id);
            }
            else
            {
                relations = ApplicationContext.Services.RelationService.GetByParentId(id);
            }
            foreach (var r in relations.Where(x => x.RelationType.Alias == type))
            {
                IContent target;
                if (r.ChildId == id && reltype.IsBidirectional)
                {
                    target = ApplicationContext.Services.ContentService.GetById(r.ParentId);
                }
                else
                {
                    target = ApplicationContext.Services.ContentService.GetById(r.ChildId);
                }

                result.Add(RelationFromContent(target, r));
            }

            return result;
        }

        public a.Relation SaveRelation(int sourceid, int targetid, string type)
        {
            var source = ApplicationContext.Services.ContentService.GetById(sourceid);
            var target = ApplicationContext.Services.ContentService.GetById(targetid);

            var r = ApplicationContext.Services.RelationService.Relate(source, target, type);

            return RelationFromContent(target, r);
        }

        public void DeleteRelation(int relationid)
        {

            var r = ApplicationContext.Services.RelationService.GetById(relationid);
            ApplicationContext.Services.RelationService.Delete(r);

            
        }


        private a.Relation RelationFromContent(IContent target, IRelation relation )
        {
            return new a.Relation()
            {
                ContentId = target.Id,
                Name = target.Name,
                RelationId = relation.Id,
                Icon = target.ContentType.Icon,
                Path = target.Path,
                Published = target.Published,
                Thrashed = target.Trashed,
                Url = (target.Published ? Umbraco.TypedContent(target.Id)?.Url ?? "" : "")
                
            };
        }
    }
}
