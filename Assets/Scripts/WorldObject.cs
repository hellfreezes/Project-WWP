using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldObject : Unit {
    protected override string LayerName
    {
        get
        {
            return "WorldObject";
        }
    }

    public WorldObject(Vector2 position) : base (position)
    {
        
    }

    public WorldObject(string name, Sprite sprite, int tileSizeWidth, int tileSizeHeigth) 
        : base (name, sprite, tileSizeWidth, tileSizeHeigth)
    {
        
    }

    public WorldObject(WorldObject other) : base(other)
    {

    }

    public override Unit Clone(Tile t)
    {
        WorldObject worldObject = new WorldObject(this);
        worldObject.Position = new Vector2(t.X, t.Y);
        SetTilesReference(worldObject, t);
        return worldObject;
    }
}
