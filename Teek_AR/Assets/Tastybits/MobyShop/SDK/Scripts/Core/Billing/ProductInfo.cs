/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;

namespace MobyShop {


	public enum ProductType {
		Unlockable = 0,
		Consumeable
	}


	public enum BillingType {
		RealLifeCurrency,
		IngameCurrency
	}


	[System.Serializable]
	public class ProductInfo {
		public string productDispName;
		public static bool _throwsErrors = false;

		[SerializeField]
		string desc;

		[SerializeField]
		string desc2;

		// 
		public override string ToString () {
			return string.Format ("[ProductInfo: ProductId={0}; Desc={1}, DescFormattet={2}, Price={3}, P.Class={4}, P.Name={5}, Price={6}, HasBeenBought={7}, BillingId_iOS={8}, BillingId_Android={9}, Consumeable={10}, Unlockable={11}, Value={12}, P.Id={13}, Unlocked={14}, BillingId={15}]", 
				GameId,
				_UnformattetShortDescription, 
				DescriptionShort, 
				priceTagInEditor,
				ProductClass,
				ProductDisplayName, 
				PriceTag,
				HasBeenBought,
				BId_iOS,
				BId_Android, 
				Consumeable, 
				Unlockable, 
				Value, 
				ProductId, 
				Unlocked,
				BillingId
			);
		}


		public ProductInfo() {

		}



		public string _UnformattetLongDescription {
			get { 
				return desc2;
			}
			set { 
				desc2 = value;
			}
		}


		public string _UnformattetShortDescription {
			get {
				return desc;
			}	
			set {
				desc = value;
			}
		}
			

		public string DescriptionShort {
			get {
				return desc.Replace ("%amount%", NumberFormatter.FormatNumber ("" + IncrementOnBuy, NumberFormatter.NumberFormat.Currency) ).
					Replace("%product%", this.ProductDisplayName);
			}
		}


		public bool HasShortDescription {
			get {
				return !string.IsNullOrEmpty(DescriptionShort);
			}
		}


		public string DescriptionLong {
			get {
				return desc2.Replace ("%amount%", NumberFormatter.FormatNumber ("" + IncrementOnBuy, NumberFormatter.NumberFormat.Currency) ).
					Replace("%product%", this.ProductDisplayName);
			}
		}


		// 
		public int IncrementOnBuy = 1;

		/**
		 * this is the pricetag . that we configure to use in the editor
		 * or when a price has not been downloaded yet.
		 */
		public string priceTagInEditor {
			get {
				return FormatNumber(""+price, NumberFormat.Currency) + " " + Currency;
			}
		}


		public enum NumberFormat {
			Number = 0,
			Currency = 1
		}
		static public string FormatNumber( string value, NumberFormat fmt ) {
			if( value.Length > 3 && fmt == NumberFormat.Currency ) {
				value = value.Insert( value.Length - 3, "." );
			}
			return value;
		}


		public int price = 1;

		/**
		 * Ingame currency type
		 */
		public string ingameCurrencyClass = "coins";


		/**
		 * 
		 */
		public Sprite icon;



		public Sprite Icon {
			get {
				return icon;
			}
		}


		static Texture2D _defaultProductIcon=null;
		static Texture2D _notFoundTexture=null;
		public Texture2D IconOrDefaultTexture {
			get {
				if( icon != null && icon.texture!=null ) return icon.texture;
#if UNITY_EDITOR
				if( _defaultProductIcon!=null ) {
					return _defaultProductIcon;
				}
				if(_defaultProductIcon==null ) {
					_defaultProductIcon = (Texture2D)UnityEditor.EditorGUIUtility.Load("MobyShop/Product.png");
				}
#endif
				_defaultProductIcon = Resources.Load("DefaultProductIcon", typeof(Texture2D) ) as Texture2D;
				if( _defaultProductIcon!=null ) {
					return _defaultProductIcon;
				}

				if( _notFoundTexture==null ) {
					_notFoundTexture = new Texture2D( 2, 2 );
					_notFoundTexture.SetPixels( new Color[]{Color.blue,Color.blue,Color.blue,Color.blue} );
					_notFoundTexture.Apply();
				}

				return _notFoundTexture;
			}
		}




