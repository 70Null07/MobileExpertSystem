using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MobileExpertSystem
{
    /// <summary>
    /// Логика взаимодействия для AddNewRuleWindow.xaml
    /// </summary>
    public partial class AddNewRuleWindow : Window
    {
        public AddNewRuleWindow()
        {
            InitializeComponent();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection myConnection = new SqlConnection(MainWindow.ConnectionString))
            {
                string QuestionAddCommand = "INSERT INTO Rules(_Rule,Result) VALUES ('" + Rule.Text + " " + RuleChoise.Text + "','" +
                    Result.Text + "')";
                using (SqlDataAdapter myDataAdapter = new SqlDataAdapter(QuestionAddCommand, myConnection))
                {
                    DataTable dtResult = new DataTable();
                    myDataAdapter.Fill(dtResult);
                }
            }
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
