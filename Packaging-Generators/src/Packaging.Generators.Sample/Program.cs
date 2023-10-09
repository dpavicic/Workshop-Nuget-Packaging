using System;
using Packaging.Generators.Sample;

Console.Title = "Incremental Generator with Scriban Packaging Sample";

// Let's test our generator on two classes
var helloThereA = new HelloThereTestA();
helloThereA.HelloThere();

var helloThereB = new HelloThereTestB();
helloThereB.HelloThere();