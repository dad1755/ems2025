using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class pass1 : Form
    {
        // ✅ Fixed connection string (same as MainForm)
        private string ConnectionString = "Data Source=StaffDB.sqlite;Version=3;";

        public pass1()
        {
            InitializeComponent();
            EnsureDatabaseAndTableExist(); // Ensure database structure is correct
        }

        // ✅ Check if email exists in the Staff table
        private bool IsEmailExists(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Database connected successfully!");

                    // ✅ Trim input and match case-insensitively
                    string query = "SELECT COUNT(*) FROM Staff WHERE LOWER(TRIM(Email)) = LOWER(TRIM(@Email))";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email.Trim().ToLower());
                        long count = (long)command.ExecuteScalar();

                        MessageBox.Show($"Query executed. Email count: {count}");

                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                    return false;
                }
            }
        }

        // ✅ Button click event to check if email exists and open pass2
        private void btn_resetcheckemail_Click(object sender, EventArgs e)
        {
            string email = txtbox_emailresetpass.Text.Trim(); // ✅ Trim spaces

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter an email address.");
                return;
            }

            // ✅ Check if email exists
            if (IsEmailExists(email))
            {
                MessageBox.Show("Email found! Redirecting to reset page...");
                pass2 resetPasswordForm = new pass2(email);
                resetPasswordForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("The email does not exist in our records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Ensure Staff table exists with correct columns
        private void EnsureDatabaseAndTableExist()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Staff (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Gender TEXT NOT NULL,
                        DOB TEXT NOT NULL,
                        Branch TEXT,
                        EmployeeNumber TEXT NOT NULL UNIQUE,
                        EmploymentStatus TEXT,
                        Department TEXT,
                        Email TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT,
                        StartDate TEXT NOT NULL
                    );";

                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // ✅ Ensure additional columns exist
                    AddColumnIfNotExists(connection, "PasswordHash", "TEXT");
                    AddColumnIfNotExists(connection, "basic_salary", "REAL");
                    AddColumnIfNotExists(connection, "allowance", "REAL");
                    AddColumnIfNotExists(connection, "leave", "INTEGER");
                    AddColumnIfNotExists(connection, "kwsp", "REAL");
                    AddColumnIfNotExists(connection, "perkeso", "REAL");
                    AddColumnIfNotExists(connection, "zakat", "REAL");
                    AddColumnIfNotExists(connection, "salary_gross", "REAL");
                    AddColumnIfNotExists(connection, "salary_net", "REAL");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ✅ Add column safely if not exists
        private void AddColumnIfNotExists(SQLiteConnection connection, string columnName, string columnType)
        {
            string checkColumnQuery = "PRAGMA table_info(Staff);";

            using (SQLiteCommand command = new SQLiteCommand(checkColumnQuery, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    bool columnExists = false;
                    while (reader.Read())
                    {
                        if (reader["name"].ToString().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                        {
                            columnExists = true;
                            break;
                        }
                    }

                    if (!columnExists)
                    {
                        string addColumnQuery = $"ALTER TABLE Staff ADD COLUMN {columnName} {columnType};";
                        using (SQLiteCommand addColumnCommand = new SQLiteCommand(addColumnQuery, connection))
                        {
                            addColumnCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // ✅ Button click event to go back to login form
        private void lbl_backtologin_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (this.Owner != null)
            {
                this.Owner.Show();
            }
        }
    }
}
