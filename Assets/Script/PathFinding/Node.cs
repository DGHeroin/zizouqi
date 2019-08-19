using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Node {
    [SerializeField]
    public int gridX;
    [SerializeField]
    public int gridY;

    public bool IsWall;
    public Vector3 Position;
    public Node Parent;
    public int gCost;
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public Node (bool isWall, Vector3 pos, int x, int y) {
        IsWall = isWall;
        Position = pos;
        gridX = x;
        gridY = y;
    }
}
  