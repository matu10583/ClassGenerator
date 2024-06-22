namespace ClassGenerator
{
    partial class GeneratorForm
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
            this.components = new System.ComponentModel.Container();
            this.input_className = new System.Windows.Forms.TextBox();
            this.btn_generate = new System.Windows.Forms.Button();
            this.input_srcPath = new System.Windows.Forms.TextBox();
            this.input_incPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.doGenerateClass = new System.Windows.Forms.CheckBox();
            this.input_inline = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.input_extendsList = new System.Windows.Forms.TextBox();
            this.listBox_extends = new System.Windows.Forms.ListBox();
            this.btn_removeExtends = new System.Windows.Forms.Button();
            this.btn_addExtends = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.doGenerateInline = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.input_filter = new System.Windows.Forms.TextBox();
            this.ctxm_completion = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listBox_completion = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // input_className
            // 
            this.input_className.Location = new System.Drawing.Point(72, 40);
            this.input_className.Name = "input_className";
            this.input_className.Size = new System.Drawing.Size(100, 19);
            this.input_className.TabIndex = 0;
            this.input_className.Click += new System.EventHandler(this.input_className_Click);
            this.input_className.TextChanged += new System.EventHandler(this.input_className_TextChanged);
            this.input_className.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_className_KeyDown);
            this.input_className.Leave += new System.EventHandler(this.input_className_Leave);
            // 
            // btn_generate
            // 
            this.btn_generate.Location = new System.Drawing.Point(687, 398);
            this.btn_generate.Name = "btn_generate";
            this.btn_generate.Size = new System.Drawing.Size(75, 23);
            this.btn_generate.TabIndex = 1;
            this.btn_generate.Text = "Generate";
            this.btn_generate.UseVisualStyleBackColor = true;
            this.btn_generate.Click += new System.EventHandler(this.button1_Click);
            // 
            // input_srcPath
            // 
            this.input_srcPath.Location = new System.Drawing.Point(140, 232);
            this.input_srcPath.Name = "input_srcPath";
            this.input_srcPath.Size = new System.Drawing.Size(100, 19);
            this.input_srcPath.TabIndex = 2;
            this.input_srcPath.TextChanged += new System.EventHandler(this.input_srcPath_TextChanged);
            // 
            // input_incPath
            // 
            this.input_incPath.Location = new System.Drawing.Point(140, 188);
            this.input_incPath.Name = "input_incPath";
            this.input_incPath.Size = new System.Drawing.Size(100, 19);
            this.input_incPath.TabIndex = 3;
            this.input_incPath.TextChanged += new System.EventHandler(this.input_incPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "ClassName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Header Path(.h)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Source Path(.cpp)";
            // 
            // doGenerateClass
            // 
            this.doGenerateClass.AutoSize = true;
            this.doGenerateClass.Checked = true;
            this.doGenerateClass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doGenerateClass.Location = new System.Drawing.Point(5, 140);
            this.doGenerateClass.Name = "doGenerateClass";
            this.doGenerateClass.Size = new System.Drawing.Size(103, 16);
            this.doGenerateClass.TabIndex = 5;
            this.doGenerateClass.Text = "Generate Class";
            this.doGenerateClass.UseVisualStyleBackColor = true;
            this.doGenerateClass.CheckedChanged += new System.EventHandler(this.doGenerateClass_CheckedChanged);
            // 
            // input_inline
            // 
            this.input_inline.Location = new System.Drawing.Point(140, 292);
            this.input_inline.Name = "input_inline";
            this.input_inline.Size = new System.Drawing.Size(100, 19);
            this.input_inline.TabIndex = 3;
            this.input_inline.TextChanged += new System.EventHandler(this.input_inline_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 292);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Inline Path(.inl)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.input_extendsList);
            this.groupBox1.Controls.Add(this.listBox_extends);
            this.groupBox1.Controls.Add(this.btn_removeExtends);
            this.groupBox1.Controls.Add(this.btn_addExtends);
            this.groupBox1.Location = new System.Drawing.Point(436, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 119);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extends";
            // 
            // input_extendsList
            // 
            this.input_extendsList.Location = new System.Drawing.Point(6, 45);
            this.input_extendsList.Name = "input_extendsList";
            this.input_extendsList.Size = new System.Drawing.Size(120, 19);
            this.input_extendsList.TabIndex = 13;
            this.input_extendsList.Visible = false;
            this.input_extendsList.Click += new System.EventHandler(this.input_extendList_Click);
            this.input_extendsList.TextChanged += new System.EventHandler(this.input_extendsList_TextChanged);
            this.input_extendsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_extendsList_KeyDown);
            this.input_extendsList.Leave += new System.EventHandler(this.input_extendsList_Leave);
            // 
            // listBox_extends
            // 
            this.listBox_extends.FormattingEnabled = true;
            this.listBox_extends.ItemHeight = 12;
            this.listBox_extends.Location = new System.Drawing.Point(6, 29);
            this.listBox_extends.Name = "listBox_extends";
            this.listBox_extends.Size = new System.Drawing.Size(120, 88);
            this.listBox_extends.TabIndex = 2;
            this.listBox_extends.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_extends_DoubleClicked);
            // 
            // btn_removeExtends
            // 
            this.btn_removeExtends.Location = new System.Drawing.Point(90, 0);
            this.btn_removeExtends.Name = "btn_removeExtends";
            this.btn_removeExtends.Size = new System.Drawing.Size(28, 23);
            this.btn_removeExtends.TabIndex = 1;
            this.btn_removeExtends.Text = "-";
            this.btn_removeExtends.UseVisualStyleBackColor = true;
            this.btn_removeExtends.Click += new System.EventHandler(this.btn_removeExtends_Click);
            // 
            // btn_addExtends
            // 
            this.btn_addExtends.Location = new System.Drawing.Point(56, 0);
            this.btn_addExtends.Name = "btn_addExtends";
            this.btn_addExtends.Size = new System.Drawing.Size(28, 23);
            this.btn_addExtends.TabIndex = 1;
            this.btn_addExtends.Text = "+";
            this.btn_addExtends.UseVisualStyleBackColor = true;
            this.btn_addExtends.Click += new System.EventHandler(this.btn_addExtends_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "Filter";
            // 
            // doGenerateInline
            // 
            this.doGenerateInline.AutoSize = true;
            this.doGenerateInline.Location = new System.Drawing.Point(163, 140);
            this.doGenerateInline.Name = "doGenerateInline";
            this.doGenerateInline.Size = new System.Drawing.Size(51, 16);
            this.doGenerateInline.TabIndex = 5;
            this.doGenerateInline.Text = "Inline";
            this.doGenerateInline.UseVisualStyleBackColor = true;
            this.doGenerateInline.CheckedChanged += new System.EventHandler(this.doGenerateInline_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(246, 228);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "参照";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(246, 287);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "参照";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(246, 184);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "参照";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // input_filter
            // 
            this.input_filter.Location = new System.Drawing.Point(72, 82);
            this.input_filter.Name = "input_filter";
            this.input_filter.Size = new System.Drawing.Size(100, 19);
            this.input_filter.TabIndex = 11;
            this.input_filter.TextChanged += new System.EventHandler(this.input_filter_TextChanged);
            // 
            // ctxm_completion
            // 
            this.ctxm_completion.Name = "ctxm_completion";
            this.ctxm_completion.Size = new System.Drawing.Size(61, 4);
            // 
            // listBox_completion
            // 
            this.listBox_completion.ForeColor = System.Drawing.SystemColors.Highlight;
            this.listBox_completion.FormattingEnabled = true;
            this.listBox_completion.ItemHeight = 12;
            this.listBox_completion.Location = new System.Drawing.Point(471, 287);
            this.listBox_completion.Name = "listBox_completion";
            this.listBox_completion.Size = new System.Drawing.Size(120, 88);
            this.listBox_completion.TabIndex = 12;
            this.listBox_completion.Visible = false;
            this.listBox_completion.SelectedIndexChanged += new System.EventHandler(this.listBox_completion_Selected);
            // 
            // GeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox_completion);
            this.Controls.Add(this.input_filter);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.doGenerateInline);
            this.Controls.Add(this.doGenerateClass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.input_inline);
            this.Controls.Add(this.input_incPath);
            this.Controls.Add(this.input_srcPath);
            this.Controls.Add(this.btn_generate);
            this.Controls.Add(this.input_className);
            this.Name = "GeneratorForm";
            this.Text = "GeneratorForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GenerateForm_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Input_className_Leave(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TextBox input_className;
        private System.Windows.Forms.Button btn_generate;
        private System.Windows.Forms.TextBox input_srcPath;
        private System.Windows.Forms.TextBox input_incPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox doGenerateClass;
        private System.Windows.Forms.TextBox input_inline;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_addExtends;
        private System.Windows.Forms.Button btn_removeExtends;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox doGenerateInline;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox input_filter;
        private System.Windows.Forms.ContextMenuStrip ctxm_completion;
        private System.Windows.Forms.ListBox listBox_completion;
        private System.Windows.Forms.ListBox listBox_extends;
        private System.Windows.Forms.TextBox input_extendsList;
    }
}