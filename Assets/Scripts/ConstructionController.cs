using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour {
    [SerializeField]
    private Sprite[] buildingSprites;
    [SerializeField]
    private GameObject constructionParent;


    private Dictionary<string, Construction> constructionsDatabase;

    private List<Construction> constructions;

    // Use this for initialization
    void Start()
    {
        constructionsDatabase = new Dictionary<string, Construction>();
        constructions = new List<Construction>();
        CreateBuildingsList();
    }

    private void CreateBuildingsList()
    {
        Construction b = new Construction("Lumber mill", buildingSprites[0], new Vector2(2, 2));
        constructionsDatabase.Add("Lumber mill", b);
    }

    public void PlaceBuilding(string name)
    {
        Tile t = MouseController.Instance.GetTileAtMouse();
        
        if (PlaceIsCorrect(t, constructionsDatabase[name].TileSize))
        {
            Construction c = constructionsDatabase[name].Clone(t);

            GameObject go = new GameObject();
            go.transform.SetParent(constructionParent.transform);
            //go.transform.position = new Vector3(t.X, t.Y, 0);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = c.ObjectSprite;
            c.SetGameObject(go);

            constructions.Add(c);
        } else
        {
            Debug.Log("Место занято");
        }
    }

    private bool PlaceIsCorrect(Tile t, Vector2 size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!World.Instance.GetTileAt(t.X + x, t.Y + y).Free())
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Construction construction in constructions)
        {
            construction.Update(GameManager.Instance.GameSpeed);
        }
    }
}
