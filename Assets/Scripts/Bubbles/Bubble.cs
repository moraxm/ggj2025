using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    [SerializeField]
    private string _pushAnimationName = "Push";

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

    public virtual void Release()
    {

    }

    public virtual void Pop()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
		}
        GameManager.Instance.OnNotifyPoppedBubble();
    }
}
