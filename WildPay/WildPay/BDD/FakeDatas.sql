CREATE PROCEDURE sp_AddFakeDatas
AS
BEGIN
	DECLARE @myimg varbinary(MAX); 
	SET @myimg = (SELECT * FROM Openrowset( Bulk 'C:\Users\Witz Kévin\Desktop\user.png' , Single_Blob) as img);


	INSERT INTO [User](Email, Firstname, Lastname, Password, Image) VALUES
		('admin@test.com', 'admin', 'ADMIN', 'admin1*', @myimg),
		('test1@test.com', 'first1', 'LAST1', 'test1*', @myimg),
		('test2@test.com', 'first2', 'LAST2', 'test2*', null),
		('test3@test.com', 'first3', 'LAST3', 'test3*', null),
		('test4@test.com', 'first4', 'LAST4', 'test4*', @myimg),
		('test5@test.com', 'first5', 'LAST5', 'test5*', null)

	INSERT INTO [Category](Name) VALUES
		('Cat1'),
		('Cat2'),
		('Cat3')

	INSERT INTO [Group](Name, CreatedAt) VALUES
		('G1', GETDATE()),
		('G2', GETDATE())

	INSERT INTO [UserGroup](User_Id,Group_Id) VALUES
		((SELECT Id FROM [User] WHERE Email = 'test1@test.com'),(SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [User] WHERE Email = 'test1@test.com'), (SELECT Id FROM [Group] WHERE Name = 'G2')),
		((SELECT Id FROM [User] WHERE Email = 'test2@test.com'), (SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [User] WHERE Email = 'test3@test.com'), (SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [User] WHERE Email = 'test4@test.com'), (SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [User] WHERE Email = 'test4@test.com'), (SELECT Id FROM [Group] WHERE Name = 'G2')),
		((SELECT Id FROM [User] WHERE Email = 'test5@test.com'), (SELECT Id FROM [Group] WHERE Name = 'G2'))

	INSERT INTO [GroupCategory](Category_Id,Group_Id) VALUES
		((SELECT Id FROM [Category] WHERE Name = 'Cat1'),(SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [Category] WHERE Name = 'Cat2'),(SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [Category] WHERE Name = 'Cat3'),(SELECT Id FROM [Group] WHERE Name = 'G1')),
		((SELECT Id FROM [Category] WHERE Name = 'Cat1'),(SELECT Id FROM [Group] WHERE Name = 'G2')),
		((SELECT Id FROM [Category] WHERE Name = 'Cat2'),(SELECT Id FROM [Group] WHERE Name = 'G2'))

	INSERT INTO [Expense](CreatedAt, Title, Value, FkCategoryId, FkGroupId, FkUserId) VALUES
		(
			'18-02-2019',
			'Expense1',
			10.35,
			(SELECT Id FROM [Category] WHERE Name = 'Cat1'),
			(SELECT Id FROM [Group] WHERE Name = 'G1'),
			(SELECT Id FROM [User] WHERE Email = 'test1@test.com')
		),
		(
			'19-02-2019',
			'Expense2',
			12.05,
			(SELECT Id FROM [Category] WHERE Name = 'Cat2'),
			(SELECT Id FROM [Group] WHERE Name = 'G1'),
			(SELECT Id FROM [User] WHERE Email = 'test1@test.com')
		),
		(
			'19-02-2019',
			'Expense3',
			102.36,
			(SELECT Id FROM [Category] WHERE Name = 'Cat1'),
			(SELECT Id FROM [Group] WHERE Name = 'G1'),
			(SELECT Id FROM [User] WHERE Email = 'test2@test.com')
		),
		(
			'21-03-2019',
			'Expense4',
			5.25,
			(SELECT Id FROM [Category] WHERE Name = 'Cat1'),
			(SELECT Id FROM [Group] WHERE Name = 'G2'),
			(SELECT Id FROM [User] WHERE Email = 'test1@test.com')
		),
			(
			'23-07-2020',
			'Expense5',
			65.42,
			(SELECT Id FROM [Category] WHERE Name = 'Cat3'),
			(SELECT Id FROM [Group] WHERE Name = 'G1'),
			(SELECT Id FROM [User] WHERE Email = 'test4@test.com')
		),
		(
			'18-02-2019',
			'Expense6',
			10,
			(SELECT Id FROM [Category] WHERE Name = 'Cat1'),
			(SELECT Id FROM [Group] WHERE Name = 'G2'),
			(SELECT Id FROM [User] WHERE Email = 'test5@test.com')
		),
			(
			'10-08-2021',
			'Expense7',
			5.25,
			(SELECT Id FROM [Category] WHERE Name = 'Cat1'),
			(SELECT Id FROM [Group] WHERE Name = 'G2'),
			(SELECT Id FROM [User] WHERE Email = 'test3@test.com')
		)
END

--SELECT * FROM [User];
--SELECT * FROM [Group];
--SELECT * FROM [Category];
--SELECT * FROM [GroupCategory];
--SELECT * FROM [userGroup];
--SELECT * FROM Expense;
