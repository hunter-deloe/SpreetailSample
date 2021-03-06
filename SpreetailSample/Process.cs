using System.Text.RegularExpressions;

namespace SpreetailSample
{
    public class Process
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Process()
        {
            _dictionary = new Dictionary<string, HashSet<string>>();
        }

        #region DATA
        /// <summary>
        /// Dictionary to hold our data
        /// </summary>
        private static Dictionary<string, HashSet<string>> _dictionary { get; set; }

        /// <summary>
        /// Defines a command and its function
        /// </summary>
        public readonly Dictionary<CommandEnum, Action<string[]>> CommandFunctions = new Dictionary<CommandEnum, Action<string[]>>() 
        {
            [CommandEnum.KEYS] = new Action<string[]>(GetKeys),
            [CommandEnum.MEMBERS] = new Action<string[]>(GetMembers),
            [CommandEnum.ADD] = new Action<string[]>(Add),
            [CommandEnum.REMOVE] = new Action<string[]>(Remove),
            [CommandEnum.REMOVEALL] = new Action<string[]>(RemoveAll),
            [CommandEnum.CLEAR] = new Action<string[]>(Clear),
            [CommandEnum.KEYEXISTS] = new Action<string[]>(KeyExists),
            [CommandEnum.MEMBEREXISTS] = new Action<string[]>(MemberExists),
            [CommandEnum.ALLMEMBERS] = new Action<string[]>(GetAllMembers),
            [CommandEnum.ITEMS] = new Action<string[]>(GetItems),
            [CommandEnum.HELP] = new Action<string[]>(Help),
            [CommandEnum.CLS] =  new Action<string[]>(ClearConsole)
        };
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Converts the input into a command and arguments
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Bool for validity, command enum, command args</returns>
        public (bool, CommandEnum?, string[]?) ConvertInput(string input)
        {
            //clean the input
            input = Regex.Replace(input, @"\s+", " ");
            var command = input.Trim().Split(' ');

            //Make sure the user invokes one of the valid commands
            if (Enum.GetNames(typeof(CommandEnum)).AsEnumerable().Contains(command[0].ToUpper()))
            {
                var key = (CommandEnum) Enum.Parse(typeof(CommandEnum), command[0].ToUpper());
                return (true, key, command.Skip(1).ToArray());
            }

            Console.WriteLine("- Please enter a valid command. Use `HELP` to view commands.\n");
            return (false, null, null);
        }

        /// <summary>
        /// Returns all keys in the dictionary.
        /// </summary>
        public static void GetKeys(string[] args)
        {
            if(args.Length != 0)
            {
                Console.WriteLine("- KEYS should not be invoked with an argument.");
                return;
            }

            if(_dictionary.Count == 0)
            {
                Console.WriteLine("(empty set)");
            }
            else
            {
                var i = 1;
                foreach (var key in _dictionary.Keys)
                {
                    Console.WriteLine($"{i}) {key}");
                    i++;
                }
            }
        }

        /// <summary>
        /// Returns the collection of strings for the given key
        /// </summary>
        /// <param name="key"></param>
        public static void GetMembers(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("- MEMBERS must be invoked with 1 argument. Ex: MEMBERS foo");
                return;
            }

            var key = args[0];

