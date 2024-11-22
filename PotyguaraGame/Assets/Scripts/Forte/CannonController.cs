using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    public GameObject canonBallPrefab;
    public Transform attach;

    private bool canShoot = true;
    private float timeBetweenShoots = 0.7f;
    private float count = 0;

    void Update()
    {
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
        float force = Random.Range(3000f, 7500f);
        cannonBall.GetComponent<Rigidbody>().linearVelocity = attach.forward * force * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PotyPlayer.Instance.inputDeviceRight.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out float triggerValueL);
            PotyPlayer.Instance.inputDeviceLeft.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out float triggerValueR);
            if ((triggerValueL > 0.1f || triggerValueR > 0.1f) && canShoot)
            {
                Invoke("NewCanonBall", 0.2f);
                canShoot = false;
            }
        }
    }
}
