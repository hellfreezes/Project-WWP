using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveFileController : DialogBoxController {
    [SerializeField]
    private InputField inputSaveField;
    [SerializeField]
    private GameObject fileListItemPrefab;
    [SerializeField]
    private Transform fileList;

    private AutomaticVerticalSize fileListSizeController;

    private static SaveFileController instance;

    public static SaveFileController Instance
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
            go.GetComponent<DialogListItem>().inputField = inputSaveField;
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

    public void SaveOkayWasClicked()
    {
        string fileName = inputSaveField.text;
        string filePath = Path.Combine(GameManager.Instance.FileSaveBasePath(), fileName + ".sav");

        if (File.Exists(filePath) == true)
        {
            //Перезапись
            return;
        }

        // Записать
        SaveFile(filePath);
        CloseDialog();
    }

    private void SaveFile(string filePath)
    {
        JGameData jgd = new JGameData();
        jgd.version = GameManager.Instance.gameVersion;
        jgd.constructions   = GameManager.Instance.constructionController.GetSerialized();
        jgd.worldObjects    = GameManager.Instance.worldObjectController.GetSerialized();
        jgd.tiles           = GameManager.Instance.tileController.GetSerialized();
        string data = JsonUtility.ToJson(jgd);

        File.WriteAllText(filePath, data);
    }
}
