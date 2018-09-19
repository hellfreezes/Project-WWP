using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Construction : Unit {
    
    public Resources Resources { get; protected set; }
    public Action<MapObject, float> ControlAction { get; protected set; }
    public JInteger[] JResource;
    public string[] JControlAction;

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
        JResource = other.JResource;
        ControlAction = other.ControlAction;
        JControlAction = other.JControlAction;
    }

    public override MapObject Clone()
    {
        Construction construction = new Construction(this);
        return construction;
    }

    public override MapObject DeserliazePattern()
    {
        base.DeserliazePattern();
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
        if (this.JControlAction != null)
        {
            foreach (string action in this.JControlAction)
            {
                //TODO: надо бы как-то покрасивше это записать:
                ControlAction += ConstructionFunctions.Instance.GetAction(action);
            }
        }

        return this;
    }
}
