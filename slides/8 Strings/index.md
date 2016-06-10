- title : C# - Working with strings and text
- description : Working with strings and text
- author : Programulya
- theme : league
- transition : default

***

## C# course
#### Lecture 8
# Working with strings and text

***

### System.String type

A string is an object of type String whose value is text. Internally, the text is stored as a sequential read-only collection of Char objects.

<br/>
<br/>

<div class="fragment">
In C#, the string keyword is an alias for String.
</div>

***

### Strings initialization

```cs
// Declare without initializing. 
string message1;

// Initialize to null. 
string message2 = null;

// Initialize as an empty string. 
// Use the Empty constant instead of the literal "".
string message3 = System.String.Empty;

// Initialize with a regular string literal. 
string oldPath = "c:\\Program Files\\Microsoft Visual Studio 8.0";

// Initialize with a verbatim string literal. 
string newPath = @"c:\Program Files\Microsoft Visual Studio 9.0";
```

***

### Strings initialization

```cs
// Use System.String if you prefer.
System.String greeting = "Hello World!";

// In local variables (i.e. within a method body) 
// you can use implicit typing. 
var temp = "I'm still a strongly-typed System.String!";

// Use a const string to prevent 'message4' from 
// being used to store another string value. 
const string message4 = "You can't get rid of me!";

// Use the String constructor only when creating 
// a string from a char*, char[], or sbyte*. 
// See System.String documentation for details. 
char[] letters = { 'A', 'B', 'C' };
string alphabet = new string(letters);
```

***

### Constructors overview

- **String(Char*)** by a pointer to array of Unicode chars (UC).
- **String(Char[])** by an array of UC.
- **String(SByte*)** by a pointer to array of 8-bit signed ints.
- **String(Char, Int32)** by a UC repeated a number of times.
- **String(Char*, Int32, Int32)** by a specified pointer to array of UC, 
starting char position in array and length.
- **String(Char[], Int32, Int32)** by array of UC, 
starting char position in array and length.
- **String(SByte*, Int32, Int32)** by a pointer to array of 8-bit signed ints, 
a starting position in array and length.
- **String(SByte*, Int32, Int32, Encoding)** by a pointer to an array of 8-bit signed ints, 
a starting position in that array, a length, and an Encoding object.

***

### Strings initialization (example)

```cs
// CS code
public static class Program
{
    public static void Main()
    {
        string s = "Hi there.";
        System.Console.WriteLine(s);
    }
}
```

```cs
// Intermediate language (IL):
.method public hidebysig static void Main() cil managed 
{ 
    .entrypoint 
    // Code size 13 (0xd) 
    .maxstack 1 
    .locals init (string V_0) 
    IL_0000: ldstr "Hi there." 
    IL_0005: stloc.0 IL_0006: ldloc.0 
    IL_0007: call void [mscorlib]System.Console::WriteLine(string) IL_000c: ret 
} // end of method Program::Main
```

***

### String as a reference type

#### It is a reference type
It's a common misconception that string is a value type. That's because its 
immutability (see next point) makes it act sort of like a value type. It actually acts like a normal *reference type*.

