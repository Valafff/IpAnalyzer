using CommandLine;

namespace IpAnalyzer
{
	internal class Options
	{
		[Option('l', "file-log", Required = true, HelpText = "путь к файлу с логами")]
		public required string FLog { get; set; }
		[Option('o', "file-output", Required = true, HelpText = "путь к файлу с результатом")]
		public required string FOut { get; set; }
		[Option('a', "address-start", Required = false, HelpText = "нижняя граница диапазона адресов, необязательный параметр, по умолчанию обрабатываются все адреса")]
		public string? AdrrStart { get; set; }
		[Option('m', "address-mask", Required = false, HelpText = "маска подсети, задающая верхнюю границу диапазона десятичное число. Необязательный параметр. В случае, если он не указан, обрабатываются все адреса, начиная с нижней границы диапазона. Параметр нельзя использовать, если не задан address-start")]
		public string? AdrrMask { get; set; }
		[Option('s', "time-start", Required = true, HelpText = "нижняя граница временного интервала")]
		public required string TimeStart { get; set; }
		[Option('e', "time-end", Required = true, HelpText = "верхняя граница временного интервала")]
		public required string TimeEnd { get; set; }


	}
}
