Use [WildPay-1]
GO

-- USER
DROP PROCEDURE IF EXISTS sp_CreerUser;
GO
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


DROP PROCEDURE IF EXISTS sp_GetUserByEmail;
GO
CREATE PROCEDURE sp_GetUserByEmail
	@UserEmail VARCHAR(100)
   AS
   BEGIN
    SELECT * FROM [WildPay-1].[dbo].[User] WHERE Email = @UserEmail;
   END
GO


DROP PROCEDURE IF EXISTS sp_GetUserById;
GO
CREATE PROCEDURE sp_GetUserById
	@UserId INT
   AS
   BEGIN
    SELECT * FROM [WildPay-1].[dbo].[User] WHERE Id = @UserId;
   END
GO

DROP PROCEDURE IF EXISTS sp_UpdateUserImageById;
GO
CREATE PROCEDURE sp_UpdateUserImageById
	@UserId VARCHAR(100),
	@ImageFile varbinary(MAX)
   AS
   BEGIN
	   BEGIN TRANSACTION
		   UPDATE [WildPay-1].[dbo].[User] 
			SET [UserImage] = @ImageFile
			WHERE Id = @UserId;
	   COMMIT;
   END
GO

DROP PROCEDURE IF EXISTS sp_UpdateUser;
GO
CREATE PROCEDURE sp_UpdateUser
	@UserId VARCHAR(100),
	@firstname VARCHAR(50),
	@lastname VARCHAR(50),
	@password VARCHAR(80)
   AS
   BEGIN
	   BEGIN TRANSACTION
		   UPDATE [WildPay-1].[dbo].[User] 
			SET [Firstname] = @firstname,
			[Lastname] = @lastname,
			[Password] = @password
			WHERE Id = @UserId;
	   COMMIT;
   END
GO


-- CATEGORY
DROP PROCEDURE IF EXISTS sp_CreerCategory;
GO
CREATE PROCEDURE sp_CreerCategory
@name VARCHAR(100),
@group_Id INT
AS
BEGIN
	DECLARE @category_Id INT
	INSERT INTO [Category](Name)
	OUTPUT INSERTED.Id INTO @category_Id VALUES
	(@name)
	INSERT INTO [GroupCategory](Group_Id, Category_Id) VALUES
	(@group_Id, @category_Id)
END
GO

DROP PROCEDURE IF EXISTS sp_SuppressionCategory;
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
DROP PROCEDURE IF EXISTS sp_CreerGroup;
GO
CREATE PROCEDURE sp_CreerGroup
@name VARCHAR(200)
AS
BEGIN
	INSERT INTO [Group](Name) VALUES
	(@name)
END
GO

-- EXPENSE
DROP PROCEDURE IF EXISTS sp_CreerExpense;
GO
CREATE PROCEDURE sp_CreerExpense
@date DATETIME,
@title VARCHAR(200),
@value DECIMAL(10,2),
@user_Id INT,
@category_Id INT,
@group_Id INT
AS
BEGIN
	INSERT INTO [Expense](CreatedAt,Title,Value,FkUserId,FkCategoryId,FkGroupId) VALUES
	(@date, @title, @value, @user_Id, @category_Id, @group_Id)
END
GO

