using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif 

namespace MobyShop.UI {
	

	/**
	 * The Tabbar widget is a widget to make a tabbar with items to be triggered. 
	 */
	[ExecuteInEditMode]
	public class ShopTabbarWidget : MonoBehaviour {
		// The active index represents the index of the page in the 
		// tabbar which is have activated. 
		public int activeIndex = 0;

		// Tint tabbar buttons this color when active.
		public Color colorActive = Color.white;

		// Tint tabbra buttons this color when they are inactive.
		public Color colorInactive = Color.white;

		// set this to true if you want the tabbar to change the selected button
		// after creation.
		public bool updateInEditMode = true;

		// the initial index of the item we want the app to be active with the first time
		// the tabbar is initiaized.
		public int initialIndex = 0;

		// Event Listeners.
		public GameObject[] OnTabClickedListeners;

		/**
		 * Sets the active target to the  initial index and then makes sure that the initial tabbar item is activated.
		 */
		void Awake() {
			if( Application.isPlaying ) 	
				activeIndex = initialIndex;
			ShopTabbarTabWidget[] tabs = this.gameObject.GetComponentsInChildren<ShopTabbarTabWidget>(true);
			foreach( var tab in tabs ) {
				tab.SetTabActive( tab.index == activeIndex, colorActive , colorInactive );
			}
		}

		/**
		 * Invoked when the tabbar is clicked.
		 */
		public void OnTabClicked( GameObject go ) {
			var tabClicked = go.GetComponent<ShopTabbarTabWidget> ();
			ShopTabbarTabWidget[] tabs = this.gameObject.GetComponentsInChildren<ShopTabbarTabWidget> (true);
			var old = activeIndex;
			foreach (var tab in tabs) {
				tab.SetTabActive (tab == tabClicked, colorActive, colorInactive);
				if (tab == tabClicked) {
					activeIndex = tab.index;
				}
			}
			if (OnTabClickedListeners != null && old != activeIndex ) {
				foreach( var golistener in OnTabClickedListeners ) {
					golistener.SendMessage ("OnTabbarChanged", tabClicked.index, SendMessageOptions.DontRequireReceiver);
				}
			}
		}


		/**
		 * Changes the seleted tabbar item.
		 */
		public void SetSelected( int selindex ) {
			bool fnd = false;
			ShopTabbarTabWidget[] tabs = this.gameObject.GetComponentsInChildren<ShopTabbarTabWidget> (true);
			foreach (var tab in tabs) {
				tab.SetTabActive (tab.index == selindex, colorActive, colorInactive);
				if (tab.index == selindex) {
					activeIndex = tab.index;
					fnd = true;
				}
			}
			if( OnTabClickedListeners != null && fnd ) {
				foreach( var golistener in OnTabClickedListeners ) {
					golistener.SendMessage ("OnTabbarChanged", selindex, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		/**
		 * Updates the ShopTabbarWidget to show 
		 */
#if UNITY_EDITOR
		void Update(){
			if( Application.isEditor && Application.isPlaying == false && updateInEditMode ) {
				ShopTabbarTabWidget[] tabs = this.gameObject.GetComponentsInChildren<ShopTabbarTabWidget>(true);
				if( activeIndex <= 0 ) { activeIndex = 0; }
				if( activeIndex>=tabs.Length ) activeIndex = tabs.Length - 1;
				if( activeIndex == -1 ) return;
				foreach( var tab in tabs ) {
					tab.SetTabActive( tab.index == activeIndex, colorActive, colorInactive );

					int fnd=0;
					GameObject obj=null;
					foreach( var tab2 in tabs ) {
						if( tab2.index == tab.index ) { 
							fnd++;
							obj = tab2.gameObject;
						}
					}
					if( fnd >= 2 ) {
						Debug.LogError("Error more than one tabbar Tab has the index set to index #" + tab.index, obj );
					}
				}
			}
		}
#endif


	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ShopTabbarWidget))]
	public class TabbarEditor : Editor {
		public override void OnInspectorGUI() {
			GUILayout.Label("Click a tab to change th active tab");
	
			var targ = this.target as ShopTabbarWidget;

			ShopTabbarTabWidget[] tabs = targ.gameObject.GetComponentsInChildren<ShopTabbarTabWidget>(true);
			System.Collections.Generic.List<ShopTabbarTabWidget> tablist = tabs.ToList();
			var descend = tablist.OrderBy( o=>o.index );
			var tmp = new System.Collections.Generic.List<string>();
			foreach( var e in descend ) {
				tmp.Add( e.name );
			}
			GUILayout.Label("Num items #" + tmp.Count );
			targ.activeIndex = Tabs( tmp.ToArray(), targ.activeIndex );

			base.OnInspectorGUI ();

		}
		public static int Tabs(string[] options, int selected)
		{
			const float DarkGray = 0.4f;
			const float LightGray = 0.9f;
			const float StartSpace = 10;

			GUILayout.Space(StartSpace);
			Color storeColor = GUI.backgroundColor;
			Color highlightCol = new Color(LightGray, LightGray, LightGray);
			Color bgCol = new Color(DarkGray, DarkGray, DarkGray);

			GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.padding.bottom = 8;

			GUILayout.BeginHorizontal();
			{   //Create a row of buttons
				for (int i = 0; i < options.Length; ++i)
				{
					GUI.backgroundColor = i == selected ? highlightCol : bgCol;
					if (GUILayout.Button(options[i], buttonStyle))
					{
						selected = i; //Tab click
					}
				}
			} GUILayout.EndHorizontal();
			//Restore color
			GUI.backgroundColor = storeColor;
			//Draw a line over the bottom part of the buttons (ugly haxx)
			var texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, highlightCol);
			texture.Apply();
			GUI.DrawTexture(new Rect(0, buttonStyle.lineHeight + buttonStyle.border.top + buttonStyle.margin.top + StartSpace,  Screen.width, 4),texture);

			return selected;
		}
	}
#endif

}


