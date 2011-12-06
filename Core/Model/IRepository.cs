using System.Linq;

namespace WaveTech.Scutex.Model
{
	public interface IRepository<T> where T : class
	{
		void DeleteOnSubmit(T entity);
		IQueryable<T> GetAll();
		T GetById(object id);
		void SaveOrUpdate(T entity);
	}
}