            if (!_dictionary.ContainsKey(key))
            {
                Console.WriteLine(") ERROR, key does not exist");
            }
            else
            {
                var i = 1;
                foreach (var member in _dictionary[key])
                {
                    Console.WriteLine($"{i}) {member}");
                    i++;
                }
            }
        }

        /// <summary>
        /// Returns all members in the dictionary
        /// </summary>
        public static void GetAllMembers(string[] args)
        {
            if(args.Length != 0)
            {
                Console.WriteLine("- ALLMEMBERS should not be invoked with an argument");
                return;
            }

            if(_dictionary.Count == 0)
            {
                Console.WriteLine("(empty set)");
            }
            else
            {
                var i = 1;
                foreach(var key in _dictionary.Keys)
                {
                    foreach(var member in _dictionary[key])
                    {
                        Console.WriteLine($"{i}) {member}");
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Returns all keys in the dictionary and their members
        /// </summary>
        public static void GetItems(string[] args)
        {
            if(args.Length != 0)
            {
                Console.WriteLine("- ITEMS should not be invoked with an argument");
                return;
            }

            if(_dictionary.Count == 0)
            {
                Console.WriteLine("(empty set)");
            }
            else
            {
                var i = 1;
                foreach(var key in _dictionary.Keys)
                {
                    foreach(var member in _dictionary[key])
                    {
                        Console.WriteLine($"{i}) {key}: {member}");
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Returns whether a key exists or not
        /// </summary>
        /// <param name="key"></param>
        public static void KeyExists(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("- KEYEXISTS should be invoked with 1 argument. Ex: KEYEXISTS foo");
                return;
            }

            Console.WriteLine($") {_dictionary.ContainsKey(args[0])}");
        }

        /// <summary>
        /// Returns whether a member exists within a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public static void MemberExists(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("- MEMBEREXISTS should be invoked with 2 arguments. Ex: MEMBEREXISTS foo bar");
                return;
            }

            var key = args[0];
            var member = args[1];

            if (!_dictionary.ContainsKey(key))
            {
                Console.WriteLine(") false");
            }
            else
            {
                Console.WriteLine($") {_dictionary[key].Contains(member)}");
            }
        }

        /// <summary>
        /// Removes a member from a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public static void Remove(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("- REMOVE must be invoked with 2 arguments. Ex: REMOVE foo bar");
                return;
            }

            var key = args[0];
            var member = args[1];

            if (!_dictionary.ContainsKey(key))
            {
                Console.WriteLine(") ERROR, key does not exist");
            }
            else
            {
                if (!_dictionary[key].Remove(member))
                {
                    Console.WriteLine(") ERROR, member does not exist");
                }
                else
                {
                    Console.WriteLine(") Removed");
                    if (_dictionary[key].Count == 0)
                    {
                        _dictionary.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all members for a key and removes the key from the dictionary
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveAll(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("- REMOVEALL must be invoked with 1 argument. Ex: REMOVEALL foo");
                return;
            }

            var key = args[0];

            if (!_dictionary.ContainsKey(key))
            {
                Console.WriteLine(") ERROR, key does not exist");
            }
            else
            {
                _dictionary.Remove(key);
                Console.WriteLine(") Removed");
            }
        }

        /// <summary>
        /// Removes all keys and members from the dictionary
        /// </summary>
        public static void Clear(string[] args)
        {
            if(args.Length != 0)
            {
                Console.WriteLine("- CLEAR should not be invoked with an argument.");
                return;
            }

            _dictionary.Clear();
            Console.WriteLine(") Cleared");
        }

        /// <summary>
        /// Adds a member to a collection for a given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public static void Add(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("- ADD should be invoked with 2 arguments. Ex: ADD foo bar");
                return;
            }

            var key = args[0];
            var member = args[1];

            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, new HashSet<string>() { member });
                Console.WriteLine(") Added");
            }
            else
            {
                if (!_dictionary[key].Add(member))
                {
                    Console.WriteLine(") ERROR, member already exists for key");
                }
                else
                {
                    Console.WriteLine(") Added");
                }
            }
        }

        /// <summary>
        /// Display help
        /// </summary>
        /// <param name="args"></param>
        public static void Help(string[] args)
        {
            if (args.Length != 0) 
            { 
                Console.WriteLine("- HELP should not be invoked with an argument.");
                return;
            }

            var lines = File.ReadAllLines("Help.txt");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Clear console
        /// </summary>
        /// <param name="args"></param>
        public static void ClearConsole(string[] args)
        {
            if (args.Length != 0) 
            { 
                Console.WriteLine("- CLS should not be invoked with an argument.");
                return;
            }

            Console.Clear();
        }
        #endregion
    }
}
