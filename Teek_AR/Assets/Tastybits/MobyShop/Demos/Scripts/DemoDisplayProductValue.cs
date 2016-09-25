/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;

public class DemoDisplayProductValue : MonoBehaviour {
	public enum Display {
		None,
		ProductClassAmt,
		DisplayRemoveAds
	}
	public Display display = Display.None;
	public string productId;
	public string productClass;

	// Update is called once per frame
	void Update () {
		if( display == Display.None ) return;
		var text = this.GetComponent<UnityEngine.UI.Text>();
		if( text==null ) {
			return;
		}
		if( display == Display.ProductClassAmt ) {
			var val = MobyShop.Shop.GetProductClassAmount( productClass );
			text.text = ""+val; 
		} else if( display == Display.DisplayRemoveAds ) {
			text.text = MobyShop.Shop.HasProductBeenBought( productId ) ? "remove_ads bought" : ""; 
		}
	}

}
