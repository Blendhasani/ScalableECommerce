# High-Level Architecture

**Week 2**: Provide a high-level architecture for the application (service components, communication patterns, databases, etc.)

## Tech Stack

- .NET Core 8  
- SQL Server Database  
- Docker  
- Ocelot (API Gateway)  
- RabbitMQ (Message Bus/Queue)  

## Microservices

The project is divided into these microservices, each with its own database:

- **ProductService**  
- **OrderService**  
- **UserService**  
- **InventoryService**  
- **NotificationService**  

## Containerization

Each microservice runs in its own Docker container, which ensures:

- No dependency conflicts  
- Independent development, deployment and scaling  

## API Gateway (Ocelot)

- Routes external HTTP requests to the appropriate service  
- Hides internal service structure  
- Supports routing, logging, rate limiting and authentication  

_Example:_ instead of `/users/api/users`, `/products/api/products`, or `/orders/api/orders`, clients call `/api/users`, `/api/products`, `/api/orders`, etc.

## RabbitMQ (Message Bus/Queue)

- Acts as a message broker for asynchronous communication between services  
- Allows services to publish and consume messages without direct coupling  

**Benefits:**  
- Loose coupling between services  

## CI/CD Pipeline (`build.yml`)

### Continuous Integration

- Builds and tests the code on every push  
- Notifies the team if the build or tests fail  

### Continuous Deployment

- Builds Docker images after successful tests  
- Deploys containers automatically upon success  

---

# Week 3: Database Design and API Implementation

- Created individual **SQL Server databases** for each microservice:
  - `dev-product`, `dev-order`, `dev-user`, `dev-inventory`, `dev-notification`
- Designed and created **initial schemas** for each service (e.g., `Products`, `Categories`, `Orders`, etc.)
- Used **EF Core scaffolding** to reverse-engineer models into each service’s `Infrastructure/Models` folder
- Configured and registered **DbContexts** in each `.API` project using `Program.cs` and `appsettings.json`
- Added **project references**:
  - `.API → .Application → .Infrastructure`
- Built full **CRUD operations** for `ProductService`:
  - `AddProductCommand` and `EditProductCommand` for input
  - `ProductDto` and `GetProductByIdDto` for output
  - `ProductServiceImpl` in Application layer for business logic
  - `ProductsController` in API for HTTP endpoints


# Week 4: Caching & Performance Optimization

**Goal**  
Improve response times and reduce database load for frequently accessed endpoints.

**Implementation Details**  
1. **Memory caching** in `GetByIdAsync` using `IMemoryCache`  
 - Key: `product:{id}`  
 - Duration: 10 minutes  
2. **Cache invalidation**  
 - On delete: remove `product:{id}`  
 - On update: remove `product:{id}`  
3. **Query optimization**  
 - Added index on `CategoryId` for faster filtering  

**Performance Impact**  
- `GetByIdAsync` responses now return in ~1–2 ms from cache  
- No EF Core query if data is cached  
- Significantly fewer database hits under load  

**Tools Used**  
- `IMemoryCache` (`Microsoft.Extensions.Caching.Memory`)  
- .NET scoped DI  
- SQL Server index on `CategoryId`  

---

# Event-Driven Architecture & Message Broker Choice

**Why did I choose RabbitMQ**  
- **Simplicity & flexibility**: supports fan-out, direct, topic exchanges  
- **Reliable delivery**: retains messages until acknowledged  
- **High throughput**: lightweight and fast for real-time events  
- **.NET support**: mature `RabbitMQ.Client` library  

**Design Overview**  
- **Publisher (ProductService)**:  
- Deletes product → publishes `Product deleted: {id}` to `product-deleted` queue  
- **Consumer (InventoryService)**:  
- Background worker listens on `product-deleted`  
- Logs and handles cleanup  

**Key Benefits**  
- Loose coupling via events  
- Resilience: RabbitMQ buffers messages if a consumer is down  
- Scalability: add new consumers without touching the publisher  
- Observability through the RabbitMQ UI and logs  

---

# Demonstrating Failure and Recovery

**Scenario:** InventoryService is down when an event is published

1. **InventoryService offline**  
 - No listener on `product-deleted`  
2. **Publish event**  
 - ProductService publishes `Product deleted: {id}`  
 - Message stays in queue (Ready = 1)  
3. **InventoryService restart**  
 - Reconnects to RabbitMQ  
 - Consumes pending message  
 - Logs:  
   ```
   [InventoryService] Received: Product deleted: {id} at 2025-05-18T23:12:50Z
   ```

**Outcomes**  
- Independent availability: services fail and recover safely  
- Durable messaging: no data loss  
- Automatic recovery: pending messages processed on restart  
- End-to-end resilience in distributed architecture  

![Alt text](high_level_architecture_diagram.jpeg "high level architecture diagram.jpeg")
