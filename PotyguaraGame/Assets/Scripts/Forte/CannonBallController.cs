using UnityEngine;
using UnityEngine.VFX;

public class CannonBallController : MonoBehaviour
{
    private VisualEffect visualEffect;
    private bool isNavio = false;
    public bool wasInstantiatedForNavio { get; set; } = false;

    void Awake()
    {
        visualEffect = transform.GetChild(0).GetComponent<VisualEffect>();
        visualEffect.Reinit();
    }

    public void SetIsNavio(bool value)
    {
        isNavio = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10 && !isNavio)
        {
            FindFirstObjectByType<GameForteController>().SetCurrentScore(1);
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            visualEffect.Play();
            Destroy(gameObject, 1f);
        }
        if(collision.gameObject.layer == 11)
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            visualEffect.Play();
            Destroy(gameObject, 1f);
        }
    }
}
