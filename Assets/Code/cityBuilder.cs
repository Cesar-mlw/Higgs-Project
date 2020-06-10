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

    private List<Vector3> borders = new List<Vector3>();

    private float bound = 4500; // Variable that controls total area that the city will cover

    private float maxPointdistance = 3500; //basically controls how space the 

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
        List<Vector3> spwnLocations = new List<Vector3>();
        int buildingFootPrint = 3;
        foreach (RegionBorder reg in regions){
            for (int i = 0; i < 10; i++){
                GameObject building = Instantiate(buildings[Random.Range(0, buildings.Length - 1)]);
                Vector3 spwnPoint = reg.centerPoint + new Vector3(Random.Range(1, 400) * buildingFootPrint, 0, Random.Range(1, 400) * buildingFootPrint);
                if(spwnLocations.Contains(spwnPoint)) continue;
                building.transform.position = spwnPoint;
                building.transform.localScale = new Vector3(20, 20, 20);
                building.GetComponent<buildingCollsion>().parentCenterPoint = reg.centerPoint;
                spwnLocations.Add(spwnPoint);                
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

        //Place the black 
        foreach (Edge edge in edges){
            if(edge.ClippedEnds == null) continue;
            PlaceRoads(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT]);
        }
        
    }
    
    private void PlaceRoads(Vector2f p0, Vector2f p1, int offset = 0) {
        int x0 = (int)p0.x;
        int x1 = (int)p1.x;
        int y0 = (int)p0.y;
        int y1 = (int)p1.y;

        // Alpha 2 code that spawn single blocks
        // int dx = Mathf.Abs(x1 - x0);
        // int dy = Mathf.Abs(y1 - y0);
        // int sx = x0 < x1 ? 1 : -1;
        // int sy = y0 < y1 ? 1 : -1;
        // int err = dx - dy;
        
        // while(true){
        //     GameObject line = Instantiate(roads[0], new Vector3(x0+offset, 0, y0+offset), Quaternion.Euler(0, 0, 0));
        //     if(x0 == x1 && y0 == y1) break;
        //     int e2 = 2*err;
        //     if (e2 > -dy){
        //         err -= dy;
        //         x0 += sx;
        //     }
        //     if (e2 < dx) {
        //         err += dx;
        //         y0 += sy;
        //     }
        // }


        

        GameObject road = Instantiate(roads[0]);
        Vector3 dir = new Vector3(x0, 0, y0) - new Vector3(x1, 0, y1);
        Vector3 centerPos = new Vector3(x0 + x1, 0, y0 + y1) / 2f;
        float scaleX = Mathf.Abs(x0 - x1);
        borders.Add(centerPos);

        //Itersection idea - create a vector to get the repeated positions, and create insersections there

        centerPos.x -= 0.5f;
        centerPos.y += 0.5f;
        road.transform.position = centerPos;
        road.transform.localScale = new Vector3(dir.magnitude, 0.5f, 60);
        road.transform.localRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0),  dir);
    }

    
}

