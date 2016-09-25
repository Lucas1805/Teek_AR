/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop {


	public class Tween {
		public enum TweenTypes {
			Linear = 0,
			Lerp,
			SmoothStep,
			Spring,
			easeInQuad,
			easeOutQuad, 
			easeInOutQuad,
			easeInCubic,
			easeOutCubic,
			easeInQuart,
			easeOutQuart,
			easeInOutQuart, 
			easeInSine, 
			easeOutSine,
			easeInOutSine, 
			easeInExpo, 
			easeOutExpo,
			easeInOutExpo, 
			easeInCirc, 
			easeOutCirc, 
			easeInOutCirc, 
			easeInBounce,
			easeOutBounce,
			easeInBack,
			easeOutBack,
			punch,
			EaseInOutElastic,
			EaseInElastic,
			EaseOutElastic
		}

		public static float Interpolate( TweenTypes tween, float v0, float v1, float t ) {
			switch( tween ) {
			case TweenTypes.Linear: return Linear(v0,v1,t);
			case TweenTypes.Lerp: return Lerp(v0,v1,t );
			case TweenTypes.SmoothStep: return SmoothStep(v0,v1,t);
			case TweenTypes.Spring: return spring(v0,v1,t);
			case TweenTypes.easeInQuad: return easeInQuad( v0, v1, t );
			case TweenTypes.easeOutQuad: return easeOutQuad( v0, v1, t );
			case TweenTypes.easeInOutQuad: return easeInOutQuad( v0, v1, t );
			case TweenTypes.easeInCubic: return easeInCubic( v0, v1, t );
			case TweenTypes.easeOutCubic: return easeOutCubic( v0, v1, t );
			case TweenTypes.easeInQuart: return easeInQuart( v0, v1, t );
			case TweenTypes.easeOutQuart: return easeOutQuart( v0, v1, t );
			case TweenTypes.easeInSine: return easeInSine( v0, v1, t );
			case TweenTypes.easeOutSine: return easeOutSine(v0,v1,t);
			case TweenTypes.easeInOutSine: return easeInOutSine( v0, v1, t );
			case TweenTypes.easeInExpo: return easeInExpo(v0,v1,t);
			case TweenTypes.easeOutExpo: return easeOutExpo(v0,v1,t);
			case TweenTypes.easeInOutExpo: return easeInOutExpo(v0,v1,t);
			case TweenTypes.easeInCirc: return easeInCirc(v0,v1,t);
			case TweenTypes.easeOutCirc: return easeOutCirc(v0,v1,t);
			case TweenTypes.easeInOutCirc: return easeInOutCirc(v0,v1,t);
			case TweenTypes.easeInBounce: return easeInBounce(v0,v1,t);
			case TweenTypes.easeOutBounce: return easeOutBounce(v0,v1,t);
			case TweenTypes.easeInBack: return easeInBack(v0,v1,t);
			case TweenTypes.easeOutBack: return easeOutBack(v0,v1,t);
			case TweenTypes.punch: return punch(v0,v1,t);
			case TweenTypes.EaseInOutElastic: return EaseInOutElastic(v0,v1,t);
			case TweenTypes.EaseInElastic: return EaseInElastic(v0,v1,t);
			case TweenTypes.EaseOutElastic: return EaseOutElastic(v0,v1,t);
			}
			throw new System.Exception("Not implemneted :  " + tween );
		}


		// evaluate a point on a bezier-curve. t goes from 0 to 1.0
		public static Vector2 Bezier2D( Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t ) {
			Vector2 ab,bc,cd,abbc,bccd;
			ab = Vector2.Lerp( a,b,t);           // point between a and b (green)
			bc = Vector2.Lerp( b,c,t);           // point between b and c (green)
			cd = Vector2.Lerp( c,d,t);           // point between c and d (green)
			abbc = Vector2.Lerp( ab,bc,t);       // point between ab and bc (blue)
			bccd = Vector2.Lerp( bc,cd,t);       // point between bc and cd (blue)
			return Vector2.Lerp( abbc,bccd,t);   // point on the bezier-curve (black)
		}


		public static float Linear(float start, float end, float value){
			return Mathf.Lerp(start, end, value);
		}


		public static float clerp(float start, float end, float value){
			float min = 0.0f;
			float max = 360.0f;
			float half = Mathf.Abs((max - min) * 0.5f);
			float retval = 0.0f;
			float diff = 0.0f;
			if ((end - start) < -half){
				diff = ((max - start) + end) * value;
				retval = start + diff;
			}else if ((end - start) > half){
				diff = -((max - end) + start) * value;
				retval = start + diff;
			}else retval = start + (end - start) * value;
			return retval;
		}


		public static float spring(float start, float end, float value){
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
			return start + (end - start) * value;
		}


		public static float easeInQuad(float start, float end, float value){
			end -= start;
			return end * value * value + start;
		}


		public static float easeOutQuad(float start, float end, float value){
			end -= start;
			return -end * value * (value - 2) + start;
		}


		public static float easeInOutQuad(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end * 0.5f * value * value + start;
			value--;
			return -end * 0.5f * (value * (value - 2) - 1) + start;
		}


		public static float easeInCubic(float start, float end, float value){
			end -= start;
			return end * value * value * value + start;
		}


		public static float easeOutCubic(float start, float end, float value){
			value--;
			end -= start;
			return end * (value * value * value + 1) + start;
		}


		public static float easeInOutCubic(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end * 0.5f * value * value * value + start;
			value -= 2;
			return end * 0.5f * (value * value * value + 2) + start;
		}


		public static float easeInQuart(float start, float end, float value){
			end -= start;
			return end * value * value * value * value + start;
		}


		public static float easeOutQuart(float start, float end, float value){
			value--;
			end -= start;
			return -end * (value * value * value * value - 1) + start;
		}


		public static float easeInOutQuart(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end * 0.5f * value * value * value * value + start;
			value -= 2;
			return -end * 0.5f * (value * value * value * value - 2) + start;
		}


		public static float easeInQuint(float start, float end, float value){
			end -= start;
			return end * value * value * value * value * value + start;
		}


		public static float easeOutQuint(float start, float end, float value){
			value--;
			end -= start;
			return end * (value * value * value * value * value + 1) + start;
		}


		public static float easeInOutQuint(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end * 0.5f * value * value * value * value * value + start;
			value -= 2;
			return end * 0.5f * (value * value * value * value * value + 2) + start;
		}


		public static float easeInSine(float start, float end, float value){
			end -= start;
			return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
		}


		public static float easeOutSine(float start, float end, float value){
			end -= start;
			return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
		}


		public static float easeInOutSine(float start, float end, float value){
			end -= start;
			return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
		}


		public static float easeInExpo(float start, float end, float value){
			end -= start;
			return end * Mathf.Pow(2, 10 * (value - 1)) + start;
		}


		public static float easeOutExpo(float start, float end, float value){
			end -= start;
			return end * (-Mathf.Pow(2, -10 * value ) + 1) + start;
		}


		public static float easeInOutExpo(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
			value--;
			return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
		}


		public static float easeInCirc(float start, float end, float value){
			end -= start;
			return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
		}


		public static float easeOutCirc(float start, float end, float value){
			value--;
			end -= start;
			return end * Mathf.Sqrt(1 - value * value) + start;
		}


		public static float easeInOutCirc(float start, float end, float value){
			value /= .5f;
			end -= start;
			if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
			value -= 2;
			return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
		}


		public static float easeInBounce(float start, float end, float value){
			end -= start;
			float d = 1f;
			return end - easeOutBounce(0, end, d-value) + start;
		}


		public static float easeOutBounce(float start, float end, float value){
			value /= 1f;
			end -= start;
			if (value < (1 / 2.75f)){
				return end * (7.5625f * value * value) + start;
			}else if (value < (2 / 2.75f)){
				value -= (1.5f / 2.75f);
				return end * (7.5625f * (value) * value + .75f) + start;
			}else if (value < (2.5 / 2.75)){
				value -= (2.25f / 2.75f);
				return end * (7.5625f * (value) * value + .9375f) + start;
			}else{
				value -= (2.625f / 2.75f);
				return end * (7.5625f * (value) * value + .984375f) + start;
			}
		}


		public static float easeInOutBounce(float start, float end, float value){
			end -= start;
			float d = 1f;
			if (value < d* 0.5f) return easeInBounce(0, end, value*2) * 0.5f + start;
			else return easeOutBounce(0, end, value*2-d) * 0.5f + end*0.5f + start;
		}


		public static float easeInBack(float start, float end, float value){
			end -= start;
			value /= 1;
			float s = 1.70158f;
			return end * (value) * value * ((s + 1) * value - s) + start;
		}


		public static float easeOutBack(float start, float end, float value){
			float s = 1.70158f;
			end -= start;
			value = (value) - 1;
			return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
		}


		public static float easeInOutBack(float start, float end, float value){
			float s = 1.70158f;
			end -= start;
			value /= .5f;
			if ((value) < 1){
				s *= (1.525f);
				return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
			}
			value -= 2;
			s *= (1.525f);
			return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
		}


		public static float punch(float start, float amplitude, float value){
			float s = 9;
			if (value == 0){
				return 0;
			}
			else if (value == 1){
				return 0;
			}
			float period = 1 * 0.3f;
			s = period / (2 * Mathf.PI) * Mathf.Asin(0);
			return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
		}


		public static float EaseInOutElastic(float start, float end, float value){
			end -= start;

			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;

			if (value == 0) return start;

			if ((value /= d*0.5f) == 2) return start + end;

			if (a == 0f || a < Mathf.Abs(end)){
				a = end;
				s = p / 4;
			}else{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
			return a * Mathf.Pow(2, -10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
		}	

		public static float SmoothStep(float start, float end, float value){
			return Mathf.SmoothStep( start, end, value );
		}	


		public static float Lerp( float start, float end, float t ) {
			return Mathf.Lerp( start, end, t );	
		}


		public static float EaseInElastic(float start, float end, float value){
			end -= start;

			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;

			if (value == 0) return start;

			if ((value /= d) == 1) return start + end;

			if (a == 0f || a < Mathf.Abs(end)){
				a = end;
				s = p / 4;
			}else{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return -(a * Mathf.Pow(2, 10 * (value-=1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
		}		


		public static float EaseOutElastic(float start, float end, float value){
			/* GFX47 MOD END */
			//Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
			end -= start;

			float d = 1f;
			float p = d * .3f;
			float s = 0;
			float a = 0;

			if (value == 0) return start;

			if ((value /= d) == 1) return start + end;

			if (a == 0f || a < Mathf.Abs(end)){
				a = end;
				s = p * 0.25f;
			}else{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
		}	

		/**********************************************************************************************************
		 * 
		 **********************************************************************************************************/

		public static float Hermite(float start, float end, float value) {
			return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
		}

		public static float Sinerp(float start, float end, float value) {
			return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
		}

		public static float Coserp(float start, float end, float value) {
			return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
		}

		public static Vector3 CubicBezier (Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3, float t) {
			t = Mathf.Clamp01 (t);
			float t2 = 1-t;
			return Mathf.Pow(t2,3) * p0 + 3 * Mathf.Pow(t2,2) * t * p1 + 3 * t2 * Mathf.Pow(t,2) * p2 + Mathf.Pow(t,3) * p3;
		}


		public static Vector2 GetBezier2D( Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t ) {
			float u = 1f - t;
			float tt = t*t;
			float uu = u*u;
			float uuu = uu * u;
			float ttt = tt * t;

			Vector2 p = uuu * p0; //first term
			p += 3f * uu * t * p1; //second term
			p += 3f * u * tt * p2; //third term
			p += ttt * p3; //fourth term

			return p;
		}


		static public Vector2 GetBezier2D2( Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t) {

			float cx = 3f * (p1.x - p0.x);
			float bx = 3f * (p2.x - p1.x) - cx;
			float ax = p3.x - p0.x - cx - bx;

			float cy = 3f * (p1.y - p0.y);
			float _by = 3f * (p2.y - p1.y) - cy;
			float ay = p3.y - p0.y - cy - _by;

			float tSquared = t * t;
			float tCubed = tSquared * t;
			float resultX = (ax * tCubed) + (bx * tSquared) + (cx * t) + p0.x;
			float resultY = (ay * tCubed) + (_by * tSquared) + (cy * t) + p0.y;
			return new Vector2( resultX, resultY );
		}



		public static Vector2 Bezier2D3( Vector2 a, Vector2 b, Vector2 c, float t ) {
			var ab = Vector2.Lerp(a,b,t);
			var bc = Vector2.Lerp(b,c,t);
			return Vector2.Lerp(ab,bc,t);
		}


		public static float Berp(float start, float end, float value) {
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
			return start + (end - start) * value;
		}

		/*public static float SmoothStep (float x, float min, float max)  {
			x = Mathf.Clamp (x, min, max);
			float v1 = (x-min)/(max-min);
			float v2 = (x-min)/(max-min);
			return -2*v1 * v1 *v1 + 3*v2 * v2;
		}

		public static float Lerp(float start, float end, float value) {
			return ((1.0f - value) * start) + (value * end);
		}*/

		public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			Vector3 lineDirection = Vector3.Normalize(lineEnd-lineStart);
			float closestPoint = Vector3.Dot((point-lineStart),lineDirection)/Vector3.Dot(lineDirection,lineDirection);
			return lineStart+(closestPoint*lineDirection);
		}

		public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			Vector3 fullDirection = lineEnd-lineStart;
			Vector3 lineDirection = Vector3.Normalize(fullDirection);
			float closestPoint = Vector3.Dot((point-lineStart),lineDirection)/Vector3.Dot(lineDirection,lineDirection);
			return lineStart+(Mathf.Clamp(closestPoint,0.0f,Vector3.Magnitude(fullDirection))*lineDirection);
		}


		public static float Bounce(float x) {
			return Mathf.Abs(Mathf.Sin(6.28f*(x+1f)*(x+1f)) * (1f-x));
		}

		// test for value that is near specified float (due to floating point inprecision)
		// all thanks to Opless for this!
		public static bool Approx(float val, float about, float range) {
			return ( ( Mathf.Abs(val - about) < range) );
		}

		// test if a Vector3 is close to another Vector3 (due to floating point inprecision)
		// compares the square of the distance to the square of the range as this 
		// avoids calculating a square root which is much slower than squaring the range
		public static bool Approx(Vector3 val, Vector3 about, float range) {
			return ( (val - about).sqrMagnitude < range*range);
		}

		/*
		 * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
		 * This is useful when interpolating eulerAngles and the object
		 * crosses the 0/360 boundary.  The standard Lerp function causes the object
		 * to rotate in the wrong direction and looks stupid. Clerp fixes that.
		 */
		public static float Clerp(float start , float end, float value){
			float min = 0.0f;
			float max = 360.0f;
			float half = Mathf.Abs((max - min)/2.0f);//half the distance between min and max
			float retval = 0.0f;
			float diff = 0.0f;

			if((end - start) < -half){
				diff = ((max - start)+end)*value;
				retval =  start+diff;
			}
			else if((end - start) > half){
				diff = -((max - end)+start)*value;
				retval =  start+diff;
			}
			else retval =  start+(end-start)*value;

			// Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
			return retval;
		}

	}


}