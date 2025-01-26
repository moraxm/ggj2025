using UnityEngine;

public class AutoKill : MonoBehaviour
{
    public float KillTime = 1.0f;

    private void Start()
    {
        Destroy(gameObject, KillTime);
    }
}
