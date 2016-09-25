/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Tastybits.EmojiAssist;

namespace Tastybits.EmojiAssist {
	public static class StringExtension {
		public static IEnumerable<int> AsCodePoints(this string s) {
			for(int i = 0; i < s.Length; ++i) {
				//Debug.LogError("index="+i);
				yield return char.ConvertToUtf32(s, i);
				if( char.IsHighSurrogate(s, i) )
					i++;
			}
		}
	}

}


namespace UnityEngine {

	namespace UI {

		[ExecuteInEditMode] 
		public class TextExt : Text {
			public static bool InputIsAppending = false;
			public static bool _verbose=false;
			//private Image icon;
			//private Vector3 iconPosition;
			//private List<Image> icons;
			//private List<Vector2> positions = new List<Vector2>(); 
			private float _fontHeight;
			private float _fontWidth;


			public float charHeight = 0f;


			//public float maxSizeBaseSize = 14f;
			public float maxSizeMultiplier = 1f;
			public MaxSizeResizeModes autoAdjustMaxSize = MaxSizeResizeModes.None;
			public enum MaxSizeResizeModes {
				None,
				AutoAdjustMaxSize = 1
			}
			public void UpdateMaxSize() {
				if (maxSizeMultiplier >= 1f) {
					maxSizeMultiplier = 1f;
				} else if( maxSizeMultiplier < 0f ) {
					maxSizeMultiplier = 0f;
				}
				if( autoAdjustMaxSize == MaxSizeResizeModes.AutoAdjustMaxSize ) {
					var rectSize = this.GetComponent<RectTransform> ().GetSizeOnScreen ();
					var old = this.resizeTextMaxSize;
					this.resizeTextMaxSize = Mathf.RoundToInt (rectSize.y * maxSizeMultiplier);
					if (this.resizeTextMaxSize != old) {
						this.SetVerticesDirty ();
					}
				}
			}



			
			public float ImageScale=1f;
			
			UIVertex[] m_TempVerts  =new UIVertex[4];
			
			//public Material emojiiMaterial;
			
			//private List<GameObject> activateTheese = new List<GameObject>(); 
			
			//private List<GameObject> deleteTheese = new List<GameObject>(); 

			/*[HideInInspector]
			public GameObject emojiContainer;
			public RectTransform EmojiContainer {
				get {
					if( emojiContainer == null ) {
						if( _verbose ) Debug.Log("Creating EmojiContainer");
						emojiContainer = new GameObject("EmojiContainer" );
						emojiContainer.AddComponent<UnityEngine.CanvasRenderer>();
						var rt = emojiContainer.AddComponent<UnityEngine.RectTransform>();
						rt.SetParent( this.GetComponent<RectTransform>() );
						emojiContainer.transform.localPosition = Vector3.zero;
						emojiContainer.transform.localScale = Vector3.one;
						emojiContainer.transform.localRotation = Quaternion.identity;
						//emojiContainer.hideFlags = HideFlags.HideAndDontSave;
					}
					try {
						var rt2 = emojiContainer.GetComponent<RectTransform>();
						if( rt2 == null ) {
							Debug.LogError("no rect transform");
						}
					} catch{
						emojiContainer = null;
					}
					var ret = emojiContainer.GetComponent<RectTransform>();
					//emojiContainer = null;
					return ret;
				}
			}*/

			[HideInInspector]
			public bool shortCodesEnabled = true;

			public List<Vector2> charSizes = new List<Vector2>();
			public List<Vector2> charPositions = new List<Vector2>();


			public float GetAvgCharHeight(){
				float val = 0f;
				int it = 0;
				foreach (var p in charSizes) {
					val += p.y;
					it++;
				}
				if (it == 0)
					return 0f;
				return val / (float)it;
			}


			public float GetHighestChar(){
				float val = 0f;
				foreach (var p in charSizes) {
					if (p.y >= val)
						val = p.y;
				}
				return val;
			}




