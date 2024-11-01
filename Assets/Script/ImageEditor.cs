using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImageController))]
public class ImageEditor : Editor
{
    public ImageController imageController;
    
    private void OnEnable()
    {
        if(AssetDatabase.Contains(target)) {
            imageController = null;
        }
        else
        {
            // target은 Object형이므로 Enemy로 형변환
            imageController = (ImageController)target;
        }
    }

    public override void OnInspectorGUI()
    {
        if(imageController == null)
        {
            return;
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Select Type (Texture and Sprite only)");
        EditorGUILayout.Space(10);
        imageController.imageType = (ImageType)EditorGUILayout.EnumPopup("imageType", imageController.imageType);
        switch(imageController.imageType)
        {
            case ImageType.Sprite:
                imageController.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", imageController.sprite, typeof(Sprite), true);
                break;
            case ImageType.Texture2D:
                imageController.texture = (Texture)EditorGUILayout.ObjectField("Texture", imageController.texture, typeof(Texture), true);
                break;

        }
    }
}
