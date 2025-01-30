using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CannonController : MonoBehaviour
{
    public GameObject canonBallPrefab;
    public Transform attach;

    private bool canShoot = true;
    private float timeBetweenShoots = 0.3f;
    private float count = 0;
    private bool playerInArea = false;
    private List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

    void Update()
    {
        if (playerInArea)
        {
            InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, devices);
            if(devices.Count != 0)
                devices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out float trigger);

            if ((/*trigger > 0.1f || */Input.GetKeyDown(KeyCode.Space)) && canShoot)
            {
                NewCanonBall();
                canShoot = false;
            }
        }
        if (!canShoot)
        {
            count += Time.deltaTime;
        }

        if(count >= timeBetweenShoots)
        {
            canShoot = true;
            count = 0;
        }
    }

    void NewCanonBall()
    {
        GameObject cannonBall = Instantiate(canonBallPrefab, attach.position, attach.parent.rotation);
        float force = Random.Range(2000f, 6600f);
        cannonBall.GetComponent<Rigidbody>().velocity = attach.forward * force * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }
}
