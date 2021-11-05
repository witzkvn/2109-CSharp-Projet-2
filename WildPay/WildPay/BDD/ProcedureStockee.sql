-- USER
CREATE PROCEDURE sp_CreerUser
@firstname VARCHAR(50),
@lastname VARCHAR(50),
@email VARCHAR(80),
@password VARCHAR(80)
AS
BEGIN
	INSERT INTO [User](Firstname,Lastname,Email,Password) VALUES
	(@firstname, @lastname, @email, @password)
END
GO

CREATE PROCEDURE sp_GetUserByEmail
	@UserEmail VARCHAR(100)
   AS
   BEGIN
    SELECT * FROM [WildPay-1].[dbo].[User] WHERE Email = @UserEmail;
   END
GO

CREATE PROCEDURE sp_GetUserById
	@UserId INT
   AS
   BEGIN
    SELECT * FROM [WildPay-1].[dbo].[User] WHERE Id = @UserId;
   END
GO


-- CATEGORY
CREATE PROCEDURE sp_CreerCategory
@name VARCHAR(100)
AS
BEGIN
	INSERT INTO [Category](Name) VALUES
	(@name)
END
GO

CREATE PROCEDURE sp_SuppressionCategory
@CategoryId INT
AS
BEGIN
DELETE FROM [Category]
WHERE Id = @CategoryId
END
GO

-- GROUP
CREATE PROCEDURE sp_CreerGroup
@name VARCHAR(200)
AS
BEGIN
	INSERT INTO [Group](Name) VALUES
	(@name)
END
GO

-- EXPENSE
CREATE PROCEDURE sp_CreerExpense
@date DATETIME,
@title VARCHAR(200),
@value DECIMAL(10,2)
AS
BEGIN
	INSERT INTO [Expense](Date,Title,Value) VALUES
	(@date, @title, @value)
END
GO

