using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit {
    public Vector2 Position { get; set; }

    protected UnitRenderer unitRenderer;
    protected List<Tile> tilesRef;

    protected abstract string LayerName { get; }

    public GameObject ObjectHandler { get; protected set; }
    public Sprite ObjectSprite { get; protected set; }
    public string ObjectName { get; protected set; }
    public Vector2 TileSize { get; protected set; }

    public Dictionary<string, float> Parameters { get; protected set; }
    public Action<Unit, float> UpdateActions { get; protected set; }

    public Unit(string name, Sprite objectSprite, Vector2 tileSize)
    {
        this.ObjectName = name;
        this.ObjectSprite = objectSprite;
        this.TileSize = tileSize;
        this.Parameters = new Dictionary<string, float>();
    }

    public Unit(Vector2 position)
    {
        this.Position = position;
    }

    public Unit(Unit other)
    {
        TileSize = other.TileSize;
        ObjectName = other.ObjectName;
        ObjectSprite = other.ObjectSprite;
        Parameters = new Dictionary<string, float>(other.Parameters);
        tilesRef = new List<Tile>();
        UpdateActions = other.UpdateActions;
    }

    public abstract Unit Clone(Tile t);

    public virtual void Place()
    {
        GameObject go = new GameObject();
        go.name = this.ObjectName;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = this.ObjectSprite;
        sr.sortingLayerName = LayerName;
        this.SetGameObject(go);
    }

    protected virtual void SetGameObject(GameObject go)
    {
        ObjectHandler = go;
        // (Y / 100f) - чтобы то, что находится дальше находилось под тем, что находится ближе
        ObjectHandler.transform.localPosition = new Vector3(Position.x + ((TileSize.x - 1) / 2), Position.y + ((TileSize.y - 1) / 2), (Position.y / 100f));
    }

    public virtual bool IsBuildTileVaild(Tile t)
    {
        for (int x = 0; x < TileSize.x; x++)
        {
            for (int y = 0; y < TileSize.y; y++)
            {
                if (!World.Instance.GetTileAt(t.X + x, t.Y + y).Free())
                {
                    return false;
                }
            }
        }

        return true;
    }

    public virtual void Update(float deltaTime)
    {
        if (UpdateActions != null)
        {
            UpdateActions(this, deltaTime);
        }
    }

    public float GetParam(string name)
    {
        return Parameters[name];
    }

    public void SetParam(string name, float value)
    {
        Parameters[name] = value;
    }

    public void RegisterOnUpdate(Action<Unit, float> callback)
    {
        UpdateActions += callback;
    }

    public void UnregisterOnUpdate(Action<Unit, float> callback)
    {
        UpdateActions -= callback;
    }

    protected void SetTilesReference(Unit unit, Tile t)
    {
        for (int i = 0; i < unit.TileSize.x; i++)
        {
            for (int j = 0; j < unit.TileSize.y; j++)
            {
                Tile tileToPlace = World.Instance.GetTileAt(t.X + i, t.Y + j);
                tileToPlace.SetUnit(unit);
                unit.tilesRef.Add(tileToPlace);
            }
        }
    }
}
