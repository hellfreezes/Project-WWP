using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBoxController : DialogBoxController {
    [SerializeField]
    GameObject rightMenu;

    private void Start()
    {
        GameManager.Instance.constructionController.ConstructionPlaced += OnConstructionPlaced;
    }

    public void OnConstructionPlaced(object source, ConstructionArgs args)
    {
        Debug.Log("Подписываюсь на " + args.CurrentConstruction.ObjectName);
        args.CurrentConstruction.Clicked += OnConstructionClicked;
    }

    private void OnConstructionClicked(object source, EventArgs args)
    {
        Construction c = (Construction)source;
        ShowDialog();
    }

    public override void ShowDialog()
    {
        rightMenu.SetActive(true);
    }

    public override void CloseDialog()
    {
        rightMenu.SetActive(false);
    }
}
