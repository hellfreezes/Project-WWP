using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MapObject
{
    #region OldClass
    //public int X { get; protected set; }
    //public int Y { get; protected set; }

    //private UnitRenderer unitRenderer;
    //private GameObject obj;
    //private Unit unit;

    //public Tile(int x, int y)
    //{
    //    X = x;
    //    Y = y;
    //}

    //public void SetUnit(Unit u)
    //{
    //    unit = u;
    //}

    //public bool Free()
    //{
    //    if (unit != null)
    //        return false;

    //    return true;
    //}
    #endregion

    private Unit unit;

    public Tile(Tile other) : base(other)
    {

    }

    public override MapObject Clone()
    {
        Tile tile = new Tile(this);
        return tile;
    }

    public override void Place(Vector2 position)
    {
        Position = position;
        base.Place(position);
    }

    protected override string LayerName
    {
        get
        {
            return "Tile";
        }
    }

    public int X
    {
        get { return (int)Position.x; }
    }

    public int Y
    {
        get { return (int)Position.y; }
    }

    public void SetUnit(Unit u)
    {
        unit = u;
    }

    public bool Free()
    {
        if (unit != null)
            return false;

        return true;
    }
}
