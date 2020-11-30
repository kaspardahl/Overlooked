using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimationCode : MonoBehaviour
{
    [Range(-1, 1)]
    public float vertical; //inputX

    [Range(-1, 1)]
    public float horizontal; //InputZ


    private Animator anim;
    public bool enableRootMotion;
    public bool aiming;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //når man ikke kan bevæge sig bruges animationens rootmotion(fx jumps og attacks)
        enableRootMotion = !anim.GetBool("canMove");
        anim.applyRootMotion = enableRootMotion;


        //gør at man kun kan strafe når man er i aim locomotion 
        if (aiming == false)
        {
            vertical = 0;
            horizontal = Mathf.Clamp01(horizontal);
        }

        anim.SetBool("aiming", aiming);

        //med en return gør det at man ikke kan udføre en handling man allerede er igang med, før animationen er færdig.
        if (enableRootMotion)
            return;

        //kan bruges til at lade armen med spydet hænge lidt ned langs siden via en avatar mask i animationLayer Left Hand (Weapon).
        //animator.SetBool("weaponEquiped", true); 


        if (Input.GetButton("Fire1"))
        {
            //print("SunController");
            anim.SetBool("sunControl", true);

            //Spilleren skal stå stille når man bruger suncontroller
            //cameraet skal fokusere på sunarrow

        }
        else
        {
            anim.SetBool("sunControl", false);
        }

        if (Input.GetButton("Fire3"))
        {
            //print("CrouchJump");
            anim.SetBool("crouchJump", true);
        }
        else
        {
            anim.SetBool("crouchJump", false);



         

        }
        anim.SetFloat("vertical", vertical);
        anim.SetFloat("horizontal", horizontal);
    }
}
