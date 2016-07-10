using System;
using System.Text;
using System.Diagnostics;

namespace TestDN
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			string s = GetPathFromDN("cn=afrikanec,ou=gb,dc=anekdot,dc=ru");
			sw.Stop();
			Console.WriteLine ("Path: {0}, Elapsed: {1}", s, sw.Elapsed);
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
