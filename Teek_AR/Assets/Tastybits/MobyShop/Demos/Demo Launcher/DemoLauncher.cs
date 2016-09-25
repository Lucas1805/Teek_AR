using UnityEngine;
using System.Collections;

namespace MobyShop {


	/**
	 * Launches the demo.
	 */ 
	public class DemoLauncher : MobyShop.DemoBase {
		void OnGUI () {
			var buttons = new System.Collections.Generic.List<string> ();
			buttons.Add( "Demo1 - Getting Started" );
			buttons.Add( "Demo2 - ShopTempate1" );
			buttons.Add( "Demo3 - ShopTemplate2" );
			buttons.Add( "Demo4 - ShopTemplate3" );
			buttons.Add( "Demo5 - ShopTemplate4" );
			buttons.Add( "Demo6 - Restore Purchases" );
			bool simOn = (PlayerPrefs.GetInt ("MobyShop_BillingSimulator", 0) != 0 ? true : false);
			buttons.Add( "Billing Simulator: " + simOn );
			DrawDemoMenu (buttons, (int clicked) => {
				var clickedItem = buttons[clicked];
				Debug.Log("clickedItem =  " + clickedItem + " index = "+ clicked );
				if( clickedItem.StartsWith( "Billing Simulat") ) {
					simOn = !simOn;
					PlayerPrefs.SetInt ("MobyShop_BillingSimulator", simOn ? 1 : 0 );
					return;
				}
				Debug.Log( "Loading : " + clickedItem );
				UnityEngine.SceneManagement.SceneManager.LoadScene( clickedItem );
			});
		}
	}


}