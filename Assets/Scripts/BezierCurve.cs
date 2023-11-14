using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BezierCurve : Graphic
{
    public List<Vector3> vectors;
    [Range(0.5f, 500f)]
    public float thickness= 1f;
    private Vector3 initVec;
    private Vector3 unitVector;
    protected override void OnPopulateMesh(VertexHelper vh)
    {

        vh.Clear();
        initVec = Vector3.zero;
        var _thickness = thickness * 0.5f;
        unitVector = (Vector3.right * _thickness);

        if(vectors[0]!=Vector3.zero)
        {
            var tmp = new List<Vector3>();
            tmp.Add(Vector3.zero);
            tmp.AddRange(vectors);

            vectors = tmp;
        }

        var bezier = DrawBezierCurve(vectors, 100);
        for(int i = 0; i< bezier.Count; i++)
        {
            DrawLine(vh, bezier[i], i);
        }

        for (int i = 0; i < vectors.Count; i++)
        {
            DrawPoint(vh, vectors[i], i+ bezier.Count);
        }

    }

    void DrawLine(VertexHelper vh, Vector3 point, int index)
    {
        var vertex = UIVertex.simpleVert;

        vertex.color = color;
        var angle = GetAngle(initVec, point) + 90f;

        vertex.position = initVec;
        vertex.position -= Quaternion.Euler(0, 0, angle) * unitVector;
        vh.AddVert(vertex);

        vertex.position = initVec;
        vertex.position += Quaternion.Euler(0, 0, angle) * unitVector;
        vh.AddVert(vertex);

        vertex.position = point;
        vertex.position -= Quaternion.Euler(0, 0, angle) * unitVector;
        vh.AddVert(vertex);

        vertex.position = point;
        vertex.position += Quaternion.Euler(0, 0, angle) * unitVector;
        vh.AddVert(vertex);

        vh.AddTriangle(index * 4 + 0 , index * 4 + 1, index * 4 + 2);
        vh.AddTriangle(index * 4 + 2 , index * 4 + 3 , index * 4 + 1);

        if(index > 0)
        {
            var idx = index - 1;
            vh.AddTriangle(idx * 4 + 2, idx * 4 + 3, index * 4 + 0);
            vh.AddTriangle(idx * 4 + 2, idx * 4 + 3, index * 4 + 1);
        }

        initVec = point;
    }

    void DrawPoint(VertexHelper vh, Vector3 point, int index)
    {
        var vertex = UIVertex.simpleVert;

        vertex.color = Color.red;

        var u = 15f;

        vertex.position = point + new Vector3(u, 0, 0);
        vh.AddVert(vertex);

        
        vertex.position = point + new Vector3(-0, +u, 0);
        vh.AddVert(vertex);

        
        vertex.position = point + new Vector3(0, -u, 0);
        vh.AddVert(vertex);

        
        vertex.position = point + new Vector3(-u, 0, 0);
        vh.AddVert(vertex);

        vh.AddTriangle(index * 4 + 0, index * 4 + 1, index * 4 + 2);
        vh.AddTriangle(index * 4 + 2, index * 4 + 3, index * 4 + 1);

    }

    float GetAngle(Vector3 begin, Vector3 end)
    {
        return (float)(Mathf.Atan2(end.y - begin.y, end.x - begin.x) * (180 / Mathf.PI));
    }


    public List<Vector3> DrawBezierCurve(List<Vector3> points, int capacity)
    {
        var delta = 1f / capacity;
        float time = 0f;

        List<Vector3> returnList = new List<Vector3>();

        var p0 = points[0];
        var p1 = points[1];
        var p2 = points[2];

        for(int i = 0; i<= capacity;i++)
        {
            var q0 = Vector3.Lerp(p0, p1, time);
            var q1 = Vector3.Lerp(p1, p2, time);

            var r0 = Vector3.Lerp(q0, q1, time);
            time += delta;

            returnList.Add(r0);
        }

        return returnList;
    }



}
