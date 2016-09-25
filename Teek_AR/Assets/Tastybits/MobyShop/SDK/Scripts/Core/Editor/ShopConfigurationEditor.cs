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


namespace MobyShop {



	public class KEditorGUIUtility {
		public static GUIContent TextContent( string name ) {
			System.Reflection.BindingFlags bf = 
				System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
			var mi = typeof(UnityEditor.EditorGUIUtility).GetMethod( "TextContent", bf );
			return (GUIContent)mi.Invoke( null, new object[]{ name } );
		}
	}


	public class ShopConfigurationEditor : UnityEditor.EditorWindow {
		public float saveCount = -1f;
		static float products_width = 415f;
		static float products_height= 381f;


		ShopConfigurationEditor() {
			base.position = new Rect(50f, 50f, 955f, 530f);
			base.minSize = new Vector2(955f, 530f);
			base.maxSize = new Vector2(995f, 530f);
			base.titleContent = new GUIContent ("-= MobyShop =-");
		}


		[MenuItem("Window/MobyShop/Shop Configuration %k", false, 175 ) ]
		public static void _Open( ){
			Open(null);
		}


		public static void Open( ShopConfig config=null ){
			var wnd = EditorWindow.GetWindow<ShopConfigurationEditor>( true );		
			wnd.Show();
			if( config!=null ) {
				wnd.shopcfg= config;
			} else {
				wnd.shopcfg = ShopConfig.instance;
			}
			wnd.CenterOnMainWin();
		}


		/*[MenuItem("Window/MobyShop/Delete PlayerPrefs", false, 175 ) ]
		public static void Open_(){
			PlayerPrefs.DeleteAll();
		}*/


		public static void OpenAndSelectProductWithId( string ProductId ) {
			var wnd = EditorWindow.GetWindow<ShopConfigurationEditor>( true );		
			wnd.Show();
			wnd.shopcfg = ShopConfig.instance;
			wnd.CenterOnMainWin();
			ProductInfo prod = wnd.shopcfg.products.ToList().Find( x=>x.ProductId == ProductId );
			if( prod==null ) {
				Debug.LogError("Error finding product with id : " + ProductId );
			} else {
				wnd.selProd = prod;
				wnd.SelectedProductIdx = wnd.shopcfg.products.ToList().IndexOf(prod);
			}
		}


		public ShopConfig shopcfg;

		 
		static Texture2D _defaultProductIcon=null;
		public static Texture2D GetProductIcon( ProductInfo prodInfo ) {
			if( prodInfo!=null && prodInfo.Icon != null ) {
				return prodInfo.Icon.texture;
			}
			if(_defaultProductIcon==null ) {
				_defaultProductIcon = (Texture2D)EditorGUIUtility.Load("MobyShop/Product.png");
			}
			return _defaultProductIcon;
		}


		public static Texture2D null_built {
			get {
				return null;
			}
		}


		void OnGUI(){
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			//string empty = string.Empty;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Height(products_height) });
			ShowProductsGUI();
			GUILayout.Space(10f);


			GUILayout.BeginVertical(new GUILayoutOption[0]);
			DrawSelectedProduct();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();


			GUILayout.Space(10f);

			GUILayout.EndVertical();

			GUILayout.Space(10f);

			shopcfg.androidPurchaseLicenseBase64 = EditorGUILayout.TextField("Google Play: App License", shopcfg.androidPurchaseLicenseBase64 );
			EditorGUILayout.HelpBox("On Google play you need to specify the 'App License Key'. This key can be copy/pasted from Services & API's for your app in the Google Developer Console", MessageType.Info );

			shopcfg.SimulateBillingOnDevice = EditorGUILayout.ToggleLeft( "Simulate Billing on Device", shopcfg.SimulateBillingOnDevice );
			EditorGUILayout.HelpBox("If you toggle simuate billing on device, the billing simulator is started on the device instead of the real build.", MessageType.Info );


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();
		}


		void ShowProductsGUI(){
			DrawProductList( shopcfg.products );
		}


