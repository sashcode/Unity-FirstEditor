using UnityEngine;
using UnityEditor;
using System.Collections;

public class FirstEditor : EditorWindow
{
	private EditorData data;
	private bool drag = false;
	
	[MenuItem ("Window/First Editor Window")]
	static void Init ()
	{
		FirstEditor window = (FirstEditor)EditorWindow.GetWindow (typeof(FirstEditor));
	}
	
	void OnGUI ()
	{
		if (data == null) {
			GUI.Label (new Rect (20, 20, 300, 100), "no data");
			return;
		}

		Event e = Event.current;

		Undo.SetSnapshotTarget (data, "data");
		switch (e.type) {
		case EventType.KeyDown:
			{
				if (e.keyCode == KeyCode.Escape) {
					if (drag) {
						drag = false;
						Undo.RestoreSnapshot ();
					}
				}
				break;
			}
		case EventType.MouseDown:
			{
				Debug.Log ("MouseDown");
				if (data.rect.Contains (e.mousePosition)) {
					drag = true;
					Undo.CreateSnapshot ();
				}
				break;
			}
		case EventType.MouseDrag:
			{
				//Debug.Log ("MouseDrag");
				if (drag) {
					data.rect.x += e.delta.x;
					data.rect.y += e.delta.y;
					//Repaint ();
				}
				break;
			}
		case EventType.MouseUp:
			{
				if (drag) {
					drag = false;
					Undo.RegisterSnapshot ();
				}
				Debug.Log ("MouseUp");
			
				break;
			}
		case EventType.repaint:
			{
				Repaint ();
				break;
			}
		}		
		
		GUI.Box (data.rect, "");
	}
	
	void OnSelectionChange ()
	{
		GameObject gameObject = Selection.activeGameObject;
		if (!gameObject) {
			data = null;
			return;
		}
		data = gameObject.GetComponent<EditorData> ();
		
		Repaint ();
	}
	
	
}
