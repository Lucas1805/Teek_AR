/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop.UI {


	public class OneProductShopUI : ShopUIBase {
		public GameObject closeButton;
		public GameObject[] shopItems;
		public bool verbose = false;

		static OneProductShopUI _instance=null;
		public static OneProductShopUI instance {
			get {
				if( _instance == null ) {
					var gos = Object.FindObjectsOfType<GameObject>();
					foreach( var go in gos ) {
						var tmp = go.GetComponentInChildren2<OneProductShopUI>(true);
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
		 * Set the singleton instance as well as deactivating if the game just started.
		 */
		void Awake() {
			_instance=this;
		}

		// Support recompile and reset the singleton instance.
#if UNITY_EDITOR
		void Update() {
			if( _instance == null ){
				_instance=this;
			}
		}
#endif

		/**
		 * Called when the close button is clicked.
		 */
		public void OnCloseClicked() {
			OnClose ();
		}


	
		/**
		 * reset the position on enabling the view.
		 */
		void OnEnable(){
			this.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		}


		/**
		 * Override the Show from ShopUIBase in order to show the view.
		 */
		public override void Show( int groupId, System.Action cbOnDismiss = null ) {
			base.Show (groupId, cbOnDismiss);
		}


		/**
		 * Call this to show a view of this type. Will fail if there is no instance of this object availble.
		 */
		public static void ShowView( System.Action cbOnDismiss = null ) {
			if( instance==null ) {
				Debug.LogError("Error  locating ShopUI maybe you forgot to insert the prefab into the Canvas. it it's living in another scene");
				return;
			}
			instance.Show( 0, cbOnDismiss );
		}


	}

}
