CREATE DATABASE Passenger

USE Passenger

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Salt NVARCHAR(200) NOT NULL,
    Username NVARCHAR(100) NOT NULL,
    FullName nvarchar(100),
    Role NVARCHAR(100) not null,
    CreatedAt DATETIME2 not null,
    UpdatedAt DATETIME2 not null
)

drop TABLE Users

select * from Users

delete from Users