			//string _prevText="";
			//string _formattetText="";
			private string _text {
				get {
					/*if( shortCodesEnabled ) {
						if( string.IsNullOrEmpty(_prevText) || _prevText!=this.text ) {
							_prevText = this.text;
							if( !this.text.Contains(":") ) {
								_formattetText = this.text;
							} else {
								_formattetText = Tastybits.EmojiAssist.Shortcodes.Emojify( this.text );
						 	}
						} 
						return _formattetText;
					}*/
					return this.text;
				}
			}

			// use this to get the string with the emojis as unicode chars in them.
			/*public string TextEmojified {
				get {
					return _text;
				}
			}*/

			// use this to get the text that is not emojifed.
			/*public string TextUnemojified {
				get {
					return Tastybits.EmojiAssist.Shortcodes.Unemojify( _text );
				}
			}*/
			
			
			// Get the number of emoji's available. // change it to the replacement character...
			/*public int CountNumberOfEmojiAvail( out int numNormal ){
				int ret = 0;
				numNormal = 0;
				foreach( int codePoint in _text.AsCodePoints() ) {
					//Debug.Log( "codepnt = " + codePoint + " ch = " + char.ConvertFromUtf32(codePoint) );
					if( codePoint >= 0x1F600 && codePoint <= 0x1F64F ) { // Emoticons
						ret++;
					} else if( codePoint >= 0x2700 && codePoint <= 0x27BF ) { // Dingbats
						ret++;
					} else if( codePoint >= 0x1F680 && codePoint <= 0x1F6C0 ) { // Transport and map symbols
						ret++;
					} else if( codePoint >= 0x1F300 && codePoint <= 0x1F5FF ) { // Miscellaneous Symbols and Pictographs
						ret++;
					} else {
						numNormal++;
					}
				}
				return ret;
			}*/


			/*public string TextWithEmojisAsReplacementChars {
				get {
					string result = "";
					foreach( int codePoint in _text.AsCodePoints() ) {
						if( codePoint >= 0x1F600 && codePoint <= 0x1F64F || 
							codePoint >= 0x2700 && codePoint <= 0x27BF ||
							codePoint >= 0x1F680 && codePoint <= 0x1F6C0 || 
							codePoint >= 0x1F300 && codePoint <= 0x1F5FF) 
						{ // Emoticons
							result += System.Char.ConvertFromUtf32( 0xFFFD );
						} else {
							string strCh = System.Char.ConvertFromUtf32( codePoint );
							int szBefore = result.Length;
							result += strCh;
							if( result.Length > szBefore + 1 ) {
								Debug.LogError("Warning the string increasd more than one character");
							}
						}
					}
					return result;
				}
			}*/



			// used to get a text string with the emoji's replacedwith 
			/*string _textAsStringWithEmojisReplaced {
				get {
					string result = "";
					foreach( int codePoint in _text.AsCodePoints() ) {
						if( codePoint >= 0x1F600 && codePoint <= 0x1F64F ) { // Emoticons
							result += System.Char.ConvertFromUtf32( 0xFFFD );
						} else if( codePoint >= 0x2700 && codePoint <= 0x27BF ) { // Dingbats
							result += System.Char.ConvertFromUtf32( 0xFFFD );
						} else if( codePoint >= 0x1F680 && codePoint <= 0x1F6C0 ) { // Transport and map symbols
							result += System.Char.ConvertFromUtf32( 0xFFFD );
						} else if( codePoint >= 0x1F300 && codePoint <= 0x1F5FF ) { // Miscellaneous Symbols and Pictographs
							result += System.Char.ConvertFromUtf32( 0xFFFD );
						} else {
							string strCh = System.Char.ConvertFromUtf32( codePoint );
							int szBefore = result.Length;
							result += strCh;
							if( result.Length > szBefore + 1 ) {
								Debug.LogError("Warning the string increasd more than one character");
							}
						}
					}
					return result;
				}
			}*/
			
			
			/*public static bool IsEmojiChar( int codePoint ) {
				if( codePoint >= 0x1F600 && codePoint <= 0x1F64F ) { // Emoticons
				} else if( codePoint >= 0x2700 && codePoint <= 0x27BF ) { // Dingbats
				} else if( codePoint >= 0x1F680 && codePoint <= 0x1F6C0 ) { // Transport and map symbols
				} else if( codePoint >= 0x1F300 && codePoint <= 0x1F5FF ) { // Miscellaneous Symbols and Pictographs
				} else {
					return false;
				}
				return true;
			}*/
			
			
			/*public string TextAsStringWithOutEmojisInIT {
				get {
					string result = "";
					foreach( int codePoint in _text.AsCodePoints() ) {
						if( codePoint >= 0x1F600 && codePoint <= 0x1F64F ) { // Emoticons
						} else if( codePoint >= 0x2700 && codePoint <= 0x27BF ) { // Dingbats
						} else if( codePoint >= 0x1F680 && codePoint <= 0x1F6C0 ) { // Transport and map symbols
						} else if( codePoint >= 0x1F300 && codePoint <= 0x1F5FF ) { // Miscellaneous Symbols and Pictographs
						} else {
							string strCh = System.Char.ConvertFromUtf32( codePoint );
							int szBefore = result.Length;
							result += strCh;
							if( result.Length > szBefore + 1 ) {
								Debug.LogError("Warning the string increasd more than one character");
							}
						}
					}
					return result;
				}
			}*/


