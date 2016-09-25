#pragma warning disable 414
/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif 


namespace MobyShop {


	/**
	 * 
	 */
	public enum BoughtOrRestored {
		Bought, 
		Restored
	}


	/**
     * @Desc The store class is an interface to the Store implementation that is used to create
	 * A ingame shop in your game. The job of Store is to provide you with the means to call
	 * platform specific Store related functionality with a simple mechanism.
	 */
	public class Billing : MonoBehaviour {
		static Billing inst=null;

		static bool initializing = false;
		static bool initialized = false;

		private static int numPurchasesInProgress = 0;

		static System.Action<string> onRestoredProduct=null;

		// referance to the Canvas object.
		public Canvas canvas {
			get { 
				return ShopViews.instance.canvas;
			}
			set { 
				ShopViews.instance.canvas = value;
			}
		}

		// billing simulator UI
		public GameObject billingSimulatorUIPrefab {
			get { 
				return ShopViews.instance.billingSimulatorUIPrefab;
			}
		}

		[HideInInspector]
		public SimulatedBillingUI editorBillingUI;

		[HideInInspector]
		public GameObject NotEnoughCoins;


		public GameObject PrefabNotEnoughCoins {
			get { 
				return ShopViews.instance.PrefabNotEnoughCoins;
			}
		}


		static bool buyingProduct=false;


		Billing():base(){
			inst=this;
		}


		public static Billing instance {
			get {
				if( inst == null ) {
					Debug.LogError( "Error getting billing instance" );
				}
				return inst;
			}
		}


		// Returns a list of products.
		ProductInfo[] ProductList {
			get {
				return ShopConfig.ProductList;
			}
		}


		// Android purchase 
		string AndroidPurchaseLicenseBase64 {
			get {
				return ShopConfig.AndroidPurchaseLicenseBase64;
			}
		}


		// Returns if the poduct is consumable or not.
		static bool GetIsConsumeable( string id ) {
			var p = GetProductWithBillingId(id);
			if( p == null ) {
				Debug.LogError("MobyShop: Error getting product with id : " + id );
				return false;
			}
			return p.Consumeable;
		}
		

		// Product id's of a set of unlockables.
		// ONLY THE UNLOCKABELS WHICH IS REAL LIFE PURCHASES.
		// TODO RENAME PROPERTY
		string[] ProductIdsOfUnlockables {
			get {
				var ret = new System.Collections.Generic.List<string>();
				foreach( var p in inst.ProductList ) {
					if (p.billing != BillingType.RealLifeCurrency)
						continue;
					if( p.type != ProductType.Unlockable ) {
						continue;
					}
#if UNITY_IOS
					ret.Add( p.BId_iOS );
#elif UNITY_ANDROID
					ret.Add( p.BId_Android );
#else
					ret.Add( p.BId_iOS );
#endif

				}
				return ret.ToArray();
			}
		}


		/**
		 * Used to return the billing id of a product with a certain product id.
		 */ 
		static string GetBillingIdOfProductWithProductId( string productId ) {
			foreach( var p in inst.ProductList ) {
				if( p.GameId == productId ) {
					return p.BillingId;
				}
			}
			foreach( var p in inst.ProductList ) {
				if( p.BillingId == productId ) {
					Debug.LogError( "MobyShop: Could not find product with Product id : " + productId + " instead we found one with the billing id of the same type. Which we returned" );
					return p.BillingId;
				}
			}
			Debug.LogError("MobyShop: Error getting billing id of project with product id : " + productId );
			return "";
		}


		/**
		 * Used to return the product billing id list.
		 */
		static string[] ProductBillingIdList {
			get {
				var tmp = new System.Collections.Generic.List<string>();
				foreach( var t in inst.ProductList ) {
					tmp.Add( t.BillingId );
				}
				return tmp.ToArray();
			}
		}


