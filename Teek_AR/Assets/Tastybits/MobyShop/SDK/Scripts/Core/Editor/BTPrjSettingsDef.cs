/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using System.Linq;


namespace MobyShop {

	public enum BTSettingType {
		String,
		Int,
		Bool,
		Float,
		Array,
		Enum,
		Class
	}


	public enum  BTSFormat {
		Undefined = -1,
		CanBeEmptyString = 0,
		MustBeNonEmptyStr,
		MustBeValidURL, 
		JSON,
		_LAST
	}


	public enum BTProjectTypes {
		None = 0,
		GenericUnityProject = 1,
		QuizGame = 2,
		RunnerGame = 3,
		RateApp
	}


	public class BTPrjSetting {
		public int index;
		public string key;
		public string dispName;
		public BTSettingType type;
		public string defaultValue;
		public string value;
		public bool writeEnabled;
		public bool needsChange;
		public BTSFormat format;
	}


	public interface IPropertiesInfoSet {
		bool WriteEnabled( string key );
		bool NeedsChange( string key );
		bool HasChangedFromDefault( string key );
		bool IsValidValue( string key, out string reason );
		string GetValue( string key );
		string GetValue( string key, string undefined );
		string GetValue( int i, string undefined=null );
		string DispName( string key );
		System.Type GetSystemType( string k );
		object GetValueAsRealButAnonType( string key );
		void SetValue( string key, string value );
		string GetStrType( string key );
		string[] Keys { get; }
		string GetGroup( int i );
		void ForEachPropertyInGroup( string grp, System.Action<BTPrjSetting> d );
	}


	[System.Serializable]
	public class BTPrjSettingsDef : IPropertiesInfoSet {
		public BTProjectTypes ProjectType;
		public System.Collections.Generic.List<string> keys = new System.Collections.Generic.List<string>();
		public System.Collections.Generic.List<string> dispName = new System.Collections.Generic.List<string>();
		public System.Collections.Generic.List<string> types = new System.Collections.Generic.List<string>();
		public System.Collections.Generic.List<string> defaultValue = new System.Collections.Generic.List<string>();
		public System.Collections.Generic.List<string> value = new System.Collections.Generic.List<string>();
		public System.Collections.Generic.List<bool> writeEnabled = new System.Collections.Generic.List<bool>();
		public System.Collections.Generic.List<bool> needsChange = new System.Collections.Generic.List<bool>();
		public System.Collections.Generic.List<int> format = new System.Collections.Generic.List<int>();
		public System.Collections.Generic.List<string> group = new System.Collections.Generic.List<string>();
		public System.Collections.Generic.List<string> logic = new System.Collections.Generic.List<string>();


		public void ForEach( System.Action<BTPrjSetting> d ) {
			foreach( var k in keys ){
				var s = new BTPrjSetting();
				int i = keys.IndexOf(k);
				s.index = i;
				s.key = k;
				s.dispName = dispName[i];
				s.type = Str2ST(types[i]);
				s.defaultValue = defaultValue[i];
				s.value = value[i];
				s.writeEnabled = writeEnabled[i];
				s.needsChange = needsChange[i];
				s.format = IntToFormat( format[i] );
				d(s);
			}
		}


		public void ForEachPropertyInGroup( string grp, System.Action<BTPrjSetting> d ) {
			for( int i=0; i<keys.Count ; i++ ) {
				string k = keys[i];
				//string v = value[i];
				string g = group[i].ToLower();
				if( g == grp.ToLower() ){
					var s = new BTPrjSetting();
					s.index = i;
					s.key = k;
					s.dispName = dispName[i];
					s.type = Str2ST( GetPropertyTypeAsString(i) );
					s.defaultValue = defaultValue[i];
					s.value = value[i];
					s.writeEnabled = writeEnabled[i];
					s.needsChange = needsChange[i];
					s.format = IntToFormat( format[i] );
					d(s);
				}
			}
		}


		public string GetPropertyTypeAsString( int i ) {
			while( types.Count < i + 1 ) {
				types.Add( "string" );
			}
			return types[i];
		}


		public string GetGroup( int index ) {
			while( group.Count < index +1 ) {
				group.Add( "Default" );
			}
			return group[index];
		}


		public bool ContainsKey( string k ) {
			int i = keys.IndexOf (k);
			return i >= 0;
		}


		static ArrayList ToArrayList<T>( ref System.Collections.Generic.List<T> src ) {
			var dst = new ArrayList();
			for( int i = 0; i<src.Count; i++ ) {
				dst.Add( src[i] );
			} 
			return dst;
		}


