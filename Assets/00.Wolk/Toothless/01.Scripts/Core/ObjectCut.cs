using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectCut
{
    public static GameObject[] CutObject(GameObject target, Vector3 normal, Vector3 point, Material material)
    {
        Debug.Log($"normal : {normal} point : {point}");
        Mesh oMesh = target.GetComponent<MeshFilter>().mesh;
        Vector3[] oVertices = oMesh.vertices;
        Vector3[] oNormals = oMesh.normals;
        Vector2[] oUVs = oMesh.uv;

        //정점들을 두 가지로 나눌 준비

        List<Vector3> sideVertices1 = new List<Vector3>();
        List<Vector3> sideNormals1 = new List<Vector3>();
        List<Vector2> sideUVs1 = new List<Vector2>();
        List<int> sideTris1 = new List<int>();

        List<Vector3> sideVertices2 = new List<Vector3>();
        List<Vector3> sideNormals2 = new List<Vector3>();
        List<Vector2> sideUVs2 = new List<Vector2>();
        List<int> sideTris2 = new List<int>();

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector3> newNormals = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        
        int triCount = oMesh.triangles.Length / 3;

        for (int i = 0; i < triCount; i++)
        {
            //값 저장

            int index0 = i * 3;
            int index1 = index0 + 1;
            int index2 = index1 + 1;

            int vertexIdx0 = oMesh.triangles[index0];
            int vertexIdx1 = oMesh.triangles[index1];
            int vertexIdx2 = oMesh.triangles[index2];

            Vector3 vertex0 = oMesh.vertices[vertexIdx0];
            Vector3 vertex1 = oMesh.vertices[vertexIdx1];
            Vector3 vertex2 = oMesh.vertices[vertexIdx2];

            Vector3 normal0 = oMesh.normals[vertexIdx0];
            Vector3 normal1 = oMesh.normals[vertexIdx1];
            Vector3 normal2 = oMesh.normals[vertexIdx2];

            Vector2 uv0 = oMesh.uv[vertexIdx0];
            Vector2 uv1 = oMesh.uv[vertexIdx1];
            Vector2 uv2 = oMesh.uv[vertexIdx2];

            // 방향 구하기 위한 내적 값
            float dot0 = Vector3.Dot(normal, vertex0 - point);
            float dot1 = Vector3.Dot(normal, vertex1 - point);
            float dot2 = Vector3.Dot(normal, vertex2 - point);

            if (dot0 < 0 && dot1 < 0 && dot2 < 0) // 절단면의 노멀과 같은 방향
            {
                sideVertices1.Add(vertex0);
                sideVertices1.Add(vertex1);
                sideVertices1.Add(vertex2);

                sideNormals1.Add(normal0);
                sideNormals1.Add(normal1);
                sideNormals1.Add(normal2);

                sideUVs1.Add(uv0);
                sideUVs1.Add(uv1);
                sideUVs1.Add(uv2);

                sideTris1.Add(sideTris1.Count);
                sideTris1.Add(sideTris1.Count);
                sideTris1.Add(sideTris1.Count);
            }
            else if (dot0 >= 0 && dot1 >= 0 && dot2 >= 0) // 다른 방향
            {
                sideVertices2.Add(vertex0);
                sideVertices2.Add(vertex1);
                sideVertices2.Add(vertex2);

                sideNormals2.Add(normal0);
                sideNormals2.Add(normal1);
                sideNormals2.Add(normal2);

                sideUVs2.Add(uv0);
                sideUVs2.Add(uv1);
                sideUVs2.Add(uv2);

                sideTris2.Add(sideTris2.Count);
                sideTris2.Add(sideTris2.Count);
                sideTris2.Add(sideTris2.Count);
            }
            else
            {
                Debug.Log("dpftmans");
                int aloneVertexIdx = Mathf.Sign(dot0) == Mathf.Sign(dot1)
                    ? vertexIdx2
                    : (Mathf.Sign(dot0) == Mathf.Sign(dot2) ? vertexIdx1 : vertexIdx0);
                int otherVertexIdx0 = Mathf.Sign(dot0) == Mathf.Sign(dot1)
                    ? vertexIdx0
                    : (Mathf.Sign(dot0) == Mathf.Sign(dot2) ? vertexIdx2 : vertexIdx1);
                int otherVertexidx1 = Mathf.Sign(dot0) == Mathf.Sign(dot1)
                    ? vertexIdx1
                    : (Mathf.Sign(dot0) == Mathf.Sign(dot2) ? vertexIdx0 : vertexIdx2);

                Vector3 aloneVertex = oVertices[aloneVertexIdx];
                Vector3 otherVertex0 = oVertices[otherVertexIdx0];
                Vector3 otherVertex1 = oVertices[otherVertexidx1];

                Vector3 aloneNormal = oNormals[aloneVertexIdx];
                Vector3 otherNormal0 = oNormals[otherVertexIdx0];
                Vector3 otherNormal1 = oNormals[otherVertexidx1];

                Vector2 aloneUV = oUVs[aloneVertexIdx];
                Vector2 otherUV0 = oUVs[otherVertexIdx0];
                Vector2 otherUV1 = oUVs[otherVertexidx1];

                float alone2PlaneDist = Mathf.Abs(Vector3.Dot(normal, aloneVertex - point));
                float other02PlaneDist = Mathf.Abs(Vector3.Dot(normal, otherVertex0 - point));
                float other12PlaneDist = Mathf.Abs(Vector3.Dot(normal, otherVertex1 - point));
                float alone2Other0Ratio = alone2PlaneDist / (alone2PlaneDist + other02PlaneDist);
                float alone2Other1Ratio = alone2PlaneDist / (alone2PlaneDist + other12PlaneDist);

                Vector3 createdVert0 = Vector3.Lerp(aloneVertex, otherVertex0, alone2Other0Ratio);
                Vector3 createdVert1 = Vector3.Lerp(aloneVertex, otherVertex1, alone2Other1Ratio);

                Vector3 createdNormal0 = Vector3.Lerp(aloneNormal, otherNormal0, alone2Other0Ratio);
                Vector3 createdNormal1 = Vector3.Lerp(aloneNormal, otherNormal1, alone2Other1Ratio);

                Vector2 createdUV0 = Vector2.Lerp(aloneUV, otherUV0, alone2Other0Ratio);
                Vector2 createdUV1 = Vector2.Lerp(aloneUV, otherUV1, alone2Other1Ratio);

                newVertices.Add(createdVert0);
                newVertices.Add(createdVert1);

                newNormals.Add(createdNormal0);
                newNormals.Add(createdNormal1);

                newUVs.Add(createdUV0);
                newUVs.Add(createdUV1);

                float aloneSide = Vector3.Dot(normal, aloneVertex - point);

                if (aloneSide < 0)
                {
                    //1번
                    sideVertices1.Add(aloneVertex);
                    sideVertices1.Add(createdVert0);
                    sideVertices1.Add(createdVert1);

                    sideNormals1.Add(aloneNormal);
                    sideNormals1.Add(createdNormal0);
                    sideNormals1.Add(createdNormal1);

                    sideUVs1.Add(aloneUV);
                    sideUVs1.Add(createdUV0);
                    sideUVs1.Add(createdUV1);

                    sideTris1.Add(sideTris1.Count);
                    sideTris1.Add(sideTris1.Count);
                    sideTris1.Add(sideTris1.Count);


                    //2번
                    sideVertices2.Add(otherVertex0);
                    sideVertices2.Add(otherVertex1);
                    sideVertices2.Add(createdVert0);

                    sideNormals2.Add(otherNormal0);
                    sideNormals2.Add(otherNormal1);
                    sideNormals2.Add(createdNormal0);

                    sideUVs2.Add(otherUV0);
                    sideUVs2.Add(otherUV1);
                    sideUVs2.Add(createdUV0);

                    sideTris2.Add(sideTris2.Count);
                    sideTris2.Add(sideTris2.Count);
                    sideTris2.Add(sideTris2.Count);


                    sideVertices2.Add(otherVertex1);
                    sideVertices2.Add(createdVert1);
                    sideVertices2.Add(createdVert0);

                    sideNormals2.Add(otherNormal1);
                    sideNormals2.Add(createdNormal1);
                    sideNormals2.Add(createdNormal0);

                    sideUVs2.Add(otherUV1);
                    sideUVs2.Add(createdUV1);
                    sideUVs2.Add(createdUV0);

                    sideTris2.Add(sideTris2.Count);
                    sideTris2.Add(sideTris2.Count);
                    sideTris2.Add(sideTris2.Count);
                }
                else
                {
                    //2번 넣어주기
                    sideVertices2.Add(aloneVertex);
                    sideVertices2.Add(createdVert0);
                    sideVertices2.Add(createdVert1);

                    sideNormals2.Add(aloneNormal);
                    sideNormals2.Add(createdNormal0);
                    sideNormals2.Add(createdNormal1);

                    sideUVs2.Add(aloneUV);
                    sideUVs2.Add(createdUV0);
                    sideUVs2.Add(createdUV1);

                    sideTris2.Add(sideTris2.Count);
                    sideTris2.Add(sideTris2.Count);
                    sideTris2.Add(sideTris2.Count);

                    //1번 넣우기
                    sideVertices1.Add(otherVertex0);
                    sideVertices1.Add(otherVertex1);
                    sideVertices1.Add(createdVert0);

                    sideNormals1.Add(otherNormal0);
                    sideNormals1.Add(otherNormal1);
                    sideNormals1.Add(createdNormal0);

                    sideUVs1.Add(otherUV0);
                    sideUVs1.Add(otherUV1);
                    sideUVs1.Add(createdUV0);

                    sideTris1.Add(sideTris1.Count);
                    sideTris1.Add(sideTris1.Count);
                    sideTris1.Add(sideTris1.Count);


                    sideVertices1.Add(otherVertex1);
                    sideVertices1.Add(createdVert1);
                    sideVertices1.Add(createdVert0);

                    sideNormals1.Add(otherNormal1);
                    sideNormals1.Add(createdNormal1);
                    sideNormals1.Add(createdNormal0);

                    sideUVs1.Add(otherUV1);
                    sideUVs1.Add(createdUV1);
                    sideUVs1.Add(createdUV0);

                    sideTris1.Add(sideTris1.Count);
                    sideTris1.Add(sideTris1.Count);
                    sideTris1.Add(sideTris1.Count);
                }
            }
        }

        List<Vector3> sortedVertices = new List<Vector3>();
        SortVertices(newVertices, out sortedVertices);

        List<Vector3> sideCapVertices1 = new List<Vector3>(), sideCapVertices2 = new List<Vector3>();
        List<Vector3> sideCapNormals1 = new List<Vector3>(), sideCapNormals2 = new List<Vector3>();
        List<Vector2> sideCapUVs1 = new List<Vector2>(), sideCapUVs2 = new List<Vector2>();
        List<int> sideCapTrigs1 = new List<int>(), sideCapTrigs2 = new List<int>();

        MakeCap(normal, sortedVertices, sideCapVertices1, sideCapVertices2,
            sideCapNormals1, sideCapNormals2,
            sideCapUVs1, sideCapUVs2,
            sideCapTrigs1, sideCapTrigs2);

        Mesh mesh1 = new Mesh();
        Mesh mesh2 = new Mesh();

        //메쉬 1 가져온거 세팅
        mesh1.vertices = sideVertices1.ToArray();
        mesh1.normals = sideNormals1.ToArray();
        mesh1.uv = sideUVs1.ToArray();
        mesh1.subMeshCount = target.GetComponent<MeshRenderer>().sharedMaterials.Length + 1; //서브 메쉬 하나 늘리기
        mesh1.SetTriangles(sideTris1, 0);
        mesh1.SetTriangles(sideCapTrigs1, target.GetComponent<MeshRenderer>().materials.Length);

        //메쉬 2 가져온거 세팅
        mesh2.vertices = sideVertices2.ToArray();
        mesh2.normals = sideNormals2.ToArray();
        mesh2.uv = sideUVs2.ToArray();
        mesh2.subMeshCount = target.GetComponent<MeshRenderer>().sharedMaterials.Length + 1; 
        mesh2.SetTriangles(sideTris2, 0);
        mesh2.SetTriangles(sideCapTrigs2, target.GetComponent<MeshRenderer>().materials.Length);

        GameObject object1 = new GameObject(target.name + "_A", typeof(MeshFilter), typeof(MeshRenderer));
        GameObject object2 = new GameObject(target.name + "_B", typeof(MeshFilter), typeof(MeshRenderer));

        Material[] mats = new Material[target.GetComponent<MeshRenderer>().materials.Length + 1];
        
        for (int i = 0; i < target.GetComponent<MeshRenderer>().materials.Length; i++)
        {
            mats[i] = target.GetComponent<MeshRenderer>().materials[i];
        }
        
        mats[target.GetComponent<MeshRenderer>().materials.Length] = material;
        //Material mats = target.GetComponent<MeshRenderer>().material;
        
        //오브젝트 1 만들기
        object1.GetComponent<Renderer>().materials = mats;
        object1.GetComponent<MeshFilter>().mesh = mesh1;
        object1.transform.position = target.transform.position;
        object1.transform.rotation = target.transform.rotation;
        object1.transform.localScale = target.transform.localScale;

        //오브젝트 2 만들기
        object2.GetComponent<Renderer>().materials = mats;
        object2.GetComponent<MeshFilter>().mesh = mesh2;
        object2.transform.position = target.transform.position;
        object2.transform.rotation = target.transform.rotation;
        object2.transform.localScale = target.transform.localScale;

        target.SetActive(false);

        return new GameObject[] { object1, object2 };
    }

    public static void MakeCap(Vector3 faceNormal, List<Vector3> relatedVertices,
        List<Vector3> sideCapVertices1, List<Vector3> sideCapVertices2,
        List<Vector3> sideCapNormals1, List<Vector3> sideCapNormals2,
        List<Vector2> sideCapUVs1, List<Vector2> sideCapUVs2,
        List<int> sideCapTris1, List<int> sideCapTris2)
    {
        sideCapVertices1.AddRange(relatedVertices);
        sideCapVertices2.AddRange(relatedVertices);

        if (relatedVertices.Count < 2) return;

        Vector3 center = Vector3.zero;
        foreach (Vector3 item in relatedVertices)
        {
            center += item;
        }

        center /= relatedVertices.Count;
        sideCapVertices1.Add(center);
        sideCapVertices2.Add(center);

        for (int i = 0; i < sideCapVertices1.Count; i++)
        {
            sideCapNormals1.Add(faceNormal);
            sideCapNormals2.Add(faceNormal);
        }

        Vector3 forward = Vector3.zero;
        forward.x = faceNormal.y;
        forward.y = -faceNormal.x;
        forward.z = faceNormal.z;

        Vector3 left = Vector3.Cross(forward, faceNormal);
        for (int i = 0; i < relatedVertices.Count; i++)
        {
            Vector3 dir = relatedVertices[i] - center;
            Vector2 relatedUV = Vector2.zero;
            relatedUV.x = 0.5f + Vector3.Dot(dir, left);
            relatedUV.y = 0.5f + Vector3.Dot(dir, forward);
            sideCapUVs1.Add(relatedUV);
            sideCapUVs2.Add(relatedUV);
        }

        sideCapUVs1.Add(new Vector2(0.5f, 0.5f));
        sideCapUVs2.Add(new Vector2(0.5f, 0.5f));

        int centerIdx = sideCapVertices1.Count - 1;

        float faceDir = Vector3.Dot(faceNormal,
            Vector3.Cross(relatedVertices[0] - center, relatedVertices[1] - relatedVertices[0]));

        for (int i = 0; i < sideCapVertices1.Count - 1; i++)
        {
            int idx0 = i;
            int idx1 = (i + 1) % (sideCapVertices1.Count - 1);
            if (faceDir < 0)
            {
                sideCapTris1.Add(centerIdx);
                sideCapTris1.Add(idx1);
                sideCapTris1.Add(idx0);

                sideCapTris2.Add(centerIdx);
                sideCapTris2.Add(idx0);
                sideCapTris2.Add(idx1);
            }
            else
            {
                sideCapTris1.Add(centerIdx);
                sideCapTris1.Add(idx0);
                sideCapTris1.Add(idx1);

                sideCapTris2.Add(centerIdx);
                sideCapTris2.Add(idx1);
                sideCapTris2.Add(idx0);
            }
        }
    }

    public static void SortVertices(List<Vector3> target,out List<Vector3> result)
    {
        result = new List<Vector3>();
        result.Add(target[0]);
        result.Add(target[1]);

        int vertexSetCount = target.Count / 2;

        for (int i = 0; i < vertexSetCount - 1; i++)
        {
            Vector3 vertex0 = target[i * 2];
            Vector3 vertex1 = target[i * 2 + 1];

            for (int j = i + 1; j < vertexSetCount; j++)
            {
                Vector3 cVertex0 = target[j * 2];
                Vector3 cVertex1 = target[j * 2 + 1];

                if (cVertex0 == cVertex1)
                {
                    result.Add(cVertex1);
                    SwapTwoIndexSet(target, i * 2 + 2, i * 2 + 3, j * 2, j * 2 + 1);
                }
                else if (vertex1 == cVertex1)
                {
                    result.Add(cVertex0);
                    SwapTwoIndexSet(target, i * 2 + 2, i * 2 + 3, j * 2 + 1, j * 2);
                }
            }
        }

        if (result[0] == result[result.Count - 1]) result.RemoveAt(result.Count - 1);
    }

    public static void SwapTwoIndexSet<T>(List<T> _target, int _idx00, int _idx01, int _idx10, int _idx11)
    {
        T temp0 = _target[_idx00];
        T temp1 = _target[_idx01];
        _target[_idx00] = _target[_idx10];
        _target[_idx01] = _target[_idx11];
        _target[_idx10] = temp0;
        _target[_idx11] = temp1;
    }
}