using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zavin.Slideshow.wpf
{
    /// <summary>
    /// Interaction logic for WeekGraphPage.xaml
    /// </summary>
    public partial class WeekGraphPage : Page
    {
        public WeekGraphPage()
        {
            InitializeComponent();
        }

        MainController mainController = new MainController();

        private void BarGraphAnimations()
        {

            // Set animation on Bar Graph upon loading of the window.

            //Animation for Production.
            DoubleAnimation moveAnimation = new DoubleAnimation();
            moveAnimation.From = -400;
            moveAnimation.To = ActualHeight;
            moveAnimation.Duration = TimeSpan.FromMilliseconds(6000);
            BarSeriesProductie.BeginAnimation(Canvas.HeightProperty, moveAnimation);

            //Animation for Aanvoer.
            DoubleAnimation moveAnimation2 = new DoubleAnimation();
            moveAnimation2.From = -400;
            moveAnimation2.To = ActualHeight;
            moveAnimation2.Duration = TimeSpan.FromMilliseconds(6000);
            BarSeriesAanvoer.BeginAnimation(Canvas.HeightProperty, moveAnimation2);


        }

        private void WeekGraphPage1_Loaded(object sender, RoutedEventArgs e)
        {
            BarGraphAnimations();

            ((ColumnSeries)MainChart.Series[0]).ItemsSource = mainController.GetProduction();

            ((ColumnSeries)MainChart.Series[1]).ItemsSource = mainController.GetAcaf();
        }

        // Standard data object representing a Student
        public class Student : INotifyPropertyChanged
        {
            // Student's name
            public string Name { get; private set; }

            // Student's favorite color
            public Brush FavoriteColor { get; private set; }

            // Student's grade
            public double Grade
            {
                get { return _grade; }
                set
                {
                    _grade = value;
                    Helpers.InvokePropertyChanged(PropertyChanged, this, "Grade");
                }
            }
            private double _grade;

            // Student constructor
            public Student(string name, Brush favoriteColor)
            {
                Name = name;
                FavoriteColor = favoriteColor;
            }

            // INotifyPropertyChanged event
            public event PropertyChangedEventHandler PropertyChanged;
        }

        // Custom data object to wrap a Student object for the view model
        public class WeekGraphModel : INotifyPropertyChanged
        {
            // Student object
            public Student Student { get; private set; }

            // Color representing Student's Grade
            public Brush GradeColor { get; private set; }

            // StudentViewModel constructor
            public WeekGraphModel(Student student)
            {
                Student = student;
                student.PropertyChanged += new PropertyChangedEventHandler(HandleStudentPropertyChanged);
            }

            // Detect changes to the Student's grade and update GradeColor
            void HandleStudentPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if ("Grade" == e.PropertyName)
                {
                    Console.WriteLine("Grade has a value for some reason");
                    if (Student.Grade < 50)
                    {
                        GradeColor = new SolidColorBrush { Color = Colors.Red };
                    }
                    else if (Student.Grade < 80)
                    {
                        GradeColor = new SolidColorBrush { Color = Colors.Yellow };
                    }
                    else
                    {
                        GradeColor = new SolidColorBrush { Color = Colors.Green };
                    }
                    Helpers.InvokePropertyChanged(PropertyChanged, this, "GradeColor");
                }
            }

            // INotifyPropertyChanged event
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