		/**
		 * Returns the ingame currenct which can be used to 
		 * activate items for sale.
		 */
		string Currency {
			get {
				if( this.billing == BillingType.IngameCurrency ) {
					if( string.IsNullOrEmpty(ingameCurrencyClass ) ) {
						ingameCurrencyClass = "coins";
					}
					return ingameCurrencyClass;
				}
				return "$";
			}
		}



		/**
		 * 
		 */
		[HideInInspector]
		public string _ProductClass;
		public string ProductClass {
			get {
				if( !string.IsNullOrEmpty(_ProductClass) ) {
					return _ProductClass;
				}
				return ProductId;
			}
		}


		public int GetClassAmount() {
			if( this.Unlockable ) {
				return this.HasBeenBought ? 1 : 0;
			}
			int boughtAmount = PlayerPrefs.GetInt("PRODUCT_"+this.ProductClass+"_VALUE", 0 );
			return boughtAmount;
		}



		public StoreSetting[] StoreSettings = new StoreSetting[]{
			new StoreSetting( "AppleAppstore", "Apple App Store" ),
			new StoreSetting( "GooglePlay", "Google Play Store" ),
			new StoreSetting( "Amazon", "Amazon Store" ),
			new StoreSetting( "Windows", "Windows Store" ),
		};


		public ProductInfo( string productId, string displayName ) {
			gameId = productId;
			productDispName = displayName;
			desc = "";
		}


		public string ProductDisplayName {
			get {
				if( string.IsNullOrEmpty(productDispName) ) {
					if( !string.IsNullOrEmpty(this.gameId) ) {
						return this.gameId;
					}
					if( this.HasBillingId ) {
						return this.BillingId;
					}
					return "";
				}
				return productDispName;
			}
			set {
				productDispName = value;
			}
		}



		public bool HasProductDisplayName {
			get {
				var str = ProductDisplayName;
				return string.IsNullOrEmpty (str) == false;
			}
		}

	
		public string PriceTag {
			get {
				string strPriceTagFromSDK = "";
				if( billing == BillingType.IngameCurrency ) {
					return priceTagInEditor;
				}

#if UNITY_IPHONE && !UNITY_EDITOR
				if( BillingiOS.Billing_HasReceivedProductCatalogue() ) {
					strPriceTagFromSDK = BillingiOS.Billing_GetPriceTag( BillingId );
					strPriceTagFromSDK = strPriceTagFromSDK.Replace("GBP", "£").Replace("USD", "$" );
				}
#elif UNITY_ANDROID && !UNITY_EDITOR	
				if( BillingAndroid.HasRecvProductCatalogue ) {
					strPriceTagFromSDK = BillingAndroid.GetPriceTag( BillingId );
				}
#endif
				// Buffer the pricetag comming from the SDK.
				if( !string.IsNullOrEmpty(strPriceTagFromSDK) ) {
					PlayerPrefs.SetString("PRODUCT_"+this.ProductId+"_PRICETAG", strPriceTagFromSDK );
					return strPriceTagFromSDK;
				}

				// We were not ready to return a price tag from the SDK.
				// read from the Buffer or return from the editor setting.
				var str_val = PlayerPrefs.GetString("PRODUCT_"+this.ProductId+"_PRICETAG", "" );
				if( string.IsNullOrEmpty(str_val) ) {
					str_val = priceTagInEditor;
				}
				if( string.IsNullOrEmpty(str_val) ) {
					Debug.LogError("MobyShop: Warning you don't have a pricetag set in the inspector of a product info and it has not been saved from a previous session - solving by setting pricetag to '?$'");
					str_val = "?$";
				}

				return str_val;
			}
			set {

			}
		}
	

