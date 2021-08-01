using a = Arknu.Umbraco.Relations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Arknu.Umbraco.Relations.Controllers
{
    [PluginController("ArknuRelations")]
    public class RelationsApiController : UmbracoAuthorizedJsonController
    {
        private readonly IRelationService _relationService;
        private readonly IContentService _contentService;

        public RelationsApiController()
        {
            _relationService = Services.RelationService;
            _contentService = Services.ContentService;
        }

        public IEnumerable<a.RelationType> GetRelationTypes()
        {
            var result = new List<a.RelationType>();

            var relations = _relationService.GetAllRelationTypes();
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

            var reltype = _relationService.GetRelationTypeByAlias(type);

            IEnumerable<IRelation> relations = null;
            if (reltype.IsBidirectional)
            {
                relations = _relationService.GetByParentOrChildId(id);
            }
            else
            {
                relations = _relationService.GetByParentId(id);
            }
            foreach (var r in relations.Where(x => x.RelationType.Alias == type))
            {
                IContent target;
                if (r.ChildId == id && reltype.IsBidirectional)
                {
                    target = _contentService.GetById(r.ParentId);
                }
                else
                {
                    target = _contentService.GetById(r.ChildId);
                }

                result.Add(RelationFromContent(target, r));
            }

            return result;
        }

        public a.Relation SaveRelation(int sourceid, int targetid, string type)
        {
            var source = _contentService.GetById(sourceid);
            var target = _contentService.GetById(targetid);

            var r = _relationService.Relate(source, target, type);

            return RelationFromContent(target, r);
        }

        public void DeleteRelation(int relationid)
        {
            var r = _relationService.GetById(relationid);
            _relationService.Delete(r);
        }


        private a.Relation RelationFromContent(IContent target, IRelation relation)
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
                Url = (target.Published ? Umbraco.Content(target.Id)?.Url ?? "" : "")

            };
        }
    }
}
