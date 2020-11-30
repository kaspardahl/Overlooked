using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ActivatePostPros : MonoBehaviour
{

    void Start()
    {
        GetComponent<PostProcessVolume>().enabled = false;
    }

    void Update()
    {
        if (Input.GetButton("A"))
        {

            GetComponent<PostProcessVolume>().enabled = true;

        }
        else
        {
            GetComponent<PostProcessVolume>().enabled = false;
        }
    }
}
