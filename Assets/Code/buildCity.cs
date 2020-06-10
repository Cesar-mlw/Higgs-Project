using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildCity : MonoBehaviour
{
    public GameObject[] buildings;
    public int mapWidth = 20;
    public int mapHeight = 20;
    public int buildingFootprint = 3;
    void Start(){
        float seed = Random.Range(0, 100);
        for(int h = 0; h < mapHeight; h++){
            for(int w = 0; w < mapWidth; w++){
                int result = (int) (Mathf.PerlinNoise(w/5.0f + seed, h/5.0f + seed) * 10);
                Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint);
                if(result < 2)
                    Instantiate(buildings[0], pos, Quaternion.identity);
                else if(result < 4)
                    Instantiate(buildings[1], pos, Quaternion.identity);
                else if(result < 5)
                    Instantiate(buildings[2], pos, Quaternion.identity);
                else if(result < 6)
                    Instantiate(buildings[3], pos, Quaternion.identity);
                else if(result < 7)
                    Instantiate(buildings[4], pos, Quaternion.identity);
                else if(result < 10)
                    Instantiate(buildings[5], pos, Quaternion.identity);
                //IDEA - Make an array of GameObjects that is sorted by their heights, or a data type that holds the building's height value
                // so that we can sort through multiple buildings with similar heights
                //IDEA - Make a better decision making algorithm to determine which building to instantiate
            }
        }
    }

    
}
