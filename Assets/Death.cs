using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class Death : MonoBehaviour
{
 private void OnCollisionEnter(Collision other) {
     
     if (other.gameObject.tag=="Lava")
     {
         SceneManager.LoadScene(1);
     }
     if (other.gameObject.tag=="Win")
     {
         SceneManager.LoadScene(2);
     }
 }
}
