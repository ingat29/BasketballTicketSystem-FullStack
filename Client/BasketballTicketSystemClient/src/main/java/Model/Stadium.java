package Model;

public class Stadium extends Entity<Integer> {
    private String name;
    private int capacity;

    public Stadium(Integer id, String name, int capacity) {
        super(id);
        this.name = name;
        this.capacity = capacity;
    }

    public String getName() { return name; }
    public void setName(String name) { this.name = name; }

    public int getCapacity() { return capacity; }
    public void setCapacity(int capacity) { this.capacity = capacity; }
}
