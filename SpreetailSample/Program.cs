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
                if (string.IsNullOrWhiteSpace(input)) continue;

                var (valid, command) = process.IsValid(input);
                if (valid)
                {
                    //We know the command is valid now, so we can convert the call back into a key to invoke the proper function
                    var key = (CommandEnum)Enum.Parse(typeof(CommandEnum), command[0].ToUpper());

                    //At this point, we should be able to index the command for function calls without out-of-bounds exceptions
                    //Lets try, catch for exceptions anyway
                    try
                    {
                        process.CommandFunctions[key].DynamicInvoke(command.Skip(1).ToArray());
                        Console.WriteLine();
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