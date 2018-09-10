using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogListItem : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private InputField inputField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.text = GetComponentInChildren<Text>().text;
    }
}
