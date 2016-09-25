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
	 * Draws the Inspector for the Shop component.
	 */
	[UnityEditor.CustomEditor(typeof(Shop))]
	public class ShopInspector : UnityEditor.Editor {

		public override void OnInspectorGUI(){
			var targ = this.target as Shop;
			//base.OnInspectorGUI();
			targ._verbose  = UnityEditor.EditorGUILayout.ToggleLeft("Verbose/Debugging", targ._verbose );

			ShopConfigurationEditor.DrawProductListSimple();
			if( GUILayout.Button("\nOpen Shop Config\n( Manage Products )\n") ) {
				ShopConfigurationEditor.Open();
			}
				
			targ.overrideEmbeddedShopConfig  = UnityEditor.EditorGUILayout.ToggleLeft("Override Embedded Shop Config File", targ.overrideEmbeddedShopConfig );
			if( targ.overrideEmbeddedShopConfig  ) {
				GUI.enabled=true;
			} else {
				GUI.enabled=false;
			}
			targ.shopConfigInst = (ShopConfig)UnityEditor.EditorGUILayout.ObjectField( "Shop Config", targ.shopConfigInst, typeof(ShopConfig), false);


			GUI.enabled=true;
		}




	}


}