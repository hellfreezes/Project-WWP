using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    GameObject gameMenuPanel;
    [SerializeField]
    List<DialogBoxController> dialogBoxes;
    [SerializeField]
    TileController tileController;
    [SerializeField]
    WorldObjectController worldObjectController;
    [SerializeField]
    ConstructionController constructionController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        KeyInput();
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MouseController.Instance.SetSelecetMode();
            HideAll();
            gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
        }
    }

    private void HideMenu()
    {
        gameMenuPanel.SetActive(false);
    }

    public void ShowDialogBox(int index)
    {
        HideMenu();
        HideAll();
        dialogBoxes[index].ShowDialog();
    }

    private void HideAll()
    {
        foreach(DialogBoxController box in dialogBoxes)
        {
            box.gameObject.SetActive(false);
        }
    }
}
