using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseMode
{
    SELECT,
    BUILD
}

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private int cameraDragSpeed = 50;
    [SerializeField]
    private ConstructionController constructionController;

    private MouseMode mouseMode = MouseMode.SELECT;

    private static MouseController instance;

    private string buildName = "";

    public static MouseController Instance
    {
        get { return instance; }
    }

    // Use this for initialization
    void Start () {
        if (instance != null)
        {
            Debug.Log("На сцене не может быть больше одного MouseController");
        }
        instance = this;
	}

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float speed = cameraDragSpeed * Time.deltaTime;
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed, 0);
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            switch (mouseMode)
            {
                case MouseMode.BUILD:
                    BuildMode();
                    break;
                case MouseMode.SELECT:
                    break;
            }
        }
    }

    private void BuildMode()
    {
        Tile tile = GetTileAtMouse();
        if (tile != null)
        {
            Debug.Log("Строим на: " + tile.X + ", " + tile.Y);
            constructionController.PlaceBuilding(buildName);
        }
    }

    public void SetBuildingName(string name)
    {
        buildName = name;
        mouseMode = MouseMode.BUILD;
    }

    public Tile GetTileAtMouse()
    {
        Vector3 coords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return World.Instance.GetTileAt(Mathf.FloorToInt(coords.x+0.5f), Mathf.FloorToInt(coords.y+0.5f));
    }
}
