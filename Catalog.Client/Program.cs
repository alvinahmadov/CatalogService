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
		public readonly static string[] OPTIONS = new String[] {
			"debug",
			"message"
		};

		public static string RootDir { get; set; }

		public static Process process = null;

		public static SplashForm splashform = null;

		public static Form form = null;

		static Program()
		{
			RootDir = Path.GetFullPath(".").Replace("Bin", "");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);
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
					if (arg == OPTIONS[1])
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
