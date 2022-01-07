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
        public static void DirCleaner(string path)
        {

        }
        
        /// <summary>
        /// Прежде чем удалять - давайте посмотрим
        /// </summary>
        /// <param name="path"></param>
        public static void ShowContent(string path)
        {
            if (Directory.Exists(path)) // Проверим, что директория существует
            {
                //Console.WriteLine("Папки:");
                string[] dirs = Directory.GetDirectories(path);  // Получим все директории каталога

                foreach (string d in dirs) // Выведем их все
                {
                    Console.WriteLine();
                    Console.Write("Папка:");
                    TimeSpan unused = DateTime.Now.Subtract(File.GetLastWriteTime(Path.GetFullPath(d)));
                    Console.WriteLine($"{d} Последняя запись: {File.GetLastWriteTime(Path.GetFullPath(d))}, время бездействия мин: {unused.TotalMinutes:F} / дни: {unused:%d} ");
                    CheckFiles(d);
                    ShowContent(Path.GetFullPath(d));
                }
                CheckFiles(path);
            }
        }
        public static void CheckFiles(string folder)
        {
           string[] files = Directory.GetFiles(folder);
            Console.WriteLine("Файлы:");

            foreach (string s in files)
            {
                DateTime lastmodified = File.GetLastWriteTime(Path.GetFullPath(s));
                TimeSpan unused = DateTime.Now.Subtract(File.GetLastWriteTime(Path.GetFullPath(s)));
                Console.Write(s);
                if (unused>TimeSpan.FromMinutes(30))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" СТЕРЕТЬ!!!");
                    Console.ResetColor();
                    Console.Write($" Последняя запись: {File.GetLastWriteTime(Path.GetFullPath(s))}, время бездействия мин: {unused.TotalMinutes:F} / дни: {unused:%d} ");
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" ОК");
                    Console.ResetColor();
                    Console.Write($" Последняя запись: {File.GetLastWriteTime(Path.GetFullPath(s))}, время бездействия мин: {unused.TotalMinutes:F} / дни: {unused:%d} ");
                    Console.WriteLine();
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
                Console.WriteLine("есть такая папка");
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Теперь есть такая папка");
            }

            FolderManager.ShowContent(path);


            Console.ReadKey();  
        }
    }
}