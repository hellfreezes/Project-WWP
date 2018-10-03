using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandBoxController : DialogBoxController {
    [SerializeField]
    GameObject rightMenu;
    [SerializeField]
    Text headerText;
    [SerializeField]
    Transform panelsField;
    [SerializeField]
    GameObject valueWindowPrefab;

    private Construction construction;

    private static CommandBoxController instance;

    List<GameObject> loadedWindows;


    public EventHandler PanelClosed;

    public static CommandBoxController Instance
    {
        get { return instance; }
    }

    private void Start()
    {
        if (instance != null)
            Debug.Log("Задвоение!");
        instance = this;

        loadedWindows = new List<GameObject>();

        GameManager.Instance.constructionController.ConstructionPlaced += OnConstructionPlaced;
    }

    public void OnConstructionPlaced(object source, ConstructionArgs args)
    {
        Debug.Log("Подписываюсь на " + args.CurrentConstruction.ObjectName);
        args.CurrentConstruction.Clicked += OnConstructionClicked;
    }

    private void OnConstructionClicked(object source, EventArgs args)
    {
        ShowDialog();

        ClearPanel();
        construction = (Construction)source;
        FillPanel(construction);
        
    }

    private void FillPanel(Construction c)
    {
        headerText.text = c.ObjectName;
        c.ControlAction(c, Time.deltaTime);
    }

    public override void ShowDialog()
    {
        rightMenu.SetActive(true);
    }

    public override void CloseDialog()
    {
        OnPanelClosed();
        rightMenu.SetActive(false);
    }

    protected virtual void OnPanelClosed()
    {
        if (PanelClosed != null)
        {
            PanelClosed(this, EventArgs.Empty);
        }
        ClearPanel();
    }

    private void ClearPanel()
    {
        foreach (GameObject go in loadedWindows)
        {
            Destroy(go);
        }
    }

    public ValueWindow AddValueWindow()
    {
        GameObject go = Instantiate(valueWindowPrefab);
        go.name = "ValueWindow";
        go.transform.SetParent(panelsField);
        loadedWindows.Add(go);
        return go.GetComponent<ValueWindow>();
    }
}
