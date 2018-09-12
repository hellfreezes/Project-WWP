using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WorldObjectController : MonoBehaviour {

    [SerializeField]
    Transform worldObjectsHandler;

    private Dictionary<string, WorldObject> worldobjectsPatterns;

    private List<WorldObject> worldObjects;

    private void Start()
    {
        worldObjects = new List<WorldObject>();
        LoadWorldObjectsPatterns();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadWorldObjectsPatterns()
    {
        worldobjectsPatterns = new Dictionary<string, WorldObject>();
        

        string path = Application.streamingAssetsPath + "/JSON/worldobjects.json";
        WorldObjectListJSONHelper list = new WorldObjectListJSONHelper();
        using (StreamReader stream = new StreamReader(path))
        {
            string json = stream.ReadToEnd();
            list = JsonUtility.FromJson<WorldObjectListJSONHelper>(json);
        }

        foreach (WorldObjectJSONHelper woJSON in list.worldObjects)
        {
            WorldObject wo = LoadWorldObjectFromJSONHelper(woJSON);
            worldobjectsPatterns.Add(wo.ObjectName, wo);
        }
    }

    private WorldObject LoadWorldObjectFromJSONHelper(WorldObjectJSONHelper woJSON)
    {
        Debug.Log(woJSON.spriteName);
        Sprite sprite = SpriteManager.current.GetSprite("WorldObjects", woJSON.spriteName);
        WorldObject wo = new WorldObject(woJSON.objectName, sprite, new Vector2(woJSON.width, woJSON.heigth));
        return wo;
    }

    public WorldObject[] GetAllPatterns()
    {
        return worldobjectsPatterns.Values.ToArray();
    }

    public WorldObject GetPattern(string name)
    {
        if (!worldobjectsPatterns.ContainsKey(name))
        {
            Debug.LogError("Отсутствует WorldObject " + name);
            return null;
        }
        return worldobjectsPatterns[name];
    }

    public void PlaceObject(string name, Tile t)
    {
        if (/* тут проверка на возможность установки*/true)
        {
            WorldObject c = (WorldObject)worldobjectsPatterns[name].Clone(t);

            c.Place();
            c.ObjectHandler.transform.SetParent(worldObjectsHandler);

            worldObjects.Add(c);

            // Тут событие если оно нада
        }
        else
        {
            //Debug.Log("Место занято");
        }
    }
}
