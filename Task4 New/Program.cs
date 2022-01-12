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

//Мои попытки открыть оригинальный файл

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
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
    class Program
    {
        /// <summary>
        /// Создает бинарный файл по заданному адресу
        /// </summary>
        /// <param name="path"></param>
        public static void CreateTestFile(string path)
        {
            Student[] student = new Student[6];
            student[0] = new Student("Вася", "математик");
            student[1] = new Student("Петя", "физик");
            student[2] = new Student("Женя", "химик", new DateTime(1984, 1, 12, 0, 0, 0));
            student[3] = new Student("Аня", "математик", new DateTime(1984, 11, 15, 0, 0, 0));
            student[4] = new Student("Эдик", "физик", new DateTime(1983, 9, 6, 0, 0, 0));
            student[5] = new Student("Полина", "химик", new DateTime(1983, 12, 10, 0, 0, 0));

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {

                    formatter.Serialize(fs, student);


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Что-то пошло не так: {0}", ex.Message);
            }


        }
        static void Main(string[] args)
        {
            string sourcefile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students.dat";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            if (!File.Exists(sourcefile))
                CreateTestFile(sourcefile);

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream reader = new FileStream(sourcefile, FileMode.Open))
                {
                    List<string> Groups = new List<string>();   //Следить за мусором от предыдущих запусков + сохранить категории если вдруг понадобится что-то делать с файлами далее

                    Student[] students = (Student[])formatter.Deserialize(reader);
                    Console.WriteLine("Содержание файла");
                    foreach (Student s in students)
                    {
                        Console.WriteLine("Считано: {0} {1}, д.р.:{2}", s.Name, s.Group, s.DateOfBirth.ToString("D"));

                        ///Пишем по категории в текстовый файл
                        string Grouppath = Path.Combine(path, s.Group + ".txt");
                        if (!File.Exists(Grouppath))
                            Console.WriteLine("создаем файл {0}", Grouppath);
                        else if (!Groups.Contains(s.Group))
                        {
                            File.Delete(Grouppath); //чистим мусор от прежних запусков
                            Console.WriteLine("Удаляем старый файл {0} и создаем новый", Grouppath);
                        }

                        var textfile = new FileInfo(Grouppath);
                        try
                        {
                            using (StreamWriter w = textfile.AppendText())
                            {
                                w.Write(s.Name + " ");
                                w.WriteLine(s.DateOfBirth.ToString("D"));

                                w.Close();
                            }

                        }
                        catch (Exception ex)
                        { Console.WriteLine("Не получается добавить запись в файл {0}: {1}", Grouppath, ex.Message); }

                        if (!Groups.Contains(s.Group))
                            Groups.Add(s.Group);
                    }







                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