		/**
		* Used to return the product billing id list.
		*/
		static string[] ProductBillingIdListForRealProducts {
			get {
				var tmp = new System.Collections.Generic.List<string>();
				foreach( var t in inst.ProductList ) {
					if (t.billing != BillingType.RealLifeCurrency)
						continue;
					tmp.Add( t.BillingId );
				}
				return tmp.ToArray();
			}
		}

		
		/**
		 * Used to return the product billing id.
		 */
		static ProductInfo GetProductWithBillingId( string billingId ) {
			foreach( var p in inst.ProductList ) {
				if( p.BillingId == billingId ) {
					return p;
				}
			}
			foreach( var p in inst.ProductList ) {
				if( p.GameId == billingId ) {
					Debug.LogError( "MobyShop: Error: product with product id was not found  :" + billingId + " however a product with ingame id was found" );
					return p;
				}
			}
			throw new System.Exception("MobyShop: Error getting product info with name : " + billingId );
			//return null;
		}

		/**
		 * Used to return a list of consumeable product ids.
		 * TODO RENAME TO SOMETHING tHAT INDICATES IT ONLY RETUNS THE PRODUCTS WITH REAL LIFE CURRENCY AS BILLING
		 */
		string[] ProductIdsOfConsumeables {
			get {
				var ret = new System.Collections.Generic.List<string>();
				foreach( var p in inst.ProductList ) {
					if (p.billing != BillingType.RealLifeCurrency)
						continue;
					if( p.type != ProductType.Consumeable ) {
						continue;
					}
#if UNITY_IOS
					ret.Add(p.BId_iOS);
#elif UNITY_ANDROID
					ret.Add(p.BId_Android);
#else
					ret.Add(p.BId_iOS);
#endif

				}
				return ret.ToArray();
			}
		}


		/**
		 * Returns a list of billing id's
		 */
		static string[] BillingIdList {
			get {
				var ret = new System.Collections.Generic.List<string>();
				foreach( var p in inst.ProductList ) {
					ret.Add(p.BillingId);
				}
				return ret.ToArray();
			}
		}


		/**
		 * Return s a list of product info's.
		 */
		static ProductInfo[] ProductInfoList {
			get {
				return inst.ProductList;
			}
		}


		/**
		 * Sets up all infomration used to do native billing.
		 */
		void Awake() {
			if( Application.isEditor && this.gameObject.GetComponent<BillingSimulator>() == null ) {
				this.gameObject.AddComponent<BillingSimulator>();
			}
			DontDestroyOnLoad( this.gameObject );
			inst=this;
			if( initializing == true ) {
				throw new System.Exception("MobyShop: Cannot initialize twice.");
			}
			initializing=true;
#if !UNITY_EDITOR
			Debug.Log("MobyShop: Init");
			Debug.Log("MobyShop: Base64 License Code : " + AndroidPurchaseLicenseBase64);
			Debug.Log("MobyShop: ProductId List ( Unlockables )" + string.Join(",",ProductIdsOfUnlockables) );
			Debug.Log("MobyShop: ProductId List ( Consumeables ) " + string.Join(",",ProductIdsOfConsumeables) );
			Debug.Log("MobyShop: this.Object " + this.gameObject.name );
#endif
			initialized=false;

			if( Application.isEditor || ShopConfig.instance.SimulateBillingOnDevice ) {
				BillingSimulator.Init( ( bool ok ) => {
					if( MobyShop.Shop.Verbose ) Debug.Log("MobyShop: Initialized Billing simulator");
				} );
				return;
			}
#if UNITY_IOS
			BillingiOS.Billing_Init( inst.name, string.Join(",",ProductBillingIdListForRealProducts), NativeCallback.Create( ( Hashtable res ) => {
				initializing=false;
				bool ok = res.ContainsKey("ok") ? System.Convert.ToBoolean(res["ok"]) : false;
				string msg = res.ContainsKey("msg") ? System.Convert.ToString(res["msg"]) : "";
				Debug.Log("MobyShop: Store Init Done; ok=" + ok + " msg = '"+ msg + "'" );
				if( ok ) {
					initialized=true;
				} else {
					
				}
			} ) );
			initializing=false;
			initialized=true;
#elif UNITY_ANDROID
			initializing=true;
			BillingAndroid.Init( 
				AndroidPurchaseLicenseBase64,
				ProductIdsOfUnlockables,
				ProductIdsOfConsumeables, 
				this.gameObject.name,
				NativeCallback.Create( ( Hashtable res ) => {
					initializing=false;
					bool ok = res.ContainsKey("ok") ? System.Convert.ToBoolean(res["ok"]) : false;
					string msg = res.ContainsKey("msg") ? System.Convert.ToString(res["msg"]) : "";
					Debug.Log("MobyShop: Store Init Done; ok=" + ok + " msg = '"+ msg + "'" );
					if( ok ) {
						initialized=true;
					} else {
						
					}
				} )
			);
#else
			UnsupportedPlatform();
#endif
		}


