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
			int disciplineId = Connector.GetDisciplineId("discipline_id", "Disciplines", "JavaScript");

			Console.WriteLine($"Полученный идентификатор дисциплины: {disciplineId}");

			Console.ReadLine();
		}
	}
}
