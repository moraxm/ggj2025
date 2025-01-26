using UnityEngine;

public class AnimatorOffset : MonoBehaviour
{
    public float Offset = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<Animator>().SetFloat("offset", Offset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
