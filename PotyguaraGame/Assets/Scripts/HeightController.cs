using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeightController : MonoBehaviour
{
    [SerializeField] private float height = 0f;
    private GameObject player;
    private bool insideLift = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        transform.position = GameObject.Find("InitialPosition").transform.position;
        height = player.transform.position.y;
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
            FixedHeight(player);
        }
        else
        {
            VariableHeight(player);
        }
    }

    public void SetBool(bool value)
    {
        insideLift = value;
    }

    public bool GetBool()
    {
        return insideLift;
    }
    public void FixedHeight(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, height, obj.transform.position.z);
    }

    public void VariableHeight(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
    }
}
