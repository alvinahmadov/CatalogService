using System;
using System.Diagnostics;
using System.Threading;

namespace Catalog.Common
{
	public class UpdateTimer
	{
		/// <summary>
		/// As minutes (ex. 30, 45)
		/// </summary>
		static Int64 DueTime { get; set; }

		static Int64 Period { get; set; }

		public UpdateTimer(Int32 interval, Action updateCallback)
		{
			DueTime = ConvertMinutesToMilliseconds(interval);
			Period = ConvertMinutesToMilliseconds(interval);
			this.updateCallback = updateCallback;
		}

		public UpdateTimer(Int32 dueTime, Int32 period, Action updateCallback)
		{
			DueTime = ConvertMinutesToMilliseconds(dueTime);
			Period = ConvertMinutesToMilliseconds(period);
			this.updateCallback = updateCallback;
		}


		public void Update(Object state)
		{
			updateCallback?.Invoke();
		}

		public void Start()
		{
			if (DueTime != 0 || Period != 0)
				this.timer = new Timer(new TimerCallback(Update), null, DueTime, Period);
		}

		public void Stop() 
		{
			updateCallback = null;
			this.timer?.Dispose();
		}

		private static Int64 ConvertMinutesToMilliseconds(int minutes)
		{
			return (Int64)TimeSpan.FromMinutes(minutes).TotalMilliseconds;
		}

		private static Int64 ConvertSecondsToMilliseconds(int seconds)
		{
			return (Int64)TimeSpan.FromSeconds(seconds).TotalMilliseconds;
		}

		private Action updateCallback;

		private Timer timer;
	}
}
