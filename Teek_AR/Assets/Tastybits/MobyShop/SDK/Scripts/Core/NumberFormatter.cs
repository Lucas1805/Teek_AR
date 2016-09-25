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
	 * Simple Utilty class to help format numbers.
	 */
	public class NumberFormatter  {

		/**
		 *  Format the price 
		 */
		public enum NumberFormat {
			Number = 0,
			Currency = 1
		}


		static public string FormatNumber( string value, NumberFormat fmt ) {
			if( value.Length > 3 && fmt == NumberFormat.Currency ) {
				value = value.Insert( value.Length - 3, "." );
			}
			return value;
		}
	}

}