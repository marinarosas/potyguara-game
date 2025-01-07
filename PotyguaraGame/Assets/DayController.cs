using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        System.DateTime currentTime = System.DateTime.UtcNow;
        GetComponent<TextMeshProUGUI>().text = currentTime.ToString("HH:mm:ss");
    }
}
