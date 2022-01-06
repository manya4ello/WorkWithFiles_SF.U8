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
        public static void ShowContent(string path)
        {
            if (Directory.Exists(path)) // Проверим, что директория существует
            {
                Console.WriteLine("Папки:");
                string[] dirs = Directory.GetDirectories(path);  // Получим все директории каталога

                foreach (string d in dirs) // Выведем их все
                    Console.WriteLine(d);

                Console.WriteLine();
                Console.WriteLine("Файлы:");
                string[] files = Directory.GetFiles(path);// Получим все файлы каталога

                foreach (string s in files)   // Выведем их все
                    Console.WriteLine(s);
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