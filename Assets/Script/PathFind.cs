using System;
using System.Collections;
using System.Collections.Generic;
public class PathFind {
    public class Point{
        public int x;
        public int y;
    }

    /// <summary>
    /// 
    /// </summary>
     int[][] Dir = new int[4][] {
        new int[2]{ 1,  0}, // -- →
        new int[2]{ 0,  1}, // -- ↑
        new int[2]{ 0, -1}, // -- ↓
        new int[2]{-1,  0}, // -- ←
    };
    int[][] map;
    Point start;
    Point end;
    int cost;
    float diag = 1.4f;
    List<Point> openList = new List<Point>();
    List<Point> closeList = new List<Point>();


    public void Init(int[][] map, Point start, Point end) {
        this.map = map;
        this.start = start;
        this.end = end;
    }

    public List<Point> Search() {
        List<Point> result = null;
        // 终点是阻挡的话, 直接返回空路径
        if(this.map[end.x][end.y] != 0) {

        }
        return result;
    }
}
