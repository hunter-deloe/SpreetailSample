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
        /// _dictionary to hold our data
        /// </summary>
        private Dictionary<string, HashSet<string>> _dictionary { get; set; }

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
            [CommandEnum.HELP] = 1
        };
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Validates the command and argument count
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Bool for validity, split list of command and arguments</returns>
        public (bool, string[]) IsValid(string input)
        {
            //clean the input
            input = Regex.Replace(input, @"\s+", " ");
            var command = input.Split(' ');

            //Make sure the user invokes one of the valid commands
            if (Enum.GetNames(typeof(CommandEnum)).AsEnumerable().Contains(command[0].ToUpper()))
            {
                var key = (CommandEnum) Enum.Parse(typeof(CommandEnum), command[0].ToUpper());

                //And that it covers required arguments
                if (_validCommands[key] == command.Length)
                {
                    return (true, command);
                }
            }

            Console.WriteLine("Please enter a valid command. Use `HELP` to view commands.");
            return (false, command);
        }

        /// <summary>
        /// Returns all keys in the dictionary.
        /// </summary>
        public void GetKeys()
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
        public void GetMembers(string key)
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
        public void GetAllMembers()
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
        public void GetItems()
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
        public void KeyExists(string key)
        {
            Console.WriteLine($") {_dictionary.ContainsKey(key)}");
        }

        /// <summary>
        /// Returns whether a member exists within a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public void MemberExists(string key, string member)
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
        public void Remove(string key, string member)
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
        public void RemoveAll(string key)
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
        public void Clear()
        {
            _dictionary.Clear();
            Console.WriteLine(") Cleared");
        }

        /// <summary>
        /// Adds a member to a collection for a given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public void Add(string key, string member)
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
        public void Help()
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
