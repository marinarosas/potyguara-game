using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    private int maxBullets = 10;
    private int currentBullets;
    private InputDevice targetDevice;
    private bool isLeft = false;
    private bool isRight = false;
    private float timeout = 0.5f;

    public TextMeshProUGUI bullets;

    [SerializeField] float nextTimeForFire = 0f; // tempo para o proximo tiro
    [SerializeField] private float delayBetweenShots = 0.3f;  // Delay entre cada disparo em segundos
    [SerializeField] private bool canShoot = true; // booleano para saber se pode ou não atirar

    [Header("Prefab Refrences")]
    public GameObject line;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 700f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 250f;


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        Reload();
    }

    public void setLeftHand()
    {
        isLeft = true;
    }

    public void setRightHand()
    {
        isRight = true;
    }

    void Update()
    {
        // controle do tempo entre disparos
        if (!canShoot)
            nextTimeForFire += Time.deltaTime;

        if (nextTimeForFire >= delayBetweenShots)
        {
            canShoot = true;
            nextTimeForFire = 0f;
        }

        if (isLeft)
        {
           //targetDevice = FindObjectOfType<LeftHandController>().GetTargetDevice();
        }
        else if (isRight)
        {
            //targetDevice = FindObjectOfType<RightHandController>().GetTargetDevice();
        }
        if (isRight != false || isLeft != false)
        {
            //targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            if (/*triggerValue > 0.1f ||*/ Input.GetKeyDown(KeyCode.F))
            {
                if (currentBullets > 0)
                {
                    if (canShoot)
                    {
                        //Calls animation on the gun that has the relevant animation events that will fire
                        if (isRight)
                        {
                            FindFirstObjectByType<RightHandController>().AnimationFinger();
                        }
                        else if (isLeft)
                        {
                            FindFirstObjectByType<LeftHandController>().AnimationFinger();
                        }
                        gunAnimator.SetTrigger("Fire");
                        canShoot = false;
                    }
                }
                else
                {
                    Reload();
                }
                if (Vector3.Angle(transform.up, Vector3.up) > 100 && currentBullets < 10){
                    Reload();
                }
            }
        }
        bullets.text = currentBullets.ToString();
    }

    public void Reload()
    {
        currentBullets = maxBullets;
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        currentBullets--;
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }
        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hitInfo, 100);
        if (line)
        {
            GameObject liner = Instantiate(line);
            liner.GetComponent<LineRenderer>().SetPositions(new Vector3[] {barrelLocation.position, hasHit ? hitInfo.point : barrelLocation.position + barrelLocation.forward*100});
            Destroy(liner, 0.5f);
        }
        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
