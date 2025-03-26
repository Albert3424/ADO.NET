using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalBase
{
	class Program
	{
		static void Main(string[] args)
		{
			//Connector.Select("*", "Disciplines");
			Console.WriteLine(Connector.GetDisciplineID("Объектно-ориентированное программирование на языке C++"));
			Console.WriteLine(Connector.GetTeacherID("Глазунов"));
			Console.WriteLine(Connector.Count("Students"));

			Console.ReadLine();
		}
	}
}
