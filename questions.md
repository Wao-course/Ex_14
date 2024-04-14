# Questions

- How do we partition data between microservices (ownership and locality)?

  **Data should be partitioned based on ownership and locality, ensuring that each microservice has control over the data it needs. This involves identifying boundaries for each service and determining which data each service owns or has access to locally.**

- How do we select database technologies for our system?

  **Database selection depends on factors such as data structure, scalability requirements, consistency needs, and integration capabilities with the chosen technology stack. Considerations include SQL vs. NoSQL databases, relational vs. non-relational models, and specific features like ACID compliance or eventual consistency.**

- How can caching help us with optimizing our system?

  **Caching can enhance system performance by storing frequently accessed data closer to the application, reducing the need to retrieve data from the database repeatedly. This improves response times and reduces latency, especially for read-heavy workloads.**

- What is failure propagation (can you come up with a concrete example in code)?

  **Failure propagation occurs when an error in one microservice cascades through the system, affecting other interconnected services. For example, if a critical service fails to respond, downstream services relying on it may also experience failures, leading to a domino effect of errors.**


<details>
  <summary><h1>Failure Propagation Example</h1></summary>
  
  ```csharp
  public void PerformTask()
  {
      try
      {
          // Simulate an error
          throw new Exception("Error in Service A");
      }
      catch (Exception ex)
      {
          Console.WriteLine($"Error in Service A: {ex.Message}");
          // Log the error
          LogError(ex);
          // Propagate the error to dependent services
          ServiceB serviceB = new ServiceB();
          serviceB.HandleError(ex);
      }
  }
```
</details>

- How does the retry pattern work?

  **The retry pattern involves automatically reattempting a failed operation after a specified delay or number of attempts. This can help mitigate transient failures and improve system reliability by allowing services to recover from temporary issues without manual intervention.**

- How does circuit breaker pattern work?

  **The circuit breaker pattern is a fault-tolerance mechanism that monitors service calls and prevents cascading failures by "opening" when a service is unavailable or failing. Once the circuit breaker is open, subsequent calls are rejected or redirected to a fallback mechanism, reducing load on the failing service and allowing it to recover.**

- How can we track down errors across a microservice system?

  **Tracking errors across a microservice system involves implementing centralized logging, distributed tracing, and monitoring tools to collect and analyze logs, metrics, and traces from each service. This allows developers to identify the root cause of errors, troubleshoot issues, and optimize system performance.**

- What are the benefits with structred logging?
  **Structured logging improves log readability and searchability by organizing log messages into a predefined format, such as key-value pairs or JSON objects. This makes it easier to filter, query, and analyze logs, enabling developers to quickly identify issues, troubleshoot errors, and monitor system performance.**
