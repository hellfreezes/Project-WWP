using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObject {

    public Vector2 Position { get; set; }

    protected UnitRenderer unitRenderer;
    protected abstract string LayerName { get; }

    public GameObject ObjectHandler { get; protected set; }
    public Sprite ObjectSprite { get; protected set; }
    public string ObjectName { get; protected set; }

    public Dictionary<string, float> Parameter { get; protected set; }
    public Action<MapObject, float> UpdateAction { get; protected set; }

    public string JName;
    public string JSpriteName;
    public int JPositionX;
    public int JPositionY;
    public JFloat[] JParameters;
    public string[] JOnUpdateAction;

    public MapObject()
    {

    }

    public MapObject(Vector2 position)
    {
        Position = position;
    }

    public MapObject(string name, Sprite objectSprite)
    {
        this.ObjectName = name;
        this.ObjectSprite = objectSprite;
        this.Parameter = new Dictionary<string, float>();
    }

    public MapObject(MapObject other)
    {
        ObjectName = other.ObjectName;
        ObjectSprite = other.ObjectSprite;
        Parameter = new Dictionary<string, float>(other.Parameter);
        UpdateAction = other.UpdateAction;
    }

    public abstract MapObject Clone();

    public virtual MapObject DeserelizePattern()
    {
        this.ObjectName = this.JName;
        this.ObjectSprite = SpriteManager.current.GetSprite(LayerName, this.JSpriteName);
        this.Parameter = new Dictionary<string, float>();

        if (this.JParameters != null)
        {
            foreach (JFloat param in this.JParameters)
            {
                SetParam(param.name, param.value);
            }
        }
        return this;
    }

    public virtual void Place(Vector2 position)
    {
        GameObject go = new GameObject();
        go.name = GameObjectName(this.ObjectName);
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = this.ObjectSprite;
        sr.sortingLayerName = LayerName;
        this.SetGameObject(go);
    }

    protected virtual string GameObjectName(string name)
    {
        return name + "[" + Position.x + ", " + Position.y + "]";
    }

    protected virtual void SetGameObject(GameObject go)
    {
        ObjectHandler = go;
        ObjectHandler.transform.localPosition = new Vector3(Position.x, Position.y, 0);
    }

    public virtual void Update(float deltaTime)
    {
        if (UpdateAction != null)
        {
            UpdateAction(this, deltaTime);
        }
    }

    public float GetParam(string name)
    {
        if (Parameter.ContainsKey(name))
        {
            return Parameter[name];
        }
        else
        {
            return 0f;
        }
    }

    public void SetParam(string name, float value)
    {
        if (Parameter.ContainsKey(name) == false)
        {
            Parameter.Add(name, value);
        }
        else
        {
            Parameter[name] = value;
        }
    }

    public void RegisterOnUpdate(Action<MapObject, float> callback)
    {
        UpdateAction += callback;
    }

    public void UnregisterOnUpdate(Action<MapObject, float> callback)
    {
        UpdateAction -= callback;
    }
}
