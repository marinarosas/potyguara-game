using UnityEngine;

public class WallController : MonoBehaviour
{
    public Material forceField;
    private bool receivedDamage = false;
    private Renderer wallRenderer;
    private Material material;

    public void setDamage(bool value)
    {
        receivedDamage = value;
    }

    private void Start()
    {
        forceField = new Material(forceField);
        wallRenderer = GetComponent<Renderer>();
        material = wallRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(receivedDamage)
            material.SetFloat("_isDamage", 1.0f);
        else
            material.SetFloat("_isDamage", 0.0f);

    }

    public void resetWall()
    {
        //gameObject.GetComponent<MeshRenderer>().material = normalWall;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!receivedDamage)
        {
            if (collision.gameObject.CompareTag("Head") || collision.gameObject.CompareTag("Body"))
            {
                setDamage(true);
                var parent = collision.gameObject.transform.parent;
                while (parent.gameObject.layer != 7)
                {
                    parent = parent.parent;
                }
                FindFirstObjectByType<SpawnerController>().SetWallsDestroyed();
            }
        }
    }
}
