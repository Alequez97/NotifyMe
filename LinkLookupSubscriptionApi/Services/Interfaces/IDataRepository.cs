using LinkLookupSubscriptionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinkLookupSubscriptionApi.Services.Interfaces
{
    public interface IDataRepository<T> where T : ModelBase, new()
    {
        /// <summary>
        /// Gets object by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if object not found. Otherwise return object</returns>
        T Read(string id);

        /// <summary>
        /// Get all model records
        /// </summary>
        /// <returns>null if no records was found. Otherwise return list of objects</returns>
        List<T> ReadAll();

        /// <summary>
        /// Creates new record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if object was saved. Otherwise false</returns>
        bool Write(T obj);

        /// <summary>
        /// Updates existing record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if object was updated. Otherwise false</returns>
        bool Update(T obj);

        /// <summary>
        /// Deletes existing record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if object was deleted. Otherwise false</returns>
        bool Delete(string id);

        /// <summary>
        /// Find object by linq-type expressions
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Records that matches condition in expression</returns>
        T FindByExpression(Expression<Func<T, bool>> expression);
    }
}
