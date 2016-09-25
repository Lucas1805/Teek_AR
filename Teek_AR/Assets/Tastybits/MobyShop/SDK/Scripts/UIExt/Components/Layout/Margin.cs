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
	[RequireComponent(typeof(RectTransform))]
	public class Margin : MonoBehaviour {
		public bool adjustInEditMode=true;

		public enum Axis {
			None = -1,
			XAxis = 0,
			YAxis = 1,
			ZAxis = 2
		}

		public enum Adjust {
			None=0,
			Advanced=1,
			XAnchorEqualsYMin,
			YAnchorEqualsXMin
		}

		public enum AdjustSettingLogic {
			None=0,
			AdjustDistToYMin,
			AdjustDistToYMax,
			AdjustDistToXMin,
			AdjustDistToXMax
		}

		[System.Serializable]
		public class AdjustSetting {
			public Axis axis = Axis.None;
			public AdjustSettingLogic adjust = AdjustSettingLogic.None; 
		}

		public bool canAdjust = false;
		public Adjust adjust = Adjust.None;
		public float factorY = 1f;
		public AdjustSetting[] adjustList = new AdjustSetting[0];


		// Same axis.
		public bool yMaxEqualsYmin = false;
		public bool yMinEqualsYmax = false;
		public bool xMaxEqualsXmin = false;
		public bool xMinEqualsXmax = false;

		// other axis.
		public bool XAnchorsEqualsYMin = false;
		public bool YAnchorsEqualsXMin = false;





		void Update () {
			canAdjust=false;
			if( yMaxEqualsYmin == true ) yMinEqualsYmax = false;
			if( xMaxEqualsXmin == true ) xMinEqualsXmax = false;
			if( Application.isEditor && Application.isPlaying && adjustInEditMode ==false) {
				return;
			}
			if( adjust == Adjust.None && adjust != Adjust.Advanced ) return;

			var rt = this.GetComponent<RectTransform>();
			if( rt == null ) {
				Debug.LogError("no RecTransform available");
				return;
			}
			canAdjust=true;

			//this.GetComponentInChildren<>(true);
				

			Vector3[] corners = new Vector3[4];
			rt.GetLocalCorners(corners);


			var size = corners[2] - corners[0];

			if( (float)size.x == 0 ) {
				if( Application.isPlaying ) 
					Debug.LogError("Error getting RT size");
				return;
			}


			if( adjust != Adjust.None ) {
				if( adjust == Adjust.XAnchorEqualsYMin ) {
					float aspect = size.y / size.x;
					float minX = rt.anchorMin.y * aspect;
					rt.anchorMin = new Vector2( minX, rt.anchorMin.y );
					rt.anchorMax = new Vector2( 1f-minX, rt.anchorMax.y );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				} else if( adjust == Adjust.YAnchorEqualsXMin ) {
					float aspect = size.x / size.y;
					float minX = rt.anchorMin.x * aspect;
					rt.anchorMin = new Vector2( rt.anchorMin.x, minX );
					rt.anchorMax = new Vector2( rt.anchorMax.x, 1f-minX );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				}
			}


			if( adjust == Adjust.Advanced ) {
				if( yMaxEqualsYmin ) {
					rt.anchorMax = new Vector2( rt.anchorMax.x, 1f-rt.anchorMin.y );
					//Debug.Log("adasd");
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				} else if( yMinEqualsYmax ) {
					rt.anchorMin = new Vector2( rt.anchorMin.x, 1f-rt.anchorMax.y );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				}

				if( xMaxEqualsXmin ) {
					rt.anchorMax = new Vector2( 1f-rt.anchorMin.x, rt.anchorMax.y );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				} else if( xMinEqualsXmax ) {
					rt.anchorMin = new Vector2( 1f-rt.anchorMax.x, rt.anchorMin.y );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				}

				if(XAnchorsEqualsYMin ) YAnchorsEqualsXMin =false;
				if( XAnchorsEqualsYMin ) {
					float aspect = size.y / size.x;
					float minX = rt.anchorMin.y * aspect;
					rt.anchorMin = new Vector2( minX, rt.anchorMin.y );
					rt.anchorMax = new Vector2( 1f-minX, rt.anchorMax.y );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				} else if( YAnchorsEqualsXMin ) {
					float aspect = size.x / size.y;
					float minX = rt.anchorMin.x * aspect;
					rt.anchorMin = new Vector2( rt.anchorMin.x, minX );
					rt.anchorMax = new Vector2( rt.anchorMax.x, 1f-minX );
					rt.offsetMin = rt.offsetMax = Vector2.zero;
				}

			} 
				

		}


	}
}
