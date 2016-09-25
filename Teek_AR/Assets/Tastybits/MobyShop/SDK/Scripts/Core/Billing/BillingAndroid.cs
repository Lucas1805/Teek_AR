/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop {
	

	// This class represents the native billing of the Android billing system.
	public class BillingAndroid  {
		// set this to true in order to enable extensize debugging on device.
		static bool dbg = false; 

		// referance ot the java class instance.
		static AndroidJavaClass _StoreKitCls;

		// property used internally to get the java class.
		private static AndroidJavaClass StoreKitCls {
			get {
				if(_StoreKitCls==null)
					_StoreKitCls = new AndroidJavaClass("com.tastybits.monetizeassist.StoreKit");
				return _StoreKitCls;
			}
		}



		public static string GetPriceTag( string productId ) {
			if( dbg ) Debug.Log("MobyShop: BEFORE AndroidPriceAsString : " + productId );
			string result = StoreKitCls.CallStatic<string>( "GetProductPrice", productId );
			if( dbg ) Debug.Log("MobyShop: AFTER Init result = " + result );
			return result;
		}
						

		public static void Init( string purcKey, string[] skuUnlockables, string[] skuConsumables, string callbackObjName, string delegObjectName ){
			if( dbg ) Debug.Log("MobyShop: BEFORE Init");
			StoreKitCls.CallStatic( "InitBilling", purcKey, skuUnlockables, skuConsumables, callbackObjName, delegObjectName );
			if( dbg ) Debug.Log("MobyShop: AFTER Init");
		}



		public static void RestorePurchases( string gameObjectId, string methodName, string cbid ) {
			StoreKitCls.CallStatic( "RestorePurchases", gameObjectId, methodName, cbid );
		}


		public static bool BillingSupported {
			get {
				if( dbg ) Debug.Log("MobyShop: BEFORE BillingSupported");
				var ret = 
					StoreKitCls.CallStatic<bool>( "IsBillingSupported" );
				if( dbg ) Debug.Log("MobyShop: AFTER BillingSupported");
				return ret;
			}
		}


		public static bool HasRecvProductCatalogue {
			get {
				if( dbg ) Debug.Log("MobyShop: BEFORE HasRecvProductCatalogue");
				var ret =
					StoreKitCls.CallStatic<bool>( "HasInventory" );
				if( dbg ) Debug.Log("MobyShop: AFTER HasRecvProductCatalogue");
				return ret;
			}
		}


		public static bool HasProduct( string productId ) {
			if( dbg ) Debug.Log("MobyShop: BEFORE HasProduct");
			var ret = 	
				StoreKitCls.CallStatic<bool>( "HasProduct", productId );
			if( dbg ) Debug.Log("MobyShop: AFTER HasProduct");
			return ret;
		}


		public static void BuyProduct( string productId, string cbid ){
			if( dbg ) Debug.Log("MobyShop: BEFORE BuyProduct");
			StoreKitCls.CallStatic( "BuyProduct", productId, cbid );
			if( dbg ) Debug.Log("MobyShop: AFTER BuyProduct");
		}


		public static void ShowError( string errmsg ) {
			if( dbg ) Debug.Log("MobyShop: BEFORE ShowError");
			Debug.LogError("BillingError :  " + errmsg );
			if( dbg ) Debug.Log("MobyShop: AFTER ShowError");
		}


	}

}