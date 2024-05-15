using System;
using System.Collections.Generic;
using System.Data.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Permissions;

namespace MobileExpertSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string ConnectionString = "Data Source=localhost;Initial Catalog=Database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        private static string RulesCommand = "SELECT Result FROM Rules WHERE _Rule='Empty'";

        private static string ResultCommand = "SELECT * FROM Asknowledge WHERE";

        private bool firstInline = true;

        public MainWindow()
        {
            InitializeComponent();

            Update();
        }

        public void Update()
        {
            QuestionsListView.Items.Clear();
            AnswersListView.Items.Clear();
            ResultsListView.Items.Clear();

            TextBlock questions = new TextBlock(), answers = new TextBlock(), results = new TextBlock();
            questions.Text = "Вопросы";
            answers.Text = "Ответы";
            results.Text = "Результаты";
            QuestionsListView.Items.Add(questions);
            AnswersListView.Items.Add(answers);
            ResultsListView.Items.Add(results);

            string Command = "Select * FROM Questions";

            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, myConnection))
                {
                    DataTable dtResult = new DataTable();
                    myDataAdapter.Fill(dtResult);

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        DataRow row = dtResult.Rows[i];

                        TextBlock tx = new TextBlock();
                        tx.Text = row[1].ToString();
                        QuestionsListView.Items.Add(tx);

                        Grid answerGrid = new Grid();
                        answerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        answerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        answerGrid.ColumnDefinitions.Add(new ColumnDefinition());

                        CheckBox answerCheckBox = new CheckBox();
                        answerCheckBox.Content = row[2].ToString();
                        answerCheckBox.TabIndex = i;
                        answerCheckBox.Checked += AnswerCheckBox_Checked;
                        answerCheckBox.Unchecked += AnswerCheckBox_Unchecked;

                        answerGrid.Children.Add(answerCheckBox);
                        Grid.SetColumn(answerCheckBox, 0);

                        CheckBox answerCheckBox2 = new CheckBox();
                        answerCheckBox2.Content = row[3].ToString();
                        answerCheckBox2.TabIndex = i;
                        answerCheckBox2.Checked += AnswerCheckBox_Checked;
                        answerCheckBox2.Unchecked += AnswerCheckBox_Unchecked;

                        answerGrid.Children.Add(answerCheckBox2);
                        Grid.SetColumn(answerCheckBox2, 1);

                        if (row[4].ToString() != "")
                        {
                            CheckBox answerCheckBox3 = new CheckBox();
                            answerCheckBox3.Content = row[4].ToString();
                            answerCheckBox3.TabIndex = i;
                            answerCheckBox3.Checked += AnswerCheckBox_Checked;
                            answerCheckBox3.Unchecked += AnswerCheckBox_Unchecked;
                            answerGrid.Children.Add(answerCheckBox3);
                            Grid.SetColumn(answerCheckBox3, 2);
                        }

                        AnswersListView.Items.Add(answerGrid);
                    }
                }
            }
        }

        private void AnswerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ResultCommand = "SELECT * FROM Asknowledge WHERE";
            RulesCommand = "SELECT Result FROM Rules WHERE _Rule='Empty'";

            firstInline = true;

            ResultsListView.Items.Clear();

            TextBlock tx = new TextBlock();
            tx.Text = "Результаты";
            ResultsListView.Items.Add(tx);
        }

        private void AnswerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox t = sender as CheckBox;

            var question = QuestionsListView.Items[t.TabIndex + 1] as TextBlock;

            RulesCommand += " OR _Rule='" + question.Text + " " + t.Content + "'";

            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter myDataAdapter = new SqlDataAdapter(RulesCommand, myConnection))
                {
                    DataTable dtResult = new DataTable();
                    myDataAdapter.Fill(dtResult);

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        DataRow row = dtResult.Rows[i];

                        TextBlock tx = new TextBlock();
                        tx.Text = row[0].ToString();

                        if (firstInline)
                        {
                            ResultCommand += " CONTAINS(*, '\"" + tx.Text + "\"')";
                            firstInline = false;
                        }
                        else if (!char.IsDigit(t.Content.ToString(), 0))
                            ResultCommand += " AND CONTAINS(*, '\"" + tx.Text + "\"')"; // OR
                        else
                            ResultCommand += " AND CONTAINS(*, '\"" + tx.Text + "\"')";
                    }
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(ResultCommand, myConnection))
                    {
                        DataTable dtResult2 = new DataTable();
                        sqlDataAdapter.Fill(dtResult2);

                        ResultsListView.Items.Clear();

                        for (int i2 = 0; i2 < dtResult2.Rows.Count; i2++)
                        {
                            DataRow dataRow2 = dtResult2.Rows[i2];

                            TextBlock tx2 = new TextBlock();
                            tx2.Text += dataRow2[1].ToString();

                            ResultsListView.Items.Add(tx2);
                        }
                    }

                    //ResultCommand = "SELECT * FROM Asknowledge WHERE CONTAINS(*, '\"Empty\"')";

                    //AnswersListView.Items.Clear();

                    //AnswersListView.Items.Add(tx);
                }
            }
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewQuestionWindow addNewQuestionWindow = new AddNewQuestionWindow();
            addNewQuestionWindow.ShowDialog();
            Update();
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewRuleWindow addNewRuleWindow = new AddNewRuleWindow();
            addNewRuleWindow.ShowDialog();
        }

        private void AddResultButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewResultWindow addNewResultWindow = new AddNewResultWindow();
            addNewResultWindow.ShowDialog();
        }
    }
}
