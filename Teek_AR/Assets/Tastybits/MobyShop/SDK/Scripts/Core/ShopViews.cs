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
	 * The shop views contains refrances to the prefabs and views needed for the shop to work.
	 */
	public class ShopViews : MonoBehaviour {
		static ShopViews _instance;

		// referance to the canvas in the game - its okay not to set this and the diffirent part of hte game will try and figure out how to get hold of the canvas
		// or create a canvas itself.
		public Canvas canvas;

		// the instance of the shop UI.
		public GameObject defaultShopUIInstance;

		// The prefab of the default shop UI.
		public GameObject defaultShopUIPrefab;

		// referance to he prefab UI that shows - now enough coins.
		public GameObject BillingInGameCurrencyPrefab;

		// referance to he prefab UI that shows - now enough coins.
		public GameObject PrefabNotEnoughCoins;

		// referance to the BIlling simulator UI prefab.
		public GameObject billingSimulatorUIPrefab;

		// 
		[HideInInspector]
		public SimulatedBillingUI editorBillingUI;

		// 
		[HideInInspector]
		public GameObject NotEnoughCoins;

		/**
		 *  Sets the instance of the awake method.
		 */
		void Awake() {
			_instance = this;
			if (canvas == null) {
				canvas = CanvasHelper.GetCanvas( false );
				if( canvas == null ) {
					Debug.LogError ("MobyShop: Error finding canvas for ShopViews");
				}
			}
		}

		// Returns the singleton instance.
		public static ShopViews instance {
			get {
				if( _instance == null ) {
					Debug.LogError( "MobyShop: Error getting singleton instance." );
				}
				return _instance;
			}
		}
			

	}



}