		/**
		 * Call this to unlock the products that was previously bought by the user.
		 */ 
		public static void RestorePurchases( System.Action<bool,string> cbOnDone, System.Action<string> cbOnRestoreProduct ) {
			if( Application.isEditor ) {
				inst.StartCoroutine( Wait( 5f, ()=>{
					int index=-1;
					foreach( var pi in BillingIdList ) {
						var product = GetProductInfoByBillingId( pi );
						index++;
						if( pi == null ) {
							Debug.LogError("MobyShop: Error: product id at index : " + index + " of product list is null");
							continue;
						}
						if( product.Consumeable ) {
							// ship consumeable...
						} else {
							CallUnlockProduct( BoughtOrRestored.Restored, product );
							if(cbOnRestoreProduct!=null ) {
								cbOnRestoreProduct( pi );
							}
						}
					}
					if( cbOnDone!=null ) {
						cbOnDone(true,"");
					}
				} ) );
			} else {
				if (ShopConfig.instance.SimulateBillingOnDevice) {
					if( cbOnDone!=null ) {
						cbOnDone( true, "sim" );
					}
					return;
				}
				onRestoredProduct = cbOnRestoreProduct;
#if UNITY_IOS
				BillingiOS.Billing_RestorePurchases( NativeCallback.Create( (Hashtable args)=>{
					bool ok = args.ContainsKey("ok") ? System.Convert.ToBoolean(args["ok"]) : false;
					string msg = args.ContainsKey("msg") ? System.Convert.ToString(args["msg"]) : "";
					//string pidlist = args.ContainsKey("msg") ? System.Convert.ToString(args["pidlist"]) : "";
					if( cbOnDone!=null ) {
						cbOnDone( ok, msg );
					}
					onRestoredProduct=null;
				}) );
#elif UNITY_ANDROID

				BillingAndroid.RestorePurchases( inst.gameObject.name, "AndroidNativeCallback_TransactionRestored", NativeCallback.Create( (Hashtable args)=>{
					bool ok = args.ContainsKey("ok") ? System.Convert.ToBoolean(args["ok"]) : false;
					string msg = args.ContainsKey("msg") ? System.Convert.ToString(args["msg"]) : "";
					Debug.Log("MobyShop: Restore Purchases DONE!! OK = " + ok );
					if( cbOnDone!=null ) {
						cbOnDone(ok,msg);
					}
					onRestoredProduct=null;
				}) );
#else
				UnsupportedPlatform();
#endif
			}
		}


