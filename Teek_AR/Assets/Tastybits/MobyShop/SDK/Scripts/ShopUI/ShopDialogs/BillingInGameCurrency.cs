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
	 * Billing in the Ingame currency needs to be confirmed or cancelled.
	 */
	public class BillingInGameCurrency : ShopConfirm {
		public enum AcceptOrCancel {
			Accepted,
			Cancelled
		}
		public UnityEngine.UI.Text text;


		public override void Show() {
			this.gameObject.SetActive(false);
		}


		public void OnAcceptClicked() {
			this.gameObject.SetActive(false);
			if( onDismissed!=null){
				onDismissed.Invoke( AcceptOrCancel.Accepted );
			}
		}

		public void OnCancelClicked() {
			this.gameObject.SetActive(false);
			if( onDismissed!=null ){
				onDismissed.Invoke( AcceptOrCancel.Cancelled );
			}
		}

		//public System.Action<AcceptOrCancel> OnDismiss;

	}

}