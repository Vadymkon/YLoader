
namespace YLoader
{
    partial class Form3
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
            this.egoldsGoogleTextBox1 = new yt_DesignUI.EgoldsGoogleTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // egoldsGoogleTextBox1
            // 
            this.egoldsGoogleTextBox1.BackColor = System.Drawing.Color.White;
            this.egoldsGoogleTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.egoldsGoogleTextBox1.BorderColorNotActive = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.egoldsGoogleTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.egoldsGoogleTextBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.egoldsGoogleTextBox1.FontTextPreview = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.egoldsGoogleTextBox1.ForeColor = System.Drawing.Color.Black;
            this.egoldsGoogleTextBox1.Location = new System.Drawing.Point(14, 118);
            this.egoldsGoogleTextBox1.Name = "egoldsGoogleTextBox1";
            this.egoldsGoogleTextBox1.SelectionStart = 0;
            this.egoldsGoogleTextBox1.Size = new System.Drawing.Size(270, 40);
            this.egoldsGoogleTextBox1.TabIndex = 33;
            this.egoldsGoogleTextBox1.TextInput = "";
            this.egoldsGoogleTextBox1.TextPreview = "DD.MM.YYYY  - date to insert";
            this.egoldsGoogleTextBox1.UseSystemPasswordChar = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(144, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 34;
            this.button1.Text = "Go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(63, 170);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 34);
            this.button2.TabIndex = 34;
            this.button2.Text = "No. Go back.";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(269, 30);
            this.button3.TabIndex = 34;
            this.button3.Text = "Click to choose videos";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(13, 48);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(269, 64);
            this.textBox1.TabIndex = 35;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 219);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.egoldsGoogleTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form3";
            this.Text = "Add Videos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private yt_DesignUI.EgoldsGoogleTextBox egoldsGoogleTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
    }
}