using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class FriendlyError : Exception
	{
		private FriendlyError(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public static FriendlyError Create(Exception inner)
		{
			return new FriendlyError(inner.Message, inner);
		}

		public void ThrowInner()
		{
			throw base.InnerException;
		}

		public static void Report(Logger logger, string? message, FriendlyError error, Exception? outer = null)
		{
			Report(logger, outer ?? error, message, error.Message);
		}

		public static void Report(Logger logger, string? message, AggregateException outer)
		{
			FriendlyError error = outer?.InnerException as FriendlyError;
			if (error != null)
			{
				Report(logger, message, error, outer);
			}
			else
			{
				Report(logger, outer, message, null);
			}
		}

		public static void Report(Logger logger, string? message, Exception? exception)
		{
			FriendlyError error = exception as FriendlyError;
			if (error != null)
			{
				Report(logger, message, error);
				return;
			}
			AggregateException outer = exception as AggregateException;
			if (outer != null)
			{
				Report(logger, message, outer);
			}
			else
			{
				Report(logger, exception, message, null);
			}
		}

		private static void Report(Logger logger, Exception? exception, string? message, string? errorMessage)
		{
			string errorMessage2 = errorMessage;
			Exception exception2 = exception;
			string message2 = message;
			if (message2 != null)
			{
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					string arg = errorMessage2 ?? (exception2 as AggregateException)?.InnerException.Message ?? exception2?.Message ?? Strings.ExceptionGeneric;
					ScreenNotification.ShowNotification(string.Format(Strings.ErrorFormat, message2, arg), (NotificationType)2, (Texture2D)null, 4);
				});
			}
			if (errorMessage2 == null)
			{
				logger.Error(exception2, message2);
			}
			else
			{
				logger.Warn(exception2, message2 ?? errorMessage2);
			}
		}
	}
}
