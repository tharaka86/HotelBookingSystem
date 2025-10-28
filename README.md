# Hotel booking API

## Summary
This project follows Clean Architecture principles and adheres to SOLID design guidelines. The goal is to keep the codebase decoupled, extensible, and easily testable.

For this example, I’ve used an in-memory database (UseInMemoryDatabase), but it can be easily replaced with a real database by updating the configuration in the Infrastructure Dependency Injection setup.

Additionally, while I’ve included try-catch blocks in the controllers for simplicity, a global exception handling middleware with custom exceptions can be introduced for a more maintainable error-handling approach.

A few unit tests have been added for the Application layer to demonstrate how testing is structured in this project.
Similar tests can be written for other layers. Integration tests can be introduce to validate the actual API endpoints.

Git hub action is in place to build and run tests

## Usage

1. First, call the api/DataAdmin/seed endpoint to populate the database with some sample data.

2. Next, use the api/Hotels endpoint to retrieve the list of available hotels.

3. Once you have a hotel ID, you can check for available rooms using the api/Hotels/rooms/available endpoint.

4. Finally, based on the available rooms, you can create a booking by sending a POST request to api/Bookings.

