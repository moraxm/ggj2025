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

	public Renderer BubblesSubObject = null;
	[SerializeField]
	private BurbujasCreatorBubbleInfo[] _creatorInfo = null;
	[SerializeField]
    private float _distanceBetweenBurbujas = 0.01f;
    [SerializeField]
    private List<SplineContainer> _splines = new List<SplineContainer>();
    // Set by Editor script code
    [SerializeField]
    private uint _totalBubbles = 0u;

    public uint TotalBubbles => _totalBubbles;

#if UNITY_EDITOR
    public void GenerateBurbujas()
    {
        DestroyAllBurbujas();

        uint totalPriorities = 0u;
		for (int i = 0; i < _creatorInfo.Length; ++i)
		{
			totalPriorities += _creatorInfo[i].Priority;
		}

        for (int i = 0; i < _splines.Count; ++i)
        {
            GenerateBurbujas(_splines[i], totalPriorities);
        }
    }

    public void DestroyAllBurbujas()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Bubble>() != null)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        _totalBubbles = 0u;
    }

    private void GenerateBurbujas(SplineContainer container, uint totalPriorities)
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

            Bubble bubblePrefab = ChooseBubbleToSpawn(totalPriorities);
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

    private Bubble ChooseBubbleToSpawn(uint totalPriorities)
    {
		Bubble bubblePrefab = null;
		if (_creatorInfo.Length <= 0)
        {
            return bubblePrefab;
        }

        // Choose random number in range of [0, totalPriorities)
        uint random = (uint)UnityEngine.Random.Range(0, totalPriorities - 1);
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
#endif
}
