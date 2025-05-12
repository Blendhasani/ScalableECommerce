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
