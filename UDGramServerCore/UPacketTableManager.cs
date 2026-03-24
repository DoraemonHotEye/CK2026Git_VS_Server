using MakePacketTemplate.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDGramServerCore
{
    public class UPacketTableManager
    {
        private class ROUNDING_TABLE_PACKET
        {
            private ushort PacketIDKey;
            private uint seqNum;

            private PacketInterface RoundingPacket;
        
            public ROUNDING_TABLE_PACKET(ushort PacketIDKey, uint seqNum, PacketInterface roundpacket)
            {
                this.PacketIDKey = PacketIDKey;
                this.seqNum = seqNum;
                this.RoundingPacket = roundpacket;
            }

            public void RemoveSelf()
            {
                UPacketTableManager.inst.EnterDestroyKey(PacketIDKey);
            }
        }

        private static UPacketTableManager inst = null;
        public static UPacketTableManager Instance
        {
            get
            {
                if (inst == null)
                    inst = new UPacketTableManager();

                return inst;
            }
        }

        private object _lock = new object();

        private Dictionary<int, ROUNDING_TABLE_PACKET> _roundPacketTable = new Dictionary<int, ROUNDING_TABLE_PACKET>();
        private List<int> DestroyKeys = new List<int>();

        public void EnterTablePacket(ushort PacketIDKey, uint seqNum, PacketInterface roundingPacket)
        {
            lock (_lock)
            {
                _roundPacketTable.Add(PacketIDKey, new ROUNDING_TABLE_PACKET(PacketIDKey, seqNum, roundingPacket));
            }
        }

        public void EnterDestroyKey(ushort PacketIDKey)
        {
            lock (_lock)
            {
                if (_roundPacketTable.ContainsKey(PacketIDKey))
                    return;

                DestroyKeys.Add(PacketIDKey);
            }
        }

        public void Update()
        {
            lock (_lock)
            {
                if (DestroyKeys.Count > 0)
                {
                    foreach(var key in DestroyKeys)
                    {
                        _roundPacketTable.Remove(key);
                    }
                }
            }
        }
    }
}
