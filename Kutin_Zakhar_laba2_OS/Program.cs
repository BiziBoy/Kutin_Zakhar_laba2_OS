using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      while (true)
      {
        char[] randomWord = new char[5];
        Random random = new Random();
        //A-Z (65-90) a-z(97-122) 0-9(48-57)
        for (int i = 0; i < 5; i++)
        {
          randomWord[i] = (char)random.Next(97, 123);
        }
        Console.WriteLine(randomWord);
        using (SHA256 sha256Hash = SHA256.Create())
        {
          //Из строки в байтовый массив
          byte[] sourceBytes = Encoding.ASCII.GetBytes(randomWord);
          byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
          string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);


  class Consumer
  {
    private ChannelReader<string> Reader;
    private string PasswordHash;

    public Consumer(ChannelReader<string> _reader, string _passwordHash)
    {
      Reader = _reader;
      PasswordHash = _passwordHash;
      Task.WaitAll(Run());
    }

    private async Task Run()
    {
      // ожидает, когда освободиться место для чтения элемента.
      while (await Reader.WaitToReadAsync())
      {
        if (!Program.foundFlag)
        {
          var item = await Reader.ReadAsync();
          //Console.WriteLine($"получены данные {item}");
          if (FoundHash(item.ToString()) == PasswordHash)
          {
            Console.WriteLine($"Пароль подобран - {item}");
            Program.foundFlag = true;
          }
        }
        else return;
      }
    }
    /// <summary>
    /// Находит хеш str
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static public string FoundHash(string str)
    {
      SHA256 sha256Hash = SHA256.Create();
      //Из строки в байтовый массив
      byte[] sourceBytes = Encoding.ASCII.GetBytes(str);
      byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
      string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
      return hash;
    }

  }

  class Program
  {
    const string PATH = "passwordHashes.txt";
    static public bool foundFlag = false;

    static public void Main()
    {
      Console.WriteLine("Выберите по какому хеш значению SHA-256 подобрать пароль: ");
      Console.WriteLine("1. 1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad");
      Console.WriteLine("2. 3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b");
      Console.WriteLine("3. 74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f");
      int sign = int.Parse(Console.ReadLine());
      string[] readText = File.ReadAllLines(PATH);
      string passwordHash = readText[sign - 1].ToUpper();
      Console.Write("Введите количество потоков: ");
      int countStream = int.Parse(Console.ReadLine());

      //создаю общий канал данных
      Channel<string> channel = Channel.CreateBounded<string>(countStream);

      //создается производитель
      var prod = Task.Run(() => { new Producer(channel.Writer); });
      Task[] streams = new Task[countStream + 1];
      streams[0] = prod;
      //создаются потребители 
      for (int i = 1; i < countStream + 1; i++)
      {
        streams[i] = Task.Run(() => { new Consumer(channel.Reader, passwordHash); });
      }
      //Ожидает завершения выполнения всех указанных объектов Task 
      Task.WaitAll(streams);
      Console.WriteLine("Введите ENTER, чтобы выйти из программы.");
      Console.ReadKey();
    }

    static public void Main()
    {
      printMenu();
    }
  }
}