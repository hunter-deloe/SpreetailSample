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
        /// Defines a command and its expected length of arguments
        /// </summary>
        private readonly Dictionary<CommandEnum, int> _validCommands = new Dictionary<CommandEnum, int>()
        {
            [CommandEnum.KEYS] = 1,
            [CommandEnum.MEMBERS] = 2,
            [CommandEnum.ADD] = 3,
            [CommandEnum.REMOVE] = 3,
            [CommandEnum.REMOVEALL] = 2,
            [CommandEnum.CLEAR] = 1,
            [CommandEnum.KEYEXISTS] = 2,
            [CommandEnum.MEMBEREXISTS] = 3,
            [CommandEnum.ALLMEMBERS] = 1,
            [CommandEnum.ITEMS] = 1,
            [CommandEnum.HELP] = 1,
            [CommandEnum.CLS] = 1
        };

        /// <summary>
        /// Defines a command and its function
        /// </summary>
        public readonly Dictionary<CommandEnum, Delegate> CommandFunctions = new Dictionary<CommandEnum, Delegate>() 
        {
            [CommandEnum.KEYS] = new Action(GetKeys),
            [CommandEnum.MEMBERS] = new Action<string>(GetMembers),
            [CommandEnum.ADD] = new Action<string, string>(Add),
            [CommandEnum.REMOVE] = new Action<string, string>(Remove),
            [CommandEnum.REMOVEALL] = new Action<string>(RemoveAll),
            [CommandEnum.CLEAR] = new Action(Clear),
            [CommandEnum.KEYEXISTS] = new Action<string>(KeyExists),
            [CommandEnum.MEMBEREXISTS] = new Action<string, string>(MemberExists),
            [CommandEnum.ALLMEMBERS] = new Action(GetAllMembers),
            [CommandEnum.ITEMS] = new Action(GetItems),
            [CommandEnum.HELP] = new Action(Help),
            [CommandEnum.CLS] =  new Action(Console.Clear)
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

                //And that it covers required arguments
                if (_validCommands[key] == command.Length)
                {
                    return (true, key, command.Skip(1).ToArray());
                }
            }

            Console.WriteLine("Please enter a valid command. Use `HELP` to view commands.");
            return (false, null, null);
        }

        /// <summary>
        /// Returns all keys in the dictionary.
        /// </summary>
        public static void GetKeys()
        {
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
        public static void GetMembers(string key)
        {
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
        public static void GetAllMembers()
        {
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
        public static void GetItems()
        {
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
        public static void KeyExists(string key)
        {
            Console.WriteLine($") {_dictionary.ContainsKey(key)}");
        }

        /// <summary>
        /// Returns whether a member exists within a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public static void MemberExists(string key, string member)
        {
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
        public static void Remove(string key, string member)
        {
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
        public static void RemoveAll(string key)
        {
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
        public static void Clear()
        {
            _dictionary.Clear();
            Console.WriteLine(") Cleared");
        }

        /// <summary>
        /// Adds a member to a collection for a given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public static void Add(string key, string member)
        {
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
        /// Displays commands
        /// </summary>
        public static void Help()
        {
            var lines = File.ReadAllLines("Help.txt");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
        #endregion
    }
}
