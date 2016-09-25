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
	 * This class is used to hide the an element in the Start event. It will only deactivate the gameobject 
	 * if the frame count is less than the specified amount or if the specified amount is -1.
	 * 
	 * When the compoent has been activated the first time it's deleted.
	 */
	public class HideOnPlay : MonoBehaviour {
		public int maxFrameCount = 1;
		void Start () {
			if (maxFrameCount < 0 || Time.frameCount < maxFrameCount) {
				this.gameObject.SetActive (false);
			} else {
				// Ignore hiding the object. ( useful for only hiding UI elements when the game initializes before the first frame is drawn. )
			}
			Component.Destroy( this );
		}
	}


}