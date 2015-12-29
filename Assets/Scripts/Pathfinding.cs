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
        while (currentNode.Position != end)
        {
            var nextSteps = currentNode.Position.GetAdjacentNeighbors()
                .Except(visitedTiles)
                .Select(x => new Node(x, end))
                .OrderBy(x => x.HeuristicDistanceToEnd)
                .ThenBy(x => x.LargestDifferenceToEnd);
            //var nextSteps = currentNode.Position.GetAdjacentNeighbors().ToArray();
            //var excludedAlreadyVisited = nextSteps.Except(visitedTiles).ToArray();
            //var testNodes = excludedAlreadyVisited.Select(x => new Node(x, end)).ToArray();
            //var ordered = testNodes.OrderBy(x => x.HeuristicDistanceToEnd)
            //    .ThenBy(x => x.LargestDifferenceToEnd);

            var bestNextStep = nextSteps.FirstOrDefault();

            if (bestNextStep == null)
                return null;

            currentNode.Next = bestNextStep;
            bestNextStep.Previous = currentNode;
            visitedTiles.Add(currentNode.Position);
            currentNode = bestNextStep;
        }

        return startNode;
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




