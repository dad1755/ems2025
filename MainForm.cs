using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class MainForm : Form
    {
        private const string ConnectionString = "Data Source=StaffDB.sqlite;Version=3;";

        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Center screen
            EnsureDatabaseAndTableExist(); // Ensure database and table exist
                                           // Reload staff data after adding new staff
            LoadStaffData();
        }

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
                Email TEXT,
                PasswordHash TEXT,
                Image BLOB, 
                StartDate TEXT NOT NULL
            );";

                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Add columns safely
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


        private void AddColumnIfNotExists(SQLiteConnection connection, string columnName, string columnType)
        {
            try
            {
                string checkColumnQuery = $"PRAGMA table_info(Staff);";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkColumnQuery, connection))
                using (SQLiteDataReader reader = checkCommand.ExecuteReader())  // Ensure reader is disposed
                {
                    bool columnExists = false;
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == columnName)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding column {columnName}: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Method to load data from the database into the DataGridView
        private void LoadStaffData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Staff;"; // Fetch all columns from the Staff table

                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    dataAdapter.Fill(dataTable);

                    // Bind the DataTable to DataGridView
                    emptab_staffview.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading staff data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Handle TabControl selection change to load data when the tab is selected
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected tab is the one where you want to show the data
            if (tabControl1.SelectedIndex == 1) // Tab index 1 corresponds to the staff data tab
            {
                LoadStaffData();
            }
        }

        private void btn_updatestaff_Click(object sender, EventArgs e)
        {
            // Retrieve updated data from form controls
            string firstName = firstname_txtbox.Text;
            string lastName = lastname_txtbox.Text;
            string gender = rb_male.Checked ? "Male" : "Female";
            string dob = dob_calendar.SelectionStart.ToString("yyyy-MM-dd");
            string branch = cb_branch.SelectedItem?.ToString() ?? "";
            string employeeNumber = emp_num_txtbox.Text;
            string employmentStatus = cb_employ_status.SelectedItem?.ToString() ?? "";
            string department = cb_department.SelectedItem?.ToString() ?? "";
            string email = email_txtbox.Text;
            string startDate = startdate_calendar.SelectionStart.ToString("yyyy-MM-dd");

            UpdateStaff(firstName, lastName, gender, dob, branch, employeeNumber, employmentStatus, department, email, startDate);
            // Reload staff data after adding new staff
            LoadStaffData();

        }

        private void UpdateStaff(string firstName, string lastName, string gender, string dob, string branch,
                                 string employeeNumber, string employmentStatus, string department, string email, string startDate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE Staff 
                        SET FirstName = @FirstName, LastName = @LastName, Gender = @Gender, DOB = @DOB, 
                            Branch = @Branch, EmploymentStatus = @EmploymentStatus, Department = @Department, 
                            Email = @Email, StartDate = @StartDate 
                        WHERE EmployeeNumber = @EmployeeNumber;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@DOB", dob);
                        command.Parameters.AddWithValue("@Branch", branch);
                        command.Parameters.AddWithValue("@EmploymentStatus", employmentStatus);
                        command.Parameters.AddWithValue("@Department", department);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);

                        if (command.ExecuteNonQuery() > 0)
                            MessageBox.Show("Staff updated successfully!");
                        else
                            MessageBox.Show("No records updated. Please check Employee Number.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating staff: " + ex.Message);
                }
            }
            btn_addnew_staff.Visible = true;
            ClearInputFields();
        }
        private void ClearInputFields()
        {
            // Clear textboxes
            firstname_txtbox.Clear();
            lastname_txtbox.Clear();
            emp_num_txtbox.Clear();
            emp_num_txtbox.Enabled = true;
            email_txtbox.Clear();
            search_txtbox.Clear();
            search_payroll_txtbox.Clear();

            // Reset radio buttons (assuming gender is represented by Male/Female radio buttons)
            rb_male.Checked = false;
            rb_female.Checked = false;

            // Clear DateTimePickers
            dob_calendar.SetDate(DateTime.Now);  // Optionally reset to current date or clear it if needed
            startdate_calendar.SetDate(DateTime.Now);  // Optionally reset to current date or clear it if needed

            // Clear ComboBoxes
            cb_branch.SelectedIndex = -1;  // Reset to default (no selection)
            cb_employ_status.SelectedIndex = -1;  // Reset to default (no selection)
            cb_department.SelectedIndex = -1;  // Reset to default (no selection)
        }

        private void btn_addnew_staff_Click(object sender, EventArgs e)
        {
            // Retrieve the values from the form controls
            string firstName = firstname_txtbox.Text;
            string lastName = lastname_txtbox.Text;
            string gender = rb_male.Checked ? "Male" : (rb_female.Checked ? "Female" : null);
            string dob = dob_calendar.SelectionStart.ToString("yyyy-MM-dd");
            string branch = cb_branch.SelectedItem?.ToString();
            string employeeNumber = emp_num_txtbox.Text;
            string employmentStatus = cb_employ_status.SelectedItem?.ToString();
            string department = cb_department.SelectedItem?.ToString();
            string email = email_txtbox.Text;
            string startDate = startdate_calendar.SelectionStart.ToString("yyyy-MM-dd");

            // Start building the validation message
            List<string> missingFields = new List<string>();

            // Validate inputs and add missing fields to the list
            if (string.IsNullOrWhiteSpace(firstName)) missingFields.Add("First Name");
            if (string.IsNullOrWhiteSpace(lastName)) missingFields.Add("Last Name");
            if (string.IsNullOrWhiteSpace(employeeNumber)) missingFields.Add("Employee Number");
            if (string.IsNullOrWhiteSpace(email)) missingFields.Add("Email");
            if (string.IsNullOrWhiteSpace(gender)) missingFields.Add("Gender");
            if (string.IsNullOrWhiteSpace(branch)) missingFields.Add("Branch");
            if (string.IsNullOrWhiteSpace(employmentStatus)) missingFields.Add("Employment Status");
            if (string.IsNullOrWhiteSpace(department)) missingFields.Add("Department");

            // Check if datepickers have valid selections
            if (dob_calendar.SelectionStart.Date.Equals(DateTime.MinValue)) missingFields.Add("Date of Birth");
            if (startdate_calendar.SelectionStart.Date.Equals(DateTime.MinValue)) missingFields.Add("Start Date");

            // If there are missing fields, show the message with details
            if (missingFields.Any())
            {
                string missingFieldsMessage = "Please fill out the following fields and make necessary selections:\n\n" + string.Join("\n", missingFields);
                MessageBox.Show(missingFieldsMessage, "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method if validation fails
            }

            // If all fields are valid, proceed to insert staff
            InsertStaff(firstName, lastName, gender, dob, branch, employeeNumber, employmentStatus, department, email, startDate);

            // Reload staff data after adding new staff
            LoadStaffData();
        }



        private void InsertStaff(string firstName, string lastName, string gender, string dob, string branch,
                                 string employeeNumber, string employmentStatus, string department, string email, string startDate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Staff (FirstName, LastName, Gender, DOB, Branch, EmployeeNumber, 
                                          EmploymentStatus, Department, Email, StartDate) 
                        VALUES (@FirstName, @LastName, @Gender, @DOB, @Branch, @EmployeeNumber, 
                                @EmploymentStatus, @Department, @Email, @StartDate);";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@DOB", dob);
                        command.Parameters.AddWithValue("@Branch", branch);
                        command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                        command.Parameters.AddWithValue("@EmploymentStatus", employmentStatus);
                        command.Parameters.AddWithValue("@Department", department);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@StartDate", startDate);

                        if (command.ExecuteNonQuery() > 0)
                            MessageBox.Show("Staff added successfully!");
                        else
                            MessageBox.Show("Error adding staff.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting staff: " + ex.Message);
                }
            }
        }



        private void btn_delete_staff_Click(object sender, EventArgs e)
        {
            string employeeNumber = emp_num_txtbox.Text.Trim();
            if (string.IsNullOrEmpty(employeeNumber))
            {
                MessageBox.Show("Please enter Employee Number to delete.");
                return;
            }

            DeleteStaff(employeeNumber);
            // Reload staff data after adding new staff
            LoadStaffData();

        }

        private void DeleteStaff(string employeeNumber)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Staff WHERE EmployeeNumber = @EmployeeNumber;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);

                        if (command.ExecuteNonQuery() > 0)
                            MessageBox.Show("Staff deleted successfully!");
                        else
                            MessageBox.Show("No record found with this Employee Number.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting staff: " + ex.Message);
                }
            }
            btn_addnew_staff.Visible = true;
            ClearInputFields();
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            string searchTerm = search_txtbox.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter an Employee Number or search term.");
                return;
            }

            // Search for the staff by EmployeeNumber (or other field)
            SearchStaff(searchTerm);
        }



        private void SearchStaff(string searchTerm)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Staff WHERE EmployeeNumber = @EmployeeNumber;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeNumber", searchTerm);

                        using (SQLiteDataReader reader = command.ExecuteReader()) // Ensure reader is disposed
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    firstname_txtbox.Text = reader["FirstName"].ToString();
                                    lastname_txtbox.Text = reader["LastName"].ToString();
                                    rb_male.Checked = reader["Gender"].ToString() == "Male";
                                    rb_female.Checked = reader["Gender"].ToString() == "Female";
                                    dob_calendar.SelectionStart = DateTime.Parse(reader["DOB"].ToString());
                                    cb_branch.SelectedItem = reader["Branch"].ToString();
                                    emp_num_txtbox.Text = reader["EmployeeNumber"].ToString();
                                    cb_employ_status.SelectedItem = reader["EmploymentStatus"].ToString();
                                    cb_department.SelectedItem = reader["Department"].ToString();
                                    email_txtbox.Text = reader["Email"].ToString();
                                    startdate_calendar.SelectionStart = DateTime.Parse(reader["StartDate"].ToString());
                                }
                                btn_addnew_staff.Visible = false;
                                emp_num_txtbox.Enabled = false;
                                show_button_setA();
                            }
                            else
                            {
                                MessageBox.Show("No staff found with the provided Employee Number.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching staff: " + ex.Message);
                }
            }
        }


        private void search_txtbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a number or control key (e.g., Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                // If it's not a digit or Backspace, suppress the key press
                e.Handled = true;
            }
        }

        private void search_txtbox_Click(object sender, EventArgs e)
        {

            search_txtbox.Clear(); // Clear the text

        }
        private void search_payroll_txtbox_Click(object sender, EventArgs e)
        {
            search_payroll_txtbox.Clear();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            hidden_button_setA();
            ClearInputFields();
            LoadAttendanceForToday();
        }
        private void hidden_button_setA()
        {
            btn_reset.Visible = false;
            btn_updatestaff.Visible = false;
            btn_delete_staff.Visible = false;
        }
        private void show_button_setA()
        {
            btn_reset.Visible = true;
            btn_updatestaff.Visible = true;
            btn_delete_staff.Visible = true;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            hidden_button_setA();
            btn_addnew_staff.Visible = true;
            search_txtbox.Clear();
            ClearInputFields();
        }

        private void btn_clear_db_Click(object sender, EventArgs e)
        {
            // Confirm the action before clearing the database
            DialogResult result = MessageBox.Show("Are you sure you want to clear the entire database? This action cannot be undone.",
                                                  "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Proceed to clear the database
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete all records from both Staff and Attendance tables
                        string query = "DELETE FROM Staff; DELETE FROM Attendance;";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("All staff and attendance records have been cleared.");
                                LoadStaffData(); // Reload staff data (this can be modified to reload attendance data too)
                            }
                            else
                            {
                                MessageBox.Show("No records to clear.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error clearing database: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Database clear action was canceled.");
            }
        }
        private void LoadAttendanceForToday()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Get today's date (ignoring the time part)
                    string today = DateTime.Now.ToString("yyyy-MM-dd"); // Get today's date in the desired format

                    string query = @"
                SELECT EmployeeNumber, CheckInTime, CheckOutTime, Status
                FROM Attendance
                WHERE DATE(Attendance.Date) = @Today;  -- Use DATE() to ignore time part
            ";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Adding parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@Today", today);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Create a DataTable to hold the data
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            // Bind the DataTable to the DataGridView
                            dg_attendanceinfo.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading attendance data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        //Tab Payroll
        //
        //
        //
        //Tab Payroll
        //
        //
        //

        private void btn_payroll_search_Click(object sender, EventArgs e)
        {
            string searchTerm = search_payroll_txtbox.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter an Employee Number or search term.");
                return;
            }

            // Search for the staff by EmployeeNumber (or other field)
            SearchStaff2(searchTerm);
        }
        private void SearchStaff2(string searchTerm)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Staff WHERE EmployeeNumber = @EmployeeNumber;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeNumber", searchTerm);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    lbl_first_name.Text = reader["FirstName"].ToString();
                                    lbl_last_name.Text = reader["LastName"].ToString();
                                    lbl_branch.Text = reader["Branch"].ToString();
                                    lbl_department.Text = reader["Department"].ToString();
                                    lbl_emp_num.Text = reader["EmployeeNumber"].ToString();
                                    lbl_emp_status.Text = reader["EmploymentStatus"].ToString();
                                    lbl_email.Text = reader["Email"].ToString();
                                    lbl_start_date.Text = DateTime.Parse(reader["StartDate"].ToString()).ToShortDateString();
                                    lbl_leave.Text = reader["Leave"].ToString();
                                    leave_txtbox.Text = reader["Leave"].ToString();
                                    // Salary-related fields
                                    decimal basicSalary = reader["basic_salary"] != DBNull.Value ? Convert.ToDecimal(reader["basic_salary"]) : 0;
                                    decimal allowance = reader["allowance"] != DBNull.Value ? Convert.ToDecimal(reader["allowance"]) : 0;
                                    decimal grossSalary = reader["salary_gross"] != DBNull.Value ? Convert.ToDecimal(reader["salary_gross"]) : 0;
                                    decimal perkeso = reader["perkeso"] != DBNull.Value ? Convert.ToDecimal(reader["perkeso"]) : 0;

                                    string kwspStr = reader["kwsp"] != DBNull.Value ? reader["kwsp"].ToString() : "0";
                                    string zakatStr = reader["zakat"] != DBNull.Value ? reader["zakat"].ToString() : "0";

                                    decimal kwspPercentage = decimal.TryParse(kwspStr, out decimal kwspVal) ? kwspVal : 0;
                                    decimal zakatPercentage = decimal.TryParse(zakatStr, out decimal zakatVal) ? zakatVal : 0;

                                    // Assign values to UI elements
                                    lbl_salary.Text = basicSalary.ToString("N2");
                                    salary_txtbox.Text = basicSalary.ToString("N2");

                                    lbl_allowance.Text = allowance.ToString("N2");
                                    allowance_txtbox.Text = allowance.ToString("N2");

                                    lbl_gross.Text = grossSalary.ToString("N2");

                                    lbl_perkeso.Text = perkeso.ToString("N2");
                                    cb_perkeso.SelectedItem = perkeso.ToString();

                                    lbl_kwsp.Text = kwspPercentage.ToString();
                                    cb_kwsp.SelectedItem = kwspPercentage.ToString();

                                    lbl_zakat.Text = zakatPercentage.ToString();
                                    cb_zakat.SelectedItem = zakatPercentage.ToString();

                                    // **Calculate Deductions**
                                    decimal kwspAmount = basicSalary * (kwspPercentage / 100);
                                    decimal zakatAmount = grossSalary * (zakatPercentage / 100);

                                    // **Show calculated amounts**
                                    lbl_kwsp_amount.Text = kwspAmount.ToString("N2");
                                    lbl_perkeso_amount.Text = perkeso.ToString("N2");
                                    lbl_zakat_amount.Text = zakatAmount.ToString("N2");

                                    // Salary Net calculation (optional)
                                    decimal salaryNet = grossSalary - kwspAmount - perkeso - zakatAmount;
                                    lbl_net.Text = salaryNet.ToString("N2");
                                }
                            }
                            else
                            {
                                MessageBox.Show("No staff found with the provided Employee Number.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching staff: " + ex.Message);
                }
            }
        }


        private void btn_update_payroll_Click(object sender, EventArgs e)
        {
            string employeeNumber = lbl_emp_num.Text.Trim();

            if (string.IsNullOrEmpty(employeeNumber))
            {
                MessageBox.Show("No employee selected. Please search for an employee first.");
                return;
            }

            UpdatePayroll(employeeNumber);
            LoadStaffData();
        }

        private void UpdatePayroll(string employeeNumber)
        {
            try
            {
                // Parse input values
                decimal basicSalary = decimal.Parse(salary_txtbox.Text);
                decimal allowance = decimal.Parse(allowance_txtbox.Text);
                decimal leave = decimal.Parse(leave_txtbox.Text);
                decimal kwspPercentage = decimal.Parse(cb_kwsp.SelectedItem?.ToString() ?? "0");
                decimal perkeso = decimal.Parse(cb_perkeso.SelectedItem?.ToString() ?? "0");
                decimal zakatPercentage = decimal.Parse(cb_zakat.SelectedItem?.ToString() ?? "0");

                // Calculate SalaryGross
                decimal salaryGross = basicSalary + allowance;

                // Calculate deductions
                decimal kwspDeduction = basicSalary * (kwspPercentage / 100); // KWSP is a percentage of basic salary
                decimal zakatDeduction = salaryGross * (zakatPercentage / 100); // Zakat is a percentage of SalaryGross

                // Calculate SalaryNet
                decimal salaryNet = salaryGross - kwspDeduction - perkeso - zakatDeduction;

                // Update the database
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string updateQuery = @"
                UPDATE Staff 
                SET 
                    basic_salary = @BasicSalary,
                    allowance = @Allowance,
                    leave = @Leave,
                    kwsp = @KWSP,
                    perkeso = @Perkeso,
                    zakat = @Zakat,
                    salary_gross = @SalaryGross,
                    salary_net = @SalaryNet
                WHERE EmployeeNumber = @EmployeeNumber;";

                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                    {
                        // Add parameters with values from the form controls
                        command.Parameters.AddWithValue("@BasicSalary", basicSalary);
                        command.Parameters.AddWithValue("@Allowance", allowance);
                        command.Parameters.AddWithValue("@Leave", leave);
                        command.Parameters.AddWithValue("@KWSP", kwspPercentage);
                        command.Parameters.AddWithValue("@Perkeso", perkeso);
                        command.Parameters.AddWithValue("@Zakat", zakatPercentage);
                        command.Parameters.AddWithValue("@SalaryGross", salaryGross);
                        command.Parameters.AddWithValue("@SalaryNet", salaryNet);
                        command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Payroll details updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the Employee Number.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payroll details: " + ex.Message);
            }
            finally
            {
                ClearInputFields2();
            }
        }
        private void ClearInputFields2()
        {
            // Clear all input fields and labels
            lbl_first_name.Text = string.Empty;
            lbl_last_name.Text = string.Empty;
            lbl_branch.Text = string.Empty;
            lbl_department.Text = string.Empty;
            lbl_emp_num.Text = string.Empty;
            lbl_emp_status.Text = string.Empty;
            lbl_email.Text = string.Empty;
            lbl_start_date.Text = string.Empty;

            lbl_salary.Text = "N/A";
            salary_txtbox.Text = string.Empty;

            lbl_allowance.Text = "N/A";
            allowance_txtbox.Text = string.Empty;

            lbl_leave.Text = "N/A";
            leave_txtbox.Text = string.Empty;

            lbl_kwsp.Text = "N/A";
            cb_kwsp.SelectedItem = null;

            lbl_perkeso.Text = "N/A";
            cb_perkeso.SelectedItem = null;

            lbl_zakat.Text = "N/A";
            cb_zakat.SelectedItem = null;

            lbl_gross.Text = "N/A";
            lbl_net.Text = "N/A";
        }

        private void btn_add_image_Click(object sender, EventArgs e)
        {
            // Open file dialog to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the image into the PictureBox
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void lbl_signout_Click(object sender, EventArgs e)
        {
            // Hide the MainForm2 to log out the user
            this.Hide();

            // Create a new instance of LoginPage
            LoginPage loginPageForm = new LoginPage();

            // Show the LoginPage
            loginPageForm.Show();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hide the MainForm2 to log out the user
            this.Hide();

            // Create a new instance of LoginPage
            LoginPage loginPageForm = new LoginPage();

            // Show the LoginPage
            loginPageForm.Show();
        }
    }
}
