using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для AddNewQuestionWindow.xaml
    /// </summary>
    public partial class AddNewQuestionWindow : Window
    {
        public AddNewQuestionWindow()
        {
            InitializeComponent();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection myConnection = new SqlConnection(MainWindow.ConnectionString))
            {
                string QuestionAddCommand = "INSERT INTO Questions(Asknowledge,Result1,Result2,Result3) VALUES (" + Question.Text + "," +
                    Answer1.Text + "," + Answer2.Text + "," + Answer3.Text + ")";
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
