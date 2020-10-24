using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Road"){
            Vector3 centerPos = other.gameObject.transform.position;
            centerPos = new Vector3(centerPos.x, centerPos.y, centerPos.z - 5f);
        }
    }
}
