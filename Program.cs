using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloNet;

namespace TrelloExample
{
    class Program
    {
        static void Usage()
        {
            Console.WriteLine("TrelloExample.exe -key <key> -token <token>");
            Console.WriteLine("\nGet an api key from the trello site and logging into\nhttps://trello.com/1/appKey/generate");
            Console.WriteLine("Run this app with the key to get a url\nto then obtain a token to use on subsequent runs");
        }

        static void Main(string[] args)
        {
            string key = null;
            string token = null;

            var options = new NDesk.Options.OptionSet
            {
                { "key=", k => key = k},
                { "token=", t => token = t}
            };

            options.Parse(args);

            if (string.IsNullOrWhiteSpace(key))
            {
                Usage();
                Environment.Exit(1);
            }

            var trello = new Trello(key);

            if (string.IsNullOrWhiteSpace(token))
            {
                var url = trello.GetAuthorizationUrl("TrelloExample", Scope.ReadOnly);
                Console.WriteLine("No token provided for this app.  Get one via this url:");
                Console.WriteLine(url);
                Usage();
                Environment.Exit(2);
            }
           
            trello.Authorize(token);
            foreach (var board in trello.Boards.ForMe())
            {
                Console.WriteLine("Board name: {0}", board.Name);

                foreach (var card in trello.Cards.ForBoard(board))
                {
                    Console.WriteLine("\tCard: {0}", card.Name);
                }
            }
        }
    }
}
