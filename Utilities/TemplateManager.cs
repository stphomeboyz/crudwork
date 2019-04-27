using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using crudwork.Utilities;
using System.Text.RegularExpressions;

namespace crudwork.Utilities
{
	internal static class TemplateManagerTester
	{
		static void Main(string[] args)
		{
			var tpl = @"Hello [FIRST], my name is [SALESPERSON] at [COMPANY].  May I interest you in a new product called '[PRODUCT]'?";

			#region Table
			var dt = new DataTable();
			dt.Columns.Add("FIRST", typeof(string));
			dt.Columns.Add("SALESPERSON", typeof(string));
			dt.Columns.Add("PRODUCT", typeof(string));

			dt.Rows.Add("Bob", "James", "Acme Drink");
			dt.Rows.Add("Jim", "Erin", "Shoe Polish");
			dt.Rows.Add("Simon", "Jack", "Slim Green Jean");
			dt.Rows.Add("Erica", "Bob", "Turbo Vacuum");
			#endregion

			#region Using various delimiters
			{
				Console.WriteLine("Using delimiter: [...]");
				var mytpl = tpl;
				var tm = new TemplateManager("\\[", "\\]", "[^\\]]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: <...>");
				var mytpl = tpl.Replace("[", "<").Replace("]", ">");
				var tm = new TemplateManager("<", ">", "[^>]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: {...}");
				var mytpl = tpl.Replace("[", "{").Replace("]", "}");
				var tm = new TemplateManager("{", "}", "[^}]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: [@...]");
				var mytpl = tpl.Replace("[", "[@").Replace("]", "]");
				var tm = new TemplateManager("\\[@", "\\]", "[^\\]]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}

			{
				Console.WriteLine("Using delimiter: <<...>>");
				var mytpl = tpl.Replace("[", "<<").Replace("]", ">>");
				var tm = new TemplateManager("<<", ">>", "[^\\>]+");
				foreach (var item in tm.Expand(dt, mytpl))
				{
					Console.WriteLine(item);
				}
			}
			#endregion

			Console.WriteLine("Press ENTER to continue...");
			Console.ReadLine();
		}
	}

	/// <summary>
	/// Template Manager
	/// </summary>
	public class TemplateManager
	{
		private readonly string[] delimiters = null;
		private readonly string regexVar;

		/// <summary>
		/// create new instance with given attributes
		/// </summary>
		/// <param name="delim1"></param>
		/// <param name="delim2"></param>
		/// <param name="variableExpression"></param>
		public TemplateManager(string delim1, string delim2, string variableExpression)
		{
			delimiters = new string[] { delim1, delim2 };
			//regexVar = delim1 + @"(?<Name>[^\]]+)" + delim2;
			regexVar = delim1 + @"(?<Name>" + variableExpression + ")" + delim2;
		}

		/// <summary>
		/// expand a template using data in data table
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="template"></param>
		/// <returns></returns>
		public IEnumerable<string> Expand(DataTable dt, string template)
		{
			if (dt == null)
				throw new ArgumentNullException("dt");

			if (!Regex.IsMatch(template, regexVar, RegexOptions.Singleline))
				throw new ArgumentException("no variables were defined in template");

			foreach (DataRow dr in dt.Rows)
			{
				var item = template;
				var expanded = Expand(dr, item);
				yield return expanded;
			}

			yield break;
		}

		private string Expand(DataRow dr, string item)
		{
			var mc = Regex.Matches(item, regexVar, RegexOptions.Singleline);
			if (mc.Count == 0)
				return item;

			var columns = dr.Table.Columns;
			var sb = new StringBuilder(item);

			// sort the list by Index desc, because we will replace the [var] with value.
			var mcs = from Match m in mc
					  orderby m.Index descending
					  select m;

			foreach (Match m in mcs)
			{
				var key = m.Groups["Name"].Value;
				if (!columns.Contains(key))
					continue;
				var value = dr[key].ToString();

				sb.Remove(m.Index, m.Length);
				sb.Insert(m.Index, dr[key].ToString());
			}

			return sb.ToString();
		}
	}
}
