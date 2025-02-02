using UnityEngine;

public class PlankDeceleration : MonoBehaviour
{
    public Rigidbody plankRigidbody;
    public float deceleration = 5f;
    public float minimumSpeed = 0.7f;
    public GameObject menu;

    private bool isInDecelerationZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == plankRigidbody.gameObject)
        {
            isInDecelerationZone = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == plankRigidbody.gameObject && isInDecelerationZone)
        {
            Vector3 currentVelocity = plankRigidbody.velocity;
            if (currentVelocity.magnitude > minimumSpeed)
            {
                Vector3 oppositeDirection = -currentVelocity.normalized;
                plankRigidbody.AddForce(oppositeDirection * deceleration, ForceMode.Acceleration);
            }
            else
            {
                plankRigidbody.velocity = Vector3.zero;
                plankRigidbody.angularVelocity = Vector3.zero;
                isInDecelerationZone = false;
                if (menu != null)
                {

                    menu.SetActive(true);
                    plankRigidbody.isKinematic = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == plankRigidbody.gameObject)
        {
            isInDecelerationZone = false;
        }
    }
}
