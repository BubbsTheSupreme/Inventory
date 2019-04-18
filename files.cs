using System;
using System.IO;

namespace checklist {
    class files {

        public void show_all_lists(){
             string dir = Directory.GetCurrentDirectory();
             foreach(string i in Directory.GetFiles(dir)){
                 string result = Path.GetFileName(i);
                 if(i.EndsWith(".db")){
                     Console.WriteLine(   $"> {result}");
                 }
             }
        }

        public bool check_for_list(string name){
            string dir = Directory.GetCurrentDirectory();
            if(File.Exists(dir + "\\" + $"{name}.db")){
                return true;
            }
            return false;
        }
        public void delete_list(string name){
            string dir = Directory.GetCurrentDirectory();
            if(File.Exists(dir + "\\" + $"{name}.db")){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Type 'yes' if you are sure you want to delete the list: '{name}'");
                Console.WriteLine("Type 'no' if you arent sure. \n");
                Console.ResetColor();
                Console.Write("> ");
                string confirm = Console.ReadLine();
                switch(confirm){
                    case "yes":
                        File.Delete(dir + $"\\{name}.db");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"The list, {name} has been successfully deleted!");
                        Console.ResetColor();
                        break;
                    case "no":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"List has not been deleted.");
                        Console.ResetColor();
                        break;
                    default:
                        Console.WriteLine("Unknown input.");
                        break;
                }
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File not found..");
                Console.ResetColor();
            }
        }
    }
}