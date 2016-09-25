/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;


namespace MobyShop.UI {


	[CustomEditor(typeof(ShopItem))]
	public class ShopUIItemEditor : Editor {
		public override void OnInspectorGUI(){ 
			var targ = ( this.target as ShopItem );
			//base.OnInspectorGUI();

			var productIdList = ShopConfig.ProductIdList;

			int selidx = productIdList.ToList().IndexOf( targ.productId );

			GUILayout.Label("Product Id: " );


			int newidx = EditorGUILayout.Popup( selidx, productIdList );
			if( newidx != selidx ) {
				targ.productId = productIdList[newidx];
			}

			if(!( selidx >= 0 && productIdList.Length > selidx ) ) {
				EditorGUILayout.HelpBox( "No product selected - select a product from dropdown", MessageType.Error );
			}


			GUI.enabled = true;


			//var bf = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

			bool fillOpt = false;

			GameObject prego, newgo;


			fillOpt=false;
			//if( targ.productName != null ) {
			fillOpt=true;
			GUILayout.BeginHorizontal();
			//}
			prego = targ.productName !=null ? targ.productName.gameObject : null;
			newgo = (UnityEngine.GameObject)UnityEditor.EditorGUILayout.ObjectField( "Product Name Label", prego, typeof(UnityEngine.GameObject), true );
			if( prego!=newgo ) {
				//Debug.Log("Setting...");
				if( newgo == null ) {
					targ.productName = null;
				} else {
					var textEl = newgo.GetComponent<UnityEngine.UI.Text>();
					if( textEl != null ) {
						targ.productName = textEl;
					} else {
						var tmpel = newgo.GetComponent<UnityEngine.UI.MobyShop.TextImageOrText>();
						if( tmpel != null ) {
							targ.productName = tmpel;
						}
					}
				}
			}
			//if( targ.productName!=null ) {
			GUI.enabled = targ.productName!=null;
			if(targ.productName!=null) targ.productName.gameObject.SetActive( EditorGUILayout.ToggleLeft( "Active", targ.productName.gameObject.activeSelf, GUILayout.MaxWidth(60) ) );
			else EditorGUILayout.ToggleLeft( "Active", false, GUILayout.MaxWidth(60) );
			//}
			if( fillOpt ) {
				GUILayout.EndHorizontal();
			}
			GUI.enabled=true;




			// draw the inspector for the icon image 
			GUI.enabled=true;
			GUILayout.BeginHorizontal();
			var preimgo = targ.iconImage !=null ? targ.iconImage : null;
			var newimgo = (UnityEngine.UI.Image)UnityEditor.EditorGUILayout.ObjectField( "Icon Image", preimgo, typeof(UnityEngine.UI.Image), true );
			if( preimgo!=newimgo ) {
				//Debug.Log("Setting...");
				targ.iconImage = newimgo;
			}
			GUI.enabled = targ.iconImage!=null;
			if(targ.iconImage!=null) targ.iconImage.gameObject.SetActive( EditorGUILayout.ToggleLeft( "Active", targ.iconImage.gameObject.activeSelf, GUILayout.MaxWidth(60) ) );
			else EditorGUILayout.ToggleLeft( "Active", false, GUILayout.MaxWidth(60) );
			GUILayout.EndHorizontal();
			GUI.enabled=true;




			// draw the inspector for the icon image 
			GUI.enabled=true;
			GUILayout.BeginHorizontal();
			var prert = targ.iconContainerTF !=null ? targ.iconContainerTF : null;
			var newrt = (UnityEngine.RectTransform)UnityEditor.EditorGUILayout.ObjectField( "Icon Container", prert, typeof(UnityEngine.RectTransform), true );
			if( prert!=newrt ) {
				//Debug.Log("Setting...");
				targ.iconContainerTF = newrt;
			}
			//GUI.enabled = targ.iconContainerTF!=null;
			//if(targ.iconImage!=null) targ.iconImage.gameObject.SetActive( EditorGUILayout.ToggleLeft( "Active", targ.iconImage.gameObject.activeSelf, GUILayout.MaxWidth(60) ) );
			//else EditorGUILayout.ToggleLeft( "Active", false, GUILayout.MaxWidth(60) );
			GUILayout.EndHorizontal();
			GUI.enabled=true;




			fillOpt=false;
			//if( targ.productDescription != null ) {
			fillOpt=true;
			GUILayout.BeginHorizontal();
			//}
			prego = targ.productDescription !=null ? targ.productDescription.gameObject : null;
			newgo = (UnityEngine.GameObject)UnityEditor.EditorGUILayout.ObjectField( "Product Description Label", prego, typeof(UnityEngine.GameObject), true );
			if( prego!=newgo ) {
				if( newgo == null ) {
					targ.productDescription = null;
				} else {
					var textEl = newgo.GetComponent<UnityEngine.UI.Text>();
					if( textEl != null ) {
						targ.productDescription = textEl;
					} else {
						var tmpel = newgo.GetComponent<UnityEngine.UI.MobyShop.TextImageOrText>();
						if( tmpel != null ) {
							targ.productDescription = tmpel;
						}
					}
				}
			}
			//if( targ.productDescription!=null ) {
			GUI.enabled = targ.productDescription!=null;

			if(targ.productDescription!=null) targ.productDescription.gameObject.SetActive( EditorGUILayout.ToggleLeft( "Active", targ.productDescription.gameObject.activeSelf, GUILayout.MaxWidth(60) ) );
			else EditorGUILayout.ToggleLeft( "Active", false, GUILayout.MaxWidth(60) );

			//}
			if( fillOpt ) {
				GUILayout.EndHorizontal();
			}
			GUI.enabled=true;




			GUILayout.BeginHorizontal ();
			prego = targ.productPriceTag !=null ? targ.productPriceTag.gameObject : null;
			newgo = (UnityEngine.GameObject)UnityEditor.EditorGUILayout.ObjectField( "Price Tag Label", prego, typeof(UnityEngine.GameObject), true );
			if( prego!=newgo ) {
				if( newgo == null ) {
					targ.productPriceTag = null;
				} else {
					var textEl = newgo.GetComponent<UnityEngine.UI.Text>();
					if( textEl != null ) {
						targ.productPriceTag = textEl;
					} else {
						var tmpel = newgo.GetComponent<UnityEngine.UI.MobyShop.TextImageOrText>();
						if( tmpel != null ) {
							targ.productPriceTag = tmpel;
						}
					}
				}
			}
			targ.priceTagUppercase = EditorGUILayout.ToggleLeft( "Uppercase", targ.priceTagUppercase, GUILayout.MaxWidth(60) );
			GUILayout.EndHorizontal ();


	
			targ.badgeWidget = (MobyShop.UI.ShopItemBadgeWidget)UnityEditor.EditorGUILayout.ObjectField( "Badge Widget", targ.badgeWidget, typeof(MobyShop.UI.ShopItemBadgeWidget), true );
	

			GUI.enabled = targ.badgeWidget != null;
			if( targ.badgeWidget!= null ) {
				targ.badgeWidget.badgeEnabled = UnityEditor.EditorGUILayout.ToggleLeft( "Badge Enabled", targ.badgeWidget.badgeEnabled );
			}else {
				EditorGUILayout.ToggleLeft( "Badge Enabled", false );
			}

			if( targ.badgeWidget != null && targ.badgeWidget.badgeEnabled ) {
				string oldText = targ.badgeWidget.Text;
				var newText = UnityEditor.EditorGUILayout.TextField( "Badge Text", oldText );
				if( newText != oldText ) {
					targ.badgeWidget.Text = newText;
					Canvas.ForceUpdateCanvases();
				}
			} else {
				UnityEditor.EditorGUILayout.TextField( "Badge Text", "" );
			}
			GUI.enabled = true;



			targ.boughtOverlay = (RectTransform)UnityEditor.EditorGUILayout.ObjectField( "Bought Overlay", targ.boughtOverlay, typeof(RectTransform), true );

			//bool productChosen = (this.target as ShopItem).productId!="" && !string.IsNullOrEmpty((this.target as ShopItem).productId);

			if( selidx >= 0 && productIdList.Length > selidx ) {
				GUILayout.BeginVertical(EditorStyles.helpBox);
				GUILayout.Label("Product Settings - From Shop Config" );
				//int selidx = productIdList.ToList().IndexOf( targ.productId );
				ProductInfo prodInfo = ShopConfig.GetProductByProductId( targ.productId );
				if( prodInfo!=null ) {
					GUILayout.BeginHorizontal();
					GUILayout.Box( prodInfo.IconOrDefaultTexture, GUILayout.Width(32), GUILayout.Height(32) );
					GUI.enabled = prodInfo.icon!=null && targ.iconImage!=null;
					targ.fillImage = EditorGUILayout.ToggleLeft( "Upd. UI Image", targ.fillImage, GUILayout.MaxWidth(90) );
					if( targ.productName !=null && targ.fillImage && prodInfo.icon!=null ) {
						targ.SetProductImage( prodInfo.icon );
					}
					GUI.enabled=true;
					GUILayout.EndHorizontal();


					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Display Name", prodInfo.ProductDisplayName );
					GUI.enabled = targ.productName!=null;
					targ.fillProductName = EditorGUILayout.ToggleLeft( "Upd. UI Label", targ.fillProductName, GUILayout.MaxWidth(90) );
					if( targ.productName !=null && targ.fillProductName ) {
						targ.SetProductName( prodInfo.ProductDisplayName );
					}
					GUI.enabled=true;
					GUILayout.EndHorizontal();


					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Description", ""+prodInfo.DescriptionShort );
					GUI.enabled = targ.productDescription!=null;
					targ.fillProductDescriptionWithProductData = EditorGUILayout.ToggleLeft( "Upd. UI Label", targ.fillProductDescriptionWithProductData, GUILayout.MaxWidth(90) );
					if( targ.productDescription !=null && targ.fillProductDescriptionWithProductData ) {
						targ.SetProductDescription( prodInfo.DescriptionShort );
					}
					GUI.enabled=true;
					GUILayout.EndHorizontal();



					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField( "Price Tag", prodInfo.PriceTag );
					if( targ.productPriceTag!=null ) { // Update the price...
						targ.SetProductPriceTag( prodInfo.PriceTag );
					}
					GUILayout.EndHorizontal();




					EditorGUILayout.LabelField("Class", prodInfo.ProductClass );
					EditorGUILayout.LabelField("Consumeable", ""+prodInfo.Consumeable );
	

				} else {
					EditorGUILayout.HelpBox( "Error resolving product '" + targ.productId + "'", MessageType.Error );
				}
				if( GUILayout.Button("Open In Shop Config") ) {
					MobyShop.ShopConfigurationEditor.OpenAndSelectProductWithId( targ.productId );
				}
				GUILayout.EndVertical();
			} else {
				GUI.enabled = true;
				if( GUILayout.Button("Open Shop Config") ) {
					MobyShop.ShopConfigurationEditor.Open();
				}
				GUI.enabled = true;
			}

			if( !string.IsNullOrEmpty (targ.productId) ) {
				ProductInfo pi = ShopConfig.GetProductByProductId( targ.productId );
				if( pi!=null ) {
					targ.UpdateContent( pi );
				}
			}


			if( string.IsNullOrEmpty((this.target as ShopItem).productId) || string.IsNullOrEmpty((this.target as ShopItem).productId.Trim()) ) {
				EditorGUILayout.HelpBox( "The product has no product id set. Fix this by setting it or creating a new product.", MessageType.Error );
			}
			else if( selidx == -1 && string.IsNullOrEmpty(targ.productId)==false ) {
				EditorGUILayout.HelpBox( "The product id : " + targ.productId + " is invalid. Fix this by creating the product or setting the product id to something diffirent.", MessageType.Error );
			}
			else if( targ.productPriceTag != null && targ.productPriceTag == targ.productDescription ) {
				EditorGUILayout.HelpBox( "The labels of the products descriptino and price tag was the same", MessageType.Error);
			}

			GUI.enabled = (this.target as ShopItem).productId!="" && !string.IsNullOrEmpty((this.target as ShopItem).productId);
			/*if( GUILayout.Button("Open Shop Config") ) {
				MobyShop.ShopConfigurationEditor.OpenAndSelectProductWithId( targ.productId );
			}*/
		}
	}


}