using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameUtility {
    #region Map Position
    private static Dictionary<string, MapPosition> mapMapping = new Dictionary<string, MapPosition>();
    private static bool IsMapMappingInit = false;
    private static void InitmapMapping() {
        if(IsMapMappingInit) {
            return;
        }
        mapMapping.Clear();
        char c = 'A';
        for (int y = 1; y <= 8; y++) {
            for (int x = 1; x <= 8; x++) {
                string Id = string.Format("B{0}{1}", c, x);
                mapMapping[Id] = new MapPosition(x, y);
                //UnityEngine.Debug.LogFormat("{0} => {1},{2}", Id, x, y);
            }
            c++;
        }

        IsMapMappingInit = true;
    }
    public static  MapPosition MapToPosition(string id) {
        InitmapMapping();
        if (mapMapping.ContainsKey(id)) {
            return mapMapping[id];
        }
        return null;
    }
    #endregion
}
public class MapPosition {
    public MapPosition(int x, int y) {
        this.X = x;
        this.Y = y;
    }
    public int X;
    public int Y;

    public int Distance(MapPosition otherPosition) {
        int x = Math.Abs(otherPosition.X - this.X);
        int y = Math.Abs(otherPosition.Y - this.Y);
        return x + y;
    }
}
