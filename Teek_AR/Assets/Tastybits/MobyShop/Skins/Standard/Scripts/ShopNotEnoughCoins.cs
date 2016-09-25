/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;


namespace MobyShop.Skins.Standard {


	/**
	 * This is an implemented version of the confirm dialog for the 
	 * view that 
	 */	
	public class ShopNotEnoughCoins : global::MobyShop.ShopNotEnoughCoins {
		public UnityEngine.UI.MobyShop.AdvImage backgroundImage;
		public UnityEngine.UI.Image imageBehindIcon;
		public UnityEngine.UI.RawImage iconImage;

		public UnityEngine.UI.Text textCaption;
		public UnityEngine.UI.Text textDesc;
		//public UnityEngine.UI.Text textPrice;

		public GameObject buttonOkay;
		public GameObject buttonCancel;

		public bool priceIsUppercase = false;

		public bool enableAnimations = false;

		public RectTransform clickedIconContainerTF;

		public Vector2 iconAbsPos1;
		public Vector2 iconAbsSz1;

		public RectTransform iconPivot;

		public UnityEngine.UI.MobyShop.AdvImage dialogBG;


		/**
		 * Called when we want to set the Data that the view is going to be showed with.
		 * It's okay to call this method before the view has been showed.
		 */
		public void SetData( Texture2D iconTexture, string _caption, string _desc, string _pricetag, Color bgcolor, Color colorBehindIcon, RectTransform clickedIconContainerTF ) {
			textCaption.text = _caption;
			textDesc.text = _desc;
			//textPrice.text = _pricetag;
			iconImage.texture = iconTexture;
			backgroundImage.color = bgcolor;
			imageBehindIcon.color = colorBehindIcon;
			this.clickedIconContainerTF = clickedIconContainerTF;
		}

		/**
		 * Call this to show  
		 */
		public override void Show() {
			this.gameObject.SetActive (true);
			if( enableAnimations ) {
				ShowWithAnimations ();
			}
		}


		void ShowWithAnimations() {
			var imageBehindIcon_tf = this.imageBehindIcon.GetComponent<RectTransform> ();

			Vector3[] corners = new Vector3[4];
			imageBehindIcon_tf.GetLocalCorners(corners);
			var imageContainerSz = corners[2] - corners[0];

			float diffx = 0f;
			if (this.clickedIconContainerTF != null) {
				this.clickedIconContainerTF.GetLocalCorners (corners);
				var clickedContainerSz = corners[2] - corners[0];
				diffx = clickedContainerSz.x / ( imageContainerSz.x > 0f ? imageContainerSz.x : 0.001f );
				//float diffy = clickedContainerSz.y / ( imageContainerSz.y > 0f ? imageContainerSz.y : 0.001f );
			}


			this.backgroundImage.GetComponent<RectTransform>().localScale = Vector2.zero;
			//Vector3 tmp_pos = this.imageBehindIcon.transform.localPosition;
			this.backgroundImage.transform.position = imageBehindIcon_tf.position;

			this.textCaption.gameObject.SetActive( false );
			this.textDesc.gameObject.SetActive( false );
			//this.textPrice.gameObject.SetActive( false );
			//Debug.Break();
			this.buttonOkay.gameObject.SetActive( false );
			this.buttonCancel.gameObject.SetActive( false );


			UITween.StartAnimateValue( dialogBG, 0.0f, 1f, 0.5f, Tween.TweenTypes.Linear, () => {
				var c = dialogBG.color;
				c.a = 0.0f;
				dialogBG.color = c;
			}, ( float value) => {
				var c = dialogBG.color;
				c.a = value;
				dialogBG.color = c;
			}, ()=>{
				var c = dialogBG.color;
				c.a = 1f;
				dialogBG.color = c;
			});

			UITween.startAlphaAnimation( this.imageBehindIcon, 0f, 1f, 0.05f, 0.0f, Tween.TweenTypes.easeInExpo, ()=>{ } );
			UITween.startScaleAnimation( this.imageBehindIcon, diffx, 1f, 0.5f, 0f, Tween.TweenTypes.Linear);
			UITween.startScaleAnimation( this.backgroundImage, 0.0001f, 1f, 0.75f, 0.15f, Tween.TweenTypes.easeInExpo, ()=>{
				textCaption.gameObject.SetActive(true);
				UITween.startAlphaAnimation( textCaption, 0f, 1f, 0.5f, 0f, Tween.TweenTypes.easeInOutExpo );

				textDesc.gameObject.SetActive(true);
				UITween.startAlphaAnimation( textDesc, 0f, 1f, 0.5f, 0.2f, Tween.TweenTypes.easeInOutExpo );

				//textPrice.gameObject.SetActive(true);
				//UITween.startAlphaAnimation( textPrice, 0f, 1f, 0.5f, 0.4f, Tween.TweenTypes.easeInOutExpo );

				buttonOkay.gameObject.SetActive(true);
				UITween.startAlphaAnimation( buttonOkay.GetComponent<UnityEngine.UI.Graphic>(), 0f, 1f, 0.5f, 0.4f, Tween.TweenTypes.easeInOutExpo );

				buttonCancel.gameObject.SetActive(true);
				UITween.startAlphaAnimation( buttonCancel.gameObject, 0f, 1f, 0.5f, 0.6f, Tween.TweenTypes.easeInOutExpo );
			} );

			var img_tf = this.imageBehindIcon.GetComponent<RectTransform> ();
			UITween.startScaleAnimation( img_tf, 0f, 1f, 0.5f, Tween.TweenTypes.easeOutBounce, ()=>{

			} );

			//UITween.startBezierAnimation( this.imageBehindIcon, imageBehindIcon_tf, this.clickedIconContainerTF, iconPivot, 0.5f, ()=>{
			//}, Tween.TweenTypes.easeOutExpo );
		}


