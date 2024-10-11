using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform canvas;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public void SetCanvas(Transform canvas)
    {
        this.canvas = canvas;
    }

    // Update is called once per frame
    void Update()
    {
        if(canvas!=null)
            canvas.transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));
    }
}
