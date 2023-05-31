namespace ClearCookiesEasily
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lvBrowsers = new ListView();
            comboBoxTimeRange = new ComboBox();
            btnClear = new Button();
            SuspendLayout();
            // 
            // lvBrowsers
            // 
            lvBrowsers.CheckBoxes = true;
            lvBrowsers.Location = new Point(13, 12);
            lvBrowsers.Margin = new Padding(4, 3, 4, 3);
            lvBrowsers.Name = "lvBrowsers";
            lvBrowsers.Size = new Size(441, 249);
            lvBrowsers.TabIndex = 3;
            lvBrowsers.UseCompatibleStateImageBehavior = false;
            // 
            // comboBoxTimeRange
            // 
            comboBoxTimeRange.FormattingEnabled = true;
            comboBoxTimeRange.Location = new Point(13, 267);
            comboBoxTimeRange.Name = "comboBoxTimeRange";
            comboBoxTimeRange.Size = new Size(121, 23);
            comboBoxTimeRange.TabIndex = 4;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(379, 267);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += BtnClear_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(467, 301);
            Controls.Add(btnClear);
            Controls.Add(comboBoxTimeRange);
            Controls.Add(lvBrowsers);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Clear Cookies Easily";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListView lvBrowsers;
        private ComboBox comboBoxTimeRange;
        private Button btnClear;
    }
}