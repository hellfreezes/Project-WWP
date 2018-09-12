using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : Unit {
    
    public Resources Resources { get; protected set; }

    protected override string LayerName
    {
        get
        {
            return "Construction";
        }
    }

    public Construction(string name, Sprite objectSprite, Vector2 tileSize, Resources res) : base(name, objectSprite, tileSize)
    {
        this.Resources = res;
    }

    public Construction(Construction other) : base(other)
    {
        Resources = other.Resources;
    }

    public override Unit Clone(Tile t)
    {
        Construction construction = new Construction(this);
        construction.Position = new Vector2(t.X, t.Y);
        SetTilesReference(construction, t);
        return construction;
    }
}
