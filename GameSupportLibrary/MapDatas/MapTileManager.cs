using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnitySupportLibrary.Static;

namespace GameSupportLibrary.MapDatas
{
    public class MapTileManager
    {
        private static MapTileManager inst = null;
        public static MapTileManager Instance => inst ?? (inst = new MapTileManager());

        private TilemapSurface mapTileSurface;
        public TilemapSurface TilemapSurface { get { return mapTileSurface; } }

        public MapTileManager()
        {

        }

        public void LoadMapDatas(string path)
        {
            string JsonDatas = System.IO.File.ReadAllText(path);
            mapTileSurface = JsonConvert.DeserializeObject<TilemapSurface>(JsonDatas);

            if (mapTileSurface.Tilemap.Count > 0)
                if (mapTileSurface.Tilemap[0].Count > 0)
                    Console.WriteLine($"[MapTileManager]: Width: {mapTileSurface.Tilemap.Count}, Height: {mapTileSurface.Tilemap[0].Count}");
        }
    }
}
