using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.Managers
{
    public interface ManagerInterface
    {
        void OnEnable(object sender, object EventKeyword);
        void OnDisable(object sender, object EventKeyword);
    }
}
