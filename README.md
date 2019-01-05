## Why?

CSVGenerator will make your life easier when creating CSVs. 
The library supports full class parsing and will work with any object (reference types) or any built in .Net type(strings, ints, booleans, etc), using any enumerable(custom Enumerables, any Collection, any Set, etc).

## Platform

Full support for **.Net Standard 2.1**
- Windows
- Mac OS
- Linux

## Examples

### List of custom objects
```
ICollection items = new List<Test>()
{
    new Test() {Name = "First Entry", Cost = 1.2m, Value = 1 },
    new Test() {Name = "Venture \"Extended Edition, Very Large\""},
    new Test() {Name = null, Cost = 20, Value = 3}
};
string csv = items.ToCsv(',');
Console.WriteLine(csv);
File.WriteAllText(Directory.GetCurrentDirectory() + @"/" + "Test.csv", csv);
```
### List using an interface
```
IList items = new List<ITestInterface>()
{
    new Test() {Name = "First Entry", Cost = 1.2m, Value = 1 },
    new Test() {Name = "Venture \"Extended Edition, Very Large\""},
    new Test() {Name = null, Cost = 20, Value = 3}
};
string csv = items.ToCsv(',');
Console.WriteLine(csv);
File.WriteAllText(Directory.GetCurrentDirectory() + @"/" + "Test.csv", csv);
```
### Array of strings
```
string[] arr = new string[2] { "First Entry", "Venture \"Extended Edition, Very Large\"" };
string csv = arr.ToCsv(',');
Console.WriteLine(csv);
File.WriteAllText(Directory.GetCurrentDirectory() + @"/" + "Test.csv", csv);
```

### When using Reference types
- Properties will be used as Headers

### When using a built in .Net types
- Header will be set to "Values" by default
