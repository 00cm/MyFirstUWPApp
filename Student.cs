using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstUWPApp
{
    public class Student : INotifyPropertyChanged
    {
        private string _fName;
        private string _lName;
        private string _className;
        private string _grade;
        public string FirstName
        {
            get => _fName;
            set
            {
                if (_fName != value)
                {
                    _fName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string LastName
        {
            get => _lName;
            set
            {
                if (_lName != value)
                {
                    _lName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ClassName
        {
            get => _className;
            set
            {
                if (_className != value)
                {
                    _className = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Id { get; set; }
        public string Grade
        {
            get => _grade;
            set
            {
                if (_grade != value)
                {
                    _grade = value;
                    OnPropertyChanged();
                }
            }
        }


        public Student(string firstName, string lastName, string className, string grade, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            ClassName = className;
            Grade = grade;
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}, {Grade}";
        }
    }
}
