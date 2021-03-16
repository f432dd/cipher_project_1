using System;
using System.Collections.Generic;
using System.IO;

namespace cipher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("программа по шифровки и расшифровки сообщений");
            while (true)
            {
                try
                {
                    Console.WriteLine(" введите 1 для шифровки, или 2 для расшифровки: ");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1)
                    {
                        encoder();
                    }
                    else if (choice == 2)
                    {
                        decoder();
                    }
                    else
                    {
                        Console.WriteLine("ошибка");
                    }
                }
                catch
                {
                    Console.WriteLine("ошибка");
                }
            }
        }
        //шифровка сообщения; (message encryption) //
        static void encoder()
        {
            //ввод сообщения для шифровки (entering a message for encryption); //
            Console.WriteLine(" введите тескт: ");
            string text = Console.ReadLine();

            //создание словаря символов для хранения значений "ключ" (creating a symbol dictionary to store the "key" values); //
            Dictionary<string, int> symbols = new Dictionary<string, int>() { };
            string[] symbol = {
                "1", "2", "3", "4", "5", "6", "7",
                "8", "9", "0", "а", "б", "в", "г",
                "д", "е", "ё", "ж", "з", "и", "й",
                "к", "л", "м", "н", "о", "п", "р",
                "с", "т", "у", "ф", "х", "ц", "ч",
                "ш", "щ", "ъ", "ы", "ь", "э", "ю",
                "я", " ", "+", "-", "="
            };

            //присвоение значений символам (assigning values to symbols); //
            int[] meaning = new int[symbol.Length];
            Random a = new Random();
            for (int i = 0; i < meaning.Length; i++)
            {
                meaning[i] = a.Next(10, 99);
                if (i >= 1)
                {
                    for (int j = 0; j < i; j++)
                    {
                        while (meaning[i] == meaning[j])
                        {
                            meaning[i] = a.Next(10, 99);
                            j = 0;
                        }
                        if (j == meaning.Length)
                        {
                            return;
                        }
                    }
                }
                symbols.Add(symbol[i], meaning[i]);
            }
            int[] message = new int[text.Length];
            char[] character_array = text.ToCharArray();
            for (int i = 0; i < character_array.Length; i++)
            {
                foreach (KeyValuePair<string, int> keyValue in symbols)
                {
                    if (character_array[i] == Convert.ToChar(keyValue.Key))
                    {
                        message[i] = keyValue.Value;
                    }
                }
            }

            //вывод зашифрованного сообщения в консоль и запись его в строку string_message для записи в файл; //
            //(output the encrypted message to the console and write it to string_message for writing to a file); //
            string string_message = "";
            for (int i = 0; i < message.Length; i++)
            {
                string_message += message[i];
            }

            //создание каталога (directory creation); //
            string dirName = "D:\\encrypted messages";
            Console.WriteLine(" введите название для каталога: ");
            string name_message = Console.ReadLine();
            string subpath = name_message;
            DirectoryInfo dir = new DirectoryInfo(dirName);
            if (!dir.Exists)
            {
                dir.Create();
            }
            dir.CreateSubdirectory(subpath);

            //создание и заполнение файла с сообщением (creating and filling a file with a message); //
            string file_path = "D:\\encrypted messages\\" + name_message + "\\" + name_message + ".txt";
            File.WriteAllText(file_path, string_message);

            //создание и заполнение файла с ключом шифра (creating and filling a file with a cipher key); //
            string key = "";
            foreach (KeyValuePair<string, int> keyValue in symbols)
            {
                key += keyValue.Value;
            }
            File.WriteAllText("D:\\encrypted messages\\" + name_message + "\\key(" + name_message + ").txt", key);
        }

        //расшифровка сообщения (message decryption); //
        static void decoder()
        {
            //создание словаря символов для заполнением значениями ключа и дальнейшей расшифровки сообщения; //
            //(creation of a dictionary of symbols for filling with key values and further decryption of the message); //
            Dictionary<string, int> symbols = new Dictionary<string, int>() { };
            string[] symbol = {
                "1", "2", "3", "4", "5", "6", "7",
                "8", "9", "0", "а", "б", "в", "г",
                "д", "е", "ё", "ж", "з", "и", "й",
                "к", "л", "м", "н", "о", "п", "р",
                "с", "т", "у", "ф", "х", "ц", "ч",
                "ш", "щ", "ъ", "ы", "ь", "э", "ю",
                "я", " ", "+", "-", "="
            };

            //получение значений ключа (getting key values); //
            Console.WriteLine(" введите название каталога с сообщением: ");
            string name_message = Console.ReadLine();
            string key_path = "D:\\encrypted messages\\" + name_message + "\\key(" + name_message + ").txt";
            string[] key_file = File.ReadAllLines(key_path);

            string string_key = "";
            for (int i = 0; i < key_file.Length; i++)
            {
                string_key += key_file[i];
            }

            string[] key_array = new string[symbol.Length];
            int a = 0;
            for (int i = 0; i < string_key.Length; i += 2)
            {
                key_array[a] = string_key.Substring(i, 2);
                a++;
            }

            //заполнение словаря "ключа" значениями (filling the dictionary "key" with values); //
            for (int i = 0; i < symbol.Length; i++)
            {
                symbols.Add(symbol[i], Convert.ToInt32(key_array[i]));
            }

            //получение и расшифровка сообщения (receiving and decrypting a message); //
            string message = "D:\\encrypted messages\\" + name_message + "\\" + name_message + ".txt";
            string[] message_file = File.ReadAllLines(message);

            string string_message = "";
            for (int i = 0; i < message_file.Length; i++)
            {
                string_message += message_file[i];
            }

            string[] message_array = new string[string_message.Length];
            int b = 0;
            for (int i = 0; i < string_message.Length; i += 2)
            {
                message_array[b] = string_message.Substring(i, 2);
                b++;
            }

            //вывод расшифрованного сообщения (output of decrypted message); //
            int c = 0;
            string message_return = "";
            while (true)
            {
                if (message_return.Length == string_message.Length / 2)
                {
                    break;
                }
                foreach (KeyValuePair<string, int> keyValue in symbols)
                {
                    if (keyValue.Value == Convert.ToInt32(message_array[c]))
                    {
                        message_return += keyValue.Key;
                        c++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            Console.WriteLine(" содержание сообения:\n" + message_return + "\n");
        }
    }
}
