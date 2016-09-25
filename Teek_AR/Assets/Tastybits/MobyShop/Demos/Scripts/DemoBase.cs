/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using MobyShop.UI;


namespace MobyShop {


	/**
	 * The demo base is a class used for the demo's and is used to make sure that the
	 */
	public class DemoBase : MonoBehaviour {
		public enum State {
			FrontPage = 0,
			ShowingShop,
		}
		public State state = State.FrontPage;


		/**
		 * 
		 */
		void Start() {
			state = State.FrontPage;
		}
			

		/**
		 * Draw a button.
		 */
		public bool DrawButton( string caption= "Show Shop" ){
			float btnheight = Screen.height / 8f;
			var space = Screen.width* 0.05f;
			var btnwidth = Screen.width-space-space;
			float vertical_space = Screen.height / 30f;
			return GUI.Button (new Rect (space, Screen.height - btnheight - vertical_space, btnwidth, btnheight), caption, ButtonStyle );
		}


		/**
		 * Draw the menu in the demo.
		 */
		protected bool DrawDemoMenu( System.Collections.Generic.List<string> items=null, System.Action<int> callback=null ){
			if( state != State.FrontPage ) {
				return true;
			}

			if (items != null && callback != null) {
				float btnheight = Screen.height / 11f;
				var space = Screen.width* 0.05f;
				var btnwidth = Screen.width-space-space;

				float offset = 0f;//(Screen.height / 3f)
				float vertical_space = Screen.height / 40f;

				int it = 0;
				foreach (var button in items) {
					if( GUI.Button( new Rect(space,space+offset, btnwidth, btnheight), button, ButtonStyle ) ) {
						callback (it);
					}
					offset += btnheight + vertical_space;
					it++;
				}
			}

			return false;
		}


		/**
		 * Button Style. this is the style of the button we daw.
		 */
		GUIStyle buttonStyle;
		GUIStyle ButtonStyle {
			get {
				if( buttonStyle == null ) {
					buttonStyle = new GUIStyle( GUI.skin.button ); 
					buttonStyle.fontSize = Mathf.RoundToInt( Screen.width / 25f );
				}
				return buttonStyle;
			}
		}


	}

}