		public bool HasBeenBought {
			get {
				int i = PlayerPrefs.GetInt("PRODUCT_"+this.ProductId+"_STATUS", (int)ProductStatus.Locked );
				bool ret = i == (int)ProductStatus.Unlocked || i == (int)ProductStatus.Amount;
				return ret;
			}
		}


		public void OnBought(){
			if( !this.Consumeable ) {
				SetUnlockedAndSaveLocalData();
			} else {
				AddValueToProductAmountAndSaveLocalData( this.IncrementOnBuy );
			}
		}


		// These are the Id's that's needed to be setup.
		public string gameId;


		public BillingType billing;

		// you can overwrite the ID with an iOS specific id
		// when talking to the app store.
		public string BId_iOS {
			get {
				if( StoreSettings[0].overridden && !string.IsNullOrEmpty(StoreSettings[0].billingId) ){
					return StoreSettings[0].billingId;
				}
				return this.gameId;
			}
		}

		// you can overwrite the ID with an Android specific id
		// when talking to Google play.
		public string BId_Android {
			get {
				if( StoreSettings[1].overridden && !string.IsNullOrEmpty(StoreSettings[1].billingId) ){
					return StoreSettings[1].billingId;
				}
				return this.gameId;
			}
		}


		public ProductType type = ProductType.Unlockable;


		public bool Consumeable {
			get {
				return this.type == ProductType.Consumeable;
			}
		}


		public bool Unlockable {
			get {
				return this.type == ProductType.Unlockable;
			}
		}


		public int Value {
			get {
				int value = PlayerPrefs.GetInt("PRODUCT_"+this.ProductClass+"_VALUE", 0 );
				return value;
			}
			set {
				if( !this.Consumeable ) {
					PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_STATUS", (int)ProductStatus.Unlocked );
					PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_VALUE", value!=0 ? 1 : 0 );
				} else {
					PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_STATUS", (int)ProductStatus.Amount );
					PlayerPrefs.SetInt("PRODUCT_"+this.ProductClass+"_VALUE", value );
				}
			}
		}


		public string TryGetProductId() {
			if (HasProductId == false) {
				return "";
			}
			if( !string.IsNullOrEmpty(gameId) ) {
				return gameId;
			}
			if( !string.IsNullOrEmpty(BId_iOS) ) {
				return BId_iOS;
			}
			if( !string.IsNullOrEmpty(BId_Android) ) {
				return BId_Android;
			}
			return "";
		}


		/**
		 *  Returns true if there is a Product id present.
		 */
		public bool HasProductId {
			get {
				if( !string.IsNullOrEmpty(gameId) ) {
					return true;
				}
				if( !string.IsNullOrEmpty(BId_iOS) ) {	
					return true;
				}
				if( !string.IsNullOrEmpty(BId_Android) ) {
					return true;
				}
				return false;
			}
		}


		/**
		 * Returns the given billing Id / Product id for the given platform.
		 */
		public string ProductId {
			get {
				if( !string.IsNullOrEmpty(gameId) ) {
					return gameId;
				}
				string id = "";
				string platform = "undefined";
				if( !string.IsNullOrEmpty(BId_iOS) ) {
					id = BId_iOS;
					platform = "iOS";
				}
				if( !string.IsNullOrEmpty(BId_Android) ) {
					id = BId_Android;
					platform = "Android";
				}
				if( Application.isEditor == false || (Application.isEditor && Application.isPlaying == true ) ) {
					Debug.LogError("MobyShop: Warning! product info instance doesn't have an GameID configured ( solving by using id = " + id + " defined as the " + platform + " product )\nDesc=" + this.desc + "; DisplayName=" + this.productDispName + "; Type=" + this.type );
				} 
				//Debug.LogError("MobyShop: Error! local product info doesn't have an ID configured");
				return id;
			}
		}


