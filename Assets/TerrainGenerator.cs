using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public int depth = 20;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    public string seed;
	public bool useRandomSeed;

    void Start()
    {
        if(useRandomSeed){
            offsetX = Random.Range(0f,9999f);
            offsetY = Random.Range(0f,9999f);
        }

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width,depth,height);

        terrainData.SetHeights(0,0,GenerateHeights());

        return terrainData; 
    }

    float[,] GenerateHeights(){
       

        float[,] heights = new float[width,height];
        for (int x=0; x<width;x++){
            for(int y=0; y<height;y++){
                heights[x,y] = CalculateHeight(x,y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y){
         	/* if (useRandomSeed) {
			seed = Time.time.ToString();
		} */

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        float xCoord;
        float yCoord;

        if(useRandomSeed){
        xCoord =  (float)x/width * scale + offsetX;
        yCoord = (float)y/height * scale + offsetY;
        }
        else{
        xCoord =  (float)x/width * scale + pseudoRandom.Next(0,9999);;
        yCoord = (float)y/height * scale + pseudoRandom.Next(0,9999);;
        }


        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
