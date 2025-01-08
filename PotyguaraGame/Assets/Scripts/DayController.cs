using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DayController : MonoBehaviour
{
    private System.DateTime currentTime;
    private Transform lightGeneral;
    //private float currentRotation = 0f;
    //public float smoothTime = 90f;
    // manhã 5h até 13h
    // tarde 13h até 18h
    // noite 18h até 5h

    void Start()
    {
        lightGeneral = GameObject.FindWithTag("Sun").transform;
        rotationSpeed = 360f / dayLenght;
    }

    float dayLenght = 86400f;
    float rotationSpeed;

    public System.DateTime GetCurrentTime()
    {
        return currentTime;
    }

    void Update()
    {
        currentTime = System.DateTime.Now;
        GetComponent<TextMeshProUGUI>().text = currentTime.ToString("HH:mm:ss");
        float hours = currentTime.Hour + (currentTime.Minute / 60f) + (currentTime.Second / 3600f);
        float sunAngle = (hours / 24f) * 360f;
        //lightGeneral.Rotate(Vector3.right * (sunAngle-90f) * rotationSpeed * Time.deltaTime);

        lightGeneral.rotation = Quaternion.Euler(sunAngle - 81f, 170f, 0f);
    }
}
