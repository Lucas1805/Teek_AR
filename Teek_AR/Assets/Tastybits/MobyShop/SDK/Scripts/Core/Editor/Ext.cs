/**
 * @desc 	The following source-file is part of MobyShop - a solution for creating a Free-2-play ingame shop in your game.
 * @author 	Tastybits | www.tastybits.io
 * @license See assetstore for license details. 
 * @copyright 2016 Tastybits
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace MobyShop {


	public static class HashTableExtras {
		public static T Get<T>( this System.Collections.Hashtable self, object key, object def=null ) {
			if( !self.ContainsKey( key ) ) self.Add( key, def );
			return (T)self[key];
		}

		public static string GetParamType( this System.Collections.Hashtable self, object key ) {
			if( !self.ContainsKey( key ) ) return "undefined";
			return self[key].GetType().Name;
		}

		public static void Set<T>( this System.Collections.Hashtable self, object key, T val ) {
			if( !self.ContainsKey( key ) ) self.Add( key, val );
			self[key] = val;
		}
		public static object Set( this System.Collections.Hashtable self, object key, object val ) {
			if( !self.ContainsKey( key ) ) self.Add( key, val );
			self[key] = val;
			return val;
		}
		public static bool GetBool( this System.Collections.Hashtable self, object key, bool def=false ) {
			if( !self.ContainsKey( key ) ) self.Add( key, def?"true":"false" );
			return ((string)self[key])=="true";
		}
		public static bool SetBool( this System.Collections.Hashtable self, object key, bool val ) {
			if( !self.ContainsKey( key ) ) self.Add( key, val?"true":"false" );
			self[key] = val?"true":"false";
			return val;
		} 	
		public static bool IsSet( this System.Collections.Hashtable self, object key ) {
			return self.ContainsKey (key);
		}
		public static bool IsSet( this System.Collections.Hashtable self, object key, System.Type t ) {
			return self.ContainsKey (key) && self[key].GetType() == t;
		}

		public static TRet GetAs<TRet>( this System.Collections.Hashtable self, string key, TRet def=default(TRet) )  {
			if( !self.ContainsKey( key ) ) self.Add( key, def );
			if( ( (self[key] as Hashtable ) == null )) {
				throw new System.Exception ("Type not a deviation of Hashtable");
			}
			var tmp = (Hashtable)self[key];
			var inst = System.Activator.CreateInstance( typeof(TRet), new object[]{ tmp } );
			return (TRet)(inst);
		}
		public static TRet[] GetArrayOf<TRet>( this System.Collections.Hashtable self, string key, object def=default(object) )  {
			if( !self.ContainsKey( key ) ) self.Add( key, def );
			if (self [key] == null) {
				self [key] = new ArrayList ();
			} else if( ( (self[key] as ArrayList ) == null )) {
				throw new System.Exception ("Type us not ArrayList its : " + self[key].GetType().Name );
			}
			var arr = (ArrayList)self[key];
			var ret = new System.Collections.Generic.List<TRet>();
			for (int i = 0; i < arr.Count; i++) {
				Debug.Log ("" + arr [i].GetType ().Name);
				var obj = (Hashtable)arr [i];
				if( obj != null ) {
					var inst = System.Activator.CreateInstance( typeof(TRet), new object[]{ obj } );
					ret.Add( (TRet)(inst) );
				} else {
					throw new System.Exception ("object is null");
				}
			}
			return ret.ToArray();
		}
		public static TRet[] GetArrayOfSimple<TRet>( this System.Collections.Hashtable self, string key, object def=default(object) )  {
			if( !self.ContainsKey( key ) ) self.Add( key, def );
			if( self[key] == null) {
				self[key] = new ArrayList ();
			} else if( ( (self[key] as ArrayList ) == null )) {
				throw new System.Exception ("Type us not ArrayList its : " + self[key].GetType().Name );
			}
			var arr = (ArrayList)self[key];
			var ret = new System.Collections.Generic.List<TRet>();
			for (int i = 0; i < arr.Count; i++) {
				//Debug.Log( "" + arr [i].GetType ().Name );
				var obj = (TRet)arr [i];
				ret.Add( (TRet)(obj) );
			}
			return ret.ToArray ();
		}

	}


	public static class SysType
	{
		public static IEnumerable<MethodInfo> GetMethodsBySig(this System.Type type, string name, System.Type returnType, params System.Type[] parameterTypes)
		{
			return type.GetMethods().Where((m) =>
				{
					if (m.ReturnType != returnType || m.Name != name ) return false;
					var parameters = m.GetParameters();
					if ((parameterTypes == null || parameterTypes.Length == 0))
						return parameters.Length == 0;
					if (parameters.Length != parameterTypes.Length)
						return false;
					for (int i = 0; i < parameterTypes.Length; i++)
					{
						if (parameters[i].ParameterType != parameterTypes[i])
							return false;
					}
					return true;
				});
		}
		public static MethodInfo GetMethodBySig( this System.Type self,string name, System.Type returnType, params System.Type[] parameterTypes) {
			return self.GetMethodsBySig( name, returnType, parameterTypes ).FirstOrDefault();
		}
	}


	public static class EditorWndExt {
		public static System.Type[] GetAllDerivedTypes(this System.AppDomain aAppDomain, System.Type aType) {
			var result = new List<System.Type>();
			var assemblies = aAppDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					if (type.IsSubclassOf(aType))
						result.Add(type);
				}
			}
			return result.ToArray();
		}

		public static Rect GetEditorMainWindowPos() {
			var containerWinType = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).Where(t => t.Name == "ContainerWindow").FirstOrDefault();
			if (containerWinType == null)
				throw new System.MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
			var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var positionProperty = containerWinType.GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			if (showModeField == null || positionProperty == null)
				throw new System.MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
			var windows = Resources.FindObjectsOfTypeAll(containerWinType);
			foreach (var win in windows)
			{
				var showmode = (int)showModeField.GetValue(win);
				if (showmode == 4) // main window
				{
					var pos = (Rect)positionProperty.GetValue(win, null);
					return pos;
				}
			}
			throw new System.NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
		}

		public static void CenterOnMainWin(this UnityEditor.EditorWindow aWin) {
			var main = GetEditorMainWindowPos();
			var pos = aWin.position;
			float w = (main.width - pos.width)*0.5f;
			float h = (main.height - pos.height)*0.5f;
			pos.x = main.x + w;
			pos.y = main.y + h;
			aWin.position = pos;
		}


		public static void CenterOnMainWinHalfTheSize(this UnityEditor.EditorWindow aWin) {
			var main = GetEditorMainWindowPos();
			var pos = aWin.position;
			pos.width = main.width / 2;
			pos.height = main.height / 2;
			float w = (main.width - pos.width)*0.5f;
			float h = (main.height - pos.height)*0.5f;
			pos.x = main.x + w;
			pos.y = main.y + h;
			aWin.position = pos;
		}

		public static void CenterOnMainWinAsPortrait(this UnityEditor.EditorWindow aWin) {
			var main = GetEditorMainWindowPos();
			var pos = aWin.position;
			pos.height = main.height - (main.height/10);
			pos.width = (pos.height / 4) * 3; //main.width / 2;
			float w = (main.width - pos.width)*0.5f;
			float h = (main.height - pos.height)*0.5f;
			pos.x = main.x + w;
			pos.y = main.y + h;
			aWin.position = pos;
		}
	}





}