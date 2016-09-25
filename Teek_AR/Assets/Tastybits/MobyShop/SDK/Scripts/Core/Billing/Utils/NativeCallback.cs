/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop  {


	/**
	 * @Desc NativeCallback is a way to handle the return from a Native Plugin. ( iOS or Android ) 
	 * Essentially what this does is that it creates a gameobjet with a random id and uses that as a 
	 * callback object , the idea is that you want an object per request and translate that into calling a method
	 */
	public class NativeCallback : MonoBehaviour {
		public string randomId;
		bool invoked = false;
		float timeout=-1337f;
		System.Action< System.Collections.Hashtable > deleg;

		public static string Create( System.Action< System.Collections.Hashtable > methodToCall ) {
			int rndId = Random.Range( 100000, 999999 );
			int iter = 0;
			string go_name = "NativeDelegate_" + rndId;

			while( GameObject.Find( go_name ) != null && iter++ < 100 ) {
				if( iter >= 100 ) {
					Debug.LogError("Error infinite loop might have been detected, or random funtion might not return random numbes");
				}
				rndId = Random.Range( 100000, 999999 );
				go_name = "NativeDelegate_" + rndId;
			}

			if( GameObject.Find( go_name ) != null ) {
				Debug.LogError( "Error there is allready a callback with the existing id : " + go_name );
			}

			GameObject delegRoot = GameObject.Find( "NativeCallback" );
			if( delegRoot == null ) {
				delegRoot = new GameObject( "NativeCallback");
			}

			GameObject goDeleg = new GameObject( go_name );
			goDeleg.name = go_name;
			goDeleg.transform.parent = delegRoot.transform;
			NativeCallback test = goDeleg.AddComponent<NativeCallback>() as NativeCallback;
			test.deleg = methodToCall;

			return test.name;
		}


		void Update() {
			if( !(timeout <= -1337 ) ) {
				if( timeout > 0f ) {
					timeout -= Time.fixedTime;
					if( timeout <= 0f ) {
						OnTimeout();
					}
				}
			}
		}


		void OnTimeout() {

		}


		public void CallDelegateFromNative( string strData ) {
			if( invoked ) {
				Debug.LogError( "Error the delegate is allready invoked" );
				return;
			}

			System.Collections.Hashtable retParams = MiniJSON.jsonDecode( strData ) as System.Collections.Hashtable;

			if( retParams == null ) {
				Debug.LogError( "Error parsing return data from native call based on data : " + (string.IsNullOrEmpty(strData) ? "[EMPTY STRING]" : strData ) );
				return;
			}

			deleg( retParams );
			this.enabled = false;
			this.gameObject.SetActive( false );
			GameObject.DestroyObject( this.gameObject );
		}



	}

}
