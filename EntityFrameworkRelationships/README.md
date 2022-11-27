# Modeling Most SQL Relationships In Entity Framework Core

Object Relational Mappers (ORMs) allow us to define relationships between entities in our relational database using C# objects. Depending on which ORM we use, we can also get the library to generate the relationships’ schema. Entity Framework Core is one of those tools with a built-in SQL migration paradigm.

This post will explore most of the relationships found within a relational database and how we would model those relationships using a code-first approach. We’ll use SQL Server, but many of these modeling techniques will work across most relational databases and likely yield a similar schema.

> Original post used SQLite.

We’ll keep our entity definitions to a minimum so we can see what’s necessary when modeling our relationship type. These definitions are starting points, so developers should feel free to experiment and make them their own.

<hr>

## dotnet-ef

The command-line interface (CLI) tools for Entity Framework Core perform design-time development tasks. For example, they create migrations, apply migrations, and generate code for a model based on an existing database. dotnet-ef can be installed as either a global or local tool. This command installs it globally:

```
dotnet tool install --g dotnet-ef
```

To create a migration use this command:

```
dotnet ef migrations add Relationships
```

**In this example project applying migrations to the database is not necessary. We create them just to browse the generated code.**

<hr>

## Non-Related Entities

The first type of relationship is the stand-alone kind. In this scenario, the defining entity has no connection with any other entity in our database context. These are straightforward to design and typically require a single identifier column.

```cs
public class NotRelated
{
    public int Id { get; set; }
}
```

Within our `DbContext`, we need to define the `DbSet` property.

```cs
public DbSet<NotRelated> NotRelateds { get; set; }
```

<hr>

## One-to-One Bidirectional Relationship

When defining one-to-one bidirectional relationships, we still need to specify which entity in the connection is first. The specification allows EF Core to insert one row and then execute additional queries to tie the two entities together. Let’s define our one-to-one models first.

```cs
public class OneToOneLeft
{
    public int Id { get; set; }
    public int OneToOneRightId { get; set; }
    public OneToOneRight Right { get; set; }
}

public class OneToOneRight
{
    public int Id { get; set; }
    public int OneToOneLeftId { get; set; }
    public OneToOneLeft Left { get; set; }
}
```

We’ll notice that both entities have a reference to their one-to-one related entity. In this case, we see `OneToOneLeft` has a reference to `OneToOneRight` and vice versa. To define the order, we’ll need to add some additional code into our `OnModelCreating` method within the `DbContext`.

```cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // 1 to 1 (bidirectional)
    modelBuilder.Entity<OneToOneLeft>()
        .HasOne<OneToOneRight>()
        .WithOne(r => r.Left)
        .HasForeignKey<OneToOneRight>(r => r.Id);

    base.OnModelCreating(modelBuilder);
}
```

Finally, we can add both `DbSet` entities to our `DbContext`.

```cs
// 1 to 1 (bidirectional)
public DbSet<OneToOneLeft> OneToOneLefts { get; set; }
public DbSet<OneToOneRight> OneToOneRights { get; set; }
```

<hr>

## One-to-one Owned Relationship

EF Core has a concept of owned entities. An owned entity is one that we can only access through its parent relationship. For example, take the relationship between `OneToOneOwner` and `OneToOneOwned`.

```cs
public class OneToOneOwner
{
    public int Id { get; set; }
    public OneToOneOwned Owned { get; set; }
}

[Owned]
public class OneToOneOwned
{
    [Required]
    public string Value { get; set; }
}
```

As we may have noticed, the `OneToOneOwned` doesn’t have an identifier column. By default, EF Core will store one-to-one owned values in the same table as the owning entity. The optimization enhances query performance while still maintaining C# modeling intent. Let’s take a look at the SQL schema generated for our `OneToOneOwners` table.

```sql
create table OneToOneOwners
(
   Id INTEGER not null
      constraint PK_OneToOneOwners
         primary key autoincrement,
   Owned_Value TEXT
)
```

EF Core has table splitting capabilities if we still want to store owned-entity data in a separate table, but we’ll not delve into that for this post. To add a one-to-one owned relationship to the `DbContext`, we need to add a `DbSet` of the parent entity, which is `OneToOneOwner`.

```cs
// 1 to 1 (owned)
public DbSet<OneToOneOwner> OneToOneOwners { get; set; }
```

<hr>

## One-to-Many Relationships

