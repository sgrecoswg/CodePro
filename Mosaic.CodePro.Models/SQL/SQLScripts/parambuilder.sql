SELECT 
	SO.name AS [ObjectName],
	P.parameter_id AS [ParameterID],
	P.name AS [ParameterName],
	TYPE_NAME(P.user_type_id) AS [ParameterDataType],
	Concat(' param.Add("',P.name,'", ',
				case when P.Name like '@UserId' then 'CurrentUser' else Concat(' detail.',Replace(P.name,'@','')) end,
				', DbType.',
				case when TYPE_NAME(P.user_type_id) = 'int' then 'Int32' 
					 when TYPE_NAME(P.user_type_id) = 'nvarchar' then 'String'  
					 when TYPE_NAME(P.user_type_id) = 'varchar' then 'String' 
					 when TYPE_NAME(P.user_type_id) = 'bit' then 'Boolean' 
					 when TYPE_NAME(P.user_type_id) = 'float' then 'Decimal' 
					 when TYPE_NAME(P.user_type_id) = 'datetime' then 'DateTime'
				end
			,');')
FROM sys.objects AS SO
	INNER JOIN sys.parameters AS P     ON    SO.OBJECT_ID = P.OBJECT_ID --AND SO.name not in @sp
		WHERE SO.OBJECT_ID IN ( SELECT OBJECT_ID 
								FROM sys.objects
								WHERE TYPE IN ('P') AND 
									SO.name in ('~tablename~'))
