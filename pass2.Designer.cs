namespace EmployeeManagementSystem
{
    partial class pass2
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
            label3 = new Label();
            txtbox_newpass = new TextBox();
            txtbox_reenternewpass = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            btn_resetnewpass = new Button();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(39, 54);
            label3.Name = "label3";
            label3.Size = new Size(277, 38);
            label3.TabIndex = 3;
            label3.Text = "Reset Your Password";
            // 
            // txtbox_newpass
            // 
            txtbox_newpass.Location = new Point(42, 157);
            txtbox_newpass.Name = "txtbox_newpass";
            txtbox_newpass.Size = new Size(354, 27);
            txtbox_newpass.TabIndex = 4;
            txtbox_newpass.TextChanged += txtbox_newpass_TextChanged;
            // 
            // txtbox_reenternewpass
            // 
            txtbox_reenternewpass.Location = new Point(42, 206);
            txtbox_reenternewpass.Name = "txtbox_reenternewpass";
            txtbox_reenternewpass.Size = new Size(354, 27);
            txtbox_reenternewpass.TabIndex = 5;
            txtbox_reenternewpass.TextChanged += txtbox_reenternewpass_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 92);
            label1.Name = "label1";
            label1.Size = new Size(187, 20);
            label1.TabIndex = 6;
            label1.Text = "Please Enter Your Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(42, 134);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 7;
            label2.Text = "Password";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(42, 187);
            label4.Name = "label4";
            label4.Size = new Size(131, 20);
            label4.TabIndex = 8;
            label4.Text = "Re-enter Password";
            // 
            // btn_resetnewpass
            // 
            btn_resetnewpass.Location = new Point(210, 248);
            btn_resetnewpass.Name = "btn_resetnewpass";
            btn_resetnewpass.Size = new Size(186, 29);
            btn_resetnewpass.TabIndex = 9;
            btn_resetnewpass.Text = "Reset Password";
            btn_resetnewpass.UseVisualStyleBackColor = true;
            btn_resetnewpass.Click += btn_resetnewpass_Click;
            // 
            // pass2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(433, 320);
            Controls.Add(btn_resetnewpass);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtbox_reenternewpass);
            Controls.Add(txtbox_newpass);
            Controls.Add(label3);
            Name = "pass2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Password Maintainance";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label3;
        private TextBox txtbox_newpass;
        private TextBox txtbox_reenternewpass;
        private Label label1;
        private Label label2;
        private Label label4;
        private Button btn_resetnewpass;
    }
}