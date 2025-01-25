using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    [SerializeField]
    private string _pushAnimationName = "Push";
	[SerializeField]
	private string _popAnimationName = "Pop";

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
		_animator.CrossFade(_pushAnimationName, 0.1f);
    }

    public virtual void Pop()
    {
        _animator.CrossFade(_popAnimationName, 0.1f);
        if (_collider != null)
        {
            _collider.enabled = false;
		}
    }
}
