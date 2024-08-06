using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour {
    
	PathRequestManager requestManager;
	Grid grid;
	Tilemap collisionTileMap;

	public int gridArea;
	
	void Awake() {
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
		collisionTileMap = transform.GetChild(0).GetComponent<Tilemap>();
	}
	
	
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
		StartCoroutine(FindPath(startPos,targetPos));
	}
	
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		
		Node startNode = NodeFromWorldPoint(startPos);
		Node targetNode = NodeFromWorldPoint(targetPos);
		
		if (startNode.walkable && targetNode.walkable) {
			Heap<Node> openSet = new Heap<Node>(gridArea);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
			while(openSet.Count > 0) 
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				
				if(currentNode.gridX == targetNode.gridX && currentNode.gridY == targetNode.gridY) 
				{
					targetNode.parent = currentNode.parent;
					pathSuccess = true;
					break;
				}
				
				foreach(Node neighbour in GetNodeNeighbours(currentNode)) 
				{
					if(!neighbour.walkable || closedSet.Contains(neighbour)) 
					{
						continue;
					}
					
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) 
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if(!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}	
					}
				}
			}
		}
		yield return null;
		if (pathSuccess) {
			waypoints = RetracePath(startNode,targetNode);
		}
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
	}

	// public Node NodeFromWorldPoint(Vector3 worldPosition) {
	// 	float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
	// 	float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
	// 	percentX = Mathf.Clamp01(percentX);
	// 	percentY = Mathf.Clamp01(percentY);

	// 	int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
	// 	int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
	// 	return grid[x,y];
	// }

	public Node NodeFromWorldPoint(Vector3 worldPosition) //<------------
    {
        Vector3Int nodePosition = grid.WorldToCell(worldPosition);

		Node node = new Node(!collisionTileMap.HasTile(nodePosition), grid.CellToWorld(nodePosition), nodePosition.x, nodePosition.y);

		return node;
    }

    public Node NodeFromVector3(Vector3Int nodePos) //<------------
    {
        Vector3 worldPosition = grid.CellToWorld(nodePos);

		Node node = new Node(!collisionTileMap.HasTile(nodePos), worldPosition, nodePos.x, nodePos.y);

		return node;
    }

	public List<Node> GetNodeNeighbours(Node node) { //<------------
		List<Node> neighbours = new List<Node>();

		for(int x = -1; x <= 1; x++) 
		{
			for(int y = -1; y <= 1; y++) 
			{
				if(x == 0 && y == 0)
				{
					continue;
				}
					
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				neighbours.Add(NodeFromVector3(new Vector3Int(checkX, checkY, 0)));
			}
		}

		//Debug.Log("Node (" + node.gridX + ", " + node.gridY + ") tem " + neighbours.Count + " vizinhos");
		return neighbours;
	}
	
	Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
	}
	
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}