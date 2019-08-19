using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {
    Grid grid;
    public Transform StartPosition;
    public Transform TargetPosition;
    void Awake() {
        grid = GetComponent<Grid>();
    }

    void Start() {
        DoFilePath();
        InvokeRepeating("DoFilePath", 1, 1);
    }

    private void DoFilePath() {
        var startTime = DateTime.Now;
        FindPath(StartPosition.position, TargetPosition.position);
        var elapsedTime = DateTime.Now.Subtract(startTime);
        Debug.Log("elapsed:" + elapsedTime.TotalSeconds);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Node StartNode = grid.NodeFromWorldPosition(startPos);
        Node TargetNode = grid.NodeFromWorldPosition(targetPos);

        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Add(StartNode);
        while (OpenList.Count > 0) {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++) {
                if (OpenList[i].FCost < CurrentNode.FCost ||
                    OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost) {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode) {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach(Node neighborNode in grid.GetNeighboringNodes(CurrentNode)) {
                if (!neighborNode.IsWall || ClosedList.Contains(neighborNode)) {
                    continue;
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, neighborNode);

                if (MoveCost < neighborNode.gCost || !OpenList.Contains(neighborNode)) {
                    neighborNode.gCost = MoveCost;
                    neighborNode.hCost = GetManhattenDistance(neighborNode, TargetNode);
                    neighborNode.Parent = CurrentNode;
                    if (!OpenList.Contains(neighborNode)) {
                        OpenList.Add(neighborNode);
                    }
                }
            }
        }
    }

    void GetFinalPath(Node startNode, Node targetNode) {
        List<Node> FinalPath = new List<Node>();
        Node currentNode = targetNode;

        while(currentNode != startNode) {
            FinalPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        FinalPath.Reverse();
        grid.FinalPath = FinalPath;
    }

    int GetManhattenDistance(Node a, Node b) {
        int x = Mathf.Abs(a.gridX - b.gridX);
        int y = Mathf.Abs(a.gridY - b.gridY);
        return x + y;
    }
}
