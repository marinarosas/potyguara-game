using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    private CanvasGroup canvas;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private GameObject objToDesactive;
    private GameObject objToTeleport;
    private Vector3 newPos;
    private Animator animator;

    private bool status = true;

    private void Start()
    {
        canvas = gameObject.GetComponent<CanvasGroup>();
    }

    public void FadeWithStatus()
    {
        if (status) FadeIn();
        else FadeOut();

        status = !status;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }

    public void FadeWithTime(string type, float time)
    {
        if (type.Equals("Out"))
            Invoke("FadeOut", time);
        else
            Invoke("FadeIn", time);
    }

    public void FadeInForFadeOut(float time)
    {
        FadeIn();
        Invoke("FadeOut", time);
    }

    public void FadeInForFadeOutWithAnimator(float time, Animator ani)
    {
        animator = ani;
        FadeIn();
        Invoke("FadeOut", time);
    }

    public void FadeInForFadeOutWithDeactivationOfGameObject(float time, GameObject objeto)
    {
        FadeIn();
        SetObject(objeto);
        Invoke("FadeOut", time);
    }

    public void FadeInForFadeOutWithTeleportOfGameObject(float time, GameObject objeto, Vector3 pos)
    {
        FadeIn();
        SetObjectTeleport(objeto);
        newPos = pos;
        Invoke("FadeOut", time);
    }

    public void FadeOutWithDeactivationOfGameObject(GameObject objeto)
    {
        FadeOut();
        SetObject(objeto);
    }

    public void FadeOutForFadeIn(float time)
    {
        FadeOut();
        Invoke("FadeIn", time);
    }

    public void SetObject(GameObject obj)
    {
        this.objToDesactive = obj;
    }

    public void SetObjectTeleport(GameObject obj)
    {
        this.objToTeleport = obj;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (canvas.alpha < 1f)
                canvas.alpha += Time.deltaTime;
            else
                fadeIn = false;
        }
        if (fadeOut)
        {
            if (canvas.alpha > 0f)
                canvas.alpha -= Time.deltaTime;
            else
            {
                fadeOut = false;
                if (this.objToDesactive != null)
                    this.objToDesactive.SetActive(false);
                else
                {
                    if (SceneManager.GetActiveScene().buildIndex == 2) // ponta negra
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DownTerreo"))
                            FindFirstObjectByType<HeightController>().NewHeight(0f);
                        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("MoveUpTerreo"))
                            FindFirstObjectByType<HeightController>().NewHeight(9.158f);
                    }
                }

                if(objToTeleport != null)
                {
                    this.objToTeleport.transform.position = newPos;
                }
            }
        }
    }
    public void FadeIn()
    {
        fadeIn = true;
    }
}
