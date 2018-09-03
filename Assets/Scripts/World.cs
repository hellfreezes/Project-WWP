﻿using System;
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

        CreateWorld();
    }

    private void CreateWorld()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                Tile tile = new Tile(x, y);
                tiles.Add(new Vector2(x, y), tile);
                GameObject go = new GameObject();
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                int i = UnityEngine.Random.Range(0, 3);
                sr.sprite = gameManager.GetTile(i);
                go.transform.SetParent(gameManager.WorldHandler.transform);
                go.transform.position = new Vector3(x, y, 0);
            }
        }
    }

    public Tile GetTileAtCoords(Vector2 coords)
    {
        return tiles[coords];
    }
}