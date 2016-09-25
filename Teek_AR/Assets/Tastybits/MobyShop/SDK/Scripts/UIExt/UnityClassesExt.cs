using UnityEngine;
using System.Collections;

public static class UnityClassesExt {
	public static Vector3 AsVec3( this Vector2 self ){
		return new Vector3 ( self.x, self.y, 0f ); 
	}
	public static T AddMissingComponent<T>( this GameObject go ) where T:Component {
		var ret = go.GetComponent<T> ();
		if( ret == null ) {
			ret = go.AddComponent<T>();
		}
		return ret;
	}
	public static T GetComponentCompatibleWith<T>( this GameObject go ) where T:Component {
		if (go == null) {
			Debug.LogError ("Error self is null.");
			return default(T);
		}
		var tmp_c = go.GetComponent<T> ();
		if( tmp_c != null ) {
			return tmp_c;
		}
		var comps = go.GetComponents<Component>();
		foreach (var comp in comps) {
			var comp_cast = comp as T;
			if( comp_cast != null ) {
				return comp_cast;
			}
		}
		return null;
	}


	public static void FitAnchorsToCorners( this RectTransform rt ) {
		rt.offsetMin = rt.offsetMax = Vector2.zero;
		rt.anchorMin = Vector2.zero;
		rt.anchorMax = Vector2.one;
	}


	public static Vector2 GetSizeOnScreen( this RectTransform tf ){
		Vector3[] corners = new Vector3[4];
		tf.GetLocalCorners(corners);
		var dimensions = corners[2] - corners[0];
		return dimensions;
	}


}