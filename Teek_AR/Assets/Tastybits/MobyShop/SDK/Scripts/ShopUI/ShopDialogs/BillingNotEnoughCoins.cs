/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;

namespace MobyShop {


	public class BillingNotEnoughCoins : ShopNotEnoughCoins {

		public UnityEngine.UI.Text text;


		public void OnAcceptClicked() {
			this.gameObject.SetActive(false);
			if( OnDismiss!=null){
				OnDismiss.Invoke(  );
			}
		}

		public void OnCancelClicked() {
			this.gameObject.SetActive(false);
			if( OnDismiss!=null ){
				OnDismiss.Invoke(  );
			}
		}

		public System.Action OnDismiss;

	}

}