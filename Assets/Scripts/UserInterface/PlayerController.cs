using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    GameObject gameMenuPanel;
    [SerializeField]
    GameObject saveDialogBox;
    [SerializeField]
    GameObject loadDialogBox;

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
            gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
        }
    }

    private void HideMenu()
    {
        gameMenuPanel.SetActive(false);
    }

    public void ShowSaveDialog()
    {
        HideMenu();
        saveDialogBox.SetActive(true);
    }
}
