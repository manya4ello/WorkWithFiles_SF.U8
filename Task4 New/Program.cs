//ЗАДАНИЕ 4
//Написать программу-загрузчик данных из бинарного формата в текст.

//На вход программа получает бинарный файл, предположительно, это база данных студентов.

//Свойства сущности Student:

//Имя — Name(string);
//Группа — Group(string);
//Дата рождения — DateOfBirth (DateTime).

//Ваша программа должна:
//Создать на рабочем столе директорию Students.
//Внутри раскидать всех студентов из файла по группам (каждая группа-отдельный текстовый файл), в файле группы студенты перечислены построчно в формате "Имя, дата рождения".
//Критерии оценивания
//0 баллов: задача решена неверно или отсутствует доступ к репозиторию.
//2 балла (хорошо): есть недочеты.
//4 балла (отлично): программа работает верно.

//Мои попытки открыть оригинальный файл успехом не увенчались

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    class Program
    {
        [Serializable]
        public class Student
        {
            public string Name { get; set; }
            public string Group { get; set; }
            public DateTime DateOfBirth { get; set; }
            
            public Student(string name, string group)
            {
                Name = name;
                Group = group;
                DateOfBirth = new DateTime(1983, 10, 15, 0, 0, 0);
            }
            public Student(string name, string group, DateTime date)
            {
                Name = name;
                Group = group;
                DateOfBirth = date;
            }
        }
        
        static void Main(string[] args)
        {
            string sourcefile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students.dat";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students";

            if (Directory.Exists(path))
                Console.WriteLine("Есть такая папка");
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Теперь есть такая папка");
            }

            if (File.Exists(sourcefile))
            {

                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream reader = new FileStream(sourcefile, FileMode.Open))
                    {
                        
                        Student[] students = (Student[])formatter.Deserialize(reader);
                        Console.WriteLine("Вот что удалось считать:");
                        foreach (Student s in students)
                        {
                            Console.WriteLine(s.Name);  
                            Console.WriteLine(s.Group); 
                            Console.WriteLine(s.DateOfBirth.ToString("D"));
                        }                                           

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Что-то пошло не так: {0}",ex.Message);
                }
            }
            else
                Console.WriteLine("Файл {0} не найден",sourcefile); 
        }
    }
}

