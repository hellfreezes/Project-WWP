﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionFunctions {

    static ConstructionFunctions instance;
    static Dictionary<string, Action<MapObject, float>> actions;

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
        actions = new Dictionary<string, Action<MapObject, float>>();
        actions.Add("LumberMillUpdate", LumberMillUpdate);
        actions.Add("LumberMillControls", LumberMillControls);
    }

    public Action<MapObject, float> GetAction(string name)
    {
        if (!actions.ContainsKey(name))
        {
            Debug.LogError("Функция " + name + " не найдена в библиотеке функций");
            return null;
        }
        return actions[name];
    }

    public static void LumberMillUpdate(MapObject construction, float deltaTime)
    {
                                                                            // зависимость от количества работающих людей
        construction.SetParam("counter", construction.GetParam("counter") - (deltaTime));
        if (construction.GetParam("counter") <= 0)
        {
            //Тут добавить ресурсов
            ResourcesController.Instance.AddResource(new Resources(ResourceName.WOOD, (int)construction.GetParam("productionValue")));

            construction.SetParam("counter", construction.GetParam("timeToWait"));
        } 
    }

    public static void LumberMillControls(MapObject construction, float deltaTime)
    {
        Debug.Log("Заполнение панели командами");
        ValueWindow window =  CommandBoxController.Instance.AddValueWindow();
        window.Init((Construction)construction, "Workers", "people", 
            0, construction.GetParam("peopleMax"), ResourceName.HUMAN, 
            ResourcesController.Instance.MainResources);
    }
}
