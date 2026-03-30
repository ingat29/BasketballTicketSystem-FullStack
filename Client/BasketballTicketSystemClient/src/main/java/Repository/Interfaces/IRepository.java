package Repository.Interfaces;

import java.util.List;

public interface IRepository<ID, T> {
    T add(T entity);      // C
    T findById(ID id);    // R
    List<T> findAll();    // R
    T update(T entity);   // U
    T delete(ID id);      // D
}