/*
 * InputControl.cs
 * Created by: Jadson Almeida [jadson.sistemas@gmail.com]
 * Created on: 20/12/21 (dd/mm/yy)
 * Revised on: 22/12/21 (dd/mm/yy)
 */

using UnityEngine;

/// <summary>
/// Handles the user's inputs and delegates to behaviours scripts
/// </summary>
public class InputControl : MonoBehaviour
{
    /// <summary>
    /// Script that handles screenshot behaviours
    /// </summary>
    [SerializeField]
    PhotoCapture photoCapture;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && photoCapture)
            photoCapture.Play();
    }
}
