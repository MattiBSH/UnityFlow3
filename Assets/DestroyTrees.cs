using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrees : MonoBehaviour
{
    private void OnCollisionStay(Collision other) {
        
    
        if (other.gameObject.name=="Player")
        {
            
        }else
        {
            Destroy(other.gameObject);
        }
    }
}
