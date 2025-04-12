
namespace AcademyDataSet
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbGroups = new System.Windows.Forms.ComboBox();
			this.cbDirections = new System.Windows.Forms.ComboBox();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbGroups
			// 
			this.cbGroups.FormattingEnabled = true;
			this.cbGroups.Location = new System.Drawing.Point(132, 13);
			this.cbGroups.Name = "cbGroups";
			this.cbGroups.Size = new System.Drawing.Size(160, 21);
			this.cbGroups.TabIndex = 0;
			// 
			// cbDirections
			// 
			this.cbDirections.FormattingEnabled = true;
			this.cbDirections.Location = new System.Drawing.Point(298, 13);
			this.cbDirections.Name = "cbDirections";
			this.cbDirections.Size = new System.Drawing.Size(248, 21);
			this.cbDirections.TabIndex = 1;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(553, 10);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.cbDirections);
			this.Controls.Add(this.cbGroups);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cbGroups;
		private System.Windows.Forms.ComboBox cbDirections;
		private System.Windows.Forms.Button btnRefresh;
	}
}

