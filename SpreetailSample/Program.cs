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

                var (valid, cmd, cmdArgs) = process.ConvertInput(input);
                if (valid)
                {
                    //Try catch in case dynamic invoke blows up for some reason
                    try
                    {
                        //This exception should never be thrown
                        if (cmd == null) throw new Exception(") Command is null");
                        process.CommandFunctions[cmd.Value].DynamicInvoke((object)cmdArgs);

                        //Write line to improve readability
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