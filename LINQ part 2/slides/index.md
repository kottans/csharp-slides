- title : C# LINQ part 2
- description : C# LINQ part 2
- author : Alex Aza
- theme : league
- transition : default

***

## C# course
#### Lecture 10
# LINQ part 2

***

## Agenda
- anonymous types
- IEnumerable vs IQueryable
- EntityFramework 
- deferred execution
- sortings
- groupings 
- joins
- aggregation
 
***

### Anonymous types

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
                  ), cust);

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

### EntityFramework 

***

### Deferred execution

All standard query operators provide deferred execution, with the following exceptions:
- Opereators that return a single element or scalar value, such as `First` or `Count`
- Conversion operators like `ToArray`, `ToList`, `ToDictionary`, `ToLookup`

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
orderby s.FirstName ascending, s.LastName descending
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
join c in Courses on s.StudentID equals a.StudentID into t
select { StudentName = s.Name, Courses = t }

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
select { AddressID = (int?)st.AddressID, st.City, StudentName = s.Name }

```

---

#### Cross outer join

Cross join returns records or rows that are multiplication of record number from both the tables means each row on left table will related to each row of right table

---

```cs

from s in Students
from a in Addresses
select { a.City, s.Name }

```

***

### Aggregation