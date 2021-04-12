using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjects : MonoBehaviour
{
    // Start is called before the first frame update
    public int bredde;
    public int laengde;

    public int amount;
    public GameObject spawner;
    public GameObject[] g;
    public GameObject go;
    void Start()
    {
       Spawn();
    }

    void Spawn(){
        
        for (int i = 0; i < amount; i++)
        {
        int z2 = Random.Range(0,bredde);
        int x2 = Random.Range(0,laengde);
        spawner.transform.position=new Vector3(x2, 0, z2);
        int ob=Random.Range(0,g.Length);
        Instantiate(g[ob],spawner.transform.position,Quaternion.identity);
        
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
