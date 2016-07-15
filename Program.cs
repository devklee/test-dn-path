using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestDN
{
	class MainClass
	{
		private static Regex rgx = new Regex(@"cn=[A-Za-z0-9 ]+,(.+)", RegexOptions.Compiled);
		private static Random random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static void Main (string[] args)
		{
			/*string ss = String.Format ("cn={0},ou={1},ou={2},dc=anekdot,dc=ru", RandomString (15), RandomString (10), RandomString (8));
			Console.WriteLine (ss);
			Console.WriteLine (GetPathFromDN3(ss));
			return;*/

			Stopwatch sw = new Stopwatch();

			List<string> dnList = new List<string> ();
			for (int i = 0; i < 100000; i++) 
			{
				dnList.Add (String.Format("cn={0},ou={1},ou={2},dc=anekdot,dc=ru", RandomString(15), RandomString(10), RandomString(8)));
			}

			sw.Restart();
			foreach(string s in dnList)
			{
				GetPathFromDN (s);
			}
			sw.Stop();
			Console.WriteLine ("Elapsed proletarian: {0}", sw.Elapsed);
			sw.Restart();
			foreach(string s in dnList)
			{
				GetPathFromDN2 (s);
			}
			sw.Stop();
			Console.WriteLine ("Elapsed african: {0}", sw.Elapsed);

			sw.Restart();
			foreach(string s in dnList)
			{
				GetPathFromDN3 (s);
			}
			sw.Stop();
			Console.WriteLine ("Elapsed regexp: {0}", sw.Elapsed);
		}

		public static string GetPathFromDN3(string dn)
		{
			if (String.IsNullOrEmpty (dn)) 
			{
				return string.Empty;
			}
			/*Match result = rgx.Match (dn);
			if (result.Success) 
			{
				Console.WriteLine ("OK");
				return result.Value; 
			}*/		
			return rgx.Replace(dn, @"$1");
		}

		public static string GetPathFromDN2(string dn)
		{
			if (String.IsNullOrEmpty (dn)) 
			{
				return string.Empty;
			}

			if(!dn.StartsWith("cn=", StringComparison.OrdinalIgnoreCase))
			{
				return dn;
			}
			int pos = dn.IndexOf (',');
			if (pos < 0 || pos+1 >= dn.Length) 
			{
				return dn;
			}
			return dn.Substring(pos+1);
		}

		public static string GetPathFromDN(string dn)
		{
			if (String.IsNullOrEmpty (dn)) 
			{
				return string.Empty;
			}
			int len = dn.Length;
			//cn=*,[*]
			if (len < 5) 
			{
				return string.Empty;
			}
			var sb = new StringBuilder();
			char[] arr = dn.ToCharArray ();
			int i = 0;
			int step = 0;

			for (i = 0; i < len; i++) 
			{
				if (step == 0) 
				{
					if (arr [i] == 'c' || arr [i] == 'C') 
					{
						step = 1;
					} 
					else 
					{
						break;
					}
				}
				else if (step == 1) 
				{
					if (arr [i] == 'n' || arr [i] == 'N') 
					{
						step = 2;
					} 
					else 
					{
						break;
					}
				}
				else if (step == 2) 
				{
					if (arr [i] == '=') 
					{
						step = 3;
					} 
					else 
					{
						break;
					}
				}
				else if (step == 3) 
				{
					if (arr [i] == ',') 
					{
						step = 4;
					} 
				}
				else if (step == 4) 
				{
					sb.Append (arr [i]);
				}
			}
			return sb.ToString ();
		}
	}
}
