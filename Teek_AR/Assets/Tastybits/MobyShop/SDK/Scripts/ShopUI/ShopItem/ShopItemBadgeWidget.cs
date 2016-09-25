using UnityEngine;
using System.Collections;


namespace MobyShop.UI {
	

	public class ShopItemBadgeWidget : MonoBehaviour {


		public UnityEngine.UI.Text textElement;

		public string Text {
			get {
				if( textElement == null ) return "";
				return textElement.text;
			}
			set {
				if( textElement==null) { return; }
				textElement.text = value;
			}
		}


		public bool badgeEnabled {
			get {
				return this.gameObject.activeSelf;
			}
			set {
				this.gameObject.SetActive(value);
			}
		}

	}


}