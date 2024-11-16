using UnityEngine;

namespace TictactoeTictactoe.SlidingPuzzle
{
    public class ConvertType : MonoBehaviour
    {
        public static ConvertType Instance;


        public  static Texture Convert(Object albedoTexture)
        {
            Sprite sprite = (Sprite)albedoTexture;
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                            (int)sprite.textureRect.y,
                                                            (int)sprite.textureRect.width,
                                                            (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }

        public static Material setMaterial(GameObject gameObject, Texture texture)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.mainTexture = (Texture)texture;
            gameObject.GetComponent<Renderer>().material = material;
            return material;
        }
    }
    
}