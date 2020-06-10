using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using csDelaunay;
public class VoronoiDiagram : MonoBehaviour{
    // Number of polygons / sites we want
    public int polygonNumber = 50;

    // This is where we will store the resulting data
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;

    void Start(){
        // Create your sites (let's call the center of your polygons)
        List<Vector2f> points = CreateRandomPoint();

        // Create the bounds of the Voronoi Diagram
        // Use Rectf instead of Rect; it's a struct just like the Rect and does pretty much the same,
        // but like that it allows you to run the delaunay library outside of Unity (which also mean in another thread)
        Rectf bounds = new Rectf(0, 0, 512, 512);

        // There are two ways you can create the Voronoi diagram: with or withoug lloyd Relaxation
        // Here I use it with two iterations of the lloyd relaxation
        Voronoi voronoi = new Voronoi(points, bounds, 1);

        // But you could also create it without lloyd relaxxation and call that function later if you want
        //Voronoir voronoi = new Voronoi(points, bounds);
        //voronoi.LloydRelaxation(5);

        // Now retreive the edges from it, and the new sites positions if you use dthe lloyd relaxation
        sites = voronoi.SitesIndexedByLocation;
        edges = voronoi.Edges;

        DisplayVoronoiDiagram();
    }

    private List<Vector2f> CreateRandomPoint(){
        // Use Vector2f, instead of Vector2
        // Vector2f is pretty much the same than Vector2, but like you could run Voronoi in antoher thread

        List<Vector2f> points = new List<Vector2f>();
        for(int i = 0; i < polygonNumber; i++){
            points.Add(new Vector2f(Random.Range(0, 512), Random.Range(0, 512)));
        }

        return points;
    }

    private void DisplayVoronoiDiagram() {
        Texture2D tx = new Texture2D(512, 512);
        foreach (KeyValuePair<Vector2f, Site> kv in sites){
            tx.SetPixel((int)kv.Key.x, (int)kv.Key.y, Color.red);
        }
        foreach (Edge edge in edges){
            // if the edge doens't have clippedEnds, if was not withing the bounds, dont draw it
            if(edge.ClippedEnds == null) continue;

            DrawLine(edge.ClippedEnds[LR.LEFT], edge.ClippedEnds[LR.RIGHT], tx, Color.black);
        }
        tx.Apply();

        this.GetComponent<RawImage>().texture = tx;
    }

    // Bresenham Line algorithm
    private void DrawLine(Vector2f p0, Vector2f p1, Texture2D texture, Color color, int offset = 0){
        int x0 = (int)p0.x;
        int y0 = (int)p0.y;
        int x1 = (int)p1.x;
        int y1 = (int)p1.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while(true){
            texture.SetPixel(x0+offset, y0+offset, color);
            if(x0 == x1 && y0 == y1) break;
            int e2 = 2*err;
            if (e2 > -dy){
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }
    }
}