			protected override void OnPopulateMesh( VertexHelper toFill ) {
				//activateTheese.Clear();
				//DeactivateAllEmojiChildren(false);
				NewPopulate( toFill );
				//CheckNeedsUpdate();
			}
			
			
			
			/*public void CheckNeedsUpdate() {
#if UNITY_EDITOR
				UnityEngine.UI.InputField ino;
				if( activateTheese.Count > 0 ) {
					if( _verbose ) Debug.Log("Needs update");
					if( UnityEngine.Application.isPlaying == false ) {
						UnityEditor.EditorApplication.update -= UpdateEditor;
						UnityEditor.EditorApplication.update += UpdateEditor;
					}
				}
#endif
			}*/
			
			
			public int GetCharCnt() {
				return this.cachedTextGenerator.characterCount;
			}
			
			
			public int GetCharCntVis() {
				return this.cachedTextGenerator.characterCount;
			}
			

			protected override void OnEnable () {
				base.OnEnable();
				//EmojiContainer.gameObject.SetActive(true);
			}


			protected override void OnDisable () {
				base.OnDisable();
				//EmojiContainer.gameObject.SetActive(false);
			}
				
			
			/*void DeactivateAllEmojiChildren( bool allowDelete ) {
				//Debug.Log( "DeactivateAllEmojiChildren" );
				deleteTheese.Clear();
				for( int i=0; i<EmojiContainer.childCount; i++ ){
					var child = EmojiContainer.GetChild(i);
					var emoji = child.GetComponent<Tastybits.EmojiAssist.Emoji>();
					if( emoji != null ) {
						//child.gameObject.SetActive(false);

						//child.gameObject.SetActive(false);
						if( allowDelete ) {
							if( UnityEngine.Application.isPlaying == false && Application.isEditor ) {
								Debug.Log("Destroy 1");
								GameObject.DestroyImmediate( child.gameObject );
							} else {
								Debug.Log("Destroy 2");
								GameObject.Destroy(child.gameObject);
							}
						} else {
							deleteTheese.Add( child.gameObject );
						}
						
						
					}
				}
			}*/
			
			
			/*Tastybits.EmojiAssist.Emoji GetEmoji() {
				for( int i=0; i<EmojiContainer.childCount; i++ ){ // reuse an emoji...
					if( _verbose ) Debug.Log("Reusing emoji");
					var child = EmojiContainer.GetChild(i); 
					var emoji = child.GetComponent<Tastybits.EmojiAssist.Emoji>();
					if( emoji != null && emojiContainer.gameObject.activeSelf == false ) {
						return emoji;
					}
				}
				return Tastybits.EmojiAssist.Emoji.Create( EmojiContainer );
			}*/
			
			
			
