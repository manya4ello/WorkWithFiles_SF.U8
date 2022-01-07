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



using System;
using System.IO;
namespace Students
{
    class Program
    {
        public class Student
        {
            public string Name;
            public string Group;
            private DateTime _DateOfBirth;
            private DateTimeOffset _Date;
            public long DateOfBirth
            { 
                get
                { 
                    
                    return _Date.ToUnixTimeSeconds;
                }
                set {
                    _DateOfBirth = new DateTime(value);
                }
            }
            public Student(string name, string group)
            {
                Name = name;
                Group = group;
                _DateOfBirth = new DateTime(1967, 7, 20); 
            }
        }

        public static void CreateTestFile(string path)
        {
            Student[] student = new Student[3];
            student[0] = new Student("Вася", "грузчик");
            student[1] = new Student("Петя", "грузчик");
            student[2] = new Student("Женя", "пианист");

            if (!File.Exists(path))
                File.Create(path);


            if (File.Exists(path))
            {
                try
                {
                    using (BinaryWriter w = new BinaryWriter(File.Open(path, FileMode.Append)))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            w.Write(student[i].Name);
                            w.Write(student[i].Group);
                            w.Write(student[i].DateOfBirth);
                        }
                        w.Close();
                    }

                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
            }
        }
        static void Main(string[] args)
        {
            string sourcefile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students.dat";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students";
            string testpath = path + @"\test.dat";
            if (Directory.Exists(path))
                Console.WriteLine("Есть такая папка");
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Теперь есть такая папка");
            }

            CreateTestFile(testpath);
           

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(testpath, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        string Name = reader.ReadString();
                        string Group = reader.ReadString();
                        long DateOfBirth = reader.ReadInt64();
                        Console.WriteLine("Из файла считано:{0}, {1}, {2}", Name, Group, new DateTime(DateOfBirth));
                    }

                }

            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            ;
            //if (File.Exists(sourcefile))
            //{
            //    try
            //    {
            //        using (BinaryReader reader = new BinaryReader(File.Open(sourcefile, FileMode.Open)))
            //        {
            //            while (reader.PeekChar() > -1)
            //            {
            //                string Name = reader.ReadString();
            //                string Group = reader.ReadString();
            //                string DateOfBirth = reader.ReadString();
            //                Console.WriteLine("Из файла считано:{0}, {1}", Name, Group);
            //            }

            //        }

            //    }
            //    catch (Exception ex)
            //    { Console.WriteLine(ex.Message); }  

            //    //Console.WriteLine(student.Name);
            //    //Console.WriteLine(student.Group);

            //}

        }
    }
}

