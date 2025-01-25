using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class BurbujasCreator : MonoBehaviour
{
    public GameObject BurbujaPrefab;
    public float DistanceBetweenBurbujas;
    public List<SplineContainer> Splines;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Splines.Count; ++i)
        {
            GenerateBurbujas(Splines[i]);
        }
    }

    void GenerateBurbujas(SplineContainer container)
    {
        BurbujaOffset burbujaOffset = null;
        float Offset = 0;
        if (burbujaOffset = container.GetComponent<BurbujaOffset>())
        {
            Offset = burbujaOffset.Offset;
        }

        float a = container.Spline.GetLength() / DistanceBetweenBurbujas;
        for (int i = 0; i < a; i++)
        {
            float b = DistanceBetweenBurbujas / container.Spline.GetLength();

            container.Spline.Evaluate((b * i) + Offset, out float3 pos, out float3 tangent, out float3 up);

            Debug.DrawRay(transform.TransformPoint(pos), up, Color.red, 213809218932);
            GameObject Burbuja = GameObject.Instantiate(BurbujaPrefab, container.transform.TransformPoint(pos), Quaternion.LookRotation(up), transform);
        }
    }
}
