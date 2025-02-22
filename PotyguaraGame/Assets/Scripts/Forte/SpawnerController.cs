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

    private void Start()
    {
        FindFirstObjectByType<GameForteController>().SetCurrentLevel(currentLevel);
        player = GameObject.FindWithTag("Player");
        finishUI = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;

        for (var ii = 0; ii < destinyLevel1.childCount; ii++)
            spawnRandowZombie.Add(destinyLevel1.GetChild(ii));
    }

    #region Levels
    public void SetLevelIsRunning(bool value)
    {
        levelIsRunning = value;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetDestinyRandow(int value)
    {
        spawnRandowZombie.Clear();
        if (value == 2)
        {
            for (var ii = 0; ii < destinyLevel2.childCount; ii++)
                spawnRandowZombie.Add(destinyLevel2.GetChild(ii));
        }
        else
        {
            for (var ii = 0; ii < destinyLevel1.childCount; ii++)
                spawnRandowZombie.Add(destinyLevel1.GetChild(ii));
        }
    }
    public void SetLevel()
    {
        finishUI.SetActive(false);
        if (FindFirstObjectByType<GameForteController>().GetMode() == 1)
        {
            cannons.SetActive(true);
            if (FindFirstObjectByType<TechGuaraController>().GetMode())
            {
                AudioSource audio = FindFirstObjectByType<TechGuaraController>().SelectReport("Techyguara.ApresentaçãoBatalhadoForte");
                FindFirstObjectByType<TechGuaraController>().CreateReport("Defenda o Forte dos Invasores Maritimos!!!", "Na Batalha do Forte, você será um defensor da fortaleza durante uma época de invasões no século 15. Escolha um" +
                    " canhão e lute para proteger a fortaleza a todo custo! Prepare-se para desafios intensos enquanto vive momentos de pura " +
                    "estratégia e ação.", audio.clip.length, new Vector3(665f, 20.44f, 400.36f), 90f);
                audio.Play();
            }
            FindFirstObjectByType<HeightController>().NewHeight(17.19f);
            NextLevel(90f, new Vector3(659f, 17.19f, 400.44f));
        }
        else
        {
            if (currentLevel == 1)
            {
                SetDestinyRandow(1);
                FindFirstObjectByType<HeightController>().NewHeight(7.98f);

                if (FindFirstObjectByType<TechGuaraController>().GetMode())
                {
                    AudioSource audio = FindFirstObjectByType<TechGuaraController>().SelectReport("Techyguara.ApresentaçãoZombieMode");
                    FindFirstObjectByType<TechGuaraController>().CreateReport("Zumbis a Vista!!!", "Em uma história misteriosa que você descobrirá em um futuro distante, os zumbis tomaram conta da cidade" +
                        " do Natal, e o último refúgio da humanidade é a Fortaleza dos Reis Magos.\r\nDefenda a fortaleza contra hordas de zumbis famintos pela " +
                        "sobrevivência!", audio.clip.length, new Vector3(752.74f, 11.35f, 400.29f), 90f);
                    audio.Play();
                }

                NextLevel(90f, new Vector3(749f, 7.98f, 400.44f));
            }
            if (currentLevel == 2)
            {
                cannons.SetActive(false);
                FindFirstObjectByType<GameForteController>().handMenuLevel2.SetActive(true);
                FindFirstObjectByType<GameForteController>().handMenuLevel2.GetComponent<FadeController>().FadeIn();
                SetDestinyRandow(2);
                FindFirstObjectByType<GameForteController>().ResetCount();
                FindFirstObjectByType<HeightController>().NewHeight(17.19f);
                UpdateLevelBar();
                NextLevel(90f, new Vector3(659f, 17.19f, 400.44f));
            }
        }
    }

    public void NextLevel(float angulationY, Vector3 initialPosition)
    {
        player.transform.position = initialPosition;
        player.transform.eulerAngles = new Vector3(0, angulationY, 0);
    }

    public void SendForRanking(int mode)
    {
        FindFirstObjectByType<NetworkManager>().SendPontuacionForte(FindFirstObjectByType<GameForteController>().GetTotalPoints(), mode);
        FindFirstObjectByType<RankingController>().gameObject.GetComponent<FadeController>().FadeIn();
    }

    private void UpdateLevelBar()
    {
        GameObject.FindWithTag("Level").GetComponent<Image>().fillAmount = 0.5f * currentLevel;
        GameObject.FindWithTag("Level").transform.GetChild(3).GetComponent<Text>().text = currentLevel+"";
    }
    #endregion

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
                    foreach (Transform enemy in slot)
                        Destroy(enemy.gameObject);

                    timer.gameObject.SetActive(false);
                    timer.GetComponent<Image>().fillAmount = 1f;
                    levelIsRunning = false;
                    finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parabéns!!!";
                    finishUI.transform.GetChild(3).gameObject.SetActive(false);
                    finishUI.transform.GetChild(6).gameObject.SetActive(true);
                    finishUI.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().ResetGame);
                    Achievement.Instance.partidas_defesaForte++;

                    if(Achievement.Instance.partidas_defesaForte == 50)
                        Achievement.Instance.UnclockAchievement("guerreiro_fortaleza");
                    if(Achievement.Instance.partidas_defesaForte == 100)
                        Achievement.Instance.UnclockAchievement("maquina_guerra");

                    Achievement.Instance.SetStat("navios_levas", Achievement.Instance.ships_levas);

                    finishUI.transform.GetChild(5).GetComponent<Text>().text = FindFirstObjectByType<GameForteController>().GetCurrrentScore() + "";
                    FindFirstObjectByType<GameForteController>().SetTotalPoints();
                    finishUI.SetActive(true);
                    finishUI.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                    SendForRanking(1);
                }
                else
                    if (slot.childCount == 0)
                {
                    Achievement.Instance.ships_levas++;

                    if (Achievement.Instance.partidas_defesaForte == 1)
                        Achievement.Instance.UnclockAchievement("defensor");
                    if (Achievement.Instance.partidas_defesaForte == 3)
                        Achievement.Instance.UnclockAchievement("capitan_fortaleza");
                    if (Achievement.Instance.partidas_defesaForte == 7)
                        Achievement.Instance.UnclockAchievement("mao_de_martelo");

                    Achievement.Instance.SetStat("navios_levas", Achievement.Instance.ships_levas);
                    FindFirstObjectByType<SpawnerController>().SetSpawn();
                }
            }
        }
        else
        {
            if (GameObject.Find("UIPlayer").transform.GetChild(3).GetComponent<Image>().fillAmount <= 0 && currentLevel == 1)
            {
                levelIsRunning = false;
                FindFirstObjectByType<GameForteController>().GameOver();
                return;
            }
            if (slot.childCount == 0 && levelIsRunning)
            {
                levelIsRunning = false;
                if (currentLevel == 1)
                {
                    FindFirstObjectByType<GameForteController>().ChangeStateWalls(false);
                    GameObject.Find("UIPlayer").transform.GetChild(3).gameObject.SetActive(false);
                }

                finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parabéns!!!";

                if (currentLevel == 2)
                {
                    Achievement.Instance.UnclockAchievement("end_line");

                    finishUI.transform.GetChild(3).gameObject.SetActive(false);
                    finishUI.transform.GetChild(6).gameObject.SetActive(true);
                    finishUI.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().ResetGame);

                    currentLevel = 1;
                    FindFirstObjectByType<GameForteController>().SetCurrentLevel(currentLevel);
                    GameObject.FindWithTag("Level").transform.GetChild(3).GetComponent<Text>().text = currentLevel + "";
                    finishUI.transform.GetChild(5).GetComponent<Text>().text = FindFirstObjectByType<GameForteController>().GetCurrrentScore() + "";
                    GameObject.FindWithTag("Level").SetActive(false);
                    FindFirstObjectByType<GameForteController>().SetTotalPoints();
                    SendForRanking(0);
                }
                else
                {
                    currentLevel++;
                    FindFirstObjectByType<GameForteController>().SetCurrentLevel(currentLevel);
                    finishUI.transform.GetChild(5).GetComponent<Text>().text = FindFirstObjectByType<GameForteController>().GetCurrrentScore() + "";
                    FindFirstObjectByType<GameForteController>().SetTotalPoints();
                    finishUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Proximo Nivel";
                    finishUI.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(FindFirstObjectByType<GameForteController>().NextLevel);
                }

                finishUI.SetActive(true);
                finishUI.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void CleanSlot()
    {
        foreach (Transform enemy in slot)
            Destroy(enemy.gameObject);
    }

    #region Spawners
    public void SetSpawn()
    {
        levelIsRunning = true;
        if (FindFirstObjectByType<GameForteController>().GetMode() == 0)
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
    #endregion
}
