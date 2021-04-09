using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop : MonoBehaviour
{
    public GameObject balloon;
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision with: " + collider.gameObject.name);
        
        if(collider.gameObject.tag == "Terrain"){
            balloon.SetActive(false);
        }
    }

}