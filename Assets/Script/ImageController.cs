using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ImageController : MonoBehaviour
{


    public Texture texture = null;
    public Sprite sprite = null;

    public ImageType imageType;

    public RenderType renderType;
    
    private Material material = null;

    [Range(0, 255)]
    public int alpha = 255;

    public void Convert()
    {
        if(imageType == ImageType.Sprite && sprite != null)
        {
            texture = ConvertType.Convert(sprite);
        }
    }

    public Material SetMaterial()
    {
        Convert();
        getMaterial();
        return material;
    }

    private void getMaterial()
    {
        if(material == null)
        {
            material = ConvertType.setMaterial(this.gameObject, texture);
        }
    }

    public void SetMaterialRenderingMode(Material material)
    {
        
        if (renderType == RenderType.Opaque){
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }
        else
        {
            // 0 ~ 255 값을 0 ~ 1로 변환
            float normalizedAlpha = alpha / 255.0f;
            Color color = material.color;
            //알파 값을 변경
            color.a = normalizedAlpha;
            material.color = color;
            // Fade 모드로 변경
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
}
public enum ImageType
{
    Sprite,
    Texture2D
}

public enum RenderType
{
    Opaque,
    Transparent
}

