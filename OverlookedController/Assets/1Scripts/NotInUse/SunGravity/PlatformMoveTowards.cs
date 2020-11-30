using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveTowards : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;

    private Transform target;
    public bool movable = false;

    


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("PlatformTarget").GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //Lav en collider 

        if (movable == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Renderer render = GetComponent<Renderer>();
        render.material.color = Color.blue;
    }
}
