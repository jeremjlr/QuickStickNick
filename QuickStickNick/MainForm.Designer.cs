namespace QuickStickNick
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
            this.startButton = new System.Windows.Forms.Button();
            this.BMButton = new System.Windows.Forms.RadioButton();
            this.ArmsButton = new System.Windows.Forms.RadioButton();
            this.ProtButton = new System.Windows.Forms.RadioButton();
            this.SurvivalButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(298, 170);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(194, 103);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // BMButton
            // 
            this.BMButton.AutoSize = true;
            this.BMButton.Location = new System.Drawing.Point(354, 33);
            this.BMButton.Name = "BMButton";
            this.BMButton.Size = new System.Drawing.Size(67, 17);
            this.BMButton.TabIndex = 1;
            this.BMButton.TabStop = true;
            this.BMButton.Text = "BM Hunt";
            this.BMButton.UseVisualStyleBackColor = true;
            // 
            // ArmsButton
            // 
            this.ArmsButton.AutoSize = true;
            this.ArmsButton.Location = new System.Drawing.Point(354, 81);
            this.ArmsButton.Name = "ArmsButton";
            this.ArmsButton.Size = new System.Drawing.Size(71, 17);
            this.ArmsButton.TabIndex = 2;
            this.ArmsButton.TabStop = true;
            this.ArmsButton.Text = "Arms War";
            this.ArmsButton.UseVisualStyleBackColor = true;
            // 
            // ProtButton
            // 
            this.ProtButton.AutoSize = true;
            this.ProtButton.Location = new System.Drawing.Point(354, 104);
            this.ProtButton.Name = "ProtButton";
            this.ProtButton.Size = new System.Drawing.Size(67, 17);
            this.ProtButton.TabIndex = 3;
            this.ProtButton.TabStop = true;
            this.ProtButton.Text = "Prot War";
            this.ProtButton.UseVisualStyleBackColor = true;
            // 
            // SurvivalButton
            // 
            this.SurvivalButton.AutoSize = true;
            this.SurvivalButton.Location = new System.Drawing.Point(354, 56);
            this.SurvivalButton.Name = "SurvivalButton";
            this.SurvivalButton.Size = new System.Drawing.Size(89, 17);
            this.SurvivalButton.TabIndex = 4;
            this.SurvivalButton.TabStop = true;
            this.SurvivalButton.Text = "Survival Hunt";
            this.SurvivalButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SurvivalButton);
            this.Controls.Add(this.ProtButton);
            this.Controls.Add(this.ArmsButton);
            this.Controls.Add(this.BMButton);
            this.Controls.Add(this.startButton);
            this.Name = "MainForm";
            this.Text = "QuickStickNick";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        public System.Windows.Forms.RadioButton BMButton;
        public System.Windows.Forms.RadioButton ArmsButton;
        public System.Windows.Forms.RadioButton ProtButton;
        public System.Windows.Forms.RadioButton SurvivalButton;
    }
}

