using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightMenu : MonoBehaviour {
    [SerializeField]
    WorldObjectController woController;
    [SerializeField]
    TileController tileController;
    [SerializeField]
    GameObject buildButton;
    [SerializeField]
    Transform worldObjectsHandler;
    [SerializeField]
    Transform tilesHanler;
    [SerializeField]
    Transform worldObjectsPanel;
    [SerializeField]
    Transform tilesPanel;

    private void Awake()
    {
        woController.PatternsLoaded += InitWorldObjects;
        tileController.PatternsLoaded += InitTiles;
    }

    // Use this for initialization
    void InitWorldObjects(object source, EventArgs args)
    {
        foreach (WorldObject wo in woController.GetAllPatterns())
        {
            GameObject go = Instantiate(buildButton);
            go.transform.SetParent(worldObjectsHandler);

            go.name = "Button - Build " + wo.ObjectName;
            Image img = go.transform.Find("Image").GetComponent<Image>();
            img.sprite = wo.ObjectSprite;

            Button b = go.GetComponent<Button>();

            b.onClick.AddListener(delegate { MouseController.Instance.SetEditorWorldObjectMode(wo.ObjectName); });
        }
    }

    void InitTiles(object source, EventArgs args)
    {
        foreach (Tile tile in tileController.GetAllPatterns())
        {
            GameObject go = Instantiate(buildButton);
            go.transform.SetParent(tilesHanler);

            go.name = "Button - Build " + tile.ObjectName;
            Image img = go.transform.Find("Image").GetComponent<Image>();
            img.sprite = tile.ObjectSprite;

            Button b = go.GetComponent<Button>();

            b.onClick.AddListener(delegate { MouseController.Instance.SetEditorTileMode(tile.ObjectName); });
        }
    }

    public void ShowObjects()
    {
        worldObjectsPanel.gameObject.SetActive(true);
        tilesPanel.gameObject.SetActive(false);
    }

    public void ShowTiles()
    {
        worldObjectsPanel.gameObject.SetActive(false);
        tilesPanel.gameObject.SetActive(true);
    }
}
