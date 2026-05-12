# Repository Layer Instructions

This document details best practices and patterns for implementing and maintaining the repository (data access) layer in WebAPIShop.

---

## General Principles

- **Repository Pattern:** Each entity (e.g., Product, Category, Order, User) has a dedicated repository interface and implementation.
- **DbContext Usage:** All repositories receive `dbSHOPContext` via constructor injection.
- **No Business Logic:** Repositories are strictly for data access; business logic belongs in the service layer.
- **Async Operations:** All repository methods must be asynchronous (`Task<T>`, `Task<List<T>>`, etc.).
- **No Direct SQL:** Use Entity Framework Core LINQ queries for all data access.

---

## Dependency Injection

- Register all repository interfaces and implementations in `Program.cs` using `AddScoped<TInterface, TImplementation>()`.
```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>(); builder.Services.AddScoped<IProductRepository, ProductRepository>(); // ...etc.
```

---

## Data Access Patterns

- **Querying:** Use `DbSet<TEntity>` and LINQ for queries. Use `Include` for eager loading related entities.
- **Adding:** Use `await context.AddAsync(entity)` and `await context.SaveChangesAsync()`.
- **Updating:** Use `context.Update(entity)` and `await context.SaveChangesAsync()`.
- **Deleting:** Use `context.Remove(entity)` and `await context.SaveChangesAsync()`.

---

## Example: UserRepository


```csharp
public class UserRepository : IUserRepository { private readonly dbSHOPContext _context;
public UserRepository(dbSHOPContext context)
{
    _context = context;
}
public async Task<User?> GetUserById(int id)
{
    return await _context.Users
        .Include(u => u.Orders)
        .FirstOrDefaultAsync(u => u.UserId == id);
}
public async Task<User> AddUser(User user)
{
    await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();
    return user;
}
}
```


---

## Testing

- Use **Moq** and **Moq.EntityFrameworkCore** to mock `DbSet<TEntity>` and `dbSHOPContext` in unit tests.
- All repository methods should be covered by unit tests in the `TestProject1/` project.

---

## Error Handling

- Do not catch exceptions in repositories unless you need to handle specific data access errors.
- Let exceptions bubble up to be handled by the global error handling middleware.

---

## Additional Notes

- **No direct use of connection strings** in repositories; always use DI and configuration.
- **No static methods** in repositories.
- **No direct calls to `SaveChanges()`**; always use `SaveChangesAsync()`.

---