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

    private MouseMode mouseMode = MouseMode.SELECT;

    // Use this for initialization
    void Start () {

	}

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float speed = cameraDragSpeed * Time.deltaTime;
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed, 0);
        }
    }
}
