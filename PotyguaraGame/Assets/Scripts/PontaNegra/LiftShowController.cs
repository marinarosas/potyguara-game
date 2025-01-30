using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LiftShowController : MonoBehaviour
{
    public bool isInsideLift = false;
    public bool isGoingToShow = false;
    public bool isGoOutOfTheShow = false;
    Transform player;
    public Animator ani;

    private int state = -1;

    public Transform catraca1;
    public Transform catraca2;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
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
            FindFirstObjectByType<HeightController>().NewHeight(1.9f);
            //player.transform.position = new Vector3(177.5f, 1.9f, 72f);
        }
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
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("MainCamera").transform.GetChild(4).GetComponent<FadeController>().FadeInForFadeOutWithAnimator(6f, ani);
            isInsideLift = true;
            player.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInsideLift = false;
            if(state==1)
                catraca1.GetComponent<Animator>().Play("CatracaClose");
            else
                catraca2.GetComponent<Animator>().Play("CatracaClose");
        }
    }
}