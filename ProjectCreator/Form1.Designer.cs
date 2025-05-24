namespace ProjectCreator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            button1 = new Button();
            textBox2 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.ButtonHighlight;
            textBox1.Font = new Font("Microsoft YaHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            textBox1.Location = new Point(42, 97);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(820, 32);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button1.Location = new Point(879, 97);
            button1.Name = "button1";
            button1.Size = new Size(51, 32);
            button1.TabIndex = 1;
            button1.Text = "· · ·";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.AcceptsReturn = true;
            textBox2.AcceptsTab = true;
            textBox2.Location = new Point(42, 135);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(888, 560);
            textBox2.TabIndex = 2;
            // 
            // button2
            // 
            button2.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button2.Location = new Point(42, 38);
            button2.Name = "button2";
            button2.Size = new Size(209, 41);
            button2.TabIndex = 3;
            button2.Text = "生成项目至指定路径";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button3.Location = new Point(721, 38);
            button3.Name = "button3";
            button3.Size = new Size(209, 41);
            button3.TabIndex = 4;
            button3.Text = "从指定路径删除项目";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI Light", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(378, 36);
            label1.Name = "label1";
            label1.Size = new Size(220, 38);
            label1.TabIndex = 5;
            label1.Text = "MakeByElanima";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(972, 745);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            MaximizeBox = false;
            Name = "Form1";
            Text = "ProjectCreator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private TextBox textBox2;
        private Button button2;
        private Button button3;
        private Label label1;
    }
}
