using UnityEngine;
using System.Collections;

public class ShopConfirm : MonoBehaviour {

	public System.Action<MobyShop.BillingInGameCurrency.AcceptOrCancel > onDismissed;

		
	public virtual void Show() {
		throw new System.NotImplementedException ("ShopConfirmInterface: Show not implemented");	
	}

}
