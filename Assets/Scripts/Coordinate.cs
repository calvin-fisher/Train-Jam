using System.Collections.Generic;
using System.Linq;

public enum Direction : byte
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4,
}

public struct Coordinate
{
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly int X;
    public readonly int Y;

    public Coordinate Up
    {
        get {  return new Coordinate(X, Y + 1); }
    }
    public Coordinate Down
    {
        get { return new Coordinate(X, Y - 1); }
    }
    public Coordinate Left
    {
        get { return new Coordinate(X - 1, Y); }
    }
    public Coordinate Right
    {
        get { return new Coordinate(X + 1, Y); }
    }
    private IEnumerable<Coordinate> AdjacentNeighbors
    {
        get
        {
            yield return Up;
            yield return Down;
            yield return Left;
            yield return Right;
        }
    }

    public Coordinate GetNeighbor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Up;

            case Direction.Right:
                return Right;

            case Direction.Down:
                return Down;

            case Direction.Left:
                return Left;

            default:
                return this;
        }
    }

    public Direction GetDirectionToNeighbor(Coordinate neighbor)
    {
        if (neighbor == Up)
        {
            return Direction.Up;
        }
        else if (neighbor == Right)
        {
            return Direction.Right;
        }
        else if (neighbor == Down)
        {
            return Direction.Down;
        }
        else if (neighbor == Left)
        {
            return Direction.Left;
        }
        else
        {
            return Direction.None;
        }
    }

    public IEnumerable<Coordinate> GetAdjacentNeighbors()
    {
        return AdjacentNeighbors
            .Where(TileManager.Instance.IsValidCoordinate);
    }

    #region Equality Operators
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Coordinate)obj);
    }

    public bool Equals(Coordinate other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X * 397) ^ Y;
        }
    }

    public static bool operator ==(Coordinate first, Coordinate second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    public static bool operator !=(Coordinate first, Coordinate second)
    {
        return !(first == second);
    }
    #endregion
}