The one-to-many relationship is one of the more versatile relationships when dealing with database modeling. With EF Core, we can use most collection types found in C#, but I would generally recommend `List<T>` as it has one of the more useful interface implementations with methods like `Add` and `AddRange`.

Let’s start by defining our parent entity, `OneToMany`, and its children entities `OneToManyItem` types.

```cs
public class OneToMany
{
    public int Id { get; set; }
    public List<OneToManyItem> Items { get; set; }
}

public class OneToManyItem
{
    public int Id { get; set; }
    public int OneToManyId { get; set; }
    public OneToMany OneToMany { get; set; }
}
```

As you may notice, the `Items` property in `OneToMany` is a `List<OneToManyItem>`. We can also see that `OneToManyItem` has a navigation property back to the parent entity. The navigation property to the parent isn’t always necessary, but it can help when writing LINQ queries. Finally, we can add both entities to our `DbContext` definition. It’s not always necessary to add the children entity as a `DbSet` property, but again, it can help us when constructing LINQ queries.

```cs
// 1 to Many
public DbSet<OneToMany> OneToManys { get; set; }
public DbSet<OneToManyItem> OneToManyItems { get; set; }
```

<hr>

## One-to-Many Owned Relationships

Unlike the one-to-one owned relationships, one-to-many owned relationships will generate separate tables. In our case, we have `OneToManyOwner` and `OneToManyOwnedItem` entities.

```cs
public class OneToManyOwner
{
    public int Id { get; set; }
    public List<OneToManyOwnedItem> Items { get; set; }
}

[Owned]
public class OneToManyOwnedItem
{
    public string Name { get; set; }
}
```

Even though we didn’t define an `Id` property on our `OneToManyOwnedItem` entity, EF Core will create what is known as a shadow property for the primary key `Id`. We can see this in the generated SQL schema.

```sql
create table OneToManyOwnedItem
(
   OneToManyOwnerId INTEGER not null
      constraint FK_OneToManyOwnedItem_OneToManyOwners_OneToManyOwnerId
         references OneToManyOwners
            on delete cascade,
   Id INTEGER not null,
   Name TEXT,
   constraint PK_OneToManyOwnedItem
      primary key (OneToManyOwnerId, Id)
);
```

To add the `OneToManyOwner` entity to our `DbContext`, we only need to add one `DbSet` property.

```cs
// 1 to Many (owned)
public DbSet<OneToManyOwner> OneToManyOwners { get; set; }
```

<hr>

## Many To Many Transparent Relationship

Developers coming from Entity Framework 6 Code-First will be familiar with transparent many-to-many relationships. EF Core manages the relationship table that connects two entities, abstracting it away from the developer. The transparent many-to-many relationship is only available since EF Core 5. Let’s look at two entities of `ManyToManyLeft` and `ManyToManyRight`.

```cs
public class ManyToManyLeft
{
    public int Id { get; set; }
    public List<ManyToManyRight> Rights { get; set; }
}

public class ManyToManyRight
{
    public int Id { get; set; }
    public List<ManyToManyLeft> Lefts { get; set; }
}
```

As we can see, both entities have a collection of the other entity. In this case, EF Core manages a transparent table of `ManyToManyLeftManyToManyRight` in our database schema.

```sql
create table ManyToManyLeftManyToManyRight
(
   LeftsId INTEGER not null
      constraint FK_ManyToManyLeftManyToManyRight_ManyToManyLefts_LeftsId
         references ManyToManyLefts
            on delete cascade,
   RightsId INTEGER not null
      constraint FK_ManyToManyLeftManyToManyRight_ManyToManyRights_RightsId
         references ManyToManyRights
            on delete cascade,
   constraint PK_ManyToManyLeftManyToManyRight
      primary key (LeftsId, RightsId)
);

create index IX_ManyToManyLeftManyToManyRight_RightsId
   on ManyToManyLeftManyToManyRight (RightsId);
```

A transparent many-to-many relationship is ideal for scenarios where the connection between two entities is matter-of-fact, meaning the relationship itself has no distinguishing attributes. As we’ll see in the next section, we can also model a many-to-many with the relationship realized as an entity.

To finish the modeling, we only need to add both entities in the connection to our `DbContext`.

```cs
// Many To Many (Transparent)
public DbSet<ManyToManyLeft> ManyToManyLefts { get; set; }
public DbSet<ManyToManyRight> ManyToManyRights { get; set; }
```

<hr>

## Modeled Many-To-Many Relationship

