using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public TileController tileController;
    public WorldObjectController worldObjectController;
    public ConstructionController constructionController;


    [SerializeField]
    private GameObject worldHandler;
    [SerializeField]
    private int width;
    [SerializeField]
    private int heigth;
    [SerializeField]
    private Sprite[] tiles;
    [SerializeField]
    private string saveFolder = "Saves";

    World world;

    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public GameObject WorldHandler
    {
        get { return worldHandler; }
    }

    public float GameSpeed { get; protected set; }

    // Use this for initialization
    void Start () {
        if (instance != null)
        {
            Debug.Log("На сцене не может быть больше одного GameManager");
        }
        instance = this;
        GameSpeed = Time.deltaTime;
        Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Init()
    {
        world = new World(width, heigth);
    }

    public Sprite GetSpriteTile(int index)
    {
        if (index < 0 || index > tiles.Length - 1)
        {
            index = 0;
        }

        return tiles[index];
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x + 0.5f);
        int y = Mathf.FloorToInt(coord.y + 0.5f);

        return world.GetTileAt(x, y);
    }

    public string FileSaveBasePath()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFolder);

        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }
}
