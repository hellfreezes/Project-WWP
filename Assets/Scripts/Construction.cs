using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction {
    public int X { get; protected set; }
    public int Y { get; protected set; }
    
    private List<Tile> tilesRef;

    private UnitRenderer unitRenderer;

    public GameObject ObjectReference { get; protected set; }
    public Sprite ObjectSprite { get; protected set; }
    public string ObjectName { get; protected set; }
    public Vector2 TileSize { get; protected set; }

    public Construction(string name, Sprite objectSprite, Vector2 tileSize)
    {
        this.ObjectName = name;
        this.ObjectSprite = objectSprite;
        this.TileSize = tileSize;
    }

    public Construction(Construction other)
    {
        TileSize = other.TileSize;
        ObjectName = other.ObjectName;
        ObjectSprite = other.ObjectSprite;
        tilesRef = new List<Tile>();
    }

    public Construction Clone(Tile t)
    {
        Construction construction = new Construction(this);
        construction.X = t.X;
        construction.Y = t.Y;
        for (int i = 0; i < construction.TileSize.x; i++)
        {
            for (int j = 0; j < construction.TileSize.y; j++)
            {
                Tile constrTile = World.Instance.GetTileAt(t.X + i, t.Y + j);
                constrTile.SetConstruction(construction);
                Debug.Log(constrTile);
                tilesRef.Add(constrTile);
            }
        }
        return construction;
    }

    public void SetGameObject(GameObject go)
    {
        ObjectReference = go;
    }

    public void Update(float speed)
    {

    }
}
