using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour
{
    private float height = 0f;
    public GameObject obj;
    private bool insideLift = false;
    // Start is called before the first frame update
    void Start()
    {
        height = obj.transform.position.y;
    }

    public void NewHeight(float value)
    {
        height = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (!insideLift)
        {
            FixedHeight(obj);
        }
        else
        {
            VariableHeight(obj);
        }
    }

    public void SetBool(bool value)
    {
        insideLift = value;
    }

    public void FixedHeight(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, height, obj.transform.position.z);
    }

    public void VariableHeight(GameObject obj)
    {
        if(obj.transform.parent.name == "Height")
        {
            obj.transform.position = Vector3.zero;
        }
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }
}
