
DECLARE @char char
SET @char = '-'
DECLARE @str nvarchar(50)
SET @str = '1-223-32-322'
DECLARE @i integer
SET @i = 4

select reverse(PARSENAME(REPLACE(reverse(@str), @char, '.') ,@i))
