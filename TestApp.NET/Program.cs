using System.Reflection.Emit;
using System.Text.Json;
using TestApp.NET;

const int MAXLIMIT = 10000;

int method;
Console.WriteLine("Укажите способ генерации IP-адресов и времени входа: 1 - автоматический, 2 - с параметрами");
try
{
	method = Convert.ToInt32(Console.ReadLine());
	switch (method)
	{
		case 1:
			{

				List<IpGenerator> Iplist = new List<IpGenerator>();
				Random random = new Random();
				int limit = random.Next(MAXLIMIT);
				for (int i = 0; i < limit; i++)
				{
					IpGenerator ip = new IpGenerator(random);
					Iplist.Add(ip);
					//Console.WriteLine(ip.IP);
				}
				using (FileStream fs = new FileStream("IpList.json", FileMode.OpenOrCreate))
				{
					JsonSerializer.Serialize(fs, Iplist);
                    Console.WriteLine($"Файл создан по пути: {Directory.GetCurrentDirectory()}\\IpList.json");
                }
				break;
			}
		case 2:
			Console.WriteLine("222222");
			break;

		default:
			break;
	}
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
