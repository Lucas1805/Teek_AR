/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif 
using System.Linq;


namespace MobyShop {


	/**
	 * ShopConfig is a central part of the MobyShop SDK, there is where all the configuration about a shop setup is stored.
	 * Each item you sell in a shop is refered to as a ProductInfo which represents an item in your game.
	 * 
	 * This is also in this class other types of infomation is stored like store id's.
	 * 
	 */
	public class ShopConfig : ScriptableObject {
		// singleton instnace referance.
		public static ShopConfig _instance=null;

		// Holds a list of product infos saved and used in the shop.
		[SerializeField]
		public ProductInfo[] products;


		// holds a referance to the App License in google play developer console. for the app your making.
		[SerializeField]
		public string androidPurchaseLicenseBase64;

		// if simulated billing on device is set the simulated billing will be opened in your game on device.
		[SerializeField]
		bool simulateBillingOnDevice = false;


		public bool SimulateBillingOnDevice {
			get { 
				bool simOn = (PlayerPrefs.GetInt ("MobyShop_BillingSimulator", 0) != 0 ? true : false);
				return simulateBillingOnDevice || simOn;
			}
			set { 
				PlayerPrefs.SetInt ("MobyShop_BillingSimulator", value ? 1 : 0);
				simulateBillingOnDevice = value;
			}
		}


		// _printErrors are here for debugging purposes.
		//public static bool _printErrors=true;

		// returns a list of all product id's.
		static public string[] ProductIdList {
			get {
				System.Collections.Generic.List<string> ret = new System.Collections.Generic.List<string>();
				foreach( var p in instance.products ) {
					ret.Add( p.ProductId );
				}
				return ret.ToArray();
			}
		} 


		// Property that returns the android purchase license.
		public static string AndroidPurchaseLicenseBase64 {
			get {
				return instance.androidPurchaseLicenseBase64;
			}
		}


		// The product classes is a list of string identifiers which could be something like
		// "coins", "arrows", "fireballs", "matches" - these are id's that is used to refer to values
		// that a class of products represents upon purchasing.
		public static string[] ProductClasses {
			get {
				var tmp = new System.Collections.Generic.List<string>();
				foreach( var prod in ProductList ) {
					if( string.IsNullOrEmpty(prod.ProductClass)==false && tmp.Contains(prod.ProductClass) == false ) {
						tmp.Add( prod.ProductClass );
					}
				}
				return tmp.ToArray();
			}
		}


		// Property that returns a list of ProductInfo's.
		public static ProductInfo[] ProductList {
			get {
				return instance.products;
			}
		}


		/**
		 * This is used for testing purpuses returns a Commasperted
		 * string with the product id's.
		 */
		public static string ProductListAsString {
			get {
				string Ret = "";
				foreach (var prod in ProductList) {
					Ret += (Ret != "" ? "," : "") + prod.ProductId;
				}
				return Ret;
			}
		}
			
		// Property that returns the singleton instance.
		public static ShopConfig instance {
			get {
				if( _instance != null ) {
					return _instance;
				}
				_instance = Resources.Load("ShopConfig") as ShopConfig;
				if( _instance == null ) {
#if UNITY_EDITOR
					_instance = CreateAsset<ShopConfig>();
#else
					Debug.LogError("Error reading ShopConfig from Resources");
#endif
				}
				return _instance;
			} 	
		}


#if UNITY_EDITOR
		/**
		 * Editor pnly: Creates a product.
		 */
		public static void CreateProduct( string productId ) {
			var tmp = instance.products.ToList();
			ProductInfo p = new ProductInfo( productId, productId );
			tmp.Add( p );
			instance.products = tmp.ToArray();
		}


		/**
		 * Editor only: Removes a product with the given product id.
		 */
		public static void RemoveProduct( string productId ) {
			var p = GetProductByProductId( productId );
			if( p == null ) {
				Debug.LogError("MobyShop: Error product with id doesn't exist : "+ productId );
			} else {
				var tmp = instance.products.ToList();
				tmp.Remove( p );
				instance.products = tmp.ToArray();
			}
		}

		/**
		 * Editor only: Create the ShopConfig data as an asset.
		 */
		static T CreateAsset<T> () where T : ShopConfig {
			T asset = ScriptableObject.CreateInstance<T> ();
			string path = "Assets/Tastybits/MobyShop/Resources";
			if( System.IO.Directory.Exists(path)==false) {
				System.IO.Directory.CreateDirectory( path );
			}
			if( System.IO.Directory.Exists(path)==false) {
				Debug.LogError("Error directory does not exists : " + path );
			}
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath( path + "/ShopConfig.asset");
			AssetDatabase.CreateAsset( asset, assetPathAndName );
			AssetDatabase.SaveAssets( );
			AssetDatabase.Refresh( );
			EditorUtility.FocusProjectWindow( );
			Selection.activeObject = asset;
			return asset;
		}
#endif


		/**
		 * Initiallize the singleton instance.
		 */
		void Awake() {
			_instance = this;
		}


