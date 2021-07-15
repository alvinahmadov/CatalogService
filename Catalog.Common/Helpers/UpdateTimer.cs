using System;
using System.Diagnostics;
using System.Threading;

namespace Catalog.Common
{
	public static class UpdateTimer
	{
		/// <summary>
		/// As minutes (ex. 30, 45)
		/// </summary>
		public static Int32 DueTime
		{
			get => _dueTime;
			set
			{
				if (value != 0)
					if (_dueTime != value)
						_dueTime = value;
			}
		}

		public static Int32 Period
		{
			get => _period;
			set
			{
				if (value != 0)
					if (_period != value)
						_period = value;
			}
		}

		public static Timer Timer { get => timer; set { if (timer != value) timer = value; } }

		public static bool IsUpdating { get; set; } = false;

		public static bool IsEnabled { get => Timer != null; }

		public static Action Callback
		{
			get => updateCallback;
			set
			{
				if (updateCallback != value && value != null)
					updateCallback = value;
			}
		}

		public static void Update(Object state)
		{
			if (IsUpdating)
				return;

			IsUpdating = true;

			updateCallback?.Invoke();

			IsUpdating = false;
		}

		public static void Start()
		{
			if (Timer == null)
				if (DueTime != 0 || Period != 0)
				{
					var dueTime = ConvertMinutesToMilliseconds(DueTime);
					var period = ConvertMinutesToMilliseconds(Period);
					Timer = new Timer(new TimerCallback(Update), null, DueTime, Period);
				}
		}

		public static void Change(Int32 interval)
		{
			Change(interval, interval);
		}

		public static void Change(Int32 dueTime, Int32 period)
		{
			if (DueTime != 0 || Period != 0)
			{
				DueTime = dueTime;
				Period = period;

				var dueTime_ = ConvertMinutesToMilliseconds(DueTime);
				var period_ = ConvertMinutesToMilliseconds(Period);

				Timer?.Change(dueTime_, period_);
			}
		}

		public static void Stop()
		{
			Timer?.Dispose();
			Timer = null;
		}

		private static Int64 ConvertMinutesToMilliseconds(int minutes)
		{
			return (Int64)TimeSpan.FromMinutes(minutes).TotalMilliseconds;
		}

		private static Int64 ConvertSecondsToMilliseconds(int seconds)
		{
			return (Int64)TimeSpan.FromSeconds(seconds).TotalMilliseconds;
		}

		private static Action updateCallback;

		private static Timer timer = null;

		private static Int32 _dueTime;

		private static Int32 _period;
	}
}
