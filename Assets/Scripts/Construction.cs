using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Construction : Unit {
    
    public Resources Resources { get; protected set; }

    public JInteger[] JResource;

    protected override string LayerName
    {
        get
        {
            return "Construction";
        }
    }

    public Construction(string name, Sprite objectSprite, int tileSizeWidth, int tileSizeHeigth, Resources res) 
        : base(name, objectSprite, tileSizeWidth, tileSizeHeigth)
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

    public override Unit DeserelizePattern()
    {
        base.DeserelizePattern();
        Resources = new Resources();
        if (this.JResource != null)
        {
            foreach (JInteger r in this.JResource)
            {
                Resources.SetResource((ResourceName)Enum.Parse(typeof(ResourceName), r.name), r.value);
            }
        }
        if (this.JOnUpdateAction != null)
        {
            foreach (string action in this.JOnUpdateAction)
            {
                //TODO: надо бы как-то покрасивше это записать:
                RegisterOnUpdate(ConstructionFunctions.Instance.GetAction(action));
            }
        }

        return this;
    }
}
