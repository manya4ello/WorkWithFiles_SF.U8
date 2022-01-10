//ЗАДАНИЕ 3
//Доработайте программу из задания 1, используя ваш метод из задания 2.

//При запуске программа должна:

//Показать, сколько весит папка до очистки. Использовать метод из задания 2. 
//Выполнить очистку.
//Показать сколько файлов удалено и сколько места освобождено.
//Показать, сколько папка весит после очистки.

//Критерии оценивания
//0 баллов: задача решена неверно или отсутствует доступ к репозиторию.
//2 балла (хорошо): написан метод и код выполняет свою функцию.
//4 балла (отлично): предусмотрена обработка исключений и валидация пути.


using System;
using System.IO;
namespace Task3
{
    public static class FolderManager
    {

        public static long GetSize(string path)
        {
            long size = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
                size += file.Length;

            DirectoryInfo[] subdirs = dir.GetDirectories();
            foreach (DirectoryInfo subdir in subdirs)
                size += GetSize(subdir.FullName);

            return size;
        }
        public static void DirRunner(string path)
        {
            long delsize = 0;
            int delcount = 0;
            long oldfoldersize = 0;
            if (Directory.Exists(path))
            {
                DateTime lastaccesscatch = Directory.GetLastAccessTime(path); //считываем время последнего доступа ДО обращения к директории
                try
                {
                    oldfoldersize = FolderManager.GetSize(path);
                    Console.WriteLine("Размер директории {0} - {1} byte ({2} MB)", path, oldfoldersize, oldfoldersize / 1048576);                       
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удалось рассчитать размер {0} \tОшибка: {1}", path, ex.Message);
                }

                DirCheck(path, lastaccesscatch);

                if (!Directory.Exists(path))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Директория {0} удалена, освоюождено {1} byte ({2} MB)", path, oldfoldersize, oldfoldersize / 1048576);
                    Console.ResetColor();
                }
                else
                {
                    CheckFiles(path, out delsize, out delcount);
                    long newfoldersize = 0;
                    try
                    {
                        newfoldersize = FolderManager.GetSize(path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось рассчитать размер {0} \tОшибка: {1}", path, ex.Message);
                    }

                    Console.WriteLine("Удалено файлов: {0} шт. размером {1} байт ({2} MB)\tНовый размер папки {3} байт ({4} MB)", delcount, delsize, delsize / 1048576, newfoldersize, newfoldersize / 1048576);
                    string[] dirs = Directory.GetDirectories(path);

                    foreach (string d in dirs)
                    {
                        DirRunner(d);
                    }
                }
            }
            else
                Console.WriteLine("Нет такой директории");
        }
        /// <summary>
        /// Проверяет файлы в папке и, если не использовались более 30 мин - удаляет
        /// </summary>
        /// <param name="folder"></param>
        public static void CheckFiles(string folder, out long size, out int count)
        {
            size = 0;
            count = 0;

            DirectoryInfo dir = new DirectoryInfo(folder);
            FileInfo[] files = dir.GetFiles();

            if (files.Length != 0)
                Console.WriteLine("Файлы: в папке {0}", folder);

            foreach (FileInfo s in files)
            {
                DateTime lastmodified = s.LastAccessTime;
                TimeSpan unused = DateTime.Now.Subtract(lastmodified);
                Console.Write(s);
                if (unused > TimeSpan.FromMinutes(30))
                {

                    Console.Write(" Надо стереть!");
                    try
                    {
                        long temp = s.Length;
                        s.Delete();
                        Console.Write("Файл успешно удален");
                        count++;
                        size +=temp;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" пусть еще поживет ({0})", (TimeSpan.FromMinutes(30) - unused).ToString(@"hh\:mm\:ss"));

                }
                Console.ResetColor();
               
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Проверяет, что директория:
        /// не использовалась более 30 минут
        /// </summary>
        /// <param name="path"></param>
        public static void DirCheck(string path, DateTime lastaccess)
        {
                       
            if (Directory.Exists(path))
            {
                bool check4 = (DateTime.Now.Subtract(lastaccess) > TimeSpan.FromMinutes(30));

                Console.Write("Проверка {0}", path);
                if (check4)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; 
                    Console.Write(" надо удалить. ");
                    Console.ResetColor();
                    try
                    {
                        Directory.Delete(path, true); 
                        Console.Write("Дериктория успешно удалена");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Не получилось удалить: {0}",ex.Message);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" пусть еще поживет ({0})", (TimeSpan.FromMinutes(30) - DateTime.Now.Subtract(lastaccess)).ToString(@"hh\:mm\:ss"));
                    Console.ResetColor();
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\SF Tests";
            if (Directory.Exists(path))
                Console.WriteLine("Есть такая папка");
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Теперь есть такая папка");
            }
            Console.WriteLine();

            //Console.WriteLine("Проверка времени пока не начали: {0}",TimeSpan.FromMinutes(30) - DateTime.Now.Subtract(Directory.GetLastAccessTime(path)));

            FolderManager.DirRunner(path);


            Console.ReadKey();
        }
    }
}