Modeling a many-to-many relationship with an exposed connecting entity is useful for scenarios where the relationship itself has defining attributes. An example might be between an individual and a home, where the individual could either own, lease to own, or rent.

Let’s look at an example with the following entities of `ManyToManyWithModeledLeft`, `ManyToManyWithModeledRight`, and `ManyToManyRelationship`.

```cs
public class ManyToManyWithModeledLeft
{
    public int Id { get; set; }
    public ManyToManyRelationship Relationship { get; set; }
}

public class ManyToManyWithModeledRight
{
    public int Id { get; set; }
    public ManyToManyRelationship Relationship { get; set; }
}

public class ManyToManyRelationship
{
    public int Id { get; set; }
    public List<ManyToManyWithModeledLeft> Lefts { get; set; }
    public List<ManyToManyWithModeledRight> Rights { get; set; }
}
```

As we may have noticed, the modeled many-to-many is a combination of one-to-one relationships and one-to-many relationships. The types `ManyToManyWithModeledLeft` and `ManyToManyWithModeledRight` each have a one-to-one relationship to the `ManyToManyRelationship` entity. The `ManyToManyRelationship` has a navigation collection to both `ManyToManyWithModeledLeft` and `ManyToManyWithModeledRight`.

Again, this approach’s advantage is how we can attach additional data to the relationship, where it makes sense, rather than incorrectly adding that data to either entity included in the many-to-many scenario.

We’ll need to add all these relationships to our `DbContext`.

```cs
// Many To Many (Modeled Relationship)
public DbSet<ManyToManyWithModeledLeft> ManyToManyWithModeledLefts { get; set; }
public DbSet<ManyToManyWithModeledRight> ManyToManyWithModeledRights { get; set; }
public DbSet<ManyToManyRelationship> ManyToManyRelationships { get; set; }
```

<hr>

## Hierarchical Relationships

Hierarchical relationships are when rows within the same table reference another row, typically in a parent/child relationship. Let’s look at how to model this type of relationship, starting with some C#.

```cs
public class Hierarchical
{
    public int Id { get; set; }
    public Hierarchical Parent { get; set; }
    public List<Hierarchical> Children { get; set; }
}
```

We can see that we have a navigation property of `Parent` and a collection navigation property of `Children`. While this relationship is straightforward to model, it is relatively challenging to query using LINQ. Use this modeling pattern with extreme caution, as hierarchical queries can be expensive to execute even when using raw SQL constructs.

We can see the SQL schema when generating the EF Core migrations.

```sql
create table Hierarchicals
(
   Id INTEGER not null
      constraint PK_Hierarchicals
         primary key autoincrement,
   ParentId INTEGER not null
      constraint FK_Hierarchicals_Hierarchicals_ParentId
         references Hierarchicals
            on delete cascade
);

create index IX_Hierarchicals_ParentId
   on Hierarchicals (ParentId);
```

To add this relationship to our `DbContext`, we only need to add our `Hierarchial` entity as a single `DbSet` property.

```cs
// Hierarchical
public DbSet<Hierarchical> Hierarchicals { get; set; }
```

<hr>

## Conclusion

We explored non-related, one-to-many, many-to-many, and hierarchical relationships with EF Core. While there are many permutations of each relationship type, this post will help beginners have a starting point for defining relationships and serve as a refresher for even the most experienced EF Core developers.

<hr>
<hr>
<hr>
<hr>
<hr>
<hr>
<hr>
<hr>
<hr>
<hr>

diagram kepek az osszes projektbe
hogyan kell a create table scriptet csinalni

| id  |  ownership   |
| :-: | :----------: |
|  1  |     own      |
|  2  | lease to own |
|  3  |     rent     |

Individuals table

| id  | first name | last name | relationship id |
| :-: | :--------: | :-------: | :-------------: |
|  1  |   Norman   |   Hill    |                 |
|  2  |  Charles   |  Barrett  |                 |
|  3  |   Malik    |   Faure   |                 |
|  4  |    Nina    |  Morris   |                 |

Homes table

| id  |    country    |     street      | number | relationship id |
| :-: | :-----------: | :-------------: | :----: | :-------------: |
|  1  | United States | Pecan Acres Ln  |  2459  |                 |
|  2  |  Switzerland  |  Rue Chazière   |  9541  |                 |
|  3  |  New Zealand  | Hugh Watt Drive |  9033  |                 |
|  4  | United States |  Brown Terrace  |  3833  |                 |
