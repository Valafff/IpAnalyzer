﻿using CommandLine;
using IpAnalyzer;

static void Main(string[] args)
{
	string fileLogPath;
	string fileOutputPath;
	string? addressStartSearch;
	string? maskSearch = null;
	DateTime startTime; 
	DateTime endTime;

	Parser parser = new Parser(with => with.HelpWriter = null);

	ParserResult<Options> result = parser.ParseArguments<Options>(args);
	result.WithParsed(options =>
	{
		try
		{
			if (options.AdrrStart != null && options.AdrrMask == null)
			{
				addressStartSearch = options.AdrrStart;
			}
			else if (options.AdrrStart == null && options.AdrrMask != null)
			{
				Console.WriteLine("Ошибка при вводе аргументов: Параметр 'address-mask' нельзя использовать, если не задан address-start");
				return;
			}
			else
			{
				addressStartSearch = options.AdrrStart;
				maskSearch = options.AdrrMask;
			}
			fileLogPath = options.FLog;

			fileOutputPath = options.FOut;

			startTime = Convert.ToDateTime(options.TimeStart);
			endTime = Convert.ToDateTime(options.TimeEnd);
		}
		catch
		{
			Console.WriteLine("Ошибочный формата аргументов");
			return;
		}
		Analysis analysis = new Analysis() { FileLogPath = fileLogPath, FileOutputPath = fileOutputPath, AddressStartSearch = addressStartSearch, Mask = maskSearch, StartTime = startTime, EndTime = endTime };
		analysis.Analyze();
	});
	result.WithNotParsed(errors =>
	{
		Console.WriteLine("Ошибка при вводе аргументов:");
		foreach (var error in errors)
		{
			Console.WriteLine(error);
			return;
		}
	});


}
Main(args);