# Controller Layer Instructions

This document outlines best practices for implementing API controllers in WebAPIShop.

---

## General Principles

- **RESTful Design:** All controllers must follow RESTful conventions (use HTTP verbs, resource-based URIs).
- **DTO Usage:** Controllers must use DTOs (C# records) for all input/output, never expose entity models directly.
- **Service Layer:** Controllers interact only with service interfaces, not repositories or DbContext directly.
- **Dependency Injection:** All services are injected via constructor.

---

## API Standards

- **Route Naming:** Use `[Route("api/[controller]")]` and `[HttpGet]`, `[HttpPost]`, etc.
- **Status Codes:** Return appropriate HTTP status codes (`200 OK`, `201 Created`, `400 BadRequest`, `404 NotFound`, etc.).
- **Validation:** Use `[FromBody]`, `[FromQuery]`, and model validation attributes. Return `BadRequest` for invalid input.
- **Error Handling:** Do not use try/catch in controllers; rely on global error handling middleware.
- **Authorization:** Use `[Authorize]` and policy attributes as needed.

---

## DTOs & AutoMapper

- **Input/Output:** Accept and return DTOs, not entity models.
- **Mapping:** Use AutoMapper to map between DTOs and entities in the service layer.
- **No direct mapping in controllers.**

---

## Example: ProductsController
```csharp
public ProductsController(IPrudectsService service)
{
    _service = service;
}
[HttpGet("{id}")]
public async Task<ActionResult<ProductDto>> GetProduct(int id)
{
    var product = await _service.GetProductByIdAsync(id);
    if (product == null)
        return NotFound();
    return Ok(product);
}
[HttpPost]
public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);
    var created = await _service.CreateProductAsync(dto);
    return CreatedAtAction(nameof(GetProduct), new { id = created.ProductId }, created);
}
```


---

## Swagger & Documentation

- Use XML comments on controller actions for Swagger documentation.
- All public endpoints must be documented.

---

## Additional Notes

- **No business logic in controllers.**
- **No direct access to repositories or DbContext.**
- **Return DTOs only.**
- **Use async/await for all actions.**

---