#pragma warning disable 219
/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;


namespace MobyShop {


	public class PropertyDrawer {


		// 
		public static void DrawProperties( IPropertiesInfoSet p, System.Func<IPropertiesInfoSet,BTPrjSetting,bool> onDrawProperty, ref bool hasChanged2 ) {
			GUILayout.Label("");
			int numNC = 0;
			bool hasChanged = false;
			//Selection.activeObject = this;
			if( p == null || p.Keys == null ){
				GUILayout.Label("could not draw properties (null)");
				return;
			}

			if( p.Keys.Length == 0 ) {
				GUILayout.Label("could not draw properties (no keys)");
				return;
			}

			// sort properties into groups.
			int iter = 0;
			System.Collections.Generic.List<string> groups = new System.Collections.Generic.List<string>();
			groups.Add( "" );
			foreach( var k in p.Keys ){
				if( k == null )
					Debug.LogError ("Error key in collection is null");
				int i = iter;
				string s_grp = p.GetGroup(i).ToLower();
				if( !groups.Contains( s_grp ) ) {
					groups.Add( s_grp );
				}
				iter++;
			}

			// 
			foreach( var g in groups ) {
				if( g==null || g == "" || g == "default" ) {  // You're not able to fold or Unfold a default group.
					p.ForEachPropertyInGroup( g, ( BTPrjSetting propInfo ) => {
						if( onDrawProperty == null || onDrawProperty( p, propInfo ) == false ) {
							DrawProperty( p, propInfo, ref numNC, ref hasChanged );
						}
					} );
				} else {
					var v1 = GetGroupFoldValue(g);
					bool v2 = EditorGUILayout.Foldout( v1, g );  
					if( v1 != v2 ) {
						SetGroupFoldValue( g, v2 );
					}
					if( v2 ) {
						p.ForEachPropertyInGroup( g, ( BTPrjSetting propInfo ) => {
							if( onDrawProperty == null || onDrawProperty( p, propInfo ) == false ) {
								DrawProperty( p, propInfo, ref numNC, ref hasChanged );
							}
						} );
					}
				}
			}

			if( numNC > 0 ) {
				var normColor = GUI.color;
				GUI.color = Color.yellow;
				GUILayout.Label( "* the marked items needs to be filled out before you can continue." );
				GUI.color = normColor;
			}

			hasChanged2 = hasChanged;
		}


		static public System.Collections.Generic.Dictionary<string,bool> groupOnOff = new System.Collections.Generic.Dictionary<string, bool>();


		static bool GetGroupFoldValue( string str_group, bool defaultValue=true ) {
			if( !groupOnOff.ContainsKey( str_group ) ) {
				groupOnOff.Add( str_group, defaultValue );
			} 
			return groupOnOff[str_group];
		}


		static void SetGroupFoldValue( string str_group, bool new_Value ) {
			if( !groupOnOff.ContainsKey( str_group ) ) {
				groupOnOff.Add( str_group, new_Value );
			} else {
				groupOnOff[str_group] = new_Value;
			}
		}


		public static void DrawProperty( IPropertiesInfoSet p, BTPrjSetting setting, ref int numNC, ref bool hasChanged ){
			int i = setting.index;
			string k = setting.key;
			var wr = p.WriteEnabled( k );
			if( wr ) {
				GUI.enabled= true;
				bool nc = p.NeedsChange( k ) && p.HasChangedFromDefault( k );
				if( nc ) numNC++;
				var normColor = GUI.color;
				GUI.color = nc ? Color.yellow : normColor;
				string reason = "";
				if( nc == false && p.IsValidValue( k, out reason )==false ) {
					GUI.color = Color.red;
				}
				//GUILayout.Label( "" + p.DispName( k ) + " = " + p.GetValue( k ) ); 
				string oldvalue = p.GetValue( k );

				string strT = p.GetStrType(k);
				if( strT.StartsWith("string:") ) {
					var str_options = strT.Replace("string:", "" ).Replace(" ", "").Split( new char[]{','} ).ToList();
					int iof = str_options.IndexOf( p.GetValue( i ) );
					string str_option = str_options[iof];
					if( str_options.Count > 0 ) {
						GUILayout.BeginHorizontal();
						GUILayout.Label(""+k,GUILayout.Width(145));
						int inew = EditorGUILayout.Popup( iof, str_options.ToArray() );
						if( inew != iof ) {
							iof = inew;
							//Debug.Log( "SetVAlue = str_options[iof] = " + str_options[iof] );
							p.SetValue( k, str_options[iof] );
						}
						GUILayout.EndHorizontal();
					} else {
						GUI.enabled = false;
						EditorGUILayout.TextField( p.DispName(k), "NO OPTIONS AVAIL" );
					}
				} else {
					try {
						bool drawn;
						object objvalue = FieldByType( p.GetSystemType(k), p.DispName(k), p.GetValueAsRealButAnonType(k), out drawn );
						string strNewVal = System.Convert.ToString( objvalue );
						if( strNewVal != oldvalue ) {
							hasChanged = true;
							if( strNewVal == "" ) {
								//Debug.LogError("new value is empty");
							}
							//Debug.Log( "Setting value : oldval = " + oldvalue + " newv=" + strNewVal );
							p.SetValue( k, strNewVal );
						}	
					} catch(System.Exception e ) {
						Debug.LogError(""+e);	
					}
				}

				if( !string.IsNullOrEmpty(reason) ) {
					GUILayout.Label( "* " + reason ); 
				}
				GUI.color = normColor;
			} else {
				GUI.enabled = false;
				EditorGUILayout.TextField( p.DispName(k), p.GetValue(k) );
			}
		}
			

