using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour {

    [SerializeField]
    private float counter;
    private float lifeTime = 5f;

    private Text text;

    public bool Free { get; set; }

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        Free = true;
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            Free = true;
            gameObject.SetActive(false);
        }
	}

    public void SetLifeTime(float time)
    {
        lifeTime = time;
    }

    public void SetMessage(string message)
    {
        counter = lifeTime;
        text.text = message;
        Free = false;
        gameObject.SetActive(true);
    }

    public string GetMessage()
    {
        return text.text;
    }
}
