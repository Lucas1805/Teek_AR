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
	 * The ShopNotEnoughCoinsBase defines the logic used for that particular type of dialog shown when you don't have enough coins available 
	 * You can enherit and override the methods in this class to make diffirent types of dialogues for this purpose.
	 */
	public class ShopNotEnoughCoins : MonoBehaviour {
		// This can be set to be listened on.
		public System.Action<Dismissed> onDismissed;

		/**
		 * This definition is used to define which results the dialog will be able to return with.
		 */ 
		public enum Dismissed {
			Cancel = 0,
			BuyMoreCoins = 1
		}

		/**
		 * Must be implemented to show the dialog.
		 */
		public virtual void Show( ) {
			throw new System.NotImplementedException( "You must subclass ShopNotEnoughCoins and implement 'Show' in order to customize this type of dialog." );
		}
	}

}