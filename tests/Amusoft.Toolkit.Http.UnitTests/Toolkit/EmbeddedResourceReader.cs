using System;
using System.IO;
using System.Reflection;
using NLog;

namespace Amusoft.Toolkit.Http.UnitTests.Toolkit
{
	public class EmbeddedResourceReader
	{
		private static readonly Logger Log = LogManager.GetLogger(nameof(EmbeddedResourceReader));

		private readonly Assembly _assembly;

		public EmbeddedResourceReader(Assembly assembly)
		{
			_assembly = assembly;
		}

		public string GetContent(string accessPath)
		{
			try
			{
				using var reader = GetStream(accessPath);
				using var streamReader = new StreamReader(reader);
				return streamReader.ReadToEnd();
			}
			catch (Exception e)
			{
				Log.Debug("available names: {@Values}", _assembly.GetManifestResourceNames());
				throw new Exception($"Failed to get content for accessPath {accessPath}", e);
			}
		}

		private Stream GetStream(string accessPath)
		{
			var fullPath = _assembly.GetName().Name + "." + accessPath;
			return _assembly.GetManifestResourceStream(fullPath);
		}
	}
}