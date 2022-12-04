using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using System.Net;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Html;
using System.Linq;
using System.Globalization;
using System.Diagnostics;


namespace telegram_bot_test
{
    internal class Program
    {
        // Api ключ
        private static string token { get; set; } = "5466316556:AAEHGTgXH64T0bl9dLEu9mXN_v2WjbNfLEA";
        //создание бота
        private static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            //иниациализация по Api ключу
            Bot = new TelegramBotClient(token);
            //Начало работы
            Bot.StartReceiving();
            Bot.OnMessage += OnMessegeHendler;
            Console.ReadLine();
            //Конец работы
            Bot.StopReceiving();
        }

        // асинхроная функция для ответа на сообщения
        private static async void OnMessegeHendler(object sender, MessageEventArgs e)
        {
            var Client_massge = e.Message;
            // Bit Coin curse 
            if (Client_massge != null) 
            {
                switch (Client_massge.Text) 
                {
                    case "/start":
                        await Bot.SendStickerAsync(
                            chatId: Client_massge.Chat.Id,
                            sticker: "https://tlgrm.ru/_/stickers/e65/38d/e6538d88-ed55-39d9-a67f-ad97feea9c01/192/71.webp",
                            replyMarkup: Getbuttons());
                        break;
                    case "Курс":
                        var sw = new Stopwatch();
                        sw.Start();
                        string[] Valute_URL = new string[] { "https://ru.investing.com/currencies/usd-rub", "https://ru.investing.com/currencies/eur-rub",
                "https://ru.investing.com/crypto/bitcoin/btc-usd?cid=1035793", "https://ru.investing.com/crypto/bitcoin/btc-eur?cid=1166554","https://ru.investing.com/crypto/bitcoin/btc-rub"};
                        string[] Name_valute = new string[] { "USD/RUB",
                    "EUR/RUB", "BTC/USD", "BTC/EUR", "BTC/RUB" };
                        int n = 0;
                        string Write_valute = "";
                        foreach (string i in Valute_URL)
                        {
                            GetCode(i);
                            Write_valute += Name_valute[n] + $" :{Valute}" + "\n";
                            n++;
                        }
                        await Bot.SendTextMessageAsync(Client_massge.Chat.Id, Write_valute);
                        await Bot.SendTextMessageAsync(Client_massge.Chat.Id, "Найдено за :" + Convert.ToString(sw.Elapsed)) ;
                        await Bot.SendStickerAsync(
                            chatId: Client_massge.Chat.Id,
                            sticker: "https://tlgrm.ru/_/stickers/e65/38d/e6538d88-ed55-39d9-a67f-ad97feea9c01/192/64.webp",
                            replyToMessageId: Client_massge.MessageId,replyMarkup:Getbuttons());
                        break;
                    case "Туты" :
                        string tuts =
                            "https://zetcode.com/csharp/anglesharp-parse-html/ \n" +
                            "https://ru.stackoverflow.com/questions/622823/%D0%9D%D1%83%D0%B6%D0%BD%D0%BE-%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B8%D1%82%D1%8C-%D0%BD%D0%B0-c-%D0%B8%D1%81%D1%85%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9-%D1%82%D0%B5%D0%BA%D1%81%D1%82-html-%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B8%D1%86%D1%8B \n" +
                            "https://www.youtube.com/watch?v=OTKJfGbgfxM";
                            await Bot.SendTextMessageAsync(Client_massge.Chat.Id, tuts);

                        break;
                    default:
                        await Bot.SendStickerAsync(
                            chatId: Client_massge.Chat.Id,
                            sticker: "https://tlgrm.ru/_/stickers/e65/38d/e6538d88-ed55-39d9-a67f-ad97feea9c01/192/14.webp",
                            replyToMessageId: Client_massge.MessageId,replyMarkup:Getbuttons());
                    
                        break;
                
                }
            }
            
            Console.WriteLine(Client_massge.Text);
        }

        
        // Переменная для вывода значений валюты
        public static string Valute;
        //Данный метод берет HTML страницы и делает из него текст 
        public static void GetCode(string urlAddress)
        {
            //string urlAddress = "http://google.com";
            string data = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            //Принятие куки файлов
            Cookie cookie = new Cookie
            {
                Name = "beget",
                Value = "begetok"
            };

            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(urlAddress), cookie);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }
                    data = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                }
                Info_about_valute(data);
            }
            catch 
            {
                Console.WriteLine("Error");
            }
        }
        // Данный метод вытискивает определенную информацию из Текста HTML кода и сохраняет в переменную valute
        async static void Info_about_valute(string html_code)
        {
            // Сохранение в строковую переменную кода html страницы

            var context = BrowsingContext.New(Configuration.Default);
            //обрата текста страницы для дальнейшей с ним работы 
            var doc = await context.OpenAsync(req => req.Content(html_code));
            //кладем данные которые задали фильрами : Тег div а Класс YMlKec fxKbKc также обязательно проверяем чтобы класс небыл пустым
            var info = doc.QuerySelectorAll("span").Where(item => item.ClassName != null && item.ClassName.Contains("text-2xl"));
            //Вывод отфильтрованных данных
            foreach (var i in info)
                Valute = i.Text();
        }
        //Кнопки 
        private static IReplyMarkup Getbuttons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton {Text = "Курс"}}
                }
            };
        }    
    }
}