		void HideWithAnimations( System.Action cb ) {
			var imageBehindIcon_tf = this.imageBehindIcon.GetComponent<RectTransform> ();

			Vector3[] corners = new Vector3[4];
			imageBehindIcon_tf.GetLocalCorners(corners);
			//var imageContainerSz = corners[2] - corners[0];


			float fadeTextDur = 0.2f;
			float fadeTextOff = 0f;
			UITween.startAlphaAnimation( textCaption, 1f, 0f, fadeTextDur, fadeTextOff, Tween.TweenTypes.easeInOutExpo, ()=>{
				textCaption.gameObject.SetActive(false);
			} );

			//fadeTextOff += fadeTextDur / 2f;
			UITween.startAlphaAnimation( textDesc, 1f, 0f, fadeTextDur, fadeTextOff, Tween.TweenTypes.easeInOutExpo, ()=>{
				textDesc.gameObject.SetActive(false);
			} );

			//textPrice.gameObject.SetActive(true);
			//UITween.startAlphaAnimation( textPrice, 0f, 1f, fadeTextDur, 0.4f, Tween.TweenTypes.easeInOutExpo );

			//fadeTextOff += fadeTextDur / 2f;
			UITween.startAlphaAnimation( buttonOkay.GetComponent<UnityEngine.UI.Graphic>(), 1f, 0f, fadeTextDur, fadeTextOff, Tween.TweenTypes.easeInOutExpo, ()=>{
				buttonOkay.gameObject.SetActive(false);
			} );

			//fadeTextOff += fadeTextDur / 2f;
			UITween.startAlphaAnimation( buttonCancel.gameObject, 1f, 0f, fadeTextDur, fadeTextOff, Tween.TweenTypes.easeInOutExpo, ()=>{
				buttonCancel.gameObject.SetActive(false);
			} );


			fadeTextOff += fadeTextDur / 2f;


			//float scaleIconFrom = 0f;
			/*if (this.clickedIconContainerTF != null) {
				this.clickedIconContainerTF.GetLocalCorners (corners);
				var clickedContainerSz = corners[2] - corners[0];
				scaleIconFrom = clickedContainerSz.x / ( imageContainerSz.x > 0f ? imageContainerSz.x : 0.001f );
				float diffy = clickedContainerSz.y / ( imageContainerSz.y > 0f ? imageContainerSz.y : 0.001f );
			}*/


			UITween.startScaleAnimation( this.imageBehindIcon, 1f, 0f, 0.5f, 0.4f+0.0f, Tween.TweenTypes.easeOutBounce);
			UITween.startAlphaAnimation( this.imageBehindIcon, 1f, 0f, 0.25f, 0.4f+0.25f, Tween.TweenTypes.easeInExpo, ()=>{ } );
			UITween.startScaleAnimation( this.backgroundImage, 1f, 0f, 0.5f, 0.0f, Tween.TweenTypes.easeInExpo, ()=>{ } );

			// Fade away the white background of the dialogue.
			UITween.StartAnimateValue( dialogBG, 1f, 0f, 0.5f, 0.5f, Tween.TweenTypes.Linear, () => {
				var c = dialogBG.color;
				c.a = 1.0f;
				dialogBG.color = c;
				dialogBG.SetAllDirty();
			}, ( float value) => {
				var c = dialogBG.color;
				c.a = value;
				dialogBG.color = c;
				dialogBG.SetAllDirty();
			}, ()=>{
				var c = dialogBG.color;
				c.a = 0f;
				dialogBG.color = c;
				dialogBG.SetAllDirty();
				cb();
			});






		}


		public void OnOkayClicked() {
			if( onDismissed != null ) {	
				onDismissed( ShopNotEnoughCoins.Dismissed.BuyMoreCoins );
			}
			if( enableAnimations ) {
				HideWithAnimations ( ()=>{
					this.gameObject.SetActive (false);
				} );
			} else {
				this.gameObject.SetActive (false);
			}
		}


		public void OnCancelClicked() {
			this.gameObject.SetActive (false);
			if( onDismissed != null ) {
				onDismissed( ShopNotEnoughCoins.Dismissed.Cancel );
			}
		}


	}

}
