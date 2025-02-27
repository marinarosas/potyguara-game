using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    public GameObject board;
    private void Start()
    {
        StartPositionOfGame();
    }
    public void StartHover()
    {
        Invoke("ModifyPositionOfPlayer", 2f);
    }

    public void ModifyPositionOfPlayer() {
        GameObject point = GameObject.Find("PointOfStart");
        if(point != null)
        {
            Achievement.Instance.partidas_hover++;
            if (Achievement.Instance.partidas_defesaForte == 1)
                Achievement.Instance.UnclockAchievement("first_hover");
            if (Achievement.Instance.partidas_defesaForte == 50)
                Achievement.Instance.UnclockAchievement("50tou");
            if (Achievement.Instance.partidas_defesaForte == 100)
                Achievement.Instance.UnclockAchievement("centenario");

            Achievement.Instance.SetStat("partidas_hover", Achievement.Instance.ships_levas);

            board.transform.position = point.transform.position;
            board.transform.eulerAngles = new Vector3(0f, point.transform.eulerAngles.y, 0f);
            board.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void RestartTheGame()
    {
        StartPositionOfGame();
    }

    public void StartPositionOfGame()
    {
        board.GetComponent<Rigidbody>().isKinematic = true;
        Transform initialPosition = GameObject.Find("InitialPosition").transform;
        board.transform.position = initialPosition.position;
        board.transform.eulerAngles = new Vector3(0, initialPosition.eulerAngles.y, 0);
    }
}
