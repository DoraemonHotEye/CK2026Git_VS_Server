using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool.DefaultClass
{
    public class StaticMembers
    {
        public static ConstList<string> memberList = 
            new ConstList<string>("ushort", "short", "int", "uint", "float", "string", "long", "char");
    }
}
