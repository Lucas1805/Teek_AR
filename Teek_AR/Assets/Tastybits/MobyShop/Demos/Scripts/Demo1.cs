/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using MobyShop.UI;


namespace MobyShop {

	/**
	 * This demo illustrates how simple it is to get started with mobyshop.
	 * 
	 */
	public class Demo1 : DemoBase {
		void OnGUI(){
			if( DrawDemoMenu () )
				return;
			if( DrawButton() ) {
				state = State.ShowingShop;
				MobyShop.Shop.ShowShopUI (0,()=>{
					state = State.FrontPage;
				});
			}
		}
	}

}
