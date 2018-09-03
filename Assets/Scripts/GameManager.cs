using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject worldHandler;
    [SerializeField]
    private int width;
    [SerializeField]
    private int heigth;
    [SerializeField]
    private Sprite[] tiles;

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

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Debug.Log("На сцене не может быть больше одного GameManager");
        }
        instance = this;

        Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Init()
    {
        world = new World(width, heigth);
    }

    public Sprite GetTile(int index)
    {
        if (index < 0 || index > tiles.Length - 1)
        {
            index = 0;
        }

        return tiles[index];
    }
}
