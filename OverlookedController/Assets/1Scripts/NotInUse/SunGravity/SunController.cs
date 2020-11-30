using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;

    public float smoothX;
    public float smoothY;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    public float inputSensitivity = 150.0f;

    public KD.StateManager stateManager;



    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotationX = rotation.x;
        rotationY = rotation.y;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //if (stateManager.sunControl)
        //{
        //    //print("SunController");
        //    float inputX = Input.GetAxis("RightStickHor");
        //    float inputZ = Input.GetAxis("RightStickVer");
        //    mouseX = Input.GetAxis("Mouse X");
        //    mouseY = Input.GetAxis("Mouse Y");
        //    finalInputX = inputX + mouseX;
        //    finalInputZ = inputZ + mouseY;

        //    rotationY += finalInputX * inputSensitivity * Time.deltaTime;
        //    rotationX += finalInputZ * inputSensitivity * Time.deltaTime;

        //    Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        //    transform.rotation = localRotation;

           
        //}

        if (Input.GetButtonUp("A"))
        {
            //Prøver at resette rotationen på lyset så det ikke er inverted styring når solen kommer op på den anden side af landskabet. Det virker ikke..
            //print("normalized");
            //gameObject.transform.rotation.Normalize();
        }



    }

   
}
