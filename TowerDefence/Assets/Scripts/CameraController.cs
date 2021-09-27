using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] float sensX;
    [SerializeField] float sensY;
    [SerializeField] Transform orientation;
    [SerializeField] Camera cam;
    float mouseX;
    float mouseY;
    float multiplier = 0.1f;

    float xRot;
    float yRot;

    private bool _CameraEnabled = true;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        myInput();
        cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }

    private void myInput(){
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRot += mouseX * sensX * multiplier;
        xRot -= mouseY * sensY * multiplier;

        xRot = Mathf.Clamp(xRot, -90f, 90f);
    }

    public void DisableCameraControl()
    {
        _CameraEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EnableCameraControl()
    {
        _CameraEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
