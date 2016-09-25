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
	 * This is a UI class used to show some UI for billing in the editor.
	 * it's needed when testing the billing inside the editor.
	 */
	public class SimulatedBillingUI : MonoBehaviour {

		public System.Action<bool> callback;

		void Awake(){
			if(Time.frameCount<=1 ) {
				this.gameObject.SetActive(false);
			}
		}


		void OnEnable(){
			this.transform.localPosition = Vector3.zero;
		}


		public void OnOkClicked(  ) {
			if( MobyShop.Shop.Verbose ) Debug.Log("MobyShop: EditorStore - ok");
			if( callback!=null ) callback(true);
			this.gameObject.SetActive(false);
		}


		public void OnCancelClicked() {
			if( MobyShop.Shop.Verbose ) Debug.Log("MobyShop: EditorStore - cancel");
			if( callback!=null ) callback(false);
			this.gameObject.SetActive(false);
		}


	}

}