## Inheritance

![Inheritance](/Images/inheritance.png)

<hr>

## Single Table Inheritance (Table Per Hierarchy)

- Everything is represented in one table.
- We have a column for each one of the properties of the derived class.
- Discriminator is using the name of the derived class so when the data is loaded in as objects it is specified which derived class should be populated.
- Special properties belonging to a derived class are NULLs in other derived classes in the database because these properties are not present in the other derived classes.
- The advantage of this approach is that we don't have to have JOINs to query the data.
- The disadvantage of this approach is that when we have a lot of derived classes in one table we need to store all the NULLs.
- Another disadvantage is that if we add another derived class it is going to require the modification of the existing table.
- Generally this solution is the most efficient.

<hr>

## Class Table Inheritance (Table Per Type)

- Rather than having one table representing how many tables we got for every class in our database we have exactly one table for every base and derived class.
- We need to override the OnModelCreating method and need to specify the table mappings.
- The advantage is that it follows the open close principle meaning that we add new functionality by adding new classes and not modifying existing ones but this concept applied to the database so if we add another derived class it will going to be a new table in the database.
- The disadvantage of this approach is that we have to have JOINs to query the data.

<hr>

## Concrete Table Inheritance (Table Per Concrete Type)

- We only have one table for each concrete class that means that in the database we just have derived classes as tables.
- We need to specify the base class as an abstract class.
- This is not available in Entity Framework Core just in the older Entity Framework.
