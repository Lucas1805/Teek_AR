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
	 * The ShopItem Extra UI Data is an example of a component used to define extra data for ShopItem
	 * That only makes sense in UI context. In this case we define some colors for each product shown. 
	 * 
	 * These colors are resused to color the UI diffirent according to what buttons you click.
	 * 
	 * This type of infomation doesn't make sense per say to implement in the MobyShop SDK therefore you can implement it here.
	 */
	public class ShopItemExtraUIData : MonoBehaviour {
		public Color colorBackground = Color.white;
		public Color colorGradient = Color.white;
		public Color colorBehindIcon = Color.white;
		public bool clickedIconTransition = true;
	}

}