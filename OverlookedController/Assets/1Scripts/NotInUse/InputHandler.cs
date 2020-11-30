using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace KD {
//    public class InputHandler : MonoBehaviour
//    {
//        float vertical;
//        float horizontal;
//        bool b_input;
//        bool a_input;
//        bool a_inputRelease;
//        bool x_input;
//        bool y_input;

//        bool rb_input;
//        float rt_axis;
//        bool rt_input;
//        bool lb_input;
//        float lt_axis;
//        bool lt_input;

//        bool leftAxis_down;
//        bool rightAxis_down;
        

//        StateManager states;
//        CameraManager camManager;

//        float delta;
//    void Start()
//        {
//            states = GetComponent<StateManager>();
//            states.Init();

//            camManager = CameraManager.singleton;
//            camManager.Init(states);  
//    }


//        void FixedUpdate()
//        {

//            delta = Time.fixedDeltaTime;

//            GetInput();     // måske skal den i Update()
//            UpdateStates();

//            states.FixedTick(delta);
//            camManager.Tick(delta);     //  måske skal den i LateUpdate()

//        }

//        private void Update()
//        {
//            delta = Time.deltaTime;
//            states.Tick(delta);

    

//        }

//        void GetInput()
//        {
//            vertical = Input.GetAxis("Vertical");
//            horizontal = Input.GetAxis("Horizontal");
//            a_input = Input.GetButtonDown("A");
//            a_inputRelease = Input.GetButtonUp("A");
//            b_input = Input.GetButton("B");
//            x_input = Input.GetButton("X");
//            y_input = Input.GetButtonUp("Y");
//            rt_input = Input.GetButton("RT");
//            rt_axis = Input.GetAxis("RT");
//            if(rt_axis != 0)
//                rt_input = true;
//            lt_input = Input.GetButton("LT");
//            lt_axis = Input.GetAxis("LT");
//            if (lt_axis != 0)
//                lt_input = true;
//            rb_input = Input.GetButton("RB");
//            lb_input = Input.GetButton("LB");

//            rightAxis_down = Input.GetButtonUp("L");
//        }

//        void UpdateStates()
//        {
//            states.horizontal = horizontal;
//            states.vertical = vertical;

//            //de tre nedenstående linjer gør at man bevæger sig i forhold til kameraets view
//            Vector3 v = vertical * camManager.transform.forward;
//            Vector3 h = horizontal * camManager.transform.right;
//            states.moveDir = (v + h).normalized;

//            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
//            states.moveAmount = Mathf.Clamp01(m);

//            states.runJumpInput = b_input;

//            if(a_input)
//            {
//                states.sunControl = !states.sunControl;
//                states.HandleSunControl();
//            }
      
            

//            if (b_input)
//            {
//                //states.run = (states.moveAmount > 0);
//            }
//            else
//            {
//                //states.run = false;
//            }
//            states.rt = rt_input;
//            states.lt = lt_input;
//            states.rb = rb_input;
//            states.lb = lb_input;

//            if (y_input)
//            {
//                states.weaponEquiped = !states.weaponEquiped;
//                states.HandleEquiped();
//            }

//            if (rightAxis_down)
//            {  
//                states.lockOn = !states.lockOn;
//                if (states.lockonTransform == null)
//                    states.lockOn = false;

//                camManager.lockonTransform = states.lockonTransform; //det her blev ændret fra .transform til GetTarget(), da SA gjorde så man kunne sætte lock on til bestemte bones af et humanoid - Lock and Roll #2 (34:20)
//                states.lockonTransform = camManager.lockonTransform;
//                camManager.lockon = states.lockOn;
//            }

//        }
//    }
//}