		public ProductInfo[] Products {
			get {  
				return shopcfg.products;
			}
		}


		public static void DrawProductListSimple() {
			int iter=0;
			foreach( var prod in ShopConfig.ProductList ) {
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField( "ID:" + prod.ProductId, "Name:" +  prod.ProductDisplayName );
				if( GUILayout.Button("...", GUILayout.Width(30) ) ) {
					ShopConfigurationEditor.OpenAndSelectProductWithId( prod.ProductId );
				}
				GUILayout.EndHorizontal();
				iter++;
			}
		}


		/*void ShowSelProductGUI(){

			string product_name = "";
			if( SelectedProductIdx >= 0 && SelectedProductIdx >= Products.Length -1 ) product_name = Products[SelectedProductIdx].ProductDisplayName;

			GUILayout.Label("Sel product #" + SelectedProductIdx + " " + product_name );		


			if( GUILayout.Button("Delete product") ) {
				try {
					//var billingId = jobs[selectedJob].BillingId;
					var id = Products[SelectedProductIdx].Id;
					var dispName = Products[SelectedProductIdx].ProductDisplayName;
			 		bool no = EditorUtility.DisplayDialog( "Are you sure?", "Do you want to permanently delete the product : " + dispName, "Cancel", "Delete" );
					if( !no ) {
						ShopConfig.RemoveProduct( id );
						SelectedProductIdx = -1;
						SaveConfig();
					} else {

					}
				} catch( System.Exception e ) {
					Debug.LogError("Exception: " + e );
				}
			}
		}*/


		public class ListViewState {
			public float rowHeight=0f;	
		}
		private ListViewState lv = new ListViewState();
		Vector2 scrollPosition = Vector2.zero;
		Vector2 productInspectorScroll = Vector2.zero;
		public int SelectedProductIdx = -1;
		public ProductInfo selProd = null;
		public bool loadSelProd=false;


		ProductInfo Selected {
			get {
				if( Products == null || SelectedProductIdx < 0 || SelectedProductIdx >= Products.Length ) return null;
				return Products[SelectedProductIdx];
			}
		}


		void DrawSelectedProduct(){

			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[] {
				GUILayout.Width(500f)
			});
				

			if( SelectedProductIdx < 0 ) {
				GUILayout.Label( "", ShopConfigurationEditor.styles.title, new GUILayoutOption[0]);					
			} else {
				GUILayout.Label( ""+Products[SelectedProductIdx].ProductDisplayName, ShopConfigurationEditor.styles.title, new GUILayoutOption[0]);					
			}

			this.productInspectorScroll = GUILayout.BeginScrollView( this.productInspectorScroll, "OL Box" );

			if( SelectedProductIdx < 0 ) {
				DrawCenterMessage( "No product selected" );
			} else {
				DrawProductProperties();
			}

			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			//BuildTarget buildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUI.enabled = true; //(BuildPipeline.IsBuildTargetSupported(buildTarget) && BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) != BuildPipeline.GetBuildTargetGroup(buildTarget));



			if( Selected == null ) {
				GUILayout.Label(""); // empry space..
			} else {
				if( GUILayout.Button("Delete product") ) {
					try {
						//var billingId = jobs[selectedJob].BillingId;
						var id = Products[SelectedProductIdx].ProductId;
						var dispName = Products[SelectedProductIdx].ProductDisplayName;
						bool no = EditorUtility.DisplayDialog( "Are you sure?", "Do you want to permanently delete the product : " + dispName, "Cancel", "Delete" );
						if( !no ) {
							ShopConfig.RemoveProduct( id );
							SelectedProductIdx = -1;
						} else {

						}
					} catch( System.Exception e ) {
						Debug.LogError("Exception: " + e );
					}
				}
			}



			//GUI.enabled = BuildPipeline.IsBuildTargetSupported(buildTarget);
			//if (GUILayout.Button(new GUIContent("Player Settings..."), new GUILayoutOption[] { GUILayout.Width(110f) })) {
			//Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
			//}
			GUILayout.EndHorizontal();
			GUI.enabled = true;
			GUILayout.EndVertical();


		}


