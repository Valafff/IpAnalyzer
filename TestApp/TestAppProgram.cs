using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TestApp
{
	internal class TestAppProgram
	{

		static void Main(string[] args)
		{
			////конвертация из десятичной в двоичную систему
			//int testNumber = 100;
			//Console.WriteLine(Convert.ToString(testNumber, 2));
			////конвертация из двоичной в десятичную систему
			//string teststring = "00001000";
			//Console.WriteLine(Convert.ToInt32(teststring, 2));

			////Регулярные выражения
			//Console.Write("Введите IP-адрес: ");
			//string input = Console.ReadLine();

			//// Паттерн для поиска IP-адреса \b - для разграничения IP адреса от остального возможного текста
			////string pattern = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
			////https://stackoverflow.com/questions/5284147/validating-ipv4-addresses-with-regexp
			//string pattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";
			//Regex ipRegex = new Regex(pattern);

			//MatchCollection matches = ipRegex.Matches(input);

			//if (matches.Count > 0)
			//{
			//	Console.WriteLine($"Найден IP-адрес: {matches[0].Value}");
			//}
			//else
			//{
			//	Console.WriteLine("IP-адрес не найден.");
			//}

			////разбиение IP адреса на октеты
			//string[] octets = input.Split('.');
			//foreach (var item in octets)
			//{
			//             Console.WriteLine(item);
			//         }
			////Представление в двоичном виде
			//foreach (var item in octets)
			//{
			//	Console.WriteLine(Convert.ToString(Convert.ToInt32(item), 2));
			//}

			//Конъюнкция!!!
			//Console.WriteLine($"{255 & 192}");
			int b = 0b01111111;
			int bb = 0b_11111111;
			Console.WriteLine($"{b&b}");
			int _bitMask = 24;

			byte[] maskOctets = BitConverter.GetBytes(_bitMask);

			foreach (var item in maskOctets)
			{
                Console.WriteLine(item);
            }

		}
	}
}
