using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveFileController : DialogBoxController {

    [SerializeField]
    GameObject saveFileInputField;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public override void ShowDialog()
    {
        base.ShowDialog();

        // Построить список файлов

    }

    public void SaveOkayWasClicked()
    {
        string fileName = saveFileInputField.GetComponent<InputField>().text;
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName + "sav");

        if (File.Exists(filePath) == true)
        {
            //Перезапись
            return;
        }

        // Записать
    }
}
