using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KD
{
    public class InsSunArrow : MonoBehaviour
    {



        //insæt PlatformMoveTarget
        public GameObject arrowPointTo;
        public GameObject sunArrow;
        public Transform parent;
        public StateManager stateManager;
        public Transform arrowTarget;

        private GameObject arrow;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            arrowTarget = GameObject.FindGameObjectWithTag("PlatformTarget").GetComponent<Transform>();
            transform.LookAt(arrowTarget); // virker ikke dog peger den på solen når man starter, men så snart man bevæger solen følger pilen ikke med. der er en advarsel omkring linje 35 mangler en quarternion      

            //if (Input.GetButtonDown("A"))
            //{
            //    HandleSunControlArrow();
            //}
        }
        //public void HandleSunControlArrow()
        //{
        //    print("suncontrol");
        //    if (stateManager.sunControl)
        //    {
        //        arrow = Instantiate(sunArrow, transform.position, transform.rotation, parent);
        //    }
        //    else
        //    {
        //        Destroy(arrow);
        //    }
        //}
    }
}