		protected static void DrawArrayList( string label, ArrayList obj ){
			EditorGUILayout.TextField( label, MiniJSON.jsonEncode(obj) );
		}


		protected static void DrawEnum( string label, System.Enum en ) {
			throw new System.NotImplementedException();
		}


		protected static object StringToValue( System.Type type, object str_value ) {
			System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
			if( type  == typeof( float ) ) {
				return System.Convert.ToSingle( str_value );
			} else if( type == typeof(bool) ) {
				return System.Convert.ToBoolean( str_value );
			} else if( type == typeof(string) ) {
				return System.Convert.ToString( str_value );
			} else if( type == typeof(int) ) {
				return (int)System.Convert.ToInt32( str_value );
			} else if( type == typeof(ArrayList) ) {
				string str = (string)str_value;
				var comps = str.Split( new char[]{','}, System.StringSplitOptions.RemoveEmptyEntries );
				var arr = new ArrayList();
				foreach( var comp in comps ) {
					arr.Add( comp );
				}
				return arr;
			} else if( type == typeof(System.Enum) ) {
				//return (System.Enum)(int)System.Convert.ToInt32( str_value );
			}
			throw new System.Exception("unknown type : " + type.Name );
		}

		/*protected static T FieldByType<T>( string label, T value ) {
			System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
			if( typeof(T)  == typeof( float ) ) {
				return (T)typeof(EditorGUILayout).GetMethod( "FloatField", bf ).Invoke( null, new object[]{ label,value } );
			} else if( typeof(T) == typeof(bool) ) {
				return (T)typeof(EditorGUILayout).GetMethod( "ToggleLeft", bf ).Invoke( null, new object[]{ label,value } );
			} else if( typeof(T) == typeof(string) ) {
				return (T)typeof(EditorGUILayout).GetMethod( "TextField", bf ).Invoke( null, new object[] { label,value } );
			} else if( typeof(T) == typeof(int) ) {
				return (T)typeof(EditorGUILayout).GetMethod( "IntField", bf ).Invoke( null, new object[] { label,value } );
			} else if( typeof(T) == typeof(ArrayList) ) {
				return (T)typeof(NewProjectWnd).GetMethod( "DrawArrayList", bf ).Invoke( null, new object[] { label,value } );
			} else if( typeof(T) == typeof(System.Enum) ) {
				return (T)typeof(NewProjectWnd).GetMethod( "DrawEnum", bf ).Invoke( null, new object[] { label,value } );
			}
			throw new System.Exception("unknown type : " + typeof(T).Name );
		}*/


		protected static object FieldByType( System.Type t, string label, object value, out bool drawn ) {
			drawn = true;
			System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
			if( t  == typeof( float ) ) {
				var sig = new System.Type[]{ typeof(string),typeof(float),typeof(GUILayoutOption[])};
				var tret = typeof(float);
				return typeof(EditorGUILayout).GetMethodBySig( "FloatField", tret, sig ).Invoke( null, new object[]{ label,System.Convert.ToSingle(value), null  } );
			} else if( t == typeof(bool) ) {
				var sig = new System.Type[]{ typeof(string),typeof(bool),typeof(GUILayoutOption[])};
				var tret = typeof(bool);
				return typeof(EditorGUILayout).GetMethodBySig( "ToggleLeft", tret, sig ).Invoke( null, new object[]{ label,System.Convert.ToBoolean(value), null } );
			} else if( t == typeof(string) ) {
				var sig = new System.Type[]{ typeof(string),typeof(string),typeof(GUILayoutOption[])};
				var tret = typeof(string);
				return typeof(EditorGUILayout).GetMethodBySig( "TextField", tret, sig ).Invoke( null, new object[] { label,System.Convert.ToString(value), null } );
			} else if( t == typeof(int) ) {
				var sig = new System.Type[]{ typeof(string),typeof(int),typeof(GUILayoutOption[])};
				var tret = typeof(int);
				return typeof(EditorGUILayout).GetMethodBySig( "IntField", tret, sig ).Invoke( null, new object[] { label,System.Convert.ToInt32(value), null  } );
			} else if( t == typeof(ArrayList) ) {
				//var sig = new System.Type[]{ typeof(string),typeof(int),typeof(GUILayoutOption[])};
				//var tret = typeof(int);
				//return typeof(NewProjectWnd).GetMethod( "DrawArrayList", tret, sig ).Invoke( null, new object[] { label,value } );
			} else if( t == typeof(System.Enum) ) {
				//return typeof(NewProjectWnd).GetMethod( "DrawEnum", tret, sig ).Invoke( null, new object[] { label,value } );
			} else if( t == null ) {
				drawn=false;
				return null;
			} else if( t.IsClass ) {
				drawn=false;
				return null;
			} else if( t.IsEnum ) {
				//Debug.Log("valueType = " + value.GetType().Name );
				GUILayout.BeginHorizontal();
				GUILayout.Label(label);
				System.Enum retVal = EditorGUILayout.EnumPopup( (System.Enum)value );
				GUILayout.EndHorizontal();
				return retVal;
			} 
			throw new System.Exception("unknown type : " + t.Name );
		}




	}

}