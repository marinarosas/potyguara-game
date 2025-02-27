using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class LiftShowController : MonoBehaviour
{
    public bool isInsideLift = false;
    public bool isGoingToShow = false;
    public bool isGoOutOfTheShow = false;
    private bool hasTicket = false;

    private Transform player;
    private Animator ani;
    private int state = -1;

    public Transform catraca1;
    public Transform catraca2;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        transform.GetChild(1).GetComponent<TeleportationArea>().enabled = false;
        player = GameObject.FindWithTag("Player").transform;
    }

    public void UnleashLift()
    {
        transform.GetChild(1).GetComponent<TeleportationArea>().enabled = false;
        catraca1.GetChild(0).GetChild(0).gameObject.SetActive(false);
        catraca2.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }

    public void BlockLift()
    {
        transform.GetChild(1).GetComponent<TeleportationArea>().enabled = true;
        catraca1.GetChild(0).GetChild(0).gameObject.SetActive(true);
        catraca2.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    public void OpenCatraca1()
    {
        catraca1.GetComponent<Animator>().Play("CatracaOpen");
        player.parent = null;
        if (isInsideLift)
        {
            FindFirstObjectByType<HeightController>().NewHeight(8.15f);
            //player.transform.position = new Vector3(177.8f, 8.3f, 111.95f);
        }
    }

    public void OpenCatraca2()
    {
        catraca2.GetComponent<Animator>().Play("CatracaOpen");
        player.parent = null;
        if (isInsideLift)
        {
            FindFirstObjectByType<HeightController>().NewHeight(1.85f);
            //player.transform.position = new Vector3(177.5f, 1.9f, 72f);
        }
    }

    public void GoToShow()
    {
        ani.Play("GoingToTheShow");
    }

    public void GoOutFromTheShow()
    {
        ani.Play("GoOutShow");
    }

    public void StartTrasition()
    {
        FindFirstObjectByType<PotyPlayerController>().HideControllers();
    }

    public void EndTransition()
    {
        FindFirstObjectByType<PotyPlayerController>().ShowControllers();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInsideLift)
        {
            if(state==1)
                ani.Play("GoingToTheShow");
            else
                ani.Play("GoOutShow");
        }
    }

    public void ChangeThePoint(int state)
    {
        if(state != this.state)
            this.state = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MainCamera"))
        {
            if (hasTicket)
            {
                GameObject.FindWithTag("MainCamera").transform.GetChild(4).GetComponent<FadeController>().FadeInForFadeOutWithAnimator(6f, ani);
                isInsideLift = true;
                player.parent = transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MainCamera"))
        {
            isInsideLift = false;
            if(state==1)
                catraca1.GetComponent<Animator>().Play("CatracaClose");
            else
                catraca2.GetComponent<Animator>().Play("CatracaClose");
        }
    }
}