		public string ToJSONEncodedString() {
			var tbl = new Hashtable();

			tbl.Add( "keys", ToArrayList<string>( ref keys ) );
			tbl.Add( "dispName", ToArrayList<string>( ref dispName ) );
			tbl.Add( "types", ToArrayList<string>( ref types ) );
			tbl.Add( "defaultValue", ToArrayList<string>( ref defaultValue ) );
			tbl.Add( "value", ToArrayList<string>( ref value ) );
			tbl.Add( "writeEnabled", ToArrayList<bool>( ref writeEnabled ) );
			tbl.Add( "needsChange", ToArrayList<bool>( ref needsChange ) );
			tbl.Add( "format", ToArrayList<int>( ref format ) );

			return MiniJSON.jsonEncode( tbl );
		}



		public static BTPrjSettingsDef FromJSONEncodedString( string str ) {
			var tbl = (Hashtable)MiniJSON.jsonDecode( str );

			if( tbl == null ) {
				throw new System.Exception("Error, string is not json or json object : " + str );
			}

			var ret = new BTPrjSettingsDef();

			ret.keys = tbl.GetArrayOfSimple<string>( "keys" ).ToList();
			ret.dispName = tbl.GetArrayOfSimple<string>( "dispName" ).ToList();
			ret.types = tbl.GetArrayOfSimple<string>( "types" ).ToList();
			ret.defaultValue = tbl.GetArrayOfSimple<string>( "defaultValue" ).ToList();
			ret.value = tbl.GetArrayOfSimple<string>( "value" ).ToList();
			ret.writeEnabled = tbl.GetArrayOfSimple<bool>( "writeEnabled" ).ToList();
			ret.needsChange = tbl.GetArrayOfSimple<bool>( "needsChange" ).ToList();

			//Debug.Log( tbl.Get<ArrayList>( "format" )[0].GetType().Name );

			var _format = tbl.GetArrayOfSimple<System.Double> ("format");
			ret.format = new System.Collections.Generic.List<int> ();
			foreach( var e in _format ) {
				ret.format.Add (System.Convert.ToInt32 ((System.Double)e));
			}

			return ret;
		}


		public bool GetAllSet( out System.Collections.Generic.List<string> problems ) {
			problems = new System.Collections.Generic.List<string>();

			int i=0;
			bool r = true;
			foreach( var k in keys ) {
				bool nc = needsChange[i];// must be diffirent than original value
				bool wc = defaultValue[i] == value[i];  // check if original value..

				var tmp = false;
				if (tmp) {
					Debug.Log( "tmp" + tmp + " " + k );
				}


				BTSFormat fmt = IntToFormat(format[i]);
				string problemstr = "";
				bool formatOk = true;
				if( fmt== BTSFormat.CanBeEmptyString ) {
					// everything is ok.
				} else if( fmt == BTSFormat.MustBeNonEmptyStr) {
					if( string.IsNullOrEmpty(value[i].Trim()) ) {
						formatOk = false;
						problemstr = ""+ dispName[i] + ": cannot be empty string";
					} 
				} else if( fmt == BTSFormat.MustBeValidURL ) {

				}

				if( (nc && wc) && problemstr == "" ) {
					problemstr = ""+ dispName[i] + ": must be set";
				}

				if( problemstr!="" ) {
					problems.Add( problemstr );
				}

				if( (nc && wc) || !formatOk )  {
					r = false;
				}
				i++;
			}

			return r;
		}

		public bool AllSet {
			get {
				System.Collections.Generic.List<string> dummy;
				return GetAllSet( out dummy );
			}
		}


		static void AddDefaultSettings( ref BTPrjSettingsDef templ ) {
			templ.AddItem( "bi", "Bundle Identifier", "", "com.ThisLittleCompany.ThisAppId" );
		}


		// The prjoect settings....
		static public BTPrjSettingsDef GetProjectSettingsByProjectType( BTProjectTypes pt ) {
			var templ = new BTPrjSettingsDef();
			AddDefaultSettings( ref templ );
			if( pt == BTProjectTypes.GenericUnityProject ) {
				templ.AddItem( "apptype", "App type", "string", "", true, true, false );
				return templ;
			}
			else if( pt == BTProjectTypes.QuizGame ) {
				templ.AddItem( "apptype", "App Type", "string", "", true, true, false );
				return templ;
			}
			throw new System.Exception("invalid pt");
		}


