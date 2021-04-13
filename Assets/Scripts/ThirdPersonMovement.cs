using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    
    public CharacterController controller;
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    public Transform cam;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Animator anim;



    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Move(runSpeed, targetAngle);
            if(Input.GetKeyDown(KeyCode.Space)){
                anim.SetTrigger("Jump");
            }
            
        }
        else if(direction.magnitude >= 0.25f){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Move(walkSpeed, targetAngle);
            
        }
        else if(direction.magnitude <= 0.25f){
            anim.SetFloat("Speed", 0f);
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            anim.ResetTrigger("Jump");
           
        }
        /* if(Input.GetKey(KeyCode.E)){
            anim.SetTrigger("Smash");
        }
        if(Input.GetKeyUp(KeyCode.E)){
            anim.ResetTrigger("Smash");
           
        } */
            
    }

    private void Move(float speed, float targetAngle){
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            anim.SetFloat("Speed", speed);
            controller.Move(moveDir.normalized * speed * Time.deltaTime); 
    }
}
