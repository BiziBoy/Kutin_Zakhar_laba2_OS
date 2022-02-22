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

          Console.WriteLine("The SHA256 hash of " + randomWord + " is: " + hash);
          if (hash == "1115DD800FEAACEFDF481F1F9070374A2A81E27880F187396DB67958B207CBAD")
          {
            Console.WriteLine("Пароль подобран");
            break;
          }
        }
      }
      
      
      Console.ReadLine();
    }
  }
}