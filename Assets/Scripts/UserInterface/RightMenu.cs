using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightMenu : MonoBehaviour {
    [SerializeField]
    WorldObjectController woController;
    [SerializeField]
    GameObject buildButton;
    [SerializeField]
    Transform worldObjectsHandler;

    private void Awake()
    {
        woController.PatternsLoaded += Init;
    }

    // Use this for initialization
    void Init(object source, EventArgs args)
    {
        woController = FindObjectOfType<WorldObjectController>();

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
}
