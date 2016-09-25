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


	/**
	 * Editor overwrite for the advanced image.
	 */
	[CustomEditor(typeof(UnityEngine.UI.MobyShop.AdvImage))]
	public class AdvImageEditor : UnityEditor.UI.ImageEditor {


		public override void OnInspectorGUI () {
			try {
				base.OnInspectorGUI ();
			}catch ( System.Exception e ) {
				Debug.LogError ("" + e);
				//EditorGUIUtility.ExitGUI ();
			}

			var targ = this.target as UnityEngine.UI.MobyShop.AdvImage;

			//targ.maskable = EditorGUILayout.ToggleLeft ("maskable", targ.maskable);
			//var maskable = targ as UnityEngine.UI.MaskableGraphic;

			targ.borderGivenInPct = EditorGUILayout.ToggleLeft ( "Border in Percent", targ.borderGivenInPct);

			targ.borderPctLeft 		= EditorGUILayout.FloatField ( "Border Left", 	targ.borderPctLeft);
			targ.borderPctRight 	= EditorGUILayout.FloatField ( "Border Right", 	targ.borderPctRight);
			targ.borderPctTop 		= EditorGUILayout.FloatField ( "Border Top", 	targ.borderPctTop);
			targ.borderPctBottom 	= EditorGUILayout.FloatField ( "Border Bottom", targ.borderPctBottom);


			//EditorGUILayout.LabelField( "Material Name", "" + targ.material.name );
			//EditorGUILayout.LabelField( "layoutPriority", "" + targ.layoutPriority );
			//var material = EditorGUILayout.ObjectField (maskable.GetModifiedMaterial (targ.material), typeof(Material), false);


		}

	}

}