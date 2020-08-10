# Payment Gateway

This project exposes a payment gateway, an API based application that allows a merchant to offer a way for their shoppers to pay for their product. It also implements JWT token based authentication with refresh token. 
It uses InMemory Database for keeping things simple
ASP.NET Core 3.1 
Entity Frameword Core 3.1


## WebAPI
	Contains controller classes and API setup 

## Domain
	Contains Entities, Repositories, Services and Helper classes.

## Tests
	Contains test cases for services, Repositories, helpers and controllers.
	
## APIClient 
	Contains third part implementations such as Aquiring Bank API.


## Things to Improve
  Idempotency: at the moment payment post method is not idempotent. I would add a header key (e.g. Idempotency-Key) requirment for posting payments which would require a unique code
  Viewmodel/data contracts annotation
  A few more test cases to increase code coverage
  At least a powerShell script to build project and run test cases.
  

## Notes
  I would setup encryption keys and token secert to  be stored in AWS Secrets Manager on Production
  Aquiring Bank implementation can be switched using IOC.
  Asymmetric RSA encryption rather than symmetric AES encryption
  PKDF2 hashing with HMACSHA256 to hash passwords with 10,000 iterations
  
 