			void ProcessVerts(){
				charSizes.Clear ();
				charPositions.Clear ();

				IList<UIVertex> verts = this.cachedTextGenerator.verts ;
							
				int chidx=0;
				int j=0;
				var arr = _text.AsCodePoints().ToArray();
				
				float posoff=0f;
				for( int it = 0; it < verts.Count; it++) {
					// each time j is 0 we are standing at a chacter...
					if( j == 0 ) {
						
						// Get the character of the string..
						var charInfo = this.cachedTextGenerator.characters[j];
						//this.cachedTextGenerator.GetLinesArray()[0].startCharIdx;
						
						// invisible charas in the end?
						if( chidx >= arr.Count() ) {
							continue;
						}
						
						//var codePoint = arr[chidx];
						
						//string strChar = System.Char.ConvertFromUtf32(codePoint);
						
						posoff+=charInfo.charWidth;
						var vp1 = verts[it].position;
						var vp2 = verts[it+2].position;
						
						//charInfo.
						//Debug.LogError ( "#" + chidx + " ch:" + strChar + " posx:" + charInfo.cursorPos.x );
						
						//Debug.Log ( "codePoint=" + codePoint );
						/*if( IsEmojiChar(codePoint) ) {
							var vert = verts[it];  
							UnityEngine.Color c = vert.color;
							c.a = 0f;
							
							vert.color = c; // change color
							verts[it] = vert;
							
							var vert2 = verts[it+1];  
							vert2.color = c; // change color
							verts[it+1] = vert2;
							
							var vert3 = verts[it+2];  
							vert3.color = c; // change color
							verts[it+2] = vert3;
							
							var vert4 = verts[it+3];  
							vert4.color = c; // change color
							verts[it+3] = vert4;
							
							var emoji = GetEmoji();
							//var rawImg = emoji.GetComponent<RawImage>();
							activateTheese.Add(emoji.gameObject);
							
							//Debug.Log ( "added emoji" );
							emoji.SetEmoji( codePoint );
							//emoji.gameObject.SetActive(true);
							
							//	Debug.LogError ( "charInfo.cursorPos = " + charInfo.cursorPos );
							var vp_x = (Mathf.Max(vp1.x,vp2.x) - Mathf.Min(vp1.x,vp2.x)); 
							var vp_y = (Mathf.Max(vp1.y,vp2.y) - Mathf.Min(vp1.y,vp2.y)); 
							//Debug.Log ( "vp_y = " + vp_y );
							var rt = emoji.GetComponent<RectTransform>();
							rt.localPosition = new Vector2(vp1.x + (vp_x/2f), vp1.y - (vp_y/2f));
							rt.sizeDelta = new Vector2 (vp_x, vp_y);
							
							
						} else {
							var vert = verts[it];  
							var p = vert.position;
							vert.position = p;
							//vert.color = Color.green; // change color
							verts[it] = vert;
							
						}*/
			

						//var c = Color.black;
						var vert = verts[it];  
						var p = vert.position;
						vert.position = p;
			//vert.color = c; // change color
						verts[it] = vert;

						var vert2 = verts[it+1];  
			//			vert2.color = c; // change color
						verts[it+1] = vert2;

						var vert3 = verts[it+2];  
			//			vert3.color = c; // change color
						verts[it+2] = vert3;

						var vert4 = verts[it+3];  
			//			vert4.color = c; // change color
						verts[it+3] = vert4;


						var vp_x = (Mathf.Max(vp1.x,vp2.x) - Mathf.Min(vp1.x,vp2.x)); 
						var vp_y = (Mathf.Max(vp1.y,vp2.y) - Mathf.Min(vp1.y,vp2.y)); 

						var localCharPos = new Vector2(vp1.x + (vp_x/2f), vp1.y - (vp_y/2f));
						var localCharSize = new Vector2 (vp_x, vp_y);
						
						charPositions.Add(localCharPos);
						charSizes.Add( localCharSize );  
						
						//charInfo.cursorPos
						
					}
					if( j++ >= 3 ) {
						j=0;
						chidx++;
					}
				}


				
				//this.mainTexture = tx;

				
			}

			
			void NewPopulate( VertexHelper toFill ) {
				if( this.font == null ) {
					return;
				}
				if( InputIsAppending ) {
					Debug.Log("Input is changing text ignore it." );
				}
				if( _verbose ) Debug.Log("New populate");
				
				//System.Reflection.BindingFlags bindFl = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
				
				this.m_DisableFontTextureRebuiltCallback = true;
				Vector2 size = this.rectTransform.rect.size;
				TextGenerationSettings generationSettings = this.GetGenerationSettings (size);
				
				this.cachedTextGenerator.Populate ( _text, generationSettings);
				//this.cachedTextGenerator.Populate( _textAsStringWithEmojisReplaced, generationSettings );
				
				Rect rect = this.rectTransform.rect;
				
				Vector2 textAnchorPivot = Text.GetTextAnchorPivot( this.alignment );
				Vector2 pivotPoint = Vector2.zero;
				pivotPoint.x = ((textAnchorPivot.x != 1f) ? rect.xMin : rect.xMax );
				pivotPoint.y = ((textAnchorPivot.y != 0f) ? rect.yMax : rect.yMin );
				Vector2 offset = base.PixelAdjustPoint (pivotPoint) - pivotPoint;
				
				IList<UIVertex> verts = this.cachedTextGenerator.verts;
				
				ProcessVerts();
				
				//DoFillVBO( this.cachedTextGenerator, ref verts );
					
				float units = 1f / this.pixelsPerUnit;
				int numCharVerts = verts.Count - 4;
				toFill.Clear ();


				
				
				if (offset != Vector2.zero ) {
					for( int i = 0; i < numCharVerts; i++ ) {
						int num3 = i & 3;
						this.m_TempVerts [num3] = verts[ (i) ];
						UIVertex[] expr_147_cp_0 = this.m_TempVerts;
						int expr_147_cp_1 = num3;
						expr_147_cp_0 [expr_147_cp_1].position = expr_147_cp_0 [expr_147_cp_1].position * units;
						UIVertex[] expr_16B_cp_0_cp_0 = this.m_TempVerts;
						int expr_16B_cp_0_cp_1 = num3;
						expr_16B_cp_0_cp_0 [expr_16B_cp_0_cp_1].position.x = expr_16B_cp_0_cp_0 [expr_16B_cp_0_cp_1].position.x + offset.x;
						UIVertex[] expr_190_cp_0_cp_0 = this.m_TempVerts;
						int expr_190_cp_0_cp_1 = num3;
						expr_190_cp_0_cp_0 [expr_190_cp_0_cp_1].position.y = expr_190_cp_0_cp_0 [expr_190_cp_0_cp_1].position.y + offset.y;
						if (num3 == 3) {
							toFill.AddUIVertexQuad (this.m_TempVerts);
						}
						//string tmpCh = System.Text.RegularExpressions.Regex.Unescape( "\ud83d\ude02" );
					}
				} else {
					for (int j = 0; j < numCharVerts; j++) {
						int num4 = j & 3;
						this.m_TempVerts [num4] = verts[ (j) ];
						UIVertex[] expr_201_cp_0 = this.m_TempVerts;
						int expr_201_cp_1 = num4;
						expr_201_cp_0 [expr_201_cp_1].position = expr_201_cp_0 [expr_201_cp_1].position * units;
						if (num4 == 3) {
							toFill.AddUIVertexQuad (this.m_TempVerts);
						}
					}
				}
				
				//this.GetType().GetField( "m_TempVerts", bindFl ).SetValue(this, this.m_TempVerts );
				
				
				this.m_DisableFontTextureRebuiltCallback = false;
			}


