using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.NET
{
	public class IpGenerator
	{
		public string IP { get; set; }
		public DateTime Date { get; set; }


		public IpGenerator(Random random)
		{
			int counter = 0;
			do
			{
				int octet = random.Next(1, 255);
				IP += octet.ToString();
				if (counter < 3)
				{
					IP += ".";
				}
				counter++;
			} while (counter < 4);
		}
	}
}
