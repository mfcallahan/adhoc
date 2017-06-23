<Query Kind="Statements">
  <Namespace>System</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

string[] formats = {"MM/DD/YYYY"};

string orig = "07/01/2013";

var converted = DateTime.ParseExact(orig,formats,new CultureInfo("en-US"),DateTimeStyles.None);

Console.WriteLine("orig: " + orig);
Console.WriteLine("converted: ");
