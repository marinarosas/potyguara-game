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
    // Update is called once per frame

    private void Start()
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

        lightGeneral.rotation = Quaternion.Euler(sunAngle - 80f, 170f, 0f);

        /*if (currentTime.Hour >= 5)
        {
            if (currentTime.Hour <= 12 && currentTime.Minute <= 59)
            {
                float smoothRotation = Mathf.SmoothDampAngle(lightGeneral.eulerAngles.x, 90f, ref currentRotation, smoothTime); //- 15
                lightGeneral.rotation = Quaternion.Euler(smoothRotation, 0f, 0f);
            }
        }
        if (currentTime.Hour >= 13)
        {
            if(currentTime.Hour <= 17 && currentTime.Minute <= 59)
            {
                float smoothRotation = Mathf.SmoothDampAngle(lightGeneral.eulerAngles.x, 190f, ref currentRotation, smoothTime);
                lightGeneral.rotation = Quaternion.Euler(smoothRotation, 0f, 0f);
            }
        }
        if (currentTime.Hour >= 18)
        {
            if (currentTime.Hour <= 23 && currentTime.Minute <= 59)
            {
                float smoothRotation = Mathf.SmoothDampAngle(lightGeneral.eulerAngles.x, 330f, ref currentRotation, smoothTime);
                lightGeneral.rotation = Quaternion.Euler(smoothRotation, 0f, 0f);
            }
        }*/
    }
}
