using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.IO;

namespace crudwork.DynamicRuntime
{
	/// <summary>
	/// The Scripting Environment
	/// </summary>
	public class ScriptEnvironment
	{
		#region Properties
		public ScriptRuntime Runtime
		{
			get;
			private set;
		}
		public ScriptScope Scope
		{
			get;
			private set;
		}
		public ObjectOperations Operations
		{
			get;
			private set;
		}
		public ScriptEngine Engine
		{
			get;
			private set;
		}
		public string LanguageId
		{
			get;
			private set;
		}

		private Dictionary<int, CompiledCode> codeCache;
		#endregion

		#region sample app.config
		/*
		<?xml version="1.0" encoding="utf-8" ?>
		<configuration>
			<configSections>
				<section name="microsoft.scripting"
					type="Microsoft.Scripting.Hosting.Configuration.Section, Microsoft.Scripting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />
			</configSections>
			<microsoft.scripting>
				<languages>
					<language names="IronPython;Python;py" extensions=".py"
						displayName="IronPython v2.6"
						type="IronPython.Runtime.PythonContext, IronPython, Version=2.6.10920.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />

					<language names="IronRuby;Ruby;rb" extensions=".rb"
						displayName="IronRuby v1.0"
						type="IronRuby.Runtime.RubyContext, IronRuby, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />

				</languages>
			</microsoft.scripting>
		</configuration>
		*/
		#endregion

		#region Constructors
		/// <summary>
		/// Create new instance with given attribute
		/// </summary>
		/// <param name="languageId"></param>
		public ScriptEnvironment(string languageId)
		{
			LanguageId = languageId;

			Runtime = ScriptRuntime.CreateFromConfiguration();
			Scope = Runtime.CreateScope();
			Operations = Runtime.CreateOperations();
			Engine = Runtime.GetEngine(LanguageId);

			codeCache = new Dictionary<int, CompiledCode>();
		}
		#endregion

		#region Helper methods
		/// <summary>
		/// Compile the text and add it to the cache, for later re-use.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private CompiledCode CompileScript(string text)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");

			var hash = text.GetHashCode();

			if (codeCache.ContainsKey(hash))
				return codeCache[hash];

			var s = Engine.CreateScriptSourceFromString(text, SourceCodeKind.Statements);
			var c = s.Compile();

			codeCache.Add(hash, c);
			return c;
		}
		#endregion

		/// <summary>
		/// Get or set the scope's variable
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object this[string name]
		{
			get
			{
				return Scope.GetVariable(name);
			}
			set
			{
				Scope.SetVariable(name, value);
			}
		}

		/// <summary>
		/// Source the filename
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public dynamic ExecuteFile(string filename)
		{
			//var script = Engine.CreateScriptSourceFromFile(filename);
			//var c = script.Compile();
			//return c.Execute(Scope);

			var text = File.ReadAllText(filename);
			return Execute(text);
		}

		//public ScriptScope UseFile(string filename)
		//{
		//    //return Runtime.UseFile(filename);
		//}

		/// <summary>
		/// Execute the text
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public dynamic Execute(string text)
		{
			var c = CompileScript(text);
			return c.Execute(Scope);
		}
	}
}
