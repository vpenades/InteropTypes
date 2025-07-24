// See https://aka.ms/new-console-template for more information
using InteropTypes;

Console.WriteLine("Running Tests...");

FilePreviewTests.RunTests();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Completed!");
Console.ResetColor();
