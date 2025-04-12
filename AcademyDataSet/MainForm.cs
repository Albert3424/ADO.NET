using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using System.Data.SqlClient;
using System.Configuration;

namespace AcademyDataSet
{
	public partial class MainForm : Form
	{

		Cache GroupsRelatedData;
		public MainForm()
		{
			InitializeComponent();
			AllocConsole();

			GroupsRelatedData = new Cache();
			GroupsRelatedData.AddTable("Directions", "direction_id,direction_name");
			GroupsRelatedData.AddTable("Groups", "group_id,group_name,direction");
			GroupsRelatedData.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
			GroupsRelatedData.Load();
			GroupsRelatedData.Print("Directions");
			GroupsRelatedData.Print("Groups");

			cbDirections.DataSource = GroupsRelatedData.Set.Tables["Directions"];
			cbDirections.DisplayMember = "direction_name";
			cbDirections.ValueMember = "direction_id";

			cbGroups.DataSource = GroupsRelatedData.Set.Tables["Groups"];
			cbGroups.DisplayMember = "group_name";
			cbGroups.ValueMember = "group_id";
		}

		private void cbDirections_SelectedIndexChanged(object sender, EventArgs e)
		{
			GroupsRelatedData.Set.Tables["Groups"].DefaultView.RowFilter = $"direction={cbDirections.SelectedValue}";
		}
		public void RefreshCache()
		{
			GroupsRelatedData.AddTable("Directions", "direction_id,direction_name");
			GroupsRelatedData.AddTable("Groups", "group_id,group_name,direction");
			GroupsRelatedData.AddTable("Students", "stud_id,last_name,first_name,middle_name,birth_date,group");
			GroupsRelatedData.AddRelation("GroupsDirections", "Groups,direction", "Directions,direction_id");
			GroupsRelatedData.AddRelation("StudentsGroups", "Students,group", "Groups,group_id");
			GroupsRelatedData.Load();
			GroupsRelatedData.Print("Directions");
			GroupsRelatedData.Print("Groups");
			GroupsRelatedData.Print("Students");
		}
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			GroupsRelatedData.ClearCache();
			RefreshCache();
		}
		
		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();
		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();
	}
}