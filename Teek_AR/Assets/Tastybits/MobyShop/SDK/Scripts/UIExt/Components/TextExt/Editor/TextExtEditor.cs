using UnityEditor;
using UnityEngine;
using Tastybits;

namespace UnityEngine {
	
	namespace UI {
		   

		[CustomEditor(typeof(TextExt))]
		public class TextWithEditor : UnityEditor.UI.TextEditor {

			// use this to to add a text element.
			/*[MenuItem("GameObject/UI/Text (With Emoji Support)")]
			public static void _(){
				var go = new GameObject("Text(WithEmojiSupport)");
				go.AddComponent<RectTransform>();
				var text = go.AddComponent<UI.TextExt>();
				//text.text = "New Text " + System.Char.ConvertFromUtf32(0x1F602);
				if( UnityEditor.Selection.activeGameObject!=null ) {
					go.transform.parent = UnityEditor.Selection.activeGameObject.transform;
				} else {
				}
				go.SetActive(false);
				text.enabled=false;
				text.text = "New Text :smiley:";
				text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				text.SetAllDirty();
				var p = text.rectTransform.localPosition;
				p.x--;
				text.rectTransform.localPosition = p;
				EditorApplication.CallbackFunction tmp;
				int cnt=0;
				tmp=()=>{
					if(cnt++>3){
						EditorApplication.update -= tmp;
						text.enabled=true;
						go.SetActive(true);
					}
				};
				EditorApplication.update += tmp;
			}*/


			public static void Test() {
				EditorApplication.update -= Test;
			}


			protected override void OnEnable() {
		        base.OnEnable();
		    }


		    public override void OnInspectorGUI() {
				var targ = this.target as TextExt;

				base.OnInspectorGUI();


				//GUILayout.Label("text=" +  targ.text );
				//GUILayout.Label("textemojified=" +  targ.TextEmojified );
				//GUILayout.Label("textunemojified=" +  targ.TextUnemojified );


				if (targ.resizeTextForBestFit) {
					// This is something that can be used to change the max size 
					targ.autoAdjustMaxSize = (TextExt.MaxSizeResizeModes)EditorGUILayout.EnumPopup( "Max Size Fits Screensize", targ.autoAdjustMaxSize);
					if( targ.autoAdjustMaxSize == TextExt.MaxSizeResizeModes.AutoAdjustMaxSize ) {
						//targ.maxSizeBaseSize = EditorGUILayout.FloatField( "Max Size Base Size", targ.maxSizeBaseSize );
						var old = targ.maxSizeMultiplier;
						targ.maxSizeMultiplier = EditorGUILayout.FloatField( "Max Size Multiplier", targ.maxSizeMultiplier );
						if (old != targ.maxSizeMultiplier) {
							targ.UpdateMaxSize ();
							targ.SetVerticesDirty ();
							targ.enabled = false;
							targ.enabled = true;
						}
					}
					if (targ.maxSizeMultiplier >= 1f) {
						targ.maxSizeMultiplier = 1f;
					} else if( targ.maxSizeMultiplier < 0f ) {
						targ.maxSizeMultiplier = 0f;
					}
				}
		        

				/*EditorGUILayout.TextField ("this.materialForRendering", targ.materialForRendering.name);
				EditorGUILayout.TextField ("this.material", targ.material.name );
				EditorGUILayout.TextField ("this.material.mainTexture", targ.material.mainTexture.name );
				EditorGUILayout.ObjectField ("this.material.mainTexture", targ.material.mainTexture, typeof(Texture), false );


				EditorGUILayout.TextField ("this.material.mainTexture", targ.material.mainTexture.name );
*/
				EditorGUILayout.TextField ("Character Height", ""+targ.GetHighestChar () );


				//(target as TextExt).shortCodesEnabled = EditorGUILayout.ToggleLeft( "Enable short codes", (target as TextExt).shortCodesEnabled );
				//GUILayout.Label("* To test: try inputting :smiley: in the text");


				/*GUILayout.BeginHorizontal();
				if( GUILayout.Button ("Set Random Emoji" ) ) {
					// Pick a random emoticon
					var emojiCharCode = Random.Range( 0x1F600, 0x1F64F );
					string str = System.Char.ConvertFromUtf32( emojiCharCode );
					(this.target as TextWithEmojiSupport ).text = str + " Emoji support" + str;
					(this.target as TextWithEmojiSupport).SetAllDirty();
					UnityEditor.EditorUtility.SetDirty( target );
				}
				if( GUILayout.Button ("Add Random Emoji" ) ) {
					// Pick a random emoticon
					var emojiCharCode = Random.Range( 0x1F600, 0x1F64F );
					string str = System.Char.ConvertFromUtf32( emojiCharCode );
					(this.target as TextWithEmojiSupport ).text += str;
					(this.target as TextWithEmojiSupport).SetAllDirty();
					UnityEditor.EditorUtility.SetDirty( target );
				}
				GUILayout.EndHorizontal();*/
		    }
		}

	}
}
