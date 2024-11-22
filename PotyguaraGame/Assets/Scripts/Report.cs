using TMPro;
using UnityEngine;

public class Report : MonoBehaviour
{
    public string title;
    public string message;

    public Report(string title, string message)
    {
        this.title = title;
        this.message = message;
    }
}
