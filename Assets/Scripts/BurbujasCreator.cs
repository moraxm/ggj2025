using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class BurbujasCreator : MonoBehaviour
{
    [System.Serializable]
    private struct BurbujasCreatorBubbleInfo
    {
        public Bubble BubblePrefab;
        public uint Priority;
    }

    [SerializeField]
	private BurbujasCreatorBubbleInfo[] _creatorInfo = null;
    [SerializeField]
    private float _distanceBetweenBurbujas;
    [SerializeField]
    private List<SplineContainer> _splines = new List<SplineContainer>();

    private uint _totalPriorities = 0u;
    private uint _totalBubbles = 0u;

    public uint TotalBubbles => _totalBubbles;

	private void Awake()
	{
		for (int i = 0; i < _creatorInfo.Length; ++i)
		{
			_totalPriorities += _creatorInfo[i].Priority;
		}
	}

	private void Start()
    {
		for (int i = 0; i < _splines.Count; ++i)
        {
            GenerateBurbujas(_splines[i]);
        }
    }

    private void GenerateBurbujas(SplineContainer container)
    {
        BurbujaOffset burbujaOffset;
        float Offset = 0;
        if (burbujaOffset = container.GetComponent<BurbujaOffset>())
        {
            Offset = burbujaOffset.Offset;
        }

        float a = container.Spline.GetLength() / _distanceBetweenBurbujas;
        for (int i = 0; i < a; i++)
        {
            float b = _distanceBetweenBurbujas / container.Spline.GetLength();

            container.Spline.Evaluate((b * i) + Offset, out float3 pos, out _, out float3 up);

            Bubble bubblePrefab = ChooseBubbleToSpawn();
            if (bubblePrefab != null)
            {
                Bubble bubble = Instantiate(bubblePrefab, container.transform.TransformPoint(pos), Quaternion.LookRotation(up), transform);
                if (bubble != null && bubble is not BubblePopped)
                {
                    ++_totalBubbles;
				}
            }
        }
    }

    private Bubble ChooseBubbleToSpawn()
    {
		Bubble bubblePrefab = null;
		if (_creatorInfo.Length <= 0)
        {
            return bubblePrefab;
        }

        // Choose random number in range of [0, totalPriorities)
        uint random = (uint)UnityEngine.Random.Range(0, _totalPriorities - 1);
        // Loop through all creatorInfo summing the priorities.
        // When accumulated priorities are higher than the random number, that's the prefab we will use
        uint accumPriorities = 0u;
        for (int i = 0; i < _creatorInfo.Length; ++i)
        {
            accumPriorities += _creatorInfo[i].Priority;
            if (accumPriorities >= random)
            {
                bubblePrefab = _creatorInfo[i].BubblePrefab;
                break;
            }
        }

        return bubblePrefab;
    }
}
