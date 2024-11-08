using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;

public class SpawnerController : MonoBehaviour
{
    [Header("Normal Mode")]
    public Transform destinyLevel;
    public GameObject prefabNavio;
    private List<Transform> destinyRandowNavio = new List<Transform>();

    [Header("Zombie Mode")]
    public GameObject prefabZumbi;
    public Transform destinyLevel1;
    public Transform destinyLevel2;
    public Transform destinyLevel3;
    public Transform slot;

    private int currentLevel = 1;
    private int wallsDestroyed = 0;
    private List<Transform> destinyRandowZombie = new List<Transform>();

    [Header("General")]
    private bool initLevel = false;
    private GameObject player;
    private int currentAmount;
    public void setWallsDestroyed()
    {
        wallsDestroyed++;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        for (var ii = 0; ii < destinyLevel.childCount; ii++)
        {
            destinyRandowNavio.Add(destinyLevel.GetChild(ii));
        }
        for (var ii = 0; ii < destinyLevel1.childCount; ii++)
        {
            destinyRandowZombie.Add(destinyLevel1.GetChild(ii));
        }
    }

    public Transform getIAPoint()
    {
        if(FindObjectOfType<GameForteController>().getMode() == 1)
        {
            return destinyRandowNavio[Random.Range(0, destinyRandowNavio.Count - 1)];
        }
        else
        {
            return destinyRandowZombie[Random.Range(0, destinyRandowZombie.Count - 1)];
        }

    }

    public Transform getIAPoint(int value, int categoria)
    {
        if(categoria == 0)
        {
            return destinyRandowZombie[value];
        }
        else
        {
            return destinyRandowNavio[value];
        }

    }

    public void SetDestinyRandow(int value)
    {
        destinyRandowZombie.Clear();
        if (value == 2)
        {
            for (var ii = 0; ii < destinyLevel2.childCount; ii++)
            {
                destinyRandowZombie.Add(destinyLevel2.GetChild(ii));
            }
        }
        if (value == 3)
        {
            for (var ii = 0; ii < destinyLevel3.childCount; ii++)
            {
                destinyRandowZombie.Add(destinyLevel3.GetChild(ii));
            }
        }
    }
    public void SetLevel()
    {
        GameObject finishUI = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
        finishUI.SetActive(false);
        if (currentLevel == 1)
        {
            NextLevel(90f, new Vector3(746.14f, 9.3f, 400.35f));
        }
        if (currentLevel == 2)
        {
            SetDestinyRandow(2);
            FindObjectOfType<GameForteController>().ResetCount();
            FindObjectOfType<HeightController>().NewHeight(18.6f);
            UpdateLevelBar();
            NextLevel(90f, new Vector3(654.91f, 18.6f, 400.95f));
        }
        if (currentLevel == 3)
        {
            SetDestinyRandow(3);
            FindObjectOfType<GameForteController>().ResetCount();
            FindObjectOfType<HeightController>().NewHeight(8.35f);
            UpdateLevelBar();
            NextLevel(90f, new Vector3(710.36f, 8.35f, 401.15f));
        }
    }

    public void SetSpawn()
    {
        initLevel = true;
        if(FindObjectOfType<GameForteController>().getMode() == 0)
        {
            if (currentLevel == 1)
            {
                currentAmount = 12;
                InitSpawner(destinyRandowZombie);
            }
            if (currentLevel == 2)
            {
                currentAmount = 9;
                InitSpawner(destinyRandowZombie);
            }
            if (currentLevel == 3)
            {
                currentAmount = 7;
                InitSpawner(destinyRandowZombie);
            }
        }
        else
        {
            currentAmount = 3;
            InitSpawner(destinyRandowNavio);
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

    private void Update()
    {
        if (wallsDestroyed >= 13 && currentLevel == 1)
        {
            initLevel = false;
            GameObject finishUI = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
            finishUI.SetActive(true);
            FindObjectOfType<GameForteController>().GameOver();

        }
        if (slot.childCount == 0 && initLevel)
        {
            initLevel = false;
            GameObject finishUI = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
            finishUI.transform.GetChild(1).GetComponent<Text>().text = "Parabéns!!!";

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
            finishUI.transform.GetChild(5).GetComponent<Text>().text = FindObjectOfType<GameForteController>().GetCurrrentPoints() + "";
            FindObjectOfType<GameForteController>().SetTotalPoints();
            finishUI.SetActive(true);
            finishUI.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
            if(currentLevel < 3)
            {
                currentLevel++;
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
        FindObjectOfType<NetworkManager>().SendPontuacionForte(FindObjectOfType<GameForteController>().GetTotalPoints());
        Invoke("ShowRanking", 0.7f);
    }

    private void ShowRanking()
    {
        FindObjectOfType<RankingController>().ShowRanking();
    }

    private void InitSpawner(List<Transform> points)
    {
        for (int ii = 0; ii < currentAmount; ii++)
        {
            int numInt = Random.Range(0, points.Count-1);
            GameObject prefab = FindObjectOfType<GameForteController>().getMode() == 1 ? prefabNavio : prefabZumbi;
            if (ii == 0)
            {
                Instantiate(prefab, points[numInt].position, Quaternion.identity, slot);
            }
            else
            {
                StartCoroutine(TimeForSpawn(points[numInt], prefab));
            }
        }
    }

    IEnumerator TimeForSpawn(Transform point, GameObject prefab)
    {
        yield return new WaitForSeconds(4f);
        Instantiate(prefab, point.position, Quaternion.identity, slot);
    }
}