		/**
		 * Get the value of product class.
		 */
		public static int GetProductClassValue( string productClass ) {
			if( productClass == "" ) {
				Debug.LogError("MobyShop: Invalid product class - Empty String");
				return -1;
			}
			int fnd=0;
			var availClasses = new System.Collections.Generic.List<string>();
			ProductInfo product=null;
			foreach( var p in ProductList ) {
				if( fnd==0 && p.ProductClass == productClass ) {
					product = p;
					fnd++;
				} else if( fnd > 0 && p.ProductClass == productClass ) {
					fnd++;
				}
				availClasses.Add( p.ProductClass );
			}
			if( fnd==0 ) {
				Debug.LogError("MobyShop: Product class :" + productClass + " was not found!\nAvailable classes:" + string.Join(",",availClasses.ToArray()) ); 
				return -1;
			}
			return product.GetClassAmount();
		}


		/**
		 * Returns true if the product with the gievn class exists.
		 */
		public static bool ProductClassExists( string productClass ) {
			if( productClass == "" ) {
				Debug.LogError("MobyShop: Invalid product class - Empty String");
				return false;
			}
			int fnd=0;
			var availClasses = new System.Collections.Generic.List<string>();
			//ProductInfo product=null;
			foreach( var p in ProductList ) {
				if( fnd==0 && p.ProductClass == productClass ) {
					//product = p;
					availClasses.Add( p.ProductClass );
					fnd++;
				} else if( fnd > 0 && p.ProductClass == productClass ) {
					fnd++;
				}

			}
			return fnd > 0;
		}
			

		/**
		 * Returns a list of the available prodcut classes.
		 */
		public static string[] GetAvailableProductClasses(  ) {
			//int fnd=0;
			var availClasses = new System.Collections.Generic.List<string>();
			foreach( var p in ProductList ) {
				if( !string.IsNullOrEmpty(p.ProductClass) && !string.IsNullOrEmpty(p.ProductClass.Trim()) ) {
					if( !availClasses.Contains(p.ProductClass) ) {
						availClasses.Add( p.ProductClass );
					}
				}
			}
			return availClasses.ToArray();
		}


		/**
		 * Returns the product info with the given class id.
		 */
		public static ProductInfo GetProductByClassId( string productClass ) {
			if( productClass == "" ) {
				Debug.LogError("MobyShop: Invalid product class - Empty String");
				return null;
			}
			int fnd=0;
			var availClasses = new System.Collections.Generic.List<string>();
			ProductInfo product=null;
			foreach( var p in ProductList ) {
				if( fnd==0 && p.ProductClass == productClass ) {
					product = p;
					fnd++;
				} else if( fnd > 0 && p.ProductClass == productClass ) {
					fnd++;
				}
				availClasses.Add( p.ProductClass );
			}
			if( fnd==0 ) {
				Debug.LogError("MobyShop: Product class :" + productClass + " was not found!\nAvailable classes:" + string.Join(",",availClasses.ToArray()) ); 
				return null;
			}
			return product;
		}


		/**
		 * returns true if the prooduct with the given product id exists - false if not.
		 */
		public static bool GetProductExists( string productId ) {
			foreach( var p in ProductList ) {
				if( p.GameId == productId ) {
					return true;
				}
			}
			foreach( var p in ProductList ) {
				if( p.BillingId == productId ) {
					return true;
				}
			}
			return false;
		}


		/**
		 * Returns the ProductInfo by the given product id.
		 */
		public static ProductInfo GetProductByProductId( string productId ) {
			if (string.IsNullOrEmpty (productId)) {
				Debug.LogError( "ShopConfig: Cannot get product with an EMPTY-STRING as product id." );
				return null;
			}
			foreach( var p in ProductList ) {
				if( p.ProductId == productId ) {
					return p;
				}
			}
			foreach( var p in ProductList ) {
				if( p.GameId == productId ) {
					return p;
				}
			}
			foreach (var p in ProductList) {
				if( p.BillingId == productId ) {
					Debug.LogError ("MobyShop: Error: product with editor name was not found  :" + productId + " however a product with the billingid storeid was found");
					return p;
				}
			}
			GameObject go=null;
			if( MobyShop.Shop._instance != null ) {
				go = MobyShop.Shop._instance.gameObject;
			}
			Debug.Log("MobyShop: Error getting product info with productId : " + productId + " - Open Shop Config and add the product if it's missing.\ncollection:" + ProductListAsString, go );
			return null;
		}

	
		/**
		 * Use this to cleara the player preferances for all the saved products.
		 */
		public static void ClearPrefsForPurchases(){
			foreach( var product in ShopConfig.ProductList ) {
				if( PlayerPrefs.HasKey( "PRODUCT_" + product.ProductClass + "_VALUE" ) ) {
					PlayerPrefs.DeleteKey( "PRODUCT_" + product.ProductClass + "_VALUE" );
				}
				if( PlayerPrefs.HasKey( "PRODUCT_" + product.ProductId + "_VALUE" ) ) {
					PlayerPrefs.DeleteKey( "PRODUCT_" + product.ProductId + "_VALUE" );
				}
				if( PlayerPrefs.HasKey( "PRODUCT_" + product.ProductId + "_PRICETAG" ) ) {
					PlayerPrefs.DeleteKey( "PRODUCT_" + product.ProductId + "_PRICETAG" );
				}
				if( PlayerPrefs.HasKey( "PRODUCT_" + product.ProductId + "_STATUS" ) ) {
					PlayerPrefs.DeleteKey( "PRODUCT_" + product.ProductId + "_STATUS" );
				}
			}
		}

	}


}