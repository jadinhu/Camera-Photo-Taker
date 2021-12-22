/*
 * PhotoCapture.cs
 * Created by: Jadson Almeida [jadson.sistemas@gmail.com]
 * Created on: 22/12/21 (dd/mm/yy)
 * Revised on: 22/12/21 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the photo taker behaviours, switching between the <see cref="cameraTargetVision"/> and 
/// a photo on <see cref="objPhoto"/> 
/// </summary>
public class PhotoCapture : MonoBehaviour
{
    /// <summary>
    /// Panel with target aim displayed when ready to take a photo
    /// </summary>
    [SerializeField]
    GameObject cameraTargetVision;
    /// <summary>
    /// Photo object displayed on screen with <see cref="ShowPhoto"/>
    /// </summary>
    [SerializeField]
    GameObject objPhoto;
    /// <summary>
    /// Image component of <see cref="objPhoto"/> which the photo is displayed
    /// </summary>
    [SerializeField]
    Image imagePhotoDisplay;
    /// <summary>
    /// The sound of photo taker
    /// </summary>
    [SerializeField]
    AudioClip soundPhotoTaked;
    /// <summary>
    /// Texture used on photo capture
    /// </summary>
    Texture2D textureCaptured;
    /// <summary>
    /// Centralized anchor for new sprites to <see cref="imagePhotoDisplay"/>
    /// </summary>
    Vector2 spriteAnchor;

    void Start()
    {
        Setup();
    }

    /// <summary>
    /// Sets the started values of local variables
    /// </summary>
    void Setup()
    {
        textureCaptured = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        spriteAnchor = new Vector2(.5f, .5f);
    }

    /// <summary>
    /// If <see cref="objPhoto"/> is actived, takes a photo with <see cref="CapturePhoto"/>. Otherwise, 
    /// remove the already photo on screen with <see cref="RemovePhoto"/>
    /// </summary>
    public void Play()
    {
        if (!objPhoto.activeSelf)
            StartCoroutine(CapturePhoto());
        else
            RemovePhoto();
    }

    /// <summary>
    /// Hides <see cref="cameraTargetVision"/>, play <see cref="soundPhotoTaked"/>, takes a new screenshot with 
    /// <see cref="Texture2D.ReadPixels(Rect, int, int, bool)"/> of <see cref="textureCaptured"/> 
    /// and calls <see cref="ShowPhoto"/>
    /// </summary>
    IEnumerator CapturePhoto()
    {
        cameraTargetVision.SetActive(false);
        yield return new WaitForEndOfFrame();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(soundPhotoTaked);
        Rect retionToRead = new Rect(0, 0, Screen.width, Screen.height);
        textureCaptured.ReadPixels(retionToRead, 0, 0, false);
        textureCaptured.Apply();
        ShowPhoto();
    }

    /// <summary>
    /// Updates <see cref="Image.sprite"/> of <see cref="imagePhotoDisplay"/> from <see cref="textureCaptured"/>
    /// and shows <see cref="objPhoto"/>
    /// </summary>
    void ShowPhoto()
    {
        imagePhotoDisplay.sprite = Sprite.Create(textureCaptured,
            new Rect(0f, 0f, textureCaptured.width, textureCaptured.height), spriteAnchor, 100.0f);
        objPhoto.SetActive(true);
    }

    /// <summary>
    /// Hides <see cref="objPhoto"/> and shows <see cref="cameraTargetVision"/>
    /// </summary>
    void RemovePhoto()
    {
        objPhoto.SetActive(false);
        cameraTargetVision.SetActive(true);
    }
}
