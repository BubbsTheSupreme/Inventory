using System;
 
namespace checklist {
    class Program {
        
        static void menu(){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" _________________________________________________ ");
            Console.WriteLine("|                INVENTORY MENU                   |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|           Enter 'new' for new list              |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|           Enter 'view' to view list             |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|   Enter 'lists' to see all the existing lists   |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|        Enter 'delete' to delete a list          |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|     Enter 'quit' to return to menu from list    |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|              Enter 'quit' to quit               |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|     If you select view and the list doesnt      |");
            Console.WriteLine("|   exist an empty list will be created for you   |");
            Console.WriteLine("|_________________________________________________|");
            Console.ResetColor();
        }

        static void Main(string[] args){
            inventory inv = new inventory();
            files file = new files();
            while(true){
                menu();
                Console.Write("> ");
                string input = Console.ReadLine();
                switch(input){
                    case "new":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.Write("What do you want the list to be called? \n");
                        Console.Write("List name: ");
                        input = Console.ReadLine();
                        inv.new_list(input);
                        break;
                    case "view":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.Write("What list do you want to view? \n");
                        Console.Write("List name: ");
                        input = Console.ReadLine();
                        inv.view_list(input);
                        break;
                    case "lists":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        file.show_all_lists();
                        Console.ResetColor();
                        break;
                    case "delete":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.Write("What list do you want to delete? \n");
                        Console.Write("List name: ");
                        input = Console.ReadLine();
                        file.delete_list(input);
                        break;
                    case "quit":
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
