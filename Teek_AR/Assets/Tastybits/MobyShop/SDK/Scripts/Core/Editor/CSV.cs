/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using System.Linq;


namespace MobyShop {


	/**
	 * CSV
	 */
	public class CSV {



		public static string[] Currencies {
			get {
				System.Collections.Generic.List<string> currencies = new System.Collections.Generic.List<string> () {
					"DK", "CL", "CR", "KZ", "TW", "PE", "LB", "FI", "JP", "MY", "IT", "HK", "LU", "BR", "CO", "MX", "PK", "SA", "TH", "GB", "VN", "OM", "SI", "PH", "EE", "EG", "BO", "PL", "AU", "NG", "IL", "MA", "ID", "NO", "CA", "BH", "FR", "UA", "NL", "LI", "RO", "CZ", "IE", "IN", "CH", "LT", "NZ", "SE", "CY", "HU", "AE", "GR", "BG", "TR", "QA", "ZA", "KR", "PT", "US", "DE", "RU", "ES", "SG", "SK", "AT", "BE", "KW"
				};
				return currencies.ToArray();
			}
		}

		/**
		 * Export the CSV - to google play; for google play import.
		 */
		public static void ExportForGooglePlay( string fileName, string defaultCurrency ) {


			string outp = "Product ID,Published State,Purchase Type,Auto Translate,Locale; Title; Description,Auto Fill Prices,Price,Pricing Template ID\n";

			
			foreach( var product in ShopConfig.ProductList ) {
				if( product.billing != BillingType.RealLifeCurrency ) {
					//Debug.Log ("Ignoring export of product : " + product.ProductDisplayName + " it's a product payed with ingame currency");
					continue;
				}
				Debug.Log ("Exporting product : " + product.ProductDisplayName );
				if( string.IsNullOrEmpty (product.BId_Android) && string.IsNullOrEmpty(product.BillingId) ) {
					throw new System.Exception ("Error: BillingId is not set for android");
				}
				if( string.IsNullOrEmpty (product.ProductDisplayName) ) {
					throw new System.Exception ("Error: ProductDisplayName is not set");
				}
				if( string.IsNullOrEmpty (product.PriceTag) ) {
					throw new System.Exception ("Error: PriceTag is not set");
				}

				string line = "";


				string billingId = product.BId_Android;
				if( string.IsNullOrEmpty(billingId) ) {
					billingId = product.BillingId;
				}

				line += billingId + ",";
				line += "published,";
				line += "managed_by_android,";
				line += "false,";
				line += "en_US; " + product.ProductDisplayName + "; " + product.DescriptionShort.Replace("\n","") + ",";
				line += "false,";

				//Debug.Log ("defaultCurrency= " + defaultCurrency);

				string price = "DK; 30000000; CL; 2900000000; CR; 2500000000; KZ; 1590000000; TW; 140000000; PE; 14990000; LB; 6750000000; FI; 4990000; JP; 460000000; MY; 17990000; IT; 4890000; HK; 34900000; DK; 37000000; LU; 4690000; BR; 13990000; CO; 13000000000; MX; 85000000; PK; 470000000; SA; 16990000; TH; 150000000; GB; 4190000; VN; 100000000000; OM; 4490000; SI; 4890000; PH; 210000000; EE; 4790000; EG; 39990000; BO; 30990000; PL; 20990000; AU; 5990000; NG; 1400000000; IL; 17000000; MA; 43990000; ID; 59000000000; NO; 46000000; CA; 5990000; BH; 4490000; FR; 4790000; UA; 109990000; NL; 4890000; LI; 4400000; RO; 21990000; CZ; 129990000; IE; 4990000; IN; 300000000; CH; 4400000; LT; 4890000; NZ; 6490000; SE; 48000000; CY; 4790000; HU; 1590000000; AE; 16990000; GR; 4990000; BG; 9490000; TR; 12990000; QA; 16000000; ZA; 60990000; KR; 5000000000; PT; 4990000; US; 4490000; DE; 4790000; RU; 289000000; ES; 4890000; SG; 5990000; SK; 4790000; AT; 4790000; BE; 4890000; KW; 4490000";
				var priceLines = price.Split( new char[]{ ';', ' ' }, System.StringSplitOptions.RemoveEmptyEntries );
				for (int i = 0; i < priceLines.Length-1; i++) {
					string country = priceLines[i];
					if (i == 0 && defaultCurrency != country) {
						//Debug.Log ("adding default : " + defaultCurrency);
						var index = priceLines.ToList ().IndexOf (defaultCurrency);
						line += priceLines [index] + "; " + priceLines [index+1] + "; ";
					}
					i++;
					string value = priceLines [i].Trim ();
					line += country + "; " + value;
					if( i+1 < priceLines.Length-1 ) {
						line += "; ";
					}
				}
				line += ",\n";

				//line += price + ",\n";

				/*System.Collections.Generic.List<string> currencies = new System.Collections.Generic.List<string> () {
					"DK", "CL", "CR", "KZ", "TW", "PE", "LB", "FI", "JP", "MY", "IT", "HK", "LU", "BR", "CO", "MX", "PK", "SA", "TH", "GB", "VN", "OM", "SI", "PH", "EE", "EG", "BO", "PL", "AU", "NG", "IL", "MA", "ID", "NO", "CA", "BH", "FR", "UA", "NL", "LI", "RO", "CZ", "IE", "IN", "CH", "LT", "NZ", "SE", "CY", "HU", "AE", "GR", "BG", "TR", "QA", "ZA", "KR", "PT", "US", "DE", "RU", "ES", "SG", "SK", "AT", "BE", "KW"
				};
				System.Collections.Generic.Dictionary<string, int> values = new System.Collections.Generic.Dictionary<string, int>() {
					{ "DK", 2900 }, { "CL", 290000 }, {"CR", 250000 }, { "KZ", 159000 }, { "TW", 14000 }, { "PE", 1499 }, { "LB", 675000 }, { "FI", 499 }, { "JP", 46000 }, { "MY", 1799 },
					{ "IT", 489 }, { "HK", 3490 }, { "LU", 469 }, { "BR", 1399 }, { "CO", 1300000 }, { "MX", 8500 }, { "PK", 47000 }, { "SA", 1699 }, { "TH", 15000 }, { "GB", 419 }, { "VN", 10000000 }, { "OM", 449 }, { "SI", 489 }, { "PH", 21000 }, { "EE", 479 }, 
					{ "EG", 3999 }, { "BO", 3099 }, { "PL", 2099 }, { "AU", 599 },  { "NG", 140000 }, { "IL", 1700 }, { "MA", 4399 }, { "ID", 5900000 }, { "NO", 4600 }, { "CA",  599 }, 
					{ "BH", 449 }, { "FR", 479 }, { "UA", 10999 }, { "NL", 489 }, { "LI", 440 }, { "RO", 2199 }, { "CZ", 12999 }, { "IE", 499 }, { "IN", 30000 }, { "CH", 440 }, 
					{ "LT", 489 }, { "NZ", 649 }, { "SE", 4800 }, { "CY", 479 }, { "HU", 159000 }, { "AE", 1699 }, { "GR", 499 }, { "BG", 949 }, { "TR", 1299 }, { "QA", 1600 }, 
					{ "ZA", 6099 }, { "KR", 500000 }, { "PT", 499 }, { "US", 449 }, { "DE", 479 }, { "RU", 28900 }, { "ES", 489 }, { "SG", 599 }, { "SK", 479 }, { "AT", 479 }, { "BE", 489 }, { "KW", 449 } 
				};
				int it = 0;
				foreach( var curr in currencies ) {
					var diff = (float)values[curr] / (float)values["US"];
					float priceInDollars = 99f * (float)product.price;
					float totPrice = priceInDollars * diff;
					int intPrice = Mathf.RoundToInt (totPrice);
					if (curr == "CL") {
						intPrice = intPrice / 1000;
						intPrice *= 1000;
					}
					if (curr == "JP") {
						intPrice = intPrice / 100;
						intPrice *= 100;
					}
					if (curr == "KR") {
						intPrice = intPrice / 1000;
						intPrice *= 1000;
					}
					string strPrice = intPrice + "0000";
					it++;
					if (it >= currencies.Count) {
						line += curr + "; " + strPrice + ",\n";
						break;
					} else {
						line += curr + "; " + strPrice + "; ";
					}
				}*/


				outp += line;
			}
			
			System.IO.File.WriteAllText (fileName,outp);
		}
	}

}