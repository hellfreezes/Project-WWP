using System;
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
    public Resources Resources { get; protected set; }

    /// <summary>
    /// Словарь содержащий кастомные параметры (float) упорядоченных по ключевой строке (string)
    /// Для получение кастомного параметра необходимо знать ключ (string).
    /// Создано чтобы в дальнейшем грузить кастомные параметры из LUA текстовых файлов
    /// </summary>
    public Dictionary<string, float> Parameters { get; protected set; } // Кастомные параметры

    /// <summary>
    /// Перечень этих методов исполняется для фурнитуры каждый апдейт
    /// В данном случае float - это deltaTime.
    /// В вызываемом методе могут быть использованы параметры из словаря parameters
    /// </summary>
    public Action<Construction, float> UpdateActions { get; protected set; } // Какие-то действия которые умеет здание
    //protected List<string> updateActions; // строковые наименования функций полученных из LUA кода

    public Construction(string name, Sprite objectSprite, Vector2 tileSize, Resources res)
    {
        this.ObjectName = name;
        this.ObjectSprite = objectSprite;
        this.TileSize = tileSize;
        this.Resources = res;
        this.Parameters = new Dictionary<string, float>();
    }

    public Construction(Construction other)
    {
        TileSize = other.TileSize;
        ObjectName = other.ObjectName;
        ObjectSprite = other.ObjectSprite;
        tilesRef = new List<Tile>();
        Resources = other.Resources;
        Parameters = new Dictionary<string, float>(other.Parameters);
        UpdateActions = other.UpdateActions;
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
                construction.tilesRef.Add(constrTile);
            }
        }
        return construction;
    }

    public void SetGameObject(GameObject go)
    {
        ObjectReference = go;
        ObjectReference.transform.localPosition = new Vector3(X + ((TileSize.x - 1) / 2), Y + ((TileSize.y - 1) / 2), 0);
    }

    public bool IsBuildTileVaild(Tile t)
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

    public void Update(float deltaTime)
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

    public void RegisterOnUpdate(Action<Construction, float> callback)
    {
        UpdateActions += callback;
    }

    public void UnregisterOnUpdate(Action<Construction, float> callback)
    {
        UpdateActions -= callback;
    }
}