		/**
		 * Unlocked; returns true when 
		 */
		public bool Unlocked {
			get {
				int value = PlayerPrefs.GetInt("PRODUCT_"+this.ProductId+"_VALUE", 0 );
				return value >= 1;
			}
			set {
				if( this.Unlockable ) {
					//Debug.LogError( "SET Unlocked : " + value );
					PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_STATUS", value?(int)ProductStatus.Unlocked:(int)ProductStatus.Locked );
					PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_VALUE", value?1:0 );
				} else { 
					//Debug.LogError("MobyShop: Why did you unlock a consumeable object? : " + this.ProductId );
					//PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_STATUS", (int)ProductStatus.Amount );
					//PlayerPrefs.SetInt("PRODUCT_"+this.Id+"_VALUE", value );
				}
			}
		}


		public void SetUnlockedAndSaveLocalData() {
			if( this.Consumeable ) {
				Debug.LogError("MobyShop: Why did you unlock a consumeable object? : " + this.ProductId );
				return;
			}
			//Debug.LogError( "SetUnlockedAndSaveLocalData" );
			Unlocked = true;
		}


		public void AddValueToProductAmountAndSaveLocalData( int amount_increment ) {
			//Debug.LogError( "AddValueToProductAmountAndSaveLocalData : " + amount_increment );
			PlayerPrefs.SetInt("PRODUCT_"+this.ProductId+"_STATUS", (int)ProductStatus.Amount );
			int cur_val = PlayerPrefs.GetInt("PRODUCT_"+this.ProductClass+"_VALUE", 0 );
			PlayerPrefs.SetInt("PRODUCT_"+this.ProductClass+"_VALUE", cur_val+amount_increment );
		}


		/**
		 * It will fall back on the billing Id.
		 */
		public bool HasBillingId {
			get { 
				string sz_pid = this.ProductId; 
				if( Application.isEditor ) {
					sz_pid = string.IsNullOrEmpty(BId_iOS)==false ? BId_iOS : BId_Android;
					if( string.IsNullOrEmpty(sz_pid) || !HasProductId ) {
						return false;
					}
					return true;
				}	 
				if( Application.platform == RuntimePlatform.IPhonePlayer ) {
					sz_pid = this.BId_iOS;
				} else if( Application.platform == RuntimePlatform.Android ) {
					sz_pid = this.BId_Android;
				}
				return !string.IsNullOrEmpty(sz_pid);
			}
		}



		/**
		 * @desc Returns the ID, if an if like Android or iOS id is specified then the id will be overwritten with that.
		 * in order to make it possible for a product to be setup diffirntly,and sometimes people setup
		 * diffirent id's on diffirent stores.
		 * 
		 * If no store spefic id is given then the ProductId will be used. 
		 */ 
		public string BillingId {
			get {
				string sz_pid = this.ProductId; 
				if( Application.isEditor ) {
					sz_pid = string.IsNullOrEmpty(BId_iOS)==false ? BId_iOS : BId_Android;
					if( string.IsNullOrEmpty(sz_pid) ) {
						sz_pid = this.ProductId;
					}
					if( string.IsNullOrEmpty(sz_pid) ) {
						if( _throwsErrors ) {
							throw new System.Exception( "MobyShop: No Product id/Billing specified for product.");
						}
						if( Application.isPlaying ) {
							throw new System.Exception("MobyShop: No product id specified for product" );	
						} else {
							Debug.LogError("MobyShop: No product id specifed for product." );
						}
					}
					return sz_pid;
				}	 
				if( Application.platform == RuntimePlatform.IPhonePlayer ) {
					sz_pid = this.BId_iOS;
				} else if( Application.platform == RuntimePlatform.Android ) {
					sz_pid = this.BId_Android;
				}
				return sz_pid;
			}
		}
	
		/**
		 * Returns the Id used in the game for referanceing the product.
		 */ 
		public string GameId {
			get {
				if( !string.IsNullOrEmpty( gameId ) ) {
					return gameId;
				}
				return BillingId;
			}
		}


	}

}