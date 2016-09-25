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
	 * This class is the heart of the MobyShop system.
	 * 
	 * You have to make this present in your initial scene.
	 */
	[ExecuteInEditMode]
	public class Shop : MonoBehaviour {
		// Shop Script.
		public static Shop _instance;

		// Shop Script.
		public bool _verbose=false;

		// 
		public enum BuyResponse {
			Ok = 1,
			Failed = 0,
			Cancelled = -2,
			BuyMoreCoins = -3,
			Wait = -4,
		}

		// shopConfigInst
		public ShopConfig shopConfigInst = null;

		// overrideEmbeddedShopConfig
		public bool overrideEmbeddedShopConfig = false;

		// Set this referance to an instance of a shop UI that you want to be shown per default.
		public MobyShop.UI.ShopUIBase defaultShopUIInstance {
			get {
				if (ShopViews.instance.defaultShopUIInstance == null)
					return null;
				return ShopViews.instance.defaultShopUIInstance.GetComponentCompatibleWith<MobyShop.UI.ShopUIBase>();	
			}
			set {
				ShopViews.instance.defaultShopUIInstance = value.gameObject;
			}
		}

		// Set this referace to a prefab which you want to initialize per default.
		public GameObject defaultShopUIPrefab {
			get {
				return ShopViews.instance.defaultShopUIPrefab;
			}
		}

		// Used to return the instance of the MobyShop singleon.
		public static Shop Instance {
			get { 
				if( _instance == null ) {
					var obj = Component.FindObjectOfType<MobyShop.Shop> ();
					if (obj != null) {
						_instance = obj;
					} else {
						Debug.LogError ("MobyShop Store instance is not initialized yet");
					}
				}
				return _instance;
			}
		}

		/**
		 * Makes sure to set the singleton.
		 */
		void Awake() {
			if( Application.isEditor && !Application.isPlaying ) return;
			_instance = this;
			TrySetShopConfigInst();
		}

		/**
		 * Makes sure to reset the singleton instance if we're runnin in the Unity editor.
		 */
		void OnDestroy() {
			if (_instance == this) {
				_instance = null;
			}
		}


		/**
		 * Show Default Show UI
		 */
		public static void ShowShopUI( int groupId, System.Action onDidDismiss=null ) {
			Instance.showShopUI( groupId, onDidDismiss );
		}


		/**
		 * Implements the showing of the view; if there is already an instance of the defaut view we will be able to trigger it.
		 * If there is no instance available there will be  
		 */ 
		void showShopUI( int groupId, System.Action onDidDismiss=null ) {
			if( defaultShopUIInstance == null && defaultShopUIPrefab == null ) {
				Debug.LogError( "MobyShop: Cannot show default show UI - no DefaultShopUI or DefaultShopUIPrefab is set.", this.gameObject );
			} else if (defaultShopUIInstance == null && defaultShopUIPrefab != null) {
				var instgo = GameObject.Instantiate (defaultShopUIPrefab);
				if (instgo == null) {
					Debug.LogError ("Error creatinng instance of Default ShopUI PRefab");
				}
				defaultShopUIInstance = instgo.GetComponentCompatibleWith<MobyShop.UI.ShopUIBase> ();
				var canvas = MobyShop.CanvasHelper.GetCanvas( );
				defaultShopUIInstance.transform.SetParent ( canvas.transform );
				defaultShopUIInstance.GetComponent<RectTransform> ().FitAnchorsToCorners ();
				defaultShopUIInstance.gameObject.SetActive (false);
				defaultShopUIInstance.Show( groupId, onDidDismiss );
			} else {
				defaultShopUIInstance.Show( groupId, onDidDismiss );
			}
		}


		/**
		 * GetProduct returns a ProductInfo instance based on the ProductId.
		 * The ProductInfo instance holds the configuration of a product as
		 * well has handles to getting the status of the product.
		 */
		public static ProductInfo GetProduct( string productId ) {
			return ShopConfig.GetProductByProductId( productId );
		}


		/**
		 * Returns true if MobyShop is initialized and the available products has been 
		 * downloaded from the internet.
		 */
		public static bool HasRecievedProductCatalogue {
			get {
				return Billing.HasRecvProductCatalogue;
			}
		}


		/**
		 * Returns the list of products configured in the ShopConfiguration editor.
		 */
		public static ProductInfo[] ProductList {
			get {
				return ShopConfig.ProductList;
			}
		}


		/**
		 * Initializes a session to buy a product, 
		 * The functino takes a product id, this is the id that you have configured the product to use
		 * in the product configuration editor. 
		 * a callback can be given wihch will return true or false when he product has been buyght.
		 * If you want to listen on the event emitted.
		 */
		public static void BuyProduct( string productId, System.Action<bool/*okay*/, string/*message*/, Shop.BuyResponse> callback, ShopConfirm confirmInterface, ShopNotEnoughCoins notEnoughCoinsInterface ) {
			ProductInfo product = ShopConfig.GetProductByProductId( productId );
			if( product != null ) {
				Billing.BuyProduct( product, ( bool okay, string message, Shop.BuyResponse response ) => {
					if( _instance!=null && Verbose ) Debug.Log("MobyShop: On Buy Product Result : " + okay + " msg=" + message );
					if( callback!=null ) callback( okay, message, response );
					//OnBuyProductResult();
					if( okay && OnProductBought!=null ) {
						//ProductInfo productBought = null;
						OnProductBought.Invoke( BoughtOrRestored.Bought, product, product.GetClassAmount() );
					}
				}, confirmInterface, notEnoughCoinsInterface );
			} else {
				Debug.LogError("MobyShop:Error buying products");
			}
		}


		/**
		 * Initialize a session to buy a product, simplified version of the method.
		 */
		public static void BuyProduct( string productId, System.Action<bool/*okay*/, string/*message*/, Shop.BuyResponse/*response*/> callback=null ) {
			BuyProduct (productId, callback, null, null);
		}


		/**
		 * Used to get the value of a product with a specified class, for instance if you have 3 products which should all add
		 * to the value type "gold coins" you should use this to get the value of the gold coins bought or accumulated.
		 */
		public static int GetProductClassAmount( string productClass ) {
			return ShopConfig.GetProductClassValue( productClass );
		}


		/**
		 * Return true if the product class exists, false if not.
		 * it. call it with 'coins' and if there is one or more products declared
		 * with the id 'coins' as the product class. then this will return true.
		 */ 
		public static bool ProductClassExists( string productClass ) {
			return ShopConfig.ProductClassExists( productClass );
		}


		/**
		 * Returns all the available product classes
		 */ 
		public static string[] GetAvailableProductClasses( ) {
			return ShopConfig.GetAvailableProductClasses( );
		}


		/**
		 * Restores the Unlockable purchases. 
		 * 
		 * The callback onDone is invoked when the entire process is over or when failed.
		 * The callback onRestoreProduct is called for each product restored. 
		 * Use the functions to changes to the gamestate or the game's data upon restoring old unlockable purcahses.
		 */
		public static void RestorePurchases( System.Action<bool/*success*/> onDone, System.Action<string/*ProductId*/> onRestoreProduct ) {
			Billing.RestorePurchases( ( bool ok, string msg )=>{
				if( onDone!=null ) {
					onDone(ok);
				}
			}, (string productid)=>{
				if( onRestoreProduct!=null ) {
					onRestoreProduct(productid);
				}
			} ); 
		}
			
		/**
		 * Returns whether a Prooduct has been bought or not. 
		 * Use this to determine if a product like "remove_ads" has been bought in case you want to check
		 * Before the ads are being shown.
		 */ 
		public static bool HasProductBeenBought( string productId ) {
			var product = MobyShop.ShopConfig.GetProductByProductId( productId );
			if( product == null ) {
				Debug.LogError("MobyShop:Store - Error getting product with id '" + productId + "'" );
				return false;
			}
			return product.HasBeenBought;
		}


		/**
		 * Increment the product class amount can be used to increment the value of a product class
		 * for instance if you have 3 products which gives you gold coins you might also want the player
		 * to be able to pickup gold coins in the game. You can use this to increment the value that the
		 * player pick's up in the game.
		 */
		public static void IncrementProductClassAmount( string productClass, int incrementWith ) {
			var prod = ShopConfig.GetProductByClassId( productClass );
			if( prod!=null ) {
				prod.AddValueToProductAmountAndSaveLocalData( incrementWith );
			} else {
				Debug.LogError("MobyShop: Couldn't find a Product With Class Name : " + productClass + "\nAvailable classes :" + string.Join(",",ShopConfig.ProductClasses) );
				// Error porduct doesnt exists...
			}
		}


		/**
		 * Use ClearAllPurchaseData to clear all data regarding the available purchases.
		 * We provided you with this so you can test clearing the bought items in the editor 
		 */
#if UNITY_EDITOR
		[UnityEditor.MenuItem("Window/MobyShop/Reset Products Bought")]
#endif
		public static void ClearAllPurchaseData(){
			ShopConfig.ClearPrefsForPurchases();
		}


		/**
		 * The callback OnProductBought is getting called whenever a product is bought, you can use it to listen for purchases
		 * in diffirent places of the game. 
		 * 
		 * For instance you might want to have some system in the game listen on this in order to change the state of hte game but maybe the 
		 * you don't want the code that triggers the purchase in the first to care about the details of some subsystem of your game.
		 * 
		 * We provided you with this so that you can be flexible in how you implements IAP in your game.
		 */ 
		public static System.Action< MobyShop.BoughtOrRestored, MobyShop.ProductInfo, int > OnProductBought;


		/**
		 * Retrives the billing UI object. 
		 */
		public GameObject BillingIngameCurrencyUIPrefab {
			get {
				return ShopViews.instance.BillingInGameCurrencyPrefab;
			}	 
		}


		/**
		 * Returns the UI element used for showing when we're billing with ingame currency. 
		 */
		GameObject billingIngameCurrencyUI;
		public static GameObject BillingIngameCurrentUI {
			get {
				if( _instance == null ) {
					Debug.LogError("MobyShop: Store object is not initialized. (Check that the Store instance is in the scene.)");
					return null;
				}
				var uigo = _instance.billingIngameCurrencyUI;
				if( uigo==null ) {					
					var prefab = _instance.BillingIngameCurrencyUIPrefab;
					if( prefab == null ) {
						Debug.LogError("MobyShop: The prefab of the ingame currency was not found.", _instance.gameObject );
						return null;
					}
					var canvas = CanvasHelper.GetCanvas();
					if( canvas == null ) {
						Debug.LogError("Error getting Canvas");
					}
					uigo = MobyShop.Shop.BillingIngameCurrentUI = GameObject.Instantiate( prefab );
					_instance.billingIngameCurrencyUI = uigo;
				}
				return uigo;
			}
			set {
				_instance.billingIngameCurrencyUI = value;
			}
		}


		/**
		 * EditorOnly: Makes sure it works when the editor reloads the scripts after compilation in playmode.
		 */
#if UNITY_EDITOR
		void Update(){
			_instance=this;
			TrySetShopConfigInst();
		}
#endif

		/**
		 * Sets the shop config to something that makes sense.
		 */
		void TrySetShopConfigInst() {
			if( ShopConfig._instance != shopConfigInst && overrideEmbeddedShopConfig && shopConfigInst!=null ) {
				ShopConfig._instance = shopConfigInst;
			}
		}


		/**
		 * 
		 */
		public static bool Verbose {
			get {
				if (Application.isEditor == false)
					return true;
				if (_instance == null)
					return false;
				return _instance._verbose;
			}
		}


	}


}