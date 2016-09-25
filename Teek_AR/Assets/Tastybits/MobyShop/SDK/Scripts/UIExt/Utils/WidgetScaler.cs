using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class WidgetScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public RectTransform rt {
		get {
			return this.GetComponent<RectTransform>();
		}
	}

	static bool updating = false;

	public bool greater=false;

	public Vector2 wannaMin = Vector2.zero;
	bool wannaSet= false;
	public Vector2 wannaMax = Vector2.zero;

	public bool GetWannaSet() {
		return wannaSet;
	}


	public float Width {
		get {
			if( wannaSet ) {
				return wannaMin.x;
			}
			return rt.offsetMin.x;
		}
		set {
			if( updating ) {
				wannaMin.x = value;
				wannaSet=true;
				return;
			}
			updating = true;
			wannaSet=false;

			var sz = rt.offsetMin;
			if( sz.x  == value ) {
				return;
			}

			greater = value > sz.x;




			var tmp = new System.Collections.Generic.List<RectTransform>();
			for( int i=0; i<rt.childCount; i++ ) {
				var chtf = rt.GetChild(i).GetComponent<RectTransform>();
				tmp.Add( chtf );
			}

			foreach( var chtf in tmp ) {
				chtf.SetParent( rt.parent );
			}

			//Debug.Log("Clear children #" + rt.childCount + " array #" + tmp.Count, this.gameObject);
			sz.x = value;
			rt.offsetMin = sz;


			#if UNITY_EDITOR
			UnityEditor.EditorApplication.CallbackFunction nextframe=null;
			int fr=0;
			nextframe = ()=>{
				if( fr++ < 2 ) {
					return;
				}
				UnityEditor.EditorApplication.update -= nextframe;

				//Debug.Log( "set parent (1)");
				/*if( tmp .Count > 0 ) {
					var chtf = tmp[0];
					if( chtf!=null ) {
						chtf.SetParent( rt );
					}
					tmp.RemoveAt(0);
				}*/
				for( int i=0; i<tmp.Count; i++ ) {
					var chtf = tmp[i];
					if( chtf!=null ) {
						chtf.SetParent( rt, true );
					}
				}
				//tmp.Clear();

				/*if( tmp.Count > 0 ) {
					fr=0;
					return;
				}*/

				updating=false;
				nextframe = null;
			};
			UnityEditor.EditorApplication.update += nextframe;
			#endif 



		}
	}

}



#if UNITY_EDITOR
[CustomEditor(typeof(WidgetScaler))]
public class WidgetScalerEditor : Editor {
	public override void OnInspectorGUI () {
		var targ = this.target as WidgetScaler;
		if( GUILayout.Button( "Fit Anchors") ) {
			/*var fitAnchors = */targ.gameObject.AddComponent<UnityEngine.UI.MobyShop.UIFitAnchors>();
		}

		var newValue = UnityEditor.EditorGUILayout.FloatField( "Width (smart size)", targ.Width );
		if( targ.GetWannaSet() ) {
			targ.Width = newValue;
		}
		else if( newValue != targ.Width ) {
			targ.Width = newValue;
		}


		var objAttach = UnityEditor.EditorGUILayout.ObjectField( "Parent this", null, typeof(RectTransform), true ) as RectTransform;
		if( objAttach!=null ) {
			objAttach.SetParent( targ.rt ); 
		}

		GUILayout.Label("children #" + targ.rt.childCount );

	}
}

#endif 