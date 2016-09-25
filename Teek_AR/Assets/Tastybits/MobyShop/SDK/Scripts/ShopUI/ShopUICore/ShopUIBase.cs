/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;

namespace MobyShop.UI {


	/**
	 * This defines a base class for all other Shop UI views made available by the MobyShop solution.
	 * 
	 * You can create you own show UI templates by creating a subclass of ShopUIBase - that will allow you 
	 * to 
 	 * 
	 */ 
	[RequireComponent(typeof(RectTransform))]
	public class ShopUIBase : MonoBehaviour {
		// Delegate invoked when the view is dismissed.
		public System.Action cbOnDidDismiss;

		// Enable Animations:
		public bool enableAnimations = false;

		// 
		public RectTransform clientAreaTF;

		/**
		 * This method is inteded to be invoked by the UI prefab 
		 * that shows the UI.
		 */ 
		public virtual void OnClose() {
			if( this.enableAnimations ) {
				this.gameObject.SetActive (true);
				StartCoroutine (HideViewWithAnimations (() => {
					FireDidDismiss();
				}));
			} else {
				this.gameObject.SetActive (false);
				FireDidDismiss();
			}
		}


		/**
		 * This method just shows the view but can be oerloaded to initalize variables and show the view in more advanced ways.
		 */
		public virtual void Show( int groupId, System.Action onDismissed=null ) {
			this.gameObject.SetActive( true );
			this.gameObject.GetComponent<RectTransform> ().localPosition = Vector2.zero;
			this.gameObject.GetComponent<RectTransform> ().localScale = Vector2.one;

			this.cbOnDidDismiss = onDismissed;

			if (this.enableAnimations) {
				StartCoroutine( ShowViewWithAnimations( ) );
			} 
		}


		/**
		 * Fire the dismiss event.
		 */
		protected void FireDidDismiss() {
			if( cbOnDidDismiss!=null ) {
				var tmp = cbOnDidDismiss;
				cbOnDidDismiss=null;
				tmp();
			}
		}


		/**
		 * Shows view with an animation. You can trigger animations in the enableAnimations.
		 */
		protected IEnumerator ShowViewWithAnimations( System.Action cb=null ) {
			if( clientAreaTF == null ) {
				clientAreaTF = this.GetComponent<RectTransform>();
			}
			float t = 0f;
			float dur = 1f;
			Canvas cv = null;
			var tf = this.GetComponent<RectTransform> ();
			while( tf != null && cv==null ) {
				cv = tf.GetComponent<Canvas> ();
				tf = tf.parent != null ? tf.parent.GetComponent<RectTransform> () : null;
			}
			if( cv == null ) {
				Debug.LogError( "Cannot animate object which didnt have a canvas." );
				yield break;
			}

			Vector3[] corners = new Vector3[4];
			cv.GetComponent<RectTransform>().GetLocalCorners( corners );
			var canvas_sz = corners[2] - corners[0];
			if( (float)canvas_sz.x == 0 ) {
				canvas_sz = cv.GetComponent<RectTransform>().sizeDelta;
			}

			tf = this.clientAreaTF;
			tf.gameObject.SetActive( true );
			while( t < 1f ) {
				t += Time.deltaTime / dur;
				if (t >= 1f) {
					t = 1f;
				}
				var y_val = canvas_sz.y;
				var lp = tf.localPosition;
				lp.y = Tween.Interpolate( Tween.TweenTypes.easeInOutExpo, y_val*1.25f, 0f, t );
				tf.localPosition = new Vector2( 0f, lp.y );
				yield return 0;
			}
			if (cb != null)
				cb ();
		}


		/**
		 * Hides the view with the animations.
		 */
		protected IEnumerator HideViewWithAnimations( System.Action cb=null ) {
			if( clientAreaTF == null ) {
				clientAreaTF = this.GetComponent<RectTransform>();
			}
			float t = 0f;
			float dur = 1f;
			Canvas cv = null;
			var tf = this.GetComponent<RectTransform> ();
			while( tf != null && cv==null ) {
				cv = tf.GetComponent<Canvas> ();
				tf = tf.parent != null ? tf.parent.GetComponent<RectTransform> () : null;
			} 
			if( cv == null ) {
				Debug.LogError( "Cannot animate object which didnt have a canvas." );
				yield break;
			}

			Vector3[] corners = new Vector3[4];
			cv.GetComponent<RectTransform>().GetLocalCorners( corners );
			var canvas_sz = corners[2] - corners[0];
			if( (float)canvas_sz.x == 0 ) {
				canvas_sz = cv.GetComponent<RectTransform>().sizeDelta;
			}

			tf = this.clientAreaTF;
			tf.gameObject.SetActive( true );
			while( t < 1f ) {
				t += Time.deltaTime / dur;
				if (t >= 1f) {
					t = 1f;
				}
				var y_val = canvas_sz.y;
				var lp = tf.localPosition;
				lp.y = Tween.Interpolate( Tween.TweenTypes.easeInOutExpo, 0f, y_val*1.25f, t );
				tf.localPosition = new Vector2( 0f, lp.y );
				yield return 0;
			}
			this.gameObject.SetActive (false);
			if (cb != null)
				cb ();
		}	


	}


}