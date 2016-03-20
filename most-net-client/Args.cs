using System;

namespace Most.Client
{
	public class Args
	{
		public Args ()
		{
			//
		}

		public static void Check (bool expr, string message) {
			Args.NotNull(expr,"Expression");
			if (!expr) {
				throw new Exception(message);
			}
		}

		public static void NotNull (object arg, string name) {
			if (arg == null) {
				throw new ArgumentNullException(name + " may not be null");
			}
		}

		public static void NotEmpty (string arg, string name) {
			Args.NotNull(arg,name);
			if (arg.Length==0) {
				throw new ArgumentException(name + " may not be empty");
			}
		}

		public static void NotNegative (int arg, string name) {
			if (arg<0) {
				throw new ArgumentException(name + " may not be negative");
			}
		}

		public static void NotNegative (float arg, string name) {
			if (arg<0) {
				throw new ArgumentException(name + " may not be negative");
			}
		}

		public static void NotNegative (double arg, string name) {
			if (arg<0) {
				throw new ArgumentException(name + " may not be negative");
			}
		}

	}
}

