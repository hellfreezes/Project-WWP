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
        previewObject.transform.SetParent(GameManager.Instance.constructionController.Handler);
        previewObject.SetActive(false);
    }

    private void RegisterEventSubscribers()
    {
        GameManager.Instance.constructionController.ConstructionPlaced += OnConstructionPlaced;
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
                    SelectMode();
                    break;
                case MouseMode.EDITOR_OBJECTS:
                    ObjectsMode();
                    break;
                case MouseMode.EDITOR_TILES:
                    TilesMode();       
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
                int x = GameManager.Instance.constructionController.GetPattern(buildName).Width;
                int y = GameManager.Instance.constructionController.GetPattern(buildName).Heigth;
                previewObject.transform.localPosition = new Vector3(t.X + ((x - 1) / 2), t.Y + ((y - 1) / 2), 0);
                if (GameManager.Instance.constructionController.GetPattern(buildName).IsBuildTileVaild(t))
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
                int x = GameManager.Instance.worldObjectController.GetPattern(buildName).Width;
                int y = GameManager.Instance.worldObjectController.GetPattern(buildName).Heigth;
                previewObject.transform.localPosition = new Vector3(t.X + ((x - 1) / 2), t.Y + ((y - 1) / 2), 0);
            }
        }
        else if (mouseMode == MouseMode.EDITOR_TILES)
        {
            Tile t = GetTileAtMouse();
            if (t != null)
            {
                previewObject.transform.localPosition = new Vector3(t.X , t.Y, 0);
            }
        }
    }

    private void BuildMode()
    {
        Tile tile = GetTileAtMouse();
        Vector2 position = GetVectorAtMouse();
        if (tile != null)
        {
            GameManager.Instance.constructionController.Place(buildName , position);
        }
    }

    private void ObjectsMode()
    {
        Tile tile = GetTileAtMouse();
        Vector2 position = GetVectorAtMouse();
        if (tile != null)
        {
            GameManager.Instance.worldObjectController.Place(buildName, position);
        }
    }

    private void TilesMode()
    {
        Tile tile = GetTileAtMouse();
        Vector2 position = GetVectorAtMouse();
        if (tile != null)
        {
            tile.SwitchTile(GameManager.Instance.tileController.GetPattern(buildName));
        }
    }

    private void SelectMode()
    {
        Tile tile = GetTileAtMouse();
        if (tile != null)
        {
            Unit unit = tile.GetUnit();
            if (unit != null)
            {
                unit.MouseClick();
            }
        }
    }

    public void SetBuildingMode(string name)
    {
        if (ResourcesController.Instance.MainResources.IsEnough(GameManager.Instance.constructionController.GetPattern(name).Resources))
        {
            buildName = name;
            mouseMode = MouseMode.BUILD;
            previewObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.constructionController.GetPattern(buildName).ObjectSprite;
            previewObject.SetActive(true);
        } else
        {
            Resources notEnough = ResourcesController.Instance.MainResources.NotEnough(GameManager.Instance.constructionController.GetPattern(name).Resources);
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
        previewObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.worldObjectController.GetPattern(buildName).ObjectSprite;
        previewObject.SetActive(true);
    }

    public void SetEditorTileMode(string name)
    {
        buildName = name;
        mouseMode = MouseMode.EDITOR_TILES;
        previewObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.tileController.GetPattern(buildName).ObjectSprite;
        previewObject.SetActive(true);
    }

    private void OnConstructionPlaced(object source, EventArgs args)
    {
        SetSelecetMode();
    }

    public void SetSelecetMode()
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

    public Vector2 GetVectorAtMouse()
    {
        Vector2 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        result = new Vector2(Mathf.FloorToInt(result.x + 0.5f), Mathf.FloorToInt(result.y + 0.5f));
        return result;
    }
}
