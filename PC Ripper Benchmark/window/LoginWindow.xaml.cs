﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace PC_Ripper_Benchmark.window {

    /// <summary>
    /// Interaction logic for <see cref="LoginWindow"/>
    /// Inherits from Window base class to have properties of a windows
    /// <para>Author: David Hartglass</para>
    /// </summary>

    public partial class LoginWindow : Window {

        /// <summary>
        /// Default constructor for <see cref="LoginWindow"/>
        /// </summary>

        public LoginWindow() {
            InitializeComponent();

            //Change the progressbar visibilty to not show on screen
            this.database_progressbar.Visibility = Visibility.Collapsed;

            CenterWindowOnScreen();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\sqle2012; Initial Catalog=LoginDB; Integrated Security=True;");
            try {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                string query = "SELECT COUNT(1) FROM tblUser WHERE Username=@Username AND Password=@Password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon) {
                    CommandType = CommandType.Text
                };
                sqlCmd.Parameters.AddWithValue("@Username", this.txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", this.txtPassword.Password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1) {
                    //MainWindow dashboard = new MainWindow();
                    //dashboard.Show();
                    Close();
                } else {
                    MessageBox.Show("Username or password is incorrect.");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            } finally {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// Centers the <see cref="LoginWindow"/>
        /// on the screen.
        /// </summary>

        private void CenterWindowOnScreen() {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;

            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
