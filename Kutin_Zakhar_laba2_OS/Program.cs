using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      string passwordHash = "1115DD800FEAACEFDF481F1F9070374A2A81E27880F187396DB67958B207CBAD";
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
                for (int p = 0; p < 5; p++)
                {
                  switch (p)
                  {
                    case 0:
                      word[p] = 'z';
                      break;
                    case 1:
                      word[p] = 'y';
                      break;
                    case 2:
                      word[p] = 'z';
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
                  Console.WriteLine("The SHA256 hash of " + hash);
                  if (hash == passwordHash)
                  {
                    Console.WriteLine("пароль подобран");
                    Thread.Sleep(15000);
                  }
                }
                Console.WriteLine();
              }
            }
          }
        }
        Thread.Sleep(300);
        
      }
    }
  }
}