using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{


    public Texture texture;
    public Sprite sprite;

    public ImageType imageType;
}
public enum ImageType
{
    Sprite,
    Texture2D
}