using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueWindow : MonoBehaviour {

    Construction construction;
    string parameterName;
    string parameter;

    float min;
    float max;

    ResourceName resourceName;
    Resources resources;

    Resources resourceStep;


    Text headerText;
    Text valueText;

    public void Init(Construction c, string parameterName, string parameter, float min, float max, ResourceName resourceName, Resources resources)
    {
        this.construction = c;
        this.parameter = parameter;
        this.parameterName = parameterName;
        this.min = min;
        this.max = max;
        this.resourceName = resourceName;
        this.resources = resources;
        headerText = transform.Find("Header").GetComponentInChildren<Text>();
        valueText = transform.Find("Value").GetComponentInChildren<Text>();

        resourceStep = new Resources(resourceName, 1); // Шаг ресурсов

        Fill();
    }

    private void Fill()
    {
        headerText.text = parameterName;
        UpdateValue();
    }

    private void UpdateValue()
    {
        valueText.text = construction.GetParam(parameterName).ToString();
    }

    public void MinButton()
    {

    }

    public void MaxButton()
    {

    }

    public void MinusAction()
    {
        if (construction.GetParam(parameterName) > 0)
        {
            resources.Add(resourceStep);
            construction.SetParam(parameterName, construction.GetParam(parameterName) - 1);
            UpdateValue();
        }
    }

    public void PlusAction()
    {
        if (resources.IsEnough(resourceStep) && construction.GetParam(parameterName) < max)
        {
            resources.Spend(resourceStep);
            construction.SetParam(parameterName, construction.GetParam(parameterName) + 1);
            UpdateValue();
        }
    }
}
