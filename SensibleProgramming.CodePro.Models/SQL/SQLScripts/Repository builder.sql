declare @TableName sysname = 'ScoreValue'
declare @Result varchar(max) = 'using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Repostories
{	
	public interface I' + @TableName + 'Repository : BaseRepository
	{
		public ' + @TableName + 'Repository(){}

		public List<' + @TableName + '> GetAll(){}

		public ' + @TableName + ' GetById(int id){}

		public ' + @TableName + ' Create(' + @TableName + ' obj){}

		public ' + @TableName + ' Edit(' + @TableName + ' obj){}

		public ' + @TableName + ' Delete(int id){}
	}

	public class ' + @TableName + 'Repository : BaseRepository, I' + @TableName + '
	{
		public ' + @TableName + 'Repository(){}

		public List<' + @TableName + '> GetAll(){}

		public ' + @TableName + ' GetById(int id){}

		public ' + @TableName + ' Create(' + @TableName + ' obj){}

		public ' + @TableName + ' Edit(' + @TableName + ' obj){}

		public ' + @TableName + ' Delete(int id){}
		'
select @Result
set @Result = @Result  + '
	}
}'
print @Result