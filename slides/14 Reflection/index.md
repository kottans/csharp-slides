- title : Reflection
- description : Understanding Reflection and Attributes 
- author : Dmytriy Hnatiuk
- theme : league
- transition : default

***
## C# course
#### Lecture 14
# Reflection

***
###Reflection overview
####All types in the CLR are self-describing
- CLR provides a reader and writer for type definitions (`System.Reflection` & `System.Reflection.emit`)
- You can ‘read’ programs
- You can map between type systems
- You can interrogate objects and types inside a running program

***
### Definitions.
- Reflection is the process of runtime type discovery.
- Reflection is a generic term that describes the ability to inspect and manipulate program elements at runtime

---
### What is "Reflection"? 
- Reflection provides objects that encapsulate assemblies, modules, and types.  (MSDN)
- You can use reflection to dynamically create an instance of a type...  (MSDN)
- You can then invoke the type's methods or access its fields and properties.  (MSDN)
- Reflection.Emit namespace contains classes that allow a compiler or tool to emit metadata and Microsoft intermediate language (MSIL)  (MSDN)

***
### System.Reflection Namespace
| Type | Meaning in Life |
| ------------- |:-------------|
| `Assembly`      |This abstract class contains a number of static methods that allow you to load, investigate, and manipulate an assembly.|
| `AssemblyName`    |This class allows you to discover numerous details behind an assembly’s identity (version information, culture information, and so forth).
| `EventInfo`    |This abstract class holds information for a given event.
| `FieldInfo`    |This abstract class holds information for a given field.
| `MemberInfo`    |This is the abstract base class that defines common behaviors for the EventInfo, FieldInfo, MethodInfo, and PropertyInfo types.
| `MethodInfo`    |This abstract class contains information for a given method.
| `Module`    | This abstract class allows you to access a given module within a multifile assembly.
| `ParameterInfo`    | This class holds information for a given parameter.
| `PropertyInfo`    | This abstract class holds information for a given property.

***
###The `System.Type` class
The `System.Type` class defines a number of members that can be used to examine a type’s metadata, a great number of which return types from the `System.Reflection` namespace.
* `Type.GetMethods()` returns an array of `MethodInfo` types, `Type.GetFields()` returns an array of `FieldInfo` types.

--- 
###Members of `System.Type`
|Type|Meaning in Life|
|:---:|:---|
|`IsAbstract` |
|`IsArray`|These properties (among others) allow you to discover a number of basic traits about the Type you are referring to (e.g., if it is an abstract entity, an array, a nested class, and so forth).
|`IsClass`|
|`IsCOMObject`|
|`IsEnum`| 
|`IsGenericTypeDefinition`|
|...|

---
|Type|Meaning in Life|
|:---:|:---|
|`GetConstructors()`| 
|`GetEvents()`| 
|`GetFields()`| 
|`GetInterfaces()`| These methods (among others) allow you to obtain an array representing the items (interface, method, property, etc.) you are interested in. Each method returns a related array (e.g., GetFields() returnsaFieldInfoarray,GetMethods()returnsa MethodInfo array, etc.). Be aware that each of these methods has a singular form (e.g., GetMethod(), GetProperty(), etc.) that allows you to retrieve a specific item by name, rather than an array of all related items.
|`GetMembers()`| 
|`GetMethods()`| 
|`GetNestedTypes()`| 
|`GetProperties()`| 

---
|Type|Meaning in Life|
|:---:|:---|
|`FindMembers()`| This method returns a MemberInfo array based on search criteria.
|`GetType()`| This static method returns a Type instance given a string name.
|`InvokeMember()`| This method allows “late binding” for a given item.

---
###Demo
[Reflection. Custom Metadata Viewer](https://dotnetfiddle.net/cq6NmM)

***
###Dynamically Loading Assemblies
The act of loading external assemblies on demand is known as a dynamic load.
* `System.Reflection` defines a class `Assembly`. Which enables to dynamically load an assembly and discover properties about the assembly
* `Assembly` type enables to dynamically load private or shared assemblies, as well as load an assembly located at an arbitrary location.

---
```cs
 public class Assembly {
         public static Assembly Load(AssemblyName assemblyRef);
         public static Assembly Load(String assemblyString);
}
```

---
The CLR forbids any code in the assembly from executing while using  ReflectionOnlyLoadFrom or ReflectionOnlyLoad. (Suitable for types investigation).

```cs
public class Assembly {
   public static Assembly ReflectionOnlyLoadFrom(String assemblyFile);
   public static Assembly ReflectionOnlyLoad(String assemblyString);
}
```

***
###Reflection Performance
####... reflection is slow.

---
If you’re writing an application that will dynamically discover and construct type instances, you should take one of the following approaches:
* Have the types derive from a base type that is known at compile time. At run time, construct an instance of the derived type, place the reference in a variable that is of the base type (by way of a cast), and call virtual methods defined by the base type.
* Have the type implement an interface that is known at compile time. At run time, construct an instance of the type, place the reference in a variable that is of the interface type (by way of a cast), and call the methods defined by the interface.

***
###Late Binding
Late binding is a technique in which you are able to create an instance of a given type and invoke its members at runtime without having hard-coded compile-time knowledge of its existence.

---
###The `System.Activator` Class
The `System.Activator` class (defined in `mscorlib.dll`) is the key to the .NET late-binding process.
######[Simple Activator Demo](https://dotnetfiddle.net/G9cwuz)
######[Activator with generics Demo](https://dotnetfiddle.net/V8rnFW)