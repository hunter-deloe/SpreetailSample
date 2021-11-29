using System;

namespace SpreetailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process();

            Console.WriteLine("To exit, use ctrl-break (ctrl-c)");
            Console.WriteLine("Use `HELP` to view commands.");

            while (true)
            {
                Console.Write("> ");

                var input = Console.ReadLine();

                var (valid, command) = process.IsValid(input);
                if (valid)
                {
                    //We know the command is valid now, so we can convert the call back into a key to invoke the proper function
                    var key = (CommandEnum)Enum.Parse(typeof(CommandEnum), command[0]);

                    //At this point, we should be able to index the command for function calls without out-of-bounds exceptions
                    //Lets try, catch for exceptions anyway
                    try
                    {
                        switch (key)
                        {
                            case CommandEnum.KEYS:
                                process.GetKeys();
                                break;
                            case CommandEnum.MEMBERS:
                                process.GetMembers(command[1]);
                                break;
                            case CommandEnum.ADD:
                                process.Add(command[1], command[2]);
                                break;
                            case CommandEnum.REMOVE:
                                process.Remove(command[1], command[2]);
                                break;
                            case CommandEnum.REMOVEALL:
                                process.RemoveAll(command[1]);
                                break;
                            case CommandEnum.CLEAR:
                                process.Clear();
                                break;
                            case CommandEnum.KEYEXISTS:
                                process.KeyExists(command[1]);
                                break;
                            case CommandEnum.MEMBEREXISTS:
                                process.MemberExists(command[1], command[2]);
                                break;
                            case CommandEnum.ALLMEMBERS:
                                process.GetAllMembers();
                                break;
                            case CommandEnum.ITEMS:
                                process.GetItems();
                                break;
                            case CommandEnum.HELP:
                                process.Help();
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}