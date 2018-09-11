using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenu : MonoBehaviour {

    [SerializeField]
    GameObject buildConstructionButton;

    // Use this for initialization
    void Start()
    {
        ConstructionController constructionController = FindObjectOfType<ConstructionController>();

        foreach (Construction c in constructionController.GetAllPatterns())
        {
            GameObject go = Instantiate(buildConstructionButton);
            go.transform.SetParent(this.transform);

            //string objectName = World.current.furniturePrototypes[s].Name;

            go.name = "Button - Build " + c.ObjectName;
            Image img = go.transform.Find("Image").GetComponent<Image>();
            img.sprite = c.ObjectSprite;

            Button b = go.GetComponent<Button>();

            b.onClick.AddListener(delegate { MouseController.Instance.SetBuildingMode(c.ObjectName); });
        }
    }
}
