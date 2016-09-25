/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


/**
 * This is part of the demo where the purchases is restored using a button.
 */
public class RestorePurchasesButton : MonoBehaviour {
	public void onButtonClicked() {

		var canvas = MobyShop.CanvasHelper.GetCanvas ();

		var go = new GameObject ("Overlay");
		var rt = go.AddComponent<RectTransform> ();

		rt.transform.SetParent (canvas.transform);
		rt.FitAnchorsToCorners ();
		var image = go.AddComponent<UnityEngine.UI.Image> ();
		var c = Color.black;
		c.a = 0.5f;
		image.color = c;

		rt.SetAsLastSibling ();

		/**
		 * If you want to restore a purchase you can use this to 
		 * the first callback is invoked when the whole procedure is done
		 * and the first one is done when the whole procedure is done first time.
		 */
		MobyShop.Shop.RestorePurchases( (bool ok) => {
			Debug.Log( "Done restoring purchases; ok="+ok);
			GameObject.Destroy( go );
		}, ( string product ) => {
			Debug.Log( "Restored product : " + product );
		});

	}
}
