﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingCollsion : MonoBehaviour{

    public Vector3 parentCenterPoint;

    private float buildingFootprint = 6;
    void OnTriggerEnter(Collider other){
        Debug.Log("Actoivate");
        if(other.gameObject.CompareTag("Building") || other.gameObject.CompareTag("Road")){
            this.RepositionBuilding();
        }
    }
    void OnTriggerStay(Collider other){
        Debug.Log("Actoivate");
        if(other.gameObject.CompareTag("Building") || other.gameObject.CompareTag("Road")){
            this.RepositionBuilding();
        }
    }

    private void RepositionBuilding() {
        Vector3 spwnPoint = this.parentCenterPoint + new Vector3(Random.Range(1, 400) * buildingFootprint, -0.5f, Random.Range(1, 400) * buildingFootprint);
        Debug.Log("Building Repositioned");
        gameObject.transform.position = spwnPoint;
    }

}