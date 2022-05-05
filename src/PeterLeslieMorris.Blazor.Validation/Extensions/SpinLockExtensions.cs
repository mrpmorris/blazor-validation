using System;
using System.Threading;
using System.Threading.Tasks;

namespace PeterLeslieMorris.Blazor.Validation.Extensions
{
	public static class SpinLockExtensions
	{
		public static void ExecuteLocked(this SpinLock instance, Action action)
		{
			if (action is null)
				throw new ArgumentNullException(nameof(action));

			bool hasLock = false;
			while (!hasLock)
				instance.TryEnter(ref hasLock);
			try
			{
				action();
			}
			finally
			{
				if (hasLock)
					instance.Exit();
			}
		}

		public static async Task ExecuteLockedAsync(this SpinLock instance, Func<Task> action)
		{
			if (action is null)
				throw new ArgumentNullException(nameof(action));

			bool hasLock = false;
			while (!hasLock)
				instance.TryEnter(ref hasLock);
			try
			{
				await action();
			}
			finally
			{
				if (hasLock)
					instance.Exit();
			}
		}
	}
}
