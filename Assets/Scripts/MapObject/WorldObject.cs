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

    public WorldObject(string name, Sprite sprite, int tileSizeWidth, int tileSizeHeigth) 
        : base (name, sprite, tileSizeWidth, tileSizeHeigth)
    {
        
    }

    public WorldObject(WorldObject other) : base(other)
    {

    }

    public override MapObject Clone()
    {
        WorldObject worldObject = new WorldObject(this);
        return worldObject;
    }
}
