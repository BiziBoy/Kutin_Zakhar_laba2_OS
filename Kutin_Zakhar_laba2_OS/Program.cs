﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
  class Producer
  {
    private ChannelWriter<string> Writer;

    public Producer(ChannelWriter<string> _writer)
    {
      Writer = _writer;
      Task.WaitAll(Run());
    }

    private async Task Run()
    {
      while (await Writer.WaitToWriteAsync())
      {
        char[] word = new char[5];
        if (!Program.foundFlag)
        {
          for (int i = 97; i < 123; i++)
          {
            word[0] = (char)i;
            for (int k = 97; k < 123; k++)
            {
              word[1] = (char)k;
              for (int l = 97; l < 123; l++)
              {
                word[2] = (char)l;
                for (int m = 97; m < 123; m++)
                {
                  word[3] = (char)m;
                  for (int n = 97; n < 123; n++)
                  {
                    word[4] = (char)n;
                    await Writer.WriteAsync(new string(word));
                  }
                }
              }
            }
          }
        }
        else return;
      }
    }
  }


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
      while (await Reader.WaitToReadAsync())
      {
        while (!Program.foundFlag)
        {
          var item = Reader.ReadAsync();
          Console.WriteLine($"получены данные {item}");
          if (FoundHash(item.ToString()) != PasswordHash)
          {
            continue;
          }
          else
          {
            Console.WriteLine($"Пароль подобран - {item}");
            Program.foundFlag = true;
          } 
        }
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
      //Console.WriteLine("The SHA256 hash of " + hash);
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
      //Console.WriteLine(c.passwordGuessing(passwordHash));
      Console.Write("Введите количество потоков: ");
      int countStream = int.Parse(Console.ReadLine());

      var channel = Channel.CreateUnbounded<string>();
      var prod = Task.Run(() => { new Producer(channel.Writer); });
      Task[] streams = new Task[countStream + 1];
      streams[0] = prod;
      for (int i = 1; i < countStream + 1; i++)
      {
        streams[i] = Task.Run(() => { new Consumer(channel.Reader, passwordHash); });
      }
      Task.WaitAll(streams);
    }
  }
}