using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class LoginPage : Form
    {
        // Database connection string
        private string ConnectionString = "Data Source=StaffDB.sqlite;Version=3;";

        public LoginPage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            // Any additional setup when the form loads (if needed)
        }

        // Handle the login button click event
        private void button1_Click_1(object sender, EventArgs e)
        {
            string username = email_txtbox.Text.Trim();
            string password = pass_txtbox.Text;

            if (username.ToLower() == "admin" && password == "1234")
            {
                MessageBox.Show("Admin Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                MainForm mainForm = new MainForm();  // Admin form, no email needed
                mainForm.Show();
                return;
            }

            if (IsValidUser(username, password))
            {
                // Create Attendance table if it doesn't exist
                CreateAttendanceTable();
                CreateLeaveTable();

                MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                MainForm2 mainForm2 = new MainForm2(username); // Pass email to MainForm2
                mainForm2.Show();
            }
            else
            {
                MessageBox.Show("Invalid Email or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateAttendanceTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // SQL command to create the Attendance table if it doesn't exist
                    string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Attendance (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    Email TEXT NOT NULL,
                    EmployeeNumber TEXT NOT NULL,
                    CheckInTime TEXT,
                    CheckOutTime TEXT,
                    TotalHoursWorked TEXT,                    
                    Status TEXT,
                    FOREIGN KEY (Email) REFERENCES Staff(Email)
                );
            ";

                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating Attendance table: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CreateLeaveTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the 'Leave' table exists and if the 'Status' column is already present
                    string checkColumnQuery = @"
PRAGMA table_info(Leave);
";

                    using (SQLiteCommand command = new SQLiteCommand(checkColumnQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            bool columnExists = false;

                            // Check if the 'Status' column exists
                            while (reader.Read())
                            {
                                string columnName = reader["name"].ToString();
                                if (columnName.Equals("Status", StringComparison.OrdinalIgnoreCase))
                                {
                                    columnExists = true;
                                    break;
                                }
                            }

                            // If the 'Status' column doesn't exist, add it
                            if (!columnExists)
                            {
                                string addColumnQuery = @"
ALTER TABLE Leave 
ADD COLUMN Status TEXT DEFAULT 'Pending';
";
                                using (SQLiteCommand addColumnCommand = new SQLiteCommand(addColumnQuery, connection))
                                {
                                    addColumnCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // Create Leave table if it doesn't exist
                    string createTableQuery = @"
CREATE TABLE IF NOT EXISTS Leave (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeNumber TEXT NOT NULL,
    Email TEXT NOT NULL,
    LeaveType TEXT NOT NULL,
    StartDate TEXT NOT NULL,
    EndDate TEXT NOT NULL,
    TotalDays INTEGER NOT NULL,
    Description TEXT,
    File BLOB,
    Status TEXT DEFAULT 'Pending',  -- Ensures the Status column is part of the table if it wasn't added manually
    FOREIGN KEY (Email) REFERENCES Staff(Email)
);
";

                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating Leave table: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Method to validate user by checking email and password hash
        private bool IsValidUser(string email, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Query to fetch the password hash from the database
                    string query = "SELECT PasswordHash FROM Staff WHERE LOWER(Email) = LOWER(@Email)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedPasswordHash = result.ToString();
                            // Compare the entered password's hash with the stored hash
                            return VerifyPasswordHash(password, storedPasswordHash);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return false;
        }

        // Method to verify the entered password against the stored password hash
        private bool VerifyPasswordHash(string enteredPassword, string storedHash)
        {
            // Hash the entered password and compare with stored hash
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedHash;
        }

        // Method to hash the password using SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pass_txtbox.UseSystemPasswordChar = true;
        }

        // Reset password link clicked
        private void lbl_resetpass_Click(object sender, EventArgs e)
        {
            // Create an instance of the new password reset form (pass1)
            pass1 resetPassForm = new pass1();

            // Set the login form as the owner of pass1
            resetPassForm.Owner = this;

            // Hide the login form
            this.Hide();

            // Show the new password reset form
            resetPassForm.Show();
        }
    }
}
