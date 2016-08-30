using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;
using System.Text;

namespace JsonToCSV
{
	class CommandLineOptions
	{
		[OptionList('h', Required = false, Separator = ',', HelpText = "Specifies the header names, comma-separated.")]
		public IList<string> Headers { get; set; }

		[OptionList('c', Required = true, Separator = ',', HelpText = "Specifies the columns to export, as JSON paths, comma-separated.")]
		public IList<string> Columns { get; set; }

		[Option('r', Required = false, DefaultValue = "value", HelpText = "Specifies the JSON path to the root element whose children to export.")]
		public string Root { get; set; }

		[Option('u', Required = true, HelpText = "Specifies the url to download the JSON from.")]
		public string Url { get; set; }

		[Option('d', Required = false, DefaultValue = '|', HelpText = "Specifies the delimiter used when writing the CSV data.")]
		public char Delimiter { get; set; }

		[HelpOption(HelpText = "Display this help screen.")]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
