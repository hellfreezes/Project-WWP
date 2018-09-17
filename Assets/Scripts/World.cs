using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {
    public int Width { get; protected set; }
    public int Heigth { get; protected set; }

    private Dictionary<Vector2, Tile> tiles;

    private GameManager gameManager;

    private static World instance;

    public static World Instance {
        get { return instance; }    
    }

    public World(int width, int heigth)
    {
        instance = this;

        tiles = new Dictionary<Vector2, Tile>();

        Width = width;
        Heigth = heigth;

        gameManager = GameManager.Instance;

        GameManager.Instance.tileController.PatternsLoaded += CreateWorld;
    }

    private void CreateWorld(object source, EventArgs args)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                int i = UnityEngine.Random.Range(0, 3);
                Vector2 position = new Vector2(x, y);
                Tile tile = GameManager.Instance.tileController.Place(i.ToString(), position);
                tiles.Add(position, tile);
            }
        }

        // Отписываемся.
        GameManager.Instance.tileController.PatternsLoaded -= CreateWorld;
    }

    public Tile GetTileAt(int x, int y)
    {
        // Проверка попадают ли введенные координаты в рамки мира
        if (x >= Width || x < 0 || y >= Heigth || y < 0)
        {
            //Debug.LogError("Tile (" + x + ", " + y + ") is out of range");
            return null;
        }
        return tiles[new Vector2(x, y)];
    }
}
