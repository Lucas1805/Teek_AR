/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using UnityEditor;


namespace MobyShop {

	[CustomEditor(typeof(ShopConfig))]
	public class ProductCatalogueEditor : UnityEditor.Editor {

		public override void OnInspectorGUI(){
			if( GUILayout.Button("Open Configuration") ) {
				ShopConfigurationEditor.Open( this.target as ShopConfig );
			}
		}

	}

}