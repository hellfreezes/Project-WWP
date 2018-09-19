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


    Text headerText;

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
        Fill();
    }

    private void Fill()
    {
        headerText.text = parameterName;
    }

    public void MinButton()
    {

    }

    public void MaxButton()
    {

    }

    public void MinusAction()
    {

    }

    public void PlusAction()
    {

    }
}
