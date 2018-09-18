using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadFileController : DialogBoxController {
    [SerializeField]
    private InputField inputLoadField;
    [SerializeField]
    private GameObject fileListItemPrefab;
    [SerializeField]
    private Transform fileList;

    private AutomaticVerticalSize fileListSizeController;

    private static LoadFileController instance;

    public static LoadFileController Instance
    {
        get { return instance; }
    }

    // Use this for initialization
    void Start() {
        instance = this;
        fileListSizeController = fileList.gameObject.GetComponent<AutomaticVerticalSize>();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void ShowDialog()
    {
        base.ShowDialog();

        // Построить список файлов
        string filePath = GameManager.Instance.FileSaveBasePath();
        string[] existingSaves = Directory.GetFiles(filePath, "*.sav");

        foreach (string file in existingSaves)
        {
            GameObject go = Instantiate(fileListItemPrefab);
            string fileName = Path.GetFileNameWithoutExtension(file);
            go.GetComponentInChildren<Text>().text = fileName;
            go.GetComponent<DialogListItem>().inputField = inputLoadField;
            go.transform.SetParent(fileList);
        }

        fileList.GetComponent<AutomaticVerticalSize>().AdjustSize();
    }

    public override void CloseDialog()
    {
        base.CloseDialog();

        while (fileList.childCount > 0)
        {
            Transform t = fileList.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
    }

    public void LoadOkayWasClicked()
    {
        string fileName = inputLoadField.text;
        string filePath = Path.Combine(GameManager.Instance.FileSaveBasePath(), fileName + ".sav");

        if (File.Exists(filePath) == false)
        {
            //Файл не обнаружен
            return;
        }

        // Загрузить
        LoadGame(filePath);

        CloseDialog();
    }

    private void LoadGame(string path)
    {
        JGameData jgd;

        using (StreamReader stream = new StreamReader(path))
        {
            string json = stream.ReadToEnd();
            jgd = JsonUtility.FromJson<JGameData>(json);
        }

        //Контроль версии
        if (jgd.version != GameManager.Instance.gameVersion)
        {
            Debug.LogError("Невозможно загрузить игру (" + GameManager.Instance.gameVersion + "). Файл сохранения (" + jgd.version + ") не совпадает с текущей версией игры.");
            return;
        }

        GameManager.Instance.tileController.LoadFromList(jgd.tiles);
        GameManager.Instance.worldObjectController.LoadFromList(jgd.worldObjects);
        GameManager.Instance.constructionController.LoadFromList(jgd.constructions);
    }
}
