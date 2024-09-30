## insight

The app manages users gives them task and allows them to create it, with custom policy and groups the system allows flexible authorizing system

## state

The project is served right, all services are injected and decoupled, the lifetime of the services was consideably choosen, the database design and init is optimal (auto migration is missing upon deployment)

Some security consideration might be adjusted and the JWT to be placed in an HttpOnly cookie

Not all possible exceptions are handled, some transactions could fail need to analyze again, database behaviour and return types are clear and the exceptions are handled
