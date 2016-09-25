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
	 * This Confirm overlay is a specific Shop Confirm dialog in the Standard Skin
	 * it inherits from ShopConfig in order to implement the needed functionality 
	 * that a ShopConfirm dialog needs to implement.
	 */	
	public class ConfirmOverlay : ShopConfirm {
		// referance to the background image which is colored diffirent depending on 
		// the various products shown.
		public UnityEngine.UI.MobyShop.AdvImage backgroundImage;

		// image shown behind the icon.
		public UnityEngine.UI.Image imageBehindIcon;

		// refernace to the icon of the product shown.
		public UnityEngine.UI.RawImage iconImage;

		// referance to the label containing the name of the product shown.
		public UnityEngine.UI.Text textCaption;

		// referance to the description of the product shown.
		public UnityEngine.UI.Text textDesc;

		// referance to the price label which holds the price of the
		// purchases.
		public UnityEngine.UI.Text textPrice;
	
		// referance to the "okay" button that initiates the purchase of the product..
		public GameObject buttonOkay;

		// referance to the "close" button which is used to close the view.
		public GameObject buttonCancel;

		// When true the price set in SetData is ensured to be represented in Uppercase.
		public bool priceIsUppercase = false;

		// When true the view will use animations when shown / hidden.
		public bool enableAnimations = false;

		// A referance to the original element clicked before we're showing this view.
		// ( this is used to animate the product icon from the clicked buttons position to the center of the confirm view )
		public RectTransform clickedIconContainerTF;

		// A referance element representing the pivot that holds the position of the product Icon.
		public RectTransform iconPivot;

		// referance to the dialog background
		public UnityEngine.UI.MobyShop.AdvImage dialogBG;


		/**
		 * Called when we want to set the Data that the view is going to be showed with.
		 * It's okay to call this method before the view has been showed.
		 */
		public void SetData( Texture2D iconTexture, string _caption, string _desc, string _pricetag, Color bgcolor, Color colorBehindIcon, RectTransform clickedIconContainerTF ) {
			textCaption.text = _caption;
			textDesc.text = _desc;
			textPrice.text = _pricetag;
			iconImage.texture = iconTexture;
			backgroundImage.color = bgcolor;
			imageBehindIcon.color = colorBehindIcon;
			this.clickedIconContainerTF = clickedIconContainerTF;
		}

		/**
		 * Call this to show the dialog
		 */
		public override void Show() {
			this.gameObject.SetActive (true);
			if( enableAnimations ) {
				ShowWithAnimations ();
			}
		}

		/**
		 * Shows the view by playing animations.
		 */
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
			this.textPrice.gameObject.SetActive( false );
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

			UITween.startAlphaAnimation( this.imageBehindIcon, 0f, 1f, 0.05f, 0f, Tween.TweenTypes.easeInExpo, ()=>{ } );
			UITween.startScaleAnimation( this.imageBehindIcon, diffx, 1f, 0.5f, 0f, Tween.TweenTypes.Linear);
			UITween.startScaleAnimation( this.backgroundImage, 0.0001f, 1f, 0.75f, 0.15f, Tween.TweenTypes.easeInExpo, ()=>{
				textCaption.gameObject.SetActive(true);
				UITween.startAlphaAnimation( textCaption, 0f, 1f, 0.5f, 0f, Tween.TweenTypes.easeInOutExpo );

				textDesc.gameObject.SetActive(true);
				UITween.startAlphaAnimation( textDesc, 0f, 1f, 0.5f, 0.2f, Tween.TweenTypes.easeInOutExpo );

				textPrice.gameObject.SetActive(true);
				UITween.startAlphaAnimation( textPrice, 0f, 1f, 0.5f, 0.4f, Tween.TweenTypes.easeInOutExpo );

				buttonOkay.gameObject.SetActive(true);
				UITween.startAlphaAnimation( buttonOkay.GetComponent<UnityEngine.UI.MobyShop.AdvImage>(), 0f, 1f, 0.5f, 0.6f, Tween.TweenTypes.easeInOutExpo );

				buttonCancel.gameObject.SetActive(true);
				UITween.startAlphaAnimation( buttonCancel.gameObject, 0f, 1f, 0.5f, 0.8f, Tween.TweenTypes.easeInOutExpo );
			} );
			UITween.StartAnimateValue( dialogBG, 0.0f, 0f, 0.32f, Tween.TweenTypes.Linear, () => { 
				this.backgroundImage.gameObject.AddMissingComponent<CanvasGroup>().alpha = 0f; 
			}, ( float value) => { 
				this.backgroundImage.gameObject.AddMissingComponent<CanvasGroup>().alpha = 0f; 
			}, ()=>{
				this.backgroundImage.gameObject.AddMissingComponent<CanvasGroup>().alpha = 1f;
			});

			if (this.clickedIconContainerTF != null) {
				UITween.startBezierAnimation (this.imageBehindIcon, imageBehindIcon_tf, this.clickedIconContainerTF, iconPivot, 0.5f, () => {
				}, Tween.TweenTypes.easeOutExpo);
			} else {
				var img_tf = this.imageBehindIcon.GetComponent<RectTransform> ();
				UITween.startScaleAnimation( img_tf, 0f, 1f, 0.5f, Tween.TweenTypes.easeOutBounce, ()=>{

				} );
			}
		}
 

		/**
		 * Hides the view with animations
		 */
		void HideWithAnimations( System.Action cb ) {
			//var imageBehindIcon_tf = this.imageBehindIcon.GetComponent<RectTransform> ();

			float fadeTextDur = 0.2f;
			UITween.startAlphaAnimation( textCaption, 1f, 0f, fadeTextDur, 0.25f, Tween.TweenTypes.easeInOutExpo, ()=>{
				textCaption.gameObject.SetActive(false);
			} );

			//fadeTextOff += fadeTextDur / 2f;
			UITween.startAlphaAnimation( textDesc, 1f, 0f, fadeTextDur, 0.25f, Tween.TweenTypes.easeInOutExpo, ()=>{
				textDesc.gameObject.SetActive(false);
			} );

			UITween.startAlphaAnimation( textPrice, 1f, 0f, fadeTextDur, 0.25f, Tween.TweenTypes.easeInOutExpo, ()=>{
			} );


			UITween.startAlphaAnimation( buttonOkay.GetComponent<UnityEngine.UI.Graphic>(), 1f, 0f, fadeTextDur, 0f, Tween.TweenTypes.easeInOutExpo, ()=>{
				buttonOkay.gameObject.SetActive(false);
			} );

			UITween.startAlphaAnimation( buttonCancel.gameObject, 1f, 0f, fadeTextDur, 0f, Tween.TweenTypes.easeInOutExpo, ()=>{
				buttonCancel.gameObject.SetActive(false);
			} );

			// 
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

		/**
		 * Callback invoked when the okay button is clicked. 
		 * 
		 * As expected by the ShopConfirm class we're inheriting from
		 * We have to invoke OnDismissed in this method.
		 */
		public void OnOkayClicked() {
			if( onDismissed != null ) {	
				onDismissed( MobyShop.BillingInGameCurrency.AcceptOrCancel.Accepted );
			}
			if( enableAnimations ) {
				HideWithAnimations ( ()=>{
					this.gameObject.SetActive( false );
				} );
			} else {
				this.gameObject.SetActive( false );
			}
		}

		/**
		 * Callback invoked when the okay button is clicked.
		 * 
		 * As expected by the ShopConfirm class we're inheriting from
		 * We have to invoke OnDismissed in this method.
		 */
		public void OnCancelClicked() {
			this.gameObject.SetActive (false);
			if( onDismissed != null ) {
				onDismissed( MobyShop.BillingInGameCurrency.AcceptOrCancel.Cancelled );
			}
		}

	}

}