using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAllEntities();
        TEntity GetEntity(int id);
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(int id);
   
    }
}
