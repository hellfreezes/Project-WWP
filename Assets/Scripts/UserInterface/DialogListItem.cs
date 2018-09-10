﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogListItem : MonoBehaviour, IPointerClickHandler {

    public InputField inputField { get; set; }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        Text t = GetComponentInChildren<Text>();
        inputField.text = t.text;
    }
}
