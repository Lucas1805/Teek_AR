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
	 * This class represents billing in the Unity editor
	 * it works to simulate billing on iOS or Android.
	 */
	public class BillingSimulator : MonoBehaviour {
		static BillingSimulator inst=null;
		public SimulatedBillingUI billingSimulatorUI;

		/**
		 * Private constructor. 
		 */
		BillingSimulator():base(){
			inst=this;
		}


		/**
		 * Property to return the singleton instance.
		 */
		public static BillingSimulator instance {
			get {
				return inst;
			}
		}


		/**
		 * 
		 */
		void Awake() {
			inst=this;
		}


		/**
		 * Support recompile in Editor.
		 */
		void Update() {
			inst=this;
		}


		// Holds a static list of productInfo's.
		static ProductInfo[] productInfoList;


		/**
		 * Simulate the retrieval of the objects.
		 */
		public static void DoRecvProductCatalogue( ref ProductInfo[] productInfoSet ) {
			if( productInfoList == null && productInfoSet!=null ) {
				Debug.Log("MobyShop: StoreEditor : Initialzing, getting owned products.");
				productInfoList = productInfoSet;
				foreach( var prod in productInfoList ) {
					prod.Unlocked = prod.Unlocked;
					//Debug.LogError( "owned == " + prod.Unlocked );
				}
			}
		}


		/**
		 * Set the product owned by the BillingSimulator. The product.
		 */
		public static void SetProductOwned( string pid ) {
			bool fnd=false;
			foreach( var prod in productInfoList ) {
				if( prod.BillingId == pid || prod.GameId == pid ) {
					prod.SetUnlockedAndSaveLocalData();
					fnd=true;
				}
			}
			if( !fnd ) {
				Debug.LogError("MobyShop: Product; " + pid + " was not found when setting it to owned");
			}
		}
			

		/**
		 * Shows the billing simulator UI.
		 */
		void ShowBillingSimulatorUI( System.Action<bool> callback ) {
			billingSimulatorUI.callback = callback;
			billingSimulatorUI.gameObject.SetActive(true); 
			billingSimulatorUI.gameObject.transform.SetAsLastSibling(); // Bring to top...
		}


		/**
		 * Simuates buying a product.
		 */
		public static void BuyProduct( string productId, System.Action<bool,string> callback ) {
			if( inst==null ) {
				Debug.LogError("Error billinginstance is null");
			}
			inst.ShowBillingSimulatorUI( ( bool ok )=>{
				if( ok ) {
					PlayerPrefs.SetInt( "StoreEditor_Owned_"+productId, 1 );
				} else {
					// PlayerPrefs....
				}
				if( callback!=null ) {
					callback(ok,"");
				}
			} );
		}


		/**
		 * ensure the instance is set.
		 */
		public void EnsureInst() {
			inst=this;
		}


		/**
		 * rip - use BuyProduct instead.
		 */
		/*public void buyProduct( string productId, System.Action<bool,string> callback ) {
			if( inst==null ) {
				inst=this;
			}
			this.ShowBillingSimulatorUI( ( bool ok )=>{
				if( ok ) {
					PlayerPrefs.SetInt( "StoreEditor_Owned_"+productId, 1 );
				} else {
					// PlayerPrefs....
				}
				if( callback!=null ) {
					callback(ok,"");
				}
			} );
		}*/


		/**
		 * initialize. ( simulate )
		 */
		public static void Init( System.Action<bool> cb ) {
			cb(true);
		}


		/**
		 * returns true if the Product Catalogue has been downlaodet.
		 */
		public static bool HasRecvProductCatalogue {
			get {
				
				return true;
			}
		}


	}

}