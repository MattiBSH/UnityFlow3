using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class Death : MonoBehaviour
{
 private void OnCollisionEnter(Collision other) {
     Debug.Log(other);
     if (other.gameObject.tag=="Lava")
     {
         SceneManager.LoadScene(1);
     }
 }
}
