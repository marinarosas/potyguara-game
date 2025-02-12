using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    public List<Image> images;
    public List<Sprite> sprites;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Image image in images)
            image.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
