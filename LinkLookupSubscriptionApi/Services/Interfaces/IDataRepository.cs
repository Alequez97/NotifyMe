using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinkLookupSubscriptionApi.Services.Interfaces
{
    public interface IDataRepository<T> where T : new()
    {
        T Read(string id);

        List<T> ReadAll();

        bool Write(T obj);

        bool Update(T obj);

        bool Delete(string id);

        T FindByExpression(Expression<Func<T, bool>> expression);
    }
}
