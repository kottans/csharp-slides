- title : C# LINQ part 2
- description : C# LINQ part 2
- author : Alex Aza
- theme : league
- transition : default

***

## C# course
#### Lecture 11
# LINQ part 2

***

## Agenda
- IEnumerable vs IQueryable
- Entity Framework 
- deferred execution
- sortings
- groupings 
- joins
- aggregation

***

### IEnumerable vs IQueryable
IQueryable derives from IEnumerable changing it's behaviour drastically

<div class="fragment">
	- Represents data about code in order to translate it to query
</div>

---

For example following code:

```cs
	ctx.Students.Where(stud => stud.City == "Kiev")
                .Select(stud => new { stud.FirstName, stud.LastName });
```

may be translated to:
<div class="fragment">

```sql

 SELECT c.FirstName, c.LastName
 FROM [dbo].[Student] c
 WHERE c.City = `Kiev`

```

</div>

---

###Step by step guide

Step 1:

```cs
	ctx.Students.Where(stud => stud.City == "Kiev")
                .Select(stud => new { stud.FirstName, stud.LastName });
```

<div class="fragment">

translated by C# compiler to:

```cs
var qry = Queryable.Select(
              Queryable.Where(
                  ctx.Students,
                  stud => stud.City == "Kiev"),
              stud => new { stud.FirstName, stud.LastName });
```

</div>

---

###Step by step guide

Step 2:

```cs
var qry = Queryable.Select(
              Queryable.Where(
                  ctx.Students,
                  stud => stud.City == "Kiev"),
              stud => new { stud.FirstName, stud.LastName });
```

<div class="fragment">

Then expression trees `stud => stud.City == "Kiev"` would be translated to:

```cs

var stud = Expression.Parameter(typeof(Student), "stud");
var lambda = Expression.Lambda<Func<Student,bool>>(
                  Expression.Equal(
                      Expression.Property(stud, "City"),
                      Expression.Constant("Kiev")
                  ), stud);

```

<div>

---

Which graphically whould be represented as:

