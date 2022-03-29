using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class SortChildren
{
	[MenuItem("GameObject/SortChildrenByName", false, 0)]

	public static void SortChildrenByName()
	{
		foreach (Transform t in Selection.transforms)
		{
			List<Transform> children = t.Cast<Transform>().ToList();
			children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
			for (int i = 0; i < children.Count; ++i)
			{
				Undo.SetTransformParent(children[i], children[i].parent, "Sort Children");
				children[i].SetSiblingIndex(i);
			}
		}
	}

}
