using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject {
    public int X { get; protected set; }
    public int Y { get; protected set; }

    private UnitRenderer unitRenderer;
    private GameObject obj;

    public WorldObject(int x, int y)
    {
        X = x;
        Y = y;
    }
}
