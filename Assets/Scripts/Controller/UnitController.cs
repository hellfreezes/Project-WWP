using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public abstract class UnitController<T> : MonoBehaviour where T : Unit {
    public Transform UnitHolder;

    protected abstract string PatternsFile { get; }

    private Dictionary<string, T> unitPatterns;

    private List<T> units;

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
            unitPatterns.Add(unit.JName, (T)unit.DeserelizePattern());
        }

        OnPatternsLoaded();
    }

    public virtual T Place(string name, Tile t)
    {
        if (!unitPatterns[name].IsBuildTileVaild(t))
        {
            Debug.Log("Место занято");
            return null;
        }

        T u = (T)unitPatterns[name].Clone(t);

        u.Place();
        if (u != null)
        {
            u.ObjectHandler.transform.SetParent(UnitHolder);

            units.Add(u);

            return u;
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Unit u in units)
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
}
