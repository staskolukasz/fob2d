using System.Linq;
using System.Collections.Generic;

using fob2d.Abstract;

namespace fob2d.StaticScheme
{
    internal class Helper : ISchemeHelper
    {
        private IScheme scheme;

        public Helper(IScheme scheme)
        {
            this.scheme = scheme;
        }

        public List<INode> GetNodesWithLoad()
        {
            IEnumerable<int> nodeNumbers = scheme.Loads
                .Where(load => load.NodeNumber != 0)
                .Select(node => node.NodeNumber);

            List<INode> nodeList = scheme.Nodes
                .Where(node => nodeNumbers.Contains(node.Number))
                .ToList();

            return nodeList;
        }
    }
}
