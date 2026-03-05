
using MakePacketTemplate.Packets;
using ServerCore;
using System;
using System.Collections.Generic;

public class UPacketManager
{
    #region Singleton
    static UPacketManager _instance = new UPacketManager();
    public static UPacketManager Instance { get { return _instance; } }
    #endregion

    UPacketManager()
    {
        Register();
    }

    
    public void Register()
    {
        
        
    }
}