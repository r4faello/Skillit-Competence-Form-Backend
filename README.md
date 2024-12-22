# **Skillit Competence Assessment Form API**

## **Overview**
The **Skillit Competence Assessment Form API** is a RESTful application built with .NET 8.0. It was created as part of the *Inovacijų Dirbtuvės* challenge in partnership with Reiz Tech, with the goal of making competence assessments easier and more modern. This prototype already includes secure user authentication and a structured way to evaluate employee skills, laying the groundwork for a future production-ready tool.

---

## **Features**

### **User Features**
- **Register and Log In**: Users can securely sign up and log in with JWT-based authentication.  
- **Complete Skill Assessments**: Employees fill out skill-related forms, which produce a score reflecting their strengths.  
- **Save Drafts**: Assessments can be saved midway and completed later.  
- **Score Calculation**: Each answer (1–5 points) influences the overall skill score.

### **Admin Features**
- **Manage Submissions**: Administrators can review all completed assessments.  
- **Skill Insights**: Lets employers see and compare different skill levels across the team.

### **Highlights**
- **Real-Time Draft Updates**: Keep data in sync without losing progress.  
- **Seeded Data**: Pre-populated questions and answers for quick testing.

---

## **Technologies Used**
- **.NET 8.0**: Latest version for reliable performance  
- **SQL Server**: Database management through Entity Framework Core  
- **JWT Authentication**: Tokens for secure session handling  
- **Design Patterns**: Uses Mediator and Repository for clearer code organization  
- **Azure Deployment**: Hosted on Azure with GitHub Actions for continuous updates  
- **Swagger UI**: In-browser interface to test and explore the API

---

## **Architecture**

The code is organized into logical sections to keep things clear and maintainable:

- **Controllers**:  
  - `AdminController`: Handles admin actions such as viewing submissions.  
  - `QuestionsController`: Manages skill questions and related data.  
  - `UserController`: Oversees user registration, login, and profile updates.
- **Project Structure**:  
  ```
  ├── Commands/
  ├── Common/
  ├── Controllers/
  ├── DTOs/
  ├── Handlers/
  ├── Migrations/
  ├── Models/
  ├── Queries/
  ├── Repositories/
  ├── Services/
  ```
- **Dependency Injection**: Declared in `Program.cs` for a cleaner, more flexible design.

---

## **Database Schema**
- **Users**: Stores user information, using salted and hashed passwords  
- **SubmittedRecords**: Records finalized forms  
- **Drafts**: Temporary storage for partial submissions  
- **Questions & Answers**: Core data for skill assessments  
- **CompetenceSets, Competences, CompetenceValues**: Defines different skill sets and their scores  
- **Migrations**: Keeps database changes organized

---

## **Security**
- **JWT-based Authentication**: Protects endpoints and validates users  
- **Password Protection**: Passwords are always salted and hashed  
- **Clear Error Messages**: Returns easy-to-understand responses for invalid requests

---

## **Deployment**
- **Azure Hosting**: Scalable cloud deployment  
- **GitHub Actions**: Automated builds and deployments to ensure code stays current

---

## **Future Plans**
- Replace any hardcoded values (like admin accounts) with fully configurable setups  
- Introduce role-based permissions for more control over user privileges  
- Package the entire system in containers for simpler deployment

---

## **Testing**
- **Swagger UI**: Quickly test each endpoint in the browser  
- **Seeded Questions**: Pre-loaded data allows immediate hands-on usage

---

## **Contact**
For any questions or collaboration ideas, feel free to reach out:

**Author**: Rapolas Jonas Labutis  
**Email**: [rapolas.jonas.labutis@gmail.com]  
**GitHub**: [https://github.com/r4faello](https://github.com/r4faello)

---
