/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop.UI {
		
	/**
	 * 
	 * The UI.ShopItem defines the logic needed to show a Product in the ProductCatalogue inside the game's shop.
	 * 
	 * It's fairly flexible and allows skinning and configuration to be done to the way a Product is displayed in various ways including :
	 * 		- Showing an overlay when a product is bought and cannot be bought more than once.
	 * 		- Showing the icon from the product catalogue versus showing an icon specific to the context of where you show the button.
	 * 		- Enabling specific hardcoded images and texts not related to the Product data. 
	 * 
	 * ShowItems can be used in ShopUI's OR it can also be used as a button you integrate directly into the game, like the score screen.
	 * 
	 * The component expects to sit on an object which is a UnityEngine.UI.Button and the button should invoke the local method called "OnProductClicked" - the method will initiate the
	 * sale of the product. 
	 * 
	 * The state of the UI.ShopItem will change depending on the state of the product. If the product has been bought and it's an unlockable you will not be able to
	 * buy the product again. The ShopItem is responsable for transalating that type of state infromation into sometihng that makes sense in the UI.
	 */
	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.UI.Button))]
	public class ShopItem : MonoBehaviour {
		public bool verbose = false;

		// The product id is the most important property of ShopItem. We use it to lookup a product instance in the product catalogue 
		// and fill in the data of the product to the labels etc. 
		public string productId = "";

		// if pricetag uppercase is set to true the text set in the price tag label will be forced to uppercase.
		public bool priceTagUppercase = false;

		// the product name label referance. the name of the product is taken from the product definded in the product catalogue.
		public UnityEngine.Component productName;

		// contains a referance to the container that shows the icon of the product. 
		// The container referance is used in the "Standard skin" to transform the confirm overlay's 
		// icon representation from the origin of the clicked button.
		public UnityEngine.RectTransform iconContainerTF;

		// referance to a UI.Image which shows the icon/logo of the product.
		public UnityEngine.UI.Image iconImage;

		// sometimes we want to ignore filling in the text in the editor.
		public bool fillProductName=true; 

		// product description label referance ( if fill Product descriptino with product data is true we set the label's text to the "short description" of the product )
		public UnityEngine.Component productDescription;

		// if true we will fill out the product description label with the "short description" from the Product.
		public bool fillProductDescriptionWithProductData=true; // sometimes we want to ignore filling in the text in the editor.

		// referance to the UI element displaying the pricetag.
		public UnityEngine.Component productPriceTag;

		// if fill image is set to true and there is a referance to the iconImage set we will fetch the 
		// icon specified in the Product Catalogue and display in the ShopItem.
		public bool fillImage=false;

		// referance to the badge widget shown ontop of the product with offer infos. 
		public ShopItemBadgeWidget badgeWidget;

		// referance to the fetched product.
		ProductInfo _product=null;

		// A referance to an instance of an overlay shown when the product has been bought an it's an unlockalble
		// you're not able to purchase again.
		public RectTransform boughtOverlay;

		// Buffered price tag string.
		public string _priceTagTextString = "";

		// You can specify a overwrite string that will overwrite the price tag string with another string.
		// this can be used to disable the pricetag for bought unlockbles or show the text "PURCHASED" in the price
		// tag instead of the price when an unlockable has been bought.
		public string _overwritePriceTagTextString = "";

		// Returns the price tag as a text string.
		string PriceTagTextString {
			get {
				if( string.IsNullOrEmpty (_overwritePriceTagTextString) == false ) {
					return priceTagUppercase ? _overwritePriceTagTextString.ToUpper() : _overwritePriceTagTextString;
				}
				if( string.IsNullOrEmpty (_priceTagTextString) ) {
					_priceTagTextString = Product.PriceTag;
				}
				return priceTagUppercase ? _priceTagTextString.ToUpper () : _priceTagTextString;
			}
		}


		/**
		 * Set the product price tag to the given string.
		 */
		public void SetProductPriceTag( string strvalue ) {
			if( productPriceTag!=null ) {
				_priceTagTextString = strvalue;
				TextObjectValue( productPriceTag, PriceTagTextString );
				CheckUpdate();
			}	
		}


		public void SetProductName( string name ) {
			if(productName!=null){
				TextObjectValue( productName, name );
				CheckUpdate();
			}
		}


		public void SetProductDescription( string name ) {
			if( productDescription != null ) {
				TextObjectValue( productDescription, name );
				CheckUpdate();
			}
		}


		public void SetProductImage( Sprite sprImage ) {
			if( this.iconImage != null ) {
				this.iconImage.sprite = sprImage;
				CheckUpdate();
			}
		}


		public void UpdateContent( ProductInfo prodInfo ) {
			if( Product == null ) {
				return;
			}
			if( fillProductDescriptionWithProductData ) {
				SetProductDescription( prodInfo.DescriptionShort );
			}
			if( fillImage && Product.Icon != null ) {
				SetProductImage( prodInfo.Icon );
			}
			if( fillProductName ) {
				SetProductName( prodInfo.ProductDisplayName );
			}
		}


		/**
		 * Return the product 
		 */
		ProductInfo Product {
			get {
				if( string.IsNullOrEmpty (this.productId) ) {
					Debug.LogError( "MobyShop.UI: There is no valid product id set for ShopButton : " + this.name, this.gameObject );
					return null;
				}

				// Resolve the product instance.
				if( Application.isEditor && Application.isPlaying == false ) {
					if( this._product!=null ) return this._product;
					if( ShopConfig.GetProductExists(this.productId) ) {
						return null;
					}
					if( !ShopConfig.GetProductExists(this.productId) ) {
						Debug.LogError( "Error getting product with id : " + this.productId + " for shop button object : " + this.name, this.gameObject );
						return null;	
					}
					this._product = ShopConfig.GetProductByProductId( this.productId );
					if( this._product != null && this._product.HasProductId == false) {
						Debug.LogError( "Error getting product with id : " + this.productId + " for shop button object : " + this.name, this.gameObject );
					}
					return this._product;
				}

				// Resolve the product instance or return the buffered product.
				if( this._product!=null ) return this._product;
				this._product = ShopConfig.GetProductByProductId( this.productId );
				return this._product;
			}
		}


		/**
		 * This method is used to make sure that the ShopItem gets updated 
		 * It's a hacky solution to force update a up element.
		 */
		void CheckUpdate() {
			if( Application.isEditor && Application.isPlaying==false ) {
				var pre = this.gameObject.activeSelf;
				this.gameObject.SetActive(false);
				this.gameObject.SetActive(pre);
			}
		}


		/**
		 * Called when the user clicks the product.
		 */
		public void OnProductClicked() {
			if( Product.HasBeenBought && Product.Unlockable ) {
				Debug.Log( "MobyShopUI: Ignored unlocking product that has alraedy been bought" );
				return;
			}

			if(verbose)Debug.Log("MobyShopUI: ShopItem clicked : " + this.productId + " initializing buy." );

			var shopUI = this.gameObject.GetComponentInAncestor<StandardShopUI>();
			if (shopUI != null ) {
				shopUI.OnShopItemClicked ( this ); 
			} else {
				Shop.BuyProduct( this.productId );
			}

            //QuanHM - set parent DialogConfirmPurchase(Clone) and DialogNotEnoughCoins(Clone) to ShopCanvas
            #region
            GameObject dialogConfirmPurchase = GameObject.Find("DialogConfirmPurchase(Clone)");
            if (dialogConfirmPurchase != null)
            {
                GameObject shopCanvas = GameObject.Find("ShopCanvas");
                dialogConfirmPurchase.transform.SetParent(shopCanvas.transform);
            }
            

            GameObject dialogNotEnoughCoins = GameObject.Find("DialogNotEnoughCoins(Clone)");
            if (dialogNotEnoughCoins != null)
            {
                GameObject shopCanvas = GameObject.Find("ShopCanvas");
                dialogNotEnoughCoins.transform.SetParent(shopCanvas.transform);
            }
            #endregion
        }


        /**
		 * This is a way of setting the text property on an instance of a object with a text property
		 * It can be used on diffirent classes as long as they have a field or property named "text".
		 */
        static void TextObjectValue( UnityEngine.Object o, string newv ) {
			if (o == null) {
				return;
			}
			var pi = o.GetType().GetProperty( "text", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance );
			if( pi != null ) {
				//string va = (string)pi.GetValue( o, new object[]{} );
				pi.SetValue( o, newv, new object[]{} );
				return;
			}
			var fi = o.GetType().GetField( "text", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance );
			if( fi!=null ) {
				//string va = (string)fi.GetValue(o);
				fi.SetValue( o, newv );
			}
		}


		/**
		 * On Enable we fill in the product data into the UI element.
		 */
		void OnEnable() {
			if( Application.isEditor && !Application.isPlaying ) {
				return;
			}
			UpdateState();
			FillProductData( false );
		}
						

		/**
		 * Fills the shop item with the product data. 
		 */
		void FillProductData( bool editMode=false ) {
			if( string.IsNullOrEmpty( productId ) ) { 
				return;
			}
			var p = Product;
			if( p == null || p.HasProductId == false || p.TryGetProductId() == "" ) {
				if( string.IsNullOrEmpty(productId) ) {
					Debug.LogError("MobyShop.UI: Cannot find product in ShopConfig with id : " + "[EMPTY STRING]", this.gameObject );
					return;
				}
				this._product = p = ShopConfig.GetProductByProductId( this.productId );		
				if( _product == null ) {
					Debug.LogError("MobyShop.UI: Cannot find product in ShopConfig with id : " + productId + " within products = " + ShopConfig.ProductListAsString, this.gameObject );
					return;
				}
			}
			if( fillProductName ) {
				if (!p.HasProductDisplayName) {
					if( verbose ) Debug.LogError ("MobyShop.UI: Product has no 'displayname' when filling in product data into UI Shop Item : " + this.name + " ProductId = " + p.TryGetProductId (), this.gameObject);
				} else {
					//Debug.Log("MobyShop.UI: Warning: productDescription was null.", this.gameObject);
					if (productName != null) {
						TextObjectValue (productName, p.ProductDisplayName);
					}
				}
			} 
			if( fillProductDescriptionWithProductData ) {
				if (!p.HasShortDescription) {
					if( verbose ) Debug.LogError ("MobyShop.UI: Product has no 'Short Description' when filling in product data into UI Shop Item : " + this.name + " ProductId = " + p.TryGetProductId (), this.gameObject);
				} else {
					if( productDescription!=null ) {
						TextObjectValue( productDescription, p.DescriptionShort );
					} else {

					}
				}
			} 
			if (productPriceTag != null) {
				TextObjectValue( productPriceTag, PriceTagTextString );
			}
		}


		/**
		 * In edit mode the product is filled with the data every frame.
		 */
		void Update() {
			if( Application.isEditor && Application.isPlaying == false ) {
				FillProductData( true );
#if UNITY_EDITOR
				//Rewire ();
#endif
				return;
			}
			UpdateState();
		}


		/**
		 * This a method that can be used as a Utility to quickly ensure that the button is invoking the right method on the right shopitem.
		 */ 
#if UNITY_EDITOR
		public void Rewire () {
			var btn = this.gameObject.GetComponent<UnityEngine.UI.Button>();
			if( btn == null ) {
				Debug.LogError ("MobyShop.UI: ShopItem has no button compoent on it");
				return;
			}
			//string dbg = "";
			//string goName = "";
			string methodName = "OnProductClicked";
			//var go = this.gameObject;
			var mi = this.GetType ().GetMethod (methodName,System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
			var obj = this;
			if( mi != null ) {
				btn.onClick.RemoveAllListeners();
				for( int i = 1; i<btn.onClick.GetPersistentEventCount(); i++ ) {
					UnityEditor.Events.UnityEventTools.RemovePersistentListener( btn.onClick, i );
				}
				var deleg = (UnityEngine.Events.UnityAction)System.Delegate.CreateDelegate( typeof(UnityEngine.Events.UnityAction), obj, mi) as UnityEngine.Events.UnityAction;
				UnityEditor.Events.UnityEventTools.RemovePersistentListener( btn.onClick, deleg );
				if( btn.onClick.GetPersistentEventCount() != 0 ) {
					UnityEditor.Events.UnityEventTools.RegisterVoidPersistentListener( btn.onClick, 0, deleg );
				} else {
					UnityEditor.Events.UnityEventTools.AddVoidPersistentListener( btn.onClick, deleg );
				}
				Debug.Log ("Rewired the ShopItem : " + this.name);
			} else {
				Debug.LogError ("Cannot find the method : " + methodName);
			}

		}
#endif


		/**
		 * This method is invoked with the purpose of changing the state of the UI Shop item so that
		 * the User can see if the item has been bought and cannot be bought again.
		 * 
		 * Or, other specifc type of product state indicators can be introduced here.
		 * 
		 */
		void SetProductBoughtState() {
			var cg = this.GetComponent<CanvasGroup>();
			var btn = this.GetComponent<UnityEngine.UI.Button>();
			if( Product.HasBeenBought && Product.Unlockable ) {
				_overwritePriceTagTextString = "Purchased";
				TextObjectValue( productPriceTag, PriceTagTextString );
				if (boughtOverlay != null) {
					this.boughtOverlay.gameObject.SetActive (true); 
				} else {
					if (cg != null) {
						cg.alpha = 0.3f;
					}
				}
				if( btn!=null ) {
					btn.interactable = false;
				}
			} else {
				_overwritePriceTagTextString = "";
				TextObjectValue( productPriceTag, PriceTagTextString );
				if (boughtOverlay != null) {
					this.boughtOverlay.gameObject.SetActive (false); 
				} else {
					if (cg != null) {
						cg.alpha = 1f;
					}
				}
				if( btn!=null ) {
					btn.interactable = true;
				}
			}
		}


		/**
		 * Check that the object has been bought and update the state accordingly.
		 */
		void UpdateState() {
			/*if( cg == null ) {
				cg = this.gameObject.AddComponent<CanvasGroup>();
			}*/
			if( Product==null ) {
				Debug.LogError("Error: Shop UI item has no product set.", this.gameObject );
				return;
			}
			SetProductBoughtState ();
		}

	}


}