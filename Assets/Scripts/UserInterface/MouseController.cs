using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MouseMode
{
    SELECT,
    BUILD,
    EDITOR_OBJECTS,
    EDITOR_TILES
}

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private int cameraDragSpeed = 50;
    [SerializeField]
    private ConstructionController constructionController;
    [SerializeField]
    private WorldObjectController woController;
    [SerializeField]
    float cameraMinX = 7.3f;
    [SerializeField]
    float cameraMaxX = 100f;
    [SerializeField]
    float cameraMinY = 3.1f;
    [SerializeField]
    float cameraMaxY = 100f;

    private MouseMode mouseMode = MouseMode.SELECT;

    private static MouseController instance;

    private GameObject previewObject;
    private SpriteRenderer previewObjectSprite;

    private string buildName = "";


    private Color COLOR_GREEN = new Color(0, 1, 0, 0.5f);
    private Color COLOR_RED = new Color(1, 0, 0, 0.5f);


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

        CreatePreviewObject();
        RegisterEventSubscribers();
        TrimPosition();
    }

    private void CreatePreviewObject()
    {
        previewObject = new GameObject();
        previewObjectSprite = previewObject.AddComponent<SpriteRenderer>();
        previewObjectSprite.sortingLayerName = "Preview";
        previewObjectSprite.color = COLOR_GREEN;
        previewObject.name = "MousePreviewObject";
        previewObject.transform.SetParent(constructionController.GetConstructionParent());
        previewObject.SetActive(false);
    }

    private void RegisterEventSubscribers()
    {
        constructionController.ConstructionPlaced += OnConstructionPlaced;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float speed = cameraDragSpeed * Time.deltaTime;
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed, 0);
            TrimPosition();
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
                case MouseMode.EDITOR_OBJECTS:
                    ObjectsMode();
                    break;
            }
        }

        UpdatePreviewObject();
    }

    /// <summary>
    /// TODO: переименовать
    /// </summary>
    private void TrimPosition()
    {
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;

        x = Mathf.Clamp(x, cameraMinX, cameraMaxX);
        y = Mathf.Clamp(y, cameraMinY, cameraMaxY);

        Camera.main.transform.position = new Vector3(x, y, -10);
    }

    private void UpdatePreviewObject()
    {
        if (mouseMode == MouseMode.BUILD)
        {
            Tile t = GetTileAtMouse();
            if (t != null)
            {
                Vector2 tileSize = constructionController.GetPattern(buildName).TileSize;
                previewObject.transform.localPosition = new Vector3(t.X + ((tileSize.x - 1) / 2), t.Y + ((tileSize.y - 1) / 2), 0);
                if (constructionController.GetPattern(buildName).IsBuildTileVaild(t))
                {
                    previewObjectSprite.color = COLOR_GREEN;
                }
                else
                {
                    previewObjectSprite.color = COLOR_RED;
                }
            }
        }
        else if (mouseMode == MouseMode.EDITOR_OBJECTS)
        {
            Tile t = GetTileAtMouse();
            if (t != null)
            {
                Vector2 tileSize = woController.GetPattern(buildName).TileSize;
                previewObject.transform.localPosition = new Vector3(t.X + ((tileSize.x - 1) / 2), t.Y + ((tileSize.y - 1) / 2), 0);
            }
        }
    }

    private void BuildMode()
    {
        Tile tile = GetTileAtMouse();
        if (tile != null)
        {
            constructionController.PlaceBuilding(buildName , tile);
        }
    }

    private void ObjectsMode()
    {
        Tile tile = GetTileAtMouse();
        if (tile != null)
        {
            woController.PlaceObject(buildName, tile);
        }
    }

    public void SetBuildingMode(string name)
    {
        if (ResourcesController.Instance.MainResources.IsEnough(constructionController.GetPattern(name).Resources))
        {
            buildName = name;
            mouseMode = MouseMode.BUILD;
            previewObject.GetComponent<SpriteRenderer>().sprite = constructionController.GetPattern(buildName).ObjectSprite;
            previewObject.SetActive(true);
        } else
        {
            Resources notEnough = ResourcesController.Instance.MainResources.NotEnough(constructionController.GetPattern(name).Resources);
            for (int i = 0; i < notEnough.Keys().Length; i++)
            {
                PopupMessageController.Instance.ShowMessage("Нехватает " + notEnough.Keys()[i].ToString());
            }
        }
    }

    public void SetEditorWorldObjectMode(string name)
    {
        buildName = name;
        mouseMode = MouseMode.EDITOR_OBJECTS;
        previewObject.GetComponent<SpriteRenderer>().sprite = woController.GetPattern(buildName).ObjectSprite;
        previewObject.SetActive(true);
    }

    private void OnConstructionPlaced(object source, EventArgs args)
    {
        SetSelecetMode();
    }

    private void SetSelecetMode()
    {
        previewObject.SetActive(false);
        mouseMode = MouseMode.SELECT;
        buildName = "";
    }

    public Tile GetTileAtMouse()
    {
        Vector3 coords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return World.Instance.GetTileAt(Mathf.FloorToInt(coords.x + 0.5f), Mathf.FloorToInt(coords.y + 0.5f));
    }
}
