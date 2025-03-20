# Barber-Booking
Barber Booking
🚀 Project Overview
Barber Booking is a web application that allows users to book barbers' appointments.
It provides an intuitive interface for selecting barber service type, available time slots, managing reservations, and handling authentication.
The project is built using Angular on the frontend and ASP.NET Core on the backend.

📌 Features
User Authentication (JWT-based login and registration)

Booking System (Select date, time, and barber service)

Admin Panel (Manage reservations, view users, and services)

Calendar Integration (Disable unavailable dates/times)

Secure API (ASP.NET Core with Entity Framework for database management)

🛠️ Technologies Used
Frontend (Angular)
Angular (Framework)

Ng-bootstrap (UI components)

RxJS (State management)

Angular Forms (Reactive form handling)

Backend (ASP.NET Core)
ASP.NET Core Web API

Entity Framework Core (Database ORM)

JWT Authentication

SQL Server (Database options)

🔧 Setup & Installation
Backend (ASP.NET Core)
Clone the repository:

git clone https://github.com/Anaksimandar/Barber-Booking.git
cd Barber-Booking/backend

Run database migrations:
dotnet ef database update

Start the server:
dotnet run

Frontend (Angular)

Navigate to the frontend folder:
cd Barber-Booking/frontend

Install dependencies:
npm install

Start the development server:
npm serve

Plan is to add next automated scenario:
One hour before reservation user will get SMS notification reminding him that he has an appointment if he cannot make it he can edit the reservation by clicking the link which will redirect him to the editing reservation page.

Next scenario will happend after appointment:
User will get SMS with question how he liked service; if grade is less then 3 starts it wont be posted on google reviews, instead user will provide feedback what it can be better but if grade is 4 or 5 starts it will be posted on google reviews and 
pulled to site's Review section. 

This will increase productivity of employes because they dont have to manually do these procedures also it will make SEO better which will affect page rankings and make potential clients feel more confident in working with us.


