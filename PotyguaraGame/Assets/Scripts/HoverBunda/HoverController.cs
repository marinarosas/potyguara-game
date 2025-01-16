using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    public void StartHover()
    {
        FindFirstObjectByType<ForceController>().SetBoolean();
    }
}
