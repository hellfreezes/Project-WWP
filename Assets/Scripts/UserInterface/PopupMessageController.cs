using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessageController : MonoBehaviour {

    [SerializeField]
    private GameObject popupMessageLinePrefab;
    [SerializeField]
    private int numberOfLines = 5;
    [SerializeField]
    private int lifeTime = 5;

    private PopupMessage[] lines;
    private static PopupMessageController instance;
    
    public static PopupMessageController Instance
    {
        get { return instance; }
    }

    // Use this for initialization
    void Start () {
		if (instance != null)
        {
            Debug.Log("PopupMessageController не один на сцене! Удаляю");
        }
        instance = this;
        lines = new PopupMessage[numberOfLines];

        for (int i = 0; i < numberOfLines; i++)
        {
            GameObject go = Instantiate(popupMessageLinePrefab);
            go.transform.SetParent(this.gameObject.transform);
            lines[i] = go.GetComponent<PopupMessage>();
            lines[i].SetLifeTime(lifeTime);
        }
	}

    public void ShowMessage(string message)
    {
        string tempMessage = message;
        for (int i = 0; i < numberOfLines; i++)
        {
            if (tempMessage == "")
                break;

            if (lines[i].Free)
            {
                lines[i].SetMessage(message);
                tempMessage = "";
            } else
            {
                tempMessage = lines[i].GetMessage();
                lines[i].SetMessage(message);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
