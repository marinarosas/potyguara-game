using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VisualEffect>().Stop();
    }

}
