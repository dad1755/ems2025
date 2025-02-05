using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace EmployeeManagementSystem
{
    public partial class MainForm2 : Form
    {
        private string ConnectionString = "Data Source=StaffDB.sqlite;Version=3;";
        private string loggedInEmail;

        // Constructor to accept email
        public MainForm2(string email)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            loggedInEmail = email;  // Store logged-in email
        }

        private void MainForm2_Load(object sender, EventArgs e)
        {
            FetchEmployeeDetails(loggedInEmail);  // Fetch details when form loads
            LoadContacts(); // Load Email & First Name in DataGridView
            CheckAttendanceStatus(); // Check if user already checked in
            LoadAttendanceHistory(); // Load last 30 days of attendance
            FetchLeaveData(loggedInEmail);
        }

        private void FetchEmployeeDetails(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Staff WHERE LOWER(Email) = LOWER(@Email);";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

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
                                }
                            }
                            else
                            {
                                MessageBox.Show("Employee details not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching employee details: " + ex.Message);
                }
            }
        }
        private void FetchLeaveData(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Updated query to include the 'Status' column
                    string query = "SELECT LeaveType, StartDate, EndDate, TotalDays, Description, Status FROM Leave WHERE LOWER(Email) = LOWER(@Email);";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Set the fetched data to the DataGridView
                            dg_leave.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching leave data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadContacts()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Email, FirstName FROM Staff";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);  // Load data into DataTable
                            dg_contact.DataSource = dt; // Bind DataTable to DataGridView
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading contacts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Check if user has already checked in
        private void CheckAttendanceStatus()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CheckInTime, CheckOutTime FROM Attendance WHERE Email = @Email AND Date = @Date";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", loggedInEmail);
                        command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string checkIn = reader["CheckInTime"]?.ToString();
                                string checkOut = reader["CheckOutTime"]?.ToString();

                                if (!string.IsNullOrEmpty(checkIn) && string.IsNullOrEmpty(checkOut))
                                {
                                    // Check-In exists, but no Check-Out, so enable "Check-Out" button
                                    btn_check_in.Enabled = false;
                                    btn_check_out.Enabled = true;
                                }
                                else if (!string.IsNullOrEmpty(checkOut))
                                {
                                    // Both Check-In and Check-Out are already recorded, disable both buttons
                                    btn_check_in.Enabled = false;
                                    btn_check_out.Enabled = false;
                                }
                                else
                                {
                                    // No Check-In recorded, enable "Check-In" button
                                    btn_check_in.Enabled = true;
                                    btn_check_out.Enabled = false;
                                }
                            }
                            else
                            {
                                // No record for today, enable "Check-In" button
                                btn_check_in.Enabled = true;
                                btn_check_out.Enabled = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking attendance status: " + ex.Message);
                }
            }
        }



        // Attendance Check-Out
        // Load Last 30 Days Attendance
        private void LoadAttendanceHistory()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Get the start date (1st of the current month) and today's date
                    DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime today = DateTime.Now.Date;

                    // Create a list of all dates from start of month to today
                    List<DateTime> allDatesInMonth = new List<DateTime>();
                    for (DateTime date = startOfMonth; date <= today; date = date.AddDays(1))
                    {
                        allDatesInMonth.Add(date);
                    }

                    // Create a DataTable to store the results
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Date", typeof(DateTime));
                    dt.Columns.Add("CheckInTime", typeof(string));
                    dt.Columns.Add("CheckOutTime", typeof(string));
                    dt.Columns.Add("TotalHoursWorked", typeof(string));
                    dt.Columns.Add("Status", typeof(string));

                    int workDaysCount = 0; // Start counting worked days

                    // Loop through all the days from start of month to today
                    foreach (var date in allDatesInMonth)
                    {
                        string query = @"
                SELECT Date, CheckInTime, CheckOutTime, Status 
                FROM Attendance 
                WHERE Email = @Email 
                AND Date = @Date";

                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Email", loggedInEmail);
                            command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string checkInTime = reader["CheckInTime"].ToString();
                                    string checkOutTime = reader["CheckOutTime"].ToString();
                                    string status = reader["Status"].ToString();
                                    string totalHoursWorked = "N/A";

                                    // Calculate Total Hours Worked
                                    if (!string.IsNullOrEmpty(checkInTime) && !string.IsNullOrEmpty(checkOutTime) &&
                                        checkInTime != "N/A" && checkOutTime != "N/A")
                                    {
                                        DateTime checkIn = DateTime.Parse(checkInTime);
                                        DateTime checkOut = DateTime.Parse(checkOutTime);
                                        TimeSpan totalHours = checkOut - checkIn;
                                        totalHoursWorked = totalHours.ToString(@"hh\:mm");
                                    }

                                    // Count workdays (assuming "Present" or valid CheckIn means worked)
                                    if (status == "Present" || (!string.IsNullOrEmpty(checkInTime) && checkInTime != "N/A"))
                                    {
                                        workDaysCount++;
                                    }

                                    // Add data to DataTable
                                    dt.Rows.Add(
                                        DateTime.Parse(reader["Date"].ToString()),
                                        checkInTime,
                                        checkOutTime,
                                        totalHoursWorked,
                                        status);
                                }
                                else
                                {
                                    // If no attendance exists, treat it as "Absent"
                                    dt.Rows.Add(date, "N/A", "N/A", "N/A", "Absent");
                                }
                            }
                        }
                    }

                    // Update the label with the number of worked days
                    lbl_daywork.Text = workDaysCount.ToString();

                    // Sort DataTable to show today's attendance at the top
                    DataView dv = dt.DefaultView;
                    dv.Sort = "Date DESC"; // Sort in descending order
                    dg_attendance.DataSource = dv.ToTable(); // Bind sorted DataTable

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading attendance history: " + ex.Message);
                }
            }
        }




        // Logout
        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }

        // Close application
        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_check_in_Click_1(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Attendance (Date, Email, EmployeeNumber, CheckInTime, Status) VALUES (@Date, @Email, @EmployeeNumber, @CheckInTime, 'Present')";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Email", loggedInEmail);
                        command.Parameters.AddWithValue("@EmployeeNumber", lbl_emp_num.Text);
                        command.Parameters.AddWithValue("@CheckInTime", DateTime.Now.ToString("HH:mm:ss"));

                        command.ExecuteNonQuery();
                    }

                    // Disable Check-In and enable Check-Out after successful check-in
                    btn_check_in.Enabled = false;
                    btn_check_out.Enabled = true;

                    LoadAttendanceHistory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during check-in: " + ex.Message);
                }
            }
        }


        private void btn_check_out_Click_1(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE Attendance SET CheckOutTime = @CheckOutTime WHERE Email = @Email AND Date = @Date";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Email", loggedInEmail);
                        command.Parameters.AddWithValue("@CheckOutTime", DateTime.Now.ToString("HH:mm:ss"));

                        command.ExecuteNonQuery();
                    }

                    // Disable both Check-In and Check-Out after successful check-out
                    btn_check_in.Enabled = false;
                    btn_check_out.Enabled = false;

                    LoadAttendanceHistory();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during check-out: " + ex.Message);
                }
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

        private void btn_submitleave_Click(object sender, EventArgs e)
        {
            SubmitLeaveRequest();
            loadLeaveData(loggedInEmail);  // Load the leave data after submission
        }

        private void loadLeaveData(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to fetch Leave data including the 'Status' column
                    string query = "SELECT LeaveType, StartDate, EndDate, TotalDays, Description, Status FROM Leave WHERE LOWER(Email) = LOWER(@Email);";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Set the fetched data to the DataGridView
                            dg_leave.DataSource = dt;

                            // Optional: Customize column headers
                            dg_leave.Columns["LeaveType"].HeaderText = "Leave Type";
                            dg_leave.Columns["StartDate"].HeaderText = "Start Date";
                            dg_leave.Columns["EndDate"].HeaderText = "End Date";
                            dg_leave.Columns["TotalDays"].HeaderText = "Total Days";
                            dg_leave.Columns["Description"].HeaderText = "Description";
                            dg_leave.Columns["Status"].HeaderText = "Status";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching leave data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SubmitLeaveRequest()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Assuming the form controls are populated with the correct values
                    string leaveType = cb_leavetype.SelectedItem.ToString();
                    string startDate = dt_start.Value.ToString("yyyy-MM-dd");
                    string endDate = dt_end.Value.ToString("yyyy-MM-dd");
                    int totalDays = (dt_end.Value - dt_start.Value).Days;  // Calculate total days
                    string description = descripleave_txtbox.Text;
                    string email = loggedInEmail;  // Use the logged-in user's email
                    string filePath = "";  // Assuming no file attachment or handle file blob accordingly

                    string insertQuery = @"
INSERT INTO Leave (EmployeeNumber, Email, LeaveType, StartDate, EndDate, TotalDays, Description, File, Status)
VALUES (@EmployeeNumber, @Email, @LeaveType, @StartDate, @EndDate, @TotalDays, @Description, @File, 'Pending');
";

                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeNumber", "12345");  // Replace with actual employee number
                        command.Parameters.AddWithValue("@Email", email);  // Use loggedInEmail
                        command.Parameters.AddWithValue("@LeaveType", leaveType);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@TotalDays", totalDays);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@File", DBNull.Value);  // Replace with actual file if needed

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Leave request submitted successfully. Status is 'Pending' until admin approval.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error submitting leave request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

