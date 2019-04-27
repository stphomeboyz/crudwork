using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace crudwork.UnitTests
{
	public static class ResourceManager
	{
		public static Stream ToStream(string resourceKey)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceKey);
		}

		public static string ToStringValue(string resourceKey)
		{
			using (var s = ResourceManager.ToStream(resourceKey))
			{
				return new StreamReader(s).ReadToEnd();
			}
		}

		public static void ExportToFile(string resourceKey)
		{
			File.WriteAllText(resourceKey, ToStringValue(resourceKey));

			if (!File.Exists(resourceKey))
				throw new ArgumentException(string.Format("cannot create file {0} from resource {1}", resourceKey, resourceKey));
		}
	}
}
