using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MapObject
{
    private Unit unit;

    public Tile(Tile other) : base(other)
    {

    }

    public override MapObject Clone()
    {
        Tile tile = new Tile(this);
        return tile;
    }

    public void SwitchTile(Tile t)
    {
        ObjectName = t.ObjectName;
        SetSprite(t.ObjectSprite);
        ObjectHandler.name = GameObjectName(ObjectName);
    }

    public override void Place(Vector2 position)
    {
        Position = position;
        base.Place(position);
    }

    protected override string GameObjectName(string name)
    {
        return "Tile-" + base.GameObjectName(name);
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
