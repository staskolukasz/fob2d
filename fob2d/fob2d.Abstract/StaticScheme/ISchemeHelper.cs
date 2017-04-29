using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fob2d.Abstract
{
    public interface ISchemeHelper
    {
        List<INode> GetNodesWithLoad();
    }
}