[Why is string a reference type](http://stackoverflow.com/questions/636932/in-c-why-is-string-a-reference-type-that-behaves-like-a-value-type)

***

### String immutability

#### It's immutable
String objects are *immutable*: they cannot be changed after they have been created. 
All of the String methods and C# operators that appear to modify a string actually return the results in a new string object.

+ [Example 1](https://dotnetfiddle.net/nETRFL)
+ [Example 2](https://dotnetfiddle.net/sHPCbZ)

***
### Equality operator and strings

#### It overloads the == operator
When operator '==' applied to strings, the *Equals* method is called.
It checks for the equality strings contents rather than the references. 

```cs
 "hello".Substring(0, 4)=="hell"; // Is true 
  // even though the references on the two sides of the operator are different
  // (they refer to two different string objects, 
  // which both contain the same character sequence). 
```
 
```cs
static Int32 Compare(String strA, String strB, StringComparison comparisonType);
static Int32 Compare(string strA, string strB, Boolean ignoreCase, CultureInfo culture);
// ...
Boolean StartsWith(String value, StringComparison comparisonType); 
Boolean StartsWith(String value, Boolean ignoreCase, CultureInfo culture);
Boolean EndsWith(String value, StringComparison comparisonType);
Boolean EndsWith(String value, Boolean ignoreCase, CultureInfo culture);
````

***

### Compare strings

#### Note! Think about case.

Use *StringComparison* and *CompareOptions*:

[Example](https://dotnetfiddle.net/dY4r91)

***

### Compare strings (best practices)

1. Use *CurrentCulture/CurrentCultureIgnoreCase* just before output to a user.
2. If comparing without culture use *Ordinal* instead of *InvariantCulture* (slow).
3. If you want to change case before comparison use *ToUpperInvariant* instead of *ToLowerInvariant* (first option is optimized).
4. Avoid *CompareTo*, *CompareOrdinal*, operators *==* and *!=* because you don’t see clearly the way of comparison.
For example, CompareTo uses regional standards, but Equals is not.
5. Use an overload of the *String.Equals* to test strings equality.

***

### Localization and strings

1. *System.Globalization.CultureInfo*;
2. *CurrentUICulture*;
3. *CurrentCulture*.

Some info:

- Usually *CurrentCulture* and *CurrentUICulture* are the same.
- Culture could be set from ‘Regional and Language Options’ in Windows.
- Compare method uses current thread’s regional standards if you do not put it explicitly.

[Example](https://dotnetfiddle.net/EZIutS)

***

### Strings interning

Reasons:

- Slow comparison;
- A lot of memory usage.

**Solution:** CLR has mechanism of optimization named ‘string interning’ (special hash-table).

```cs
// Methods for interning
public static String Intern(String str); 
public static String IsInterned(String str);
```

You could turn off for project this mechanism using attribute *System.Runtime.CompilerServices.CompilationRelaxations.NoStringInterning*.

[Example](https://dotnetfiddle.net/ggU5Fu)

***

#### String other methods

Length,
Chars, 
GetEnumerator, 
ToCharArray, 
Contains, 
IndexOf, 
LastIndexOf, 
IndexOfAny, 
LastIndexOfAny
Clone, 
Copy, 
CopyTo, 
Substring, 
ToString, 
Insert, Remove, PadLeft, Replace, Split, Join, ToLower, ToUpper, Trim, Concat, Format…

#### Note! All methods return NEW string.

***
### String vs StringBuilder

StringBuilder provides mutable string:

```cs
StringBuilder sb = new StringBuilder(); 
String s = sb.AppendFormat("{0} {1}", "Kottans1111", "Org").
	   Replace(' ', '-').Remove(4, 3).ToString(); 
Console.WriteLine(s); // "Kottans-Org"
```

***

### Encode/decode strings

Encodings:

- UTF-16;
- UTF-8;
- UTF-32;
- UTF-7;
- ASCII.

The most popular:

- UTF-16;
- UTF-8.

[Example](https://dotnetfiddle.net/7KCNtS)

***
### Useful links

1. [String class - MSDN](https://msdn.microsoft.com/ru-ru/library/system.string(v=vs.110).aspx)
2. [Strings - best practices](https://msdn.microsoft.com/en-us/library/dd465121(v=vs.110).aspx)
3. [String Interning](http://blog.jetbrains.com/dotnet/2015/02/12/string-interning-effective-memory-management-with-dotmemory)
4. [Why does C# use UTF-16 for strings?](http://blog.coverity.com/2014/04/09/why-utf-16/#.VZWxSFyqqko)
5. [Working with Strings - Channel9](https://channel9.msdn.com/Series/C-Sharp-Fundamentals-Development-for-Absolute-Beginners/Working-with-Strings-12)

**And… If you google ‘CString’ and go to the images you will forget what are you looking for ;)
 [Wow!](https://www.google.com.ua/search?q=cstring&espv=2&biw=1600&bih=775&source=lnms&tbm=isch&sa=X&ei=bWuWVe7dHMKPsAGd96u4BA&ved=0CAYQ_AUoAQ) **

























