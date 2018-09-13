using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionArgs : EventArgs
{
    public Construction CurrentConstruction { get; set; }
}

public class ConstructionController : UnitController<Construction> {
    private ConstructionFunctions constructionFunctions;

    public EventHandler<ConstructionArgs> ConstructionPlaced;

    protected override string PatternsFile
    {
        get
        {
            return "constructions";
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        constructionFunctions = new ConstructionFunctions();
    }

    public override Construction Place(string name, Tile t)
    {
        Construction c = base.Place(name, t);
        OnConstructionPlaced(c);
        return c;
    }
    
    protected virtual void OnConstructionPlaced(Construction c)
    {
        if (ConstructionPlaced != null)
        {
            ConstructionPlaced(this, new ConstructionArgs() { CurrentConstruction = c });
        }
    }
}
