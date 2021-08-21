using Arknu.Umbraco.Relations.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Arknu.Umbraco.Relations.Composers
{
    public class Startup : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<RelationsServerVariablesComponent>();
        }
    }
}
