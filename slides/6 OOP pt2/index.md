- title : CSharp - OOP part 2
- description : Object-oriented programming in C#
- author : Valentine Radchuk
- theme : league
- transition : default

***
## C# course
#### Lecture 5.1
# Object-oriented programming in C# part 2

***
### OOP principles

- Coupling/Cohesion
- Abstration
- S.O.L.I.D.

***
###Abstraction

>"Abstraction - is the process of separating ideas from specific instances of those ideas" (<a href="www.wikipedia.org">Wikipedia</a>)   

####Key ideas of abstraction:
- ignore irrelevant features, properties and methods
- emphasizing on really important features
- keep only those details about an object that are relevant to the current perspective

---
###Abstraction goals

- simplify complex representation and expose only important properties of an object
- build simplified model of an object
- hide unimportant properties of an object

---
###Abstraction in C#

Abstraction in C# might be achieved in several ways:

- with Interfaces
- with abstract classes
- with inheritance

***
###Coupling and Cohesion

Coupling and Cohesion are characteristic of code. How tightly code is written.

####**Coupling**    
is a measure of interdependence between software modules

####**Cohesion** 
is degree to which the elements of a module belong together


Coupling is usually contrasted with cohesion e.g. low coupling and high cohesion

***
###Cohesion

Cohesion describes how closely all the methods in a class support a central purpose of the class

Good module should have high (or strong) cohesion

####Cohesion is high (or strong) if:
- methods carry out a small number of related activities
- classes aimed for single purpose
- advantages of high cohesion:
- reduced module complexity
- increased system maintainability
- increased module reusability

<br />
<a href="https://dotnetfiddle.net/JJd2gz">High cohesion demo</a>  
<br />
<a href="https://dotnetfiddle.net/3dBoQZ">Low cohesion demo</a>

---
###Coupling

Coupling describes how tightly a classes or modules are related to other classes or modules

####Good class or method should have low (loose) coupling:

- modules should have a very little dependency on other modules
- a module must be easily re-used by other modules (avoid direct dependencies among modules)   

<a href="https://dotnetfiddle.net/koaQxj">Loose coupling demo</a>
<br />
<a href="https://dotnetfiddle.net/NPgw9h">Tight cohesion demo</a>

***
###S.O.L.I.D.

S.O.L.I.D. - is an abbreviation of 5 principles of object oriented design

- **S** - Single responsibility principle   
- **O** - Openness principle   
- **L** - Liskov substitution principle   
- **I** - Interface segregation principle   
- **D** - Dependency inversion principle   