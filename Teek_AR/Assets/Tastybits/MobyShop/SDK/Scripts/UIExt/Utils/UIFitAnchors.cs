/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace UnityEngine.UI.MobyShop {



	[ExecuteInEditMode]
	public class UIFitAnchors : MonoBehaviour {

		#if UNITY_EDITOR && UGUIUTILS
		[UnityEditor.MenuItem("uGUI Utils/Fit %g")]
		public static void uUIUtils_Fit() {
			RectTransform t = UnityEditor.Selection.activeTransform as RectTransform;
			if( t !=null ) {
				Debug.Log("Fitting anchors of " + t.name, t.gameObject );
				FitAnchors(t);
			} else {
				Debug.Log("Cannot fit anchords no rect transform selected");
			}
		}



		[UnityEditor.MenuItem("uGUI Utils/Encapsure Selected Obj")]
		public static void uUIUtils_Encapsule() {
			RectTransform t = UnityEditor.Selection.activeTransform as RectTransform;
			if( t != null ) {
				Debug.Log("Encapsure object :s" + t.name, t.gameObject );
				EncasuleObject(t);
			} else {
				Debug.Log("No RectTransform selected");
			}
		}


		static void EncasuleObject( RectTransform rt ) {
			var go = new GameObject("NewObject");
			var ntf = go.AddComponent<RectTransform>();
			ntf.SetParent( rt.parent );
			go.AddComponent<CanvasRenderer>();
			ntf.anchorMin = rt.anchorMin;
			ntf.anchorMax = rt.anchorMax;
			ntf.offsetMin = rt.offsetMin;
			ntf.offsetMax = rt.offsetMax;
			ntf.localScale = rt.localScale;
			ntf.localPosition = rt.localPosition;
			ntf.localRotation = rt.localRotation;
			var i = rt.GetSiblingIndex();
			rt.SetParent(ntf);
			ntf.SetSiblingIndex(i);
		}


		//static System.Collections.Generic.List<RectTransform> tfList = new System.Collections.Generic.List<RectTransform>();

		static RectTransform tmpContainer = null;
		static RectTransform tmpTarget = null;


		[UnityEditor.MenuItem("uGUI Utils/Move Transforms Out %t")]
		public static void uUIUtils_MoveOut() {
			RectTransform t = UnityEditor.Selection.activeTransform as RectTransform;
			if( t !=null ) {
				Debug.Log("Fitting anchors of " + t.name, t.gameObject );
				MoveTransformsUp(t);
			} else {
				Debug.Log("Cannot fit anchords no rect transform selected");
			}
		}


		[UnityEditor.MenuItem("uGUI Utils/Restore transforms %y")]
		public static void uUIUtils_restore() {
			RestoreTmpContainer();
		}


		static void MoveTransformsUp( RectTransform rt ) {
			var ptf = rt.parent as RectTransform;

			if( Application.isPlaying ) {
				Debug.LogError("can ony do this in editomode");
				return;
			}

			if( tmpContainer != null ) {
				Debug.LogError("you must reset the tmp container first");
				return;
			}

			if( rt == null || ptf == null ) {
				Debug.LogError("Cannot move transforms out");
				return;
			}

			tmpTarget = rt;

			Debug.Log("Moving transforms out of parent : " + rt.name );
			var tmp = new System.Collections.Generic.List<RectTransform>();
			for( int i=0; i<rt.childCount; i++ ) {
				var chtf = rt.GetChild(i).GetComponent<RectTransform>();
				tmp.Add( chtf );
			}

			var go_tmp = new GameObject("TmpContainer");
			var rt_tmp = go_tmp.AddComponent<RectTransform>();
			tmpContainer = rt_tmp;
			rt_tmp.SetParent(ptf);

			foreach( var chtf in tmp ) {
				chtf.SetParent( rt_tmp );
			}

			UnityEditor.EditorApplication.LockReloadAssemblies();
		}



		static void RestoreTmpContainer(  ) {
			if( Application.isPlaying ) {
				Debug.LogError("can ony do this in editomode");
				return;
			}

			if( tmpContainer == null || tmpTarget == null ) {
				Debug.LogError("you have no tmp stuff set.");
				return;
			}


			Debug.Log("Moving transforms bak into parent of parent : " + tmpTarget.name );
			var tmp = new System.Collections.Generic.List<RectTransform>();
			for( int i=0; i<tmpContainer.childCount; i++ ) {
				var chtf = tmpContainer.GetChild(i).GetComponent<RectTransform>();
				tmp.Add( chtf );
			}

			foreach( var chtf in tmp ) {
				chtf.SetParent( tmpTarget );
			}

			GameObject.DestroyImmediate(tmpContainer.gameObject);

			UnityEditor.EditorApplication.UnlockReloadAssemblies();
		}


		#endif


		static void FitAnchors( RectTransform rt ) {
			if(rt==null ) {
				return;
			}
			RectTransform t = rt;
			RectTransform pt = t.parent as RectTransform;

			bool aspectRatioEnabled = false;
			if( t.GetComponent<AspectRatioFitter>() != null ) {
				aspectRatioEnabled = t.GetComponent<AspectRatioFitter>().enabled;
				t.GetComponent<AspectRatioFitter> ().enabled = false;
			}


			if(t == null || pt == null) return;

			Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
				t.anchorMin.y + t.offsetMin.y / pt.rect.height);
			Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
				t.anchorMax.y + t.offsetMax.y / pt.rect.height);

			// anchor..
			t.anchorMin = newAnchorsMin;
			t.anchorMax = newAnchorsMax;
			t.offsetMin = t.offsetMax = new Vector2(0, 0);

			if( t.GetComponent<AspectRatioFitter>() != null ) {
				t.GetComponent<AspectRatioFitter>().enabled = aspectRatioEnabled;
			}
		}


		void Start () {
			FitAnchors( this.GetComponent<RectTransform>() );
			Object.DestroyImmediate( this );
		}
	}

}