		/**
		 * Can be used to determine if the Billing system has been initialized and if in game payments are allowed on the device.
		 */ 
		public static bool IsPaymentAvailable {
			get {
				if ( Application.isEditor ){
					return true;
				}
#if UNITY_IOS
				return BillingiOS.Billing_CanMakePayments();
#elif UNITY_ANDROID
				return BillingAndroid.BillingSupported;
#else
				UnsupportedPlatform();
				return false;
#endif
			}
		}

		
		public static string GetPriceTag( string productId ){
			string billingId = GetBillingIdOfProductWithProductId( productId );

			if( Application.isEditor ) {
				return GetProductWithBillingId( billingId ).priceTagInEditor;
			}
#if UNITY_IOS
			var ret = BillingiOS.Billing_GetPriceTag( billingId );
			ret = ret.Replace("GBP", "£").Replace("USD", "$" );
			return ret;
#elif UNITY_ANDROID
			return BillingAndroid.GetPriceTag( billingId );				
#else
			UnsupportedPlatform();
			return "";
#endif
		}

		
		public static bool DefaultHasProduct( string productId ) {
			if ( Application.isEditor ){
				return productId != null;
			} 
#if UNITY_IOS
			string billingId = GetBillingIdOfProductWithProductId( productId );
			return BillingiOS.Billing_HasProduct( billingId );
#elif UNITY_ANDROID
			string billingId = GetBillingIdOfProductWithProductId( productId );
			return BillingAndroid.HasProduct( billingId );			
#else
			UnsupportedPlatform();
			return false;
#endif
		}

		
		public static bool HasRecvProductCatalogue {
			get {
				if( Application.isEditor ) {
					BillingSimulator.DoRecvProductCatalogue( ref ShopConfig.instance.products );
					return BillingSimulator.HasRecvProductCatalogue;
				}
#if UNITY_IOS
				return BillingiOS.Billing_HasReceivedProductCatalogue();
#elif UNITY_ANDROID
				return BillingAndroid.HasRecvProductCatalogue;
#else
				UnsupportedPlatform();
				return false;
#endif	
			}
		}


		/**
		 * Shows an error message.
		 */
		public static void ShowError( string errmsg ) {
#if UNITY_IOS
			BillingiOS.ShowError( errmsg );
#elif UNITY_ANDROID
			BillingAndroid.ShowError( errmsg );
#else
			UnsupportedPlatform();
#endif
		}


		/**
		 * 
		 */
		static ProductInfo GetProductInfoByProductId( string id ) {
			var p = inst.ProductList.ToList().Find( x=>x.GameId == id );
			return p;
		}
		

		static ProductInfo GetProductInfoByBillingId( string id ) {
			var p = inst.ProductList.ToList().Find( x=>x.BillingId == id );
			return p;
		}
			

