using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Transform StartPosition;
    public LayerMask WallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;
    
    Node[,] grid;
    [SerializeField]
    public List<Node> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridSizeX / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int y = 0; y < gridSizeX; y++) {
            for (int x = 0; x < gridSizeX; x++) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool Wall = true;
                if (Physics.CheckSphere(worldPoint, nodeRadius, WallMask)) {
                    Wall = false;
                }
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid == null) { return; }

        foreach(Node n in grid) {
            if (n.IsWall) {
                Gizmos.color = Color.white;
            } else {
                Gizmos.color = Color.yellow;
            }

            if (FinalPath != null) {
                if (FinalPath.Contains(n)) {
                    Gizmos.color = Color.red;
                }
            }

            Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter - Distance));
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition) {
        float x = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float y = ((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);

        int _x = Mathf.RoundToInt((gridSizeX - 1) * x);
        int _y = Mathf.RoundToInt((gridSizeY - 1) * y);
        return grid[_x, _y];
    }

    public List<Node> GetNeighboringNodes(Node n) {
        List<Node> neighboringNodes = new List<Node>();

        var checckPoint = new int[] {
            n.gridX + 1, n.gridY, // right
            n.gridX - 1, n.gridY, // left
            n.gridX, n.gridY + 1, // top
            n.gridX, n.gridY - 1, // bottom
        };
        for(int i = 0; i < 8; i+=2) {
            int xCheck = checckPoint[i];
            int yCheck = checckPoint[i + 1];
            if (xCheck >= 0 && xCheck < gridSizeX) {
                if (yCheck >= 0 && yCheck < gridSizeY) {
                    neighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }
        }

        return neighboringNodes;
    }
}
