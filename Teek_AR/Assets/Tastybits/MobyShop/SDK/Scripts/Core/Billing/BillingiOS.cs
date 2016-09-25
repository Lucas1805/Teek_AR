/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


namespace MobyShop {


	// This class represents access to Apple's StoreKit for IAP/billing.
	public class BillingiOS  {
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		public static extern void Billing_Init( string gameObjectName, string commaSeperatedProductIdList, string cb );

		[DllImport ("__Internal")]
		public static extern string Billing_GetPriceTag( string productId );

		[DllImport ("__Internal")]
		public static extern void Billing_BuyProduct( string productId, string cbid );

		[DllImport ("__Internal")]
		public static extern bool Billing_HasProduct( string productId );

		[DllImport ("__Internal")]
		public static extern bool Billing_HasReceivedProductCatalogue();

		[DllImport ("__Internal")]
		public static extern bool Billing_CanMakePayments();

		[DllImport ("__Internal")]
		public static extern void Billing_RestorePurchases( string cb );

		[DllImport ("__Internal")]
		static extern void Billing_ShowError( string cb );

		public static void ShowError( string errmsg ) {
			Billing_ShowError( errmsg );
		}
#endif
	}


}