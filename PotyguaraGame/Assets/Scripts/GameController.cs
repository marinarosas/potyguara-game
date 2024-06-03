using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    // geral
    private bool startHoverBunda = false;
    private bool startForteDosReis = false;
    private bool startPontaNegra = false;
    private GameObject player;

    // forte dos reis
    private float count = 10;
    private GameObject timer;
    private int levelCurrent = 1;
    private bool startTimer = false;

    //Ponta Negra
    //HoverBunda

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void setHoverBunda(bool value)
    {
        startHoverBunda = value;
    }

    public void setForteDosReis(bool value)
    {
        startForteDosReis = value;
    }

    public void setPontaNegra(bool value)
    {
        startPontaNegra = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (startHoverBunda)
        {
            startHoverBunda = false;
            SceneManager.LoadScene("HoverBunda");
        }

        if (startForteDosReis)
        {
            startForteDosReis = false;
            SceneManager.LoadScene("ForteDosReisMagos");
        }

        if(startPontaNegra)
        {
            startPontaNegra = false;
            SceneManager.LoadScene("PontaNegra");
        }

        if (startTimer)
        {
            InitTimer();
        }
    }

    public void InitTimer()
    {
        // timer bar
        timer = GameObject.FindGameObjectWithTag("Time");
        if(count > 0)
        {
            count -= Time.deltaTime;
            timer.transform.GetChild(0).GetComponent<Text>().text = count.ToString("F0");
            timer.GetComponent<Image>().fillAmount -= Time.deltaTime / 9.6f;
            if(count <= 0)
            {
                count = 0;
                startTimer = false;
                FindObjectOfType<SpawnerController>().SetLevel(1);
                timer.SetActive(false);
            }
        }
    }

    public void SetStartTimer()
    {
        startTimer = true;
    }

    public void LevelOne()
    {
        player.transform.position = new Vector3(746.14f, 9.3f, 400.35f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void LevelTwo()
    {
        levelCurrent++;
        count = 10;
        UpdateLevelBar();
        player.transform.position = new Vector3(654.91f, 18.6f, 400.95f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
    }
    public void LevelThree()
    {
        levelCurrent++;
        count = 10;
        UpdateLevelBar();
        player.transform.position = new Vector3(710.36f, 8.35f, 401.15f);
        player.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void UpdateLevelBar()
    {
        GameObject.FindGameObjectWithTag("Level").GetComponent<Image>().fillAmount = 0.35f * levelCurrent;
        GameObject.FindGameObjectWithTag("Level").transform.GetChild(2).GetComponent<Text>().text = levelCurrent + "";
    }
}
