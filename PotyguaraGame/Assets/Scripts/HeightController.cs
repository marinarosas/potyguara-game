using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour
{
    private float height = 0f;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        height = obj.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        FixedHeight(obj);
    }

    public void FixedHeight(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, height, obj.transform.position.z);
    }
}