		void DrawCenterMessage( string te ) {
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label( te );
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}



		void DrawProductProperties() {
			DrawPropsCustom();
		}

		public string lastTooltip = "";
		public int selStoreIdx = -1;


		GUIStyle _HelpTipStyle;
		public GUIStyle HelpTipStyle {
			get {
				if( _HelpTipStyle == null ) {
					_HelpTipStyle = new GUIStyle( GUI.skin.button );
					_HelpTipStyle.active.background = null;
					_HelpTipStyle.normal.background = null;
					_HelpTipStyle.onNormal.background = null;
					_HelpTipStyle.onHover.background = null;
					_HelpTipStyle.hover.background = null;
				}
				return _HelpTipStyle;
			}
		}


		// 
		public StoreSetting[] Stores {
			get {
				return Selected.StoreSettings;
			}
		}


		GUIStyle _StoresTabStyle;
		GUIStyle StoresTabStyle {
			get {
				if( _StoresTabStyle!=null ) {
					return _StoresTabStyle;
				}
				_StoresTabStyle = new GUIStyle(EditorStyles.toolbarButton);
				_StoresTabStyle.fixedHeight = 20f;
				return _StoresTabStyle;
			}
		}

		public string helpmsg = "";
		public string reqhelpmsg = "";


		void DrawPropsCustom() {
			var product = Selected;
			if( product==null ) return;

			string caption = "";
			string tip = "";

			DrawHelp( helpmsg );
			//return;
			if( Event.current.type == EventType.Repaint ) {
				helpmsg = "";
			} 


			//EditorGUILayout.HelpBox("Name", MessageType.None);
			caption = "Product Name";
			tip = "The '"+caption+"' is the name of the purchase you want to be displayed in the game like 'Big Pack Of Coins' etc.";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			product.productDispName = EditorGUILayout.TextField( caption, product.productDispName );
			EditorGUILayout.EndHorizontal();
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}

