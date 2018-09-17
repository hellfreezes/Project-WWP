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

public class ConstructionController : MapObjectController<Construction> {
    private ConstructionFunctions constructionFunctions;

    public EventHandler<ConstructionArgs> ConstructionPlaced;

    protected override string PatternsFile
    {
        get
        {
            return "constructions";
        }
    }

    private void Awake()
    {
        constructionFunctions = new ConstructionFunctions();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public override Construction Place(string name, Vector2 position)
    {
        Tile t = World.Instance.GetTileAt((int)position.x, (int)position.y);
        if (!unitPatterns[name].IsBuildTileVaild(t))
        {
            Debug.Log("Место занято");
            return null;
        }

        Construction c = base.Place(name, position);
        if (c != null)
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
