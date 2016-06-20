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
                string name;
                if (r.ChildId == id && reltype.IsBidirectional)
                {
                    var c = ApplicationContext.Services.ContentService.GetById(r.ParentId);
                    name = c.Name;
                }
                else
                {
                    var c = ApplicationContext.Services.ContentService.GetById(r.ChildId);
                    name = c.Name;
                }
                result.Add(new a.Relation()
                {
                    Name = name,
                    RelationId = r.Id,
                    ContentId = r.ChildId
                });
            }

            return result;
        }

        public a.Relation SaveRelation(int sourceid, int targetid, string type)
        {
            var source = ApplicationContext.Services.ContentService.GetById(sourceid);
            var target = ApplicationContext.Services.ContentService.GetById(targetid);

            var r = ApplicationContext.Services.RelationService.Relate(source, target, type);

            return new a.Relation()
            {
                ContentId = targetid,
                Name = target.Name,
                RelationId = r.Id
            };
        }

        public void DeleteRelation(int relationid)
        {

            var r = ApplicationContext.Services.RelationService.GetById(relationid);
            ApplicationContext.Services.RelationService.Delete(r);

            
        }
    }
}
