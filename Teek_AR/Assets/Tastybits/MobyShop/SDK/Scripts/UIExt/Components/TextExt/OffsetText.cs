/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnityEngine.UI {

	// ONLY WORKS IF PIVOT IS IN 0.5 ; 0.5
	[AddComponentMenu("UI/Effects/Offset Text", 14)]
	public class OffsetText : UnityEngine.UI.BaseMeshEffect
	{

		protected OffsetText():base() {
		}


		public Vector2 pct = Vector2.zero;


		public override void ModifyMesh (Mesh vh) { }


		public override void ModifyMesh (VertexHelper vh) {
			if (! IsActive()) return;

			List<UIVertex> verts = new List<UIVertex> ();
			vh.GetUIVertexStream (verts); 

			Text text = GetComponent<Text>();
			if (text == null) {
				Debug.LogWarning("CenterGlyphs on Line: Missing Text component");
				return;
			}

			string[] lines = text.text.Split('\n');
			Vector3  offset = Vector3.zero;
			//float    letterOffset    = 0f * (float)text.fontSize / 100f;
			//float    alignmentFactor = 0;
			int      glyphIdx        = 0;

			switch (text.alignment)
			{
			case TextAnchor.LowerLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.UpperLeft:
				//alignmentFactor = 0f;
				break;

			case TextAnchor.LowerCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.UpperCenter:
				//alignmentFactor = 0.5f;
				break;

			case TextAnchor.LowerRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.UpperRight:
				//alignmentFactor = 1f;
				break;
			}


			for (int lineIdx=0; lineIdx < lines.Length; lineIdx++)
			{
				string line = lines[lineIdx];
				//float lineOffset = (line.Length -1) * letterOffset * alignmentFactor;

				for (int charIdx = 0; charIdx < line.Length; charIdx++)
				{
					int idx1 = glyphIdx * 4 + 0;
					int idx2 = glyphIdx * 4 + 1;
					int idx3 = glyphIdx * 4 + 2;
					int idx4 = glyphIdx * 4 + 3;

					// Check for truncated text (doesn't generate verts for all characters)
					if (idx4 > verts.Count - 1) return;

					var vert1 = verts[idx1];
					var vert2 = verts[idx2];
					var vert3 = verts[idx3];
					var vert4 = verts[idx4];
					vh.PopulateUIVertex (ref vert1, idx1);
					vh.PopulateUIVertex (ref vert2, idx2);
					vh.PopulateUIVertex (ref vert3, idx3);
					vh.PopulateUIVertex (ref vert4, idx4);


					//var height = vert1.position - vert4.position;

					Vector3[] corners = new Vector3[4];
					this.GetComponent<RectTransform> ().GetLocalCorners (corners);
					var dimensions = corners[2] - corners[0];

					//Debug.Log ("Rect size = " + dimensions);

					//Debug.Log ("Char height = " + height.magnitude );

					//Debug.Log ("Lower char corner y value = " + vert4.position.y);

					//float off = ((dimensions.y / 2f) - (height.magnitude / 2f));
					//Debug.Log ("Char Off = " + off );

					// Diff to be in the center...
					//var half_height = height.magnitude / 2f;
					//float diff_y = Mathf.Max( Mathf.Abs(half_height), Mathf.Abs(vert4.position.y) ) - Mathf.Min( Mathf.Abs(half_height), Mathf.Abs(vert4.position.y) );
					//Debug.Log ("diff_y =" + diff_y);

					//float off_y = -half_height - vert4.position.y;


					offset.x = dimensions.x * pct.x;
					offset.y = dimensions.y * pct.y;


					//pos = new Vector3 (0f, +height.magnitude / 2f, 0f); // ; // Vector3.up * ((height)*lineOffset);


					//vert1.color = Color.red;
					vert1.position += offset;

					vh.SetUIVertex ( vert1, idx1 );

					vert2.position += offset;
					vh.SetUIVertex ( vert2, idx2 );

					//vert1.color = Color.red;
					//vert3.color = Color.green;
					vert3.position += offset;
					vh.SetUIVertex ( vert3, idx3 );


					//vert1.color = Color.red;
					vert4.position += offset;
					vh.SetUIVertex ( vert4, idx4 );

					//vh.SetUIVertex ( vert2, idx2 );
					//vh.SetUIVertex ( vert3, idx3 );
					//vh.SetUIVertex ( vert4, idx4 );


					glyphIdx++;
				}

				// Offset for carriage return character that still generates verts
				glyphIdx++;
			}
		}
	}
}
