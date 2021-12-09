Use [WildPay-1]
 GO 

-- USER
DROP PROCEDURE IF EXISTS sp_CreerUser;
 GO 
CREATE PROCEDURE sp_CreerUser
@firstname VARCHAR(50),
@lastname VARCHAR(50),
@email VARCHAR(80),
@password VARCHAR(80),
@GroupID int
AS
BEGIN
	DECLARE @NewUserId INT
	INSERT INTO [User](Firstname,Lastname,Email,Password) VALUES
	(@firstname, @lastname, @email, @password)
	SELECT @NewUserId = SCOPE_IDENTITY() 
	INSERT INTO [UserGroup](User_Id, Group_Id) VALUES
	(@NewUserId, @GroupID)
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

 DROP PROCEDURE IF EXISTS sp_GetUsersForGroup;
 GO 
CREATE PROCEDURE sp_GetUsersForGroup
	@groupId INT
   AS
   BEGIN
    SELECT * FROM [WildPay-1].[dbo].[User] 
	INNER JOIN [WildPay-1].[dbo].[UserGroup]
	ON [User].Id = [UserGroup].User_Id
	WHERE Group_Id = @groupId;
   END
 GO 



 DROP PROCEDURE IF EXISTS sp_GetUser;
 GO 
CREATE PROCEDURE sp_GetUser
@groupId INT
AS
BEGIN
SELECT Id, Firstname, Lastname FROM [User]
INNER JOIN [UserGroup] ON [User].Id = UserGroup.User_Id
WHERE Group_Id = @groupId
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
	@lastname VARCHAR(50)
   AS
   BEGIN
	   BEGIN TRANSACTION
		   UPDATE [WildPay-1].[dbo].[User] 
			SET [Firstname] = @firstname,
			[Lastname] = @lastname
			WHERE Id = @UserId;
	   COMMIT;
   END
 GO 


-- CATEGORY
DROP PROCEDURE IF EXISTS sp_CreerCategory;
 GO 
CREATE PROCEDURE sp_CreerCategory
@name VARCHAR(100),
@group_Id INT,
@IsBase BIT
AS
BEGIN
	DECLARE @category_Id INT
	INSERT INTO [Category](Name, IsBase) VALUES
	(@name, @IsBase)
	SELECT @Category_Id = SCOPE_IDENTITY() 
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
UPDATE [WildPay-1].[dbo].[Expense]
SET [FkCategoryId] = null
WHERE FkCategoryId = @CategoryId
DELETE FROM [Category]
WHERE Id = @CategoryId
END
 GO 

DROP PROCEDURE IF EXISTS sp_GetCategory;
 GO 
CREATE PROCEDURE sp_GetCategory
@groupId INT
AS
BEGIN
SELECT * FROM [Category]
INNER JOIN [GroupCategory] ON Category.Id = GroupCategory.Category_Id
WHERE Group_Id = @groupId
END
 GO 

DROP PROCEDURE IF EXISTS sp_GetCategoryByName;
 GO 
CREATE PROCEDURE sp_GetCategoryByName
@name VARCHAR(200),
@groupId INT
AS
BEGIN
SELECT * FROM [Category]
INNER JOIN [GroupCategory] ON Category.Id = GroupCategory.Category_Id
WHERE Group_Id = @groupId AND Category.Name = @name
END
 GO 



-- GROUP
DROP PROCEDURE IF EXISTS sp_CreerGroup;
 GO 
CREATE PROCEDURE sp_CreerGroup
@name VARCHAR(200),
@user_Id INT, 
@GroupID int OUTPUT
AS
BEGIN
	INSERT INTO [Group](Name, CreatedAt) VALUES
	(@name, GETDATE())
	SELECT @GroupID = SCOPE_IDENTITY() 
	INSERT INTO [UserGroup](Group_Id, User_Id) VALUES
	(@GroupID, @user_Id)
END
 GO 

DROP PROCEDURE IF EXISTS sp_GetGroupePrincipalId;
 GO 
CREATE PROCEDURE sp_GetGroupePrincipalId
	@GroupID int OUTPUT
AS
BEGIN
	SELECT TOP 1 @GroupID = Id FROM [Group]
	WHERE Name = 'principal';
END
 GO 


DROP PROCEDURE IF EXISTS sp_GetDefaultGroupForUser;
 GO 
CREATE PROCEDURE sp_GetDefaultGroupForUser
	@user_Id int, 
	@GroupID int OUTPUT
AS
BEGIN
	SELECT TOP 1 @GroupID = Id FROM [Group]
	INNER JOIN [UserGroup]
	ON [Group].Id = [UserGroup].Group_Id
	WHERE [UserGroup].User_Id = @user_Id
END
 GO 

DROP PROCEDURE IF EXISTS sp_GetGroupsForUser;
 GO 
CREATE PROCEDURE sp_GetGroupsForUser
	@user_Id INT
AS
BEGIN
	SELECT * FROM [Group]
	INNER JOIN [UserGroup] 
	ON [Group].Id = [UserGroup].Group_Id
	WHERE [UserGroup].User_Id = @user_Id;
END
 GO 


DROP PROCEDURE IF EXISTS sp_GetGroupById;
 GO 
CREATE PROCEDURE sp_GetGroupById
	@group_Id INT
AS
BEGIN
	SELECT * FROM [Group]
	WHERE [Group].Id = @group_Id;
END
 GO 

