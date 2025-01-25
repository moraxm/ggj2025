using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private Animator _animator = null;

    public void Push()
    {
        _animator.CrossFade("Push", 0.1f);
    }

    public void Pop()
    {
        _animator.CrossFade("Pop", 0.1f);
    }
}
