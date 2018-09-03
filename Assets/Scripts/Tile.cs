using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public int X { get; protected set; }
    public int Y { get; protected set; }

    private UnitRenderer unitRenderer;

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }
}
