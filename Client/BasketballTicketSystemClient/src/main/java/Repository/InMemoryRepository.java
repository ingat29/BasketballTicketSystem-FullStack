package Repository;

import Model.Entity;
import Repository.Interfaces.IRepository;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public abstract class InMemoryRepository<ID, T extends Entity<ID>> implements IRepository<ID, T> {
    protected Map<ID, T> entities = new HashMap<>();

    @Override
    public T add(T entity) {//C
        if (entity.getId() == null) {
            throw new IllegalArgumentException("Entity ID cannot be null");
        }
        if (entities.containsKey(entity.getId())) {
            return null; // Or throw an exception if entity already exists
        }
        entities.put(entity.getId(), entity);
        return entity;
    }

    @Override
    public T findById(ID id) {//R
        return entities.get(id);
    }

    @Override
    public List<T> findAll() {//R
        return new ArrayList<>(entities.values());
    }

    @Override
    public T update(T entity) {//U
        if (!entities.containsKey(entity.getId())) {
            return null; // Or throw an exception if entity doesn't exist
        }
        entities.put(entity.getId(), entity);
        return entity;
    }

    @Override
    public T delete(ID id) {//D
        return entities.remove(id);
    }
}