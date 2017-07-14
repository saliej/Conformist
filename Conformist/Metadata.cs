using System;
using System.Globalization;

namespace Conformist
{
    internal class Metadata
    {
		public static DateTime BuildDate => DateTime.Parse("07/14/2017 12:17:51", CultureInfo.CurrentCulture);

		public static int BuildNumber => 738;
    }
}
