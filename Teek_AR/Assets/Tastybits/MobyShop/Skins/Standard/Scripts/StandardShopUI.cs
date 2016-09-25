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
	 * The SimpleShopUI is a controller class for the shop UI.
	 * you can use it in a game as a template for a shop and extend it right here or you can
	 * register listeners on it - the main event being cbOnDidDismiss
	 */
	public class StandardShopUI : ShopUIBase {
		// Singleton instance
		static StandardShopUI _instance=null;

		// Referance to the closebutton
		public GameObject closeButton;

		// Set this to true if we want to debug.
		public bool verbose {
			get {
				return (MobyShop.Shop.Verbose);
			}
		}

		// Set to true if twe want to show the feedback displayed when the
		// MobyShop
		public bool ShowUnlockableBoughtFeedback = false;

		// referance to a bought/unlocked feedback widget.
		// if this is null, no feedback will be shown.
		public ObjectsBoughtFeedback objectsBoughtFeedback;

		//
		public GameObject[] pageCollection1;

		//
		public GameObject[] pageCollection2;

		//
		public GameObject[] pageCollection3;

		//
		public ScrollingPages scrollingPages;

		// Confirm overlay dialog
		public MobyShop.Skins.Standard.ConfirmOverlay confirmOverlay;

		// 
		public MobyShop.Skins.Standard.ShopNotEnoughCoins notEnoughCoinsOverlay;

		// 
		public ShopTabbarWidget tabs;



		/**
		 * Instance...
		 */
		public static StandardShopUI instance {
			get {
				if( _instance == null ) {
					var gos = Object.FindObjectsOfType<GameObject>();
					foreach( var go in gos ) {
						var tmp = go.GetComponentInChildren2<StandardShopUI>(true);
						if( tmp != null ) {
							_instance = tmp;
							break;
						} 
					}
				}
				return _instance;
			}
		}


		/**
		 * Awake..
		 */
		void Awake() {
			_instance=this;
			if( confirmOverlay!=null && confirmOverlay.gameObject.activeSelf ) {
				confirmOverlay.gameObject.SetActive( false );	
			}
			if( notEnoughCoinsOverlay != null && notEnoughCoinsOverlay.gameObject.activeSelf ) {
				notEnoughCoinsOverlay.gameObject.SetActive(false);
			}
			if( Time.frameCount < 3 ) {
				instance.gameObject.SetActive(false);
			}
		}


		/**
		 * 
		 */
		void OnEnable(){
			this.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
			if( confirmOverlay!=null && confirmOverlay.gameObject.activeSelf ) {
				confirmOverlay.gameObject.SetActive( false );	
			}
			if( notEnoughCoinsOverlay!=null && notEnoughCoinsOverlay.gameObject.activeSelf ) {
				notEnoughCoinsOverlay.gameObject.SetActive( false );	
			}
		}


		/**
		 * 
		 */
		void OnDisable(){
			if( confirmOverlay!=null && confirmOverlay.gameObject.activeSelf ) {
				confirmOverlay.gameObject.SetActive( false );	
			}
			if( notEnoughCoinsOverlay!=null && notEnoughCoinsOverlay.gameObject.activeSelf ) {
				notEnoughCoinsOverlay.gameObject.SetActive( false );	
			}
		}


		/**
		 * support recompile in editor
		 */
#if UNITY_EDITOR
		void Update() {
			if( _instance == null ){
				_instance=this;
			}
		}
#endif	

		/**
		 * Shows the view. 
		 */
		public override void Show( int groupId, System.Action cbOnDismiss=null ) {
			this.cbOnDidDismiss = cbOnDismiss;
			this.gameObject.SetActive (true);
			tabs.SetSelected(groupId);
			if (this.enableAnimations) {
				StartCoroutine( ShowViewWithAnimations( ) );
			} 
		}


		/**
		 * 
		 */
		public void OnCloseClicked() {
			this.gameObject.SetActive (true);
			if (this.enableAnimations) {
				StartCoroutine (HideViewWithAnimations (() => {
					FireDidDismiss();
				}));
			} else {
				this.gameObject.SetActive (false);
				FireDidDismiss();
			}
		}


		/**
		 * This is used as a callback when the tabbar is clicked.
		 */
		public void OnTabbarChanged( int index ) {
			if( verbose ) Debug.Log( "OnTabbarChanged #" + index );
			if( scrollingPages != null ) {
				if (index == 0) {
					scrollingPages.SetPages (pageCollection1, pageCollection1.Length > 1 ? 1 : 0);
				} else if (index == 1) {
					scrollingPages.SetPages (pageCollection2, pageCollection1.Length > 1 ? 1 : 0);
				} else if (index == 2) {
					scrollingPages.SetPages (pageCollection3, pageCollection1.Length > 1 ? 1 : 0);
				}
			} else {
				Debug.Log( "No scrolling pages set." );
			}
		}


		/**
		 * This method is invoked when a ShopItem is a child of this object.
		 */
		public void OnShopItemClicked( ShopItem shopitem ) {
			if(verbose) {
				Debug.Log("Product Clicked : " + shopitem.productId );
			}
				
			var product = Shop.GetProduct (shopitem.productId);
			if (product == null) {
				Debug.LogError ("SimpleShopUI: ShopItem clicked referanced a product with Id: " + shopitem.productId + " which didn't return a valid product");
				return;
			}


			Texture2D productIconTex = null;
			if( product.Icon == null && (shopitem.iconImage != null && shopitem.iconImage.sprite.texture != null) ) {
				productIconTex = shopitem.iconImage.sprite.texture;
			} else {
				productIconTex = product.IconOrDefaultTexture;
			}

			// The Shop Items can for this skin contain an extra set of data
			// that tells the UI how to color the diffirent elements.
			var extraUIData = shopitem.GetComponent<ShopItemExtraUIData> ();
			Color bgcolor = Color.white;
			Color behindIconColor = Color.gray;
			RectTransform iconTF = shopitem.iconContainerTF;

			if( extraUIData != null ) {
				if( verbose )Debug.Log( "Set extraUIData - color" );
				bgcolor = extraUIData.colorBackground;
				behindIconColor = extraUIData.colorBehindIcon;
				if (!extraUIData.clickedIconTransition) {
					iconTF = null;
				}
			} else {
				if( verbose )Debug.LogError( "No colors available" );
			}

			confirmOverlay.backgroundImage.color = extraUIData.colorBackground;
			confirmOverlay.SetData( 
				productIconTex,
				product.DescriptionShort,
				product.DescriptionLong,
				product.PriceTag,
				bgcolor,
				behindIconColor,
				iconTF
			);
				
			if( verbose )Debug.Log( "Product unlockable : " + product.Unlockable );
			Shop.BuyProduct( shopitem.productId, ( bool ok, string msg, Shop.BuyResponse response )=> {
				if( ok ) {
					if( MobyShop.Shop.Verbose ) Debug.Log( "ShowUnlockableBoughtFeedback = " + ShowUnlockableBoughtFeedback );
					if( product!=null && product.Unlockable && ShowUnlockableBoughtFeedback && objectsBoughtFeedback!=null ) {
						if( verbose )Debug.Log( "Showing buy feedback" );
						objectsBoughtFeedback.Show( product.IconOrDefaultTexture, 2f );
					}
				} else {
					if( response == Shop.BuyResponse.BuyMoreCoins ) {
						if( verbose )Debug.Log( "User - BuyMoreCoins" );
						tabs.SetSelected( 2 );

					} else if( response == Shop.BuyResponse.Cancelled ) {
						if( verbose )Debug.Log( "User cancelled the purchase" );
					}
				}
			}, 	this.confirmOverlay, /*use this specific confirm overlay for buying*/
			  	this.notEnoughCoinsOverlay  /**/
			);
		}


	}

}