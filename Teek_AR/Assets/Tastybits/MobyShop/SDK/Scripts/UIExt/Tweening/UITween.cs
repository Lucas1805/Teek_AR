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
	 * This component can be added to a Unity UI element with the purposes of tweening the object over time.
	 * The component will selfdestroy itself upon completiong of the animation.
	 */
	[ExecuteInEditMode]
	public class UITween : MonoBehaviour {
		public RectTransform target;
		public RectTransform tf0;
		public RectTransform tf1;
		public float _time = 0f;

		public float t_size = 1f;

		public bool updateInEditMode=false;

		public Tween.TweenTypes tweenType = Tween.TweenTypes.easeInOutSine;

		public Vector2 deltaPos;

		public Vector2 lorigin;

		public float scale0 = 0f;
		public float scale1 = 0f;


		public Vector3 finalPos;


		public System.Action<float> OnUpdateValue;



		public Vector2 originalScale = Vector2.one;

		public bool deleteWhenDone = true;


		[HideInInspector]
		public bool loriginset = false;

		public bool reset = false;


		public float duration = 1f;

		public enum AnimationTypes {
			None = 0,
			Move,
			Scale,
			MoveBezier,
			AlphaAnimation,
			AnimateValue
		}

		public AnimationTypes animType = AnimationTypes.None;



		void Awake(){
			//Debug.LogError ("Created UITWeen");
		}


		void OnEnable() {
			if (Application.isEditor && !Application.isPlaying && !updateInEditMode ) {
				return;
			}
			SetOrigin ();
		}


		void SetOrigin() {
			if( !loriginset && tf1!=null && tf0 != null ) {
				loriginset = true;
				deltaPos = tf1.position - tf0.position;
				Vector2 lp = tf1.localPosition;
				lorigin = lp - deltaPos;
				originalScale = this.GetComponent<RectTransform> ().localScale;
			}
		}


		// Update is called once per frame
		void Update () {
			if (Application.isEditor && !Application.isPlaying && updateInEditMode ) {
				if (reset) {
					loriginset = false;
					SetOrigin ();
					reset = false;
				}
				if (t_size < 1f)
					t_size = 1f; 
				if (t_size > 1000f)
					t_size = 1000f;
				_time = Mathf.Max(0f, Mathf.Min(t_size, _time));
				SetValues ();
			}
		}


		void BezierPoints( out Vector2 p0, out Vector2 p1, out Vector2 p2, out Vector2 p3 ) {
			Vector2 lp = lorigin + deltaPos;
			Vector2 dp = lorigin;
			float dx = lp.x - dp.x;
			float dy = lp.y - dp.y;

			Vector2 n1 = (( new Vector2 (-dy, dx) - ( new Vector2 (-deltaPos.x, -deltaPos.y) ) / 2f));
			n1 = n1.normalized * (n1.magnitude / 4f);
			n1 = new Vector2 (-n1.x, -n1.y);


			dx = dp.x - lp.x; 
			dy = dp.y - lp.y;
			var n2 = (new Vector2 (dy, -dx)) - ((new Vector2 (deltaPos.x, deltaPos.y)  / 2f) );
			n2 = n2.normalized * (n2.magnitude / 4f);
			n2 = new Vector2 (n2.x, n2.y);

			// 
			p0 = new Vector2 (-deltaPos.x, -deltaPos.y);
			p1 = p0 - n2;
			p3 = Vector2.zero;
			p2 = n1;
		}


		void OnDrawGizmos() {
			Gizmos.color = Color.blue;
			if (animType == AnimationTypes.MoveBezier) {
				Vector2 p0,p1,p2,p3;
				BezierPoints (out p0, out p1, out p2, out p3);

				var p = this.transform.position + new Vector3( 0, 0, -20 );


				Gizmos.color = Color.yellow;
				Gizmos.DrawLine ( p + p0.AsVec3(), p + p1.AsVec3() );

				Gizmos.color = Color.cyan;
				Gizmos.DrawLine ( p + p2.AsVec3(), p + p3.AsVec3() );

				Gizmos.color = Color.gray;;
				Gizmos.DrawLine ( p + p0.AsVec3(), p + p3.AsVec3() );



				float step = 0.05f;
				float t1 = 0f;
				float mx = 1f;
				int it = 0;
				float g = 1f;
				float b = 0f;
				for( float t0 = 0f; t1 < mx;  ) {
					t0 = t1;
					t1 += step;
					if( t1 >= mx || t0+step > mx ) t1 = mx;

					b = t0;
					g = 1f - b;
					Gizmos.color = new Color (0f, g, b ,1f );

					//Debug.Log ( "#" + it + "t0=" + t0 + " t1=" + t1);
					var rp0 = Tween.GetBezier2D2( p0, p1, p2, p3, t0 );
					var rp1 = Tween.GetBezier2D2( p0, p1, p2, p3, t1 );

					// 
					Gizmos.DrawLine( p + rp0.AsVec3(), p + rp1.AsVec3() ); 
					t0 = t1;
					it++;
				}

				//Gizmos.DrawLine( p, this.transform.position + new Vector3( -n1.x, -n1.y, -20) ); 
				//UnityEditor.iOS.Xcode.PBXProject asd;

			} else if (animType == AnimationTypes.Scale) {
				
			}
		}

						
		void SetValues() {
			if (target == null)
				target = tf1;
			if (target == null)
				target = this.GetComponent<RectTransform> ();
			float tmp_t = _time / t_size;
			if (tmp_t > 1f) {
				tmp_t = 1f;
			}
			if (animType == AnimationTypes.Move) {
				var diff = deltaPos * Tween.Interpolate (tweenType, 0f, 1f, tmp_t);
				Vector2 lp = lorigin + diff;
				//Debug.Log ("Set local pos = " + tf1.localPosition);
				target.localPosition = lp;
			} else if (animType == AnimationTypes.Scale) {
				var s = Tween.Interpolate (tweenType, scale0, scale1, tmp_t);
				target.localScale = new Vector2 (s, s);
			} else if (animType == AnimationTypes.MoveBezier) {
				Vector2 p0, p1, p2, p3;
				BezierPoints (out p0, out p1, out p2, out p3);
				var lp = lorigin + deltaPos + Tween.GetBezier2D2 (p0, p1, p2, p3, tmp_t);
				target.localPosition = lp;
				if (tmp_t >= 1f) {
					target.position = finalPos;
				}
			} else if (animType == AnimationTypes.AlphaAnimation) {
				var a = Tween.Interpolate (tweenType, scale0, scale1, tmp_t);
				target.gameObject.AddMissingComponent<CanvasGroup> ().alpha = a;
			} else if (animType == AnimationTypes.AnimateValue) {
				var v = Tween.Interpolate (tweenType, scale0, scale1, tmp_t);
				OnUpdateValue (v);
			}
		}


		IEnumerator StartAnimationImpl( float wait=0f,System.Action cb=null ) {
			if (wait > 0f) {
				yield return new WaitForSeconds (wait);
			}
			float dur = this.duration;
			if( dur < 0.001f )
				dur = 0.001f;
			_time = 0f;
			t_size = 1f;
			while( _time < 1f ) {
				_time += Time.deltaTime / dur;
				SetValues();
				if( _time >= 1f ) {
					_time = 1f;
				}
				yield return 0;
			}
			//Debug.Log ("Animation done");
			if( deleteWhenDone ) 
				Component.Destroy (this);
			if (cb != null) {
				cb ();
			}
		}


		public void StartAnimation( RectTransform tf0, RectTransform tf1, float dur, System.Action cb=null ) {
			this.reset = true;
			this.duration = dur;
			this.tf0 = tf0;
			this.tf1 = tf1;
			SetOrigin ();
			this.enabled = true;
			this.animType = AnimationTypes.Move;
			//Debug.Break ();
			StartCoroutine( StartAnimationImpl(0f,cb) );
		}


		public static void startBezierAnimation( UnityEngine.UI.Graphic gfx, RectTransform target, RectTransform tf0, RectTransform tf1, float dur,  System.Action cb=null, Tween.TweenTypes tweenType = Tween.TweenTypes.easeInOutSine ) {
			gfx.gameObject.AddComponent<UITween> ().StartBezierAnimation (target, tf0, tf1, dur, cb, tweenType);
		}


		public void StartBezierAnimation( RectTransform target, RectTransform tf0, RectTransform tf1, float dur,  System.Action cb=null, Tween.TweenTypes tweenType = Tween.TweenTypes.easeInOutSine ) {
			this.reset = true;
			this.tweenType = tweenType;
			this.duration = dur;
			this.target = target;
			this.tf0 = tf0;
			this.tf1 = tf1;
			finalPos = tf1.position;
			SetOrigin ();
			this.enabled = true;
			this.animType = AnimationTypes.MoveBezier;
			//Debug.Break ();
			StartCoroutine( StartAnimationImpl(0f,cb) );
		}


		public static void startAlphaAnimation( UnityEngine.UI.Graphic gfx, float v0, float v1, float dur, float wait, Tween.TweenTypes tweenTypes, System.Action cb=null ) {
			gfx.gameObject.AddComponent<UITween> ().StartAlphaAnimation (v0, v1, dur, wait, tweenTypes, cb);
		}


		public static void startAlphaAnimation( GameObject go, float v0, float v1, float dur, float wait, Tween.TweenTypes tweenTypes, System.Action cb=null ) {
			go.AddComponent<UITween> ().StartAlphaAnimation (v0, v1, dur, wait, tweenTypes, cb );
		}


		public void StartAlphaAnimation( float v0, float v1, float _duration, float wait, Tween.TweenTypes _tweenType, System.Action cb=null ) {
			this.reset = true;
			this.tweenType = _tweenType;
			this.duration = _duration;
			this.scale0 = v0;
			this.scale1 = v1;
			this.tf1 = this.GetComponent<RectTransform> ();
			this.tf1.gameObject.AddMissingComponent<CanvasGroup>().alpha = v0;
			this.animType = AnimationTypes.AlphaAnimation;
			SetOrigin ();
			this.enabled = true;
			this.gameObject.SetActive(true);
			StartCoroutine( StartAnimationImpl(wait,cb) );
		}
			

		public static void StartAnimateValue( UnityEngine.UI.Graphic gfx, float v0, float v1, float dur, Tween.TweenTypes tweenType, System.Action onBegin, System.Action<float> onUpdate, System.Action onEnd ) {
			gfx.gameObject.AddComponent<UITween> ()._StartAnimateValue (v0, v1, dur, 0f, tweenType, onBegin, onUpdate, onEnd);
		}

		public static void StartAnimateValue( UnityEngine.UI.Graphic gfx, float v0, float v1, float dur, float wait, Tween.TweenTypes tweenType, System.Action onBegin, System.Action<float> onUpdate, System.Action onEnd ) {
			gfx.gameObject.AddComponent<UITween> ()._StartAnimateValue (v0, v1, dur, wait, tweenType, onBegin, onUpdate, onEnd);
		}


		public void _StartAnimateValue( float v0, float v1, float dur, float wait, Tween.TweenTypes tweenType, System.Action onBegin, System.Action<float> onUpdate, System.Action onEnd ) {
			this.reset = true;
			this.tweenType = tweenType;
			this.duration = dur;
			this.scale0 = v0;
			this.scale1 = v1;
			this.tf1 = this.GetComponent<RectTransform> ();
			this.animType = AnimationTypes.AnimateValue;
			OnUpdateValue = onUpdate;
			SetOrigin ();
			this.enabled = true;
			onBegin ();
			StartCoroutine( StartAnimationImpl(wait,onEnd) );
		}


		public static void startScaleAnimation( RectTransform rt, float scale0, float scale1, float dur, Tween.TweenTypes tweenTypes, System.Action cb=null ) {
			rt.gameObject.AddComponent<UITween> ().StartScaleAnimation (scale0, scale1, dur, 0f, tweenTypes, cb);
		}


		public static void startScaleAnimation( UnityEngine.UI.Graphic gfx, float scale0, float scale1, float dur, float wait, Tween.TweenTypes tweenTypes, System.Action cb=null ) {
			var rt = gfx.gameObject.GetComponent<RectTransform> ();
			rt.gameObject.AddComponent<UITween> ().StartScaleAnimation (scale0, scale1, dur, wait, tweenTypes, cb);
		}


		public void StartScaleAnimation( float scale0, float scale1, float dur, float wait, Tween.TweenTypes tweenType = Tween.TweenTypes.easeInOutSine, System.Action cb=null ) {
			//Debug.Break ();
			this.reset = true;
			this.tweenType = tweenType;
			this.duration = dur;
			this.scale0 = scale0;
			this.scale1 = scale1;
			this.tf1 = this.GetComponent<RectTransform> ();
			this.animType = AnimationTypes.Scale;
			SetOrigin ();
			this.enabled = true;
			StartCoroutine( StartAnimationImpl(wait,cb) );
		}

	}


}