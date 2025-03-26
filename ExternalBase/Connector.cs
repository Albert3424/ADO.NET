using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;

namespace ExternalBase
{
	static internal class Connector
	{
		static readonly int PADDING = 16;
		static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString;
		static SqlConnection connection;
		static Connector()
		{
			Console.WriteLine(CONNECTION_STRING);
			connection = new SqlConnection(CONNECTION_STRING);
		}
		public static void Select(string fields, string tables, string condition = "")
		{
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();
			if(reader.HasRows)
			{
				for(int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write(reader.GetName(i).PadRight(PADDING));
				}
			}
			Console.WriteLine();
			while(reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write(reader[i].ToString().PadRight(PADDING));
				}
				Console.WriteLine();
			}
			reader.Close();
			connection.Close();
			Console.ReadLine();
		}
		public static int GetID(string fields, string table, string condition)
		{
			string cmd = $"SELECT {fields} FROM {table} WHERE {condition}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();

			object result = command.ExecuteScalar();
			connection.Close();

			return Convert.ToInt32(result);
		}
		public static int GetDisciplineID(string disciplineName)
		{
			return GetID("discipline_id", "Disciplines", $"discipline_name=N'{disciplineName}'");
		}

		public static int GetTeacherID(string lastName)
		{
			return GetID("teacher_id", "Teachers", $"last_name=N'{lastName}'");
		}

		public static int Count(string table)
		{
			int count = 0;
			string cmd = $"SELECT COUNT(*) FROM {table}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();

			count = Convert.ToInt32(command.ExecuteScalar());
			connection.Close();

			return count;
		}
	}
}
