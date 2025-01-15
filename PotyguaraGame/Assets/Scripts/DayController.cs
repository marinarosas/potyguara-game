using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayController : MonoBehaviour
{
    private DateTime currentTime;
    private Transform sun;
    private Transform moon;
    private bool teste = true;
    private Material skyBox;
    //private float currentRotation = 0f;
    //public float smoothTime = 90f;
    // manhã 5h até 13h
    // tarde 13h até 18h
    // noite 18h até 5h

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 5 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            sun = GameObject.FindWithTag("Sun").transform;
            moon = GameObject.FindWithTag("Moon").transform;
        }
        rotationSpeed = 360f / dayLenght;
        skyBox = RenderSettings.skybox;
    }

    float dayLenght = 86400f;
    float rotationSpeed;

    public DateTime GetCurrentTime()
    {
        return currentTime;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && sun != null && moon != null)
        {
            currentTime = DateTime.Now.ToLocalTime();
            GetComponent<TextMeshProUGUI>().text = currentTime.ToString("HH:mm:ss");
            float hours = currentTime.Hour + (currentTime.Minute / 60f) + (currentTime.Second / 3600f);
            float sunAngle = (hours / 24f) * 360f;
            //lightGeneral.Rotate(Vector3.right * (sunAngle-90f) * rotationSpeed * Time.deltaTime);

            if(currentTime.Hour >= 18)
            {
                RenderSettings.sun = moon.GetComponent<Light>();
                moon.rotation = Quaternion.Euler(sunAngle - 85f, 170f, 0f);
                skyBox.SetFloat("_AtmosphereThickness", 0.2f);
            }
            if(currentTime.Hour >= 16 && currentTime.Hour < 18)
            {
                sun.GetComponent<Light>().enabled = true;
                RenderSettings.sun = sun.GetComponent<Light>();
                sun.rotation = Quaternion.Euler(sunAngle - 85f, 170f, 0f);
                skyBox.SetFloat("_AtmosphereThickness", 2);
            }
            if(currentTime.Hour >= 4 && currentTime.Hour < 16)
            {
                sun.GetComponent<Light>().enabled = true;
                RenderSettings.sun = sun.GetComponent<Light>();
                sun.rotation = Quaternion.Euler(sunAngle - 85f, 170f, 0f);
                skyBox.SetFloat("_AtmosphereThickness", 0.8f);
            }
            if(currentTime.Hour < 4)
            {
                RenderSettings.sun = moon.GetComponent<Light>();
                moon.rotation = Quaternion.Euler(sunAngle - 85f, 170f, 0f);
                skyBox.SetFloat("_AtmosphereThickness", 0.2f);
            }
        }
    }
}
