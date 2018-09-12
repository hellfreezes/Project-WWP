using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionFunctions {

    static ConstructionFunctions instance;
    static Dictionary<string, Action<Unit, float>> actions;

    public static ConstructionFunctions Instance
    {
        get
        {
            return instance;
        }
    }

    public ConstructionFunctions ()
    {
        if (instance != null)
        {
            Debug.Log("Дублирование ConstructionFunctions");
        }
        instance = this;

        CreateActionsLibrary();
    }

    private void CreateActionsLibrary()
    {
        actions = new Dictionary<string, Action<Unit, float>>();
        actions.Add("LumberMillUpdate", LumberMillUpdate);
    }

    public Action<Unit, float> GetAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogError("Функция " + name + " не найдена в библиотеке функций");
            return null;
        }
        return actions[name];
    }

    public static void LumberMillUpdate(Unit construction, float deltaTime)
    {
        construction.SetParam("counter", construction.GetParam("counter") - deltaTime);
        if (construction.GetParam("counter") <= 0)
        {
            //Тут добавить ресурсов
            ResourcesController.Instance.AddResource(new Resources(ResourceName.WOOD, (int)construction.GetParam("productionValue")));

            construction.SetParam("counter", construction.GetParam("timeToWait"));
        } 
    }
}