		// Used when we want settings to create the project...
		public static BTPrjSettingsDef GetCreateProjectSettingsForType( BTProjectTypes t ) {
			var templ = new BTPrjSettingsDef();
			templ.ProjectType = t;
			templ.AddItem( "bi", "Bundle Identifier", "", "com.ThisLittleCompany.ThisAppId" );
			templ.AddItem( "prjname", "Project Name", "string", "", true, true, false ); 
			if( t == BTProjectTypes.GenericUnityProject ) {
				return templ;
			} else if( t == BTProjectTypes.QuizGame ) {
				// The main scene is the scene that we must select 
				//templ.AddItem( "mainscene", "Main Scene", "string", "001_GenericQuiz", true, false, false ); 
				//templ.AddItem( "online_prefs_url", "Online Prefs Url (Google TSV/JSON Prefs)", "string", "", true ); 
				//templ.AddItem( "quiz_url", "Quiz Data Url (Google Sheet/TSV)", "string", "", true, true, false ); 

				templ.AddItem( "run_method", "Run Method On Build", "string", "QuizPublishSettings.Apply( \"https://docs.google.com/spreadsheets/d/spreadsheetId/pub?gid=0&single=true&output=tsv\" )", true, true, false );

				return templ;
			} else if( t == BTProjectTypes.RunnerGame ) {


				return templ;
			} else if( t == BTProjectTypes.RateApp ) {


				//templ.AddItem( "" );
				return templ;
			}
			throw new System.Exception("invalid settp");
		}


		public static string ST2Str( BTSettingType t ){
			if( t == BTSettingType.String ){
				return "string";
			} else if( t == BTSettingType.Int ) {
				return "int";
			} else if( t == BTSettingType.Bool) {
				return "bool";
			} else if( t == BTSettingType.Float ) {
				return "float";
			} else if( t == BTSettingType.Array ) {
				return "a";
			} else if( t == BTSettingType.Enum ) {
				return "e";
			}
			throw new System.Exception ( "unknown type :" + t );
		}


		public static BTSettingType Str2ST( string t ){
			if( t == "string" || t == "" ) {
				return BTSettingType.String;
			} else if( t == "int" ) {
				return BTSettingType.Int;
			} else if( t =="bool" ) {
				return BTSettingType.Bool;
			} else if( t == "float" ) {
				return BTSettingType.Float;
			} else if( t == "a" ) {
				return BTSettingType.Array;
			} else if( t == "e" || t.StartsWith("string:") ) {
				return BTSettingType.Enum;
			}
			throw new System.Exception ( "unknown type :" + t );
		}


		public string[] Keys {
			get {
				return keys.ToArray();
			}
		}


		public void AddItem( string key, string _dispName, string str_type, string _defaultValue, bool _writeEnabled=true, bool _needsChange=false, bool canBeEmptyString=true ) {
			AddItem( 5, key, _dispName, str_type, _defaultValue, _writeEnabled, _needsChange, canBeEmptyString);
		}


		public void AddItem( int prio, string key, string _dispName, string str_type, string _defaultValue, bool _writeEnabled=true, bool _needsChange=false, bool canBeEmptyString=true, string str_group="", string str_logic = "" ) {
			if( keys.IndexOf(key) >= 0 ) {
				throw new System.Exception("cannot add key more than one time : " + key);
			}
			AddItem( prio, key, _dispName, str_type, _defaultValue, _writeEnabled, _needsChange, (canBeEmptyString ? BTSFormat.CanBeEmptyString : BTSFormat.MustBeNonEmptyStr), str_group, str_logic );
		}


		public void AddItem( int prio, string key, string _dispName, string str_type, string _defaultValue, bool _writeEnabled, bool _needsChange, BTSFormat fmt=BTSFormat.CanBeEmptyString, string _sgroup="", string _logic="" ) {
			if( keys.IndexOf(key) >= 0 ) {
				throw new System.Exception("cannot add key more than one time : " + key);
			}
			keys.Add(key);
			types.Add(str_type);
			value.Add( _defaultValue );
			dispName.Add(_dispName);
			defaultValue.Add(_defaultValue);
			writeEnabled.Add(_writeEnabled);
			needsChange.Add(_needsChange);
			this.group.Add(_sgroup);
			this.logic.Add(_logic);
			this.format.Add( (int)fmt );
		}


		public void SetItem( string key, string _dispName, string str_type, string _defaultValue, bool _writeEnabled=true, bool _needsChange=false, bool canBeEmptyString=true ) {
			if( !(keys.IndexOf(key) >= 0 ) ) {
				throw new System.Exception("cannot add key more than one time : " + key);
			}
			int i = keys.IndexOf (key);
			types [i] = str_type;
			value[i] = ( _defaultValue );
			dispName[i] = (_dispName);
			defaultValue[i] = (_defaultValue);
			writeEnabled[i] = (_writeEnabled);
			needsChange[i] = (_needsChange);
			format[i] = ( (int)(canBeEmptyString ? BTSFormat.CanBeEmptyString : BTSFormat.MustBeNonEmptyStr) );
		}


		public void SetValueAndDefaultValue( string key, string _value ) {
			if( !(keys.IndexOf(key) >= 0 ) ) {
				throw new System.Exception("cannot add key more than one time : " + key);
			}
			int i = keys.IndexOf (key);
			value[i] = ( _value );
			defaultValue[i] = (_value);
		}


