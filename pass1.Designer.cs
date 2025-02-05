namespace EmployeeManagementSystem
{
    partial class pass1
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtbox_emailresetpass = new TextBox();
            btn_resetcheckemail = new Button();
            lbl_backtologin = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 92);
            label1.Name = "label1";
            label1.Size = new Size(354, 20);
            label1.TabIndex = 0;
            label1.Text = "Please enter email associated with your  staff details";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 134);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 1;
            label2.Text = "Your Email";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(39, 54);
            label3.Name = "label3";
            label3.Size = new Size(277, 38);
            label3.TabIndex = 2;
            label3.Text = "Reset Your Password";
            // 
            // txtbox_emailresetpass
            // 
            txtbox_emailresetpass.Location = new Point(42, 157);
            txtbox_emailresetpass.Name = "txtbox_emailresetpass";
            txtbox_emailresetpass.Size = new Size(354, 27);
            txtbox_emailresetpass.TabIndex = 3;
            // 
            // btn_resetcheckemail
            // 
            btn_resetcheckemail.Location = new Point(210, 190);
            btn_resetcheckemail.Name = "btn_resetcheckemail";
            btn_resetcheckemail.Size = new Size(186, 29);
            btn_resetcheckemail.TabIndex = 4;
            btn_resetcheckemail.Text = "Reset Password";
            btn_resetcheckemail.UseVisualStyleBackColor = true;
            btn_resetcheckemail.Click += btn_resetcheckemail_Click;
            // 
            // lbl_backtologin
            // 
            lbl_backtologin.AutoSize = true;
            lbl_backtologin.ForeColor = Color.Blue;
            lbl_backtologin.Location = new Point(42, 273);
            lbl_backtologin.Name = "lbl_backtologin";
            lbl_backtologin.Size = new Size(99, 20);
            lbl_backtologin.TabIndex = 5;
            lbl_backtologin.Text = "Back to Login";
            lbl_backtologin.Click += lbl_backtologin_Click;
            // 
            // pass1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(433, 320);
            Controls.Add(lbl_backtologin);
            Controls.Add(btn_resetcheckemail);
            Controls.Add(txtbox_emailresetpass);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "pass1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Password Maintenance";
            TransparencyKey = Color.FromArgb(255, 224, 192);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtbox_emailresetpass;
        private Button btn_resetcheckemail;
        private Label lbl_backtologin;
    }
}