# Demo Commerce App built in .NET / microservice architecture 

## Development Environment

- OS: macOS 13.2.1
- Processor chip: Apple M1
- Docker version: 4.25.1
- Docker engine: 24.0.6
- Kubernetes version: v1.28.2
- Target framework: .NET 7.0 

---

## Services

### Product Catalog Service 

> The Product Catalog Service is a microservice designed to handle product-related functionalities. It is implemented in ASP.NET Core and follows the MVC architecture.

- `ProductCatalogController.Get`: exposes an endpoint that accepts a list of product IDs and returns the corresponding product details, which are managed by in-memory object for a minimum implementation.

### Shopping Cart Service 

> The Shopping Cart Service manages user shopping carts and interacts with the Product Catalog Service for product information retrieval.

- `ShoppingCartController`:
  - `Get` /shopping-cart/{userId:int}: Retrieves the shopping cart for a specific user ID.
  - `Post` /shopping-cart/{userId:int}/items: Adds items to the shopping cart based on the provided product IDs.
  - `Delete` /shopping-cart/{userId:int}/items: Removes items from the shopping cart based on the provided product IDs.

- `EventController`:
  - `Get` /event: Retrieves a range of events based on start and end indices.

---
## Build and Run (available only for Apple M1 architecture)

1. run `chmod +x deploy.sh` to grant execution permission
2. run `./deploy.sh` to build images / deploy containers on Kubernetes

> note: Encountered difficulties in applying images built in an M1 chip onto AKS. Thereby, deployed seamless image deployment within the Kubernetes cluster on local machine.

--- 
## Endpoint examples (httpie command)

> http GET http://localhost:5000/shopping-cart/1

> http POST http://localhost:5000/shopping-cart/100/items Accept:application/json Content-Type:application/json <<< '[1, 2]'

> http DELETE http://localhost:5000/shopping-cart/100/items Accept:application/json Content-Type:application/json <<< '[1]'