		public void SetWriteable( string key, bool _value ) {
			if( !(keys.IndexOf(key) >= 0 ) ) {
				throw new System.Exception("cannot add key more than one time : " + key);
			}
			int i = keys.IndexOf (key);
			this.writeEnabled [i] = _value;
		}


		public void SetNeedsChange( string key, bool val ) {
			if( !(keys.IndexOf(key) >= 0 ) ) {
				throw new System.Exception("cannot add key more than one time : " + key);
			}
			int i = keys.IndexOf (key);
			this.needsChange [i] = val;
		}


		public void Add( string key, string value ) {
			AddItem( key, key, "string", value, true, false, true ); 
		}


		public void Add( string key, int value ) {
			AddItem( key, key, "int", ""+value, true, false, true ); 
		}

		public void Add( string key, float value ) {
			AddItem( key, key, "float", ""+value, true, false, true ); 
		}

		public void Add( string key, bool value ) {
			AddItem( key, key, "bool", ""+value, true, false, true ); 
		}


		public System.Type GetSystemType( string k ) {
			string t = types[ keys.IndexOf(k) ];
			if( t == "string" || t == "" || t.StartsWith("string:") ) {
				return typeof(string);
			} else if( t == "int" ) {
				return typeof(int);
			} else if( t =="bool" ) {
				return typeof(bool);
			} else if( t == "float" ) {
				return typeof(float);
			} else if( t == "a" ) {
				return typeof(ArrayList);
			} else if( t == "e" ) {
				return typeof(System.Enum);
			}
			throw new System.Exception ( "unknown type :" + t );
		}


		public string GetStrType( string k ) {
			return types[ keys.IndexOf(k) ];
		}


		static object StringToValue( System.Type type, object str_value ) {
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
			}
			throw new System.Exception("unknown type : " + type.Name );
		}


		public object GetValueAsRealButAnonType( string key ) {
			int i = keys.IndexOf(key);
			//string t = types[ i ];
			string v = value[ i ];
			return StringToValue( GetSystemType(key), v );
		}


		public bool NeedsChange( string key ) {
			return needsChange[ keys.IndexOf(key) ];
		}

		public bool HasChangedFromDefault( string key ) {
			int i = keys.IndexOf(key);
			return defaultValue[i] == value[i];
		}

		public bool IsValidValue( string key, out string reason ) {
			int i = keys.IndexOf(key);
			reason = "";
			if( format[i] == (int) BTSFormat.MustBeNonEmptyStr && value[i] == "" ) {
				reason = "cannot be empty string";
				return false;
			}
			return true;
		}



		public string DefaultValue( string key ) {
			return defaultValue[ keys.IndexOf(key) ];
		}


		public bool WriteEnabled( string key ) {
			return writeEnabled[ keys.IndexOf(key) ];
		}


		public string DispName( string key ) {
			return dispName[ keys.IndexOf(key) ];
		}


		public string GetValue( string key, string undefined ) {
			var i = keys.IndexOf (key);
			if (i < 0) {
				return undefined;
			}
			try {
				return value[ i ];
			} catch( System.Exception e ) {
				Debug.LogError ("Exception getting setting wth key : " + key + "\n" + e.ToString() );
				return undefined;
			}
		}


		public string GetValue( string key ) {
			var i = keys.IndexOf (key);
			if (i < 0) {
				Debug.LogError ("Error getting value for key : " + key);
				throw new System.Exception("Error getting value for key : " + key);
			}
			try {
				return value[ i ];
			} catch( System.Exception e ) {
				Debug.LogError ("Exception getting setting wth key : " + key);
				throw e;
			}
		}


		public string GetValue( int i, string undefined=null ) {
			if (i < 0) {
				Debug.LogError ("Error getting value for key : " + i);
			}
			if( i >= value.Count ) {
				Debug.LogError("Error getting the value for in dex : " + i );
			}
			try {
				return value[ i ];
			} catch( System.Exception e ) {
				Debug.LogError ("Error getting setting wth index : " + i);
				throw e;
			}
		}
			

		public void SetValue( string key, string value ) {
			this.value[ keys.IndexOf(key) ] = value;
		}


		public string this[ string key ] {
			get {
				return GetValue( key );
			}
			set {
				SetValue( key, value );
			}
		}


		public string this[ int i ] {
			get {
				return this.value[i];
			}
			set {
				this.value[i] = value;
			}
		}


		public string GetType2( string key ) {
			return types[ keys.IndexOf(key) ];
		}


		public BTSettingType GetType( string key ) {
			return Str2ST( types[ keys.IndexOf(key) ] );
		}


		public static BTSFormat IntToFormat( int i ) {
			BTSFormat r = BTSFormat.CanBeEmptyString;
			if( i>=(int)BTSFormat.CanBeEmptyString && i < (int)BTSFormat._LAST ) {
				r = (BTSFormat)i;
			}
			return r;
		}

	}



}