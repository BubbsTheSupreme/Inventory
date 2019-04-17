using System; 
using System.Data.SQLite;

namespace checklist
{
    public class inventory {

        files fileC = new files();

        public bool unchecked_status(string item, int id, string list){
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = "SELECT * FROM inventory";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                    using(SQLiteDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            if(reader["status"].Equals("[ ]") && reader["item"].Equals($"{item}")){
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool checked_status(string item, int id, string list){
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = "SELECT * FROM inventory";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                    using(SQLiteDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            if(reader["status"].Equals("[X]") && reader["item"].Equals($"{item}")){
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        
        public void new_list(string name){
            try {
                if(fileC.check_for_list(name) == true){
                    Console.WriteLine($"the list named {name} already exists.. please use another name.");
                }
                else{ 
                    SQLiteConnection.CreateFile($"{name}.db");
                    using(SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {name}.db; Version = 3;")){
                        db_connection.Open();
                        string sql = "CREATE TABLE inventory (id INTEGER PRIMARY KEY AUTOINCREMENT, status VARCHAR(12), item VARCHAR(150), price REAL, date VARCHAR(50), description VARCHAR(255))";
                        using(SQLiteCommand command = new SQLiteCommand(sql, db_connection)){
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("\n");
                    while(true){
                        Console.WriteLine("Enter the name of the item. to exit type 'quit' \n");
                        Console.Write("> ");
                        string item = Console.ReadLine();
                        if(item == "quit" || item == "Quit") {
                            break;
                        }
                        Console.WriteLine("Enter the price of the item, or to skip type 'skip' \n");
                        Console.Write("> ");
                        float price = float.Parse(Console.ReadLine());
                        if(item == "skip" || item == "Skip") {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Skipped! \n");
                            Console.ResetColor();
                            continue;
                        }
                        Console.WriteLine("Enter the date item was purchased MM/DD/YY format, or to skip type 'skip' \n");
                        Console.Write("> ");
                        string date = Console.ReadLine();
                        if(item == "skip" || item == "Skip") {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Skipped! \n");
                            Console.ResetColor();                            
                            continue;
                        }
                        Console.WriteLine("Enter a small description for the item, only 300 characters are permitted in this field. or to skip type 'skip' \n");
                        Console.Write("> ");
                        string desc = Console.ReadLine();
                        if(item == "skip" || item == "Skip") {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Skipped! \n");
                            Console.ResetColor();
                            continue;
                        }
                        using(SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {name}.db; Version = 3;")){
                            db_connection.Open();
                            string sql = $"INSERT INTO inventory (status, item, price, date, description) VALUES ('[ ]', '{item}', '{price}', '{date}', '{desc}')"; 
                            using(SQLiteCommand command = new SQLiteCommand(sql, db_connection)){
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"List {name} created. \n");
                    Console.ResetColor();
                }
            }
            catch(System.IO.IOException e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error {e} your lists' file is in use");
                Console.ResetColor();
            }
        }

        public void view_list(string list){
            Console.Clear();
            while(true) {
                try {
                    using(SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        db_connection.Open();
                        string sql = "SELECT * FROM inventory";
                        using(SQLiteCommand command = new SQLiteCommand(sql, db_connection)){
                            command.ExecuteNonQuery();
                            using(SQLiteDataReader reader = command.ExecuteReader()){
                                Console.WriteLine("\nThe number behind each '>' with the item is that item's primary key.");
                                Console.WriteLine("Some functions need the primary key to work. keep this in mind.\n");
                                while(reader.Read()){
                                    Console.WriteLine( reader["id"] + " > " + " " + reader["status"] + " " + reader["item"] + " " + reader["price"] + " " + reader["date"]);
                                    Console.WriteLine("description: " + reader["description"] + "\n");
                                }
                            }
                        }
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("Press enter to continue..");
                    Console.ReadLine();    
                    Console.WriteLine(" ___________________________________________________________________");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|        Type check if you want to check an item off the list.      |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|     Type uncheck if you want to uncheck an item off the list.     |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|       Type remove if you want to remove item from the list        |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|           Type add if you want to add item to the list            |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|              Type quit if you want to exit to menu.               |");
                    Console.WriteLine("|___________________________________________________________________|");
                    Console.Write("> ");
                    string user_input = Console.ReadLine();
                    if(user_input == "check" || user_input == "Check"){
                        while(true){
                            checkinput:
                            Console.WriteLine("What item do you want to check? Type 'quit' to exit.");
                            Console.Write("> ");
                            string item = Console.ReadLine();
                            if(item == "quit" || item == "Quit"){
                                break;
                            }
                            if(item == "" || item == " "){
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please input a item name..");
                                Console.ResetColor();
                                goto checkinput; 
                            }
                            else{
                                Console.WriteLine($"What is the Primary Key for {item}?");
                                Console.Write("> ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                check(item, list, id);
                            }
                        }
                    }

                    else if(user_input == "uncheck" || user_input == "Uncheck"){
                        while(true){
                            uncheckedinput:
                            Console.WriteLine("What item do you want to uncheck? Type quit to exit.");
                            Console.Write("> ");
                            string item = Console.ReadLine();
                            if(item == "quit" || item == "Quit"){
                                break;
                            }
                            if(item == "" || item == " "){
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please input a item name..");
                                Console.ResetColor();
                                goto uncheckedinput;
                            }
                            else{
                                Console.WriteLine($"What is the Primary Key for {item}?");
                                Console.Write("> ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                uncheck(item, list, id);
                            }
                        }
                    }

                    else if(user_input == "add" || user_input == "Add"){
                        while(true) {
                            here:
                            Console.WriteLine("What is the item called? To stop adding to list type quit");
                            Console.Write("> ");
                            string item = Console.ReadLine();
                            if(item == "quit" || item == "Quit"){
                                break;
                            }
                            if(item == "" || item == " "){
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please input a item name..");
                                Console.ResetColor();
                                goto here;
                            }
                            Console.WriteLine("Enter the price of the item \n");
                            Console.Write("> ");
                            float price = float.Parse(Console.ReadLine());
                            here2:
                            Console.WriteLine("Enter the date item was purchased MM/DD/YY format, or to skip type 'skip' \n");
                            Console.Write("> ");
                            string date = Console.ReadLine();
                            if(date == "skip" || date == "Skip") {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Skipped! \n");
                                Console.ResetColor();                            
                                continue;
                            }
                            else if(date == "" || date == " "){
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please input a date..");
                                Console.ResetColor();
                                goto here2;
                            }
                            here3:
                            Console.WriteLine("Enter a small description for the item, only 300 characters are permitted in this field. or to skip type 'skip' \n");
                            Console.Write("> ");
                            string desc = Console.ReadLine();
                            if(desc == "skip" || desc == "Skip") {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Skipped! \n");
                                Console.ResetColor();
                                continue;
                            }
                            else if(desc == "" || desc == " "){
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please input a description..");
                                Console.ResetColor();
                                goto here3;
                            }
                            add_item(item, price, date, desc, list);
                        }               
                    }

                    else if(user_input == "remove" || user_input == "Remove"){
                        while(true){
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Once an item is removed the primary key will not change.");
                            Console.WriteLine("Its as if the item was never removed. Keep this in mind.");
                            Console.ResetColor();
                            removeitem:
                            Console.WriteLine("What item do you want to remove? Type quit to exit.");
                            Console.Write("> ");
                            string item = Console.ReadLine();
                            if(item == "quit" || item == "Quit"){
                                break;
                            }
                            if(item == "" || item == " "){
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Please input a item name..");
                                Console.ResetColor();
                                goto removeitem;
                            }
                            else{
                                Console.WriteLine($"What is the Primary Key for {item}?");
                                Console.Write("> ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                remove_item(item, id, list);
                            }
                        }
                    }

                    else if(user_input == "quit" || user_input == "Quit") {
                        break;
                    }
                    
                    else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{user_input} is not a valid command, please enter a valid input..");
                        Console.ResetColor();
                    }
                }

                catch(System.Data.SQLite.SQLiteException e){
                    SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;");
                    db_connection.Open();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR {e} \n");
                    Console.ResetColor();
                    string sql = "CREATE TABLE inventory (id INTEGER PRIMARY KEY AUTOINCREMENT, status VARCHAR(12), item VARCHAR(150), price REAL, date VARCHAR(50), description VARCHAR(255))";
                    SQLiteCommand command = new SQLiteCommand(sql, db_connection);
                    command.ExecuteNonQuery();
                    db_connection.Close();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Error resolved");
                    Console.ResetColor();
                }
            }
        }
        public void check(string item, string list, int id){
            try {
                if(checked_status(item, id, list) == true){
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{item} has already been checked.");
                    Console.ResetColor();
                }
                else {
                    using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        connection.Open();
                        string sql = $"UPDATE inventory SET status = '[X]' WHERE id = '{id}'";
                        using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.Clear();
                    using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        connection.Open();
                        string sql = $"SELECT * FROM inventory";
                        using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                            command.ExecuteNonQuery();
                            using(SQLiteDataReader reader = command.ExecuteReader()){
                                while(reader.Read()){
                                    Console.WriteLine( reader["id"] + " > " + " " + reader["status"] + " " + reader["item"] + " " + reader["price"] + " " + reader["date"] + "\n");
                                }
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{item} has been checked! \n");
                    Console.ResetColor();
                }
            }
            catch(Exception e){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR {e} \n");
                Console.ResetColor();
            }
        }

        public void uncheck(string item, string list, int id){
            try {
                if(unchecked_status(item, id, list) == true){
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{item} is already unchecked.");
                    Console.ResetColor();
                }
                else{
                    using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){ 
                        connection.Open();
                        string sql = $"UPDATE inventory SET status = '[ ]' WHERE id = '{id}'";
                        using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.Clear();
                    using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        connection.Open();
                        string sql = $"SELECT * FROM inventory";
                        using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                            command.ExecuteNonQuery();
                            using(SQLiteDataReader reader = command.ExecuteReader()){
                                while(reader.Read()){
                                    Console.WriteLine( reader["id"] + " > " + " " + reader["status"] + " " + reader["item"] + " " + reader["price"] + " " + reader["date"] + "\n");
                                }
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{item} has been unchecked! \n");
                    Console.ResetColor();
                }
            } 
            catch(Exception e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }
        }

        public void add_item(string item, float price, string date, string desc, string list) {
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = $"INSERT INTO inventory (status, item, price, date, description) VALUES ('[ ]', '{item}', '{price}', '{date}', '{desc}')";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{item} has been added! \n");
            Console.ResetColor();
        }

        public void remove_item(string item, int id, string list) {
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = $"DELETE FROM inventory WHERE id = '{id}' AND item = '{item}'";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                }
            }
            Console.Clear();
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = $"SELECT * FROM inventory";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                    using(SQLiteDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            Console.WriteLine( reader["id"] + " > " + " " + reader["status"] + " " + reader["item"] + " " + reader["price"] + " " + reader["date"] + "\n");
                        }
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{item} has been removed! \n");
            Console.ResetColor();
        }
    } 
}
