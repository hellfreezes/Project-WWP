using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBoxController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    virtual public void ShowDialog()
    {
        gameObject.SetActive(true);
    }

    virtual public void CloseDialog()
    {
        gameObject.SetActive(false);
    }
}
