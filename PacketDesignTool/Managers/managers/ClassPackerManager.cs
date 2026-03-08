using PacketDesignTool.Managers.Parents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketDesignTool
{
    public class ClassPackerManager : PackerManager
    {
        public ClassPackerManager() : base()
        {
            Prefix = "class";
        }

        private static ClassPackerManager _inst;

        public static ClassPackerManager Inst => _inst ?? (_inst = new ClassPackerManager());


        public override void OnDeleteList(Packer packer)
        {

        }

        public override void OnEnterList(Packer packer)
        {

        }
    }
}
