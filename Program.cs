// See https://aka.ms/new-console-template for more information
using ConsoleApp3;
using System.Text;

// Add services to the container.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


var blocks = new TTDXBlockReader().ParseBlockFile("block_gn.dat");

foreach (var block in blocks)
{
    Console.WriteLine(block.Name);
}


Console.ReadLine();