		public static void BuyProduct( ProductInfo product, System.Action<bool,string,Shop.BuyResponse> callback ) {
			BuyProduct( product, callback, null, null );
		}

	
		public static void BuyProduct( ProductInfo product, System.Action<bool,string,Shop.BuyResponse> callback, ShopConfirm shopConfirmInterface, ShopNotEnoughCoins notEnoughCoinsInterface ) {
			string billingid = product.BillingId;

			if( MobyShop.Shop.Verbose ) Debug.Log("MobyShop: Buy Product : " + product.ToString() );
			if( product.billing == BillingType.IngameCurrency ) {
				BillingUnityEditor_BuyProductInGameCurrency( product, callback, shopConfirmInterface, notEnoughCoinsInterface );
				return;
			}

			// isEditor...
			if( Application.isEditor || ShopConfig.instance.SimulateBillingOnDevice ){
				if( MobyShop.Shop.Verbose ) Debug.Log( "MobyShop: Buying product with billing simulation turned on.");
				BillingUnityEditor_BuyProductWithSimulatedBilling( product, callback );
				return;
			}

			// Has Recieved the Product catalogue.
			if( !HasRecvProductCatalogue ) {
				Debug.LogError( "MobyShop: has not recieved product catalogue yet, and products cannot be bought." );
				if( callback!=null ) callback(false, "no product catalogue recieved and you cannot make purchase", Shop.BuyResponse.Failed );
				Billing.ShowError ("Product Catalogue not recieved yet");
				return;
			}


			if( buyingProduct ) {
				Debug.LogError( "MobyShop: Store: Already buying another project");
			}

			buyingProduct = true;
			Debug.Log( "MobyShop: Store: Purchase product : " + billingid + " (init)");

#if UNITY_IOS
			if( iOS_BuyDeleg != null ) {
				Debug.LogError("MobyShop: Ignoring double puchase, wait until previous is done");
				if(callback!=null) callback(false,"Purchase already in progress",Shop.BuyResponse.Wait);
				return;
			}	

			iOS_BuyDeleg = callback;
			BillingiOS.Billing_BuyProduct( billingid, NativeCallback.Create( (Hashtable args)=>{
				bool ok = args.ContainsKey("ok") ? System.Convert.ToBoolean(args["ok"]) : false;
				string msg = args.ContainsKey("msg") ? System.Convert.ToString(args["msg"]) : "";
				
				if( ok ) {
					Debug.Log("MobyShop: Billing(iOS): ok=true; product bought : " + billingid + "; msg="+msg );
				} else {
					Debug.LogError("MobyShop: Billing(iOS): ok=false; Failed to buy product; msg="+msg );
					if( iOS_BuyDeleg!=null ) {
						var tmp = iOS_BuyDeleg;
						iOS_BuyDeleg = null; // remember to reset this one.
						tmp( false, msg, Shop.BuyResponse.Failed );
					}
				}
				/*if( callback!=null ) {
					callback(ok,msg);
				}*/
			} ) );
#elif UNITY_ANDROID
			BillingAndroid.BuyProduct( billingid, NativeCallback.Create( (Hashtable args)=>{
				bool ok = args.ContainsKey("ok") ? System.Convert.ToBoolean(args["ok"]) : false;
				string msg = args.ContainsKey("msg") ? System.Convert.ToString(args["msg"]) : "";
				Debug.Log("MobyShop: OnBuyProduct Retuned : " + ok  + " msg = " + msg );
				var prod = Billing.GetProductInfoByBillingId( billingid );
				if( prod == null ) {
					prod = product;
					Debug.LogError("MobyShop: Error cannot get product wiht billingId: " + billingid );
				}
				if( ok ) {
					if( prod == null ) {
						Debug.LogError("MobyShop:Error unable to get the product by the given billing id : "+ billingid );
					}
					CallUnlockProduct( BoughtOrRestored.Bought, prod );
				} else {
					
				}
				if(callback!=null)
					callback(ok,"",Shop.BuyResponse.Ok);
			}));
#else
			UnsupportedPlatform();
#endif
		}

			
		/**
		 * This method initializes the inapp purchase.
		 */
		static void BillingUnityEditor_BuyProductInGameCurrency( ProductInfo product, System.Action<bool,string,Shop.BuyResponse> callback, ShopConfirm shopConfirmInterface, ShopNotEnoughCoins notEnoughCoinsInterface ) {
			if( MobyShop.Shop.Verbose ) Debug.Log( "MobyShop: Buying product with virtual currency ("+product.ingameCurrencyClass+"); price="+product.PriceTag + "; currency class=" + product.ingameCurrencyClass );
			
			if( product.billing != BillingType.IngameCurrency ) {
				throw new System.Exception("MobyShop: Billing with virtual currency is only allowed for : " + product.ProductId );
			}

			// Currency Class
			if( string.IsNullOrEmpty(product.ingameCurrencyClass) )  {
				Debug.LogError("MobyShop: Error currecy class did not exist : '"+product.ingameCurrencyClass+"'");
				callback(false,"Invalid product setup - no currency class given",Shop.BuyResponse.Failed);
			}

			if( ShopConfig.GetProductByClassId( product.ingameCurrencyClass ) == null ) {
				Debug.Log("MobyShop: Warning: No Product's exists with currency class id : '" + 
					product.ingameCurrencyClass + 
					"'\nNOTE: This might be okay if you don't intent for the user to buy the currency in the game shop" );
			}

			// Debug.Log( "MobyShop: Buy product with virtual currency (coins)" );
			int amount = Mathf.Abs( Shop.GetProductClassAmount( product.ingameCurrencyClass ) );
			if( amount < Mathf.Abs(product.price) ) {
				//Debug.Log ("Not enough coins");

				if (notEnoughCoinsInterface == null) {
					var uigo = instance.NotEnoughCoins;
					if (uigo == null) {
						if (Billing.instance.PrefabNotEnoughCoins == null) {
							Debug.LogError ("MobyShop: Error 'Not Enough Coins' Dialog Prefab was not found");
							callback (false, "",Shop.BuyResponse.Cancelled);
							return;
						}
						var goNotEn = GameObject.Instantiate (Billing.instance.PrefabNotEnoughCoins);
						uigo = goNotEn;
					}
					var canvas = CanvasHelper.GetCanvas ();
					uigo.transform.localPosition = Vector2.zero;
					uigo.gameObject.SetActive (true);
					uigo.transform.parent = canvas.transform;
					var rt = uigo.GetComponent<RectTransform> ();
					rt.FitAnchorsToCorners ();
					uigo.GetComponentCompatibleWith<global::MobyShop.ShopNotEnoughCoins> ().onDismissed = ( global::MobyShop.ShopNotEnoughCoins.Dismissed dismissedWith ) => {
						var response = dismissedWith == global::MobyShop.ShopNotEnoughCoins.Dismissed.BuyMoreCoins ? Shop.BuyResponse.BuyMoreCoins : Shop.BuyResponse.Cancelled;
						callback( false, "Not enough coins available", response );
					};
					return;
				} else {
					notEnoughCoinsInterface.onDismissed = ( ShopNotEnoughCoins.Dismissed dismissedWith ) => {
						var response = dismissedWith == ShopNotEnoughCoins.Dismissed.BuyMoreCoins ? Shop.BuyResponse.BuyMoreCoins : Shop.BuyResponse.Cancelled;
						callback( false, "Not enough coins available", response );
					};
					notEnoughCoinsInterface.Show();
					return;
				}
				//return;
			} else {				
				// 
				if( shopConfirmInterface == null ) {
				
					var uigo = MobyShop.Shop.BillingIngameCurrentUI;
					if (uigo == null) {
						callback (false, "Failed to get UI confirm object.",Shop.BuyResponse.Failed);
						return;
					}
					var canvas = CanvasHelper.GetCanvas ();
					uigo.transform.localPosition = Vector2.zero;
					uigo.gameObject.SetActive (true);
					uigo.transform.parent = canvas.transform;
					var rt = uigo.GetComponent<RectTransform> ();
					rt.offsetMin = rt.offsetMax = Vector2.zero;
					rt.anchorMin = Vector2.zero;
					rt.anchorMax = Vector2.one;
					uigo.GetComponentCompatibleWith<global::ShopConfirm> ().onDismissed = ( BillingInGameCurrency.AcceptOrCancel result) => {
						//Debug.Log("OnBuyResult : " + result );
						if (result == BillingInGameCurrency.AcceptOrCancel.Accepted) {
							Shop.IncrementProductClassAmount (product.ingameCurrencyClass, -Mathf.Abs (product.price));
							CallUnlockProduct (BoughtOrRestored.Bought, product);
							callback( true, "", Shop.BuyResponse.Ok );
						} else {
							callback( false, "User cancelled", Shop.BuyResponse.Cancelled );
						}
					};
				} else {
					shopConfirmInterface.onDismissed = ( BillingInGameCurrency.AcceptOrCancel result ) => {
						if( result == BillingInGameCurrency.AcceptOrCancel.Accepted ) {
							Shop.IncrementProductClassAmount( product.ingameCurrencyClass, -Mathf.Abs (product.price) );
							CallUnlockProduct (BoughtOrRestored.Bought, product);
							callback( true, "", Shop.BuyResponse.Ok );
						} else {
							callback( false, "User cancelled", Shop.BuyResponse.Cancelled );
						}
					};
					shopConfirmInterface.Show(   );
					return;
				}

			}

		}

	
		/**
		 * This method is invoked when we initialize the billing on device.
		 */
		static void BillingUnityEditor_BuyProductWithSimulatedBilling( ProductInfo product, System.Action<bool,string,Shop.BuyResponse> callback ) {
			if (product == null ) {
				Debug.LogError ("Error product was null - cannot puchase");
			}
			if (instance == null) {
				Debug.LogError ("Error singleton instance is null");
			}
			if( instance.editorBillingUI == null ) {
				if( instance.billingSimulatorUIPrefab == null ) {
					Debug.LogError( "Error prefab is null editorBillingUIPrefab",instance.gameObject );
				}
				var goBillingSimulator = ((GameObject)GameObject.Instantiate( instance.billingSimulatorUIPrefab ));
				if( goBillingSimulator == null ) {
					Debug.LogError ("Error instanciating prefab of simulated billing UI");
				}
				if( instance.canvas == null ) {
					Debug.LogWarning("MobyShop: Warning: Canvas was null, we're autocreating one but please setup a UI canvas in the project");
					instance.canvas = CanvasHelper.GetCanvas();
				}
				if (instance.canvas == null) {
					Debug.LogError ("MobyShop: Error no instance of the canvas set");
				}
				goBillingSimulator.transform.SetParent( instance.canvas.transform );
				var rt = goBillingSimulator.GetComponent<RectTransform> ();
				if (rt == null) {
					Debug.LogError ("Error the RectTransform component was not found on the Billing UI object.");
				}
				rt.FitAnchorsToCorners ();
				instance.editorBillingUI = goBillingSimulator.GetComponent<SimulatedBillingUI>();
				if( instance.editorBillingUI == null ) {
					Debug.LogError( "MobyShop: Error cannot show billing simulator UI becaurse no instance is set." );
				}
			}
			if( BillingSimulator.instance == null ) {
				MobyShop.Shop.Instance.gameObject.AddComponent<BillingSimulator> ();
			}
			if( BillingSimulator.instance == null ) {
				Debug.LogError( "MobyShop: Error - instance of billing simulator is not present and could NOT create instance.");
			}
			BillingSimulator.instance.billingSimulatorUI = instance.editorBillingUI;
			BillingSimulator.BuyProduct( product.BillingId, ( bool _ok, string _m)=>{
				if( _ok ) {	
					CallUnlockProduct( BoughtOrRestored.Bought, product );
				} else {

				}
				callback( _ok, _m, _ok ? Shop.BuyResponse.Ok : Shop.BuyResponse.Failed );
			} );
		}


