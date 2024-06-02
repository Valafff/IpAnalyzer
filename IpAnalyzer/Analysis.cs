using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IpAnalyzer
{
	internal class Analysis
	{
		const int OCTET = 4;
		const int BITNUMBER = 32;
		const int DELIMETR = 3;
		public required string FileLogPath { get; set; }
		public required string FileOutputPath { get; set; }
		public string? AddressStartSearch { get; set; }
		public string? Mask { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }


		List<IPAddres> InputIpList = new List<IPAddres>();
		List<IPAddres> OutputIPlist = new List<IPAddres>();
		byte[] StartIpRange = new byte[OCTET];
		public void Analyze()
		{
			StartIpRange = GetOctetLimits(AddressStartSearch);
			List<IPAddres>? temp = new List<IPAddres>();
			temp = GetInputIpList();
			if (temp != null)
			{
				InputIpList = temp;
			}
			if (InputIpList != null)
			{
				Search();
				PrintOutputFile();
			}
			else
			{
				Console.WriteLine("Исходный файл не найден.");
			}
		}

		List<IPAddres>? GetInputIpList()
		{
			using (FileStream reader = new FileStream(FileLogPath, FileMode.Open, FileAccess.Read))
			{
				return JsonSerializer.Deserialize<List<IPAddres>>(reader);
			}
		}

		bool IsIpValid(IPAddres _ip)
		{
			byte[] octets = new byte[OCTET];
			string[] tempoct = _ip.IP.Split(".");
			//Перевод string -> byte
			for (var i = 0; i < OCTET; i++)
			{
				if (byte.TryParse(tempoct[i], out byte res))
				{
					octets[i] = res;
				}
				else
				{
					Console.WriteLine($"Ошибка чтения IP - адреса {_ip.IP}");
					return false;
				}
			}
			//Проверка на вхождение ip в диапазон
			for (int i = 0; i < OCTET; i++)
			{
				if (octets[i] < StartIpRange[i])
				{
					return false;
				}
			}
			return true;
		}

		bool IsIpValid(IPAddres _ip, string[] _mask)
		{
            byte[] octets = new byte[OCTET];
			string[] tempoct = _ip.IP.Split(".");

			//Перевод string -> byte
			for (var i = 0; i < OCTET; i++)
			{
				if (byte.TryParse(tempoct[i], out byte res))
				{
					octets[i] = res;
				}
				else
				{
					Console.WriteLine($"Ошибка чтения IP - адреса {_ip.IP}");
					return false;
				}
			}

			//Проверка на вхождение ip в диапазон и маску
			for (int i = 0; i < OCTET; i++)
			{

				int range = Convert.ToInt32(_mask[i], 2);
				
                if (octets[i] < StartIpRange[i] && range > 0)
				{
					return false;
				}
				else if (octets[i] >= StartIpRange[i] && range > 0)
				{
					if (octets[i] < range && range != byte.MaxValue )
					{
						return false;
					}
				}
			}
			return true;
		}

		void Search()
		{
			List<IPAddres>? tempList = new List<IPAddres>();

			if (InputIpList != null && Mask == null)
			{
				foreach (var item in InputIpList)
				{
                    //Проверка аременного диапазона
                    if (item.Date >= StartTime && item.Date <= EndTime && !item.IsChecked && IsIpValid(item))
					{
						item.AccessCount++;
						item.IsChecked = true;
						item.AccessDate.Add(item.Date);
						for (int i = 0; i < InputIpList.Count; i++)
						{
							//Проверка совпадений и удаление таковых
							if (item.IP == InputIpList[i].IP && !InputIpList[i].IsChecked && InputIpList[i].Date >= StartTime && InputIpList[i].Date <= EndTime)
							{
								item.AccessCount++;
								item.AccessDate.Add(InputIpList[i].Date);
								InputIpList[i].IsChecked = true;
							}
						}
						tempList.Add(item);
					}
				}
				OutputIPlist = tempList;
			}
			else if (InputIpList != null && Mask != null)
			{
				string[] MaskOctets = new string[OCTET];
				Mask = Mask.Remove(0, 1);
				MaskOctets = GetNet(Convert.ToInt32(Mask)); //ToDo Привести маску в корректное состояние

				foreach (var item in InputIpList)
				{
					//Проверка аременного диапазона
					if (item.Date >= StartTime && item.Date <= EndTime && !item.IsChecked && IsIpValid(item, MaskOctets))
					{
						item.AccessCount++;
						item.IsChecked = true;
						item.AccessDate.Add(item.Date);
						for (int i = 0; i < InputIpList.Count; i++)
						{
							//Проверка совпадений и удаление таковых
							if (item.IP == InputIpList[i].IP && !InputIpList[i].IsChecked && InputIpList[i].Date >= StartTime && InputIpList[i].Date <= EndTime)
							{
								item.AccessCount++;
								item.AccessDate.Add(InputIpList[i].Date);
								InputIpList[i].IsChecked = true;
							}
						}
						tempList.Add(item);
					}
				}
				OutputIPlist = tempList;
			}
			else
			{
				Console.WriteLine("список исходных записпей пуст");
			}
			return;
		}

		void PrintOutputFile()
		{
			using (StreamWriter sw = new StreamWriter(FileOutputPath, false, Encoding.UTF8))
			{
				foreach (var item in OutputIPlist)
				{
					for (int i = 0; i < item.AccessDate.Count; i++)
					{
						sw.WriteLine($"IP {item.IP} : Количество подключений {item.AccessCount} : Время {item.AccessDate[i]}");
					}
				}
				Console.WriteLine($"Файл записан по пути {FileOutputPath}");
			}
		}

		byte[] GetOctetLimits(string? _start)
		{
			try
			{
				if (_start != null)
				{

					byte[] temp = new byte[OCTET];
					string[] tempstr = _start.Split(".");
					for (var i = 0; i < OCTET; i++)
					{
						if (byte.TryParse(tempstr[i], out byte res))
						{
							temp[i] = res;
						}
						else
						{
							temp[i] = 0;
							Console.WriteLine($"Ошибка чтения IP {tempstr[i]} - адреса присвоена нижняя граница диапазона");
						}
					}
					return temp;
				}
				else
				{
					byte[] temp = { 0, 0, 0, 0 };
					return temp;
				}
			}
			catch
			{
				Console.WriteLine("Ошибка формата диапазона Ip- адреса. Пример 192.168.0.100");
				byte[] temp = { 0, 0, 0, 0 };
				return temp;
			}
		}

		string[] GetNet(int? _bitMask)
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
	}
}
