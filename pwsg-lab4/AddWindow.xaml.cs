using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pwsg_lab4
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        private int phone;
        public int Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
                NotifyPropertyChanged(value.ToString());
            }
        }

        public Person person = null;
        public AddWindow()
        {
            InitializeComponent();
            SexComboBox.ItemsSource = Enum.GetValues(typeof(SexEnum)).Cast<SexEnum>();
            SexComboBox.SelectedValue = SexEnum.Male;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            person = new Person(NameTextBox.Text, LastNameTextBox.Text, Int32.Parse(PhoneTextBox.Text), (SexEnum)SexComboBox.SelectedItem, DateBox.SelectedDate.Value.ToShortDateString(), CityTextBox.Text);
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void phone_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string text = textbox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
                //var parsingOk = !regex.IsMatch(text);
                if (regex.IsMatch(text))
                {
                    textbox.Foreground = Brushes.Red;
                    textbox.BorderBrush = Brushes.Red;
                    redcross.Visibility = Visibility.Visible;
                }
                else
                {
                    textbox.Foreground = Brushes.Black;
                    textbox.BorderBrush = Brushes.Gray;
                    redcross.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
