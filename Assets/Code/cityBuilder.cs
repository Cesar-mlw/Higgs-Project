using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;

public class cityBuilder : MonoBehaviour
{
    public int polygonNumber = 200;

    public GameObject[] roads;

    public GameObject[] buildings;

    public GameObject redCubes;

    private List<RegionBorder> regions = new List<RegionBorder>();// This variable will store the location of the regions and it's borders 

    private List<GameObject> borders = new List<GameObject>();

    private float bound = 4500; // Variable that controls total area that the city will cover

    public float maxPointdistance = 3500; //basically controls how space the 

    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;
    // Start is called before the first frame update
    void Start(){
        List<Vector2f> points = CreateRandomPoints();

        Rectf bounds = new Rectf(0, 0, bound, bound);

        Voronoi voronoi = new Voronoi(points, bounds, 1);

        sites = voronoi.SitesIndexedByLocation;
        edges = voronoi.Edges;
        
        BuildCity();
        BuildBuildings();
    }

    public void BuildBuildings(){
        //Generate Spawn Locations
        //Alpha 1 building placement code
        // List<Vector3> spwnLocations = new List<Vector3>();
        // int buildingFootPrint = 3;
        // foreach (RegionBorder reg in regions){
        //     for (int i = 0; i < 10; i++){
        //         GameObject buildingParent = GameObject.Find("buildingParent");
        //         GameObject building = Instantiate(buildings[Random.Range(0, buildings.Length - 1)], buildingParent.transform);
        //         Vector3 spwnPoint = reg.centerPoint + new Vector3(Random.Range(1, 400) * buildingFootPrint, 0, Random.Range(1, 400) * buildingFootPrint);
        //         if(spwnLocations.Contains(spwnPoint)) continue;
        //         building.transform.position = spwnPoint;
        //         building.transform.localScale = new Vector3(20, 20, 20);
        //         building.GetComponent<buildingCollsion>().parentCenterPoint = reg.centerPoint;
                
        //         spwnLocations.Add(spwnPoint);                
        //     }
        // }
        
        for(int i = 0; i < borders.Count; i++){
            
            GameObject parent = borders[i];
            GameObject buildingParent = GameObject.Find("buildingParent");
            Transform front = GetChildWithName(parent, "front").transform;
            Transform back = GetChildWithName(parent, "back").transform;
            Vector3 buildingFootprint =  buildings[0].GetComponent<MeshRenderer>().bounds.size;
            float numberOfBuildings =  Mathf.Floor(Vector3.Distance(front.position, back.position)/buildingFootprint.x/2) - 2f;
            // Buildings on the right side of the road
            Vector3 spwnPosition = parent.transform.position - parent.transform.right * parent.transform.localScale.magnitude/2 + parent.transform.forward * 45f;
            for(int j = 0; j < numberOfBuildings; j++){
                GameObject building = Instantiate(buildings[0]);
                spwnPosition += parent.transform.right * (buildingFootprint.x + 10f);
                building.transform.position = spwnPosition;
                building.transform.localScale = new Vector3(8, 8, 8);
                building.transform.localRotation = parent.transform.localRotation * Quaternion.Euler(0f, 180f, 0f);
            }
            // Buildings on the right side of the road
            spwnPosition = parent.transform.position + -parent.transform.right * parent.transform.localScale.magnitude/2 + -parent.transform.forward * 45f;
            for(int k = 0; k < numberOfBuildings; k++){
                GameObject building = Instantiate(buildings[0]);
                spwnPosition += parent.transform.right * (buildingFootprint.x + 10f);
                building.transform.position = spwnPosition;
                building.transform.localScale = new Vector3(8, 8, 8);
                building.transform.localRotation = parent.transform.localRotation;
            }
        }
        
        

    }

    private List<Vector2f> CreateRandomPoints(){
        List<Vector2f> points = new List<Vector2f>();

        for(int i = 0; i < polygonNumber; i++){
            points.Add(new Vector2f(Random.Range(0, maxPointdistance), Random.Range(0, maxPointdistance)));
        }
    
        return points;
    }

    private void BuildCity(){
        // Place the red dots that represent the the center of the polygons, also generate spawn locations for the buildings

        foreach (KeyValuePair<Vector2f, Site> kv in sites){
            RegionBorder region = new RegionBorder(kv.Value.SiteIndex, new Vector3(kv.Key.x, 0, kv.Key.y));
            regions.Add(region);
        }
        int count = 0;
        //Place the black 
        foreach (Edge edge in edges){
            if(edge.ClippedEnds == null) continue;
            PlaceRoads(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], count);
            count++;
        }
        
    }
    
    private void PlaceRoads(Vector2f p0, Vector2f p1, int index, int offset = 0) {
        int x0 = (int)p0.x;
        int x1 = (int)p1.x;
        int y0 = (int)p0.y;
        int y1 = (int)p1.y;
        
        // alpha 2 road generation code
        Vector3 dir = new Vector3(x0, 0, y0) - new Vector3(x1, 0, y1);
        if(dir.magnitude - 40f <= 30f) return;
        GameObject road = Instantiate(roads[0]);
        GameObject roadManager = GameObject.Find("roadParent");
        
        Vector3 centerPos = new Vector3(x0 + x1, 0, y0 + y1) / 2f;
        borders.Add(road);

        

        centerPos.x -= 0.5f;
        centerPos.y += 0.5f;
        road.transform.position = centerPos;
        road.transform.localScale = new Vector3(dir.magnitude - 40f, 0.5f, 60); // dir.magnitude
        road.transform.localRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0),  dir);
        road.transform.parent = roadManager.transform;
        road.name = "road" + index;

    }

    private GameObject GetChildWithName(GameObject obj, string name){
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if(childTrans != null){
            return childTrans.gameObject;
        }
        else {
            return null;
        }
    }

    
}

