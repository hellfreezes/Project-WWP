using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public int X { get; protected set; }
    public int Y { get; protected set; }

    private UnitRenderer unitRenderer;
    private GameObject obj;
    private Unit unit;

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetUnit(Unit u)
    {
        unit = u;
    }

    public bool Free()
    {
        if (unit != null)
            return false;

        return true;
    }
}
