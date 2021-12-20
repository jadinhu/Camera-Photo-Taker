/*
 * Screenshot.cs
 * Created by: Jadson Almeida [jadson.sistemas@gmail.com]
 * Created on: 20/12/21 (dd/mm/yy)
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the screen shot feature, updating a <see cref="Image"/> with Recorded Frame
/// </summary>
public class Screenshot : MonoBehaviour
{
    /// <summary>
    /// Image to be update with every new screen shot
    /// </summary>
    [SerializeField]
    Image image;

    /// <summary>
    /// Takes a screen shot calling <see cref="RecordFrame"/>
    /// </summary>
    public void CaptureScreen()
    {
        StartCoroutine(RecordFrame());
    }

    /// <summary>
    /// Calls <see cref="DestroyCurrentTexture"/> and takes a new screen shot with 
    /// <see cref="ScreenCapture.CaptureScreenshotAsTexture"/> and updates <see cref="Sprite.texture"/> of <see cref="image"/>
    /// </summary>
    IEnumerator RecordFrame()
    {
        yield return null;
        DestroyCurrentTexture();
        var texture2D = ScreenCapture.CaptureScreenshotAsTexture();
        Sprite screenshotSprite = Sprite.Create(texture2D, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
        image.sprite = screenshotSprite;
    }

    /// <summary>
    /// Destroy the current <see cref="Sprite.texture"/> of <see cref="image"/>
    /// </summary>
    void DestroyCurrentTexture()
    {
        if (image.sprite != null)
            Destroy(image.sprite.texture);
    }
}