		// Callbacks... For Android...
#if UNITY_ANDROID 
		void AndroidNativeCallback_TransactionRestored( string productId ) {
			// transaction...
			string transaction = "?";
			Debug.Log("MobyShop: Transaction Restored: "+transaction + " pid="+productId );
			var product = GetProductInfoByBillingId( productId );

			//numPurchasesInProgress--; // not very sure this is a good idea since this gets called a multiple number of times.
			CallUnlockProduct( BoughtOrRestored.Restored, product );
			if( onRestoredProduct!=null ) {
				onRestoredProduct( productId );
			}
		}
#endif
		
	
#if UNITY_IOS 
	static System.Action<bool,string,Shop.BuyResponse> iOS_BuyDeleg=null;
		void iOSNativeCallback_TransactionPurchasing ( string json ) {
			Hashtable args = (Hashtable)MiniJSON.jsonDecode(json);
			if( args== null ){
				Debug.LogError("MobyShop: Panic! Error decoding JSON from string : " + json );
			}
		}


		void iOSNativeCallback_TransactionDeferred( string json ) {
			Debug.Log("MobyShop: Transaction Deferred: "+json);
			Hashtable args = (Hashtable)MiniJSON.jsonDecode(json);
			if( args== null ){
				Debug.LogError("MobyShop: Panic! Error decoding JSON from string : " + json );
			}
		}
		

