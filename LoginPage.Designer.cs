namespace EmployeeManagementSystem
{
    partial class LoginPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginPage));
            btn_login = new Button();
            email_txtbox = new TextBox();
            pass_txtbox = new TextBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            lbl_resetpass = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btn_login
            // 
            btn_login.Location = new Point(40, 212);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(250, 50);
            btn_login.TabIndex = 0;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += button1_Click_1;
            // 
            // email_txtbox
            // 
            email_txtbox.Location = new Point(40, 116);
            email_txtbox.Name = "email_txtbox";
            email_txtbox.Size = new Size(250, 27);
            email_txtbox.TabIndex = 1;
            // 
            // pass_txtbox
            // 
            pass_txtbox.Location = new Point(40, 165);
            pass_txtbox.Name = "pass_txtbox";
            pass_txtbox.Size = new Size(250, 27);
            pass_txtbox.TabIndex = 2;
            pass_txtbox.TextChanged += textBox2_TextChanged;
            // 
            // label3
            // 
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 12;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources._3d_illustration_computer_monitor_login_screen;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(320, 61);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(269, 195);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift Condensed", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(320, 34);
            label1.Name = "label1";
            label1.Size = new Size(255, 24);
            label1.TabIndex = 7;
            label1.Text = "Employee Management System 2025";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift SemiLight", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonShadow;
            label2.Location = new Point(37, 68);
            label2.Name = "label2";
            label2.Size = new Size(177, 18);
            label2.TabIndex = 9;
            label2.Text = "Please Enter Your Details";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Bahnschrift", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(28, 34);
            label4.Name = "label4";
            label4.Size = new Size(215, 34);
            label4.TabIndex = 10;
            label4.Text = "Welcome Back !";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Bahnschrift SemiLight", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(40, 97);
            label5.Name = "label5";
            label5.Size = new Size(92, 16);
            label5.TabIndex = 11;
            label5.Text = "Email Address";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Bahnschrift SemiLight", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(40, 146);
            label6.Name = "label6";
            label6.Size = new Size(66, 16);
            label6.TabIndex = 13;
            label6.Text = "Password";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Bahnschrift Condensed", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ButtonShadow;
            label7.Location = new Point(320, 259);
            label7.Name = "label7";
            label7.Size = new Size(269, 16);
            label7.TabIndex = 14;
            label7.Text = "Please contact Human Resource Department  for your details";
            label7.TextAlign = ContentAlignment.TopCenter;
            // 
            // lbl_resetpass
            // 
            lbl_resetpass.AutoSize = true;
            lbl_resetpass.ForeColor = SystemColors.Highlight;
            lbl_resetpass.Location = new Point(180, 265);
            lbl_resetpass.Name = "lbl_resetpass";
            lbl_resetpass.Size = new Size(110, 20);
            lbl_resetpass.TabIndex = 15;
            lbl_resetpass.Text = "Reset Password";
            lbl_resetpass.Click += lbl_resetpass_Click;
            // 
            // LoginPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 305);
            Controls.Add(lbl_resetpass);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(pass_txtbox);
            Controls.Add(email_txtbox);
            Controls.Add(btn_login);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginPage";
            Text = "Login Pages";
            Load += LoginPage_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_login;
        private TextBox email_txtbox;
        private TextBox pass_txtbox;
        private Label label3;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label lbl_resetpass;
    }
}
