using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pathfinding : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static Node FindPath(Coordinate start, Coordinate end)
    {
        var startNode = new Node(start, end);

        var currentNode = startNode;
        var visitedTiles = new List<Coordinate>();
        var route = FindPathInner(currentNode, end, visitedTiles);
        
        return route;
    }

    private static Node FindPathInner(Node currentNode, Coordinate end, List<Coordinate> visitedTiles)
    {
        if (currentNode.Position == end)
            return currentNode;

        var nextSteps = currentNode.Position.GetAdjacentNeighbors()
            .Except(visitedTiles)
            .Where(x => TileManager.Instance.Get(x).CanBuildTrack)
            .Select(x => new Node(x, end))
            .Where(x => x.HeuristicDistanceToEnd <= currentNode.HeuristicDistanceToEnd)
            .OrderBy(x => x.HeuristicDistanceToEnd)
            .ThenBy(x => x.LargestDifferenceToEnd);

        foreach (var candidate in nextSteps)
        {
            visitedTiles.Add(candidate.Position);

            var candidateRoute = FindPathInner(candidate, end, visitedTiles);
            if (candidateRoute != null)
            {
                currentNode.Next = candidate;
                candidate.Previous = currentNode;
                return currentNode;
            }
        }

        return null;
    }

    public class Node
    {
        public Node() { }
        public Node(Coordinate position, Coordinate end)
        {
            Position = position;

            var xDiff = Math.Abs(position.X - end.X);
            var yDiff = Math.Abs(position.Y - end.Y);

            HeuristicDistanceToEnd = xDiff + yDiff;
            LargestDifferenceToEnd = Math.Max(xDiff, yDiff);
        }

        public IEnumerable<Node> SelfAndSuccessors()
        {
            yield return this;
            if (Next != null)
                foreach (var successor in Next.SelfAndSuccessors())
                    yield return successor;
        }

        public Coordinate Position;
        public int HeuristicDistanceToEnd;
        public int LargestDifferenceToEnd;
        public Node Previous;
        public Node Next;
    }
}
