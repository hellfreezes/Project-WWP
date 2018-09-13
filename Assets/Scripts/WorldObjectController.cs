using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WorldObjectController : UnitController<WorldObject>
{
    protected override string PatternsFile
    {
        get
        {
            return "worldobjects";
        }
    }
}