![Expression tree](http://yuml.me/6bdd4fd7)

---

### Purpose of expression trees

The only purpose to represent code as data
which in future may be translated in other language


***

### Entity Framework 

Object-Relational Mapping (ORM) which allows you:

- Work with data-store in object-oriented way
- Query any supported data source
- Potentially change data source
- Has own change tracking system which can be extended
- Business specifications can be reused throughout your project
- Currently prefered data access in wich Microsoft invests the most

---

### Entity Framework main concepts:
#### DbContext

It is a bridge between your domain or entity classes and the database.

![DbContext](http://yuml.me/0199201a)

---

#### DbSets

DbContext contains entity set (`DbSet<TEntity>`) for all the entities which is mapped to DB tables

---

####Change Tracking 

It keeps track of changes occurred in the entities after it has been querying from the database

---

####Caching

DbContext does first level caching by default. It stores the entities which has been retrieved during the life time of a context class.

---

####Querying

DbContext converts LINQ-to-Entities queries to SQL query and send it to the database.

---

####Persistance

It also performs the Insert, update and delete operations to the database, based on the entity states.

---

#### DbContext
Example:

```cs
  ctx.Students.Where(stud => stud.City == "Kiev")
              .Select(stud => new { stud.FirstName, stud.LastName });
```

<div data-markdown class="fragment">

  You already saw this query. But in this case `ctx` is instance of DbContext.
  
</div>

---

#### Ways to generate DbContext

- Code-First
- Db-First

---

### Entity Framework 7
- Supports new types of storages (SQLite, NoSQL)
- Thanks to CLR Core can be run on any platform (Linux, Mac)
- Lightweight and modular

---

### Entity Framework resources

[Entity Framework tutorial](http://www.entityframeworktutorial.net/)

***

### Deferred execution

Execute not when constructed, but when enumerated 
(when MoveNext is called)

<div class="fragment">
	Deferred execution is important because it decouples query construction from query execution.	
</div>

---

### Deferred execution

All standard query operators provide deferred execution, with the following exceptions:

  - Operators that return a single element or scalar value, such as:
    - `First` 
    - `Count`
  - The following conversion operators: 
    - `ToArray` 
    - `ToList` 
    - `ToDictionary` 
    - `ToLookup`

---

### Multiple enumeration

IEnumerable is cool when it is used for right job.
In some cases it can introduce performance overhead:

<div class="fragment">

Please do not write such "magnificent code":

```cs

public static IEnumerable<PersonWithDish> GiveDishesBadWay(IEnumerable<Person> people, 
														   IEnumerable<Dish> dishes)
{
  for(int i = 0; i < people.Count(); i++)
  {
    yield return new PersonWithDish()
    {
      PersonName = people.ElementAt(i).Name, 
      DishName = dishes.ElementAt(i).Name
    };
  }
}

```

</div>

---

### Multiple enumeration

Other way to avoid it could be just cache result of execution with `ToList` or `ToArray` and get needed element by indexer;

Or choose right parameter type for your method:

```cs

public static IEnumerable<PersonWithDish> GiveDishesBetterWay(IReadOnlyList<Person> people, 
															  IReadOnlyList<Dish> dishes)
{
    for (int i = 0; i < people.Count; i++)
    {
        yield return new PersonWithDish()
        {
            PersonName = people[i].Name,
            DishName = dishes[i].Name
        };
    }
}

```

---

### Multiple enumeration

In this case to avoid multiple enumeration just use built-in method `Zip` :

```cs
public static IEnumerable<PersonWithDish> GiveDishesBestWay(IEnumerable<Person> people, 
															IEnumerable<Dish> dishes)
{
    return people.Zip(dishes, (p, d) => new PersonWithDish
                                        {
                                            PersonName = p.Name, 
                                            DishName = d.Name
                                        }))
}
```

[a link to the code](https://dotnetfiddle.net/AFFUFI)

***

### Sortings

LINQ queries do not provide any particular order unless you specify some

---

You can sort in acsending order using `orderby` syntax:

```cs

from s in Students
orderby s.FirstName ascending
select s;

```

or

```cs

Students.OrderBy(s => s.FirstName);

```

---

In order to sort in descending order you need to write 'descending' keyword:

```cs

from s in Students
orderby s.FirstName descending
select s;

```

or 

```cs

Students.OrderByDescending(s => s.FirstName);

```

---

Note that there is possibility to write dependent ordering:

```cs

from s in Students
orderby s.FirstName ascending, s.LastName ascending
select s;

```

or 

```cs

Students.OrderBy(s => s.FirstName)
		.ThenBy(s => s.LastName)

```

***

### Groupings 

Used when you need to precess several items grouping them by some criterea:

```cs

from s in Students
group s by s.LastName

```

or

```cs

Students.GroupBy(s => s.LastName);

```

***

### Joins

- Inner join
- Group join
- Left join
- Cross outer join

---

#### Inner join

Inner join returns only those records or rows that match or exists in both the sources.

```cs

from s in Students
join c in Cities on s.CityID equals c.CityID
select new { StudentName = s.FirstName, CityName = c.Name }

```

---

#### Inner join

We can match items by complex criterea using anonymous types:

```cs

from s in Students
join a in Addresses on new { s.CityID, s.RegionID } equals new { a.CityID, a.RegionID }
select new { StudentName = s.FirstName, CityName = a.CityName }

```

<div class="fragment">
	Note that you need to have the same names for anonymous type properties
</div>

---

#### Group join

A group join produces a sequence of object arrays based on properties equivalence of left collection and right collection. If right collection has no matching elements with left collection then an empty array will be produced.

---

#### Group join

```cs

from s in Students
join c in Courses on s.StudentID equals s.StudentID into t
select new { StudentName = s.Name, Courses = t }

```

---

#### Left join

LEFT JOIN returns all records or rows from left table and from right table returns only matched records. 
If there are no columns matching in the right table, it returns NULL values.

---

#### Left join

```cs

from s in Students
join a in Addresses on s.AddressID equals a.AddressID into t
from st in t.DefaultIfEmpty()
select new { AddressID = (int?)st.AddressID, st.City, StudentName = s.Name }

```

---

#### Cross outer join

Cross join returns records or rows that are multiplication of record number from both the tables means each row on left table will related to each row of right table

---

```cs

from s in Students
from a in Addresses
select new { a.City, s.Name }

```

***

### Aggregation

Aggregators are extensions that return some single result which is not collection, in other words they are mostly statistical operators

- Count
- LongCount
- Sum
- Min
- Max
- Average
- Aggregate

---

#### Count and LongCount

Returns a count of elements which satisfy passed condition (can be ommited):

```cs

int[] nums = {500,100,50,20,10,5,2,1,1000};
var count = nums.Count(); // 9

```

---

#### Sum

```cs

int[] nums = {300,100,50,20,10,5,2,1,1000};
var count = nums.Sum(); // 1488

```

<div class="fragment">

Can be more complex:

```cs

var totalAge = people.Sum(p => p.Age);

```

</div>

---
#### Min and Max

```cs

int[] nums = {500,100,50,20,10,5,2,1,1000};
var min = nums.Min(); // 1
var max = nums.Max(); // 1000

```

---

#### Average

```cs

int[] nums = {500,100,50,20,10,5,2,1,1000};
var min = nums.Average(); // 187.555555555556

```

---

#### Aggregate

Performs custom aggregation:

```cs

int[] nums = {300,100,50,20,10,5,2,1,1000};
var sum = nums.Aggregate(0, (total, n) => total + n); // 1488

```

***

### Other popular operators

- Any
- All
- Distinct
- OfType
- Cast
- SelectMany

---

#### Any

```cs

var values = new[] { "1", "2", "3", "AAA", "ABB" };
bool hasAAA = values.Any(i => i.StartsWith(“A”)); // true

```

---

#### All

```cs

var values = new[] { "1", "2", "3", "AAA", "ABB" };
bool hasAAA = values.All(i => i.StartsWith("A")); // false

```

---

#### Distinct

```cs

var values = new[] { "1", "1", "2", "2", "3", "3", "3" };
IEnumerable<string> strings = values.Distinct(); // "1", "2", "3"

```

---

#### OfType

```cs

object[] values = new object[] {"1", "2", "3", "AAA", 5};
IEnumerable<string> strings = values.OfType<string>(); 
// Last element witll be omited since it is not string

```

---

#### Cast

```cs

object[] values = new object[] {"1", "2", "3", "AAA", 5};
IEnumerable<string> strings = values.Cast<string>(); 
// an exception will be thrown 
// because the last element is not string

```

---

#### SelectMany

```cs

var values = new[]
{
    new[] { "1", "2" }, 
    new[] { "3", "4" }, 
    new[] { "5" }
};

var flatten = values.SelectMany(s => s); 
// one array ["1", "2", "3", "4", "5"]

```

---

Additional resources:

- https://weblogs.asp.net/dixin/linq-via-csharp
- https://codeblog.jonskeet.uk/category/edulinq
- http://linqsamples.com