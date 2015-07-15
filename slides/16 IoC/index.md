- title : IoC in .NET
- description : Inversion of control approach in .NET
- author : Valentine Radchuk
- theme : league
- transition : default

***
## C# course
#### Lecture 16
# IoC in .NET

***
###What is Inversion of Control

**Inversion of Control** - is a common principle for writting low coupled code.
Imversion control could be implemented via:

- factory pattern
- service locator pattern
- depenency injection
- strategy pattern
- using events\delegates
- using interfaces

***
###IoC vs DI vs DIP

- IoC (Inversion of Control) - most general term indicating idea of invoking client code from a framework
- DI (Dependency Injection) - set of patterns to pass dependencies to a class
- DIP - (Dependency Inversion Principle) - tells that class should depend on abstractions from the same or higher level

<a href="http://sergeyteplyakov.blogspot.com/2014/11/di-vs-dip-vs-ioc.html">More info about the terms (ru)</a>

***
###Reasons to use IoC in your project

- reduce coupling
- remove direct dependencies between classes
- force use abstractions instead of implementations
- manage dependencies in external configuration
- minimize effort on injecting other implementation
- increaze testability of code

***
###How does IoC works
![](images\IoC-basics.jpg)

<a href="">Sample code without DI</a>  
<br />
<a href="">Sample code with DI</a>

---
###Constructor injection

Pass the object of the defined class into the constructor of the dependent class for its _**entire lifetime**_

```cs
class Watcher {
    INofificationAction action = null;

    public Watcher(INofificationAction concreteAction) {
        this.action = concreteAction;
    }

    public void Notify(string message) {   
        action.ActOnNotification(message);
    }
}
```

```cs
var writer = new EventLogWriter();
var watcher = new Watcher(writer);
watcher.Notify("Sample message to log");
```
---
###Method injection

To work in a method with different concrete class we have to pass the dependency in the method only

```cs
class Watcher
{
    public void Notify(INofificationAction concreteAction, string message)
    {
        action.ActOnNotification(message);
    }
}
```

```cs
EventLogWriter writer = new EventLogWriter();
var watcher = new Watcher();
watcher.Notify(writer, "Sample message to log");
```

---
###Property injection

If the responsibility of selection of concrete class and invocation of method are in separate places we need property injection

```cs
class Watcher {
    public INofificationAction Action { get; set ; }    

    public void Notify(string message) {   
        action.ActOnNotification(message);
    }
}
```

```cs
var writer = new EventLogWriter();
var watcher = new Watcher();
// This can be done in some class
watcher.Action = writer;
// This can be done in some other class
watcher.Notify("Sample message to log");
```

***
###IoC containers

IoC containers are used to:

- automatically inject dependencies
- manage lifecycle of dependencies
- manage dependencies relationship
- split creation dependencies and configuration relationships

---
### DI cons

- high learning curve
- constructors may look complicated
- code may seem "magic" for those who don't know DI
- overkill for small projects
- makes classes hard to use outside IoC container

---
###IoC containers in .NET world

- [Ninject](www.ninject.org)
- [Unity](https://unity.codeplex.com/)
- [Castle.Windsor](http://www.castleproject.org/)
- [Autofac](http://autofac.org/)
- [StructureMap](http://docs.structuremap.net/)

[Comparison table](http://www.palmmedia.de/blog/2011/8/30/ioc-container-benchmark-performance-comparison)

[Another comparison table](http://philipm.at/2011/di_speed.html)

[IoC Battle](http://www.iocbattle.com/)

***
###Autofac - overview

- open source project
- automates constructor, method and property injection
- ligh-weight and fast enought
- has lower learning curve comparing to other containers
- could be configured either via code or xml configuration
- available for all .NET technologies (WPF, ASP MVC, Web API, WinPhone 8, Win RT etc)
- supports modules and automated type loading from an assembly
- supports interceptors

[Autofac homepage](http://autofac.org/)

***
###Sample container - manual resolution

```cs
static void Main(string[] args)
{
  var consoleOutput = new ConsoleOutput();
  var writer = new TodayWriter(consoleOutput);
  
  writer.WriteDate();
}
```
---
###Sample container - resulution with Autofac

```cs
static void Main(string[] args)
{
  // register types for DI
  var builder = new ContainerBuilder();
  // dependency
  builder.RegisterType<ConsoleOutput>().As<IOutput>();  
  // class to inject dependency
  builder.RegisterType<TodayWriter>().As<IDateWriter>();
  var container = builder.Build();
  // resolve types and use instances with injected objects
  using (var scope = Container.BeginLifetimeScope())
  {
    var writer = scope.Resolve<IDateWriter>();
    writer.WriteDate();
  }
}
```

***
###Glossary

- **Container** - manager of application Components
- **Component** - class that declares a Service and dependencies it uses
- **Service** - is a contract (interface) between Dependencies
- **Dependency** - Service required by a Component
- **Registration** - adding Component to Container
- **Scope** - is context where Instance of a component will be shared by other Components

***
###Register components

Type

```cs
builder.RegisterType<SomeType>().AsSelf().As<ISomeService>();
```
Lambda expression

```cs
builder.Register(c => new SomeClass(c.Resolve<ISomeDependency>));
```
Specific instance

```cs
builder.RegisterInstance(new SomeClass());
```
Auto registration types from an assembly

```cs
builder.RegisterAssemblyTypes(typeof(SomeClass).Assembly);
```

***
###Lifetime options

Once registered, components can be configured with their lifetime

- InstancePerDependency
- InstancePerLifeTimeScope
- InstancePerMatchingLifeTimeScope
- SingleInstance