			Material overwriteWithMaterial;
			Texture2D tx;
			
			
			void Update() {
				#if UNITY_EDITOR
					UpdateMaxSize();
				#endif
				//RefreshEmojis();
				/*if (overwriteWithMaterial == null) {
					//Debug.Log ("Creating new material");
					tx = new Texture2D (2, 2);
					tx.SetPixels (new Color[]{ Color.white, Color.white, Color.white, Color.white });
					tx.Apply ();
					var sha = Shader.Find ( "Custom/UI/Default" );
					if (sha != null) {
						overwriteWithMaterial = new Material ( sha );
						overwriteWithMaterial.mainTexture = tx;
						this.material = overwriteWithMaterial;
					}
				}
				overwriteWithMaterial.mainTexture = tx;*/
			}
			
			#if UNITY_EDITOR
			void UpdateEditor() {
				//RefreshEmojis();
			}
			#endif
			
			/*void RefreshEmojis()  {
				foreach( var go in deleteTheese ) {
					if( go == null ) continue;
					if( UnityEngine.Application.isPlaying == false && Application.isEditor ) {
						//Debug.Log("Destroy 1");
						GameObject.DestroyImmediate( go );
					} else {
						//Debug.Log("Destroy 2");
						GameObject.Destroy(go);
					}
				}
				deleteTheese.Clear();
				foreach( var go in activateTheese ) {
					if( go == null ) {
						continue;
					}
					try {
						if( go.GetComponent<Tastybits.EmojiAssist.Emoji>() != null ) {
							go.GetComponent<Tastybits.EmojiAssist.Emoji>().UpdateContent();
						}
						go.SetActive(true);
					}catch{
						
					}
				}
				activateTheese.Clear();
			}*/

		
			// The 
			void OrgPopulate( VertexHelper toFill ) {
				if( this.font == null ) {
					return;
				}
				//System.Reflection.BindingFlags bindFl = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
				
				this.m_DisableFontTextureRebuiltCallback = true;
				Vector2 size = this.rectTransform.rect.size;
				TextGenerationSettings generationSettings = this.GetGenerationSettings (size);
				this.cachedTextGenerator.Populate (_text, generationSettings);
				Rect rect = this.rectTransform.rect;
				
				
				Vector2 textAnchorPivot = Text.GetTextAnchorPivot( this.alignment );
				Vector2 zero = Vector2.zero;
				zero.x = ((textAnchorPivot.x != 1f) ? rect.xMin : rect.xMax );
				zero.y = ((textAnchorPivot.y != 0f) ? rect.yMax : rect.yMin );
				Vector2 vector = base.PixelAdjustPoint (zero) - zero;
				IList<UIVertex> verts = this.cachedTextGenerator.verts ;
				float num = 1f / this.pixelsPerUnit;
				int num2 = verts.Count - 4;
				toFill.Clear ();
				
				
				if (vector != Vector2.zero ) {
					for (int i = 0; i < num2; i++) {
						int num3 = i & 3;
						this.m_TempVerts [num3] = verts[ (i) ];
						UIVertex[] expr_147_cp_0 = this.m_TempVerts;
						int expr_147_cp_1 = num3;
						expr_147_cp_0 [expr_147_cp_1].position = expr_147_cp_0 [expr_147_cp_1].position * num;
						UIVertex[] expr_16B_cp_0_cp_0 = this.m_TempVerts;
						int expr_16B_cp_0_cp_1 = num3;
						expr_16B_cp_0_cp_0 [expr_16B_cp_0_cp_1].position.x = expr_16B_cp_0_cp_0 [expr_16B_cp_0_cp_1].position.x + vector.x;
						UIVertex[] expr_190_cp_0_cp_0 = this.m_TempVerts;
						int expr_190_cp_0_cp_1 = num3;
						expr_190_cp_0_cp_0 [expr_190_cp_0_cp_1].position.y = expr_190_cp_0_cp_0 [expr_190_cp_0_cp_1].position.y + vector.y;
						if (num3 == 3)
						{
							toFill.AddUIVertexQuad (this.m_TempVerts);
						}
					}
				} else {
					for (int j = 0; j < num2; j++) {
						int num4 = j & 3;
						this.m_TempVerts [num4] = verts[ (j) ];
						UIVertex[] expr_201_cp_0 = this.m_TempVerts;
						int expr_201_cp_1 = num4;
						expr_201_cp_0 [expr_201_cp_1].position = expr_201_cp_0 [expr_201_cp_1].position * num;
						if (num4 == 3) {
							toFill.AddUIVertexQuad (this.m_TempVerts);
						}
					}
				}
				
				this.m_DisableFontTextureRebuiltCallback = false;
			}
		}



		
	}

}









