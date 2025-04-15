﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Configuration;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


namespace Academy
{
	public partial class MainForm : Form
	{
		Connector connector;

		public Dictionary<string, int> d_directions;
		public Dictionary<string, int> d_groups;

		public Dictionary<ComboBox, List<ComboBox>> d_dependencies;

		DataGridView[] tables;
		Query[] queries = new Query[]
			{
				new Query
				(
					"last_name,first_name,middle_name,birth_date,group_name,direction_name",
					"Students JOIN Groups ON ([group]=group_id) JOIN Directions ON (direction=direction_id)"
					//"[group]=group_id AND direction=direction_id"
				),
				new Query
				(
					"group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name",
					"Groups,Directions",
					"direction=direction_id"
				),
				new Query
				(
					"direction_name,COUNT(DISTINCT group_id) AS N'Количество группов', COUNT(stud_id) AS N'Количество студентов'",
					"Students RIGHT JOIN Groups ON ([group]=group_id) RIGHT JOIN Directions ON (direction=direction_id)",
					"group_id!=0 OR stud_id!=0",
					"direction_name"
				),
				new Query
				(
						"*",
						"Disciplines"
				),
				new Query
				(
						"*",
						"Teachers"
				)


			};

		string[] status_messages = new string[]
			{
				$"Количество студентов: ",
				$"Количество групп: ",
				$"Количество направлений: ",
				$"Количество дисциплин: ",
				$"Количество преподавателей: "
			};


		public MainForm()
		{
			InitializeComponent();

			d_dependencies = new Dictionary<ComboBox, List<ComboBox>>()
			{
				{ cbStudentsDirection, new List<ComboBox>(){ cbStudentsGroup } }
			};

			tables = new DataGridView[]
			{
				dgvStudents,
				dgvGroups,
				dgvDirections,
				dgvDisciplines,
				dgvTeachers
			};

			connector = new Connector
				(
					ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString
				);

			dgvStudents.DataSource = connector.Select
						(
							"last_name,first_name,middle_name,birth_date,group_name,direction_name",
							"Students,Groups,Directions",
							"[group]=group_id AND direction=direction_id"
						);

			d_directions = connector.GetDictionary("*", "Directions");
			d_groups = connector.GetDictionary("group_id,group_name", "Groups");
			cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key).ToArray());
			cbGroupsDirection.Items.AddRange(d_directions.Select(d => d.Key).ToArray());
			cbStudentsDirection.Items.AddRange(d_directions.Select(d => d.Key).ToArray());
			cbStudentsGroup.Items.Insert(0, "Все группы");
			cbStudentsDirection.Items.Insert(0, "Все направления");
			cbGroupsDirection.Items.Insert(0, "Все направления");
			cbStudentsGroup.SelectedIndex = 0;
			cbStudentsDirection.SelectedIndex = 0;
			cbGroupsDirection.SelectedIndex = 0;

			toolStripStatusLabelCount.Text = $"Количество студентов: {dgvStudents.RowCount - 1}";
		}


		void LoadPage(int i, Query query = null)
		{
			if (query == null) query = queries[i];
			tables[i].DataSource =
				connector.Select(query.Columns, query.Tables, query.Condition, query.Group_by);
			toolStripStatusLabelCount.Text = status_messages[i] + CountRecordsInDGV(tables[i]);
		}
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			string tab_name = tabControl.SelectedTab.Name;
			Console.WriteLine(tab_name);
			//int i = tabControl.SelectedIndex;
			LoadPage(tabControl.SelectedIndex);
		}

		int CountRecordsInDGV(DataGridView dgv)
		{
			return dgv.Rows.Count == 0 ? 0 : dgv.RowCount - 1;
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			dgvDirections.DataSource = connector.Select
				(
					"direction_name,COUNT(DISTINCT group_id) AS N'Количество группов', COUNT(stud_id) AS N'Количество студентов'",
					"Students RIGHT JOIN Groups ON ([group]=group_id) RIGHT JOIN Directions ON (direction=direction_id)",
					"",
					"direction_name"
				);
		}

		private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string cb_name = (sender as ComboBox).Name;
			string tab_name = tabControl.SelectedTab.Name;

			int lastCapitalIndex = Array.FindLastIndex<char>(cb_name.ToCharArray(), Char.IsUpper);
			string cb_suffix = cb_name.Substring(lastCapitalIndex, cb_name.Length - lastCapitalIndex);
			Console.WriteLine(cb_name);
			Console.WriteLine(tab_name);
			Console.WriteLine(cb_suffix);

			string dictionary_name = $"d_{cb_suffix.ToLower()}s";
			Dictionary<string, int> dictionary =
				this.GetType().GetField(dictionary_name).GetValue(this) as Dictionary<string, int>;
			int i = (sender as ComboBox).SelectedIndex;

			if (d_dependencies.ContainsKey(sender as ComboBox))
			{
				foreach (ComboBox cb in d_dependencies[sender as ComboBox])
				{
					GetDependentData(cb, sender as ComboBox);
				}
			}

			Query query = new Query(queries[tabControl.SelectedIndex]);
			string condition =
				(i == 0 || cbStudentsDirection.SelectedItem == null ? "" : $"[{cb_suffix.ToLower()}]={dictionary[$"{(sender as ComboBox).SelectedItem}"]}");
			if (query.Condition == "") query.Condition = condition;
			else if (condition != "") query.Condition += $" AND {condition}";
			LoadPage(tabControl.SelectedIndex, query);
		}

		void GetDependentData(ComboBox dependent, ComboBox determinant)
		{
			Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
			Console.WriteLine(dependent.Name + "\t" + determinant.Name);

			string dependent_root =
				dependent.Name.Substring(Array.FindLastIndex<char>(determinant.Name.ToCharArray(), Char.IsUpper));
			string determinant_root =
				determinant.Name.Substring(Array.FindLastIndex<char>(determinant.Name.ToCharArray(), Char.IsUpper));

			Dictionary<string, int> dictionary =
				connector.GetDictionary
				(
					$"{dependent_root.ToLower()}_id,{dependent_root.ToLower()}_name",
					$"{dependent_root}s,{determinant_root}s",
					determinant.SelectedItem == null || determinant.SelectedIndex <= 0 ? "" : $"{determinant_root}={determinant.SelectedIndex}"
				);
			foreach (KeyValuePair<string, int> d in dictionary)
			{
				Console.WriteLine($"{d.Value}\t{d.Key}");
			}

			dependent.Items.Clear();
			dependent.Items.AddRange(dictionary.Select(d => d.Key).ToArray());

			Console.WriteLine("Dependent:\t" + dependent_root);
			Console.WriteLine("Determinant:\t" + determinant_root);
			Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
		}
	}
}