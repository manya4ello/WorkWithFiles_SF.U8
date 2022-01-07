//Напишите программу, которая чистит нужную нам папку от файлов  и папок, которые не использовались более 30 минут 

//На вход программа принимает путь до папки. 

//При разработке постарайтесь предусмотреть возможные ошибки (нет прав доступа, папка по заданному адресу не существует, передан некорректный путь) и уведомить об этом пользователя.

//Критерии оценивания
//0 баллов: задача решена неверно или отсутствует доступ к репозиторию.
//2 балла (хорошо): код должен удалять папки рекурсивно (если в нашей папке лежит папка со вложенными файлами, не должно возникнуть проблем с её удалением).
//4 балла(отлично): предусмотрена проверка на наличие папки по заданному пути (строчка if directory.Exists); предусмотрена обработка исключений при доступе к папке (блок try-catch, а также логирует исключение в консоль).

using System;
using System.IO;
namespace FolderCleaner
{
    public static class FolderManager
    {
              
       public static void DirRunner(string path)
        {
            if (Directory.Exists(path))
            {
                CheckFiles(path);
                string[] dirs = Directory.GetDirectories(path);
                DirCheck(path);

                foreach (string d in dirs) 
                {
                    DirRunner(d);
                }
            }
            else
                Console.WriteLine("Нет такой директории");
        }
        /// <summary>
        /// Проверяет файлы в папке и, если не использовались более 30 мин - удаляет
        /// </summary>
        /// <param name="folder"></param>
        public static void CheckFiles(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            if (files.Length !=0) 
                Console.WriteLine("Файлы: в папке {0}", folder);

            foreach (string s in files)
            {
                DateTime lastmodified = File.GetLastWriteTime(s);
                TimeSpan unused = DateTime.Now.Subtract(File.GetLastWriteTime(s));
                Console.Write(s);
                if (unused>TimeSpan.FromMinutes(30))
                {
                   
                    Console.Write(" Надо стереть!");
                    try
                    {
                        File.Delete(s);
                        Console.Write("Файл успешно удален");
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
                    Console.Write(" пусть еще поживет");
                   
                }
                Console.ResetColor();
                //Console.Write($" Последняя запись: {File.GetLastWriteTime(Path.GetFullPath(s))}, время бездействия мин: {unused.TotalMinutes:F} / дни: {unused:%d} ");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Проверяет, что директория:
        /// не содержит файлов, не содержит папок, не использовалась более 30 минут
        /// </summary>
        /// <param name="path"></param>
        public static void DirCheck (string path)
        {
            bool check1 = Directory.Exists(path);
            bool check2 = (Directory.GetDirectories(path).Length == 0);
            bool check3 = (Directory.GetFiles(path).Length == 0);
            bool check4 = (DateTime.Now.Subtract(File.GetLastWriteTime(path)) > TimeSpan.FromMinutes(30));
            if (check1)
                {
                Console.Write("Проверка {0}", path);
                if (check2 & check3 & check4)
                {
                    Console.Write(" надо удалить");
                    try
                    {
                        Directory.Delete(path, true); //хотя true тут не нужен т.к. мы проверяем, что папка пустая. А если не пустая - идем глубже и стираем все по условию
                        Console.Write("Дериктория успешно удалена");
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
                    Console.WriteLine(" пусть еще поживет");
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

            FolderManager.DirRunner(path);


            Console.ReadKey();  
        }
    }
}