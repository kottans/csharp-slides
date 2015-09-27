- title : Functional programming with C#
- description : Delegates, Events and Lambdas
- author : Valentine Radchuk\Dmytro Hnatyuk
- theme : league
- transition : default

***
## C# course
#### Lecture 7
# Delegates, Events and Lambdas

***
###Callbacks

**Callback** - is a method passed as argument and called at specific tume (after/before some kind of event)

Examples: 

- when button is clicked, a method called
- when downloading finished, method called

```js
function doSomething(integer, callbackFunction) {
    var i = 0, s = 0;
    for(i = 0; i<integer; i = i + 1) s += integer;
    callbackFunction(s);
}						
function callBackFunction(result) {
    print('result is: ' + result);						
}						
doSomething(5000, callBackFunction);
```
---
###Callback in C#

**Callback** methods can be implemented in C# using [delegates](https://msdn.microsoft.com/en-us/en-en/library/900fyy8e.aspx), [events](https://msdn.microsoft.com/en-us/en-en/library/8627sbea(v=vs.120).aspx) and [lambda expressions](https://msdn.microsoft.com/en-us/library/bb397687.aspx)

- delegate - "pointer" to a method(s)
- event - "syntax sugar" to implement publisher\subscriber pattern
- lambda expression - special syntax for anonymous delegates

***
###Delegate

**Delegate** is a reference type which holds:

- address of a method which it safely references to 
- signature of a method it can referecne to 
- return type of the method

Declaration example:

```cs
public delegate void TestDelegate(string message);
public delegate int TestDelegate(MyType m, long num);
```

[Invoke instance method example](https://dotnetfiddle.net/xPjvzE)
<br/>
[Invokation list example](https://dotnetfiddle.net/ULvc0X)
<br/>
[Real word example](https://dotnetfiddle.net/aCqstN)

---
###Delegates in a nutshell

```cs
// Following delegate type can point to whatever
// method as long as it's returning int and it has two
// parameters.
public delegate int Something(int a, int b);

// This will generate a following class! Lot of methods
// also deriving from MulticastDelegate and Delegate (base class
// for MulticastDelegate)
sealed class Something : System.MulticastDelegate
{
    public int Invoke(int x, int y);

    // Used for asynchronous call 
    public IAsyncResult BeginInvoke(int x, int y, AsyncCallback cb, object state);
    public int EndInvoke(IAsyncResult result);
}
```

---
###Operations on delegates

Assume we have a delegate and a method:

```cs
delegate void CustomDel(string s); 
// ...
public void Hello(string message) { Console.WriteLine(message); }
```

Assign method to delegate:

```cs
private CustomDel myDel = Hello;
private CustomDel myDel = Hello + Hello1; // multicast delegate
myDel += Hello2;
myDel = null;
```
Remove method from delegate:

```cs
myDel -= Hello;
```
	
***
###Anonymous methods

**Anonymous methods** let you declare a method body without giving it a name

Anonymous method without parameters:

```cs
PrintHelloWorld(delegate() {
    System.Console.Write("Hello, ");
    System.Console.WriteLine("World!");
});
```
Anonymous method with parameters:

```cs
PrintHelloWorld("Hello", delegate(string greetings) {
    System.Console.Write("{0}, ", greetings);
    System.Console.WriteLine("World!");
});
```

***
###Lambda expression - syntax sugar for delagates

Keyword _**delegate**_ might be avoided when anonymous method declared, instead use sign '=>':

```cs
PrintHelloWorld(delegate() {
    System.Console.WriteLine("Hello, World!");
});
//equals to 
PrintHelloWorld(() => {
    System.Console.WriteLine("Hello, World!");
});
```

```cs
PrintHelloWorld(delegate(string greetings, string message) {
    System.Console.WriteLine("{0}, {1}!", greetings, message);
});
//equals to 
PrintHelloWorld((greetings, message) => {
    System.Console.WriteLine("{0}, {1}!", greetings, message);
});
```

***
###Closures in C#

[Closure](https://en.wikipedia.org/wiki/Closure_(computer_programming)) -  is a data structure storing a function together with an environment (c) Wiki

A closure is attached to its parent method means that variables defined in parent's method body can be referenced from within the anonymous method.

```cs
public Person FindById(int id)
{
    return this.Find(delegate(Person p)
    {
        return (p.Id == id);
    });
}
```

[Closures explained (ru)](http://habrahabr.ru/post/141270/)

***
###Events

_**event**_ keyword is a modifier for a delegate instantiation that allows:

- it to be included in an interface
- constraints it invocation from within the class that declares it
- provides it with add/remove accessors 
- forces the signature of the delegate
- implement payyern publisher\subscriber

---
###Events and interfaces

An event can be included in an interface declaration, whereas a field cannot 

```cs
interface ITest {
    event MsgHandler msgNotifier; // compiles
    MsgHandler msgNotifier2; // error CS0525: Interfaces cannot contain fields
}

class TestClass : ITest {
    public event MsgHandler msgNotifier; 
    static void Main(string[] args) { }
}
```

---
###Event invocation

An event can be invoked from within the class that declared it

```cs
internal delegate void MsgHandler(string s);

internal class Class1 {
    public static event MsgHandler msgNotifier;
    public static MsgHandler msgNotifier2;

    private static void Main(string[] args) {
        new Class2().test();
    }
}
internal class Class2 {
    public void test() {
        Class1.msgNotifier("test"); // error CS0070
        Class1.msgNotifier2("test2"); // compiles fine
    }
}
```

[Event pattern example](https://dotnetfiddle.net/fPnn5Z)

***
###Predefined delegates

- Action - family of delegates that do not return value
- Func - family of delagates that return value
- Predicate - delegate wich returns bool

---
###Advantages of pre-defined delegates?

- save time from defining a delegates of similar definition
- use the predefined one which is also more commonly used
- with the invent of Extension methods and LINQ 

---
###Standard delegete - Action

Family of generic delegates which does NOT return a value

```cs
  public delegate void Action<T1, T2>(T1 param1, T2 param2);
  // ...
  Action 
  Action<T> 
  Action<T1, T2> 
  Action<T1, T2, T3> 
  Action< ... >
  // 17 overloads of the delegate
```

---
###Standard delegete - Func

Family of generic delegates which DOES return a value

```cs
  public delegate TResult Func<T1, T2, out TResult>(T1 param1, T2 param2);
  // ...
  Func<TResult> 
  Func<T1, TResult> 
  Func<T1, T2, TResult> 
  Func<T1, T2, T3, TResult> 
  Func< ... >
  // 17 overloads of the delegate
```

---
###Standard delegete - Predicate

Defines a method which returns bool and one generic parameter

Used as a function to affirm or deny input for specific operation

```cs
  public delegate bool Predicate<T>(T input);
```

Example:

```cs
static bool IsEvenNumber(int num) {
    return num % 2 == 0;
}
static void Main(string[] args) {
    Predicate<int> isEvenPredicate = IsEvenNumber;
    List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    List<int> evenNumbers = numbers.FindAll(isEvenPredicate);
    // output will be 2,4,6,8
}
```

