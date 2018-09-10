using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionArgs : EventArgs
{
    public Construction CurrentConstruction { get; set; }
}

public class ConstructionController : MonoBehaviour {
    [SerializeField]
    private Sprite[] buildingSprites;
    [SerializeField]
    private GameObject constructionParent;


    private Dictionary<string, Construction> constructionPatterns;

    private List<Construction> constructions;

    private ConstructionFunctions constructionFunctions;

    public EventHandler<ConstructionArgs> ConstructionPlaced;

    // Use this for initialization
    void Start()
    {
        constructionFunctions = new ConstructionFunctions();
        constructionPatterns = new Dictionary<string, Construction>();
        constructions = new List<Construction>();
        CreateConstructionsList();
    }

    private void CreateConstructionsList()
    {
        string path = Application.streamingAssetsPath + "/JSON/constructions.json";
        ConstructionListJSONHelper constructionsList = new ConstructionListJSONHelper();
        using (StreamReader stream = new StreamReader(path))
        {
            string json = stream.ReadToEnd();
            constructionsList = JsonUtility.FromJson<ConstructionListJSONHelper>(json);
        }
       
        foreach(ConstructionJSONHelper cJSON in constructionsList.Constructions)
        {
            Construction b = LoadConstructionFromJSONHelper(cJSON);
            constructionPatterns.Add(b.ObjectName, b);
        }
    }

    private Construction LoadConstructionFromJSONHelper(ConstructionJSONHelper constructionJSON)
    {
        Resources res = new Resources();
        foreach(IntegerJSONHelper r in constructionJSON.Resource)
        {
            res.SetResource((ResourceName)Enum.Parse(typeof(ResourceName), r.Name), r.Value);
        }
        Construction c = new Construction(constructionJSON.Name, 
            SpriteManager.current.GetSprite("Construction", constructionJSON.SpriteName), 
            new Vector2(constructionJSON.TileSizeWidth, constructionJSON.TileSizeHeigth), 
            res);
        foreach(FloatJSONHelper param in constructionJSON.Param)
        {
            c.SetParam(param.Name, param.Value);
        }
        foreach(string action in constructionJSON.OnUpdateAction)
        {
            c.RegisterOnUpdate(constructionFunctions.GetAction(action));
        }

        return c;
    }

    public void PlaceBuilding(string name)
    {
        Tile t = MouseController.Instance.GetTileAtMouse();
        
        if (constructionPatterns[name].IsBuildTileVaild(t))
        {
            Construction c = constructionPatterns[name].Clone(t);

            GameObject go = new GameObject();
            go.name = name;
            go.transform.SetParent(constructionParent.transform);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = c.ObjectSprite;
            sr.sortingLayerName = "Construction";
            c.SetGameObject(go);

            constructions.Add(c);

            OnConstructionPlaced(c);
        } else
        {
            //Debug.Log("Место занято");
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Construction construction in constructions)
        {
            construction.Update(GameManager.Instance.GameSpeed);
        }
    }

    public Construction GetPattern(string name)
    {
        if (!constructionPatterns.ContainsKey(name))
        {
            Debug.LogError("В базе данных прототипов отсутвует " + name + " здание");
            return null;
        }

        return constructionPatterns[name];
    }

    public Transform GetConstructionParent()
    {
        return constructionParent.transform;
    }

    protected virtual void OnConstructionPlaced(Construction c)
    {
        if (ConstructionPlaced != null)
        {
            ConstructionPlaced(this, new ConstructionArgs() { CurrentConstruction = c });
        }
    }
}
