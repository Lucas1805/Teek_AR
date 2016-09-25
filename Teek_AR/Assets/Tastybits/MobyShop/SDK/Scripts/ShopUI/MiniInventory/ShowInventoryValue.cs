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
	 * The ShopInventoryValue is used to pull a value of a product class like the number of 'coins' as well as 'arrows', 'lifes' and the like. 
	 * 
	 * The component takes the values of the product class and shows it in the referanced label.
	 * 
	 */
	[ExecuteInEditMode]
	public class ShowInventoryValue : MonoBehaviour {
		// product class is the most important the Show inventory value. The product Class can be something like "coins" or "lifes" in other words the category of 
		// that the product adds its values to.
		public string productClass;

		// update the value in the editmode in the editor.
		public bool updateValueInEditMode=false;

		// string defined to prepend before the value of the widget ( product class value )
		public string prependString = "";

		// you can define a string that you want to append to the value of the widget. ( product class value )
		public string appendString = "";

		// 
		public bool showShowUponClick = true;

		// you can define th
		public int showShopGroupUponClick = 0;

		// referance to a label 
		public UnityEngine.UI.Text label;

		// Set to to either number or currency.
		public MobyShop.NumberFormatter.NumberFormat formatNumber = MobyShop.NumberFormatter.NumberFormat.Number; //Currency;


		/**
		 * When the widget is configured for calling this method when clicked you can 
		 */
		public void OnWidgetClicked() {
			if (showShowUponClick) {
				MobyShop.Shop.ShowShopUI( showShopGroupUponClick );
			}	
		}


		/**
		 * Update is called once per frame
		 */
		void Update () {
			if( Application.isEditor && Application.isPlaying ==false ) {
				if(updateValueInEditMode==false){
					return;
				}
			}
			if(string.IsNullOrEmpty(productClass) ) {
				if(label!=null)label.text ="";
				Debug.LogError("Cannot Show Inventory Value since productClass was set to an empty string value", this.gameObject );
				return;
			}
			if( !MobyShop.Shop.ProductClassExists( productClass ) ) {
				string availableProductClasses = string.Join( ",", MobyShop.Shop.GetAvailableProductClasses() );
				Debug.LogError("Error Showing inventory value since productClass ("+productClass+" ) did not exists as a defined product in MobyShop; available classes: " + availableProductClasses + "\nCheck the spelling of the given product class id in the component.", this.gameObject );
				return;
			}
			int value = MobyShop.Shop.GetProductClassAmount( productClass );
			if( label!=null ) {
				label.text = prependString + MobyShop.NumberFormatter.FormatNumber( "" + value + appendString, formatNumber );
			} else {
				Debug.LogError("Cannot show product class value ( " + productClass + " ) since the label of the ShowInventoryValue was not specified", this.gameObject );
			}
		}


	}


}