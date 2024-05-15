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
    /// Логика взаимодействия для AddNewResultWindow.xaml
    /// </summary>
    public partial class AddNewResultWindow : Window
    {
        public AddNewResultWindow()
        {
            InitializeComponent();
        }

        private void AddResult_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection myConnection = new SqlConnection(MainWindow.ConnectionString))
            {
                string QuestionAddCommand = "INSERT INTO Asknowledge(TariffName,PlanType,InternetNeeds,ChatActivity," +
                    "MinutesNeeds,MessagesNeeds,SubscribeAvaible,IoTType,ShareAvaible)" +
                    " VALUES ('" + TariffName.Text + "','" + PlanType.Text + "','" +
                    InternetNeeds.Text + "','" + ChatActivity.Text + "','" +
                    MinutesNeeds.Text + "','" + MessagesNeeds.Text + "','" +
                    SubscribeAvaible.Text + "','" + IoTType.Text + "','" +
                    ShareAvaible.Text + "')";
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
