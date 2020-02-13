using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Helpers
{
    public static class ApplicationLogging
    {
		private static ILoggerFactory _Factory = null;

		public static void ConfigureLogger(ILoggerFactory factory)
		{
			factory.AddDebug(LogLevel.Debug);
			factory.AddConsole();
		}

		public static ILoggerFactory LoggerFactory
		{
			get
			{
				if (_Factory == null)
				{
					_Factory = new LoggerFactory();
					ConfigureLogger(_Factory);
				}
				return _Factory;
			}
			set { _Factory = value; }
		}
	}
}
