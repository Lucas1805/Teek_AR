/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop {

	[System.Serializable]
	public class StoreSetting {
		public StoreSetting( string name, string dispname ){
			this.name = name;
			this.dispname = dispname;
		}
		public Texture2D SmallIcon {
			get {
				if( smallIcon!=null ) return smallIcon;
#if UNITY_EDITOR
				string fname = "MobyShop/"+name+"StoreIcon.png";
				smallIcon = (Texture2D)UnityEditor.EditorGUIUtility.Load(fname);
				if( smallIcon == null ) {
					fname = name+"StoreIcon.png";
					smallIcon = (Texture2D)UnityEditor.EditorGUIUtility.Load(fname);
				}
				if(smallIcon==null){
					Debug.LogError("could not load : " +fname);
				}
				return smallIcon;
#else
				return null;
#endif
			}
		}
		public string name;
		public bool isDefault = false;
		public bool overwriteValues = false;
		public bool overridden = false;
		public string dispname = "";
		public Texture2D smallIcon;
		public string tooltip="";
		public string billingId = "";
	} 


}