using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MapObjectController<Tile> {
    protected override string PatternsFile
    {
        get
        {
            return "tiles";
        }
    }
}
