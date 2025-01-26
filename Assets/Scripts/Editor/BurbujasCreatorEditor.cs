using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BurbujasCreator))]
public class BurbujasCreatorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawPropertiesExcluding(serializedObject, "_totalBubbles");

		if (GUILayout.Button("Generate Bubbles"))
		{
			((BurbujasCreator)target).GenerateBurbujas();
			EditorUtility.SetDirty(target);
		}
		if (GUILayout.Button("Destroy Bubbles"))
		{
			((BurbujasCreator)target).DestroyAllBurbujas();
			EditorUtility.SetDirty(target);
		}
	}
}
