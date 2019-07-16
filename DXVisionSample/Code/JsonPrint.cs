using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DXVisionSample.Code
{
	public static class JsonPrint
	{		
		public static string Prettify(string json, int indentLength = 3)
		{
			if (string.IsNullOrEmpty(json))
				return string.Empty;

			json = json.Replace(Environment.NewLine, "").Replace("\t", "");

			StringBuilder sb = new StringBuilder();
			bool quote = false;
			bool ignore = false;
			int offset = 0;
			

			foreach (char ch in json)
			{
				switch (ch)
				{
					case '"':
						if (!ignore) quote = !quote;
						break;
					case '\'':
						if (quote) ignore = !ignore;
						break;
				}

				if (quote)
					sb.Append(ch);
				else
				{
					switch (ch)
					{
						case '{':
						case '[':
							sb.Append(ch);
							sb.Append(Environment.NewLine);
							sb.Append(new string(' ', ++offset * indentLength));
							break;
						case '}':
						case ']':
							sb.Append(Environment.NewLine);
							sb.Append(new string(' ', --offset * indentLength));
							sb.Append(ch);
							break;
						case ',':
							sb.Append(ch);
							sb.Append(Environment.NewLine);
							sb.Append(new string(' ', offset * indentLength));
							break;
						case ':':
							sb.Append(ch);
							sb.Append(' ');
							break;
						default:
							if (ch != ' ') sb.Append(ch);
							break;
					}
				}
			}

			return sb.ToString().Trim();
		}
	}
}