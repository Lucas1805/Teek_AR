/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;

namespace MobyShop {


	[System.Serializable]
	public class ProductInfoPropertiesInfo : IPropertiesInfoSet {
		public ProductInfo product;
		public System.Collections.Generic.List<System.Reflection.FieldInfo> fields = null;

		public bool hasChanged = false;
		public bool WriteEnabled( string key ) { 
			return true;
		}
		public bool NeedsChange( string key ) { 
			return false;
		}
		public bool HasChangedFromDefault( string key ) { 
			return false;
		}
		public bool IsValidValue( string key, out string reason ) {
			reason = "";
			return true;
		}
		public string GetValue( string key ) { 
			var field = fields.Find( x=>x.Name == key );
			if( field!=null ) {
				return System.Convert.ToString( field.GetValue( product ) );
			}
			return "";
		}
		public string GetValue( string key, string undefined ) { 
			return "";
		}
		public string GetValue( int i, string undefined=null ) { 
			return "";
		}

		public string DispName( string key ) { 
			if( key.Length >= 1 ) {
				string str = key.Substring(0, 1 );
				if( key.Length-1 > 0 ) {
					str = str.ToUpper() + key.Substring(1,key.Length-1);
				}
				return str;
			}
			return key.ToUpper();
		}
		System.Reflection.FieldInfo GetField( string k ) {
			EnumFields();
			var fi = fields.Find( x=>x.Name==k );
			return fi;
		}
		public System.Type GetSystemType( string k ) { 
			EnumFields();
			var fi = GetField(k);
			if( fi == null ) return null;
			return fi.FieldType;
		}
		public object GetValueAsRealButAnonType( string k ) { 
			EnumFields();
			var fi = GetField(k);
			if( fi == null ) return null;
			return fi.GetValue( this.product );
		}
		public void SetValue( string k, string value ) { 
			EnumFields();
			var fi = GetField(k);
			if( fi == null ) return;
			object objval = StringToValue( GetSystemType(k), value );
			fi.SetValue( this.product, objval );
		}
		static object StringToValue( System.Type type, string str_value ) {
			//System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;
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
			} else if( type.IsEnum ) {
				object ret = System.Enum.Parse( type, str_value );
				return ret;
			}/* else if( type.IsClass ) {
				return null;
			}*/
			throw new System.Exception("unknown type : " + type.Name );
		}
		public string GetStrType( string key ) { 
			
			return "";
		}
		public string[] Keys { 
			get {
				EnumFields();
				System.Collections.Generic.List<string> ret = new System.Collections.Generic.List<string>();
				foreach( var field in fields ) {
					ret.Add(field.Name);
				}
				return ret.ToArray();
			}
		}
		public string GetGroup( int i ) {
			return "";
		}

		public void ForEachPropertyInGroup( string grp, System.Action<BTPrjSetting> d ) {
			EnumFields();
			int iter=0;;
			foreach( var field in fields ) {
				var prop = new BTPrjSetting();
				prop.dispName = DispName(field.Name);
				prop.format = BTSFormat.Undefined;
				prop.index = iter++;
				prop.key = field.Name;
				prop.needsChange = false;
				prop.type = Type2ST(field.FieldType);
				d(prop);	
			}
		}

		public static BTSettingType Type2ST( System.Type t ){
			if( t == typeof(string) || t == typeof(System.String) ) {
				return BTSettingType.String;
			} else if( t == typeof(int) || t == typeof(System.Int32) ) {
				return BTSettingType.Int;
			} else if( t == typeof(bool) || t == typeof(System.Boolean) ) {
				return BTSettingType.Bool;
			} else if( t == typeof(System.Single) || t==typeof(System.Double) || t == typeof(float) ) {
				return BTSettingType.Float;
			} else if( t == typeof(ArrayList) ) {
				return BTSettingType.Array;
			} else if( t == typeof(System.Enum) ) {
				return BTSettingType.Enum;
			} else if( t == typeof(ProductType) ) {
				return BTSettingType.Class;
			}
			throw new System.Exception ( "Unhandled type :" + t.Name );
		}



		public void EnumFields() {
			if( fields!=null && fields.Count > 0 ) {
				return;
			}
			fields = new System.Collections.Generic.List<System.Reflection.FieldInfo>();

			System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
			var fieldList = product.GetType().GetFields( bf );
			foreach( var field in fieldList ) {
				//Debug.Log("field = " + field.Name );
				if( System.Attribute.IsDefined(field, typeof(SerializeField) ) ) {
					fields.Add( field );
				}
			}
			bf = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
			fieldList = product.GetType().GetFields( bf );
			foreach( var field in fieldList ) {
				//-Debug.Log("field = " + field.Name );
				if( !System.Attribute.IsDefined(field, typeof(System.NonSerializedAttribute) ) ) {
					fields.Add( field );
				}
			}
		}


	}



}