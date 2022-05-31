using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Hostel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly List<string> GenderList = new List<string>() { "Male", "Female", "Bigender", "X" };
        private DataContext context { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            context = new DataContext();
            UpdateData();
        }

        public void UpdateData()
        {
            List<Room> DatabaseRooms = context.Rooms.Include(room => room.Students).ToList();
            RoomsItemList.ItemsSource = DatabaseRooms;
            List<Privilege> DatabasePrivileges = context.Privileges.Include(privilege => privilege.Students).ToList();
            PrivilegesItemList.ItemsSource = DatabasePrivileges;
            List<Student> DatabaseStudents = context.Students.Include(student => student.Privilege).Include(student => student.Room).ToList();
            StudentsItemList.ItemsSource = DatabaseStudents;

            PrivilegeComboBox.ItemsSource = DatabasePrivileges;
            RoomComboBox.ItemsSource = DatabaseRooms;
            GenderComboBox.ItemsSource = GenderList;
        }


        //метод перевірки на null, пустого рядку, або пробілів
        private bool IsEmpty(string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        private void CreateRoom(object sender, RoutedEventArgs e)
        {
            //беремо дані з текстових полів
            var number = NumberTextBox.Text;
            var value = ValueTextBox.Text;
            var amount = AmountTextBox.Text;
            var floor = FloorTextBox.Text;
            //перевіряємо чи введенні данні не дорівнюють нулю або ж пробілам
            if (IsEmpty(number) || IsEmpty(value) || IsEmpty(amount) || IsEmpty(floor))
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //перевіряємо чи введені дані є числами
                if (int.TryParse(number, out int intNumber) && int.TryParse(amount, out int intAmount) && int.TryParse(floor, out int intFloor) && int.TryParse(value, out int intValue))
                {
                    //додаємо кімнату до списку
                    context.Rooms.Add(new Room()
                    {
                        Number = intNumber,
                        Value = intValue,
                        Amount = intAmount,
                        Floor = intFloor,
                    });
                    //сихнхронізуємо дані з базою даних
                    context.SaveChanges();
                    //синхронізуємо дані з інтерфейсом
                    UpdateData();
                }
                //якщо дані неправильного формату - повідомляємо про це 
                else
                {
                    MessageBox.Show("Some field is not in the correct format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteRoom(object sender, RoutedEventArgs e)
        {
            //якщо рядок вибраний
            if (RoomsItemList.SelectedItem is Room selectedRoom)
            {
                //шукаємо цей елемент по Id
                Room room = context.Rooms.Include(room => room.Students)
                                         .FirstOrDefault(x => x.Id == selectedRoom.Id);
                //видаляємо його з контексту
                context.Rooms.Remove(room);
                //синхронізуємо з базою даних
                context.SaveChanges();
                //синхронізуємо з інтерфейсом
                UpdateData();
            }
            //якщо рядок не вибраний - повідомлення користувачу
            else
            {
                MessageBox.Show("Select some row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void UpdateRoom(object sender, RoutedEventArgs e)
        {
            //знаходимо виділений в таблиці рядок
            Room selectedRoom = RoomsItemList.SelectedItem as Room;
            //беремо дані з текстових полів
            var number = NumberTextBox.Text;
            var value = ValueTextBox.Text;
            var amount = AmountTextBox.Text;
            var floor = FloorTextBox.Text;
            //перевіряємо чи ввів користувач дані та чи вибрав рядок
            if (IsEmpty(number) || IsEmpty(value) || IsEmpty(amount) || IsEmpty(floor) || selectedRoom == null)
            {
                MessageBox.Show("Fill all fields or select some row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //перевіряємо чи введені дані є числами
                if (int.TryParse(number, out int intNumber) && int.TryParse(amount, out int intAmount) && int.TryParse(floor, out int intFloor) && int.TryParse(value, out int intValue))
                {
                    //шукаємо кімнату по Id
                    Room room = context.Rooms.Find(selectedRoom.Id);
                    //оновлюємо новими значеннями
                    room.Number = intNumber;
                    room.Value = intValue;
                    room.Amount = intAmount;
                    room.Floor = intFloor;
                    //синхронізуємо з базою даних
                    context.SaveChanges();
                    //синхронізуємо з інтерфейсом
                    UpdateData();
                }
                //якщо дані неправильного формату - повідомляємо про це 
                else
                {
                    MessageBox.Show("Some field is not in the correct format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CreatePrivilege(object sender, RoutedEventArgs e)
        {
            //беремо дані з текстових полів
            var type = TypeTextBox.Text;
            var discount = DiscountTextBox.Text;
            //перевіряємо чи введенні данні не дорівнюють нулю або ж пробілам
            if (IsEmpty(type) || IsEmpty(discount))
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //перевіряємо чи знижка введена коректно
                if (int.TryParse(discount, out int intDiscount) && intDiscount >= 0 && intDiscount <= 100)
                {
                    //додаємо привілегію до списку
                    context.Privileges.Add(new Privilege()
                    {
                        Type = type,
                        Discount = intDiscount
                    });
                    //сихнхронізуємо дані з базою даних
                    context.SaveChanges();
                    //синхронізуємо дані з інтерфейсом
                    UpdateData();
                }
                //якщо дані неправильного формату - повідомляємо про це 
                else
                {
                    MessageBox.Show("Some field is not in the correct format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeletePrivrlege(object sender, RoutedEventArgs e)
        {
            //якщо рядок вибраний
            if (PrivilegesItemList.SelectedItem is Privilege selectedPrivilage)
            {
                //шукаємо цей елемент по Id
                Privilege privilege = context.Privileges.Include(privilege => privilege.Students)
                                         .FirstOrDefault(x => x.Id == selectedPrivilage.Id);
                //видаляємо його з контексту
                context.Privileges.Remove(privilege);
                //синхронізуємо з базою даних
                context.SaveChanges();
                //синхронізуємо з інтерфейсом
                UpdateData();
            }
            //якщо рядок не вибраний - повідомлення користувачу
            else
            {
                MessageBox.Show("Select some row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePrivilege(object sender, RoutedEventArgs e)
        {
            //знаходимо виділений в таблиці рядок
            Privilege selectedPrivilege = PrivilegesItemList.SelectedItem as Privilege;
            //беремо дані з текстових полів
            var type = TypeTextBox.Text;
            var discount = DiscountTextBox.Text;
            //перевіряємо чи ввів користувач дані та чи вибрав рядок
            if (IsEmpty(type) || IsEmpty(discount))
            {
                MessageBox.Show("Fill all fields or select some row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //перевіряємо чи введені дані є числами
                if (int.TryParse(discount, out int intDiscount))
                {
                    //шукаємо кімнату по Id
                    Privilege privilege = context.Privileges.Find(selectedPrivilege.Id);
                    //оновлюємо новими значеннями
                    privilege.Type = type;
                    privilege.Discount = intDiscount;
                    //синхронізуємо з базою даних
                    context.SaveChanges();
                    //синхронізуємо з інтерфейсом
                    UpdateData();
                }
                //якщо дані неправильного формату - повідомляємо про це 
                else
                {
                    MessageBox.Show("Some field is not in the correct format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateStudent(object sender, RoutedEventArgs e)
        {
            //беремо дані з текстових полів
            var name = NameTextBox.Text;
            var address = AddressTextBox.Text;
            var group = GroupTextBox.Text;
            var privilege = PrivilegeComboBox.SelectedValue;
            var room = RoomComboBox.SelectedValue;
            var gender = GenderComboBox.SelectedValue;

            //перевіряємо чи введенні данні не дорівнюють нулю або ж пробілам
            if (IsEmpty(name) || IsEmpty(address) || IsEmpty(group) || privilege == null || room == null || gender == null)
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //додаємо студента до списку
                context.Students.Add(new Student()
                {
                    Name = name,
                    Address = address,
                    Group = group,
                    PrivilegeId = (int)privilege,
                    RoomId = (int)room,
                    Gender = (string)gender

                });
                //сихнхронізуємо дані з базою даних
                context.SaveChanges();
                //синхронізуємо дані з інтерфейсом
                UpdateData();
            }
        }

        private void UpdateStudent(object sender, RoutedEventArgs e)
        {
            //знаходимо виділений в таблиці рядок
            Student selectedStudent = StudentsItemList.SelectedItem as Student;
            //беремо дані з текстових полів
            var name = NameTextBox.Text;
            var address = AddressTextBox.Text;
            var group = GroupTextBox.Text;
            var privilege = PrivilegeComboBox.SelectedValue;
            var room = RoomComboBox.SelectedValue;
            var gender = GenderComboBox.SelectedValue;
            //перевіряємо чи ввів користувач дані та чи вибрав рядок
            if (IsEmpty(name) || IsEmpty(address) || IsEmpty(group) || privilege == null || room == null || gender == null)
            {
                MessageBox.Show("Fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //перевіряємо чи введені дані є числами
                if (IsEmpty(name) == false && IsEmpty(address) == false && IsEmpty(group) == false && privilege != null && room != null && gender != null)
                {
                    //шукаємо кімнату по Id
                    Student student = context.Students.Find(selectedStudent.Id);
                    //оновлюємо новими значеннями
                    student.Name = name;
                    student.Address = address;
                    student.Group = group;
                    student.PrivilegeId = (int)privilege;
                    student.RoomId = (int)room;
                    student.Gender = (string)gender;
                    //синхронізуємо з базою даних
                    context.SaveChanges();
                    //синхронізуємо з інтерфейсом
                    UpdateData();
                }
                //якщо дані неправильного формату - повідомляємо про це 
                else
                {
                    MessageBox.Show("Some field is not in the correct format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void DeleteStudent(object sender, RoutedEventArgs e)
        {
            //якщо рядок вибраний
            if (StudentsItemList.SelectedItem is Student selectedStudent)
            {
                //шукаємо цей елемент по Id
                Student student = selectedStudent;
                //видаляємо його з контексту
                context.Students.Remove(student);
                //синхронізуємо з базою даних
                context.SaveChanges();
                //синхронізуємо з інтерфейсом
                UpdateData();
            }
            //якщо рядок не вибраний - повідомлення користувачу
            else
            {
                MessageBox.Show("Select some row", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StudentsItemList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
