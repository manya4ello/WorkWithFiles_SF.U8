//Напишите программу, которая считает размер папки на диске (вместе со всеми вложенными папками и файлами). На вход метод принимает URL директории, в ответ — размер в байтах.

//Критерии оценивания
//0 баллов: задача решена неверно или отсутствует доступ к репозиторию.
//2 балла (хорошо): написан метод и код выполняет свою функцию.
//4 балла (отлично): предусмотрена обработка исключений и валидация пути.


using System;
using System.IO;
namespace Task2
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
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\SF Tests";
            
            if (Directory.Exists(path))
            {
                
                try
                {
                    Console.WriteLine("Размер директории {0} - {1} byte ({2} MB)", path, FolderManager.GetSize(path), FolderManager.GetSize(path) / 1048576);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удалось рассчитать размер {0} \tОшибка: {1}",path, ex.Message);
                }
            }
            else
            {
                Console.WriteLine("К сожалению нет такой папки");
            }


            Console.ReadKey();
        }
    }
}