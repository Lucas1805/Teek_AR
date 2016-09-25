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
	 * Hides the object when its run on device, this is usually becaurse we want to show the object
	 * Only in the UnityEditor.
	 */
	public class HideInPlayer : MonoBehaviour {
		// Use this for initialization
		void Start () {
			if (Application.isEditor == false) {
				this.gameObject.SetActive (false);
			}
		}
	}


}