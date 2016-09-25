/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop {

	/**
	 * The Objects Bought Feedback defines a componnet that triggers some UI shown when you unlock an unlockable object
	 * in the shop.
	 */
	public class ObjectsBoughtFeedback : MonoBehaviour {
		public RectTransform p0;
		public RectTransform p1;
		public RectTransform target;
		public RectTransform unlockedText;
		public GameObject containerObject;
		public UnityEngine.UI.RawImage iconImage;
		public int state=0;
		public bool dbg = false;



		void Awake(){
			if( containerObject != null ) {
				containerObject.SetActive(false);
			} 
		}


		/**
		 * Call this to show the feedback UI graphics.
		 */
		public void Show( Texture2D iconTexture=null, float wait=0f ) {
			if (state != 0) {
				//Debug.LogError( "Wait util done showing" );
				this.gameObject.SetActive (true);
				StartCoroutine( Wait( 5f, ()=>{
					Show( iconTexture );
				} ) );
				return;
			}
			iconImage.texture = iconTexture;
			state=88;
			this.gameObject.SetActive (true);
			StartCoroutine( ShowWidget( wait, ()=> {
				//Debug.Log("On show animation ndone");
				StartCoroutine( Wait(2f,()=>{
					//Debug.Log( "done watiging");
					StartCoroutine( HideWidget( ()=> {
						//Debug.Log("Done hiding");
						state = 0;
					} ) );
				} ) );
			}) );
		}


		/**
		 * The test function is juse used to test the Widget in a closed environment.
		 */
		public void TestFunc() {
			if(state==0){
				state = 88;
				StartCoroutine( ShowWidget( 0f, ()=> {
					//Debug.Log("On show animation ndone");
					state=1;
				}) );
			}
			if (state == 1) {
				state = 89;
				StartCoroutine( HideWidget( ()=> {
					//Debug.Log("On show animation ndone");
					state=0;
				}) );
			}
		}


		/**
		 * This is a simple utility classe to wait some time and return a callback
		 */
		IEnumerator Wait( float t, System.Action cb ) {
			while (t > 0f) {
				t -= Time.deltaTime;
				yield return 0;
			}
			cb ();
		}


		/**
		 * Hardcoded animatino of showing the widget.
		 */
		IEnumerator ShowWidget( float wait, System.Action onDone ) {
			yield return new WaitForSeconds (wait);

			float t = 0f;
			containerObject.SetActive(true);
			Vector3[] corners = new Vector3[4];
			unlockedText.GetLocalCorners(corners);
			var text_elem_sz = corners[2] - corners[0];
			if( (float)text_elem_sz.x == 0 ) {
				text_elem_sz = unlockedText.sizeDelta;
			}

			unlockedText.localPosition = new Vector2 (-text_elem_sz.x, unlockedText.localPosition.y);

			bool showingText = false;
			while (t < 1f) {
				t += Time.deltaTime / 0.67f;
				if (t >= 1f) {
					t = 1f;
				}
				//Debug.Log ("Time = " + t);
				var y_val = Tween.Interpolate( Tween.TweenTypes.easeOutBounce, p0.localPosition.y, p1.localPosition.y, t );
				var lp = target.localPosition;
				lp.y = y_val;
				target.localPosition = lp;

				if (t >= 0.8f && !showingText) {
					showingText = true;
					StartCoroutine (ShowText());
				}

				if (t >= 1f) {
					break;
				}
				yield return 0;
			}

			t = 0f;




			onDone ();
		}


		/**
		 * Hardcoded animation that roll's out the text.
		 */
		IEnumerator ShowText( ) {
			float t = 0f;
			Vector3[] corners = new Vector3[4];
			unlockedText.GetLocalCorners(corners);
			var text_elem_sz = corners[2] - corners[0];
			if( (float)text_elem_sz.x == 0 ) {
				text_elem_sz = unlockedText.sizeDelta;
			}
			float x0 = -text_elem_sz.x;
			float x1 = 0f;
			while (t < 1f) {
				t += Time.deltaTime / 0.4f;
				if (t >= 1f) {
					t = 1f;
				}
				//Debug.Log ("Time = " + t);
				var x_val = Tween.Interpolate( Tween.TweenTypes.easeInSine, x0, x1, t );
				var lp = unlockedText.localPosition;
				lp.x = x_val;
				unlockedText.localPosition = lp;
				if (t >= 1f) {
					break;
				}
				yield return 0;
			}
		}


		/**
		 * Hardcoded animation that hides the text.
		 */
		IEnumerator HideText( ) {
			float t = 0f;
			Vector3[] corners = new Vector3[4];
			unlockedText.GetLocalCorners(corners);
			var text_elem_sz = corners[2] - corners[0];
			if( (float)text_elem_sz.x == 0 ) {
				text_elem_sz = unlockedText.sizeDelta;
			}
			float x0 = 0f;
			float x1 = -text_elem_sz.x;
			while (t < 1f) {
				t += Time.deltaTime / 0.1f;
				if (t >= 1f) {
					t = 1f;
				}
				//Debug.Log ("Time = " + t);
				var x_val = Tween.Interpolate( Tween.TweenTypes.easeOutSine, x0, x1, t );
				var lp = unlockedText.localPosition;
				lp.x = x_val;
				unlockedText.localPosition = lp;
				if (t >= 1f) {
					break;
				}
				yield return 0;
			}
		}


		/**
		 * Hardcoded animation that hides the widget.
		 */
		IEnumerator HideWidget( System.Action onDone ) {
			float t = 0f;
			containerObject.SetActive(true);
			Vector3[] corners = new Vector3[4];
			unlockedText.GetLocalCorners(corners);
			var text_elem_sz = corners[2] - corners[0];
			if( (float)text_elem_sz.x == 0 ) {
				text_elem_sz = unlockedText.sizeDelta;
			}


			yield return StartCoroutine( HideText() );

			unlockedText.localPosition = new Vector2 (-text_elem_sz.x, unlockedText.localPosition.y);

			bool showingText = false;
			while (t < 1f) {
				t += Time.deltaTime / 0.3f;
				if (t >= 1f) {
					t = 1f;
				}
				//Debug.Log ("Time = " + t);
				var y_val = Tween.Interpolate( Tween.TweenTypes.easeOutSine, p1.localPosition.y, p0.localPosition.y, t );
				var lp = target.localPosition;
				lp.y = y_val;
				target.localPosition = lp;

				if (t >= 0.7f && !showingText) {
					showingText = true;
					//StartCoroutine (ShowText());
				}

				if (t >= 1f) {
					containerObject.SetActive(false);
					break;
				}

				yield return 0;
			}

			onDone ();
		}


		/**
		 * Utility to test the widget
		 */
#if UNITY_EDITOR
		void OnGUI(){
			if (!dbg)
				return;
			if (GUILayout.Button ("Test Animation")) {
				TestFunc ();
			}
		}
#endif


	}

}