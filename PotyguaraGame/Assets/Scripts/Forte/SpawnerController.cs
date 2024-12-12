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
        if (value == 3)
        {
            for (var ii = 0; ii < destinyLevel3.childCount; ii++)
            {
                spawnRandowZombie.Add(destinyLevel3.GetChild(ii));
            }
        }
    }
    public void SetLevel()
    {
        finishUI.SetActive(false);
        if (FindFirstObjectByType<GameForteController>().GetMode() == 1)
        {
            cannons.SetActive(true);
            FindFirstObjectByType<PotyPlayerController>().CreateReport("Defenda o Forte dos Invasores Maritimos!!!", "Ol� jogador(a), para esse n�vel voc� deve destruir a frota de navios invasores utilizando os canh�es. Se aproxime deles e pressioner o Trigger para atirar!!!");
            FindFirstObjectByType<HeightController>().NewHeight(19.6f);
            NextLevel(90f, new Vector3(654.91f, 18.6f, 400.95f));
        }
        else
        {
            if (currentLevel == 1)
            {
                FindFirstObjectByType<PotyPlayerController>().CreateReport("Proteja a Entrada do Forte!!!", "Ol� jogador(a), para esse n�vel voc� deve evitar que os zumbis destruam as barreiras que o/a protegem. Se eles deixarem todas vermelhas, voc� perde!!!");
                NextLevel(90f, new Vector3(746.14f, 9.3f, 400.35f));
            }
            if (currentLevel == 2)
            {
                FindFirstObjectByType<GameForteController>().handMenuLevel2.SetActive(true);
                FindFirstObjectByType<PotyPlayerController>().CreateReport("Zumbis a Vista!!!", "Ol� jogador(a), para esse n�vel voc� n�o deve deixar que os zumbis cheguem at� voc�. Se eles se aproximarem demais, voc� morre!!!");
                SetDestinyRandow(2);
                FindFirstObjectByType<GameForteController>().ResetCount();
                FindFirstObjectByType<HeightController>().NewHeight(18.6f);
                UpdateLevelBar();
                NextLevel(90f, new Vector3(654.91f, 18.6f, 400.95f));
            }
            if (currentLevel == 3)
            {
                FindFirstObjectByType<GameForteController>().handMenuLevel3.SetActive(true);
                FindFirstObjectByType<PotyPlayerController>().CreateReport("Proteja o Mcgaiver!!!", "Ol� jogador(a), para esse n�vel voc� n�o deve deixar que os zumbis peguem o Mcgaiver. Se a vida dele chegar a zero, ele morre e voc� perde!!!");
                SetDestinyRandow(3);
                FindFirstObjectByType<GameForteController>().ResetCount();
                FindFirstObjectByType<HeightController>().NewHeight(8.4f);
                UpdateLevelBar();
                NextLevel(90f, new Vector3(710.36f, 8.4f, 401.15f));
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
            if (currentLevel == 3)
            {
                currentAmount = 6;
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
        GameObject.FindGameObjectWithTag("Level").GetComponent<Image>().fillAmount = 0.35f * currentLevel;
        GameObject.FindGameObjectWithTag("Level").transform.GetChild(2).GetComponent<Text>().text = currentLevel + "";
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
                    finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parab�ns!!!";
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
            if (wallsDestroyed >= 13 && currentLevel == 1)
            {
                levelIsRunning = false;
                finishUI.SetActive(true);
                FindFirstObjectByType<GameForteController>().GameOver();
            }
            if (slot.childCount == 0 && levelIsRunning)
            {
                levelIsRunning = false;
                if (currentLevel == 1)
                    FindFirstObjectByType<GameForteController>().ChangeStateWalls(false);

                finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parab�ns!!!";

                if (currentLevel == 3)
                {
                    finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Ver Ranking";
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Ranking);
                }
                else
                {
                    finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Proximo Nivel";
                }

                finishUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().NextLevel);

                if (currentLevel < 3)
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
