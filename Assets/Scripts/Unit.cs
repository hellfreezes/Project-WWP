using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Unit {
    public Vector2 Position { get; set; }
    
    protected UnitRenderer unitRenderer;
    protected List<Tile> tilesRef;
    protected abstract string LayerName { get; }

    public GameObject ObjectHandler { get; protected set; }
    public Sprite ObjectSprite { get; protected set; }
    public string ObjectName { get; protected set; }
    public int Width { get; protected set; }
    public int Heigth { get; protected set; }

    public Dictionary<string, float> Parameters { get; protected set; }
    public Action<Unit, float> UpdateActions { get; protected set; }

    //JSON public vars
    public string JName;
    public string JSpriteName;
    public int JWidth;
    public int JHeigth;
    public JFloat[] JParameters;
    public string[] JOnUpdateAction;

    public Unit()
    {

    }

    public Unit(string name, Sprite objectSprite, int tileSizeWidth, int tileSizeHeigth)
    {
        this.ObjectName = name;
        this.ObjectSprite = objectSprite;
        this.Width = tileSizeWidth;
        this.Heigth = tileSizeHeigth;
        this.Parameters = new Dictionary<string, float>();
    }

    public Unit(Vector2 position)
    {
        this.Position = position;
    }

    public Unit(Unit other)
    {
        Width = other.Width;
        Heigth = other.Heigth;
        ObjectName = other.ObjectName;
        ObjectSprite = other.ObjectSprite;
        Parameters = new Dictionary<string, float>(other.Parameters);
        tilesRef = new List<Tile>();
        UpdateActions = other.UpdateActions;
    }

    public abstract Unit Clone(Tile t);

    public virtual Unit DeserelizePattern()
    {
        this.ObjectName = this.JName;
        this.ObjectSprite = SpriteManager.current.GetSprite(LayerName, this.JSpriteName);
        this.Width = this.JWidth;
        this.Heigth = this.JHeigth;
        foreach (JFloat param in this.JParameters)
        {
            SetParam(param.name, param.value);
        }
        return this;
    }

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
        ObjectHandler.transform.localPosition = new Vector3(Position.x + ((Width - 1) / 2), Position.y + ((Heigth - 1) / 2), (Position.y / 100f));
    }

    public virtual bool IsBuildTileVaild(Tile t)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
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
        for (int i = 0; i < unit.Width; i++)
        {
            for (int j = 0; j < unit.Heigth; j++)
            {
                Tile tileToPlace = World.Instance.GetTileAt(t.X + i, t.Y + j);
                tileToPlace.SetUnit(unit);
                unit.tilesRef.Add(tileToPlace);
            }
        }
    }
}
