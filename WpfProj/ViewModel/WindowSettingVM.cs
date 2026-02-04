using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfProj.Model;
using WpfProj.MVVM;

namespace WpfProj.ViewModel
{
    public class WindowSettingVM : ViewModelBase
    {
        private int _width = 50;
        private int _height = 100;



        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }        

        public List<Person> NameList { get; set; } = new List<Person>()
            {
                new Person() { FirstName = "John", LastName = "Doe" },
                new Person() { FirstName = "Jane", LastName = "Smith" },
                new Person() { FirstName = "Michael", LastName = "Johnson" }
            };
    }
}
