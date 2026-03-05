using PacketDesignTool.Managers;
using PacketDesignTool.Managers.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool
{
    public class StructPackerManager : PackerManager
    {
        public StructPackerManager() : base()
        {
            Prefix = "struct";
        }

        private static StructPackerManager _inst;
        public static StructPackerManager Inst => _inst ?? (_inst = new StructPackerManager());


        public override void OnDeleteList(Packer packer)
        {
            
        }

        public override void OnEnterList(Packer packer)
        {

        }
    }
}
