using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WaypointsFree;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;

public class SpawnerController : MonoBehaviour
{
    [Header("Normal Mode")]
    public GameObject prefabNavio;
    public WaypointsGroup waypointsGroup;
    public GameObject cannons;

    [Header("Zombie Mode")]
    public GameObject prefabZumbi;
    public Transform destinyLevel1;
    public Transform destinyLevel2;
    public Transform slot;
    private List<Transform> spawnRandowZombie = new List<Transform>();

    [Header("General")]
    private bool levelIsRunning = false;
    private GameObject player;
    private GameObject finishUI;
    private int currentAmount;
    private int currentLevel = 1;
    private int wallsDestroyed = 0;
    public void SetWallsDestroyed()
    {
        wallsDestroyed++;
    }
    private void Start()
    {
        FindFirstObjectByType<GameForteController>().SetCurrentLevel(currentLevel);
        player = GameObject.FindWithTag("Player");
        finishUI = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
        for (var ii = 0; ii < destinyLevel1.childCount; ii++)
        {
            spawnRandowZombie.Add(destinyLevel1.GetChild(ii));
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetLevelIsRunning(bool value)
    {
        levelIsRunning = value;
    }
    public void SetDestinyRandow(int value)
    {
        spawnRandowZombie.Clear();
        if (value == 2)
        {
            for (var ii = 0; ii < destinyLevel2.childCount; ii++)
            {
                spawnRandowZombie.Add(destinyLevel2.GetChild(ii));
            }
        }
        else
        {
            for (var ii = 0; ii < destinyLevel1.childCount; ii++)
            {
                spawnRandowZombie.Add(destinyLevel1.GetChild(ii));
            }
        }
    }
    public void SetLevel()
    {
        finishUI.SetActive(false);
        if (FindFirstObjectByType<GameForteController>().GetMode() == 1)
        {
            cannons.SetActive(true);
            FindFirstObjectByType<TechGuaraController>().CreateReport("Defenda o Forte dos Invasores Maritimos!!!", "Olá jogador(a), para esse nível você deve destruir a frota de navios invasores utilizando os canhões. Se aproxime deles e pressioner o gatilho para atirar!!!");
            FindFirstObjectByType<HeightController>().NewHeight(19.6f);
            NextLevel(90f, new Vector3(654.91f, 18.6f, 400.95f));
        }
        else
        {
            if (currentLevel == 1)
            {
                SetDestinyRandow(1);
                FindFirstObjectByType<TechGuaraController>().CreateReport("Proteja a Entrada do Forte!!!", "Olá jogador(a), para esse nível você deve evitar que os zumbis destruam as barreiras que o/a protegem. Se eles deixarem todas vermelhas, você perde!!!");
                NextLevel(90f, new Vector3(746.14f, 9.3f, 400.35f));
            }
            if (currentLevel == 2)
            {
                FindFirstObjectByType<GameForteController>().handMenuLevel2.SetActive(true);
                FindFirstObjectByType<TechGuaraController>().CreateReport("Zumbis a Vista!!!", "Olá jogador(a), para esse nível você não deve deixar que os zumbis cheguem até você. Se eles se aproximarem demais, você morre!!!");
                SetDestinyRandow(2);
                FindFirstObjectByType<GameForteController>().ResetCount();
                FindFirstObjectByType<HeightController>().NewHeight(18.6f);
                UpdateLevelBar();
                NextLevel(90f, new Vector3(654.91f, 18.6f, 400.95f));
            }
        }
    }

    public void SetSpawn()
    {
        levelIsRunning = true;
        if(FindFirstObjectByType<GameForteController>().GetMode() == 0)
        {
            if (currentLevel == 1)
            {
                currentAmount = 12;
                InitSpawner(spawnRandowZombie);
            }
            if (currentLevel == 2)
            {
                currentAmount = 9;
                InitSpawner(spawnRandowZombie);
            }
        }
        else
        {
            currentAmount = 4;
            InitSpawner(waypointsGroup.waypoints);
        }
    }

    public void NextLevel(float angulationY, Vector3 initialPosition)
    {
        player.transform.position = initialPosition;
        player.transform.eulerAngles = new Vector3(0, angulationY, 0);
    }
   
    private void UpdateLevelBar()
    {
        GameObject.FindGameObjectWithTag("Level").GetComponent<Image>().fillAmount = 0.5f * currentLevel;
        GameObject.FindGameObjectWithTag("Level").transform.GetChild(3).GetComponent<Text>().text = currentLevel+"";
    }

    public void CleanSlot()
    {
        foreach(Transform enemy in slot)
        {
            Destroy(enemy.gameObject);
        }
    }

    private void Update()
    {
        if (FindFirstObjectByType<GameForteController>().GetMode() == 1)
        {
            if (levelIsRunning)
            {
                Transform timer = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(2);
                timer.gameObject.SetActive(true);
                if (timer.GetChild(0).GetComponent<Text>().text == "0")
                {
                    foreach(Transform enemy in slot)
                    {
                        Destroy(enemy.gameObject);
                    }
                    timer.gameObject.SetActive(false);
                    timer.GetComponent<Image>().fillAmount = 1f;
                    levelIsRunning = false;
                    finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parabéns!!!";
                    finishUI.transform.GetChild(3).gameObject.SetActive(false);
                    finishUI.transform.GetChild(6).gameObject.SetActive(true);
                    finishUI.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().ResetGame);

                    finishUI.transform.GetChild(5).GetComponent<Text>().text = FindFirstObjectByType<GameForteController>().GetCurrrentScore() + "";
                    FindFirstObjectByType<GameForteController>().SetTotalPoints();
                    finishUI.SetActive(true);
                    finishUI.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    if(slot.childCount == 0)
                        FindFirstObjectByType<SpawnerController>().SetSpawn();
                }
            }
        }
        else
        {
            if (wallsDestroyed >= 20 && currentLevel == 1)
            {
                wallsDestroyed = 0;
                levelIsRunning = false;
                FindFirstObjectByType<GameForteController>().GameOver();
                return;
            }
            if (slot.childCount == 0 && levelIsRunning)
            {
                levelIsRunning = false;
                if (currentLevel == 1)
                    //FindFirstObjectByType<GameForteController>().ChangeStateWalls(false);

                finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parabéns!!!";

                if (currentLevel == 2)
                {
                    // ver ranking
                    /*finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Ver Ranking";
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Ranking);*/

                    finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Menu Principal";
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                    currentLevel = 1;
  
                    LeftHandController leftHand = FindFirstObjectByType<LeftHandController>();
                    RightHandController rightHand = FindFirstObjectByType<RightHandController>();
                    if (leftHand.GetHand())
                    {
                        leftHand.ResetHand();
                    }
                    if (rightHand.GetHand())
                    {
                        leftHand.ResetHand();
                    }
                    FindFirstObjectByType<GameForteController>().SetCurrentLevel(currentLevel);
                    GameObject.FindGameObjectWithTag("Level").transform.GetChild(3).GetComponent<Text>().text = currentLevel + "";
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().ResetGame);
                }
                else
                {
                    finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Proximo Nivel";
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().NextLevel);
                }

                if (currentLevel < 2)
                {
                    currentLevel++;
                    FindFirstObjectByType<GameForteController>().SetCurrentLevel(currentLevel);
                }
                finishUI.transform.GetChild(5).GetComponent<Text>().text = FindFirstObjectByType<GameForteController>().GetCurrrentScore() + "";
                FindFirstObjectByType<GameForteController>().SetTotalPoints();
                finishUI.SetActive(true);
                finishUI.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void Ranking()
    {
        //finishUI.transform.GetChild(5).GetComponent<Text>().text = FindObjectOfType<GameController>().GetTotalPoints() + "";

        GameObject ranking = GameObject.FindGameObjectWithTag("Ranking");
        for(int ii = 0; ii < ranking.transform.childCount; ii++)
        {
            ranking.transform.GetChild(ii).gameObject.SetActive(true);
        }
        FindFirstObjectByType<NetworkManager>().SendPontuacionForte(FindFirstObjectByType<GameForteController>().GetTotalPoints());
        Invoke("ShowRanking", 0.7f);
    }

    private void ShowRanking()
    {
        FindFirstObjectByType<RankingController>().ShowRanking();
    }

    private void InitSpawner(List<Waypoint> waypoints)
    {
        for (int ii = 0; ii < currentAmount; ii++)
        {
            int numInt = Random.Range(0, waypoints.Count - 1);
            GameObject navio = Instantiate(prefabNavio, waypoints[numInt].position, Quaternion.identity, slot);
            if(ii==0)
                navio.GetComponent<WaypointsTraveler>().StartIndex = ii;
            else
                navio.GetComponent<WaypointsTraveler>().StartIndex = ii+1 > waypoints.Count-1 ? waypoints.Count-1 : ii+1;
        }
    }

    private void InitSpawner(List<Transform> points) {
        for (int ii = 0; ii < currentAmount; ii++)
        {
            int numInt = Random.Range(0, points.Count-1);
            Instantiate(prefabZumbi, points[numInt].position, Quaternion.identity, slot);
        }
    }
}
