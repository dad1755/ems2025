using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class pass2 : Form
    {
        private string userEmail; // Store email passed from pass1
        private string ConnectionString = "Data Source=StaffDB.sqlite;Version=3;"; // Use the same DB path as pass1

        // Constructor to receive email from pass1
        public pass2(string email)
        {
            InitializeComponent();
            userEmail = email; // Store the email
        }

        // Button click event to reset the password
        private void btn_resetnewpass_Click(object sender, EventArgs e)
        {
            string newPassword = txtbox_newpass.Text.Trim();
            string reenteredPassword = txtbox_reenternewpass.Text.Trim();

            // Validate passwords
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(reenteredPassword))
            {
                MessageBox.Show("Please enter and confirm your new password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != reenteredPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Store the new password (hashed)
            if (StorePassword(userEmail, newPassword))
            {
                MessageBox.Show("Password reset successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the current pass2 form
                this.Close();

                // Open the LoginPage after successful password reset
                LoginPage loginPageForm = new LoginPage(); // Create a new instance of LoginPage
                loginPageForm.Show(); // Show LoginPage
            }
            else
            {
                MessageBox.Show("Failed to reset password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Method to store the hashed password in the database
        private bool StorePassword(string email, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Hash the new password before storing it
                    string hashedPassword = HashPassword(password);

                    // Update query to store the hashed password
                    string updateQuery = "UPDATE Staff SET PasswordHash = @PasswordHash WHERE LOWER(TRIM(Email)) = LOWER(TRIM(@Email))";

                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        command.Parameters.AddWithValue("@Email", email);

                        // Execute the query and check if the update is successful
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0; // Return true if password was updated
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resetting password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
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
                return builder.ToString(); // Return hashed password
            }
        }

        // Event handlers for password textboxes (optional, for validation)
        private void txtbox_newpass_TextChanged(object sender, EventArgs e) { }

        private void txtbox_reenternewpass_TextChanged(object sender, EventArgs e) { }
    }
}
