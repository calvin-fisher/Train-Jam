using System.Collections.Generic;

public struct Coordinate
{
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly int X;
    public readonly int Y;

    public IEnumerable<Coordinate> GetAdjacentNeighbors()
    {
        var right = new Coordinate(X + 1, Y);
        if (TileManager.Instance.IsValidCoordinate(right))
            yield return right;

        var up = new Coordinate(X, Y + 1);
        if (TileManager.Instance.IsValidCoordinate(up))
            yield return up;

        var left = new Coordinate(X - 1, Y);
        if (TileManager.Instance.IsValidCoordinate(left))
            yield return left;

        var down = new Coordinate(X, Y - 1);
        if (TileManager.Instance.IsValidCoordinate(down))
            yield return down;
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