			// 
			caption = "Description (Short)";
			tip = "The '"+caption+"' is a text string you can use in the game to display some details on the product in the ShopUI.";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			GUILayout.Label( caption, GUILayout.MinWidth(145) );
			product._UnformattetShortDescription = EditorGUILayout.TextArea( product._UnformattetShortDescription, GUILayout.MaxWidth( 234 ) );
			EditorGUILayout.EndHorizontal();
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}

			// 
			caption = "Description (Long)";
			tip = "The '"+caption+"' is a text string you can use in the game to display some extra details on the product in the ShopUI.";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			GUILayout.Label( caption, GUILayout.MinWidth(145) );
			product._UnformattetLongDescription = EditorGUILayout.TextArea( product._UnformattetLongDescription, GUILayout.MaxWidth( 234 ) );
			EditorGUILayout.EndHorizontal();
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}

			// 
			caption = "Price Tag";
			tip = "The '"+caption+"' is a value which is only shown when testing purchases OR when showing purchases which uses ingame currency as payment.";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			product.price = EditorGUILayout.IntField( caption, product.price, GUILayout.MaxWidth(230) );
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}
			if( Selected.billing == BillingType.IngameCurrency ) {
				caption = "Currency";
				tip = "The '"+caption+"' is the type of Product Class from which you want to instantiate your purchase - this value will be coins per default but you can change it into something like 'stars' if your game has such a thing.";
				GUI.SetNextControlName(caption);
				product.ingameCurrencyClass = GUILayout.TextField( product.ingameCurrencyClass, GUILayout.MaxWidth(60f) );
				if( GUI.GetNameOfFocusedControl() == caption ) {
					helpmsg = tip;
				}
				if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
					reqhelpmsg = tip;
				}
			} else {
				//GUI.enabled=false;
				EditorGUILayout.LabelField( "$", GUILayout.MaxWidth(60f) );
				//GUI.enabled=true;
			}
			EditorGUILayout.EndHorizontal();





			caption = "Product type";
			tip = "The '"+caption+"' can be unlockable or consumeable. unlockable items can be bought only once per user like remove ads, more levels etc.\nA consumeable product can be gold coins, swords, arrows, diamonds etc.";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			product.type = (ProductType)EditorGUILayout.EnumPopup( caption, (System.Enum)product.type );
			EditorGUILayout.EndHorizontal();
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}


			caption = "Amount in Purchase";
			if( product.type == ProductType.Consumeable ) {
				tip = "The '"+caption+"' is a number between 1 and X which represents how many items you get when you buy the consumeable. Let's say the product is a 'coinpack' you want the users to get 100 coins every they buy it, the value is set to 100 then.";
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
					reqhelpmsg = tip;
				}
				GUI.SetNextControlName(caption);
				product.IncrementOnBuy = (int)EditorGUILayout.IntField( caption, product.IncrementOnBuy );
				if( product.IncrementOnBuy < 1 ) {
					product.IncrementOnBuy = 1;
				}
				EditorGUILayout.EndHorizontal();
				if( GUI.GetNameOfFocusedControl() == caption ) {
					helpmsg = tip;
				}

				caption = "Product Class";
				tip = "The '"+caption+"' can be set in order to group consumeable products in order make their purchases affect the same type of value. For instance you want 'coinpack1' and 'coinpack2' to affect the value 'coins'.\nIf no value is specified the name of the object will be used.";
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
					reqhelpmsg = tip;
				}
				GUI.SetNextControlName(caption);
				product._ProductClass = EditorGUILayout.TextField( caption, product._ProductClass );
				EditorGUILayout.EndHorizontal();
				if( GUI.GetNameOfFocusedControl() == caption ) {
					helpmsg = tip;
				}


			} else {
				tip = "The '"+caption+"' is only used when the Product is set to Consumeable.";
				GUI.enabled=false;
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
					reqhelpmsg = tip;
				}
				EditorGUILayout.TextField( caption, "n/a" );
				EditorGUILayout.EndHorizontal();


				caption = "Product Class";
				tip = "The '"+caption+"' can be set in order to group consumeable products in order make their purchases affect the same type of value. For instance you want 'coinpack1' and 'coinpack2' to affect the value 'coins'.\nIf no value is specified the name of the object will be used.";
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
					reqhelpmsg = tip;
				}
				GUI.SetNextControlName(caption);
				EditorGUILayout.TextField( caption, "n/a" );
				EditorGUILayout.EndHorizontal();
				if( GUI.GetNameOfFocusedControl() == caption ) {
					helpmsg = tip;
				}

				GUI.enabled=true;
			}



			caption = "Billing type";
			tip = "The '"+caption+"' can be the RealLifeCurrency: Dollars,Pounds,Yen etc through the Payment channel or IngameCurrency: coins, gp, stars or diamonds that you accumulate or buy in the game.";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			product.billing = (MobyShop.BillingType)EditorGUILayout.EnumPopup( caption, (System.Enum)product.billing );
			EditorGUILayout.EndHorizontal();
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}

			caption = "Icon";
			tip = "The '"+caption+"' can be fetched and used for UI purposes";
			EditorGUILayout.BeginHorizontal();
			if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
				reqhelpmsg = tip;
			}
			GUI.SetNextControlName(caption);
			product.icon = (Sprite)EditorGUILayout.ObjectField( caption + " (optional)", product.icon, typeof(Sprite), false );
			EditorGUILayout.EndHorizontal();
			if( GUI.GetNameOfFocusedControl() == caption ) {
				helpmsg = tip;
			}

			/****************************************************************************************
			 * Tabs for each store.
			 ****************************************************************************************/



			GUI.enabled = true;
			EditorGUI.BeginChangeCheck();
			Rect rect = EditorGUILayout.BeginVertical( GUI.skin.box, new GUILayoutOption[0] );
			rect.width -= 1f;
			var toolbarButton = StoresTabStyle;
			GUIContent defaultTab = new GUIContent("Default Store Settings"); // EditorGUIUtility.TextContent("TextureImporter.Platforms.Default");
			//int selectedi = 0;
			int numItems = Stores.Length; // number of tabs
			int tabItemHeight = 20;
			float itmwidth = 40f;
			if( defaultTab != null && GUI.Toggle(new Rect(rect.x, rect.y, rect.width - (float)numItems * itmwidth, (float)tabItemHeight), selStoreIdx == -1, defaultTab, toolbarButton)) {
				selStoreIdx = -1;
			}

			if( Selected.billing == BillingType.IngameCurrency ) {
				selStoreIdx = -1;
			}

			//Debug.Log("numItems="+numItems)
			EditorGUI.BeginDisabledGroup( Selected.billing == BillingType.IngameCurrency );
			for( int j = 0; j < numItems; j++ ) {
				Rect position;
				if( defaultTab != null ) {
					position = new Rect( rect.xMax - (float)(numItems - j) * itmwidth, rect.y, itmwidth, (float)tabItemHeight );
					//Debug.Log("pos #" + j + "  - " + position );
				} else {
					int num5 = Mathf.RoundToInt((float)j * rect.width / (float)numItems);
					int num6 = Mathf.RoundToInt((float)(j + 1) * rect.width / (float)numItems);
					position = new Rect(rect.x + (float)num5, rect.y, (float)(num6 - num5), (float)tabItemHeight);
				}
				if( Stores[j].SmallIcon == null ) {
					//Debug.LogError("Error the icon is null.");
				}
				if (GUI.Toggle(position, selStoreIdx == j, new GUIContent(Stores[j].SmallIcon, Stores[j].tooltip), toolbarButton)) {
					selStoreIdx = j;
				}
			}
			EditorGUI.EndDisabledGroup();
			GUILayoutUtility.GetRect(10f, (float)tabItemHeight);
			EditorGUI.EndChangeCheck();


			bool drawSettings = true;
			for( int i=-1; i<numItems && drawSettings; i++ ) {
				if( selStoreIdx != i ) {
					continue;
				}

				if( i>=2) {
					DrawCenterMessage( "Coming soon, please contact support for more info" );
					continue;
				}


				StoreSetting store = selStoreIdx>=0 ? Stores[selStoreIdx] : null;

				if(store !=null){
					bool before=store.overridden;
					store.overridden = GUILayout.Toggle( store.overridden, "Override for " + store.dispname, new GUILayoutOption[0] );
					bool disabled = !store.isDefault && !store.overridden;
					EditorGUI.BeginDisabledGroup(disabled);
					if( before != store.overridden ) {
						this.Repaint();
					}
					if( !disabled && before != store.overridden && string.IsNullOrEmpty(store.billingId) ) {
						store.billingId = product.gameId;
					}
				}


				EditorGUI.BeginChangeCheck();


				caption = "Product Id";
				tip = "The '"+caption+"' is used to refer to a product in the shop when the user buys a product, it id could be 'remove_ads' which could represent an purhase to remove the Ads.";
				if( i > -1 ) {
					//GUILayout.Label(" " + "( ProductId in " +store.dispname+")" );
					caption = "Billig Id";
					tip = "The '"+caption+"' is used when you want to setup a specific billing id for a store which is diffirent than the product id.";
				}
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button( new GUIContent(" ", tip), HelpTipStyle, GUILayout.Width(16) ) ) {
					reqhelpmsg = tip;
				}
				GUI.SetNextControlName(caption);

				if( i == -1 ) {
					product.gameId = EditorGUILayout.TextField( caption, product.ProductId );
				} else {
					if( store.overridden ) {
						store.billingId = EditorGUILayout.TextField( caption + " (Store Product Id)", store.billingId );
					} else {
						EditorGUILayout.TextField( caption+ " (Store Product Id)", product.gameId );
					}
				}

				EditorGUILayout.EndHorizontal();
				if( GUI.GetNameOfFocusedControl() == caption ) {
					helpmsg = tip;
				}


				EditorGUI.EndChangeCheck();


				if(store !=null){
					EditorGUI.EndDisabledGroup();
				}

			}
				
			// end stores area:
			EditorGUILayout.EndVertical();




			if( string.IsNullOrEmpty(Selected.ProductId) ) {
				//DrawIssues( "Procuct has no Id specified." );
			}
		


			/*if (!platformSetting.isDefault)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = platformSetting.overriddenIsDifferent;
				bool overriddenForAll = GUILayout.Toggle(platformSetting.overridden, "Override for " + platformSetting.name, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					platformSetting.SetOverriddenForAll(overriddenForAll);
					this.SyncPlatformSettings();
				}
			}*/











			//bool togGP = false;
			//togGP = EditorGUILayout.ToggleLeft( "Overwrite with Google Play specific Product Id.", togGP );

			//bool togITC = false;
			//togITC = EditorGUILayout.ToggleLeft( "Overwrite with Apple iTunes specific Product Id.", togITC );



			//EditorGUILayout.EndVertical();



			// EditorGUILayout.
			//EditorGUILayout.BeginHorizontal();
			//GUILayout.Label("",GUILayout.Width(146) );
			//EditorGUILayout.HelpBox("The 'Product Display Name' is the name of the purchase you want to be displayed in the game like 'Big Pack Of Coins' etc.", MessageType.None);
			//EditorGUILayout.EndHorizontal();

			//EditorGUILayout.BeginHorizontal();
			//GUILayout.Label("",GUILayout.Width(146) );
			//EditorGUILayout.HelpBox("The 'Product Display Name' is the name of the purchase you want to be displayed in the game like 'Big Pack Of Coins' etc.", MessageType.None);
			//EditorGUILayout.EndHorizontal();

			// 

		}


		void DrawHelp( string text  ){
			if( string.IsNullOrEmpty(text) ) {
				EditorGUILayout.HelpBox("Select a property to display extra infomation here", MessageType.Info);
				return;
			}
			EditorGUILayout.HelpBox(""+text + " ", MessageType.Info);
		}



		void DrawIssues( string issue ){
			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.HelpBox(""+issue + " ", MessageType.Error);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();


			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
		}


		void AutoDrawProps() {
			var sel = Selected;
			if( sel==null ) return;
			var propertyInfo = new ProductInfoPropertiesInfo();
			propertyInfo.product = sel;
			bool hasChanged = false;
			PropertyDrawer.DrawProperties( propertyInfo, ( IPropertiesInfoSet properties, BTPrjSetting setting )=>{
				return false;
			}, ref hasChanged );
			propertyInfo.hasChanged = hasChanged;
			if( hasChanged ) {
				saveCount=2f;
			}
		}

		public static int selCurr = 0;


		void DrawProductList( ProductInfo[] productInfos ) {
			GUILayout.BeginVertical( new GUILayoutOption[0] );
			GUILayout.BeginVertical( new GUILayoutOption[] { GUILayout.Width(products_width) } );

			if( ShopConfigurationEditor.styles == null ) {
				ShopConfigurationEditor.styles = new ShopConfigurationEditor.Styles();
				ShopConfigurationEditor.styles.toggleSize = ShopConfigurationEditor.styles.toggle.CalcSize(new GUIContent("X"));
				this.lv.rowHeight = (int)ShopConfigurationEditor.styles.levelString.CalcHeight(new GUIContent("X"), 100f);
			}

			GUILayout.Label("Products", ShopConfigurationEditor.styles.title, new GUILayoutOption[0]);


			this.scrollPosition = GUILayout.BeginScrollView( this.scrollPosition, "OL Box" );
			for( int i = 0; i < 1; i++ ) {
				//bool flag = i == 0;
				bool darkLine = false;

				int idx=0;
				foreach( var j in productInfos ) {
					string productName = "";
					try {
						productName = System.Convert.ToString(j.ProductDisplayName);
					} catch (System.Exception e ) {
						Debug.LogError(""+e);
					}

					var cnt = new GUIContent( productName );
					cnt.image = GetProductIcon( j );
					if( cnt.image==null ) {
						//Debug.LogError("asdasd");
					}
					/*if( j.color == "notbuilt" ) {
							cnt.image = nobuilt;
						} else {
							cnt.image = null_built;
						}*/

					ShowProduct( idx++, j.ProductDisplayName,cnt, (!darkLine) ? ShopConfigurationEditor.styles.oddRow : ShopConfigurationEditor.styles.evenRow);
					darkLine = !darkLine;

					//GUILayout.Label(""+jobName);
				}

				//	BuildPlayerWindow.BuildPlatform buildPlatform = buildPlatforms[j];


				GUI.contentColor = Color.white;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			//BuildTarget buildTarget = BuildPlayerWindow.CalculateSelectedBuildTarget();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUI.enabled = true; //(BuildPipeline.IsBuildTargetSupported(buildTarget) && BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) != BuildPipeline.GetBuildTargetGroup(buildTarget));
			/*if (GUILayout.Button("Select Job", new GUILayoutOption[] { GUILayout.Width(110f) })) {
				//EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

				GUIUtility.ExitGUI();
			}*/
			if (GUILayout.Button("New Product", new GUILayoutOption[] { GUILayout.Width(110f) })) {
				//EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
				CreateProduct();
				GUIUtility.ExitGUI();
			}
			if (GUILayout.Button("Save Config", new GUILayoutOption[] { GUILayout.Width(110f) })) {
				//EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
				SaveConfig();
				GUIUtility.ExitGUI();
			}
			if( GUILayout.Button( "Export CSV"/* (google play)"*/, new GUILayoutOption[] { GUILayout.Width(110f) } ) ) {
				string fn  = EditorUtility.SaveFilePanel ("Save CSV", Application.dataPath + "/../", "products", "csv");
				if( !string.IsNullOrEmpty( fn ) ) {
					CSV.ExportForGooglePlay( fn, "US" );
				}
			}
			//selCurr = EditorGUILayout.Popup (selCurr, CSV.Currencies );

			//GUI.enabled = BuildPipeline.IsBuildTargetSupported(buildTarget);
			//if (GUILayout.Button(new GUIContent("Player Settings..."), new GUILayoutOption[] { GUILayout.Width(110f) })) {
			//Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
			//}
			GUILayout.EndHorizontal();
			GUI.enabled = true;
			GUILayout.EndVertical();





		}


		void CreateProduct() {
			ShopConfig.CreateProduct( "NewProduct" + Random.Range(0,int.MaxValue) );
			SaveConfig();
		}


		void SaveConfig() {
			EditorUtility.SetDirty( ShopConfig.instance );
			EditorApplication.SaveAssets();
			AssetDatabase.SaveAssets( );
			AssetDatabase.Refresh( );
		}


		void ShowProduct(  int idx, string productName,  GUIContent title, GUIStyle background ) {
			Rect rect = GUILayoutUtility.GetRect(50f, 36f);
			rect.x += 1f;
			rect.y += 1f;
			bool canBuild = true;//BuildPipeline.LicenseCheck(bp.DefaultTarget);
			GUI.contentColor = new Color(1f, 1f, 1f, (!canBuild) ? 0.7f : 1f);
			bool sel = idx==SelectedProductIdx; //;//idx == 0; // false; //EditorUserBuildSettings.selectedBuildTargetGroup == bp.targetGroup;
			if (Event.current.type == EventType.Repaint) {
				background.Draw( rect, GUIContent.none, false, false, sel, false);
				GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), title.image, GUIStyle.none);
				//if (BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) == bp.targetGroup) {
				//GUI.Label(new Rect(rect.xMax - (float)JenkinsDash.styles.activePlatformIcon.width - 8f, rect.y + 3f + (float)((32 - JenkinsDash.styles.activePlatformIcon.height) / 2), (float)JenkinsDash.styles.activePlatformIcon.width, (float)JenkinsDash.styles.activePlatformIcon.height), JenkinsDash.styles.activePlatformIcon, GUIStyle.none);
				//}
			}
			if( GUI.Toggle(rect, sel, title.text, ShopConfigurationEditor.styles.platformSelector) /*&& EditorUserBuildSettings.selectedBuildTargetGroup != bp.targetGroup*/ ) {
				//Debug.Log("toggle");
				SelectedProductIdx = idx;
				var last =  selProd;
				selProd = Products[SelectedProductIdx]; 
				if( last != selProd ) {
					GUI.FocusControl("ssadasdf"); //focus nothing.
				}
				loadSelProd=true;
				//EditorUserBuildSettings.selectedBuildTargetGroup = bp.targetGroup;
				//UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(InspectorWindow));
				//for (int i = 0; i < array.Length; i++) 	{
				/*InspectorWindow inspectorWindow = array[i] as InspectorWindow;
						if (inspectorWindow != null)
						{
							inspectorWindow.Repaint();
						}*/
				//}
			}
		}


		double ts = -1;
		void Update(){
			if( ts == -1 ) {
				ts = EditorApplication.timeSinceStartup;
			}
			var dt = EditorApplication.timeSinceStartup - ts;

			//Debug.Log("dt=" + dt  );
			ts = dt + ts;
			if( saveCount > 0f ) {
				saveCount -= (float)(dt)*1f;
				//Debug.Log("saveCount = "+saveCount);
				if( saveCount <= 0f ) {
					
					SaveConfig();
				}
			}
		}


		static Styles styles;

		private class Styles {
			//public const float kButtonWidth = 110f;
			public GUIStyle selected = "ServerUpdateChangesetOn";
			public GUIStyle box = "OL Box";
			public GUIStyle title = "OL title";
			public GUIStyle evenRow = "CN EntryBackEven";
			public GUIStyle oddRow = "CN EntryBackOdd";
			public GUIStyle platformSelector = "PlayerSettingsPlatform";
			public GUIStyle toggle = "Toggle";
			public GUIStyle levelString = "PlayerSettingsLevel";
			public GUIStyle levelStringCounter = new GUIStyle("Label");
			public Vector2 toggleSize;
			public Texture2D activePlatformIcon = EditorGUIUtility.IconContent("BuildSettings.SelectedIcon").image as Texture2D;

			public GUIContent standaloneTarget;
			public GUIContent architecture;
			public GUIContent webPlayerStreamed;
			public GUIContent webPlayerOfflineDeployment;
			public GUIContent debugBuild;
			public GUIContent profileBuild;
			public GUIContent allowDebugging;
			public GUIContent symlinkiOSLibraries;
			public GUIContent explicitNullChecks;
			public GUIContent enableHeadlessMode;
			public GUIContent webGLOptimizationLevel;
			public GUIContent buildScriptsOnly;
			public Styles()
			{
				

				this.standaloneTarget = KEditorGUIUtility.TextContent("BuildSettings.StandaloneTarget");
				this.architecture = KEditorGUIUtility.TextContent("BuildSettings.Architecture");
				this.webPlayerStreamed = KEditorGUIUtility.TextContent("BuildSettings.WebPlayerStreamed");
				this.webPlayerOfflineDeployment = KEditorGUIUtility.TextContent("BuildSettings.WebPlayerOfflineDeployment");
				this.debugBuild = KEditorGUIUtility.TextContent("BuildSettings.DebugBuild");
				this.profileBuild = KEditorGUIUtility.TextContent("BuildSettings.ConnectProfiler");
				this.allowDebugging = KEditorGUIUtility.TextContent("BuildSettings.AllowDebugging");
				this.symlinkiOSLibraries = KEditorGUIUtility.TextContent("BuildSettings.SymlinkiOSLibraries");
				this.explicitNullChecks = KEditorGUIUtility.TextContent("BuildSettings.ExplicitNullChecks");
				this.enableHeadlessMode = KEditorGUIUtility.TextContent("BuildSettings.EnableHeadlessMode");
				this.webGLOptimizationLevel = KEditorGUIUtility.TextContent("BuildSettings.WebGLOptimizationLevel");
				this.buildScriptsOnly = KEditorGUIUtility.TextContent("BuildSettings.BuildScriptsOnly");

				this.levelStringCounter.alignment = TextAnchor.MiddleRight;
			}
		
		}
	}

}



