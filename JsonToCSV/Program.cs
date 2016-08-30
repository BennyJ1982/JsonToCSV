using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JsonToCSV
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new CommandLineOptions();
			if (!CommandLine.Parser.Default.ParseArguments(args, options))
			{
				return;
			}

			var data = DownloadData(options.Url).Result;
			var records = ParseJson(data, options.Root);
			ExportToCSV(options, records);

			Console.ReadKey();
		}

		private static async Task<string> DownloadData(string uri)
		{
			var request = WebRequest.CreateHttp(uri);
			var response = await request.GetResponseAsync();

			using (var responseStream = response.GetResponseStream())
			{
				using (var reader = new StreamReader(responseStream))
				{
					return await reader.ReadToEndAsync();
				}
			}
		}

		private static JArray ParseJson(string json, string pathToRoot)
		{
			var jsonObject = JObject.Parse(json);
			return (JArray)jsonObject.SelectToken(pathToRoot);
		}

		private static void ExportToCSV(CommandLineOptions options, JArray records)
		{
			if (options.Headers.Any())
			{
				WriteHeaders(options.Headers, options.Delimiter);
			}

			foreach (var record in records)
			{
				WriteRecord(record, options.Columns, options.Delimiter);
			}
		}

		private static void WriteHeaders(IEnumerable<string> headers, char delimiter)
		{
			Console.WriteLine(headers.Aggregate((line, header) => line + delimiter + header));
		}

		private static void WriteRecord(JToken record, ICollection<string> columns, char delimiter)
		{
			int columnsWritten = 0;
			foreach (var column in columns)
			{
				var columnToken = record.SelectToken(column);
				if (columnToken != null)
				{
					Console.Write(columnToken.ToString());
				}

				if (++columnsWritten < columns.Count)
				{
					Console.Write(delimiter);
				}
			}

			Console.WriteLine();
		}
	}
}
