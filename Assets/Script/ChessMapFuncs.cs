using UnityEngine;
using System.Collections;

public class ChessMapFuncs {
    /// <summary>
    /// 生成一个空的图
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static int[,] GenerateArray(int width, int height) {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++) {
            for (int y = 0; y < map.GetUpperBound(1); y++) {
                map[x, y] = 0;
            }
        }
        return map;
    }

    public static void RenderMap(int[,] map, GameObject prefab, Transform parent, Rect rect, Material[] mats) {
        float offsetX = rect.x;
        float offsetY = rect.y;
        float itemX = rect.width;
        float itemY = rect.height;

        if (prefab == null) return;
        int count = 0;
        char c = 'A';
        int idx = 0;
        for (int y = 0; y <= map.GetUpperBound(1); y++) {
            for (int x = 0; x <= map.GetUpperBound(0); x++) {
                count++;
                int type = map[x, y];
                var obj = Object.Instantiate(prefab, parent) as GameObject;
                float _x = offsetX + x * itemX;
                float _y = offsetY + y * itemY;
                obj.transform.localPosition = new Vector3(_x, 0.05f, _y);
                obj.SetActive(true);
                var comp = obj.GetComponent<ChessBlock>();
                comp.BlockTag = string.Format("B{0}{1}", c, x + 1);
                obj.isStatic = true;
                // load default materail
                var materialPath = string.Format("Material/{0}/{1}", c, x + 1);
                var mat = Resources.Load<Material>(materialPath);
                comp.GetComponent<MeshRenderer>().material = mats[idx++];
                obj.name = comp.BlockTag;
            }
            c++;
        }
    }
}