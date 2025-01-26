public class BubbleDefective : Bubble
{
	public override void Push()
	{
		// Sin sonido
		base.Push();
		Pop();
	}

	public override void Release()
	{
		base.Release();
	}

	protected override void Pop()
	{
		// Sin sonido
		base.Pop();
	}
}
