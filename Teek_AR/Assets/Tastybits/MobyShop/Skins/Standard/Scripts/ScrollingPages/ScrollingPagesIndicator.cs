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
	 * The Scrolling Pages indicator is a small widget shown on the scrolling pages
	 * it is used to indicate which page is the active page. 
	 * 
	 * NOTE: There is an issue when displaying less than 3 elements in the widget and that would need to be handled.
	 */
	public class ScrollingPagesIndicator : MonoBehaviour {
		// the template is a template of one of the indicator objects.
		public GameObject template;

		// The active page index / selected page index.
		public int activeIndex = 0;

		// initial number of items expected in the scrolling pages component.
		public int numItems=3;

		// sprite used to indicate the active page.
		public Sprite spriteActive;

		// sprite used to indicate the inactive pages.
		public Sprite spriteInactive;

		// collection of items.
		public GameObject[] items;

		/**
		 * Createa a template if none is present.
		 * Clears the widget.
		 */
		void Awake() {
			if( template == null ) {
				template = new GameObject("template");
				var tf = template.GetComponent<RectTransform>();
				template.AddComponent<UnityEngine.UI.Image>();
				tf.SetParent( rt, false );
			}
			template.SetActive(false);
			Clear();
			CreateItems( numItems );
		}


		/**
		 * Property that returns the rect transform.
		 */
		public RectTransform rt {
			get {
				var _rt = this.GetComponent<RectTransform>();
				return _rt; 
			}
		}


		/**
		 * Clers the indicator wiget so no indicator dots are available.
		 */
		public void Clear() {
			var tmp = new System.Collections.Generic.List<GameObject>();
			for( int i=0; i<rt.childCount;  i++ ) {
				var ch = rt.GetChild(i).gameObject;
				if( ch != template ) { 
					tmp.Add( ch );
				}
			}
			foreach( var go in tmp ) {
				GameObject.Destroy( go );
			}
		}


		/**
		 * Sets the active index.
		 */
		public void SetActiveIndex( int index ) {
			//Debug.Log( "SetActiveIndex: " + index );
			if( index < 0 ) Debug.LogError("invalid index #" + index + " - less than zero / out of bounds" );
			int numitems = items != null ? items.Length : 0;
			if( index >= numitems ) {
				//Debug.LogError("invalid index #" + index + " - out of bounds; numitems = " + numitems );
				return;
			}
			activeIndex = index;
			if( items!=null) {
				for( int i=0; i<items.Length; i++ ) {
					var img = items[i].GetComponent<UnityEngine.UI.Image>();
					img.sprite = this.activeIndex == i ? spriteActive : spriteInactive;
				}
			}
		}


		/**
		 * Creates a set of items. 
		 * The number given is the number of items created.
		 */
		public void CreateItems( int num ) {
			if( num < 0 ) throw new System.Exception("out of bounds");
			if( template==null)Debug.LogError("Cannot instianciate scrolling pages indicator item since there is no template set", this.gameObject );
			var tmp = new System.Collections.Generic.List<GameObject>();
			float sz = 1f / (float)num;
			float offx = 0f;
			for( int i=0; i<num; i++ ) {
				var go = GameObject.Instantiate(template);

				// Transform...
				var tf = go.GetComponent<RectTransform>();
				tf.SetParent( this.rt, false );
				go.SetActive(true);
				var lp = tf.localPosition;
				tf.localPosition = lp;

				tf.anchorMin = new Vector2(offx,0f); 
				offx += sz;
				tf.anchorMax = new Vector2(offx,1f); 
				tf.offsetMin = tf.offsetMax = Vector2.zero;

				// Sprite: 
				var spr = this.activeIndex == i ? spriteActive : spriteInactive;
				var img = go.GetComponent<UnityEngine.UI.Image>();
				img.sprite = spr;

				tmp.Add(go);
			}
			items = tmp.ToArray();
		}
	}

}