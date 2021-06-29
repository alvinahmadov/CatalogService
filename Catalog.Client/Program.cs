using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Telerik.WinControls;

namespace Catalog.Client
{
	static class Program
	{
		public static bool CreateNoWindow { get; set; }
		public static bool UseShellExecute { get; set; }
		public static bool RedirectStandardOutput { get; set; }

		public readonly static string[] OPTIONS = new String[] {
			"debug",
			"message"
		};

		public static string RootDir { get; set; }

		public readonly static string IIS_BIN_DIR = Environment.GetEnvironmentVariable("IIS_BIN");

		public readonly static string HOME_DIR = Environment.GetEnvironmentVariable("USERPROFILE");

		public static Process process = null;

		public static SplashForm splashform = null;

		public static Form form = null;

		static Program()
		{
			CreateNoWindow = !IsAssemblyDebugBuild();
			UseShellExecute = false;
			RedirectStandardOutput = false;
			RootDir = Path.GetFullPath(".").Replace("Bin", "");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);
			Environment.SetEnvironmentVariable("CATALOG_SERVICE_HOME", Path.Combine(RootDir, "Catalog.Service"), EnvironmentVariableTarget.Process);
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(String[] args)
		{
			try
			{
				String message = null;
				foreach (var arg in args)
				{
					if (arg == OPTIONS[0])
						CreateNoWindow = false;
					else if (arg == OPTIONS[1])
						message = arg;
				}

				splashform = new SplashForm(message);
				Application.Run(splashform);

				form = new MainForm();
				Application.Run(form);

			}
			catch (Exception e)
			{
				RadMessageBox.Show($"{e.Message}\n{e.StackTrace}");
			}
		}

		static void RunService(bool useApphost = false, int port = 60585)
		{
			ProcessStartInfo startInfo;
			try
			{
				startInfo = GetStartInfo(port, useApphost);
				process = Process.Start(startInfo);
			}
			catch (Exception)
			{
				startInfo = GetStartInfo(port, false);
				process = Process.Start(startInfo);
			}

		}

		static ProcessStartInfo GetStartInfo(int port = 60585, bool useApphost = false)
		{
			var ARCH = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");

			if (ARCH.Equals("AMD64"))
				ARCH = "";
			else ARCH = " (x86)";

			var serviceDir = Environment.GetEnvironmentVariable("CATALOG_SERVICE_HOME");
			var iisExpressExecutable = $"\"C:\\Program Files{ARCH}\\IIS Express\\iisexpress.exe\"";
			var args = new System.Text.StringBuilder();

			if (!useApphost || IsAssemblyDebugBuild())
			{
				args.Append("/path:").Append($"\"{serviceDir}\"")
					.Append(" ")
					.Append("/port:").Append(port.ToString());
			}
			else
				args.Append("/config:").Append($"\"{serviceDir}\\applicationhost.config\"");

			var startInfo = new ProcessStartInfo
			{
				FileName = iisExpressExecutable,
				Arguments = args.ToString(),
				RedirectStandardOutput = RedirectStandardOutput,
				UseShellExecute = UseShellExecute,
				CreateNoWindow = CreateNoWindow
			};

			return startInfo;
		}

		public static bool IsAssemblyDebugBuild() 
		{
			return IsAssemblyDebugBuild(Assembly.GetExecutingAssembly());
		}

		public static bool IsAssemblyDebugBuild(Assembly assembly)
		{
			return assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>()
				.Any(da => da.IsJITTrackingEnabled);
		}
	}
}