		void iOSNativeCallback_TransactionRestored( string json ){
			Hashtable args = (Hashtable)MiniJSON.jsonDecode(json);
			if( args == null ){
				Debug.LogError("MobyShop: Panic! Error decoding JSON from string : " + json );
			}

			// transaction...
			string transaction = (string)args["transactinId"];
			string billingId = (string)args["productId"];
			var product = GetProductInfoByBillingId( billingId );
			Debug.Log("MobyShop: Transaction Restored: "+transaction + " pid="+billingId );

			//numPurchasesInProgress--; // not very sure this is a good idea since this gets called a multiple number of times.
			CallUnlockProduct( BoughtOrRestored.Restored, product );
			if(onRestoredProduct!=null ) {
				onRestoredProduct( billingId );
			}
		}
		

		void iOSNativeCallback_TransactionFailed( string json ){
			Hashtable args = (Hashtable)MiniJSON.jsonDecode(json);
			if( args == null ){
				Debug.LogError("MobyShop: Panic! Error decoding JSON from string : " + json );
			}

			string msg = args.ContainsKey("msg") == false ? "no message" : (string)args["msg"];
			Debug.Log("MobyShop: Transaction Failed: "+ msg );
			

			if( iOS_BuyDeleg!=null ) {
				var tmp = iOS_BuyDeleg;
				iOS_BuyDeleg = null;
				tmp(false,msg,Shop.BuyResponse.Failed);
			}


			numPurchasesInProgress--;
		}

		
		void iOSNativeCallback_TransactionCompleted( string json ){
			Hashtable args = (Hashtable)MiniJSON.jsonDecode(json);
			if( args == null ){
				Debug.LogError("MobyShop: Panic! Error decoding JSON from string : " + json );
			}

			string transaction = (string)args["transactinId"];
			string billingId = (string)args["productId"];
			
			Debug.Log("MobyShop: Transaction Completed: "+transaction + " pid=" + billingId );

			var product = GetProductInfoByBillingId(billingId);
			CallUnlockProduct( BoughtOrRestored.Bought, product );
			if( iOS_BuyDeleg!=null ) {
				var tmp = iOS_BuyDeleg;
				iOS_BuyDeleg = null;
				tmp(true,"",Shop.BuyResponse.Ok);
			}
		}
#endif
	

		static void CallUnlockProduct( BoughtOrRestored state, ProductInfo product ) {
			try {
				product.OnBought();
				if( OnProductUnlocked !=null ) OnProductUnlocked.Invoke( state, product, product.GetClassAmount() );
			}catch (System.Exception e ) {
				Debug.LogError("MobyShop: Exception unlocking product :" + e.ToString() );
			}
		}
			

		public static bool IsAwaitingPurchases(){
			return numPurchasesInProgress > 0;
		}
		
		// 
		public static System.Action<BoughtOrRestored, ProductInfo, int> OnProductUnlocked = null;
	 

		//
		static void UnsupportedPlatform(){
			string str = "MobyShop: Unsupported platform : " + Application.platform;
			Debug.LogError(str);
			//throw new UnityException( str );
		}


		static IEnumerator Wait( float dur, System.Action cb ) {
			yield return new WaitForSeconds( dur );
			cb();
		}


	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(Billing))]
	public class BillingEditor : UnityEditor.Editor {
		public override void OnInspectorGUI () {
		}
	}
	#endif 



}
