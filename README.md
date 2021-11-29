Requires .NET Runtime 6.0.0 to run release: https://dotnet.microsoft.com/download/dotnet/6.0

To build, install SDK 6.0.100 from the above link.
You can also install Visual Studio Community here: https://visualstudio.microsoft.com/vs/community/

Commands (also available within the application)

| Command      | Description                           | Arguments                         |
|--------------|---------------------------------------|-----------------------------------|
| KEYS         | displays all keys in the dictionary   | Does not expect any arguments     |
| MEMBERS      | displays all members for a key        | Expects 1 argument (key)          |
| ADD          | adds a member for a given key         | Expects 2 arguments (key, member) |
| REMOVE       | removes a member from a key           | Expects 2 arguments (key, member) |
| REMOVEALL    | removes all members for a key         | Expects 1 argument (key)          |
| CLEAR        | removes all keys and members          | Does not expect any arguments     |
| KEYEXISTS    | returns whether a key exists          | Expects 1 argument (key)          |
| MEMBEREXISTS | returns whether a member exists       | Expects 2 arguments (key, member) |
| ALLMEMBERS   | returns all members                   | Does not expect any arguments     |
| ITEMS        | returns all keys and their members    | Does not expect any arguments     |
| HELP         | displays a list of available commands | Does not expect any arguments     |
