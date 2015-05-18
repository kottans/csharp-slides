- title : CSharp - OOP
- description : Object-oriented programming in C#
- author : Valentine Radchuk
- theme : league
- transition : default

***
## C# course
#### Lecture 5
# Object-oriented programming in C#

***
### OOP principles

- Inheritance
- Encapsulation
- Polymorphism   

<br/>
<br/>
   
<div class="fragment">
####More OOP pronciples

- Coupling/Cohesion
- Abstration
- S.O.L.I.D.
</div>

***
###Access modifiers in C#

- **private** - (default access modifier) - access is limited by containing type
- **public** - element is accessible from everywhere
- **protected** - access is limited by containing type and typed derived from it
- **internal** - access is limited by assembly where containing type is declared
- **protected internal** - superposition of protected and internal access levels

<div class="fragment">

```cs
namespace Test {
	public class Accesibility {
		private   		   int _private = 10; // this var will be available within this class only
		public    		   int _public = 20; // this var will be accessible anywhere
		protected 		   int _protected = 30; // this var will be available withi this class and for derived classes
		internal  		   int _internal = 40; // this var will be available anywhere within current assembly
		protected internal int _protected_internal = 50; // this var available within current assembly and for derived classes
	}
}
````
</div>

***
###Interfaces as custom types

- Interface is a reference type
- Interfaces could not be instantiated
- All members declared in an interface are public 
- Interface declares: **methods**, **properties**, **indexers** and **events**
- All members are abstract e.g. do not include any code

![Interface](images/Interface.png)

Once a class implements an interfaces it takes responsibility to provide functionality declared by interface.   

---
###Interface declaration

<div>

```cs
public interface ISomeInterface1 {
	void SomeMethod(); // declare a method
	int SomeProperty {get; set;}  // declare a property
	event EventHandler SomeEvent;  // declare an event
	int this[string index] {get; set;}  // declare an indexer
}
```
</div>	

---
###Interfaces implementation

Class might implement interface in 2 ways:

- <a href="https://dotnetfiddle.net/CzfpPL">Explicit implementation</a>    
Force some (or all) interface members belong to the interface directly.
- <a href="https://dotnetfiddle.net/XdmqIh">Implicit implementation</a>   
Share member implementation between an interface and a class which implements the interface

***
###Encapsulation in C#

Rules of thumb   

- Hides implementation details
- Class exposes *some* operation available for clients (its interface) other members should be hidden
- **ALL** data members should be hidden from client. 
- Acces to data members should be provided via properties

---
###Encapsulation on practice

- **Fields**   
Should be always declared as private and accessed via properties
- **Constructors**   
Should be declared as public (sometimes as protected)
- **Interface members (properties, methods, events)**   
Are always declared as public 
- **Other class members**   
Should be declared as private or protected


---
###Encapsulation benefits

- allows change implementations without affecting any code outside a class
- hiding implementation from client reduces code complexity

<div class="fragment">
Example of encapsulation:

```cs
class Counter {
	private int _id, _i;
	private static int s_n;

	public Counter() {
		_id = s_n++;
	}
	public void Increment() {
		Console.WriteLine(“{0} -> {1}”, _id, ++_i);
	}
	public void Decrement() {
		Console.WriteLine(“{0} -> {1}”, _id, —_i);
	}
	public int Count
	{
		get { return _i; }
	}
}
```
</div>


***
###Inheritance

![Inheritance](images/Inheritance.gif)

---
###Inheritance

There are 2 kind of inheritance in C#:   

- behaviour inheritance - inherit and extend class by its child
- interface inheritance - implement interfaces
 
---
###Behaviour inheritance

Inheritance allows child classes inherit characteristics and behaviour of parent class:   

- characteristics - data members (properties)
- behaviour - methods   

Inheritance allows child classes extend characteristics and behaviour of parent class:   

- add new fields and/or methods
- change behaviour of methods

