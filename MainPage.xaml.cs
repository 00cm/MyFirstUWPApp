using MyFirstUWPApp.DataProvider;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Threading.Tasks;


namespace MyFirstUWPApp
{

    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Student> Students { get; } = new ObservableCollection<Student>();
        private StudentDataProvider _studentDataProvider;
        public MainPage()
        {
            this.InitializeComponent();
            App.Current.Suspending += App_Suspending;
            _studentDataProvider = new StudentDataProvider();

            studentList.ItemsSource = Students;
        }

        private async void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await _studentDataProvider.SaveStudentAsync(
                Students.ToList());
            deferral.Complete();
        }

        private async void ButtonLoadStudent_Click(object sender, RoutedEventArgs e)
        {
            Students.Clear();
            var students = await _studentDataProvider.LoadStudentAsync();

            foreach (var student in students)
            {
                Students.Add(student);
            }


        }

        private async void ButtonSaveStudents_Click(object sender, RoutedEventArgs e)
        {
            await _studentDataProvider.SaveStudentAsync(
                Students.ToList());
        }

        private async void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var id = new Random().Next();

            var student = new Student(
                studentFirstName.Text.Trim(),
                studentLastName.Text.Trim(),
                studentClass.Text.Trim(),
                Grade.SelectedItem as string ?? string.Empty,
                id);
            if (student != null)
            {
                Students.Add(student);
                studentList.SelectedItem = student;
                var dialog = new MessageDialog("Student added!");
                await dialog.ShowAsync();
            }


        }

        private async void ButtonDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            var student = studentList.SelectedItem as Student;
            if (student != null)
            {
                Students.Remove(student);
            }

            var dialog = new MessageDialog("Student deleted!");
            await dialog.ShowAsync();
        }

        private void OnStudent_Select(object sender, SelectionChangedEventArgs e)
        {
            var student = studentList.SelectedItem as Student;
            if (student != null)
            {
                fNameTxt.Text = student?.FirstName ?? "";
                lNameTxt.Text = student?.LastName ?? "";
                classNameTxt.Text = student?.ClassName ?? "";
                gradeTxt.Text = student?.Grade ?? "";
            }
        }

        private void Grade_Selection(object sender, SelectionChangedEventArgs e)
        {
            if (studentList.SelectedItem is Student student)
            {
                student.Grade = (string)Grade.SelectedItem;

                var selectedStudent = studentList.SelectedItem;
                studentList.SelectedItem = null;
                studentList.SelectedItem = selectedStudent;
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateStudent();
        }

        private void UpdateStudent()
        {
            var student = studentList?.SelectedItem as Student;
            if (student != null)
            {
                student.FirstName = studentFirstName.Text;
                student.LastName = studentLastName.Text;
                student.ClassName = studentClass.Text;
            }
        }
    }
}
