using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WorldObjectController : MapObjectController<WorldObject>
{
    protected override string PatternsFile
    {
        get
        {
            return "worldobjects";
        }
    }

    public override WorldObject Place(string name, Vector2 position)
    {
        Tile t = World.Instance.GetTileAt((int)position.x, (int)position.y);
        if (!unitPatterns[name].IsBuildTileVaild(t))
        {
            Debug.Log("Место занято");
            return null;
        }

        WorldObject c = base.Place(name, position);
        return c;
    }
}
