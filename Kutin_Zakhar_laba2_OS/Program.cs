using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{  
  class Program
  {
    const string PATH = "passwordHashes.txt";
    static object locker = new();
    static AutoResetEvent waitHandler = new AutoResetEvent(true);

    static void workWithMultithreading(int _countStream, int _sign) 
    {
      //создаем потоки
      for (int i = 0; i < _countStream; i++)
      {
        Thread myThread = new Thread(passwordGuessing);
        myThread.Name = $"Поток {i + 1}";   // устанавливаем имя для каждого потока
        myThread.Start(_sign);
      }
    }

    /// <summary>
    /// Считывает хеш-значение из файла, за выбор которого отвечает Sign. Затем подбирает пароль с этим хеш-значением
    /// </summary>
    static void passwordGuessing(object _sign)
    {
      if (_sign is int intSign) 
      {
        string[] readText = File.ReadAllLines(PATH);
        string passwordHash = readText[intSign - 1].ToUpper();
        char[] word = new char[5];
        for (int i = 97; i < 123; i++)
        {
          for (int k = 97; k < 123; k++)
          {
            for (int l = 97; l < 123; l++)
            {
              for (int m = 97; m < 123; m++)
              {
                for (int n = 97; n < 123; n++)
                {
                  lock (locker)
                  {
                    for (int p = 0; p < 5; p++)
                    {
                      switch (p)
                      {
                        case 0:
                          word[p] = (char)i;
                          break;
                        case 1:
                          word[p] = (char)k;
                          break;
                        case 2:
                          word[p] = (char)l;
                          break;
                        case 3:
                          word[p] = (char)m;
                          break;
                        case 4:
                          word[p] = (char)n;
                          break;
                        default:
                          break;
                      }
                      Console.Write(word[p]);
                    }
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                      //Из строки в байтовый массив
                      byte[] sourceBytes = Encoding.ASCII.GetBytes(word);
                      byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                      string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                      //Console.WriteLine("The SHA256 hash of " + hash);
                      if (hash == passwordHash)
                      {
                        Console.WriteLine();
                        Console.WriteLine("Пароль подобран");
                        Thread.Sleep(15000);
                      }
                    }
                  }
                  Console.WriteLine();
                }
              }
            }
          }
        }
      }

    }

    /// <summary>
    /// Выводит меню для взаимодействия с пользователем
    /// </summary>
    static void printMenu() 
    {
      bool flag = true;
      while (flag)
      {
        Console.WriteLine("1. Выполнить задание.");
        Console.WriteLine("2. Очистить консоль.");
        Console.WriteLine("3. Выйти из программы.");
        Console.Write("Введите пункт меню: ");
        int menuSign = int.Parse(Console.ReadLine());
        switch (menuSign)
        {
          case 1:
            Console.WriteLine("Выберите по какому хеш значению SHA-256 подобрать пароль: ");
            Console.WriteLine("1. 1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad");
            Console.WriteLine("2. 3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b");
            Console.WriteLine("3. 74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f");
            int sign = int.Parse(Console.ReadLine());
            Console.Write("Введите количество потоков: ");
            int countStream = int.Parse(Console.ReadLine());
            workWithMultithreading(countStream, sign);
            break;
          case 2:
            Console.Clear();
            break;
          case 3:
            flag = false;
            break;
          default:
            Console.WriteLine("Данного пункта нет в меню.");
            break;
        }
      }

    }

    static void Main(string[] args)
    {
      printMenu();
    }
  }
}