namespace IpGenerator
{
	internal class IpAddress
	{
		const int OCTET = 4;

		public string IP { get; set; }
		public DateTime Date { get; set; }

		public IpAddress(Random _random, string _ip)
		{
			Date = GetRandomDate(_random);
			IP = _ip;
		}
		DateTime GetRandomDate(Random rand, int _startYaer=2000, int _startMonth = 1, int _startDay = 1)
		{
			DateTime start = new DateTime(_startYaer, _startMonth, _startDay);
			int limit = (DateTime.Today - start).Days;
			DateTime temp = start.AddDays(rand.Next(0, limit));
			temp = temp.AddHours(rand.Next(0, 24));
			temp = temp.AddMinutes(rand.Next(0, 60));
			temp = temp.AddSeconds(rand.Next(0, 60));
			return temp;
		}

	}
}
