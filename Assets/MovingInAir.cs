using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingInAir : MonoBehaviour
{
    Vector3 movement = Vector3.zero;
    private Rigidbody rig;
    public GameObject child;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
      rig = GetComponent<Rigidbody>();
      
    }
    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        movement = new Vector3(Horizontal, 0f, Vertical);
    }
     private void FixedUpdate ()
    {
   
        rig.MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
        //child.GetComponent<Rigidbody>().MovePosition(rig.position + movement * speed * Time.fixedDeltaTime);
    }
}
