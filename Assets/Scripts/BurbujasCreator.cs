using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class BurbujasCreator : MonoBehaviour
{
    public GameObject BurbujaPrefab;
    public float DistanceBetweenBurbujas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float a = gameObject.GetComponent<SplineContainer>().Spline.GetLength() / DistanceBetweenBurbujas;
        for (int i = 0; i < a; i++)
        {
            float b = DistanceBetweenBurbujas / gameObject.GetComponent<SplineContainer>().Spline.GetLength();

            gameObject.GetComponent<SplineContainer>().Spline.Evaluate(b * i,out float3 pos, out float3 tangent, out float3 up);

            //Debug.DrawRay(transform.TransformPoint(pos), up, Color.red, 213809218932);
            GameObject.Instantiate(BurbujaPrefab, transform.TransformPoint(pos), Quaternion.LookRotation(up));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
