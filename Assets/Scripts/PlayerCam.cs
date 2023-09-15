using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public float rotationSpeed;

    CameraStyle currentStyle;

    public GameObject topDownCam;
    public GameObject basicCam;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }
    private void Update()
    {

        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if(currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if(inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        if(currentStyle == CameraStyle.Basic)
        {

        }

        if(Input.GetKeyDown(KeyCode.I)) SwitchCameraStyle(CameraStyle.Basic);
        if(Input.GetKeyDown(KeyCode.Escape)) SwitchCameraStyle(CameraStyle.Topdown);
    }

    public void SwitchCameraStyle(CameraStyle newStyle)
    {
        topDownCam.SetActive(false);
        basicCam.SetActive(false);

        if(newStyle == CameraStyle.Basic) basicCam.SetActive(true);
        if(newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }
}