DROP PROCEDURE IF EXISTS sp_AddMemberToGroup;
 GO 
CREATE PROCEDURE sp_AddMemberToGroup
	@UserEmail VARCHAR(200), 
	@group_Id INT,
	@ajoutOk INT OUTPUT 
AS
BEGIN
	SELECT @ajoutOk = 0;
	DECLARE @User_Id INT;
    SELECT TOP 1 @User_Id = Id FROM [WildPay-1].[dbo].[User] WHERE Email = @UserEmail;
	IF (@User_Id <> 0)
		BEGIN
		IF NOT EXISTS (select 1 FROM UserGroup WHERE Group_Id = @group_Id AND User_Id = @User_Id)
			BEGIN
			INSERT INTO [UserGroup](User_Id, Group_Id)
			VALUES(@user_Id, @group_Id);
			SELECT @ajoutOk = 1;
			END
		END
	END
 GO 


DROP PROCEDURE IF EXISTS sp_DeleteGroup;
 GO 
CREATE PROCEDURE sp_DeleteGroup
	@group_Id INT
AS
BEGIN
 DELETE FROM [EXPENSE] 
 WHERE [EXPENSE].FkGroupId = @group_Id
 DELETE FROM [UserGroup] 
 WHERE [UserGroup].Group_Id = @group_Id 
 DELETE FROM [GroupCategory]
 WHERE Group_Id = @group_Id
--DELETE FROM Category
--WHERE category.Id in
--(SELECT * FROM Category
--INNER JOIN GroupCategory on ([GroupCategory].Category_Id = Category.Id)
--WHERE (@group_Id = GroupCategory.Group_Id))

END
 GO 



 DROP PROCEDURE IF EXISTS sp_UpdateGroup;
 GO 
CREATE PROCEDURE sp_UpdateGroup
	@group_Id INT,
	@name VARCHAR(200)
AS
BEGIN
	UPDATE [Group]
	SET
	Name = @name
	WHERE [Group].Id = @group_Id;
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


 DROP PROCEDURE IF EXISTS sp_UpdateExpense;
 GO 
CREATE PROCEDURE sp_UpdateExpense
@expense_Id INT,
@date DATETIME,
@title VARCHAR(200),
@value DECIMAL(10,2),
@user_Id INT,
@category_Id INT

AS
BEGIN
	UPDATE [Expense]
	SET
	CreatedAt = @date,
	Title = @title,
	FkUserId = @user_Id,
	FkCategoryId = @category_Id,
	[Value] = @value
	WHERE @expense_id = [Expense].Id
END
 GO 


 DROP PROCEDURE IF EXISTS sp_GetExpense;
 GO 
CREATE PROCEDURE sp_GetExpense
@groupId INT
AS
BEGIN
SELECT Expense.Id, CreatedAt, Title, Value, FkUserId, FkCategoryId, FkGroupId ,Category.Name, [User].Firstname, [User].Lastname, [User].UserImage FROM [Expense]
INNER JOIN [User] ON [User].Id = FkUserId
LEFT OUTER JOIN [Category] ON Category.Id = FkCategoryId
WHERE FkGroupId = @groupId
ORDER BY CreatedAt DESC
END
 GO 



 DROP PROCEDURE IF EXISTS sp_DeleteExpense;
 GO 
CREATE PROCEDURE sp_DeleteExpense
@expense_Id INT
AS
BEGIN
DELETE FROM [Expense]
WHERE [Expense].Id = @expense_Id
END
 GO 

 DROP PROCEDURE IF EXISTS sp_GetExpenseById;
 GO 
 CREATE PROCEDURE sp_GetExpenseById
 @expenseId INT
 AS
 BEGIN
 SELECT * from [Expense]
 WHERE [Expense].Id = @expenseId
 END
 GO 

 DROP PROCEDURE IF EXISTS sp_GetSommeDue;
 GO 
CREATE PROCEDURE sp_GetSommeDue
	@UserId INT,
	@groupId INT,
	@sommeDue MONEY OUTPUT
   AS
   BEGIN
    SELECT @sommeDue=(
	((SELECT SUM(value) FROM Expense WHERE FkGroupId = @groupId)
	/(SELECT COUNT(*) FROM [UserGroup] WHERE Group_Id = @groupId))
	-(SELECT SUM(value) FROM Expense WHERE FkGroupId = @groupId AND FkUserId = @UserId)
	)
   END
 GO 

  DROP PROCEDURE IF EXISTS sp_GetSommeDueSiNull;
 GO 
CREATE PROCEDURE sp_GetSommeDueSiNull
	@groupId INT,
	@sommeDue MONEY OUTPUT
   AS
   BEGIN
    SELECT @sommeDue=(
	((SELECT SUM(value) FROM Expense WHERE FkGroupId = @groupId)
	/(SELECT COUNT(*) FROM [UserGroup] WHERE Group_Id = @groupId))
	)
   END
 GO 
 
   DROP PROCEDURE IF EXISTS sp_SommeParUser;
 GO 
CREATE PROCEDURE sp_SommeParUser
	@groupId INT
   AS
   BEGIN
	SELECT (CONCAT (Firstname, ' ', Lastname)) AS PrenomNom, SUM(value) AS Somme FROM [Expense]
	INNER JOIN [User] ON [Expense].FkUserId = [User].Id
	WHERE FkGroupId = @groupId
	GROUP BY Firstname, Lastname
   END
 GO 