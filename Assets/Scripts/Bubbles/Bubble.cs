using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    protected Animator _animator = null;
    protected Collider _collider = null;

	protected virtual void Awake()
	{
		_collider = GetComponent<Collider>();
        _animator = GetComponentInChildren<Animator>();
	}

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

	public virtual void Push()
    {
		_animator.CrossFade("Push", 0.1f);
    }

    public virtual void Pop()
    {
        _animator.CrossFade("Pop", 0.1f);
        if (_collider != null)
        {
            _collider.enabled = false;
		}
    }
}
