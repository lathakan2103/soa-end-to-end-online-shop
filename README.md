# SOA App

This is an complete soa app with some highlights:
  - IoC container usage on the server and on the client (MEF)
  - using of data contracts equality between client and server
  - using of dto contracts for custom entities not saved in the db 
  - using of design factory pattern for creating service managers, repositories, business managers
  - using of ef code first
      - using of base classes for the entities on the server and on the client
      - entities have base classes that care about security (owned entity), error handling (on server and client) and state changes
  - using of FluentValidation on the server
  - WCF 
      - usage with securiry adapter and SOAPHeader
      - WCF exception handling with a custom pattern (base class)
      - using of service proxies (and taking care of proper disposing)
      - hosting the services (console)
  - web app as customer access to the online shop (MVC with Angular SPAs container)
      - added some highlights like rules for client side validation (knockout-like), wrapping of the http services
      - separation of the client, controller, models and contracts
  - desktop app
  -   - admin application that handles products, cart status (approved, shipped)
  - unit tests!
  

IMPORTANT:

  Desktop app as an admin app is stil in progress...
  
  
  -------------------------------------------------------------------------------------------------------------------------
  
  
Special thanks to M. Castros, to the people of knockout, G. Block, S. Smith, B. Balmer - your ideas and the way of thinking are :))))))
