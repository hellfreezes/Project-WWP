using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightMenu : MonoBehaviour {

    [SerializeField]
    GameObject buildButton;
    [SerializeField]
    Transform worldObjectsHandler;

    // Use this for initialization
    void Start()
    {
        WorldObjectController woController = FindObjectOfType<WorldObjectController>();

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
