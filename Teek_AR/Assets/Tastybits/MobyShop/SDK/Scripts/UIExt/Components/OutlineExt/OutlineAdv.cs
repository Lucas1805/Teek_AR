/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI.MobyShop {


	[AddComponentMenu("UI/Effects/Outline", 15)]
	public class OutlineAdv : Shadow
	{
		protected OutlineAdv():base() {}
		public bool normalOutline=false;

		public override void ModifyMesh (VertexHelper vh) {
			if (!this.IsActive ()) {
				return;
			}

			// Get the list pool...
			System.Type tListPool=null;
			//Debug.Log("typeof(UnityEngine.UI.Shadow).Assembly=" + typeof(UnityEngine.UI.Shadow).Assembly.GetName() );
			foreach( var t in typeof(UnityEngine.UI.Shadow).Assembly.GetTypes() ) {
				//Debug.Log("T= " + t.Name);
				if( t.Name == "ListPool`1" ) {
					tListPool = t;
					//Debug.Log("Found it");
					break;
				}
			}
			tListPool = tListPool.MakeGenericType( new System.Type[]{typeof(UIVertex)} );
			var mi = tListPool.GetMethod( "Get", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public );
			List<UIVertex> list = (List<UIVertex>)mi.Invoke(null, null);


			vh.GetUIVertexStream( list );


			if( normalOutline ) {
				int num = list.Count * 5;
				if (list.Capacity < num) {
					list.Capacity = num;
				}
				int start = 0;
				int count = list.Count;
				base.ApplyShadowZeroAlloc (list, base.effectColor, start, list.Count, base.effectDistance.x, base.effectDistance.y);
				start = count;
				count = list.Count;
				base.ApplyShadowZeroAlloc (list, base.effectColor, start, list.Count, base.effectDistance.x, -base.effectDistance.y);
				start = count;
				count = list.Count;
				base.ApplyShadowZeroAlloc (list, base.effectColor, start, list.Count, -base.effectDistance.x, base.effectDistance.y);
				start = count;
				count = list.Count;
				base.ApplyShadowZeroAlloc (list, base.effectColor, start, list.Count, -base.effectDistance.x, -base.effectDistance.y);
			} else {
				
			}
	
			vh.Clear ();
			vh.AddUIVertexTriangleStream (list);

			mi = tListPool.GetMethod( "Release", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public );
			mi.Invoke(null, new object[]{list});

		}
	}
}
