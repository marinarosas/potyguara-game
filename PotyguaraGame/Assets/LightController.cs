using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject diretionalLight;
    private GameObject[] lights;
    // Start is called before the first frame update
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("light");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = diretionalLight.transform.eulerAngles;
        if(rotation.x >= 177 && rotation.x < 350)
        {
            foreach (var light in lights)
            {
                light.GetComponent<Light>().enabled = false;
            }
        }
        else
        {
            foreach (var light in lights)
            {
                light.GetComponent<Light>().enabled = true;
            }
        }
    }
}
