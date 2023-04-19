using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuizProject.Models;
using static System.Formats.Asn1.AsnWriter;

namespace QuizProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal AccountModel account;
        internal MyDBContext context;
        internal QuestionModel[] questions;
        private MyDataContext myDataContext;

        internal QuestionModel currentQuestion;
        internal int currentId = 0;






        internal MainWindow(AccountModel account, MyDBContext context)
        {
            InitializeComponent();
            this.context = context;
            this.account = account;
            questions = context.Questions.ToArray();
            //currentQuestion = new ObservableCollection<QuestionModel>(question);
            myDataContext = new MyDataContext {
                Accounts = context.Accounts.OrderByDescending(a => a.Record).Take(10).ToArray(),
             CurrentQuestion = questions[currentId],
                Score = 0
                
            };
            DataContext = myDataContext;
            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start.Visibility = Visibility.Collapsed;
            QuizGrid.Visibility = Visibility.Visible;

        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            ColorRightButton();
            if (questions[currentId].Answer) myDataContext.Score++;
            NextBtn.Visibility = Visibility.Visible;
        }
        private void No_Click(object sender, RoutedEventArgs e)
        {
            ColorRightButton();
            if (!questions[currentId].Answer) myDataContext.Score++;
            NextBtn.Visibility = Visibility.Visible;
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            currentId++;
            if(currentId == questions.Length)
            {
                if (myDataContext.Score > account.Record)
                {
                    
                    account.Record = myDataContext.Score;
                    context.Entry(account).State = EntityState.Modified;
                    context.SaveChanges();


                    myDataContext.Accounts = context.Accounts.OrderByDescending(a => a.Record).Take(10).ToArray();
                }
                QuizGrid.Visibility= Visibility.Collapsed;
                ScoreboardGrid.Visibility= Visibility.Visible;
            }
            else
            {
                myDataContext.CurrentQuestion = questions[currentId];
                YesBtn.Background = (Brush)bc.ConvertFrom("#50577A");
                NoBtn.Background = (Brush)bc.ConvertFrom("#50577A");
            }
           
            NextBtn.Visibility=Visibility.Collapsed;
            
        }
        private void ColorRightButton()
        {
            var bc = new BrushConverter();
            if (!questions[currentId].Answer){NoBtn.Background = (Brush)bc.ConvertFrom("#660010");}
            else { YesBtn.Background = (Brush)bc.ConvertFrom("#11680B"); }

        }

        private class MyDataContext : INotifyPropertyChanged
        {
            // Define private fields for properties
            private int _score;
            private AccountModel[] _accounts;
            private QuestionModel _currentQuestion;

            // Define public properties that raise the PropertyChanged event
            public int Score
            {
                get { return _score; }
                set
                {
                    if (_score != value)
                    {
                        _score = value;
                        OnPropertyChanged(nameof(Score));
                    }
                }
            }

            public AccountModel[] Accounts
            {
                get { return _accounts; }
                set
                {
                    if (_accounts != value)
                    {
                        _accounts = value;
                        OnPropertyChanged(nameof(Accounts));
                    }
                }
            }

            public QuestionModel CurrentQuestion
            {
                get { return _currentQuestion; }
                set
                {
                    if (_currentQuestion != value)
                    {
                        _currentQuestion = value;
                        OnPropertyChanged(nameof(CurrentQuestion));
                    }
                }
            }

            // Define the PropertyChanged event
            public event PropertyChangedEventHandler PropertyChanged;

            // Define the OnPropertyChanged method to raise the PropertyChanged event
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
