CREATE DATABASE Passenger

USE Passenger

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Salt NVARCHAR(200) NOT NULL,
    Username NVARCHAR(100) NOT NULL,
    FullName nvarchar(100),
    Role NVARCHAR(10) not null,
    CreatedAt DATETIME not null,
    UpdatedAt DATETIME not null
)

select * from Users