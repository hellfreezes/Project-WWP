using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//public abstract class UnitController<C> : MapObjectController<С> where C : MapObject
//{

//}

public abstract class MapObjectController<T> : MonoBehaviour where T : MapObject {
    public Transform Handler;

    protected abstract string PatternsFile { get; }

    protected Dictionary<string, T> unitPatterns;

    protected List<T> units;

    public EventHandler PatternsLoaded;

    protected virtual void Start()
    {
        units = new List<T>();
        if (unitPatterns == null)
            LoadPatterns();
    }

    protected virtual void LoadPatterns()
    {
        unitPatterns = new Dictionary<string, T>();

        string path = Application.streamingAssetsPath + "/JSON/" + PatternsFile + ".json";
        JArray<T> serelizedList = new JArray<T>();
        Debug.Log(typeof(T).ToString());
        using (StreamReader stream = new StreamReader(path))
        {
            string json = stream.ReadToEnd();
            serelizedList = JsonUtility.FromJson<JArray<T>>(json);
        }
        foreach (T unit in serelizedList.list)
        {
            unitPatterns.Add(unit.JName, (T)unit.DeserliazePattern());
        }

        OnPatternsLoaded();
    }

    public virtual T Place(string name, Vector2 position)
    {
        T u = (T)unitPatterns[name].Clone();

        u.Place(position);
        if (u != null)
        {
            u.ObjectHandler.transform.SetParent(Handler);

            units.Add(u);

            return u;
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (T u in units)
        {
            u.Update(GameManager.Instance.GameSpeed);
        }
    }

    public T GetPattern(string name)
    {
        if (unitPatterns == null)
            LoadPatterns();

        if (!unitPatterns.ContainsKey(name))
        {
            Debug.LogError("В базе данных прототипов отсутвует " + name);
            return null;
        }

        return unitPatterns[name];
    }

    public T[] GetAllPatterns()
    {
        if (unitPatterns == null)
            LoadPatterns();

        return unitPatterns.Values.ToArray();
    }

    protected virtual void OnPatternsLoaded()
    {
        Debug.Log("Выполняю OnPatternsLoaded");
        if (PatternsLoaded != null)
        {
            PatternsLoaded(this, EventArgs.Empty);
        }
    }

    public virtual T[] GetSerialized()
    {
        JArray<T> saveData = new JArray<T>();
        foreach (MapObject unit in units)
            unit.Serialize();
        return units.ToArray();
    }

    public virtual void LoadFromList(T[] list)
    {
        Clear();

        foreach(T unit in list)
        {
            T u = (T)unitPatterns[unit.JName].Clone();
            u.Place(new Vector2(unit.JPositionX, unit.JPositionY));
            u.ObjectHandler.transform.SetParent(Handler);
            units.Add(u);
        }
    }

    public virtual void Clear()
    {
        foreach (T unit in units)
        {
            Destroy(unit.ObjectHandler);
        }

        PatternsLoaded = null;
        units.Clear();
    }
}
