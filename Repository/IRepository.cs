using System.Collections.Generic;

public interface IRepository<ID, T> where T : class
{
    T Add(T entity);//C
    T FindById(ID id);//R
    List<T> FindAll();//R
    T Update(T entity);//U
    T Delete(ID id);//D
}