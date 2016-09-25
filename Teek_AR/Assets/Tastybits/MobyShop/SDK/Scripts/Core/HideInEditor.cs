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
	 * Small utility to hide material in the Unity editor.
	 */ 
	public class HideInEditor : MonoBehaviour {
		void Start () {
			if (Application.isEditor)
				this.gameObject.SetActive (false);
		}
	}


}