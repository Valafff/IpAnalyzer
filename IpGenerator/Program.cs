using IpGenerator;
using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

const int MAXLIMIT = 10000;
const int OCTET = 4;
const int BITNUMBER = 32;
const int DELIMETR = 3;

int method;
Random random = new Random();
List<IpAddress> Iplist = new List<IpAddress>();

Console.WriteLine("Укажите способ генерации IP-адресов и времени входа: 1 - автоматический, 2 - по маске сети");
try
{
	method = Convert.ToInt32(Console.ReadLine());

	switch (method)
	{
		case 1:
			AutoGenerate();
			break;
		case 2:
			ManualGeneration();
			break;

		default:
			break;
	}
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}

void AutoGenerate()
{
	Iplist.Clear();
	string tempIp;
	int limit = random.Next(MAXLIMIT);
	for (int i = 0; i < limit; i++)
	{
		tempIp = "";
		int counter = 0;
		do
		{
			int octet = random.Next(1, byte.MaxValue);
			tempIp += octet.ToString();
			if (counter < OCTET - 1)
			{
				tempIp += ".";
			}
			counter++;
		} while (counter < OCTET);

		IpAddress ip = new IpAddress(random, tempIp);
		Iplist.Add(ip);
	}
	using (FileStream fs = new FileStream("IpList.json", FileMode.OpenOrCreate))
	{
		JsonSerializer.Serialize(fs, Iplist, new JsonSerializerOptions { WriteIndented = true });
		Console.WriteLine($"Файл создан по пути: {Directory.GetCurrentDirectory()}\\IpList.json");
	}
}

void ManualGeneration()
{
	string? ipInput;
	int bitMask;
	string ipPattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";
	Regex IpRegex = new Regex(ipPattern);
	string maskPattern = @"^/\d{1,2}$";
	Regex MaskRegex = new Regex(maskPattern);

	Console.WriteLine("Укажите адрес сети или адрес узла сети:");
	do
	{
		ipInput = Console.ReadLine();
		if (ipInput != null)
		{
			MatchCollection matches = IpRegex.Matches(ipInput);

			if (matches.Count > 0)
			{
				break;
			}
			else
			{
				Console.WriteLine("IP-адрес задан неверно.");
			}
		}
	} while (true);

	Console.WriteLine("Укажите маску сети (/nn или nnn.nnn.nnn.nnn) - генерация клиентов будет происходить в пределах маски сети:");
	do
	{
		string? maskInput = Console.ReadLine();
		string testMask = "";

		if (maskInput != null)
		{
			MatchCollection matches = IpRegex.Matches(maskInput);
			MatchCollection matchesMask = MaskRegex.Matches(maskInput);

			if (matches.Count > 0)
			{
				string[] octetsStr = maskInput.Split('.');
				for (int i = 0; i < OCTET; i++)
				{
					var temp = Convert.ToByte(octetsStr[i]);
					testMask += Convert.ToString(temp, 2);
				}
				//Проверка правильности написания маски
				if (testMask.Contains("01"))
				{
					Console.WriteLine("Маска задана неверно.");
					continue;
				}
				bitMask = testMask.Count(c => c == '1');
				break;
			}
			else if (matchesMask.Count > 0)
			{
				maskInput = maskInput.Remove(0, 1);
				bitMask = Convert.ToInt32(maskInput);
				break;
			}
			else
			{
				Console.WriteLine("Маска задана неверно.");
			}
		}

	} while (true);

	string[] GetNet(int _bitMask)
	{
		StringBuilder temp = new StringBuilder();
		int octCounter = 0;

		for (int i = 0; i < BITNUMBER + DELIMETR; i++)
		{

			if (octCounter < BITNUMBER / OCTET && _bitMask > 0)
			{
				temp.Append("1");
				_bitMask--;
				octCounter++;
			}
			else if (octCounter >= BITNUMBER / OCTET && _bitMask > 0)
			{
				octCounter = 0;
				temp.Append(".");
			}
			else if (octCounter < BITNUMBER / OCTET && _bitMask <= 0)
			{
				temp.Append("0");
				octCounter++;
			}
			else
			{
				octCounter = 0;
				temp.Append(".");
			}
		}
		return temp.ToString().Split(".");
	}

	string[] MaskOct = GetNet(bitMask);
	string[] Octets = ipInput.Split('.');

	string GetRandIpByMask(string[] _mascOct, string[] _octets, Random _rnd)
	{
		string Ip = "";


		for (int i = 0; i < OCTET; i++)
		{
			int range = byte.MaxValue - Convert.ToInt32(_mascOct[i], 2);

			int temp = (Convert.ToInt32(_octets[i]) & Convert.ToInt32(_mascOct[i], 2));
			if (Convert.ToInt32(_mascOct[i], 2) == byte.MaxValue )
			{
				Ip += temp.ToString();
			}
			else
			{
				Ip += _rnd.Next(1, range + 1);
			}
			if (i < OCTET - 1)
			{
				Ip += ".";
			}
		}
		return Ip;
	}

	Iplist.Clear();
	for (int i = 0; i < MAXLIMIT; i++)
	{
		IpAddress ip = new IpAddress(random, GetRandIpByMask(MaskOct, Octets, random));
		Iplist.Add(ip);
	}
	using (FileStream fs = new FileStream("IpList.json", FileMode.Create))
	{
		JsonSerializer.Serialize(fs, Iplist, new JsonSerializerOptions { WriteIndented = true });
		Console.WriteLine($"Файл создан по пути: {Directory.GetCurrentDirectory()}\\IpList.json");
	}
}
