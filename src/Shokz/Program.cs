using System.CommandLine;
using System.CommandLine.Parsing;
using Shokz;

var runner = CommandLine.CreateParser();
return await runner.InvokeAsync(args);
