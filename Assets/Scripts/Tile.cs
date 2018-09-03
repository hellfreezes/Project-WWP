using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public int X { get; protected set; }
    public int Y { get; protected set; }

    private UnitRenderer unitRenderer;
    private GameObject obj;
    private Construction construction;

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetConstruction(Construction c)
    {
        construction = c;
    }

    public bool Free()
    {
        if (construction != null)
            return false;

        return true;
    }
}
