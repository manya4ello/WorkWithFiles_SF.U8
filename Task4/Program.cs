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
            long test;
            public long DateOfBirth
            { 
                get
                { 
                    return _Date.ToUnixTimeSeconds();
                }
                set {
                    _Date = DateTimeOffset.FromUnixTimeSeconds(value);
                    _DateOfBirth = _Date.UtcDateTime;
                    }
            }
            public Student(string name, string group)
            {
                Name = name;
                Group = group;
                DateOfBirth = new DateTimeOffset(1983,10,15,0,0,0, new TimeSpan(0,0,0)).ToUnixTimeSeconds(); 
            }
            public Student(string name, string group, DateTimeOffset date)
            {
                Name = name;
                Group = group;
                DateOfBirth = date.ToUnixTimeSeconds();
            }
        }
        /// <summary>
        /// Создает бинарный файл по заданному адресу
        /// </summary>
        /// <param name="path"></param>
        public static void CreateTestFile(string path)
        {
            Student[] student = new Student[6];
            student[0] = new Student("Вася", "математик");
            student[1] = new Student("Петя", "физик");
            student[2] = new Student("Женя", "химик", new DateTimeOffset(1984, 1, 12, 0, 0, 0, new TimeSpan(0, 0, 0)));
            student[3] = new Student("Аня", "математик", new DateTimeOffset(1984, 11, 15, 0, 0, 0, new TimeSpan(0, 0, 0)));
            student[4] = new Student("Эдик", "физик", new DateTimeOffset(1983, 9, 6, 0, 0, 0, new TimeSpan(0, 0, 0)));
            student[5] = new Student("Полина", "химик", new DateTimeOffset(1983, 12, 10, 0, 0, 0, new TimeSpan(0, 0, 0)));

           
                try
                {
                    using (BinaryWriter w = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                    {
                        for (int i = 0; i<student.Length; i++)
                        {
                            w.Write(student[i].Name);
                            w.Write(student[i].Group);
                            w.Write(student[i].DateOfBirth);
                        }
                        w.Close();
                    }

                }
                catch (Exception ex)
                { Console.WriteLine("Не получается добавить записи: {0}",ex.Message); }
           
        }
        static void Main(string[] args)
        {
            string sourcefile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\StudentsNew.dat";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students";
        
            if (Directory.Exists(path))
                Console.WriteLine("Есть такая папка");
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Теперь есть такая папка");
            }

            if (!File.Exists(sourcefile))
                CreateTestFile(sourcefile);
           
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(sourcefile, FileMode.Open)))
                {
                    List<string> Groups = new List<string>();   //Следить за мусором от предыдущих запусков + сохранить категории если вдруг понадобится что-то делать с файлами далее

                    while (reader.PeekChar() > -1)
                    {
                        string Name = reader.ReadString();
                        string Group = reader.ReadString();
                        long DateOfBirth = reader.ReadInt64();
                        Console.WriteLine("Из файла считано:{0}, {1}, {2}", Name, Group, DateTimeOffset.FromUnixTimeSeconds(DateOfBirth).ToString("D"));
                        
                        ///Пишем по категории в бинарный файл
                        string Grouppath = Path.Combine(path, Group+".dat");
                        if (!File.Exists(Grouppath))
                            Console.WriteLine("создаем файл {0}", Grouppath);
                        else if (!Groups.Contains(Group))
                        {
                            File.Delete(Grouppath); //чистим мусор от прежних запусков
                            Console.WriteLine("Удаляем старый файл {0} и создаем новый",Grouppath);
                        }
                            try
                        {
                            using (BinaryWriter w = new BinaryWriter(File.Open(Grouppath, FileMode.Append)))
                            {
                                    w.Write(Name);                                  
                                    w.Write(DateOfBirth);
                                
                                w.Close();
                            }

                        }
                        catch (Exception ex)
                        { Console.WriteLine("Не получается добавить запись в файл {0}: {1}", Grouppath, ex.Message); }

                        ///Пишем по категории в текстовый файл
                        Grouppath = Path.Combine(path, Group + ".txt");
                        if (!File.Exists(Grouppath))
                            Console.WriteLine("создаем файл {0}", Grouppath);
                        else if (!Groups.Contains(Group))
                        {
                            File.Delete(Grouppath); //чистим мусор от прежних запусков
                            Console.WriteLine("Удаляем старый файл {0} и создаем новый", Grouppath);
                        }

                        var textfile = new FileInfo(Grouppath);
                        try
                        {
                            using (StreamWriter w = textfile.AppendText()) 
                            {
                                w.Write(Name+" ");
                                w.WriteLine(DateTimeOffset.FromUnixTimeSeconds(DateOfBirth).ToString("D"));

                                w.Close();
                            }

                        }
                        catch (Exception ex)
                        { Console.WriteLine("Не получается добавить запись в файл {0}: {1}", Grouppath, ex.Message); }
                       
                        if (!Groups.Contains(Group))
                            Groups.Add(Group);

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

