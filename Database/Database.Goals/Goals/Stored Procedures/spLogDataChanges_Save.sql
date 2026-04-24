

CREATE PROCEDURE [Goals].[spLogDataChanges_Save]
    @CompanyId int,
    @UsersIdLoggedIn int,
    @TableName NVARCHAR(100),
    @TableNameId int,
    @TableId int,
    @ColumnName NVARCHAR(100) = NULL,
    @OldValue NVARCHAR(MAX) = NULL,
    @NewValue NVARCHAR(MAX) = NULL,
    @isUpdate bit = 0,
    @isDelete bit = 0,
    @isInsert bit = 0
AS
BEGIN

    INSERT INTO [Goals].[LogDataChanges] (CompanyID, UsersIDLoggedIn, TableName, TableNameId, TableID, ChangeType, ColumnName, OldValue, NewValue)
    VALUES (@CompanyId
    , @UsersIdLoggedIn
    , @TableName
    , @TableNameId
    , @TableId
    , CASE WHEN @isInsert = 1 THEN 'INSERT' ELSE 
        CASE WHEN @isUpdate = 1 THEN 'UPDATE' ELSE
          CASE WHEN @isDelete = 1 THEN 'DELETE' END END END
    , @ColumnName
    , @OldValue
    , @NewValue);

END;
