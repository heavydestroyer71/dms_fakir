using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FokirDMS
{
	public static class ExtensionMethods
	{
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}
		public static bool IsNotNullOrEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}
		public static int SafeGetLength(this string valueOrNull)
		{
			return (valueOrNull ?? string.Empty).Length;
		}
		public static bool IsZero(this Int64 val)
		{
			return val.Equals(0);
		}
		public static bool IsNotZero(this Int64 val)
		{
			return !val.Equals(0);
		}
		public static bool IsZero(this decimal val)
		{
			return val.Equals(decimal.Zero);
		}
		public static bool IsNotZero(this decimal val)
		{
			return !val.Equals(decimal.Zero);
		}
		public static bool IsZero(this Double val)
		{
			return val.Equals(0);
		}
		public static bool IsNotZero(this Double val)
		{
			return !val.Equals(0);
		}
		public static bool IsZero(this int val)
		{
			return val.Equals(0);
		}
		public static bool IsNotZero(this int val)
		{
			return !val.Equals(0);
		}
		public static bool IsZero(this short val)
		{
			return val.Equals(0);
		}
		public static bool IsNotZero(this short val)
		{
			return !val.Equals(0);
		}
		public static bool IsTrue(this bool val)
		{
			return val;
		}
		public static bool IsFalse(this bool val)
		{
			return !val;
		}
		public static bool IsNull(this object obj)
		{
			return obj == null;
		}
		public static bool IsNotNull(this object obj)
		{
			return obj != null;
		}
		public static bool IsNullOrDbNull(this object obj)
		{
			return obj == null || obj == DBNull.Value;
		}
		public static bool IsMinValue(this DateTime obj)
		{
			return obj == DateTime.MinValue;
		}
		public static bool IsNotMinValue(this DateTime obj)
		{
			return obj != DateTime.MinValue;
		}
		public static bool IsNullOrMinValue(this DateTime? obj)
		{
			return obj == null || obj == DateTime.MinValue;
		}
		public static int ToInt(this object input, bool throwException = false)
		{
			int result;
			var valid = int.TryParse(input.ToString(), out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as int");
			return result;
		}

		public static decimal ToDecimal(this object input, bool throwException = false)
		{
			decimal result;
			var valid = decimal.TryParse(input.ToString(), out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as decimal");
			return result;
		}

		public static int ToInt(this string input, bool throwException = false)
		{
			int result;
			var valid = int.TryParse(input, out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as int");
			return result;
		}
		public static long ToLong(this string input, bool throwException = false)
		{
			long result;
			var valid = long.TryParse(input, out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as long");
			return result;
		}
		public static Double ToDouble(this string input, bool throwException = false)
		{
			Double result;
			var valid = Double.TryParse(input, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as Double");
			return result;
		}
		public static decimal ToDecimal(this string input, bool throwException = false)
		{
			decimal result;
			var valid = decimal.TryParse(input, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." }, out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as Decimal");
			return result;
		}
		public static bool ToBoolean(this string input, bool throwException = false)
		{
			bool result;
			var valid = bool.TryParse(input, out result);
			if (valid) return result;
			if (throwException) throw new FormatException($"'{input}' cannot be converted as boolean");
			return result;
		}
		public static DateTime? makeDateTimeNull(this DateTime? date)
		{
			if (date.HasValue && (date.Value.Year == 1970 || date.Value.Year == 70))
			{
				return null;
			}
			return date;
		}
		public static string makeStringDateTimeToNull(this string date)
		{
			if (date.Contains("1970") || date.Contains("70"))
			{
				return date = null;
			}
			return date;
		}
	}
}