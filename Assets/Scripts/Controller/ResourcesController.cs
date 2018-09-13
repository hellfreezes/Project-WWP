using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesController : MonoBehaviour {

    [SerializeField]
    private ConstructionController constructionController;
    [SerializeField]
    private List<ResourcesHelper> startResources;
    [SerializeField]
    private Transform resourcePanel;
    [SerializeField]
    private GameObject resourceIndicatorPrefab;

    private static ResourcesController instance;
    private Dictionary<ResourceName, Text> resourceInricator;


    private Resources resources;

    public static ResourcesController Instance
    {
        get { return instance; }
    }

    public Resources MainResources
    {
        get
        {
            return resources;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Дублирование ResourcesController");
        }
        instance = this;
    }

    // Use this for initialization
    void Start () {
        resourceInricator = new Dictionary<ResourceName, Text>();

        ResourceName[] ress = Enum.GetValues(typeof(ResourceName)).Cast<ResourceName>().ToArray();
        foreach(ResourceName name in ress)
        {
            GameObject go = Instantiate(resourceIndicatorPrefab);
            go.transform.SetParent(resourcePanel);
            Image img = go.transform.Find("Image").GetComponent<Image>();
            img.sprite = SpriteManager.current.GetSprite("Resource", name.ToString());
            go.name = "Resource - " + name.ToString();
            resourceInricator.Add(name, go.transform.Find("Text").GetComponent<Text>());
        }

        resources = new Resources();
        resources.ResourcesChanged += OnResourcesChanged;
        constructionController.ConstructionPlaced += OnConstructionPlaced;

        foreach (ResourcesHelper r in startResources)
        {
            Resources startResource = new Resources(r.name, r.value);
            resources.Add(startResource);
        }
	}

    public void AddResource(Resources r)
    {
        resources.Add(r);
    }

    public void OnResourcesChanged(object source, ResArgs args)
    {
        foreach (ResourceName name in args.Names)
        {
            Resources r = (Resources)source;
            resourceInricator[name].text = r.GetResourceValue(name).ToString();
        }
    }

    public void OnConstructionPlaced(object source, ConstructionArgs args)
    {
        resources.Spend(args.CurrentConstruction.Resources);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
