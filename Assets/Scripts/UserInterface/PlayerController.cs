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
            gameMenuPanel.SetActive(!gameMenuPanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            JGameData jgd = new JGameData();
            jgd.version = "1";
            jgd.constructions = constructionController.GetSerialized();
            jgd.worldObjects = worldObjectController.GetSerialized();
            jgd.tiles = tileController.GetSerialized();
            string data = JsonUtility.ToJson(jgd);
            Debug.Log(data);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            JGameData jgd = new JGameData();
            jgd.version = "1";
            jgd.constructions = constructionController.GetSerialized();
            jgd.worldObjects = worldObjectController.GetSerialized();
            jgd.tiles = tileController.GetSerialized();
            string data = JsonUtility.ToJson(jgd);
            Debug.Log(data);
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
