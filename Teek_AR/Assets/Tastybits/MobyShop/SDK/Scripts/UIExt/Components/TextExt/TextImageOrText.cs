/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace UnityEngine.UI.MobyShop {


[ExecuteInEditMode]
public class TextImageOrText : MonoBehaviour {
	public string _text;
	public UnityEngine.UI.Text textElement;
	public string bakedText = "change this";
	public bool forceDynamic=false;

	public string text {
		get {
			/*if( textElement!=null && string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(textElement.text) ) {
				_text = textElement.text;
			}*/
			return _text;
		}
		set {
			_text = value;
			if( textElement!=null ) {
				textElement.text = _text;
			}
			UpdateBakedVsDynamic();
		}
	}


	void UpdateBakedVsDynamic() {
		bool use_baked = bakedText == _text && !string.IsNullOrEmpty(_text) && !forceDynamic;
		this.GetComponent<UnityEngine.UI.Image>().enabled = use_baked;
		if( textElement!=null ) {
			textElement.gameObject.SetActive(!use_baked);
		}
	}

#if UNITY_EDITOR
	void Update(){
		if( Application.isPlaying == false ) {
			if( textElement==null ) {
				for( int i=0; i<this.transform.childCount; i++ ) {
					var ch = this.transform.GetChild(i);
					if( ch.GetComponent<UnityEngine.UI.Text>()!=null ) {
						textElement = ch.GetComponent<UnityEngine.UI.Text>();
					}
				}
			} else {
				textElement.text = _text;
				UpdateBakedVsDynamic();
			}
		}
		if( textElement!=null && string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(textElement.text) ) {
			_text = textElement.text;
		}
	}
#endif

}

}