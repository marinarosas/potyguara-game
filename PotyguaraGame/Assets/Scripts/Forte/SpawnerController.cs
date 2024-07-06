using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerController : MonoBehaviour
{
    public GameObject prefabZumbi;
    public Transform destinyLevel1;
    public Transform destinyLevel2;
    public Transform destinyLevel3;
    public Transform slot; // gameobject parent of the zombies

    private int currentLevel = 1;
    private int currentAmountZumbis;
    private bool initLevel = false;
    private GameObject player;
    private List<Transform> destinyRandow = new List<Transform>();

    private int wallsDestroyed = 0;

    public void setWallsDestroyed()
    {
        wallsDestroyed++;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        for (var ii = 0; ii < destinyLevel1.childCount; ii++)
        {
            destinyRandow.Add(destinyLevel1.GetChild(ii));
        }
    }

    public Transform getIAPoint()
    {
        return destinyRandow[Random.Range(0, destinyRandow.Count - 1)];
    }

    public void SetDestinyRandow(int value)
    {
        if (value == 2)
        {
            destinyRandow.Clear();
            for (var ii = 0; ii < destinyLevel2.childCount; ii++)
            {
                destinyRandow.Add(destinyLevel2.GetChild(ii));
            }
        }
        if (value == 3)
        {
            destinyRandow.Clear();
            for (var ii = 0; ii < destinyLevel3.childCount; ii++)
            {
                destinyRandow.Add(destinyLevel3.GetChild(ii));
            }
        }
    }
    public void SetLevel()
    {
        GameObject finishUI = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).gameObject;
        finishUI.SetActive(false);
        if (currentLevel == 1)
        {
            NextLevel(180f, new Vector3(746.14f, 9.3f, 400.35f));
        }
        if (currentLevel == 2)
        {
            SetDestinyRandow(2);
            FindObjectOfType<GameController>().ResetCount();
            FindObjectOfType<HeightController>().NewHeight(18.6f);
            UpdateLevelBar();
            NextLevel(0f, new Vector3(654.91f, 18.6f, 400.95f));
        }
        if (currentLevel == 3)
        {
            SetDestinyRandow(3);
            FindObjectOfType<GameController>().ResetCount();
            FindObjectOfType<HeightController>().NewHeight(8.35f);
            UpdateLevelBar();
            NextLevel(0f, new Vector3(710.36f, 8.35f, 401.15f));
        }
    }


    public void SetSpawn()
    {
        initLevel = true;
        if (currentLevel == 1)
        {
            currentAmountZumbis = 12;
            InitSpawner(destinyRandow);
        }
        if (currentLevel == 2)
        {
            currentAmountZumbis = 9;
            InitSpawner(destinyRandow);
        }
        if (currentLevel == 3)
        {
            currentAmountZumbis = 7;
            InitSpawner(destinyRandow);
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
            FindObjectOfType<GameController>().GameOver();

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
            finishUI.transform.GetChild(5).GetComponent<Text>().text = FindObjectOfType<GameController>().GetCurrrentPoints() + "";
            FindObjectOfType<GameController>().SetTotalPoints();
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
        FindObjectOfType<NetworkManager>().SendPontuacionForte(FindObjectOfType<GameController>().GetTotalPoints());
        Invoke("ShowRanking", 0.7f);
    }

    private void ShowRanking()
    {
        FindObjectOfType<RankingController>().ShowRanking();
    }

    public void InitSpawner(System.Collections.Generic.List<Transform> points)
    {
        for (int ii = 0; ii < currentAmountZumbis; ii++)
        {
            int numInt = Random.Range(0, points.Count-1);
            if (ii == 0)
            {
                Instantiate(prefabZumbi, points[numInt].position, Quaternion.identity, slot);
            }
            else
            {
                StartCoroutine(TimeForSpawn(points[numInt]));
            }
        }
    }

    IEnumerator TimeForSpawn(Transform point)
    {
        yield return new WaitForSeconds(4f);
        Instantiate(prefabZumbi, point.position, Quaternion.identity, slot);
    }
}
