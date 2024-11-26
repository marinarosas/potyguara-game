using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    public GameObject canonBallPrefab;
    public Transform attach;
    private float triggerL;
    private float triggerR;

    private bool canShoot = true;
    private float timeBetweenShoots = 0.3f;
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
        float force = Random.Range(2500f, 7000f);
        cannonBall.GetComponent<Rigidbody>().velocity = attach.forward * force * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggerL = FindFirstObjectByType<PotyPlayerController>().GetTriggerLeftButton();
            triggerR = FindFirstObjectByType<PotyPlayerController>().GetTriggerRightButton();
            if ((triggerL > 0.1f || triggerR > 0.1f || Input.GetKeyDown(KeyCode.Space)) && canShoot)
            {
                NewCanonBall();
                canShoot = false;
            }
        }
    }
}
