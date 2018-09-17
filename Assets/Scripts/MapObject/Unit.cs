using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Unit : MapObject {
    protected List<Tile> tilesRef;

    public int Width { get; protected set; }
    public int Heigth { get; protected set; }

    //JSON public vars
    public int JWidth;
    public int JHeigth;

    public Unit()
    {
        
    }

    public Unit(string name, Sprite objectSprite, int tileSizeWidth, int tileSizeHeigth) : base(name, objectSprite)
    {
        this.Width = tileSizeWidth;
        this.Heigth = tileSizeHeigth;
    }

    public Unit(Unit other) : base(other)
    {
        Width = other.Width;
        Heigth = other.Heigth;
        tilesRef = new List<Tile>();

        JWidth = other.JWidth;
        JHeigth = other.JHeigth;
    }

    public override MapObject DeserliazePattern()
    {
        base.DeserliazePattern();
        this.Width = this.JWidth;
        this.Heigth = this.JHeigth;

        return this;
    }

    public override void Place(Vector2 position)
    {
        Position = position;
        base.Place(position);
        SetTilesReference(Position);
    }

    protected override string GameObjectName(string name)
    {
        return name;
    }

    protected override void SetGameObject(GameObject go)
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

    protected void SetTilesReference(Vector2 position)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Heigth; j++)
            {
                Tile tileToPlace = World.Instance.GetTileAt((int)position.x + i, (int)position.y + j);
                tileToPlace.SetUnit(this);
                this.tilesRef.Add(tileToPlace);
            }
        }
    }
}
