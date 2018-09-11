using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject {
    public int X { get; protected set; }
    public int Y { get; protected set; }

    private UnitRenderer unitRenderer;
    private GameObject ObjectReference;

    public string ObjectName { get; set; }
    public Sprite ObjectSprite { get; set; }
    public Vector2 TileSize { get; set; }

    public WorldObject(int x, int y)
    {
        X = x;
        Y = y;
    }

    public WorldObject(string name, Sprite sprite, Vector2 size)
    {
        ObjectName = name;
        ObjectSprite = sprite;
        TileSize = size;
    }

    public WorldObject Clone(Tile t)
    {
        WorldObject w = new WorldObject(t.X, t.Y);
        w.ObjectName = this.ObjectName;
        w.ObjectSprite = this.ObjectSprite;
        w.TileSize = this.TileSize;

        return w;
    }

    public void SetGameObject(GameObject go)
    {
        ObjectReference = go;
        // (Y / 100f) - чтобы то, что находится дальше находилось под тем, что находится ближе
        ObjectReference.transform.localPosition = new Vector3(X + ((TileSize.x - 1) / 2), Y + ((TileSize.y - 1) / 2), (Y / 100f));
    }
}
