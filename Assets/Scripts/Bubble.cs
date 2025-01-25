using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private Animator _animator = null;

    private bool _exploded = false;

    public void Push()
    {
        if (!_exploded)
        {
			_animator.CrossFade("Push", 0.1f);
		}
    }

    public void Pop()
    {
        if (!_exploded)
        {
            _animator.CrossFade("Pop", 0.1f);
			_exploded = true;
		}
    }
}
