namespace VisualTestHarness
{
    partial class Form1
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
            this.PassBox1 = new System.Windows.Forms.TextBox();
            this.IPBox1 = new System.Windows.Forms.TextBox();
            this.DBBox1 = new System.Windows.Forms.TextBox();
            this.UNameBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.UNameBox2 = new System.Windows.Forms.TextBox();
            this.DBBox2 = new System.Windows.Forms.TextBox();
            this.IPBox2 = new System.Windows.Forms.TextBox();
            this.PassBox2 = new System.Windows.Forms.TextBox();
            this.compare1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // PassBox1
            // 
            this.PassBox1.Location = new System.Drawing.Point(13, 433);
            this.PassBox1.Name = "PassBox1";
            this.PassBox1.Size = new System.Drawing.Size(280, 22);
            this.PassBox1.TabIndex = 0;
            this.PassBox1.Text = "Password";
            // 
            // IPBox1
            // 
            this.IPBox1.Location = new System.Drawing.Point(12, 349);
            this.IPBox1.Name = "IPBox1";
            this.IPBox1.Size = new System.Drawing.Size(280, 22);
            this.IPBox1.TabIndex = 2;
            this.IPBox1.Text = "Server Name";
            // 
            // DBBox1
            // 
            this.DBBox1.Location = new System.Drawing.Point(12, 377);
            this.DBBox1.Name = "DBBox1";
            this.DBBox1.Size = new System.Drawing.Size(280, 22);
            this.DBBox1.TabIndex = 3;
            this.DBBox1.Text = "Database";
            // 
            // UNameBox1
            // 
            this.UNameBox1.Location = new System.Drawing.Point(12, 405);
            this.UNameBox1.Name = "UNameBox1";
            this.UNameBox1.Size = new System.Drawing.Size(280, 22);
            this.UNameBox1.TabIndex = 4;
            this.UNameBox1.Text = "Username";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(657, 349);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(648, 331);
            this.dataGridView1.TabIndex = 6;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(736, 12);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(648, 331);
            this.dataGridView2.TabIndex = 7;
            // 
            // UNameBox2
            // 
            this.UNameBox2.Location = new System.Drawing.Point(1104, 405);
            this.UNameBox2.Name = "UNameBox2";
            this.UNameBox2.Size = new System.Drawing.Size(280, 22);
            this.UNameBox2.TabIndex = 11;
            this.UNameBox2.Text = "Username";
            // 
            // DBBox2
            // 
            this.DBBox2.Location = new System.Drawing.Point(1104, 377);
            this.DBBox2.Name = "DBBox2";
            this.DBBox2.Size = new System.Drawing.Size(280, 22);
            this.DBBox2.TabIndex = 10;
            this.DBBox2.Text = "Database";
            // 
            // IPBox2
            // 
            this.IPBox2.Location = new System.Drawing.Point(1104, 349);
            this.IPBox2.Name = "IPBox2";
            this.IPBox2.Size = new System.Drawing.Size(280, 22);
            this.IPBox2.TabIndex = 9;
            this.IPBox2.Text = "Server Name";
            // 
            // PassBox2
            // 
            this.PassBox2.Location = new System.Drawing.Point(1104, 434);
            this.PassBox2.Name = "PassBox2";
            this.PassBox2.Size = new System.Drawing.Size(280, 22);
            this.PassBox2.TabIndex = 8;
            this.PassBox2.Text = "Password";
            // 
            // compare1
            // 
            this.compare1.Location = new System.Drawing.Point(622, 378);
            this.compare1.Name = "compare1";
            this.compare1.Size = new System.Drawing.Size(137, 32);
            this.compare1.TabIndex = 12;
            this.compare1.Text = "Compare(M)";
            this.compare1.UseVisualStyleBackColor = true;
            this.compare1.Click += new System.EventHandler(this.compare1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 468);
            this.Controls.Add(this.compare1);
            this.Controls.Add(this.UNameBox2);
            this.Controls.Add(this.DBBox2);
            this.Controls.Add(this.IPBox2);
            this.Controls.Add(this.PassBox2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UNameBox1);
            this.Controls.Add(this.DBBox1);
            this.Controls.Add(this.IPBox1);
            this.Controls.Add(this.PassBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IPBox1;
        private System.Windows.Forms.TextBox DBBox1;
        private System.Windows.Forms.TextBox UNameBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox PassBox1;
        private System.Windows.Forms.TextBox UNameBox2;
        private System.Windows.Forms.TextBox DBBox2;
        private System.Windows.Forms.TextBox IPBox2;
        private System.Windows.Forms.TextBox PassBox2;
        private System.Windows.Forms.Button compare1;
    }
}

