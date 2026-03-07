using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RoomManager
{
    private static RoomManager inst = null;
    public static RoomManager Instance => inst ?? (inst = new RoomManager());
    public RoomManager() { }

    Dictionary<string, Room> RoomList = new Dictionary<string, Room>();

    private object _lock = new object();
    
    public bool Remove(Room room)   // 방의 리스트 에서 RoomHashID 삭제 하기
    {
        lock (_lock)
        {
            bool succ = RoomList.Remove(room.RoomHashData);

            if(succ == true)
                Console.WriteLine($"[RoomManager]:: {room.RoomHashData} 삭제완료");

            return succ;
        }
    }
        
    public Room GenerateSingle(ushort MaxPlayerCount)
    {
        lock (_lock)
        {
            Room room = new Room(MaxPlayerCount, CreateDateHashTag());

            return room;
        }
    }

    public Room GenerateAndJoin(Player player, ushort MaxPlayerCount)
    {
        lock(_lock)
        {
            Room room = new Room(MaxPlayerCount, CreateDateHashTag());

            room.Enter(player);

            return room;
        }
    }

    public static string CreateDateHashTag()    // 날짜와 그리고 몇시 몇분에 들어왔는지
    {
        string date = DateTime.Now.ToString("yyyyMMdd");
        return $"#{date}";
    }
}

