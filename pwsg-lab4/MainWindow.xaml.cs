using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization;

namespace pwsg_lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Person> peoplecollection;


        public MainWindow()
        {
            InitializeComponent();
            peoplecollection = new ObservableCollection<Person>();
            PeopleCollectionView.DataContext = peoplecollection;
            PeopleCollectionEdit.DataContext = peoplecollection;
            CollectionViewSource.GetDefaultView(PeopleCollectionView.DataContext).Filter = PeopleFilter;
        }

        private bool PeopleFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Person).Name.IndexOf(txtFilter.Text) >= 0 || (item as Person).LastName.IndexOf(txtFilter.Text) >= 0 || (item as Person).City.IndexOf(txtFilter.Text) >= 0 || (item as Person).Phone.ToString().IndexOf(txtFilter.Text) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PeopleCollectionView.DataContext).Refresh();
        }

        private void Addcontacts_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                StringBuilder lastname = new StringBuilder("Smith");
                lastname.Append(i.ToString());
                peoplecollection.Add(new Person("John", lastname.ToString(), 123456789,SexEnum.Male,"01-01-1990","Warszawa"));
            }
        }

        private void Addcontact_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            AddWindow addwindow = new AddWindow();
            addwindow.ShowDialog();
            if (addwindow.person != null)
                peoplecollection.Add(addwindow.person);
            this.Opacity = 1;
        }
        private void Removecontact_Click(object sender, RoutedEventArgs e)
        {
            if ((MyTabControl.SelectedItem as TabItem).Header as string == "View mode")
            {
                Person person = (Person)PeopleCollectionView.SelectedItem;
                peoplecollection.Remove(person);
            }
            else
            {
                Person person = (Person)PeopleCollectionEdit.SelectedItem;
                peoplecollection.Remove(person);
            }
            PersonImage.Source = null;
            NameLabel.Visibility = Visibility.Hidden;
            LastNameLabel.Visibility = Visibility.Hidden;
            DateLabel.Visibility = Visibility.Hidden;
            PhoneLabel.Visibility = Visibility.Hidden;
            CityLabel.Visibility = Visibility.Hidden;
        }
        private void Importcontacts_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "XML (*.xml)|*.xml";
            if (dlg.ShowDialog() == true)
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<Person>));
                System.IO.Stream s = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Open);
                ObservableCollection<Person> tmp = (ObservableCollection<Person>)xs.Deserialize(s);
                foreach (Person p in tmp)
                    peoplecollection.Add(p);
                s.Close();
            }
        }
        private void Exportcontacts_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "XML (*.xml)|*.xml";
            dlg.ShowDialog();
            if (dlg.FileName != "")
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<Person>));
                System.IO.Stream s = System.IO.File.Create(dlg.FileName);
                xs.Serialize(s, peoplecollection);
                s.Close();
            }
        }
        private void Image_Click(object sender, RoutedEventArgs e)
        {
            Person person;
            if ((MyTabControl.SelectedItem as TabItem).Header as string == "View mode")
            {
                if (PeopleCollectionView.SelectedItem == null)
                {
                    MessageBox.Show("Choose a person first");
                    return;
                }
                person = (Person)PeopleCollectionView.SelectedItem;
            }
            else
            {
                if (PeopleCollectionEdit.SelectedItem == null)
                {
                    MessageBox.Show("Choose a person first");
                    return;
                }
                person = (Person)PeopleCollectionEdit.SelectedItem;
            }
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg"; 
            if (dlg.ShowDialog() == true)
            {
                person.ImageSource = dlg.FileName;
                PersonImage.Source = new BitmapImage(new Uri(person.ImageSource,UriKind.RelativeOrAbsolute));
            }
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                Person person = (Person)item.DataContext;
                ShowPerson(person);
            }
        }

        private void DataGridRow_Selected(object sender, RoutedEventArgs e)
        {
            DataGridRow item = sender as DataGridRow;
            if (item != null)
            {
                Person person = (Person)item.DataContext;
                ShowPerson(person);
            }
        }
        public void ShowPerson(Person person)
        {
            if (person.ImageSource == null || !System.IO.File.Exists(person.ImageSource))
            {
                if (person.Sex == SexEnum.Male)
                    PersonImage.Source = new BitmapImage(new Uri("./man.png", UriKind.RelativeOrAbsolute));
                else
                    PersonImage.Source = new BitmapImage(new Uri("./woman.jpg", UriKind.RelativeOrAbsolute));
            }
            else
                PersonImage.Source = new BitmapImage(new Uri(person.ImageSource, UriKind.RelativeOrAbsolute));
            NameLabel.Content = person.Name;
            NameLabel.Visibility = Visibility.Visible;
            LastNameLabel.Content = person.LastName;
            LastNameLabel.Visibility = Visibility.Visible;
            DateLabel.Content = person.Date;
            DateLabel.Visibility = Visibility.Visible;
            PhoneLabel.Content = person.Phone;
            PhoneLabel.Visibility = Visibility.Visible;
            CityLabel.Content = person.City;
            CityLabel.Visibility = Visibility.Visible;
        }
        private FrameworkElement FindByName(string name, FrameworkElement root)
        {
            Stack<FrameworkElement> tree = new Stack<FrameworkElement>();
            tree.Push(root);

            while (tree.Count > 0)
            {
                FrameworkElement current = tree.Pop();
                if (current.Name == name)
                    return current;

                int count = VisualTreeHelper.GetChildrenCount(current);
                for (int i = 0; i < count; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    if (child is FrameworkElement)
                        tree.Push((FrameworkElement)child);
                }
            }

            return null;
        }
    }
    [Serializable]
    public enum SexEnum 
    {
        Male, Female
    }
    [Serializable]
    public class Person :ISerializable
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Date { get; set; }
        public int Phone { get; set; }
        public string City { get; set; }
        public SexEnum Sex { get; set; }

        public string ImageSource;

        public Person() { }
        public Person(string name, string lastname, int phone, SexEnum sex, string date, string city)
        {
            Name = name;
            LastName = lastname;
            Phone = phone;
            Sex = sex;
            Date = date;
            City = city;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
        }
    }


}
