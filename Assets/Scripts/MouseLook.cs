/*
 * MouseLook.cs
 * Created by: Jadson Almeida [jadson.sistemas@gmail.com]
 * Created on: 11/11/21 (dd/mm/yy)
 * Revised on: 21/12/21 (dd/mm/yy)
 */
using UnityEngine;

/// <summary>
/// Handles the mouse commands to <see cref="Transform"/>: Rotate, Zoom and Walk
/// </summary>
public class MouseLook : MonoBehaviour
{
    /// <summary>
    /// The <see cref="Zoom"/> sensibility of <see cref="Camera.main"/> when rolling mouse scroll wheel
    /// </summary>
    public float zoomSpeed;
    /// <summary>
    /// Limit (min, max) of <see cref="Camera.main"/> zoom
    /// </summary>
    public Vector2 zoomLimit = new Vector2(-3, 30);
    /// <summary>
    /// Limite of positive and negative angle with <see cref="Mathf.Clamp(float, float, float)"/> of <see cref="rotX"/> 
    /// for <see cref="Camera.main"/>
    /// </summary>
    public float clampAngle = 80;
    /// <summary>
    /// Mouse sensitivity for <see cref="Camera.main"/> rotation
    /// </summary>
    public float mouseSensitivity = 100;
    /// <summary>
    /// The started position of <see cref="Camera.main"/>, used for <see cref="ResetCamera"/>
    /// </summary>
    Vector3 cameraStartedPosition;
    /// <summary>
    /// The started rotation of <see cref="Camera.main"/>, used for <see cref="ResetCamera"/>
    /// </summary>
    Quaternion cameraStartedRotation;
    /// <summary>
    /// The rotation around the up/y axis for <see cref="Camera.main"/>
    /// </summary>
    float rotY;
    /// <summary>
    /// The rotation around the right/x axis for <see cref="Camera.main"/>
    /// </summary>
    float rotX;
    /// <summary>
    /// Counter time to check if the mouse middle-button are clicked ou dragged in <see cref="CheckMouseInputs"/>
    /// </summary>
    float timePressedMiddleButton;
    /// <summary>
    /// If the <see cref="Camera.main"/> was moved after mouse middle-button has been clicked
    /// </summary>
    bool hasCameraMoved;

    /// <summary>
    /// Stated values for <see cref="CameraRotate"/>
    /// </summary>
    void Start()
    {
        cameraStartedPosition = transform.position;
        cameraStartedRotation = transform.rotation;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    /// <summary>
    /// Check if player have used mouse inputs
    /// </summary>
    void Update()
    {
        CheckMouseInputs();
    }
    
    /// <summary>
    /// Change the <see cref="mouseSensitivity"/> between a min (1) and max (200)
    /// </summary>
    /// <param name="add">value to increase or decrease mouse sensibility</param>
    public void ChangeMouseSensitivity(float add)
    {
        mouseSensitivity = Mathf.Clamp(mouseSensitivity + add, 1, 200);
    }

    /// <summary>
    /// Check the mouse inputs: 
    /// one click middle-button to <see cref="ResetCamera"/>,
    /// drag middle-button to <see cref="CameraWalk"/>,
    /// drag right-button to <see cref="CameraRotate"/> OR
    /// roll scrollwheel to <see cref="Zoom"/>
    /// </summary>
    void CheckMouseInputs()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            Zoom();
        if (Input.GetMouseButton(1))
            CameraRotate();
        if (Input.GetMouseButton(2))
        {
            timePressedMiddleButton += Time.deltaTime;
            CameraWalk();
        }
        else if (Input.GetMouseButtonUp(2))
        {
            if (timePressedMiddleButton < .15 && !hasCameraMoved)
                ResetCamera();
            timePressedMiddleButton = 0;
            hasCameraMoved = false;
        }
    }

    /// <summary>
    /// Reset the <see cref="Camera.main"/> to started position, rotation and zoom
    /// </summary>
    void ResetCamera()
    {
        transform.position = cameraStartedPosition;
        transform.rotation = cameraStartedRotation;
    }

    /// <summary>
    /// Handles the <see cref="Camera.fieldOfView"/> of <see cref="Camera.main"/> zoom in and out behavior 
    /// with mouse scroll wheel between <see cref="fovLimit"/> limits
    /// </summary>
    void Zoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, 1000);
        Vector3 scrollDestiny = ray.GetPoint(5);

        float step = zoomSpeed * 100 * Time.deltaTime;
        scrollDestiny.y = Mathf.Clamp(scrollDestiny.y, zoomLimit.x, zoomLimit.y);
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if ((wheel > 0 && scrollDestiny.y <= zoomLimit.x) || (wheel < 0 && scrollDestiny.y >= zoomLimit.y))
            return;
        transform.position = Vector3.MoveTowards(transform.position, scrollDestiny, wheel * step);
    }

    /// <summary>
    /// Makes the <see cref="Camera.main"/> follow the mouse look with <see cref="clampAngle"/> limit
    /// </summary>
    void CameraRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    /// <summary>
    /// Makes the <see cref="Camera.main"/> follow the mouse position on screen (Y-axis freezed)
    /// </summary>
    void CameraWalk()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float fixedY = transform.position.y;
        Vector3 destiny = Vector3.zero;
        Vector3 origin = transform.position;
        destiny.x -= mouseX; // inverted to perform like the world are dragged
        destiny.y += mouseY;
        transform.Translate(destiny);
        // fix y-axis
        destiny = transform.position;
        destiny.y = fixedY;
        transform.position = destiny;
        // if the camera have really moved
        if (origin != transform.position)
            hasCameraMoved = true;
    }
}
