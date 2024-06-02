using System.Runtime.Serialization;

namespace IpAnalyzer
{
	internal class IPAddres
	{
		public required string IP { get; set; }
		public DateTime Date { get; set; }
        public int AccessCount { get; set; }
        public  List<DateTime> AccessDate { get; set; } = new List<DateTime>();
        public bool IsChecked { get; set; }
    }
}
