using UnityEngine;
using System.Collections;

public class ShopTabbarTabWidget : MonoBehaviour {
	public int index = 0;

	public GameObject contentContainer;

	public void SetTabActive( bool value, Color colorActive,  Color colorInactive  ) {
		this.gameObject.SetActive(true);
		var img = GetComponent<UnityEngine.UI.Image>();
		img.color = value ? colorActive : colorInactive;
		if( contentContainer!=null ) {
			contentContainer.SetActive(value);
		}
	}

}
