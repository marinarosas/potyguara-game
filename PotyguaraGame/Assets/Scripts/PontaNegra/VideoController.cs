using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer video;
    public Transform characters;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            video.Play();
            FindObjectOfType<DragonController>().setStartDragon(true);
            for(int ii=0; ii < characters.childCount; ii++)
            {
                characters.GetChild(ii).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            video.Pause();
            for (int ii = 0; ii < characters.childCount; ii++)
            {
                characters.GetChild(ii).gameObject.SetActive(false);
            }
        }
    }
}
